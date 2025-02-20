USE GymappDB
GO
-- Insertar Usuarios
INSERT INTO Usuarios (Nombre, Apellido, Email, Password, FechaRegistro, EstaActivo, Peso, Altura, Genero) VALUES
('Carlos', 'Pérez', 'carlos@example.com', 'hashed_password_1', GETDATE(), 1, 75.5, 180, 'Masculino'),
('Ana', 'López', 'ana@example.com', 'hashed_password_2', GETDATE(), 1, 60.2, 165, 'Femenino'),
('David', 'García', 'david@example.com', 'hashed_password_3', GETDATE(), 1, 82.0, 175, 'Masculino');
GO

INSERT INTO Entrenamientos (Titulo, Descripcion, DuracionMinutos, Dificultad, ImagenURL, FechaCreacion, Publico, AutorID) VALUES
('Full Body Express', 'Rutina rápida de cuerpo completo.', 45, 'Media', 'https://darebee.com/images/workouts/muscles/air-force-workout.jpg', GETDATE(), 1, 1),
('Fuerza Máxima Piernas', 'Entrenamiento centrado en fuerza.', 60, 'Difícil', 'https://darebee.com/images/workouts/muscles/glutes-and-quads-workout.jpg', GETDATE(), 1, 2),
('Hipertrofia Pecho y Tríceps', 'Rutina para desarrollar masa muscular.', 50, 'Fácil', 'https://darebee.com/images/workouts/muscles/pushup-party-workout.jpg', GETDATE(), 1, 3);

-- Insertar Ejercicios
INSERT INTO Ejercicios (Nombre, Descripcion, GrupoMuscular, ImagenURL, EquipamientoNecesario) VALUES
('Sentadilla', 'Ejercicio de piernas y glúteos.', 'Piernas', 'sentadilla.jpg', 1),
('Press de Banca', 'Ejercicio para el pectoral mayor.', 'Pecho', 'press_banca.jpg', 1),
('Dominadas', 'Ejercicio para la espalda y bíceps.', 'Espalda', 'dominadas.jpg', 0),
('Peso Muerto', 'Ejercicio para la cadena posterior.', 'Espalda', 'peso_muerto.jpg', 1),
('Curl de Bíceps', 'Ejercicio de aislamiento para bíceps.', 'Bíceps', 'curl_biceps.jpg', 1);
GO

-- Insertar Relación Entrenamiento - Ejercicios
INSERT INTO EntrenamientoEjercicios (EntrenamientoID, EjercicioID, Series, Repeticiones, DescansoSegundos, Notas) VALUES
(1, 1, 4, 12, 60, 'Mantén la espalda recta'),
(1, 3, 3, 10, 60, 'Asegúrate de controlar el movimiento'),
(2, 1, 5, 6, 90, 'Carga máxima sin comprometer la técnica'),
(2, 4, 4, 8, 90, 'Activa bien los glúteos al levantar'),
(3, 2, 4, 10, 60, 'No rebotes la barra sobre el pecho'),
(3, 5, 4, 12, 45, 'Aísla bien el bíceps evitando balanceos');
GO

-- Insertar Entrenamientos en Favoritos
INSERT INTO EntrenamientosFavoritos (UsuarioID, EntrenamientoID, FechaAgregado) VALUES
(1, 2, GETDATE()),
(2, 3, GETDATE()),
(3, 1, GETDATE());
GO

-- Insertar Comentarios en Entrenamientos
INSERT INTO Comentarios (EntrenamientoID, UsuarioID, Contenido, Calificacion, FechaComentario) VALUES
(1, 2, 'Muy buen entrenamiento, ideal para días ocupados.', 5, GETDATE()),
(2, 3, 'Me ha costado, pero es excelente para ganar fuerza.', 4, GETDATE()),
(3, 1, 'Rutina completa y efectiva.', 5, GETDATE());
GO
