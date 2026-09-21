using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using cinemabackend.Models;
using cinemabackend.DTOs;

namespace cinemabackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngressoController : ControllerBase
{
    private readonly IConfiguration configuration;

    public IngressoController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    // Listar todos os Ingressos
    [HttpGet]
    public ActionResult<List<Ingresso>> Get()
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "SELECT * FROM INGRESSO";

        using MySqlCommand command = new MySqlCommand(sql, connection);
        using MySqlDataReader reader = command.ExecuteReader();

        List<Ingresso> ingressos = new List<Ingresso>();

        while (reader.Read())
        {
            int idIngresso = reader.GetInt32("ID_INGRESSO");
            decimal precoIngresso = reader.GetDecimal("PRECO_INGRESSO");
            int idFilmeFk = reader.GetInt32("ID_FILME_FK");
            int idUsuarioFk = reader.GetInt32("ID_USUARIO_FK");

            Logradouro logradouroDummy = new Logradouro(0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            Usuario usuarioDummy = new Usuario(idUsuarioFk, string.Empty, string.Empty, string.Empty, string.Empty, logradouroDummy);
            Filme filmeDummy = new Filme(string.Empty, idFilmeFk, string.Empty, string.Empty, default, string.Empty, null!);

            Ingresso ingresso = new Ingresso(idIngresso, precoIngresso, usuarioDummy, filmeDummy);
            ingressos.Add(ingresso);
        }

        return Ok(ingressos);
    }

    // Listar um único Ingresso por ID
    [HttpGet("{id}")]
    public ActionResult<Ingresso> GetById(int id)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "SELECT * FROM INGRESSO WHERE ID_INGRESSO = @id";

        using MySqlCommand command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        using MySqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            int idIngresso = reader.GetInt32("ID_INGRESSO");
            decimal precoIngresso = reader.GetDecimal("PRECO_INGRESSO");
            int idFilmeFk = reader.GetInt32("ID_FILME_FK");
            int idUsuarioFk = reader.GetInt32("ID_USUARIO_FK");

            Logradouro logradouroDummy = new Logradouro(0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            Usuario usuarioDummy = new Usuario(idUsuarioFk, string.Empty, string.Empty, string.Empty, string.Empty, logradouroDummy);
            Filme filmeDummy = new Filme(string.Empty, idFilmeFk, string.Empty, string.Empty, default, string.Empty, null!);

            // CORRIGIDO: alterado de Angresso para Ingresso
            return Ok(new Ingresso(idIngresso, precoIngresso, usuarioDummy, filmeDummy));
        }

        return NotFound("Ingresso não encontrado.");
    }  

    // Cadastrar um Ingresso
    [HttpPost]
    // CORRIGIDO: alterado de RogressoRequest para IngressoRequest
    public IActionResult Post([FromBody] IngressoRequest request)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using MySqlConnection connection = new MySqlConnection(connectionString);
        try
        {
            connection.Open();

            string sql = """
                INSERT INTO INGRESSO (PRECO_INGRESSO, ID_FILME_FK, ID_USUARIO_FK)
                VALUES (@preco, @id_filme, @id_usuario);
                SELECT LAST_INSERT_ID();
                """;

            using MySqlCommand command = new MySqlCommand(sql, connection);
            
            command.Parameters.AddWithValue("@preco", request.PrecoIngresso);
            command.Parameters.AddWithValue("@id_filme", request.IdFilmeFk);
            command.Parameters.AddWithValue("@id_usuario", request.IdUsuarioFk);

            int id = Convert.ToInt32(command.ExecuteScalar());

            return CreatedAtAction(
                nameof(GetById),
                new { id = id },
                request
            );
        }
        catch (MySqlException ex)
        {
            if (ex.Number == 1452)
            {
                return BadRequest("O Filme ou o Usuário informado não existem no sistema. Verifique as chaves estrangeiras.");
            }
            
            return StatusCode(500, $"Erro no banco de dados: {ex.Message}");
        }
    }

    // Alterar um Ingresso
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] IngressoRequest request)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string sql = @"UPDATE INGRESSO 
                               SET PRECO_INGRESSO = @preco, 
                                   ID_FILME_FK = @id_filme, 
                                   ID_USUARIO_FK = @id_usuario 
                               WHERE ID_INGRESSO = @id_ingresso";
               
                MySqlCommand comando = new MySqlCommand(sql, connection);
               
                comando.Parameters.AddWithValue("@id_ingresso", id);
                comando.Parameters.AddWithValue("@preco", request.PrecoIngresso);
                comando.Parameters.AddWithValue("@id_filme", request.IdFilmeFk);
                comando.Parameters.AddWithValue("@id_usuario", request.IdUsuarioFk);

                int registrosAfetados = comando.ExecuteNonQuery();

                if (registrosAfetados == 0)
                {
                    return NotFound("Ingresso não encontrado.");
                }

                return NoContent();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1452)
                {
                    return BadRequest("O Filme ou o Usuário informado não existem no sistema. Verifique as chaves estrangeiras.");
                }
                
                return StatusCode(500, $"Erro no banco de dados: {ex.Message}");
            }
        }
    }

    // Deletar um Ingresso
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "DELETE FROM INGRESSO WHERE ID_INGRESSO = @id_ingresso";
    
        MySqlCommand comando = new MySqlCommand(sql, connection);
        comando.Parameters.AddWithValue("@id_ingresso", id);

        int registrosRemovidos = comando.ExecuteNonQuery();

        if (registrosRemovidos == 0)
        {
            return NotFound("Ingresso não encontrado.");
        }

        return NoContent();
    }
}
