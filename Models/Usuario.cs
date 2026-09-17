namespace cinemabackend.Models;

public class Usuario {
        private int Id {get; private set;}
        private string Name {get; private set;}
        private string email {get; private set;}
       

        public Usuario (int id, string name){
            Id = id;
            Name = name;
            Email = email;
        }
}
