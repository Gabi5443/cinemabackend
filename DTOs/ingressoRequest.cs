namespace cinemabackend.DTOs;

public class IngressoRequest
{
    public decimal PrecoIngresso { get; set; }
    public int IdFilmeFk { get; set; }
    public int IdUsuarioFk { get; set; }
}
