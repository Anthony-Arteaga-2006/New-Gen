-- =========================================================================
-- 1. CREACIÓN DE LA BASE DE DATOS Y ESTRUCTURA DE TABLAS (HERENCIA TPT)
-- =========================================================================

CREATE DATABASE IF NOT EXISTS citas_medicas_db
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_general_ci;

USE citas_medicas_db;

-- Tabla de Médicos
CREATE TABLE `Medicos` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `Nombre` LONGTEXT NOT NULL,
    `Especialidad` LONGTEXT NOT NULL,
    CONSTRAINT `PK_Medicos` PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Tabla de Pacientes
CREATE TABLE `Pacientes` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `Nombre` LONGTEXT NOT NULL,
    `Documento` LONGTEXT NOT NULL,
    CONSTRAINT `PK_Pacientes` PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Tabla Padre: Citas (atributos comunes de la clase base)
CREATE TABLE `citas` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `PacienteId` INT NOT NULL,
    `MedicoId` INT NOT NULL,
    `FechaHora` DATETIME(6) NOT NULL,
    `Motivo` LONGTEXT NOT NULL,
    `Estado` LONGTEXT NOT NULL,
    CONSTRAINT `PK_citas` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_citas_Medicos_MedicoId` FOREIGN KEY (`MedicoId`) REFERENCES `Medicos` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_citas_Pacientes_PacienteId` FOREIGN KEY (`PacienteId`) REFERENCES `Pacientes` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Tabla Hija: citasagregadas (Hereda de citas compartiendo PK/FK Id)
CREATE TABLE `citasagregadas` (
    `Id` INT NOT NULL,
    CONSTRAINT `PK_citasagregadas` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_citasagregadas_citas_Id` FOREIGN KEY (`Id`) REFERENCES `citas` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Tabla Hija: citascanceladas (Hereda de citas + datos de cancelación)
CREATE TABLE `citascanceladas` (
    `Id` INT NOT NULL,
    `FechaHoraOriginal` DATETIME(6) NOT NULL,
    `FechaCancelacion` DATETIME(6) NOT NULL,
    `MotivoCancelacion` LONGTEXT NOT NULL,
    CONSTRAINT `PK_citascanceladas` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_citascanceladas_citas_Id` FOREIGN KEY (`Id`) REFERENCES `citas` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Tabla de historial de migraciones de Entity Framework Core
CREATE TABLE `__EFMigrationsHistory` (
    `MigrationId` VARCHAR(150) NOT NULL,
    `ProductVersion` VARCHAR(32) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- =========================================================================
-- 2. INSERCIÓN DE DATOS INICIALES (MÉDICOS, PACIENTES Y CITAS TPT)
-- =========================================================================

-- 10 Médicos
INSERT INTO `Medicos` (`Nombre`, `Especialidad`) VALUES
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

-- 10 Pacientes
INSERT INTO `Pacientes` (`Nombre`, `Documento`) VALUES
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

-- 10 Citas Agregadas (Primero en tabla padre 'citas', luego en hija 'citasagregadas')
INSERT INTO `citas` (`Id`, `PacienteId`, `MedicoId`, `FechaHora`, `Motivo`, `Estado`) VALUES
(1,  1,  1,  '2026-11-15 08:30:00', 'Control de presión arterial', 'Pendiente'),
(2,  2,  2,  '2026-11-15 09:30:00', 'Chequeo pediátrico general', 'Pendiente'),
(3,  3,  3,  '2026-11-16 10:00:00', 'Consulta médica de rutina', 'Pendiente'),
(4,  4,  4,  '2026-11-16 11:15:00', 'Revisión dermatológica', 'Pendiente'),
(5,  5,  5,  '2026-11-17 14:00:00', 'Dolor en rodilla derecha', 'Pendiente'),
(6,  6,  6,  '2026-11-17 15:30:00', 'Control prenatal mensual', 'Pendiente'),
(7,  7,  7,  '2026-11-18 09:00:00', 'Evaluación por migrañas', 'Pendiente'),
(8,  8,  8,  '2026-11-18 10:45:00', 'Examen de agudeza visual', 'Pendiente'),
(9,  9,  9,  '2026-11-19 16:00:00', 'Congestión nasal persistente', 'Pendiente'),
(10, 10, 10, '2026-11-20 08:45:00', 'Control de tiroides', 'Pendiente');

INSERT INTO `citasagregadas` (`Id`) VALUES
(1), (2), (3), (4), (5), (6), (7), (8), (9), (10);

-- 10 Citas Canceladas (Primero en tabla padre 'citas', luego detalles en 'citascanceladas')
INSERT INTO `citas` (`Id`, `PacienteId`, `MedicoId`, `FechaHora`, `Motivo`, `Estado`) VALUES
(11, 1,  2,  '2026-11-01 09:00:00', 'Consulta cardiológica', 'Cancelada'),
(12, 2,  3,  '2026-11-02 10:30:00', 'Control general', 'Cancelada'),
(13, 3,  4,  '2026-11-03 11:00:00', 'Evaluación cutánea', 'Cancelada'),
(14, 4,  5,  '2026-11-04 14:30:00', 'Revisión articular', 'Cancelada'),
(15, 5,  6,  '2026-11-05 16:00:00', 'Control ginecológico', 'Cancelada'),
(16, 6,  7,  '2026-11-06 08:30:00', 'Consulta neurológica', 'Cancelada'),
(17, 7,  8,  '2026-11-07 10:15:00', 'Control oftalmológico', 'Cancelada'),
(18, 8,  9,  '2026-11-08 15:00:00', 'Revisión auditiva', 'Cancelada'),
(19, 9,  10, '2026-11-09 09:45:00', 'Control hormonal', 'Cancelada'),
(20, 10, 1,  '2026-11-10 11:30:00', 'Electrocardiograma', 'Cancelada');

INSERT INTO `citascanceladas` (`Id`, `FechaHoraOriginal`, `FechaCancelacion`, `MotivoCancelacion`) VALUES
(11, '2026-11-01 09:00:00', '2026-10-31 17:20:00', 'El paciente reprogramó por viaje'),
(12, '2026-11-02 10:30:00', '2026-11-02 08:10:00', 'Inconveniente laboral de última hora'),
(13, '2026-11-03 11:00:00', '2026-11-02 19:00:00', 'Cruce de horarios con estudio'),
(14, '2026-11-04 14:30:00', '2026-11-03 12:15:00', 'Paciente solicitó cambio de fecha'),
(15, '2026-11-05 16:00:00', '2026-11-05 09:40:00', 'Motivos personales'),
(16, '2026-11-06 08:30:00', '2026-11-05 18:05:00', 'Falta de resultados de laboratorio'),
(17, '2026-11-07 10:15:00', '2026-11-06 15:30:00', 'Reprogramada para la próxima semana'),
(18, '2026-11-08 15:00:00', '2026-11-08 11:00:00', 'Mejoría de síntomas'),
(19, '2026-11-09 09:45:00', '2026-11-08 20:10:00', 'Imprevisto familiar'),
(20, '2026-11-10 11:30:00', '2026-11-09 14:45:00', 'Cambio de especialista');

-- ==========================================
-- CONSULTAS ADAPTADAS A TPT - citas_medicas_db
-- ==========================================

-- 1. Citas pendientes con nombre de paciente y medico (JOIN de 4 tablas en TPT)
SELECT c.Id, p.Nombre AS Paciente, m.Nombre AS Medico, c.FechaHora, c.Motivo
FROM citasagregadas ca
JOIN citas c     ON c.Id = ca.Id
JOIN Pacientes p ON p.Id = c.PacienteId
JOIN Medicos m   ON m.Id = c.MedicoId
ORDER BY c.FechaHora;


-- 2. Cantidad de citas activas por medico (incluye medicos sin citas)
SELECT m.Nombre, m.Especialidad, COUNT(ca.Id) AS TotalCitas
FROM Medicos m
LEFT JOIN citas c           ON c.MedicoId = m.Id
LEFT JOIN citasagregadas ca ON ca.Id = c.Id
GROUP BY m.Id, m.Nombre, m.Especialidad
ORDER BY TotalCitas DESC;


-- 3. Citas activas de Cardiologia (el LIKE atrapa tambien "Cardiologo")
SELECT p.Nombre AS Paciente, m.Nombre AS Medico, c.FechaHora, c.Motivo
FROM citasagregadas ca
JOIN citas c     ON c.Id = ca.Id
JOIN Pacientes p ON p.Id = c.PacienteId
JOIN Medicos m   ON m.Id = c.MedicoId
WHERE m.Especialidad LIKE 'Cardio%';


-- 4. Paciente con mas citas canceladas
SELECT p.Nombre, COUNT(*) AS Cancelaciones
FROM citascanceladas cc
JOIN citas c     ON c.Id = cc.Id
JOIN Pacientes p ON p.Id = c.PacienteId
GROUP BY p.Id, p.Nombre
ORDER BY Cancelaciones DESC
LIMIT 1;


-- 5. Con cuantas horas de anticipacion se cancelo cada cita
SELECT p.Nombre AS Paciente, cc.FechaHoraOriginal, cc.FechaCancelacion,
       TIMESTAMPDIFF(HOUR, cc.FechaCancelacion, cc.FechaHoraOriginal) AS HorasAnticipacion,
       cc.MotivoCancelacion
FROM citascanceladas cc
JOIN citas c     ON c.Id = cc.Id
JOIN Pacientes p ON p.Id = c.PacienteId
ORDER BY HorasAnticipacion;


-- 6. Citas canceladas el mismo dia en que estaban programadas
SELECT p.Nombre AS Paciente, cc.FechaHoraOriginal, cc.FechaCancelacion, cc.MotivoCancelacion
FROM citascanceladas cc
JOIN citas c     ON c.Id = cc.Id
JOIN Pacientes p ON p.Id = c.PacienteId
WHERE DATE(cc.FechaCancelacion) = DATE(cc.FechaHoraOriginal);


-- 7. Pacientes sin ninguna cita pendiente
SELECT p.Id, p.Nombre, p.Documento
FROM Pacientes p
WHERE NOT EXISTS (
    SELECT 1
    FROM citas c
    JOIN citasagregadas ca ON ca.Id = c.Id
    WHERE c.PacienteId = p.Id
);


-- 8. Total de citas por paciente (agendadas + canceladas juntas: directamente en la tabla padre 'citas')
SELECT p.Nombre, COUNT(c.Id) AS TotalCitas
FROM Pacientes p
LEFT JOIN citas c ON c.PacienteId = p.Id
GROUP BY p.Id, p.Nombre
ORDER BY TotalCitas DESC;


-- 9. Cantidad de citas pendientes por dia
SELECT DATE(c.FechaHora) AS Dia, COUNT(*) AS Citas
FROM citasagregadas ca
JOIN citas c ON c.Id = ca.Id
GROUP BY DATE(c.FechaHora)
ORDER BY Dia;


-- 10. Porcentaje de cancelacion (subconsultas)
SELECT
  (SELECT COUNT(*) FROM citasagregadas)  AS Agendadas,
  (SELECT COUNT(*) FROM citascanceladas) AS Canceladas,
  ROUND(
    (SELECT COUNT(*) FROM citascanceladas) * 100 /
    NULLIF((SELECT COUNT(*) FROM citasagregadas) + (SELECT COUNT(*) FROM citascanceladas), 0), 2
  ) AS PorcentajeCancelacion;