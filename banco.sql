CREATE DATABASE cinema;

USE cinema;

CREATE TABLE usuarios(
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL
    ) ENGINE = InnoDB;

INSERT INTO usuarios(nome)
    VALUES ("alguem ai");

SELECT * FROM usuarios;