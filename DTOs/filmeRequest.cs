namespace cinemabackend.DTOs;

public record FilmeRequest
{
    public string TituloFilme { get; set; }
    public string DescricaoFilme { get; set; }
    public string SinopseFilme { get; set; }
    public string SubtituloFilme { get; set; }
    public TimeSpan DuracaoFilme { get; set; }
    public int IdCategoriaFk { get; set; }
}
