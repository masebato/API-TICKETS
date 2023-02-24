CREATE DATABASE TICKETAPI

USE TICKETAPI

CREATE TABLE "USER"(
idUser int primary key identity(1,1),
name varchar(50),
email varchar(50)

)


CREATE TABLE TICKET(
idTicket int primary key identity(1,1),
description varchar(50),
dateCreate datetime,
dateUpdate datetime,
status varchar(10),
idUser int,
CONSTRAINT FK_IDUSER FOREIGN KEY (idUser) REFERENCES "USER"(idUser) 
)

insert into "USER"(email,name) values ('usuario2@google.com','pedro usuario')

insert into TICKET(description,dateCreate,status,idUser) values ('Ticket prueba','2023-02-24','ABIERTO', 1)

select * from TICKET