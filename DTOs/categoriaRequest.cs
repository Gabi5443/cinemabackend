namespace cinemabackend.DTOs;
public record Categoria 
{ 
  public string Nome_categoria {get; private set;}
  public int Id_categoria {get; private set;}
  public string Descricao_categoria {get; private set;}
}
