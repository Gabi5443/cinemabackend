namespace cinemabackend.DTOs;

public record IngressoRequest
{
    public decimal PrecoIngresso { get; set; }
    public int IdFilmeFk { get; set; }
    public int IdUsuarioFk { get; set; }
}
