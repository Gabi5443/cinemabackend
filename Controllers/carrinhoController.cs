
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using cinemabackend.Models;
using cinemabackend.DTOs;

namespace cinemabackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarrinhoController : ControllerBase
{
    private readonly IConfiguration configuration;

    public CarrinhoController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    // Listar todos os Carrinhos
    [HttpGet]
    public ActionResult<List<Carrinho>> Get()
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;
        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "SELECT * FROM CARRINHO";
        using MySqlCommand command = new MySqlCommand(sql, connection);
        using MySqlDataReader reader = command.ExecuteReader();

        List<Carrinho> carrinhos = new List<Carrinho>();

        while (reader.Read())
        {
            int idCarrinho = reader.GetInt32("iD_CARRINHO");
            int idIngressoFk = reader.GetInt32("ID_INGRESSO_FK");
            string formaPagamento = reader.IsDBNull(reader.GetOrdinal("FORMA_PAGAMENTO")) ? string.Empty : reader.GetString("FORMA_PAGAMENTO");
            double desconto = reader.GetDouble("DESCONTO");
            string status = reader.GetString("STATUS");
            DateTime horaPagamento = reader.GetDateTime("HORA_PAGAMENTO");

            // Ajuste os parâmetros de acordo com o construtor da sua Model Carrinho
            Carrinho carrinho = new Carrinho(idCarrinho, idIngressoFk, formaPagamento, desconto, status, horaPagamento);
            carrinhos.Add(carrinho);
        }

        return Ok(carrinhos);
    }

    // Listar um único Carrinho por ID
    [HttpGet("{id}")]
    public ActionResult<Carrinho> GetById(int id)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;
        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "SELECT * FROM CARRINHO WHERE iD_CARRINHO = @id";
        using MySqlCommand command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        using MySqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            int idCarrinho = reader.GetInt32("iD_CARRINHO");
            int idIngressoFk = reader.GetInt32("ID_INGRESSO_FK");
            string formaPagamento = reader.IsDBNull(reader.GetOrdinal("FORMA_PAGAMENTO")) ? string.Empty : reader.GetString("FORMA_PAGAMENTO");
            double desconto = reader.GetDouble("DESCONTO");
            string status = reader.GetString("STATUS");
            DateTime horaPagamento = reader.GetDateTime("HORA_PAGAMENTO");

            return Ok(new Carrinho(idCarrinho, idIngressoFk, formaPagamento, desconto, status, horaPagamento));
        }

        return NotFound("Carrinho não encontrado.");
    }

    // Cadastrar / Criar um Carrinho
    [HttpPost]
    public IActionResult Post([FromBody] CarrinhoRequest request)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;
        using MySqlConnection connection = new MySqlConnection(connectionString);
        try
        {
            connection.Open();
            string sql = """
                INSERT INTO CARRINHO (ID_INGRESSO_FK, FORMA_PAGAMENTO, DESCONTO, STATUS)
                VALUES (@id_ingresso, @forma_pagamento, @desconto, @status);
                SELECT LAST_INSERT_ID();
                """;

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id_ingresso", request.IdIngressoFk);
            command.Parameters.AddWithValue("@forma_pagamento", request.FormaPagamento ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@desconto", request.Desconto);
            command.Parameters.AddWithValue("@status", string.IsNullOrEmpty(request.Status) ? "EM ANDAMENTO" : request.Status);

            int id = Convert.ToInt32(command.ExecuteScalar());

            return CreatedAtAction(nameof(GetById), new { id = id }, request);
        }
        catch (MySqlException ex)
        {
            if (ex.Number == 1452)
                return BadRequest("O Ingresso informado não existe no sistema.");
            return StatusCode(500, $"Erro no banco de dados: {ex.Message}");
        }
    }

    // Alterar um Carrinho
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] CarrinhoRequest request)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;
        using MySqlConnection connection = new MySqlConnection(connectionString);
        try
        {
            connection.Open();
            string sql = @"UPDATE CARRINHO 
                           SET ID_INGRESSO_FK = @id_ingresso, 
                               FORMA_PAGAMENTO = @forma_pagamento, 
                               DESCONTO = @desconto, 
                               STATUS = @status 
                           WHERE iD_CARRINHO = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@id_ingresso", request.IdIngressoFk);
            command.Parameters.AddWithValue("@forma_pagamento", request.FormaPagamento ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@desconto", request.Desconto);
            command.Parameters.AddWithValue("@status", request.Status);

            int linhasAfetadas = command.ExecuteNonQuery();
            if (linhasAfetadas == 0) return NotFound("Carrinho não encontrado.");

            return NoContent();
        }
        catch (MySqlException ex)
        {
            if (ex.Number == 1452)
                return BadRequest("O Ingresso informado não existe no sistema.");
            return StatusCode(500, $"Erro no banco de dados: {ex.Message}");
        }
    }

    // Deletar um Carrinho
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;
        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "DELETE FROM CARRINHO WHERE iD_CARRINHO = @id";
        using MySqlCommand command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        int linhasRemovidas = command.ExecuteNonQuery();
        if (linhasRemovidas == 0) return NotFound("Carrinho não encontrado.");

        return NoContent();
    }
}
