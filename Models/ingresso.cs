namespace cinemabackend.Models;

public class Ingresso 
{ 
  public int Id_ingresso {get; private set;}
  public decimal Preco_ingresso {get; private set;}
  public Usuario Usuario_fk {get; private set;}


  public Ingresso (int id_ingresso, decimal preco_ingresso, Usuario usuario_fk)
  {
    Id_ingresso = id_ingresso;
    Preco_ingresso = preco_ingresso;
    Usuario_fk = usuario_fk;
    
  }
}
