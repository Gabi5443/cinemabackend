using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using cinemabackend.Models;
using cinemabackend.DTOs;

namespace cinemabackend.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UsuarioController : ControllerBase{

    private readonly IConfiguration configuration;

    public UsuarioController(IConfiguration configuration){
        this.configuration = configuration;
    }

    //Listar todos os Produtos
    [HttpGet]
    public List<Usuario> Get(){
        string connectionString = 
            configuration.GetConnectionString("DefaultConnection")!;

        MySqlConnection connection = 
            new MySqlConnection(connectionString);

        connection.Open();

        string sql = "select * from usuarios";

        MySqlCommand command = 
            new MySqlCommand(sql, connection);

        MySqlDataReader reader = 
            command.ExecuteReader();

        List<Usuario> usuarios = 
            new List<Usuario>();

        while (reader.Read())
        {
            int id = reader.GetInt32("id");
            string name = reader.GetString("name");

            Usuario usuario = 
                new Usuario(id, name);

            usuarios.Add(usuario);
        }

        reader.Close();
        command.Dispose();
        connection.Close();

        return usuarios;
    }

    //Listar um único Produto
    [HttpGet("{id}")]
    public Usuario? GetById(int id){
        string connectionString = 
            configuration.GetConnectionString("DefaultConnection")!;

        MySqlConnection connection = 
            new MySqlConnection(connectionString);

        connection.Open();

        string sql = "SELECT * FROM usuarios WHERE id = @id";

        MySqlCommand command = 
            new MySqlCommand(sql, connection);

        command.Parameters.AddWithValue("@id", id);

        MySqlDataReader reader = 
            command.ExecuteReader();

        if(reader.Read())
        {
            int usuarioId = reader.GetInt32("id");
            string name = reader.GetString("name");

            Usuario usuario = new Usuario(usuarioId, name);

            return usuario;
        }

        reader.Close();
        command.Dispose();
        connection.Close();

        return null;
    } 

    //Cadastrar um Produto
    [HttpPost]
    public IActionResult Post(UsuarioRequest request){
        string connectionString = 
            configuration.GetConnectionString("DefaultConnection")!;

        MySqlConnection connection = 
            new MySqlConnection(connectionString);

        connection.Open();

        string sql = """
            INSERT INTO usuarios (name)
            VALUES (@NAME);
            SELECT LAST_INSERT_ID();
            """;
        using MySqlCommand command = 
            new MySqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("@name", request.Name);

        int id = Convert.ToInt32(command.ExecuteScalar());

        Usuario usuario = new Usuario(id, request.Name);

        return CreatedAtAction(
            nameof(GetById),
            new {
                id = usuario.Id
            },
            usuario
        );

    }

    //Alterar um Produto

    //Deletar um Produto
}
