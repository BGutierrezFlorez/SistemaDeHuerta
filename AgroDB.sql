CREATE DATABASE AgroDB;
GO 

USE AgroDB;

/* ======Creacion de tabla usuario 
========
*/

CREATE TABLE Usuario(
	idUsuario INT IDENTITY(1,1) PRIMARY KEY,
	nombre VARCHAR(100) NOT NULL,
	tipoUsuario VARCHAR(50) NOT NULL,
	cedula VARCHAR(20) UNIQUE NOT NULL,
	correo VARCHAR(100) UNIQUE NOT NULL,
	contraseña VARCHAR(200) NOT NULL


);
GO

CREATE TABLE Cultivo(
	idCultivo INT IDENTITY (1,1) PRIMARY KEY,
	tipoCultivo VARCHAR(100) NOT NULL,
	fechaDeSiembra DATE NOT NULL,
	riego VARCHAR(50),
	idUsuario INT NOT NULL,
	FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario)


);
GO

/* ===========================
   TABLA: Comentario
   =========================== */
CREATE TABLE Comentario (
    idComentario INT IDENTITY(1,1) PRIMARY KEY,
    texto VARCHAR(500) NOT NULL,
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    idUsuario INT NOT NULL,   -- quien escribe el comentario
    idCultivo INT NOT NULL,   -- cultivo comentado
    FOREIGN KEY (idUsuario) REFERENCES Usuario(idUsuario),
    FOREIGN KEY (idCultivo) REFERENCES Cultivo(idCultivo)
);
GO