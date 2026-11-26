-- 1. Tabla Médicos
CREATE TABLE `Medicos` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `Nombres` VARCHAR(100) NOT NULL,
    `Apellidos` VARCHAR(100) NOT NULL,
    `CI` VARCHAR(20) NOT NULL,
    `Especialidad` VARCHAR(100) NOT NULL,
    `Telefono` VARCHAR(20) NOT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `UQ_Medicos_CI` (`CI`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 2. Tabla Pacientes
CREATE TABLE `Pacientes` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `Nombres` VARCHAR(100) NOT NULL,
    `Apellidos` VARCHAR(100) NOT NULL,
    `CI` VARCHAR(20) NOT NULL,
    `Telefono` VARCHAR(20) NOT NULL,
    `Direccion` VARCHAR(200) NOT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `UQ_Pacientes_CI` (`CI`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 3. Tabla Citas
CREATE TABLE `citas` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `PacienteId` INT NOT NULL,
    `MedicoId` INT NOT NULL,
    `FechaHora` DATETIME(6) NOT NULL,
    `Motivo` TEXT NOT NULL,
    `Estado` ENUM('Activa', 'Cancelada') NOT NULL DEFAULT 'Activa',
    `FechaCancelacion` DATETIME(6) NULL,
    `MotivoCancelacion` TEXT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_citas_Medicos` FOREIGN KEY (`MedicoId`) REFERENCES `Medicos` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_citas_Pacientes` FOREIGN KEY (`PacienteId`) REFERENCES `Pacientes` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 4. Tabla Historial
CREATE TABLE `Historial` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `CitaId` INT NOT NULL,
    `Accion` VARCHAR(50) NOT NULL,
    `Detalle` TEXT NOT NULL,
    `FechaRegistro` DATETIME(6) NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Historial_citas` FOREIGN KEY (`CitaId`) REFERENCES `citas` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 5. Triggers para Auditoría Automática en Historial
CREATE TRIGGER `trg_cita_insertada`
AFTER INSERT ON `citas`
FOR EACH ROW
BEGIN
    INSERT INTO `Historial` (`CitaId`, `Accion`, `Detalle`, `FechaRegistro`)
    VALUES (
        NEW.Id,
        IF(NEW.Estado = 'Cancelada', 'Cancelada', 'Creada'),
        IF(NEW.Estado = 'Cancelada',
           CONCAT('Cita registrada como cancelada. Motivo: ', IFNULL(NEW.MotivoCancelacion, 'Sin especificar')),
           CONCAT('Cita agendada para: ', NEW.Motivo)),
        NOW(6)
    );
END;

CREATE TRIGGER `trg_cita_actualizada`
AFTER UPDATE ON `citas`
FOR EACH ROW
BEGIN
    IF OLD.Estado = 'Activa' AND NEW.Estado = 'Cancelada' THEN
        INSERT INTO `Historial` (`CitaId`, `Accion`, `Detalle`, `FechaRegistro`)
        VALUES (
            NEW.Id,
            'Cancelada',
            CONCAT('Cita cancelada. Motivo: ', IFNULL(NEW.MotivoCancelacion, 'Sin especificar')),
            NOW(6)
        );
    ELSE
        INSERT INTO `Historial` (`CitaId`, `Accion`, `Detalle`, `FechaRegistro`)
        VALUES (
            NEW.Id,
            'Modificada',
            CONCAT('Cita actualizada. Motivo actual: ', NEW.Motivo),
            NOW(6)
        );
    END IF;
END;

-- 6. Procedimiento Almacenado para Cancelar Cita
CREATE PROCEDURE `sp_CancelarCita`(
    IN p_CitaId INT,
    IN p_MotivoCancelacion TEXT
)
BEGIN
    UPDATE `citas`
    SET `Estado` = 'Cancelada',
        `FechaCancelacion` = NOW(6),
        `MotivoCancelacion` = p_MotivoCancelacion
    WHERE `Id` = p_CitaId AND `Estado` = 'Activa';
END;

-- 7. Inserción de 10 Médicos
INSERT INTO `Medicos` (`Nombres`, `Apellidos`, `CI`, `Especialidad`, `Telefono`) VALUES
('Carlos', 'Mendoza', '4512389', 'Cardiología', '70145892'),
('Elena', 'Ríos', '5123490', 'Pediatría', '71236985'),
('Andrés', 'Salazar', '3987412', 'Medicina General', '68954123'),
('Patricia', 'Gómez', '6123874', 'Dermatología', '76589412'),
('Roberto', 'Vargas', '4875123', 'Traumatología', '72365418'),
('Lucía', 'Fernández', '5641238', 'Ginecología', '67412589'),
('Miguel Ángel', 'Torres', '4123987', 'Neurología', '79541236'),
('Gabriela', 'Castro', '6321458', 'Oftalmología', '73698541'),
('Fernando', 'Rojas', '5236987', 'Otorrinolaringología', '65897412'),
('Valeria', 'Paredes', '6458712', 'Endocrinología', '71458963');

-- 8. Inserción de 10 Pacientes
INSERT INTO `Pacientes` (`Nombres`, `Apellidos`, `CI`, `Telefono`, `Direccion`) VALUES
('María Fernanda', 'López', '8451236', '71548921', 'Av. Arce #2140, Sopocachi'),
('Jorge Luis', 'Quispe', '7912345', '72014589', 'Calle Comercio #845, Centro'),
('Ana Sofía', 'Vargas', '9123487', '68451239', 'Av. Ballivián #1020, Calacoto'),
('Luis Alberto', 'Mamani', '6543210', '76541289', 'Av. 6 de Marzo #450, El Alto'),
('Camila Andrea', 'Huanca', '7123984', '69854123', 'Calle 21 de Calacoto #312'),
('Diego Armando', 'Condori', '8234561', '73214569', 'Av. Busch #1580, Miraflores'),
('Natalia Belén', 'Chávez', '9345672', '67451238', 'Av. Saavedra #1920, Miraflores'),
('Sebastián Mauricio', 'Paz', '6789123', '78945612', 'Calle Loayza #320, Centro'),
('Daniela Alejandra', 'Soliz', '7456123', '65123987', 'Av. Costanera #510, Obrajes'),
('Mateo Nicolás', 'Zeballos', '8912345', '71236548', 'Av. Montenegro #780, San Miguel');

-- 9. Inserción de 10 Citas Activas
INSERT INTO `citas` (`Id`, `PacienteId`, `MedicoId`, `FechaHora`, `Motivo`, `Estado`) VALUES
(1,  1,  1,  '2026-11-15 08:30:00', 'Control de presión arterial', 'Activa'),
(2,  2,  2,  '2026-11-15 09:30:00', 'Chequeo pediátrico general', 'Activa'),
(3,  3,  3,  '2026-11-16 10:00:00', 'Consulta médica de rutina', 'Activa'),
(4,  4,  4,  '2026-11-16 11:15:00', 'Revisión dermatológica', 'Activa'),
(5,  5,  5,  '2026-11-17 14:00:00', 'Dolor en rodilla derecha', 'Activa'),
(6,  6,  6,  '2026-11-17 15:30:00', 'Control prenatal mensual', 'Activa'),
(7,  7,  7,  '2026-11-18 09:00:00', 'Evaluación por migrañas', 'Activa'),
(8,  8,  8,  '2026-11-18 10:45:00', 'Examen de agudeza visual', 'Activa'),
(9,  9,  9,  '2026-11-19 16:00:00', 'Congestión nasal persistente', 'Activa'),
(10, 10, 10, '2026-11-20 08:45:00', 'Control de tiroides', 'Activa');

-- 10. Inserción de 10 Citas Canceladas
INSERT INTO `citas` (`Id`, `PacienteId`, `MedicoId`, `FechaHora`, `Motivo`, `Estado`, `FechaCancelacion`, `MotivoCancelacion`) VALUES
(11, 1,  2,  '2026-11-01 09:00:00', 'Consulta cardiológica', 'Cancelada', '2026-10-31 17:20:00', 'El paciente reprogramó por viaje'),
(12, 2,  3,  '2026-11-02 10:30:00', 'Control general',       'Cancelada', '2026-11-02 08:10:00', 'Inconveniente laboral de última hora'),
(13, 3,  4,  '2026-11-03 11:00:00', 'Evaluación cutánea',    'Cancelada', '2026-11-02 19:00:00', 'Cruce de horarios con estudio'),
(14, 4,  5,  '2026-11-04 14:30:00', 'Revisión articular',    'Cancelada', '2026-11-03 12:15:00', 'Paciente solicitó cambio de fecha'),
(15, 5,  6,  '2026-11-05 16:00:00', 'Control ginecológico',  'Cancelada', '2026-11-05 09:40:00', 'Motivos personales'),
(16, 6,  7,  '2026-11-06 08:30:00', 'Consulta neurológica',  'Cancelada', '2026-11-05 18:05:00', 'Falta de resultados de laboratorio'),
(17, 7,  8,  '2026-11-07 10:15:00', 'Control oftalmológico', 'Cancelada', '2026-11-06 15:30:00', 'Reprogramada para la próxima semana'),
(18, 8,  9,  '2026-11-08 15:00:00', 'Revisión auditiva',     'Cancelada', '2026-11-08 11:00:00', 'Mejoría de síntomas'),
(19, 9,  10, '2026-11-09 09:45:00', 'Control hormonal',      'Cancelada', '2026-11-08 20:10:00', 'Imprevisto familiar'),
(20, 10, 1,  '2026-11-10 11:30:00', 'Electrocardiograma',    'Cancelada', '2026-11-09 14:45:00', 'Cambio de especialista');