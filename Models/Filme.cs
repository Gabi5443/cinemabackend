namespace cinemabackend.Models;

public class Filme 
{ 

    public string Titulo { get; private set; } 
    public int Id { get; private set; } 
    public string Descricao { get; private set; } 
    public string Sinopse { get; private set; } 
    public TimeOnly Duracao { get; private set; } 
    public string Subtitulo { get; private set; } 

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
