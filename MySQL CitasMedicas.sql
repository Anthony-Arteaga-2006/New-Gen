CREATE DATABASE IF NOT EXISTS citas_medicas_db;
USE citas_medicas_db;

CREATE TABLE `medicos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` longtext NOT NULL,
  `Especialidad` longtext NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `pacientes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` longtext NOT NULL,
  `Documento` longtext NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `citasagregadas` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PacienteId` int(11) NOT NULL,
  `MedicoId` int(11) NOT NULL,
  `FechaHora` datetime NOT NULL,
  `Motivo` varchar(255) DEFAULT NULL,
  `Estado` varchar(50) DEFAULT 'Agendada',
  PRIMARY KEY (`Id`),
  KEY `PacienteId` (`PacienteId`),
  KEY `MedicoId` (`MedicoId`),
  CONSTRAINT `fk_agregadas_paciente` FOREIGN KEY (`PacienteId`) REFERENCES `pacientes` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `fk_agregadas_medico` FOREIGN KEY (`MedicoId`) REFERENCES `medicos` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE `citascanceladas` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PacienteId` int(11) NOT NULL,
  `MedicoId` int(11) NOT NULL,
  `FechaHoraOriginal` datetime NOT NULL,
  `FechaCancelacion` datetime DEFAULT current_timestamp(),
  `MotivoCancelacion` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `PacienteId` (`PacienteId`),
  KEY `MedicoId` (`MedicoId`),
  CONSTRAINT `fk_canceladas_paciente` FOREIGN KEY (`PacienteId`) REFERENCES `pacientes` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `fk_canceladas_medico` FOREIGN KEY (`MedicoId`) REFERENCES `medicos` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


-- 1. INSERTAR 10 MÉDICOS
INSERT INTO medicos (Nombre, Especialidad) VALUES 
('Dr. Carlos Mendoza', 'Cardiología'),
('Dra. Elena Ríos', 'Pediatría'),
('Dr. Andrés Salazar', 'Medicina General'),
('Dra. Patricia Gómez', 'Dermatología'),
('Dr. Roberto Vargas', 'Traumatología'),
('Dra. Lucía Fernández', 'Ginecología'),
('Dr. Miguel Ángel Torres', 'Neurología'),
('Dra. Gabriela Castro', 'Oftalmología'),
('Dr. Fernando Rojas', 'Otorrinolaringología'),
('Dra. Valeria Paredes', 'Endocrinología');

-- 2. INSERTAR 10 PACIENTES
INSERT INTO pacientes (Nombre, Documento) VALUES 
('María Fernanda López', '8451236'),
('Jorge Luis Quispe', '7912345'),
('Ana Sofía Vargas', '9123487'),
('Luis Alberto Mamani', '6543210'),
('Camila Andrea Huanca', '7123984'),
('Diego Armando Condori', '8234561'),
('Natalia Belén Chávez', '9345672'),
('Sebastián Mauricio Paz', '6789123'),
('Daniela Alejandra Soliz', '7456123'),
('Mateo Nicolás Zeballos', '8912345');

-- 3. INSERTAR 10 CITAS AGREGADAS (Activas)
INSERT INTO citasAgregadas (PacienteId, MedicoId, FechaHora, Motivo, Estado) VALUES 
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 0), (SELECT Id FROM Medicos LIMIT 1 OFFSET 0), '2026-11-15 08:30:00', 'Control de presión arterial', 'Pendiente'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 1), (SELECT Id FROM Medicos LIMIT 1 OFFSET 1), '2026-11-15 09:30:00', 'Chequeo pediátrico general', 'Pendiente'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 2), (SELECT Id FROM Medicos LIMIT 1 OFFSET 2), '2026-11-16 10:00:00', 'Consulta médica de rutina', 'Pendiente'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 3), (SELECT Id FROM Medicos LIMIT 1 OFFSET 3), '2026-11-16 11:15:00', 'Revisión dermatológica', 'Pendiente'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 4), (SELECT Id FROM Medicos LIMIT 1 OFFSET 4), '2026-11-17 14:00:00', 'Dolor en rodilla derecha', 'Pendiente'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 5), (SELECT Id FROM Medicos LIMIT 1 OFFSET 5), '2026-11-17 15:30:00', 'Control prenatal mensual', 'Pendiente'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 6), (SELECT Id FROM Medicos LIMIT 1 OFFSET 6), '2026-11-18 09:00:00', 'Evaluación por migrañas', 'Pendiente'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 7), (SELECT Id FROM Medicos LIMIT 1 OFFSET 7), '2026-11-18 10:45:00', 'Examen de agudeza visual', 'Pendiente'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 8), (SELECT Id FROM Medicos LIMIT 1 OFFSET 8), '2026-11-19 16:00:00', 'Congestión nasal persistente', 'Pendiente'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 9), (SELECT Id FROM Medicos LIMIT 1 OFFSET 9), '2026-11-20 08:45:00', 'Control de tiroides', 'Pendiente');

-- 4. INSERTAR 10 CITAS CANCELADAS
INSERT INTO citasCanceladas (PacienteId, MedicoId, FechaHoraOriginal, FechaCancelacion, MotivoCancelacion) VALUES 
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 0), (SELECT Id FROM Medicos LIMIT 1 OFFSET 1), '2026-11-01 09:00:00', '2026-10-31 17:20:00', 'El paciente reprogramó por viaje'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 1), (SELECT Id FROM Medicos LIMIT 1 OFFSET 2), '2026-11-02 10:30:00', '2026-11-02 08:10:00', 'Inconveniente laboral de última hora'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 2), (SELECT Id FROM Medicos LIMIT 1 OFFSET 3), '2026-11-03 11:00:00', '2026-11-02 19:00:00', 'Cruce de horarios con estudio'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 3), (SELECT Id FROM Medicos LIMIT 1 OFFSET 4), '2026-11-04 14:30:00', '2026-11-03 12:15:00', 'Paciente solicitó cambio de fecha'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 4), (SELECT Id FROM Medicos LIMIT 1 OFFSET 5), '2026-11-05 16:00:00', '2026-11-05 09:40:00', 'Motivos personales'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 5), (SELECT Id FROM Medicos LIMIT 1 OFFSET 6), '2026-11-06 08:30:00', '2026-11-05 18:05:00', 'Falta de resultados de laboratorio'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 6), (SELECT Id FROM Medicos LIMIT 1 OFFSET 7), '2026-11-07 10:15:00', '2026-11-06 15:30:00', 'Reprogramada para la próxima semana'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 7), (SELECT Id FROM Medicos LIMIT 1 OFFSET 8), '2026-11-08 15:00:00', '2026-11-08 11:00:00', 'Mejoría de síntomas'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 8), (SELECT Id FROM Medicos LIMIT 1 OFFSET 9), '2026-11-09 09:45:00', '2026-11-08 20:10:00', 'Imprevisto familiar'),
((SELECT Id FROM Pacientes LIMIT 1 OFFSET 9), (SELECT Id FROM Medicos LIMIT 1 OFFSET 0), '2026-11-10 11:30:00', '2026-11-09 14:45:00', 'Cambio de especialista');

CONSULTAS - BASE DE DATOS citas_medicas_db
==========================================

-- 1. Citas pendientes con nombre de paciente y medico (JOIN de 3 tablas)
SELECT c.Id, p.Nombre AS Paciente, m.Nombre AS Medico, c.FechaHora, c.Motivo
FROM citasagregadas c
JOIN pacientes p ON p.Id = c.PacienteId
JOIN Medicos m   ON m.Id = c.MedicoId
ORDER BY c.FechaHora;


-- 2. Cantidad de citas por medico (incluye medicos sin citas)
SELECT m.Nombre, m.Especialidad, COUNT(c.Id) AS TotalCitas
FROM Medicos m
LEFT JOIN citasagregadas c ON c.MedicoId = m.Id
GROUP BY m.Id, m.Nombre, m.Especialidad
ORDER BY TotalCitas DESC;


-- 3. Citas de Cardiologia (el LIKE atrapa tambien "Cardiologo")
SELECT p.Nombre AS Paciente, m.Nombre AS Medico, c.FechaHora, c.Motivo
FROM citasagregadas c
JOIN pacientes p ON p.Id = c.PacienteId
JOIN Medicos m   ON m.Id = c.MedicoId
WHERE m.Especialidad LIKE 'Cardio%';


-- 4. Paciente con mas citas canceladas
SELECT p.Nombre, COUNT(*) AS Cancelaciones
FROM citascanceladas cc
JOIN pacientes p ON p.Id = cc.PacienteId
GROUP BY p.Id, p.Nombre
ORDER BY Cancelaciones DESC
LIMIT 1;


-- 5. Con cuantas horas de anticipacion se cancelo cada cita
SELECT p.Nombre AS Paciente, cc.FechaHoraOriginal, cc.FechaCancelacion,
       TIMESTAMPDIFF(HOUR, cc.FechaCancelacion, cc.FechaHoraOriginal) AS HorasAnticipacion,
       cc.MotivoCancelacion
FROM citascanceladas cc
JOIN pacientes p ON p.Id = cc.PacienteId
ORDER BY HorasAnticipacion;


-- 6. Citas canceladas el mismo dia en que estaban programadas
SELECT p.Nombre AS Paciente, cc.FechaHoraOriginal, cc.FechaCancelacion, cc.MotivoCancelacion
FROM citascanceladas cc
JOIN pacientes p ON p.Id = cc.PacienteId
WHERE DATE(cc.FechaCancelacion) = DATE(cc.FechaHoraOriginal);


-- 7. Pacientes sin ninguna cita pendiente
SELECT p.Id, p.Nombre, p.Documento
FROM pacientes p
LEFT JOIN citasagregadas c ON c.PacienteId = p.Id
WHERE c.Id IS NULL;


-- 8. Total de citas por paciente (agendadas + canceladas juntas)
SELECT p.Nombre, COUNT(t.Id) AS TotalCitas
FROM pacientes p
LEFT JOIN (
    SELECT Id, PacienteId FROM citasagregadas
    UNION ALL
    SELECT Id, PacienteId FROM citascanceladas
) t ON t.PacienteId = p.Id
GROUP BY p.Id, p.Nombre
ORDER BY TotalCitas DESC;


-- 9. Cantidad de citas pendientes por dia
SELECT DATE(FechaHora) AS Dia, COUNT(*) AS Citas
FROM citasagregadas
GROUP BY DATE(FechaHora)
ORDER BY Dia;


-- 10. Porcentaje de cancelacion (subconsultas)
SELECT
  (SELECT COUNT(*) FROM citasagregadas)  AS Agendadas,
  (SELECT COUNT(*) FROM citascanceladas) AS Canceladas,
  ROUND(
    (SELECT COUNT(*) FROM citascanceladas) * 100 /
    ((SELECT COUNT(*) FROM citasagregadas) + (SELECT COUNT(*) FROM citascanceladas)), 2
  ) AS PorcentajeCancelacion;

