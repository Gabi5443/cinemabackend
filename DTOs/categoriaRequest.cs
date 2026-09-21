namespace cinemabackend.DTOs;
public record CategoriaRequest 
{ 
  public string NomeCategoria { get; set; } 
  public string DescricaoCategoria { get; set; } 
}