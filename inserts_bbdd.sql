USE RecetasDB
GO

-- Insertar Unidades de Medida
INSERT INTO UnidadesMedida (Nombre, Abreviatura) VALUES 
('Gramo', 'g'),
('Kilogramo', 'kg'),
('Mililitro', 'ml'),
('Litro', 'L'),
('Cucharada', 'cda'),
('Cucharadita', 'cdta'),
('Taza', 'tz'),
('Unidad', 'u'),
('Pizca', 'pz')
GO

-- Insertar Categorías
INSERT INTO Categorias (Nombre, Descripcion) VALUES
('Postres', 'Dulces y postres variados'),
('Platos Principales', 'Platos fuertes para comidas y cenas'),
('Ensaladas', 'Platos frescos y saludables'),
('Sopas', 'Caldos, cremas y sopas'),
('Bebidas', 'Bebidas y cócteles'),
('Desayunos', 'Recetas para el desayuno')
GO

-- Insertar Usuarios
INSERT INTO Usuarios (Nombre, Apellido, Email, Password) VALUES
('María', 'González', 'maria@email.com', 'hash123'),
('Juan', 'Pérez', 'juan@email.com', 'hash456'),
('Ana', 'Martínez', 'ana@email.com', 'hash789')
GO

-- Insertar Ingredientes
INSERT INTO Ingredientes (Nombre, Descripcion, UnidadPredeterminada) VALUES
('Harina de trigo', 'Harina refinada todo uso', 1),
('Azúcar', 'Azúcar blanca refinada', 1),
('Huevos', 'Huevos frescos', 8),
('Leche', 'Leche entera', 4),
('Sal', 'Sal de mesa', 1),
('Aceite de oliva', 'Aceite de oliva extra virgen', 3),
('Levadura', 'Levadura seca instantánea', 1),
('Agua', 'Agua natural', 4)
GO

-- Insertar una receta y sus componentes usando transacción
BEGIN TRANSACTION

DECLARE @RecetaPanID INT

-- Insertar Receta
INSERT INTO Recetas (Titulo, Descripcion, TiempoPreparacion, Porciones, Dificultad, AutorID, CategoriaID)
VALUES ('Pan casero', 'Pan básico para principiantes', 120, 8, 'Media', 1, 2)

SET @RecetaPanID = SCOPE_IDENTITY()

-- Insertar Ingredientes por Receta
INSERT INTO RecetaIngredientes (RecetaID, IngredienteID, Cantidad, UnidadID)
SELECT @RecetaPanID, IngredienteID, 500, 1 FROM Ingredientes WHERE Nombre = 'Harina de trigo'
UNION ALL
SELECT @RecetaPanID, IngredienteID, 300, 3 FROM Ingredientes WHERE Nombre = 'Agua'
UNION ALL
SELECT @RecetaPanID, IngredienteID, 10, 1 FROM Ingredientes WHERE Nombre = 'Sal'
UNION ALL
SELECT @RecetaPanID, IngredienteID, 7, 1 FROM Ingredientes WHERE Nombre = 'Levadura'

-- Insertar Pasos de Preparación
INSERT INTO PasosPreparacion (RecetaID, NumeroPaso, Descripcion)
VALUES
(@RecetaPanID, 1, 'Mezclar la harina y la sal en un bowl grande'),
(@RecetaPanID, 2, 'Disolver la levadura en agua tibia'),
(@RecetaPanID, 3, 'Incorporar la levadura a los ingredientes secos y amasar por 10 minutos'),
(@RecetaPanID, 4, 'Dejar reposar la masa por 1 hora'),
(@RecetaPanID, 5, 'Hornear a 180°C por 45 minutos')

-- Insertar Etiquetas
INSERT INTO Etiquetas (Nombre) VALUES
('Vegetariano'),
('Sin Gluten'),
('Bajo en Calorías'),
('Rápido'),
('Para Niños')

-- Insertar Relaciones Recetas-Etiquetas
INSERT INTO RecetaEtiquetas (RecetaID, EtiquetaID)
SELECT @RecetaPanID, EtiquetaID FROM Etiquetas WHERE Nombre IN ('Vegetariano', 'Para Niños')

-- Insertar Comentarios
INSERT INTO Comentarios (RecetaID, UsuarioID, Contenido, Calificacion)
VALUES
(@RecetaPanID, 2, '¡Excelente receta! El pan quedó muy esponjoso', 5),
(@RecetaPanID, 3, 'Muy fácil de seguir, lo recomiendo', 4)

-- Insertar Favoritos
INSERT INTO RecetasFavoritas (UsuarioID, RecetaID)
VALUES
(2, @RecetaPanID),
(3, @RecetaPanID)

COMMIT TRANSACTION
GO