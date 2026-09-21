namespace cinemabackend.DTOs;

public record UsuarioRequest
{
    public string CpfUsuario { get; set; }
    public string NomeUsuario { get; set; }
    public string EmailUsuario { get; set; }
    public int IdLogradouroFk { get; set; }
    public string SenhaUsuario { get; set; }
};
