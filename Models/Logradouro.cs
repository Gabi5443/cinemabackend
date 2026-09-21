namespace cinemabackend.Models;

public class Logradouro
{ 

   public int Id_logradouro{get; private set;}
   public string Bairro{get; private set;}
   public string Cep {get; private set;}
   public string Rua {get; private set;}
   public string Descricao {get; private set;}
   public string Numero_casa {get; private set;}
   public string Descricao {get; private set;}


public Logradouro (int id_logradouro, string bairro, string cep, string rua , string descricao , string numero_casa , string descricao)
{
  Id_logradouro = id_logradouro;
  Bairro = bairro;
  Cep = cep;
  Rua = rua;
  Descricao = descricao;
  Numero_casa = numero_casa;
  Descricao = descricao;
  
}


  
}
