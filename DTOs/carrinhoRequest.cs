
namespace cinemabackend.DTOs;

public class CarrinhoRequest
{
    public int IdIngressoFk { get; set; }
    public string FormaPagamento { get; set; }
    public double Desconto { get; set; }
    public string Status { get; set; }
}
