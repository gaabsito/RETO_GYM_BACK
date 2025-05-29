USE GymappDB
GO

-- Insertar Usuarios (CON CAMPO EsAdmin AÑADIDO)
INSERT INTO Usuarios (Nombre, Apellido, Email, Password, FechaRegistro, EstaActivo, EsAdmin, ResetPasswordToken, ResetPasswordExpires) VALUES
('Carlos', 'Pérez', 'carlos@example.com', 'hashed_password_1', GETDATE(), 1, 0, NULL, NULL),
('Ana', 'López', 'ana@example.com', 'hashed_password_2', GETDATE(), 1, 0, NULL, NULL),
('David', 'García', 'david@example.com', 'hashed_password_3', GETDATE(), 1, 0, NULL, NULL),
('Juanjo', 'Gutierrez', 'juanjo@example.com', 'hashed_password_4', GETDATE(), 1, 0, NULL, NULL);
GO

-- Insertar Entrenamientos
INSERT INTO Entrenamientos (Titulo, Descripcion, DuracionMinutos, Dificultad, ImagenURL, FechaCreacion, Publico, AutorID) VALUES
('Full Body Express', 'Rutina rápida de cuerpo completo.', 45, 'Media', 'https://darebee.com/images/workouts/muscles/air-force-workout.jpg', GETDATE(), 1, 1),
('Fuerza Máxima Piernas', 'Entrenamiento centrado en fuerza.', 60, 'Difícil', 'https://darebee.com/images/workouts/muscles/glutes-and-quads-workout.jpg', GETDATE(), 1, 2),
('Hipertrofia Pecho y Tríceps', 'Rutina para desarrollar masa muscular.', 50, 'Fácil', 'https://darebee.com/images/workouts/muscles/pushup-party-workout.jpg', GETDATE(), 1, 3),
('Abdominales y Core', 'Rutina para desarrollar masa muscular y core.', 40, 'Difícil', 'https://darebee.com/images/workouts/muscles/titan-core-workout.jpg', GETDATE(), 1, 4);

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

-- Insertar Ejercicios Adicionales
INSERT INTO Ejercicios (Nombre, Descripcion, GrupoMuscular, ImagenURL, VideoURL, EquipamientoNecesario) VALUES
-- Piernas
('Zancadas', 'Ejercicio para cuádriceps, glúteos e isquiotibiales.', 'Piernas', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Lunge_600x600.png?v=1612138903', 'https://www.youtube.com/watch?v=eFWCn5iEbTU', 0),
('Prensa de Piernas', 'Ejercicio en máquina para desarrollar cuádriceps.', 'Piernas', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Leg-Press_f7febd5c-75e5-42f4-9bb4-c938969ce293_600x600.png?v=1612138836', 'https://www.youtube.com/watch?v=yZmx_Ac3880', 1),
('Extensión de Cuádriceps', 'Aislamiento para la parte frontal del muslo.', 'Piernas', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Leg-Extension_41d91d3f-4b9c-4374-82e2-1d697ce35fe4_600x600.png?v=1612138862', 'https://www.youtube.com/watch?v=m0FOpMEgero', 1),
('Curl Femoral', 'Ejercicio para fortalecer isquiotibiales.', 'Piernas', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Lying-Leg-Curl_203153d8-79dd-4bb9-9125-708aa4327107_600x600.png?v=1612139013', 'https://www.youtube.com/watch?v=n5WDXD_mpVY', 1),
-- Pecho
('Aperturas con Mancuernas', 'Ejercicio de aislamiento para pectorales.', 'Pecho', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Dumbbell-Fly_119e2918-4241-4f55-bd77-c98a0c76c6c8_600x600.png?v=1612137840', 'https://www.youtube.com/watch?v=JFm8KbhjibM', 1),
('Push-Ups', 'Ejercicio básico para pecho y tríceps.', 'Pecho', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Push-Ups_600x600.png?v=1640121436', 'https://www.youtube.com/watch?v=mm6_WcoCVTA', 0),
('Press Inclinado con Mancuernas', 'Enfoca la parte superior del pectoral.', 'Pecho', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Incline-Dumbbell-Bench-Press_c2bf89a2-433f-4a8f-9801-67c679980867_600x600.png?v=1612138008', 'https://www.youtube.com/watch?v=5CECBjd7HLQ', 1),
-- Espalda
('Remo con Barra', 'Ejercicio para el desarrollo de la espalda media.', 'Espalda', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Barbell-Row_4beb1d94-bac9-4538-9578-2d9cf93ef008_600x600.png?v=1612138201', 'https://www.youtube.com/watch?v=6FZHJGzMFEc', 1),
('Remo con Mancuerna', 'Ejercicio unilateral para equilibrio muscular.', 'Espalda', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Dumbbell-Bent-Over-Row-_Single-Arm_49867db3-f465-4fbc-b359-29cbdda502e2_600x600.png?v=1612138069', 'https://www.youtube.com/watch?v=DMo3HJoawrU', 1),
-- Hombros
('Press Militar', 'Ejercicio compuesto para hombros.', 'Hombros', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Dumbbell-Shoulder-Press_da0aa742-620a-45f7-9277-78137d38ff28_600x600.png?v=1612138495', 'https://www.youtube.com/watch?v=HzIiNhHhhtA', 1),
('Elevaciones Laterales', 'Aislamiento para el deltoides medio.', 'Hombros', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Dumbbell-Lateral-Raise_31c81eee-81c4-4ffe-890d-ee13dd5bbf20_600x600.png?v=1612138523', 'https://www.youtube.com/watch?v=OuG1smZTsQQ', 1),
-- Brazos
('Extensión de Tríceps', 'Ejercicio de aislamiento para tríceps.', 'Tríceps', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Triceps-Pressdown_e759437b-6200-4b44-b484-14db770024a4_600x600.png?v=1612136845', 'https://www.youtube.com/watch?v=6Fzep104f0s', 1),
('Fondos en Paralelas', 'Ejercicio compuesto para tríceps y pecho.', 'Tríceps', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Parallel-Dip-Bar_600x600.png?v=1619977962', 'https://www.youtube.com/watch?v=4LA1kF7yCGo', 0),
('Press Francés con Mancuernas', 'Ejercicio para bíceps y braquial.', 'Bíceps', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Lying-Dumbbell-Triceps-Extension_600x600.png?v=1619978076', 'https://www.youtube.com/watch?v=l3rHYPtMUo8', 1),
-- Core y Abdominales
('Crunch', 'Ejercicio básico para abdominales superiores.', 'Abdominales', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Crunch_f3498d5d-82d9-4a7f-8dee-98a2e55a62f2_600x600.png?v=1612138317', 'https://www.youtube.com/watch?v=6GMKPQVERzw&list=PLyqKj7LwU2RvTgEW_QlCCjtIL5d_KP_-I&index=6', 0),
('Plancha', 'Ejercicio isométrico para el core.', 'Abdominales', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Plank_3a82d566-9cb2-4c20-b301-bc8bd635c4d1_600x600.png?v=1612138431', 'https://www.youtube.com/watch?v=Ff4_A3y7JR0&list=PLyqKj7LwU2RvTgEW_QlCCjtIL5d_KP_-I&index=10', 0),
('Crunch Oblicuo', 'Ejercicio para oblicuos y core rotacional.', 'Abdominales', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Oblique-Crunch_253d0361-395d-443b-8228-aff440c1eee9_600x600.png?v=1612138354', 'https://www.youtube.com/watch?v=KzEakx0Oja8&list=PLyqKj7LwU2RvTgEW_QlCCjtIL5d_KP_-I&index=13', 0),
('Elevación de Piernas', 'Ejercicio para abdominales inferiores.', 'Abdominales', 'https://cdn.shopify.com/s/files/1/0269/5551/3900/files/Hanging-Leg-Raise_36986393-d0a6-494a-981f-4ea06a99b0b5_600x600.png?v=1612138457', 'https://www.youtube.com/watch?v=7FwGZ8qY5OU&list=PLyqKj7LwU2RvTgEW_QlCCjtIL5d_KP_-I&index=2', 0);
GO

INSERT INTO Logros (Nombre, Descripcion, Icono, Color, Experiencia, Categoria, CondicionLogro, ValorMeta, Secreto)
VALUES
-- Logros de Entrenamientos Totales
('Primeros Pasos', 'Completa tu primer entrenamiento', 'mdi-shoe-print', '#9E9E9E', 10, 'Entrenamiento', 'TOTAL_ENTRENAMIENTOS', 1, 0),
('Principiante', 'Completa 5 entrenamientos', 'mdi-run', '#8BC34A', 20, 'Entrenamiento', 'TOTAL_ENTRENAMIENTOS', 5, 0),
('Habitual', 'Completa 20 entrenamientos', 'mdi-dumbbell', '#4CAF50', 50, 'Entrenamiento', 'TOTAL_ENTRENAMIENTOS', 20, 0),
('Dedicado', 'Completa 50 entrenamientos', 'mdi-arm-flex', '#2196F3', 100, 'Entrenamiento', 'TOTAL_ENTRENAMIENTOS', 50, 0),
('Atleta', 'Completa 100 entrenamientos', 'mdi-medal', '#FF9800', 200, 'Entrenamiento', 'TOTAL_ENTRENAMIENTOS', 100, 0),
('Imparable', 'Completa 250 entrenamientos', 'mdi-trophy', '#F44336', 500, 'Entrenamiento', 'TOTAL_ENTRENAMIENTOS', 250, 0),
('Leyenda', 'Completa 500 entrenamientos', 'mdi-crown', '#E91E63', 1000, 'Entrenamiento', 'TOTAL_ENTRENAMIENTOS', 500, 0),
-- Logros de Racha (Días Consecutivos)
('Consistente', 'Entrena 3 días consecutivos', 'mdi-calendar-check', '#8BC34A', 30, 'Racha', 'DIAS_CONSECUTIVOS', 3, 0),
('Persistente', 'Entrena 7 días consecutivos', 'mdi-calendar-week', '#4CAF50', 50, 'Racha', 'DIAS_CONSECUTIVOS', 7, 0),
('Máquina', 'Entrena 14 días consecutivos', 'mdi-robot', '#2196F3', 100, 'Racha', 'DIAS_CONSECUTIVOS', 14, 0),
('Sobrehumano', 'Entrena 30 días consecutivos', 'mdi-super-hero', '#F44336', 300, 'Racha', 'DIAS_CONSECUTIVOS', 30, 0),
-- Logros de Dificultad
('Desafío Aceptado', 'Completa un entrenamiento difícil', 'mdi-fire', '#FF9800', 30, 'Dificultad', 'ENTRENAMIENTOS_DIFICILES', 1, 0),
('Sin Límites', 'Completa 10 entrenamientos difíciles', 'mdi-flash', '#F44336', 100, 'Dificultad', 'ENTRENAMIENTOS_DIFICILES', 10, 0),
-- Logros de Rol
('Ascenso', 'Alcanza el nivel Constante', 'mdi-arrow-up-bold', '#8BC34A', 30, 'Rol', 'NIVEL_ROL', 2, 0),
('Compromiso', 'Alcanza el nivel Comprometido', 'mdi-handshake', '#4CAF50', 50, 'Rol', 'NIVEL_ROL', 3, 0),
('Élite', 'Alcanza el nivel Élite', 'mdi-star', '#E91E63', 200, 'Rol', 'NIVEL_ROL', 7, 0),
-- Logros Secretos (más difíciles de conseguir)
('Entrenador Nocturno', 'Completa 5 entrenamientos después de las 10 PM', 'mdi-weather-night', '#673AB7', 50, 'Secreto', 'ENTRENAMIENTOS_NOCTURNOS', 5, 1),
('Madrugador', 'Completa 5 entrenamientos antes de las 7 AM', 'mdi-weather-sunset-up', '#FF5722', 50, 'Secreto', 'ENTRENAMIENTOS_MADRUGADA', 5, 1),
('Domingo de Entrenamiento', 'Entrena 5 domingos consecutivos', 'mdi-calendar-weekend', '#9C27B0', 100, 'Secreto', 'DOMINGOS_CONSECUTIVOS', 5, 1);