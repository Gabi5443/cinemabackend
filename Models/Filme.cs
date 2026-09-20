namespace cinemabackend.Models;

public class Filme 
{ 

    private string Titulo { get; private set; } 
    private int Id { get; private set; } 
    private string Descricao { get; private set; } 
    private string Sinopse { get; private set; } 
    private TimeOnly Duracao { get; private set; } 
    private string Subtitulo { get; private set; } 

    public Filme(string titulo, int id, string descricao, string sinopse, TimeOnly duracao, string subtitulo) 
    { 
        Titulo = titulo; 
        Id = id; 
        Descricao = descricao; 
        Sinopse = sinopse; 
        Duracao = duracao; 
        Subtitulo = subtitulo; 
    } 
}
