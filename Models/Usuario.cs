namespace cinemabackend.Models;

public class Usuario {
        public int Id {get; private set;}
        public string Name {get; private set;}

        public Usuario (int id, string name){
            Id = id;
            Name = name;
        }
}