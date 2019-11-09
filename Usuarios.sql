create database Usuarios;
use Usuarios;

create table usuarios (
	Codigo int not null auto_increment,
    Nome varchar(50),
    Login varchar(35),
    primary key (Codigo)
    );
    
    insert into usuarios values (1, "Samuel Silva", "ssbsoftware29");
    insert into usuarios values (2, "Jairo Doni", "jairos");
    insert into usuarios values (3, "Cristiane Miamura", "cris");
    insert into usuarios values (4, "Samuel Silva", "monwalker");
    insert into usuarios values (NULL,"Samuel Silva", "astaton");
    
    Delete from usuarios where Codigo = 2;
    
    insert into usuarios (Codigo, Nome, Login)
    values (null, "Samuel Braga", "anacry29");
    
    SELECT Codigo, Nome, Login FROM usuarios WHERE Codigo = 2;
    
    select * from usuarios;