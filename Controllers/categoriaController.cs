using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using cinemabackend.Models;
using cinemabackend.DTOs;

namespace cinemabackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriaController : ControllerBase
{
    private readonly IConfiguration configuration;

    public CategoriaController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    // Listar todas as Categorias
    [HttpGet]
    public ActionResult<List<Categoria>> Get()
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "SELECT * FROM CATEGORIA";

        using MySqlCommand command = new MySqlCommand(sql, connection);
        using MySqlDataReader reader = command.ExecuteReader();

        List<Categoria> categorias = new List<Categoria>();

        while (reader.Read())
        {
            int idCategoria = reader.GetInt32("ID_CATEGORIA");
            string nomeCategoria = reader.GetString("NOME_CATEGORIA");
            string descricaoCategoria = reader.GetString("DESCRICAO_CATEGORIA");

            // Atenção à ordem dos parâmetros do construtor da sua model: (nome, id, descricao)
            Categoria categoria = new Categoria(nomeCategoria, idCategoria, descricaoCategoria);
            categorias.Add(categoria);
        }

        return Ok(categorias);
    }

    // Listar uma única Categoria por ID
    [HttpGet("{id}")]
    public ActionResult<Categoria> GetById(int id)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "SELECT * FROM CATEGORIA WHERE ID_CATEGORIA = @id";

        using MySqlCommand command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        using MySqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            int idCategoria = reader.GetInt32("ID_CATEGORIA");
            string nomeCategoria = reader.GetString("NOME_CATEGORIA");
            string descricaoCategoria = reader.GetString("DESCRICAO_CATEGORIA");

            return Ok(new Categoria(nomeCategoria, idCategoria, descricaoCategoria));
        }

        return NotFound("Categoria não encontrada.");
    }  

    // Cadastrar uma Categoria
    [HttpPost]
    public IActionResult Post([FromBody] CategoriaRequest request)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using MySqlConnection connection = new MySqlConnection(connectionString);
        try
        {
            connection.Open();

            string sql = """
                INSERT INTO CATEGORIA (NOME_CATEGORIA, DESCRICAO_CATEGORIA)
                VALUES (@nome, @descricao);
                SELECT LAST_INSERT_ID();
                """;

            using MySqlCommand command = new MySqlCommand(sql, connection);
            
            command.Parameters.AddWithValue("@nome", request.NomeCategoria);
            command.Parameters.AddWithValue("@descricao", request.DescricaoCategoria);

            int id = Convert.ToInt32(command.ExecuteScalar());

            return CreatedAtAction(
                nameof(GetById),
                new { id = id },
                request
            );
        }
        catch (MySqlException ex)
        {
            return StatusCode(500, $"Erro no banco de dados: {ex.Message}");
        }
    }

    // Alterar uma Categoria
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] CategoriaRequest request)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string sql = @"UPDATE CATEGORIA 
                               SET NOME_CATEGORIA = @nome, 
                                   DESCRICAO_CATEGORIA = @descricao 
                               WHERE ID_CATEGORIA = @id_categoria";
               
                MySqlCommand comando = new MySqlCommand(sql, connection);
               
                comando.Parameters.AddWithValue("@id_categoria", id);
                comando.Parameters.AddWithValue("@nome", request.NomeCategoria);
                comando.Parameters.AddWithValue("@descricao", request.DescricaoCategoria);

                int registrosAfetados = comando.ExecuteNonQuery();

                if (registrosAfetados == 0)
                {
                    return NotFound("Categoria não encontrada.");
                }

                return NoContent();
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, $"Erro no banco de dados: {ex.Message}");
            }
        }
    }

    // Deletar uma Categoria
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "DELETE FROM CATEGORIA WHERE ID_CATEGORIA = @id_categoria";
    
        MySqlCommand comando = new MySqlCommand(sql, connection);
        comando.Parameters.AddWithValue("@id_categoria", id);

        int registrosRemovidos = comando.ExecuteNonQuery();

        if (registrosRemovidos == 0)
        {
                    return NotFound("Categoria não encontrada.");
        }

        return NoContent();
    }
}
