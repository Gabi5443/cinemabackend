using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using cinemabackend.Models;
using cinemabackend.DTOs;

namespace cinemabackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IConfiguration configuration;

    public UsuarioController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    // Listar todos os Usuários
    [HttpGet]
    public List<Usuario> Get()
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "SELECT * FROM USUARIO";

        using MySqlCommand command = new MySqlCommand(sql, connection);
        using MySqlDataReader reader = command.ExecuteReader();

        List<Usuario> usuarios = new List<Usuario>();

        while (reader.Read())
        {
            int id = reader.GetInt32("ID_USUARIO");
            string name = reader.GetString("NOME_USUARIO");
            
            // CORREÇÃO: Extraindo os campos adicionais exigidos pelo construtor da Model
            string email = reader.GetString("EMAIL_USUARIO");
            string cpf = reader.GetString("CPF_USUARIO");
            string senha = reader.GetString("SENHA_USUARIO");
            
            // Instancia o logradouro temporário exigido na última posição do construtor
            Logradouro logradouroDummy = new Logradouro(0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            // Passando os 6 parâmetros obrigatórios na ordem correta:
            // (int id, string nome, string email, string cpf, string senha, Logradouro logradouro)
            Usuario usuario = new Usuario(id, name, email, cpf, senha, logradouroDummy);
            usuarios.Add(usuario);
        }

        return usuarios;
    }

    // Listar um único Usuário
    [HttpGet("{id}")]
    public Usuario? GetById(int id)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "SELECT * FROM USUARIO WHERE ID_USUARIO = @id";

        using MySqlCommand command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        using MySqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            int usuarioId = reader.GetInt32("ID_USUARIO");
            string name = reader.GetString("NOME_USUARIO");
            
            // CORREÇÃO: Extraindo os campos adicionais para o GetById
            string email = reader.GetString("EMAIL_USUARIO");
            string cpf = reader.GetString("CPF_USUARIO");
            string senha = reader.GetString("SENHA_USUARIO");
            
            Logradouro logradouroDummy = new Logradouro(0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            return new Usuario(usuarioId, name, email, cpf, senha, logradouroDummy);
        }

        return null;
    }  

    // Cadastrar um Usuário
    [HttpPost]
    public IActionResult Post(UsuarioRequest request)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = """
            INSERT INTO USUARIO (CPF_USUARIO, NOME_USUARIO, EMAIL_USUARIO, ID_LOGRADOURO_FK, SENHA_USUARIO)
            VALUES (@cpf, @nome, @email, @id_logradouro, @senha);
            SELECT LAST_INSERT_ID();
            """;

        using MySqlCommand command = new MySqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("@cpf", request.CpfUsuario);
        command.Parameters.AddWithValue("@nome", request.NomeUsuario);
        command.Parameters.AddWithValue("@email", request.EmailUsuario);
        command.Parameters.AddWithValue("@id_logradouro", request.IdLogradouroFk);
        command.Parameters.AddWithValue("@senha", request.SenhaUsuario);

        int id = Convert.ToInt32(command.ExecuteScalar());

        return CreatedAtAction(
            nameof(GetById),
            new { id = id },
            request
        );
    }

    // Alterar um Usuário
    [HttpPut("{ID_USUARIO}")]
    public IActionResult Update(int ID_USUARIO, [FromBody] UsuarioRequest usuario)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string sql = @"UPDATE USUARIO 
                               SET CPF_USUARIO = @cpf, 
                                   NOME_USUARIO = @nome, 
                                   EMAIL_USUARIO = @email, 
                                   ID_LOGRADOURO_FK = @id_logradouro, 
                                   SENHA_USUARIO = @senha 
                               WHERE ID_USUARIO = @id_usuario";
             
                MySqlCommand comando = new MySqlCommand(sql, connection);
             
                comando.Parameters.AddWithValue("@id_usuario", ID_USUARIO);
                comando.Parameters.AddWithValue("@cpf", usuario.CpfUsuario);
                comando.Parameters.AddWithValue("@nome", usuario.NomeUsuario);
                comando.Parameters.AddWithValue("@email", usuario.EmailUsuario);
                comando.Parameters.AddWithValue("@id_logradouro", usuario.IdLogradouroFk);
                comando.Parameters.AddWithValue("@senha", usuario.SenhaUsuario);

                int registrosAfetados = comando.ExecuteNonQuery();

                if (registrosAfetados == 0)
                {
                    return NotFound("Usuário não encontrado.");
                }

                return NoContent();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1452)
                {
                    return BadRequest("O Logradouro informado não existe no sistema. Verifique a chave estrangeira.");
                }
                
                return StatusCode(500, $"Erro no banco de dados: {ex.Message}");
            }
        }
    }

    // Deletar um Usuário
    [HttpDelete("{ID_USUARIO}")]
    public IActionResult Delete(int ID_USUARIO)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "DELETE FROM USUARIO WHERE ID_USUARIO = @id_usuario";
    
        MySqlCommand comando = new MySqlCommand(sql, connection);
        comando.Parameters.AddWithValue("@id_usuario", ID_USUARIO);

        int registrosRemovidos = comando.ExecuteNonQuery();

        if (registrosRemovidos == 0)
        {
            return NotFound("Usuário não encontrado.");
        }

        return NoContent();
    }
}
