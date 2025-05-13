-- Crear base de datos
CREATE DATABASE GymappDB;
GO

USE GymappDB;
GO

-- Tabla de Usuarios
CREATE TABLE Usuarios (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    EstaActivo BIT DEFAULT 1,
    FotoPerfilURL NVARCHAR(255) NULL,
    Peso FLOAT NULL,
    Altura FLOAT NULL,
    Genero VARCHAR(20) NULL,
    Edad INT NULL,
    ResetPasswordToken VARCHAR(255) NULL,
    ResetPasswordExpires DATETIME NULL
);
GO

-- Tabla de Entrenamientos
CREATE TABLE Entrenamientos (
    EntrenamientoID INT IDENTITY(1,1) PRIMARY KEY,
    Titulo VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(500),
    DuracionMinutos INT NOT NULL,
    Dificultad VARCHAR(20) CHECK (Dificultad IN ('Fácil', 'Media', 'Difícil')),
    ImagenURL VARCHAR(255),
    FechaCreacion DATETIME DEFAULT GETDATE(),
    Publico BIT DEFAULT 1,
    AutorID INT NULL REFERENCES Usuarios(UsuarioID) ON DELETE SET NULL
);
GO

-- Tabla de Ejercicios
CREATE TABLE Ejercicios (
    EjercicioID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(200),
    GrupoMuscular VARCHAR(50),
    ImagenURL VARCHAR(255),
    VideoURL VARCHAR(255),
    EquipamientoNecesario BIT DEFAULT 0
);
GO

-- Tabla de Relación Entrenamiento - Ejercicios
CREATE TABLE EntrenamientoEjercicios (
    EntrenamientoID INT REFERENCES Entrenamientos(EntrenamientoID) ON DELETE CASCADE,
    EjercicioID INT REFERENCES Ejercicios(EjercicioID) ON DELETE CASCADE,
    Series INT NOT NULL,
    Repeticiones INT NOT NULL,
    DescansoSegundos INT NOT NULL,
    Notas VARCHAR(100),
    PRIMARY KEY (EntrenamientoID, EjercicioID)
);
GO

-- Tabla de Favoritos (Usuarios guardan entrenamientos)
CREATE TABLE EntrenamientosFavoritos (
    UsuarioID INT REFERENCES Usuarios(UsuarioID) ON DELETE CASCADE,
    EntrenamientoID INT REFERENCES Entrenamientos(EntrenamientoID) ON DELETE CASCADE,
    FechaAgregado DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (UsuarioID, EntrenamientoID)
);
GO

-- Tabla de Comentarios en Entrenamientos
CREATE TABLE Comentarios (
    ComentarioID INT IDENTITY(1,1) PRIMARY KEY,
    EntrenamientoID INT REFERENCES Entrenamientos(EntrenamientoID) ON DELETE CASCADE,
    UsuarioID INT NULL REFERENCES Usuarios(UsuarioID) ON DELETE SET NULL,
    Contenido VARCHAR(500) NOT NULL,
    Calificacion INT CHECK (Calificacion BETWEEN 1 AND 5),
    FechaComentario DATETIME DEFAULT GETDATE()
);
GO


-- 1. Agregar campos de objetivos a la tabla Usuarios existente
ALTER TABLE Usuarios
ADD ObjetivoPeso FLOAT NULL,
    ObjetivoIMC FLOAT NULL,
    ObjetivoPorcentajeGrasa FLOAT NULL;

-- 2. Crear tabla para almacenar mediciones corporales
CREATE TABLE Mediciones (
    MedicionID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT NOT NULL REFERENCES Usuarios(UsuarioID) ON DELETE CASCADE,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Peso FLOAT NULL,
    Altura FLOAT NULL,
    IMC FLOAT NULL,
    PorcentajeGrasa FLOAT NULL,
    CircunferenciaBrazo FLOAT NULL,
    CircunferenciaPecho FLOAT NULL,
    CircunferenciaCintura FLOAT NULL,
    CircunferenciaMuslo FLOAT NULL,
    Notas VARCHAR(500) NULL
);

-- 3. Agregar índice para búsqueda rápida por usuario
CREATE INDEX IDX_Mediciones_Usuario ON Mediciones(UsuarioID);

-- 4. Crear vista para obtener datos resumidos por mes (para gráficas)
CREATE VIEW MedicionesMensuales AS
SELECT 
    UsuarioID,
    YEAR(Fecha) AS Anio,
    MONTH(Fecha) AS Mes,
    AVG(Peso) AS PesoPromedio,
    AVG(IMC) AS IMCPromedio,
    AVG(PorcentajeGrasa) AS GrasaPromedio,
    AVG(CircunferenciaCintura) AS CinturaPromedio
FROM Mediciones
GROUP BY UsuarioID, YEAR(Fecha), MONTH(Fecha);


-- Índices para mejorar el rendimiento
CREATE INDEX IDX_Entrenamientos_Autor ON Entrenamientos(AutorID);
CREATE INDEX IDX_Comentarios_Entrenamiento ON Comentarios(EntrenamientoID);
CREATE INDEX IDX_EntrenamientoEjercicios_Entrenamiento ON EntrenamientoEjercicios(EntrenamientoID);
GO