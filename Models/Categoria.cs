namespace cinemabackend.Models;

public class Categoria 
{ 
  private string Nome_categoria {get; private set;}
  private int Id_categoria {get; private set;}
  private string Descricao_categoria {get; private set;}

  public Categoria (string nome_categoria , int id_categoria, string descricao_categoria)
  {
    Nome_categoria = nome_categoria;
    Id_categoria = id_categoria;
    Descricao_categoria = Descricao_categoria;
    
    
  }
}
