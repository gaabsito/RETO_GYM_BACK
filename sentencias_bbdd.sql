-- Crear base de datos
CREATE DATABASE RecetasDB
GO

USE RecetasDB
GO

-- Tabla de Usuarios
CREATE TABLE Usuarios (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    EstaActivo BIT DEFAULT 1
)
GO

-- Tabla de Categorías
CREATE TABLE Categorias (
    CategoriaID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(200)
)
GO

-- Tabla de Unidades de Medida
CREATE TABLE UnidadesMedida (
    UnidadID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(20) NOT NULL,
    Abreviatura VARCHAR(5) NOT NULL
)
GO

-- Tabla de Ingredientes
CREATE TABLE Ingredientes (
    IngredienteID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(200),
    UnidadPredeterminada INT REFERENCES UnidadesMedida(UnidadID)
)
GO

-- Tabla de Recetas
CREATE TABLE Recetas (
    RecetaID INT IDENTITY(1,1) PRIMARY KEY,
    Titulo VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(500),
    TiempoPreparacion INT, -- en minutos
    Porciones INT,
    Dificultad VARCHAR(20) CHECK (Dificultad IN ('Fácil', 'Media', 'Difícil')),
    ImagenURL VARCHAR(255),
    FechaCreacion DATETIME DEFAULT GETDATE(),
    AutorID INT REFERENCES Usuarios(UsuarioID),
    CategoriaID INT REFERENCES Categorias(CategoriaID)
)
GO

-- Tabla de Ingredientes por Receta
CREATE TABLE RecetaIngredientes (
    RecetaID INT REFERENCES Recetas(RecetaID),
    IngredienteID INT REFERENCES Ingredientes(IngredienteID),
    Cantidad DECIMAL(10,2) NOT NULL,
    UnidadID INT REFERENCES UnidadesMedida(UnidadID),
    Notas VARCHAR(100),
    PRIMARY KEY (RecetaID, IngredienteID)
)
GO

-- Tabla de Pasos de Preparación
CREATE TABLE PasosPreparacion (
    PasoID INT IDENTITY(1,1) PRIMARY KEY,
    RecetaID INT REFERENCES Recetas(RecetaID),
    NumeroPaso INT NOT NULL,
    Descripcion VARCHAR(500) NOT NULL,
    ImagenURL VARCHAR(255),
    UNIQUE (RecetaID, NumeroPaso)
)
GO

-- Tabla de Favoritos
CREATE TABLE RecetasFavoritas (
    UsuarioID INT REFERENCES Usuarios(UsuarioID),
    RecetaID INT REFERENCES Recetas(RecetaID),
    FechaAgregado DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (UsuarioID, RecetaID)
)
GO

-- Tabla de Comentarios
CREATE TABLE Comentarios (
    ComentarioID INT IDENTITY(1,1) PRIMARY KEY,
    RecetaID INT REFERENCES Recetas(RecetaID),
    UsuarioID INT REFERENCES Usuarios(UsuarioID),
    Contenido VARCHAR(500) NOT NULL,
    Calificacion INT CHECK (Calificacion BETWEEN 1 AND 5),
    FechaComentario DATETIME DEFAULT GETDATE()
)
GO

-- Tabla de Etiquetas
CREATE TABLE Etiquetas (
    EtiquetaID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(30) NOT NULL UNIQUE
)
GO

-- Tabla de Relación Recetas-Etiquetas
CREATE TABLE RecetaEtiquetas (
    RecetaID INT REFERENCES Recetas(RecetaID),
    EtiquetaID INT REFERENCES Etiquetas(EtiquetaID),
    PRIMARY KEY (RecetaID, EtiquetaID)
)
GO

-- Índices para mejorar el rendimiento
CREATE INDEX IDX_Recetas_Categoria ON Recetas(CategoriaID)
CREATE INDEX IDX_Recetas_Autor ON Recetas(AutorID)
CREATE INDEX IDX_Comentarios_Receta ON Comentarios(RecetaID)
CREATE INDEX IDX_RecetaIngredientes_Receta ON RecetaIngredientes(RecetaID)
GO