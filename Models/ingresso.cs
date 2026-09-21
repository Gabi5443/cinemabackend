namespace cinemabackend.Models;

public class Ingresso 
{ 
  public int Id_ingresso {get; private set;}
  public decimal Preco_ingresso {get; private set;}
  public Usuario Usuario {get; private set;}
  public Filme filme {get; private set;}


  public Ingresso (int id_ingresso, decimal preco_ingresso, Usuario usuario, Filme filme)
  {
    Id_ingresso = id_ingresso;
    Preco_ingresso = preco_ingresso;
    Usuario = usuario;
    Filme = filme;
    
  }
}
