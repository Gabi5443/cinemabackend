namespace cinemabackend.Models;

public class Logradouro
{ 

   private int Id_logradouro{get; private set;}
   private string Bairro{get; private set;}
   private string Cep {get; private set;}
   private string Rua {get; private set;}
   private string Descricao {get; private set;}
   private string Numero_casa {get; private set;}
   private string Descricao {get; private set;}


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
