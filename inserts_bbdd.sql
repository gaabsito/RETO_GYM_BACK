USE GymappDB
GO
-- Insertar Usuarios
INSERT INTO Usuarios (Nombre, Apellido, Email, Password, FechaRegistro, EstaActivo, ResetPasswordToken, ResetPasswordExpires) VALUES
('Carlos', 'Pérez', 'carlos@example.com', 'hashed_password_1', GETDATE(), 1, NULL, NULL),
('Ana', 'López', 'ana@example.com', 'hashed_password_2', GETDATE(), 1, NULL, NULL),
('David', 'García', 'david@example.com', 'hashed_password_3', GETDATE(), 1, NULL, NULL);
GO

-- Insertar Entrenamientos
INSERT INTO Entrenamientos (Titulo, Descripcion, DuracionMinutos, Dificultad, ImagenURL, FechaCreacion, Publico, AutorID) VALUES
('Full Body Express', 'Rutina rápida de cuerpo completo.', 45, 'Media', 'https://darebee.com/images/workouts/muscles/air-force-workout.jpg', GETDATE(), 1, 1),
('Fuerza Máxima Piernas', 'Entrenamiento centrado en fuerza.', 60, 'Difícil', 'https://darebee.com/images/workouts/muscles/glutes-and-quads-workout.jpg', GETDATE(), 1, 2),
('Hipertrofia Pecho y Tríceps', 'Rutina para desarrollar masa muscular.', 50, 'Fácil', 'https://darebee.com/images/workouts/muscles/pushup-party-workout.jpg', GETDATE(), 1, 3);

-- Insertar Ejercicios
INSERT INTO Ejercicios (Nombre, Descripcion, GrupoMuscular, ImagenURL, VideoURL, EquipamientoNecesario) VALUES
('Sentadilla', 'Ejercicio de piernas y glúteos.', 'Piernas', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Squat_d752e42d-02ba-4692-b300-c6e67ad5a4f5_600x600.png?v=1612138811', 'https://www.youtube.com/watch?v=i7J5h7BJ07g&list=PLyqKj7LwU2RuAg-us4mzap6izNcc1fuRW&index=5', 1),
('Press de Banca', 'Ejercicio para el pectoral mayor.', 'Pecho', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Barbell-Bench-Press_0316b783-43b2-44f8-8a2b-b177a2cfcbfc_600x600.png?v=1612137800', 'https://www.youtube.com/watch?v=EeE3f4VWFDo&list=PLyqKj7LwU2RuyZwWCIiDHuFZGN11QW3Ff&index=33', 1),
('Jalón al Pecho', 'Ejercicio para la espalda.', 'Espalda', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Wide-Grip-Pulldown_91fcba9b-47a2-4185-b093-aa542c81c55c_600x600.png?v=1612138105', 'https://www.youtube.com/watch?v=EUIri47Epcg&list=PLyqKj7LwU2RsCtKw3UlE85HYgPM3-xoO1&index=16', 0),
('Peso Muerto', 'Ejercicio para la cadena posterior.', 'Espalda', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Barbell-Romanian-Deadlift_34ede1b4-63ac-451d-9536-bbf9942b560c_600x600.png?v=1621162957', 'https://www.youtube.com/watch?v=CN_7cz3P-1U&list=PLyqKj7LwU2Rvx_O313dzJNFKPiEqRMWiV&index=10', 1),
('Curl de Bíceps', 'Ejercicio de aislamiento para bíceps.', 'Bíceps', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Barbell-Curl_f38580d5-412e-4082-b453-5d319afa94fd_600x600.png?v=1612137128', 'https://www.youtube.com/watch?v=JnLFSFurrqQ&list=PLyqKj7LwU2Rt1cwOsHwdXa2TiRzjA6uoB&index=3', 1);
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