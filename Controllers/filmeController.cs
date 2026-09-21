using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using cinemabackend.Models;
using cinemabackend.DTOs;

namespace cinemabackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilmeController : ControllerBase
{
    private readonly IConfiguration configuration;

    public FilmeController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    // Listar todos os filmes
    [HttpGet]
    public List<Filme> Get()
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "SELECT * FROM FILME";

        using MySqlCommand command = new MySqlCommand(sql, connection);
        using MySqlDataReader reader = command.ExecuteReader();

        List<Filme> filmes = new List<Filme>();

        while (reader.Read())
        {
            int id = reader.GetInt32("ID_FILME");
            string titulo = reader.GetString("TITULO_FILME");
            // Nota: Se a sua model Filme precisar de mais campos no construtor, 
            // você pode ajustá-los aqui conforme a sua Model.

            Filme filme = new Filme(id, titulo);
            filmes.Add(filme);
        }

        return filmes;
    }

    // Listar um único Filme
    [HttpGet("{id}")]
    public Filme? GetById(int id)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "SELECT * FROM FILME WHERE ID_FILME = @id";

        using MySqlCommand command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        using MySqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            int filmeId = reader.GetInt32("ID_FILME");
            string titulo = reader.GetString("TITULO_FILME");

            return new Filme(filmeId, titulo);
        }

        return null;
    }  

    // Cadastrar um Filme
    [HttpPost]
    public IActionResult Post(FilmeRequest request)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = """
            INSERT INTO FILME (TITULO_FILME, DESCRICAO_FILME, SINOPSE_FILME, SUBTITULO_FILME, DURACAO_FILME, ID_CATEGORIA_FK)
            VALUES (@titulo, @descricao, @sinopse, @subtitulo, @duracao, @id_categoria);
            SELECT LAST_INSERT_ID();
            """;

        using MySqlCommand command = new MySqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("@titulo", request.TituloFilme);
        command.Parameters.AddWithValue("@descricao", request.DescricaoFilme);
        command.Parameters.AddWithValue("@sinopse", request.SinopseFilme);
        command.Parameters.AddWithValue("@subtitulo", request.SubtituloFilme);
        command.Parameters.AddWithValue("@duracao", request.DuracaoFilme);
        command.Parameters.AddWithValue("@id_categoria", request.IdCategoriaFk);

        int id = Convert.ToInt32(command.ExecuteScalar());

        return CreatedAtAction(
            nameof(GetById),
            new { id = id },
            request
        );
    }

    // Alterar um Filme
    [HttpPut("{ID_FILME}")]
    public IActionResult Update(int ID_FILME, [FromBody] FilmeRequest filme)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string sql = @"UPDATE FILME 
                               SET TITULO_FILME = @titulo, 
                                   DESCRICAO_FILME = @descricao, 
                                   SINOPSE_FILME = @sinopse, 
                                   SUBTITULO_FILME = @subtitulo, 
                                   DURACAO_FILME = @duracao,
                                   ID_CATEGORIA_FK = @id_categoria 
                               WHERE ID_FILME = @id_filme";
             
                MySqlCommand comando = new MySqlCommand(sql, connection);
             
                comando.Parameters.AddWithValue("@id_filme", ID_FILME);
                comando.Parameters.AddWithValue("@titulo", filme.TituloFilme);
                comando.Parameters.AddWithValue("@descricao", filme.DescricaoFilme);
                comando.Parameters.AddWithValue("@sinopse", filme.SinopseFilme);
                comando.Parameters.AddWithValue("@subtitulo", filme.SubtituloFilme);
                comando.Parameters.AddWithValue("@duracao", filme.DuracaoFilme);
                comando.Parameters.AddWithValue("@id_categoria", filme.IdCategoriaFk);

                int registrosAfetados = comando.ExecuteNonQuery();

                if (registrosAfetados == 0)
                {
                    return NotFound("Filme não encontrado.");
                }

                return NoContent();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1452)
                {
                    return BadRequest("A Categoria informada não existe no sistema. Verifique a chave estrangeira.");
                }
                
                return StatusCode(500, $"Erro no banco de dados: {ex.Message}");
            }
        }
    }

    // Deletar um Filme
    [HttpDelete("{ID_FILME}")]
    public IActionResult Delete(int ID_FILME)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "DELETE FROM FILME WHERE ID_FILME = @id_filme";
    
        MySqlCommand comando = new MySqlCommand(sql, connection);
        comando.Parameters.AddWithValue("@id_filme", ID_FILME);

        int registrosRemovidos = comando.ExecuteNonQuery();

        if (registrosRemovidos == 0)
        {
            return NotFound("Filme não encontrado.");
        }

        return NoContent();
    }
}
