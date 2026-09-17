SELECT @@lc_time_names;
SET @@lc_time_names = 'pt_BR';

DROP DATABASE IF EXISTS CINEMA;  
CREATE DATABASE IF NOT EXISTS CINEMA;
SHOW DATABASES;  
USE CINEMA;  


CREATE TABLE LOGRADOURO (
    ID_LOGRADOURO INT PRIMARY KEY AUTO_INCREMENT,
    RUA VARCHAR (100),
    BAIRRO VARCHAR (100),
    CIDADE VARCHAR (100),
    CEP VARCHAR (100),
    DESCRICAO VARCHAR (1000),
    NUMERO_CASA VARCHAR (100)
);



CREATE TABLE USUARIO(
    ID_USUARIO INT PRIMARY KEY AUTO_INCREMENT,
    CPF_USUARIO VARCHAR (20),
    NOME_USUARIO VARCHAR(100),
    EMAIL_USUARIO VARCHAR (100),
    ID_LOGRADOURO_FK INT ,
    FOREIGN KEY (ID_LOGRADOURO_FK) REFERENCES LOGRADOURO (ID_LOGRADOURO),
    SENHA_USUARIO VARCHAR (100)
);

 CREATE TABLE CATEGORIA (
        ID_CATEGORIA INT PRIMARY KEY AUTO_INCREMENT,
        NOME_CATEGORIA VARCHAR (100),
        DESCRICAO_CATEGORIA TEXT
);


CREATE TABLE FILME (
    TITULO_FILME VARCHAR (100),
    ID_FILME INT PRIMARY KEY AUTO_INCREMENT,
    DESCRICAO_FILME TEXT,
    SINOPSE_FILME TEXT,
    SUBTITULO_FILME VARCHAR (100),
    DURACAO_FILME TIME,
    ID_CATEGORIA_FK INT,
    FOREIGN KEY (ID_CATEGORIA_FK) REFERENCES CATEGORIA (ID_CATEGORIA)

);



CREATE TABLE INGRESSO (
     ID_INGRESSO INT PRIMARY KEY AUTO_INCREMENT,
     PRECO_INGRESSO DECIMAL(10,2),
     ID_FILME_FK INT,
     FOREIGN KEY (ID_FILME_FK) REFERENCES FILME (ID_FILME),
     ID_USUARIO_FK INT,
     FOREIGN KEY (ID_USUARIO_FK) REFERENCES USUARIO (ID_USUARIO)


);

CREATE TABLE CARRINHO (
    iD_CARRINHO INT PRIMARY KEY AUTO_INCREMENT,
    ID_INGRESSO_FK INT,
    FOREIGN KEY (ID_INGRESSO_FK) REFERENCES INGRESSO (ID_INGRESSO),
    FORMA_PAGAMENTO TEXT,
    DESCONTO DOUBLE,
    STATUS ENUM('EM ANDAMENTO', 'PAGAMENTO CONCLUÍDO', 'PAGAMENTO CANCELADO') DEFAULT 'EM ANDAMENTO',
    HORA_PAGAMENTO TIMESTAMP DEFAULT CURRENT_TIMESTAMP


);


INSERT INTO CATEGORIA (NOME_CATEGORIA, DESCRICAO_CATEGORIA) VALUES 
('AÇÃO', 'Foco em dinamismo, perseguições, lutas, explosões e ritmo acelerado. O herói geralmente enfrenta grandes perigos para vencer um antagonista ou superar uma crise iminente.'), 
('TERROR', 'Destinado a amedrontar, chocar ou causar repulsa no espectador, explorando o medo do desconhecido, da morte ou de forças sobrenaturais.'), 
('AVENTURA', 'Narrativas centradas em jornadas épicas, exploração de territórios desconhecidos e busca por objetivos grandiosos ou tesouros.'), 
('FANTASIA', 'Histórias ambientadas em universos imaginários que rompem as leis da física e da realidade conhecida.'), 
('DRAMA', 'Foco no desenvolvimento profundo de personagens, em conflitos emocionais intensos e em situações humanas realistas ou comoventes.'), 
('CRIMINAL', 'Histórias centradas na prática de delitos, investigações de crimes, julgamentos ou na vida de criminosos e detetives.');



