namespace cinemabackend.Models;

public class Usuario {
        private int Id {get; private set;}
        private string Name {get; private set;}
        private string Email {get; private set;}
        private string Cpf {get; private set;}
        private string Senha {get; private set;}
       

        public Usuario (int id, string name){
            Id = id;
            Name = name;
            Email = email;
            Cpf = cpf;
            Senha = senha;
        }
}
