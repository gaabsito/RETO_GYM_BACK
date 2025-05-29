-- Crear base de datos
CREATE DATABASE GymappDB;
GO

USE GymappDB;
GO

-- Tabla de Usuarios (CON CAMPO ADMIN AGREGADO)
CREATE TABLE Usuarios (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    EstaActivo BIT DEFAULT 1,
    EsAdmin BIT DEFAULT 0, -- NUEVO CAMPO PARA ADMINISTRADORES
    FotoPerfilURL NVARCHAR(255) NULL,
    Peso FLOAT NULL,
    Altura FLOAT NULL,
    Genero VARCHAR(20) NULL,
    Edad INT NULL,
    ResetPasswordToken VARCHAR(255) NULL,
    ResetPasswordExpires DATETIME NULL,
    ObjetivoPeso FLOAT NULL,
    ObjetivoIMC FLOAT NULL,
    ObjetivoPorcentajeGrasa FLOAT NULL
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

-- Tabla para almacenar mediciones corporales
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
GO

-- Tabla para almacenar las rutinas completadas por los usuarios
CREATE TABLE RutinasCompletadas (
    RutinaCompletadaID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT NOT NULL REFERENCES Usuarios(UsuarioID) ON DELETE CASCADE,
    EntrenamientoID INT NOT NULL REFERENCES Entrenamientos(EntrenamientoID) ON DELETE CASCADE,
    FechaCompletada DATETIME NOT NULL DEFAULT GETDATE(),
    Notas VARCHAR(500) NULL,
    DuracionMinutos INT NULL,
    CaloriasEstimadas INT NULL,
    NivelEsfuerzoPercibido INT NULL
);
GO

-- Tabla de Logros
CREATE TABLE Logros (
    LogroID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255) NOT NULL,
    Icono NVARCHAR(50) NOT NULL,
    Color NVARCHAR(20) NOT NULL,
    Experiencia INT NOT NULL DEFAULT 10,
    Categoria NVARCHAR(50) NOT NULL,
    CondicionLogro NVARCHAR(50) NOT NULL,
    ValorMeta INT NOT NULL,
    Secreto BIT NOT NULL DEFAULT 0
);
GO

-- Tabla de relación Usuario-Logro
CREATE TABLE UsuarioLogros (
    UsuarioLogroID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT NOT NULL,
    LogroID INT NOT NULL,
    FechaDesbloqueo DATETIME NULL,
    ProgresoActual INT NOT NULL DEFAULT 0,
    Desbloqueado BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_UsuarioLogros_Usuarios FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID),
    CONSTRAINT FK_UsuarioLogros_Logros FOREIGN KEY (LogroID) REFERENCES Logros(LogroID),
    CONSTRAINT UQ_UsuarioLogros UNIQUE (UsuarioID, LogroID)
);
GO

-- Tabla de Roles (Agregada para el sistema de gamificación)
CREATE TABLE Roles (
    RolID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(255) NOT NULL,
    Icono NVARCHAR(50) NOT NULL,
    Color NVARCHAR(20) NOT NULL,
    DiasPorSemanaMinimo INT NOT NULL,
    DiasPorSemanaMaximo INT NOT NULL
);
GO

-- Tabla de relación Usuario-Rol
CREATE TABLE UsuarioRoles (
    UsuarioRolID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT NOT NULL,
    RolID INT NOT NULL,
    FechaAsignacion DATETIME NOT NULL DEFAULT GETDATE(),
    RolActual BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_UsuarioRoles_Usuarios FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID),
    CONSTRAINT FK_UsuarioRoles_Roles FOREIGN KEY (RolID) REFERENCES Roles(RolID)
);
GO

-- ÍNDICES PARA MEJORAR RENDIMIENTO
CREATE INDEX IDX_Mediciones_Usuario ON Mediciones(UsuarioID);
CREATE INDEX IDX_RutinasCompletadas_Usuario ON RutinasCompletadas(UsuarioID);
CREATE INDEX IDX_RutinasCompletadas_Entrenamiento ON RutinasCompletadas(EntrenamientoID);
CREATE INDEX IDX_RutinasCompletadas_Fecha ON RutinasCompletadas(FechaCompletada);
CREATE INDEX IDX_Entrenamientos_Autor ON Entrenamientos(AutorID);
CREATE INDEX IDX_Comentarios_Entrenamiento ON Comentarios(EntrenamientoID);
CREATE INDEX IDX_EntrenamientoEjercicios_Entrenamiento ON EntrenamientoEjercicios(EntrenamientoID);
CREATE INDEX IDX_Usuarios_Admin ON Usuarios(EsAdmin); -- NUEVO ÍNDICE PARA ADMINS
GO

-- VISTAS PARA ESTADÍSTICAS
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
GO

CREATE VIEW ResumenRutinasCompletadas AS
SELECT 
    UsuarioID,
    COUNT(*) AS TotalRutinas,
    SUM(CASE WHEN FechaCompletada >= DATEADD(day, -7, GETDATE()) THEN 1 ELSE 0 END) AS RutinasUltimaSemana,
    SUM(CASE WHEN FechaCompletada >= DATEADD(day, -30, GETDATE()) THEN 1 ELSE 0 END) AS RutinasUltimoMes,
    AVG(CAST(NivelEsfuerzoPercibido AS FLOAT)) AS PromedioEsfuerzo,
    SUM(CaloriasEstimadas) AS CaloriasTotales,
    SUM(DuracionMinutos) AS MinutosTotales
FROM RutinasCompletadas
GROUP BY UsuarioID;
GO

