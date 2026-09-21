namespace cinemabackend.Models;

public class Usuario {
        public int Id {get; private set;}
        public string Name {get; private set;}
        public string Email {get; private set;}
        public string Cpf {get; private set;}
        public string Senha {get; private set;}
        public Logradouro Logradouro {get; private set;}
       

        public Usuario (int id, string name, string email, string cpf, string senha, Logradouro logradouro){
            Id = id;
            Name = name;
            Email = email;
            Cpf = cpf;
            Senha = senha;
            Logradouro = logradouro;
        }
}
