CREATE TABLE Raza (
    ID_Raza INT PRIMARY KEY,
    Nombre_Raza VARCHAR(80) NOT NULL
);

CREATE TABLE Tipo_Animal (
    ID_TipoAnimal INT PRIMARY KEY,
    Nombre_Tipo VARCHAR(60) NOT NULL
);

CREATE TABLE Estado_Animal (
    ID_EstadoAnimal INT PRIMARY KEY,
    Nombre_Estado VARCHAR(60) NOT NULL
);

CREATE TABLE Categoria_Productiva (
    ID_Categoria INT PRIMARY KEY,
    Nombre_Categoria VARCHAR(60) NOT NULL
);

-- ANIMALES
CREATE TABLE Animal (
    ID_Animal INT PRIMARY KEY,
    Nombre_Animal VARCHAR(100) NOT NULL,
    Fecha_Nacimiento DATE,
    Sexo CHAR(1),
    Peso DECIMAL(10,2),
    ID_Raza INT NOT NULL,
    ID_TipoAnimal INT NOT NULL,
    ID_EstadoAnimal INT NOT NULL
);

CREATE TABLE Animal_Pesaje (
    ID_Pesaje INT PRIMARY KEY,
    ID_Animal INT NOT NULL,
    Fecha_Pesaje DATE NOT NULL,
    Peso DECIMAL(10,2) NOT NULL
);

-- M:N Animal - Categoria
CREATE TABLE Animal_Categoria (
    ID_Animal INT NOT NULL,
    ID_Categoria INT NOT NULL,
    CONSTRAINT PK_Animal_Categoria PRIMARY KEY (ID_Animal, ID_Categoria)
);

-- ALIMENTACION
CREATE TABLE Alimento (
    ID_Alimento INT PRIMARY KEY,
    Nombre_Alimento VARCHAR(100) NOT NULL
);

CREATE TABLE Tipo_Alimento (
    ID_TipoAlimento INT PRIMARY KEY,
    Nombre_Tipo VARCHAR(60) NOT NULL
);

CREATE TABLE Racion (
    ID_Racion INT PRIMARY KEY,
    Nombre_Racion VARCHAR(100) NOT NULL
);

-- M:N Animal - Racion
CREATE TABLE Animal_Racion (
    ID_Animal INT NOT NULL,
    ID_Racion INT NOT NULL,
    Fecha_Asignacion DATE NOT NULL,
    CONSTRAINT PK_Animal_Racion PRIMARY KEY (ID_Animal, ID_Racion, Fecha_Asignacion)
);

-- M:N Racion - Alimento
CREATE TABLE Racion_Alimento (
    ID_Racion INT NOT NULL,
    ID_Alimento INT NOT NULL,
    Cantidad DECIMAL(10,2) NOT NULL,
    CONSTRAINT PK_Racion_Alimento PRIMARY KEY (ID_Racion, ID_Alimento)
);

CREATE TABLE Consumo_Registro (
    ID_Consumo INT PRIMARY KEY,
    ID_Animal INT NOT NULL,
    ID_Alimento INT NOT NULL,
    Fecha_Consumo DATE NOT NULL,
    Cantidad DECIMAL(10,2) NOT NULL
);

CREATE TABLE Control_Nutricional (
    ID_ControlNutri INT PRIMARY KEY,
    ID_Animal INT NOT NULL,
    Fecha_Evaluacion DATE NOT NULL,
    Condicion_Corporal VARCHAR(40)
);

--  SANIDAD 
CREATE TABLE Vacuna (
    ID_Vacuna INT PRIMARY KEY,
    Nombre_Vacuna VARCHAR(100) NOT NULL
);

CREATE TABLE Tratamiento (
    ID_Tratamiento INT PRIMARY KEY,
    Nombre_Tratamiento VARCHAR(100) NOT NULL
);

CREATE TABLE Medicamento (
    ID_Medicamento INT PRIMARY KEY,
    Nombre_Medicamento VARCHAR(100) NOT NULL
);

CREATE TABLE Veterinario (
    ID_Veterinario INT PRIMARY KEY,
    Nombre_Veterinario VARCHAR(120) NOT NULL
);

-- M:N Animal - Vacuna
CREATE TABLE Animal_Vacuna (
    ID_Animal INT NOT NULL,
    ID_Vacuna INT NOT NULL,
    Fecha_Aplicacion DATE NOT NULL,
    CONSTRAINT PK_Animal_Vacuna PRIMARY KEY (ID_Animal, ID_Vacuna, Fecha_Aplicacion)
);

-- M:N Animal - Tratamiento
CREATE TABLE Animal_Tratamiento (
    ID_Animal INT NOT NULL,
    ID_Tratamiento INT NOT NULL,
    ID_Veterinario INT NOT NULL,
    Fecha_Tratamiento DATE NOT NULL,
    Observacion VARCHAR(200),
    CONSTRAINT PK_Animal_Tratamiento PRIMARY KEY (ID_Animal, ID_Tratamiento, Fecha_Tratamiento)
);

-- M:N Tratamiento - Medicamento
CREATE TABLE Tratamiento_Medicamento (
    ID_Tratamiento INT NOT NULL,
    ID_Medicamento INT NOT NULL,
    Dosis VARCHAR(40),
    CONSTRAINT PK_Tratamiento_Medicamento PRIMARY KEY (ID_Tratamiento, ID_Medicamento)
);

CREATE TABLE Registro_Sanitario (
    ID_RegistroSan INT PRIMARY KEY,
    ID_Animal INT NOT NULL,
    Fecha_Registro DATE NOT NULL,
    Descripcion VARCHAR(200)
);

-- PASTOREO Y AMBIENTE
CREATE TABLE Potrero (
    ID_Potrero INT PRIMARY KEY,
    Nombre_Potrero VARCHAR(100) NOT NULL,
    Area DECIMAL(10,2)
);

CREATE TABLE Tipo_Pasto (
    ID_TipoPasto INT PRIMARY KEY,
    Nombre_Pasto VARCHAR(80) NOT NULL
);

-- M:N Animal - Potrero
CREATE TABLE Animal_Potrero (
    ID_Animal INT NOT NULL,
    ID_Potrero INT NOT NULL,
    Fecha_Entrada DATE NOT NULL,
    Fecha_Salida DATE NULL,
    CONSTRAINT PK_Animal_Potrero PRIMARY KEY (ID_Animal, ID_Potrero, Fecha_Entrada)
);

CREATE TABLE Muestreo_Suelo (
    ID_Muestreo INT PRIMARY KEY,
    ID_Potrero INT NOT NULL,
    Fecha_Muestreo DATE NOT NULL,
    pH DECIMAL(4,2),
    Materia_Organica DECIMAL(5,2)
);

CREATE TABLE Gestion_Agua (
    ID_GestionAgua INT PRIMARY KEY,
    ID_Potrero INT NOT NULL,
    Fecha_Revision DATE NOT NULL,
    Estado_Bebedero VARCHAR(50)
);

CREATE TABLE Control_Ambiental (
    ID_ControlAmb INT PRIMARY KEY,
    ID_Potrero INT NOT NULL,
    Fecha_Control DATE NOT NULL,
    Observacion VARCHAR(200)
);

-- PRODUCCION LECHERA 
CREATE TABLE Control_Lechero (
    ID_Control INT PRIMARY KEY,
    Fecha_Control DATE NOT NULL
);

CREATE TABLE Periodo_Produccion (
    ID_Periodo INT PRIMARY KEY,
    Nombre_Periodo VARCHAR(60) NOT NULL
);

CREATE TABLE Ordenador ( 
    ID_Ordenador INT PRIMARY KEY,
    Nombre_Ordenador VARCHAR(120) NOT NULL
);

CREATE TABLE Ordeno ( 
    ID_Ordeno INT PRIMARY KEY,
    ID_Ordenador INT NOT NULL,
    Fecha_Ordeno DATE NOT NULL,
    Turno VARCHAR(20)
);

-- M:N Animal - Control_Lechero
CREATE TABLE Animal_ControlLechero (
    ID_Animal INT NOT NULL,
    ID_Control INT NOT NULL,
    Litros_Leche DECIMAL(10,2) NOT NULL,
    CONSTRAINT PK_Animal_ControlLechero PRIMARY KEY (ID_Animal, ID_Control)
);

--PERSONAL
CREATE TABLE Empleado (
    ID_Empleado INT PRIMARY KEY,
    Nombre_Empleado VARCHAR(120) NOT NULL,
    Fecha_Contratacion DATE
);

CREATE TABLE Cargo (
    ID_Cargo INT PRIMARY KEY,
    Nombre_Cargo VARCHAR(80) NOT NULL
);

-- M:N Empleado - Cargo
CREATE TABLE Empleado_Cargo (
    ID_Empleado INT NOT NULL,
    ID_Cargo INT NOT NULL,
    Fecha_Asignacion DATE NOT NULL,
    CONSTRAINT PK_Empleado_Cargo PRIMARY KEY (ID_Empleado, ID_Cargo, Fecha_Asignacion)
);

-- INFRAESTRUCTURA
CREATE TABLE Corral (
    ID_Corral INT PRIMARY KEY,
    Nombre_Corral VARCHAR(80) NOT NULL,
    Capacidad INT
);

-- M:N Animal - Corral
CREATE TABLE Animal_Corral (
    ID_Animal INT NOT NULL,
    ID_Corral INT NOT NULL,
    Fecha_Entrada DATE NOT NULL,
    Fecha_Salida DATE NULL,
    CONSTRAINT PK_Animal_Corral PRIMARY KEY (ID_Animal, ID_Corral, Fecha_Entrada)
);

--INICIO DE SESION
CREATE TABLE Rol (
    ID_Rol INT PRIMARY KEY,
    Nombre_Rol VARCHAR(50) NOT NULL
);

CREATE TABLE Usuario (
    ID_Usuario INT PRIMARY KEY,
    Nombre_Usuario VARCHAR(100) NOT NULL,
    Contrasena VARCHAR(255) NOT NULL,
    Correo VARCHAR(120),
    ID_Rol INT NOT NULL
);

-- TABLAS DE AUDITORIA Y BITACORA
CREATE TABLE Bitacora_Evento (
    ID_Bitacora INT IDENTITY(1,1) PRIMARY KEY,
    Fecha_Hora DATETIME NOT NULL DEFAULT GETDATE(),
    ID_Usuario INT NULL,
    Nombre_Usuario VARCHAR(100) NULL,
    Tipo_Evento VARCHAR(50) NOT NULL,
    Entidad VARCHAR(100) NULL,
    ID_Registro VARCHAR(200) NULL,
    Valores_Antes VARCHAR(MAX) NULL,
    Valores_Nuevos VARCHAR(MAX) NULL,
    Mensaje VARCHAR(4000) NULL,
    IP_Usuario VARCHAR(45) NULL
);

CREATE TABLE Bitacora_Error (
    ID_Error INT IDENTITY(1,1) PRIMARY KEY,
    Fecha_Hora DATETIME NOT NULL DEFAULT GETDATE(),
    ID_Usuario INT NULL,
    Nombre_Usuario VARCHAR(100) NULL,
    Tipo_Error VARCHAR(200) NOT NULL,
    Mensaje VARCHAR(4000) NOT NULL,
    Detalle_Error VARCHAR(MAX) NULL,
    Pagina VARCHAR(300) NULL,
    IP_Usuario VARCHAR(45) NULL
);

CREATE TABLE Bitacora_Request (
    ID_Request INT IDENTITY(1,1) PRIMARY KEY,
    Fecha_Inicio DATETIME NOT NULL,
    Fecha_Fin DATETIME NULL,
    Ruta VARCHAR(300) NOT NULL,
    Query_String VARCHAR(1000) NULL,
    Codigo_Estado INT NULL,
    ID_Usuario INT NULL,
    Nombre_Usuario VARCHAR(100) NULL,
    IP_Usuario VARCHAR(45) NULL,
    Tiempo_Ms INT NULL
);


-- LLAVES FORANEAS (ALTER TABLE)

ALTER TABLE Animal
ADD CONSTRAINT FK_Animal_Raza
FOREIGN KEY (ID_Raza) REFERENCES Raza(ID_Raza);

ALTER TABLE Animal
ADD CONSTRAINT FK_Animal_Tipo
FOREIGN KEY (ID_TipoAnimal) REFERENCES Tipo_Animal(ID_TipoAnimal);

ALTER TABLE Animal
ADD CONSTRAINT FK_Animal_Estado
FOREIGN KEY (ID_EstadoAnimal) REFERENCES Estado_Animal(ID_EstadoAnimal);

ALTER TABLE Animal_Pesaje
ADD CONSTRAINT FK_Pesaje_Animal
FOREIGN KEY (ID_Animal) REFERENCES Animal(ID_Animal);

ALTER TABLE Animal_Categoria
ADD CONSTRAINT FK_AnimalCategoria_Animal
FOREIGN KEY (ID_Animal) REFERENCES Animal(ID_Animal);

ALTER TABLE Animal_Categoria
ADD CONSTRAINT FK_AnimalCategoria_Categoria
FOREIGN KEY (ID_Categoria) REFERENCES Categoria_Productiva(ID_Categoria);

-- Alimentación
ALTER TABLE Racion_Alimento
ADD CONSTRAINT FK_RacionAlimento_Racion
FOREIGN KEY (ID_Racion) REFERENCES Racion(ID_Racion);

ALTER TABLE Racion_Alimento
ADD CONSTRAINT FK_RacionAlimento_Alimento
FOREIGN KEY (ID_Alimento) REFERENCES Alimento(ID_Alimento);

ALTER TABLE Animal_Racion
ADD CONSTRAINT FK_AnimalRacion_Animal
FOREIGN KEY (ID_Animal) REFERENCES Animal(ID_Animal);

ALTER TABLE Animal_Racion
ADD CONSTRAINT FK_AnimalRacion_Racion
FOREIGN KEY (ID_Racion) REFERENCES Racion(ID_Racion);

ALTER TABLE Consumo_Registro
ADD CONSTRAINT FK_Consumo_Animal
FOREIGN KEY (ID_Animal) REFERENCES Animal(ID_Animal);

ALTER TABLE Consumo_Registro
ADD CONSTRAINT FK_Consumo_Alimento
FOREIGN KEY (ID_Alimento) REFERENCES Alimento(ID_Alimento);

ALTER TABLE Control_Nutricional
ADD CONSTRAINT FK_ControlNutri_Animal
FOREIGN KEY (ID_Animal) REFERENCES Animal(ID_Animal);

-- Sanidad
ALTER TABLE Animal_Vacuna
ADD CONSTRAINT FK_AnimalVacuna_Animal
FOREIGN KEY (ID_Animal) REFERENCES Animal(ID_Animal);

ALTER TABLE Animal_Vacuna
ADD CONSTRAINT FK_AnimalVacuna_Vacuna
FOREIGN KEY (ID_Vacuna) REFERENCES Vacuna(ID_Vacuna);

ALTER TABLE Animal_Tratamiento
ADD CONSTRAINT FK_AnimalTratamiento_Animal
FOREIGN KEY (ID_Animal) REFERENCES Animal(ID_Animal);

ALTER TABLE Animal_Tratamiento
ADD CONSTRAINT FK_AnimalTratamiento_Tratamiento
FOREIGN KEY (ID_Tratamiento) REFERENCES Tratamiento(ID_Tratamiento);

ALTER TABLE Animal_Tratamiento
ADD CONSTRAINT FK_AnimalTratamiento_Veterinario
FOREIGN KEY (ID_Veterinario) REFERENCES Veterinario(ID_Veterinario);

ALTER TABLE Tratamiento_Medicamento
ADD CONSTRAINT FK_TrataMedi_Tratamiento
FOREIGN KEY (ID_Tratamiento) REFERENCES Tratamiento(ID_Tratamiento);

ALTER TABLE Tratamiento_Medicamento
ADD CONSTRAINT FK_TrataMedi_Medicamento
FOREIGN KEY (ID_Medicamento) REFERENCES Medicamento(ID_Medicamento);

ALTER TABLE Registro_Sanitario
ADD CONSTRAINT FK_RegistroSan_Animal
FOREIGN KEY (ID_Animal) REFERENCES Animal(ID_Animal);

-- Pastoreo y ambiente
ALTER TABLE Animal_Potrero
ADD CONSTRAINT FK_AnimalPotrero_Animal
FOREIGN KEY (ID_Animal) REFERENCES Animal(ID_Animal);

ALTER TABLE Animal_Potrero
ADD CONSTRAINT FK_AnimalPotrero_Potrero
FOREIGN KEY (ID_Potrero) REFERENCES Potrero(ID_Potrero);

ALTER TABLE Muestreo_Suelo
ADD CONSTRAINT FK_Muestreo_Potrero
FOREIGN KEY (ID_Potrero) REFERENCES Potrero(ID_Potrero);

ALTER TABLE Gestion_Agua
ADD CONSTRAINT FK_GestionAgua_Potrero
FOREIGN KEY (ID_Potrero) REFERENCES Potrero(ID_Potrero);

ALTER TABLE Control_Ambiental
ADD CONSTRAINT FK_ControlAmb_Potrero
FOREIGN KEY (ID_Potrero) REFERENCES Potrero(ID_Potrero);

-- Producción lechera
ALTER TABLE Ordeno
ADD CONSTRAINT FK_Ordeno_Ordenador
FOREIGN KEY (ID_Ordenador) REFERENCES Ordenador(ID_Ordenador);

ALTER TABLE Animal_ControlLechero
ADD CONSTRAINT FK_AnimalControl_Animal
FOREIGN KEY (ID_Animal) REFERENCES Animal(ID_Animal);

ALTER TABLE Animal_ControlLechero
ADD CONSTRAINT FK_AnimalControl_Control
FOREIGN KEY (ID_Control) REFERENCES Control_Lechero(ID_Control);


ALTER TABLE Animal_Corral
ADD CONSTRAINT FK_AnimalCorral_Animal
FOREIGN KEY (ID_Animal) REFERENCES Animal(ID_Animal);

ALTER TABLE Animal_Corral
ADD CONSTRAINT FK_AnimalCorral_Corral
FOREIGN KEY (ID_Corral) REFERENCES Corral(ID_Corral);

-- Login
ALTER TABLE Usuario
ADD CONSTRAINT FK_Usuario_Rol
FOREIGN KEY (ID_Rol) REFERENCES Rol(ID_Rol);

-- Auditoria 
ALTER TABLE Bitacora_Evento
ADD CONSTRAINT FK_BitacoraEvento_Usuario
FOREIGN KEY (ID_Usuario) REFERENCES Usuario(ID_Usuario);

ALTER TABLE Bitacora_Error
ADD CONSTRAINT FK_BitacoraError_Usuario
FOREIGN KEY (ID_Usuario) REFERENCES Usuario(ID_Usuario);

ALTER TABLE Bitacora_Request
ADD CONSTRAINT FK_BitacoraRequest_Usuario
FOREIGN KEY (ID_Usuario) REFERENCES Usuario(ID_Usuario);

GO
-- 1. Raza
CREATE PROCEDURE InsertarRaza
    @ID_Raza INT,
    @Nombre_Raza VARCHAR(80)
AS
BEGIN
    INSERT INTO Raza (ID_Raza, Nombre_Raza)
    VALUES (@ID_Raza, @Nombre_Raza);
    SELECT @ID_Raza AS ID_Raza;
END;
GO

CREATE PROCEDURE ListarRaza
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Raza, Nombre_Raza FROM Raza;
END;
GO


CREATE PROCEDURE ActualizarRaza
    @ID_Raza INT,
    @Nombre_Raza VARCHAR(80)
AS
BEGIN
    UPDATE Raza SET Nombre_Raza = @Nombre_Raza WHERE ID_Raza = @ID_Raza;
    SELECT @ID_Raza AS ID_Raza, @Nombre_Raza AS Nombre_Raza;
END;
GO

CREATE PROCEDURE EliminarRaza
    @ID_Raza INT
AS
BEGIN
    DELETE FROM Raza WHERE ID_Raza = @ID_Raza;
END;
GO

-- =========================================
-- 2. Tipo_Animal
-- =========================================
CREATE PROCEDURE InsertarTipoAnimal
    @ID_TipoAnimal INT,
    @Nombre_Tipo VARCHAR(60)
AS
BEGIN
    INSERT INTO Tipo_Animal (ID_TipoAnimal, Nombre_Tipo)
    VALUES (@ID_TipoAnimal, @Nombre_Tipo);
    SELECT @ID_TipoAnimal AS ID_TipoAnimal;
END;
GO

CREATE PROCEDURE ListarTipoAnimal
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_TipoAnimal, Nombre_Tipo FROM Tipo_Animal;
END;
GO

CREATE PROCEDURE ActualizarTipoAnimal
    @ID_TipoAnimal INT,
    @Nombre_Tipo VARCHAR(60)
AS
BEGIN
    UPDATE Tipo_Animal SET Nombre_Tipo = @Nombre_Tipo WHERE ID_TipoAnimal = @ID_TipoAnimal;
    SELECT @ID_TipoAnimal AS ID_TipoAnimal, @Nombre_Tipo AS Nombre_Tipo;
END;
GO

CREATE PROCEDURE EliminarTipoAnimal
    @ID_TipoAnimal INT
AS
BEGIN
    DELETE FROM Tipo_Animal WHERE ID_TipoAnimal = @ID_TipoAnimal;
END;
GO
-- =========================================
-- 3. Estado_Animal
-- =========================================
CREATE PROCEDURE InsertarEstadoAnimal
    @ID_EstadoAnimal INT,
    @Nombre_Estado VARCHAR(60)
AS
BEGIN
    INSERT INTO Estado_Animal (ID_EstadoAnimal, Nombre_Estado)
    VALUES (@ID_EstadoAnimal, @Nombre_Estado);
    SELECT @ID_EstadoAnimal AS ID_EstadoAnimal;
END;
GO

CREATE PROCEDURE ListarEstadoAnimal
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_EstadoAnimal, Nombre_Estado FROM Estado_Animal;
END;
GO

CREATE PROCEDURE ActualizarEstadoAnimal
    @ID_EstadoAnimal INT,
    @Nombre_Estado VARCHAR(60)
AS
BEGIN
    UPDATE Estado_Animal SET Nombre_Estado = @Nombre_Estado WHERE ID_EstadoAnimal = @ID_EstadoAnimal;
    SELECT @ID_EstadoAnimal AS ID_EstadoAnimal, @Nombre_Estado AS Nombre_Estado;
END;
GO

CREATE PROCEDURE EliminarEstadoAnimal
    @ID_EstadoAnimal INT
AS
BEGIN
    DELETE FROM Estado_Animal WHERE ID_EstadoAnimal = @ID_EstadoAnimal;
END;
GO

-- =========================================
-- 4. Categoria_Productiva
-- =========================================
CREATE PROCEDURE InsertarCategoriaProductiva
    @ID_Categoria INT,
    @Nombre_Categoria VARCHAR(60)
AS
BEGIN
    INSERT INTO Categoria_Productiva (ID_Categoria, Nombre_Categoria)
    VALUES (@ID_Categoria, @Nombre_Categoria);
    SELECT @ID_Categoria AS ID_Categoria;
END;
GO

CREATE PROCEDURE ListarCategoriaProductiva
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Categoria, Nombre_Categoria FROM Categoria_Productiva;
END;
GO

CREATE PROCEDURE ActualizarCategoriaProductiva
    @ID_Categoria INT,
    @Nombre_Categoria VARCHAR(60)
AS
BEGIN
    UPDATE Categoria_Productiva SET Nombre_Categoria = @Nombre_Categoria WHERE ID_Categoria = @ID_Categoria;
    SELECT @ID_Categoria AS ID_Categoria, @Nombre_Categoria AS Nombre_Categoria;
END;
GO

CREATE PROCEDURE EliminarCategoriaProductiva
    @ID_Categoria INT
AS
BEGIN
    DELETE FROM Categoria_Productiva WHERE ID_Categoria = @ID_Categoria;
END;
GO

-- =========================================
-- 5. Animal
-- =========================================
CREATE PROCEDURE InsertarAnimal
    @ID_Animal INT,
    @Nombre_Animal VARCHAR(100),
    @Fecha_Nacimiento DATE,
    @Sexo CHAR(1),
    @Peso DECIMAL(10,2),
    @ID_Raza INT,
    @ID_TipoAnimal INT,
    @ID_EstadoAnimal INT
AS
BEGIN
    INSERT INTO Animal (ID_Animal, Nombre_Animal, Fecha_Nacimiento, Sexo, Peso, ID_Raza, ID_TipoAnimal, ID_EstadoAnimal)
    VALUES (@ID_Animal, @Nombre_Animal, @Fecha_Nacimiento, @Sexo, @Peso, @ID_Raza, @ID_TipoAnimal, @ID_EstadoAnimal);
    SELECT @ID_Animal AS ID_Animal;
END;
GO

CREATE PROCEDURE ListarAnimal
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Animal, Nombre_Animal, Fecha_Nacimiento, Sexo, Peso, ID_Raza, ID_TipoAnimal, ID_EstadoAnimal FROM Animal;
END;
GO

CREATE PROCEDURE ActualizarAnimal
    @ID_Animal INT,
    @Nombre_Animal VARCHAR(100),
    @Fecha_Nacimiento DATE,
    @Sexo CHAR(1),
    @Peso DECIMAL(10,2),
    @ID_Raza INT,
    @ID_TipoAnimal INT,
    @ID_EstadoAnimal INT
AS
BEGIN
    UPDATE Animal
    SET Nombre_Animal = @Nombre_Animal,
        Fecha_Nacimiento = @Fecha_Nacimiento,
        Sexo = @Sexo,
        Peso = @Peso,
        ID_Raza = @ID_Raza,
        ID_TipoAnimal = @ID_TipoAnimal,
        ID_EstadoAnimal = @ID_EstadoAnimal
    WHERE ID_Animal = @ID_Animal;
    SELECT @ID_Animal AS ID_Animal;
END;
GO


CREATE PROCEDURE EliminarAnimal
    @ID_Animal INT
AS
BEGIN
    DELETE FROM Animal WHERE ID_Animal = @ID_Animal;
END;
GO
-- =========================================
-- 6. Animal_Pesaje
-- =========================================
CREATE PROCEDURE InsertarAnimalPesaje
    @ID_Pesaje INT,
    @ID_Animal INT,
    @Fecha_Pesaje DATE,
    @Peso DECIMAL(10,2)
AS
BEGIN
    INSERT INTO Animal_Pesaje (ID_Pesaje, ID_Animal, Fecha_Pesaje, Peso)
    VALUES (@ID_Pesaje, @ID_Animal, @Fecha_Pesaje, @Peso);
    SELECT @ID_Pesaje AS ID_Pesaje;
END;
GO

CREATE PROCEDURE ListarAnimalPesaje
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Pesaje, ID_Animal, Fecha_Pesaje, Peso FROM Animal_Pesaje;
END;
GO

CREATE PROCEDURE ActualizarAnimalPesaje
    @ID_Pesaje INT,
    @ID_Animal INT,
    @Fecha_Pesaje DATE,
    @Peso DECIMAL(10,2)
AS
BEGIN
    UPDATE Animal_Pesaje
    SET ID_Animal = @ID_Animal, Fecha_Pesaje = @Fecha_Pesaje, Peso = @Peso
    WHERE ID_Pesaje = @ID_Pesaje;
    SELECT @ID_Pesaje AS ID_Pesaje;
END;
GO

CREATE PROCEDURE EliminarAnimalPesaje
    @ID_Pesaje INT
AS
BEGIN
    DELETE FROM Animal_Pesaje WHERE ID_Pesaje = @ID_Pesaje;
END;
GO

-- =========================================
-- 7. Animal_Categoria
-- =========================================
CREATE PROCEDURE InsertarAnimalCategoria
    @ID_Animal INT,
    @ID_Categoria INT
AS
BEGIN
    INSERT INTO Animal_Categoria (ID_Animal, ID_Categoria)
    VALUES (@ID_Animal, @ID_Categoria);
    SELECT @ID_Animal AS ID_Animal, @ID_Categoria AS ID_Categoria;
END;
GO

CREATE PROCEDURE ListarAnimalCategoria
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Animal, ID_Categoria FROM Animal_Categoria;
END;
GO

CREATE PROCEDURE EliminarAnimalCategoria
    @ID_Animal INT,
    @ID_Categoria INT
AS
BEGIN
    DELETE FROM Animal_Categoria WHERE ID_Animal = @ID_Animal AND ID_Categoria = @ID_Categoria;
END;
GO

-- =========================================
-- 8. Alimento
-- =========================================
CREATE PROCEDURE InsertarAlimento
    @ID_Alimento INT,
    @Nombre_Alimento VARCHAR(100)
AS
BEGIN
    INSERT INTO Alimento (ID_Alimento, Nombre_Alimento)
    VALUES (@ID_Alimento, @Nombre_Alimento);
    SELECT @ID_Alimento AS ID_Alimento;
END;
GO

CREATE PROCEDURE ListarAlimento
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Alimento, Nombre_Alimento FROM Alimento;
END;
GO

CREATE PROCEDURE ActualizarAlimento
    @ID_Alimento INT,
    @Nombre_Alimento VARCHAR(100)
AS
BEGIN
    UPDATE Alimento
    SET Nombre_Alimento = @Nombre_Alimento
    WHERE ID_Alimento = @ID_Alimento;
    SELECT @ID_Alimento AS ID_Alimento, @Nombre_Alimento AS Nombre_Alimento;
END;
GO

CREATE PROCEDURE EliminarAlimento
    @ID_Alimento INT
AS
BEGIN
    DELETE FROM Alimento WHERE ID_Alimento = @ID_Alimento;
END;
GO

-- =========================================
-- 9. Tipo_Alimento
-- =========================================
CREATE PROCEDURE InsertarTipoAlimento
    @ID_TipoAlimento INT,
    @Nombre_Tipo VARCHAR(60)
AS
BEGIN
    INSERT INTO Tipo_Alimento (ID_TipoAlimento, Nombre_Tipo)
    VALUES (@ID_TipoAlimento, @Nombre_Tipo);
    SELECT @ID_TipoAlimento AS ID_TipoAlimento;
END;
GO

CREATE PROCEDURE ListarTipoAlimento
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_TipoAlimento, Nombre_Tipo FROM Tipo_Alimento;
END;
GO

CREATE PROCEDURE ActualizarTipoAlimento
    @ID_TipoAlimento INT,
    @Nombre_Tipo VARCHAR(60)
AS
BEGIN
    UPDATE Tipo_Alimento
    SET Nombre_Tipo = @Nombre_Tipo
    WHERE ID_TipoAlimento = @ID_TipoAlimento;
    SELECT @ID_TipoAlimento AS ID_TipoAlimento, @Nombre_Tipo AS Nombre_Tipo;
END;
GO

CREATE PROCEDURE EliminarTipoAlimento
    @ID_TipoAlimento INT
AS
BEGIN
    DELETE FROM Tipo_Alimento WHERE ID_TipoAlimento = @ID_TipoAlimento;
END;
GO

-- =========================================
-- 10. Racion
-- =========================================
CREATE PROCEDURE InsertarRacion
    @ID_Racion INT,
    @Nombre_Racion VARCHAR(100)
AS
BEGIN
    INSERT INTO Racion (ID_Racion, Nombre_Racion)
    VALUES (@ID_Racion, @Nombre_Racion);
    SELECT @ID_Racion AS ID_Racion;
END;
GO


CREATE PROCEDURE ListarRacion
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Racion, Nombre_Racion FROM Racion;
END;
GO

CREATE PROCEDURE ActualizarRacion
    @ID_Racion INT,
    @Nombre_Racion VARCHAR(100)
AS
BEGIN
    UPDATE Racion
    SET Nombre_Racion = @Nombre_Racion
    WHERE ID_Racion = @ID_Racion;
    SELECT @ID_Racion AS ID_Racion, @Nombre_Racion AS Nombre_Racion;
END;
GO

CREATE PROCEDURE EliminarRacion
    @ID_Racion INT
AS
BEGIN
    DELETE FROM Racion WHERE ID_Racion = @ID_Racion;
END;
GO

-- =========================================
-- 11. Animal_Racion (M:N, PK incluye Fecha_Asignacion)
-- =========================================
CREATE PROCEDURE InsertarAnimalRacion
    @ID_Animal INT,
    @ID_Racion INT,
    @Fecha_Asignacion DATE
AS
BEGIN
    INSERT INTO Animal_Racion (ID_Animal, ID_Racion, Fecha_Asignacion)
    VALUES (@ID_Animal, @ID_Racion, @Fecha_Asignacion);
    SELECT @ID_Animal AS ID_Animal, @ID_Racion AS ID_Racion, @Fecha_Asignacion AS Fecha_Asignacion;
END;
GO

CREATE PROCEDURE ListarAnimalRacion
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Animal, ID_Racion, Fecha_Asignacion FROM Animal_Racion;
END;
GO

-- No hay campos no-clave para actualizar (la PK incluye la fecha)
-- Eliminaci�n por clave compuesta
CREATE PROCEDURE EliminarAnimalRacion
    @ID_Animal INT,
    @ID_Racion INT,
    @Fecha_Asignacion DATE
AS
BEGIN
    DELETE FROM Animal_Racion
    WHERE ID_Animal = @ID_Animal
      AND ID_Racion = @ID_Racion
      AND Fecha_Asignacion = @Fecha_Asignacion;
END;
GO


-- =========================================
-- 12. Racion_Alimento (M:N con Cantidad)
-- =========================================
CREATE PROCEDURE InsertarRacionAlimento
    @ID_Racion INT,
    @ID_Alimento INT,
    @Cantidad DECIMAL(10,2)
AS
BEGIN
    INSERT INTO Racion_Alimento (ID_Racion, ID_Alimento, Cantidad)
    VALUES (@ID_Racion, @ID_Alimento, @Cantidad);
    SELECT @ID_Racion AS ID_Racion, @ID_Alimento AS ID_Alimento;
END;
GO

CREATE PROCEDURE ListarRacionAlimento
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Racion, ID_Alimento, Cantidad FROM Racion_Alimento;
END;
GO

CREATE PROCEDURE ActualizarRacionAlimento
    @ID_Racion INT,
    @ID_Alimento INT,
    @Cantidad DECIMAL(10,2)
AS
BEGIN
    UPDATE Racion_Alimento
    SET Cantidad = @Cantidad
    WHERE ID_Racion = @ID_Racion AND ID_Alimento = @ID_Alimento;
    SELECT @ID_Racion AS ID_Racion, @ID_Alimento AS ID_Alimento, @Cantidad AS Cantidad;
END;
GO

CREATE PROCEDURE EliminarRacionAlimento
    @ID_Racion INT,
    @ID_Alimento INT
AS
BEGIN
    DELETE FROM Racion_Alimento
    WHERE ID_Racion = @ID_Racion AND ID_Alimento = @ID_Alimento;
END;
GO

-- =========================================
-- 13. Consumo_Registro
-- =========================================
CREATE PROCEDURE InsertarConsumoRegistro
    @ID_Consumo INT,
    @ID_Animal INT,
    @ID_Alimento INT,
    @Fecha_Consumo DATE,
    @Cantidad DECIMAL(10,2)
AS
BEGIN
    INSERT INTO Consumo_Registro (ID_Consumo, ID_Animal, ID_Alimento, Fecha_Consumo, Cantidad)
    VALUES (@ID_Consumo, @ID_Animal, @ID_Alimento, @Fecha_Consumo, @Cantidad);
    SELECT @ID_Consumo AS ID_Consumo;
END;
GO

CREATE PROCEDURE ListarConsumoRegistro
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Consumo, ID_Animal, ID_Alimento, Fecha_Consumo, Cantidad FROM Consumo_Registro;
END;
GO

CREATE PROCEDURE ActualizarConsumoRegistro
    @ID_Consumo INT,
    @ID_Animal INT,
    @ID_Alimento INT,
    @Fecha_Consumo DATE,
    @Cantidad DECIMAL(10,2)
AS
BEGIN
    UPDATE Consumo_Registro
    SET ID_Animal = @ID_Animal,
        ID_Alimento = @ID_Alimento,
        Fecha_Consumo = @Fecha_Consumo,
        Cantidad = @Cantidad
    WHERE ID_Consumo = @ID_Consumo;
    SELECT @ID_Consumo AS ID_Consumo;
END;
GO


CREATE PROCEDURE EliminarConsumoRegistro
    @ID_Consumo INT
AS
BEGIN
    DELETE FROM Consumo_Registro WHERE ID_Consumo = @ID_Consumo;
END;
GO

-- =========================================
-- 14. Control_Nutricional
-- =========================================
CREATE PROCEDURE InsertarControlNutricional
    @ID_ControlNutri INT,
    @ID_Animal INT,
    @Fecha_Evaluacion DATE,
    @Condicion_Corporal VARCHAR(40)
AS
BEGIN
    INSERT INTO Control_Nutricional (ID_ControlNutri, ID_Animal, Fecha_Evaluacion, Condicion_Corporal)
    VALUES (@ID_ControlNutri, @ID_Animal, @Fecha_Evaluacion, @Condicion_Corporal);
    SELECT @ID_ControlNutri AS ID_ControlNutri;
END;
GO

CREATE PROCEDURE ListarControlNutricional
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_ControlNutri, ID_Animal, Fecha_Evaluacion, Condicion_Corporal FROM Control_Nutricional;
END;
GO

CREATE PROCEDURE ActualizarControlNutricional
    @ID_ControlNutri INT,
    @ID_Animal INT,
    @Fecha_Evaluacion DATE,
    @Condicion_Corporal VARCHAR(40)
AS
BEGIN
    UPDATE Control_Nutricional
    SET ID_Animal = @ID_Animal,
        Fecha_Evaluacion = @Fecha_Evaluacion,
        Condicion_Corporal = @Condicion_Corporal
    WHERE ID_ControlNutri = @ID_ControlNutri;
    SELECT @ID_ControlNutri AS ID_ControlNutri;
END;
GO

CREATE PROCEDURE EliminarControlNutricional
    @ID_ControlNutri INT
AS
BEGIN
    DELETE FROM Control_Nutricional WHERE ID_ControlNutri = @ID_ControlNutri;
END;
GO


-- =========================================
-- 15. Vacuna
-- =========================================
CREATE PROCEDURE InsertarVacuna
    @ID_Vacuna INT,
    @Nombre_Vacuna VARCHAR(100)
AS
BEGIN
    INSERT INTO Vacuna (ID_Vacuna, Nombre_Vacuna)
    VALUES (@ID_Vacuna, @Nombre_Vacuna);
    SELECT @ID_Vacuna AS ID_Vacuna;
END;
GO

CREATE PROCEDURE ListarVacuna
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Vacuna, Nombre_Vacuna FROM Vacuna;
END;
GO

CREATE PROCEDURE ActualizarVacuna
    @ID_Vacuna INT,
    @Nombre_Vacuna VARCHAR(100)
AS
BEGIN
    UPDATE Vacuna
    SET Nombre_Vacuna = @Nombre_Vacuna
    WHERE ID_Vacuna = @ID_Vacuna;
    SELECT @ID_Vacuna AS ID_Vacuna, @Nombre_Vacuna AS Nombre_Vacuna;
END;
GO

CREATE PROCEDURE EliminarVacuna
    @ID_Vacuna INT
AS
BEGIN
    DELETE FROM Vacuna WHERE ID_Vacuna = @ID_Vacuna;
END;
GO

-- =========================================
-- 16. Tratamiento
-- =========================================
CREATE PROCEDURE InsertarTratamiento
    @ID_Tratamiento INT,
    @Nombre_Tratamiento VARCHAR(100)
AS
BEGIN
    INSERT INTO Tratamiento (ID_Tratamiento, Nombre_Tratamiento)
    VALUES (@ID_Tratamiento, @Nombre_Tratamiento);
    SELECT @ID_Tratamiento AS ID_Tratamiento;
END;
GO

CREATE PROCEDURE ListarTratamiento
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Tratamiento, Nombre_Tratamiento FROM Tratamiento;
END;
GO

CREATE PROCEDURE ActualizarTratamiento
    @ID_Tratamiento INT,
    @Nombre_Tratamiento VARCHAR(100)
AS
BEGIN
    UPDATE Tratamiento
    SET Nombre_Tratamiento = @Nombre_Tratamiento
    WHERE ID_Tratamiento = @ID_Tratamiento;
    SELECT @ID_Tratamiento AS ID_Tratamiento, @Nombre_Tratamiento AS Nombre_Tratamiento;
END;
GO

CREATE PROCEDURE EliminarTratamiento
    @ID_Tratamiento INT
AS
BEGIN
    DELETE FROM Tratamiento WHERE ID_Tratamiento = @ID_Tratamiento;
END;
GO


-- =========================================
-- 17. Medicamento
-- =========================================
CREATE PROCEDURE InsertarMedicamento
    @ID_Medicamento INT,
    @Nombre_Medicamento VARCHAR(100)
AS
BEGIN
    INSERT INTO Medicamento (ID_Medicamento, Nombre_Medicamento)
    VALUES (@ID_Medicamento, @Nombre_Medicamento);
    SELECT @ID_Medicamento AS ID_Medicamento;
END;
GO


CREATE PROCEDURE ListarMedicamento
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Medicamento, Nombre_Medicamento FROM Medicamento;
END;
GO


CREATE PROCEDURE ActualizarMedicamento
    @ID_Medicamento INT,
    @Nombre_Medicamento VARCHAR(100)
AS
BEGIN
    UPDATE Medicamento
    SET Nombre_Medicamento = @Nombre_Medicamento
    WHERE ID_Medicamento = @ID_Medicamento;
    SELECT @ID_Medicamento AS ID_Medicamento, @Nombre_Medicamento AS Nombre_Medicamento;
END;
GO


CREATE PROCEDURE EliminarMedicamento
    @ID_Medicamento INT
AS
BEGIN
    DELETE FROM Medicamento WHERE ID_Medicamento = @ID_Medicamento;
END;
GO


-- =========================================
-- 18. Veterinario
-- =========================================
CREATE PROCEDURE InsertarVeterinario
    @ID_Veterinario INT,
    @Nombre_Veterinario VARCHAR(120)
AS
BEGIN
    INSERT INTO Veterinario (ID_Veterinario, Nombre_Veterinario)
    VALUES (@ID_Veterinario, @Nombre_Veterinario);
    SELECT @ID_Veterinario AS ID_Veterinario;
END;
GO


CREATE PROCEDURE ListarVeterinario
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Veterinario, Nombre_Veterinario FROM Veterinario;
END;
GO


CREATE PROCEDURE ActualizarVeterinario
    @ID_Veterinario INT,
    @Nombre_Veterinario VARCHAR(120)
AS
BEGIN
    UPDATE Veterinario SET Nombre_Veterinario = @Nombre_Veterinario WHERE ID_Veterinario = @ID_Veterinario;
    SELECT @ID_Veterinario AS ID_Veterinario, @Nombre_Veterinario AS Nombre_Veterinario;
END;
GO

CREATE PROCEDURE EliminarVeterinario
    @ID_Veterinario INT
AS
BEGIN
    DELETE FROM Veterinario WHERE ID_Veterinario = @ID_Veterinario;
END;
GO


-- =========================================
-- 19. Animal_Vacuna
-- =========================================
CREATE PROCEDURE InsertarAnimalVacuna
    @ID_Animal INT,
    @ID_Vacuna INT,
    @Fecha_Aplicacion DATE
AS
BEGIN
    INSERT INTO Animal_Vacuna (ID_Animal, ID_Vacuna, Fecha_Aplicacion)
    VALUES (@ID_Animal, @ID_Vacuna, @Fecha_Aplicacion);
    SELECT @ID_Animal AS ID_Animal, @ID_Vacuna AS ID_Vacuna, @Fecha_Aplicacion AS Fecha_Aplicacion;
END;
GO


CREATE PROCEDURE ListarAnimalVacuna
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Animal, ID_Vacuna, Fecha_Aplicacion FROM Animal_Vacuna;
END;
GO

CREATE PROCEDURE EliminarAnimalVacuna
    @ID_Animal INT,
    @ID_Vacuna INT,
    @Fecha_Aplicacion DATE
AS
BEGIN
    DELETE FROM Animal_Vacuna
    WHERE ID_Animal = @ID_Animal AND ID_Vacuna = @ID_Vacuna AND Fecha_Aplicacion = @Fecha_Aplicacion;
END;
GO



-- =========================================
-- 20. Animal_Tratamiento
-- =========================================
CREATE PROCEDURE InsertarAnimalTratamiento
    @ID_Animal INT,
    @ID_Tratamiento INT,
    @ID_Veterinario INT,
    @Fecha_Tratamiento DATE,
    @Observacion VARCHAR(200)
AS
BEGIN
    INSERT INTO Animal_Tratamiento (ID_Animal, ID_Tratamiento, ID_Veterinario, Fecha_Tratamiento, Observacion)
    VALUES (@ID_Animal, @ID_Tratamiento, @ID_Veterinario, @Fecha_Tratamiento, @Observacion);
    SELECT @ID_Animal AS ID_Animal, @ID_Tratamiento AS ID_Tratamiento;
END;
GO


CREATE PROCEDURE ListarAnimalTratamiento
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Animal, ID_Tratamiento, ID_Veterinario, Fecha_Tratamiento, Observacion FROM Animal_Tratamiento;
END;
GO


CREATE PROCEDURE ActualizarAnimalTratamiento
    @ID_Animal INT,
    @ID_Tratamiento INT,
    @Fecha_Tratamiento DATE,
    @ID_Veterinario INT,
    @Observacion VARCHAR(200),
    @Nueva_Fecha_Tratamiento DATE
AS
BEGIN
    
    IF @Fecha_Tratamiento <> @Nueva_Fecha_Tratamiento
    BEGIN
        
        INSERT INTO Animal_Tratamiento (ID_Animal, ID_Tratamiento, ID_Veterinario, Fecha_Tratamiento, Observacion)
        VALUES (@ID_Animal, @ID_Tratamiento, @ID_Veterinario, @Nueva_Fecha_Tratamiento, @Observacion);
        
        
        DELETE FROM Animal_Tratamiento 
        WHERE ID_Animal = @ID_Animal AND ID_Tratamiento = @ID_Tratamiento AND Fecha_Tratamiento = @Fecha_Tratamiento;
    END
    ELSE
    BEGIN
        
        UPDATE Animal_Tratamiento
        SET ID_Veterinario = @ID_Veterinario,
            Observacion = @Observacion
        WHERE ID_Animal = @ID_Animal AND ID_Tratamiento = @ID_Tratamiento AND Fecha_Tratamiento = @Fecha_Tratamiento;
    END
    
    SELECT @ID_Animal AS ID_Animal, @ID_Tratamiento AS ID_Tratamiento;
END;
GO

CREATE PROCEDURE EliminarAnimalTratamiento
    @ID_Animal INT,
    @ID_Tratamiento INT,
    @Fecha_Tratamiento DATE
AS
BEGIN
    DELETE FROM Animal_Tratamiento
    WHERE ID_Animal = @ID_Animal AND ID_Tratamiento = @ID_Tratamiento AND Fecha_Tratamiento = @Fecha_Tratamiento;
END;
GO


-- =========================================
-- 21. Tratamiento_Medicamento
-- =========================================
CREATE PROCEDURE InsertarTratamientoMedicamento
    @ID_Tratamiento INT,
    @ID_Medicamento INT,
    @Dosis VARCHAR(40)
AS
BEGIN
    INSERT INTO Tratamiento_Medicamento (ID_Tratamiento, ID_Medicamento, Dosis)
    VALUES (@ID_Tratamiento, @ID_Medicamento, @Dosis);
    SELECT @ID_Tratamiento AS ID_Tratamiento, @ID_Medicamento AS ID_Medicamento;
END;
GO


CREATE PROCEDURE ListarTratamientoMedicamento
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Tratamiento, ID_Medicamento, Dosis FROM Tratamiento_Medicamento;
END;
GO


CREATE PROCEDURE ActualizarTratamientoMedicamento
    @ID_Tratamiento INT,
    @ID_Medicamento INT,
    @Dosis VARCHAR(40)
AS
BEGIN
    UPDATE Tratamiento_Medicamento
    SET Dosis = @Dosis
    WHERE ID_Tratamiento = @ID_Tratamiento AND ID_Medicamento = @ID_Medicamento;
    SELECT @ID_Tratamiento AS ID_Tratamiento, @ID_Medicamento AS ID_Medicamento, @Dosis AS Dosis;
END;
GO


CREATE PROCEDURE EliminarTratamientoMedicamento
    @ID_Tratamiento INT,
    @ID_Medicamento INT
AS
BEGIN
    DELETE FROM Tratamiento_Medicamento WHERE ID_Tratamiento = @ID_Tratamiento AND ID_Medicamento = @ID_Medicamento;
END;
GO

-- =========================================
-- 22. Registro_Sanitario
-- =========================================
CREATE PROCEDURE InsertarRegistroSanitario
    @ID_RegistroSan INT,
    @ID_Animal INT,
    @Fecha_Registro DATE,
    @Descripcion VARCHAR(200)
AS
BEGIN
    INSERT INTO Registro_Sanitario (ID_RegistroSan, ID_Animal, Fecha_Registro, Descripcion)
    VALUES (@ID_RegistroSan, @ID_Animal, @Fecha_Registro, @Descripcion);
    SELECT @ID_RegistroSan AS ID_RegistroSan;
END;
GO


CREATE PROCEDURE ListarRegistroSanitario
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_RegistroSan, ID_Animal, Fecha_Registro, Descripcion FROM Registro_Sanitario;
END;
GO


CREATE PROCEDURE ActualizarRegistroSanitario
    @ID_RegistroSan INT,
    @ID_Animal INT,
    @Fecha_Registro DATE,
    @Descripcion VARCHAR(200)
AS
BEGIN
    UPDATE Registro_Sanitario
    SET ID_Animal = @ID_Animal,
        Fecha_Registro = @Fecha_Registro,
        Descripcion = @Descripcion
    WHERE ID_RegistroSan = @ID_RegistroSan;
    SELECT @ID_RegistroSan AS ID_RegistroSan;
END;
GO


CREATE PROCEDURE EliminarRegistroSanitario
    @ID_RegistroSan INT
AS
BEGIN
    DELETE FROM Registro_Sanitario WHERE ID_RegistroSan = @ID_RegistroSan;
END;
GO


-- =========================================
-- 23. Potrero
-- =========================================
CREATE PROCEDURE InsertarPotrero
    @ID_Potrero INT,
    @Nombre_Potrero VARCHAR(100),
    @Area DECIMAL(10,2)
AS
BEGIN
    INSERT INTO Potrero (ID_Potrero, Nombre_Potrero, Area)
    VALUES (@ID_Potrero, @Nombre_Potrero, @Area);
    SELECT @ID_Potrero AS ID_Potrero;
END;
GO


CREATE PROCEDURE ListarPotrero
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Potrero, Nombre_Potrero, Area FROM Potrero;
END;
GO

CREATE PROCEDURE ActualizarPotrero
    @ID_Potrero INT,
    @Nombre_Potrero VARCHAR(100),
    @Area DECIMAL(10,2)
AS
BEGIN
    UPDATE Potrero
    SET Nombre_Potrero = @Nombre_Potrero,
        Area = @Area
    WHERE ID_Potrero = @ID_Potrero;
    SELECT @ID_Potrero AS ID_Potrero;
END;
GO


CREATE PROCEDURE EliminarPotrero
    @ID_Potrero INT
AS
BEGIN
    DELETE FROM Potrero WHERE ID_Potrero = @ID_Potrero;
END;
GO

-- =========================================
-- 24. Tipo_Pasto
-- =========================================
CREATE PROCEDURE InsertarTipoPasto
    @ID_TipoPasto INT,
    @Nombre_Pasto VARCHAR(80)
AS
BEGIN
    INSERT INTO Tipo_Pasto (ID_TipoPasto, Nombre_Pasto)
    VALUES (@ID_TipoPasto, @Nombre_Pasto);
    SELECT @ID_TipoPasto AS ID_TipoPasto;
END;
GO

CREATE PROCEDURE ListarTipoPasto
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_TipoPasto, Nombre_Pasto FROM Tipo_Pasto;
END;
GO

CREATE PROCEDURE ActualizarTipoPasto
    @ID_TipoPasto INT,
    @Nombre_Pasto VARCHAR(80)
AS
BEGIN
    UPDATE Tipo_Pasto SET Nombre_Pasto = @Nombre_Pasto WHERE ID_TipoPasto = @ID_TipoPasto;
    SELECT @ID_TipoPasto AS ID_TipoPasto, @Nombre_Pasto AS Nombre_Pasto;
END;
GO

CREATE PROCEDURE EliminarTipoPasto
    @ID_TipoPasto INT
AS
BEGIN
    DELETE FROM Tipo_Pasto WHERE ID_TipoPasto = @ID_TipoPasto;
END;
GO


-- =========================================
-- 25. Animal_Potrero
-- =========================================
CREATE PROCEDURE InsertarAnimalPotrero
    @ID_Animal INT,
    @ID_Potrero INT,
    @Fecha_Entrada DATE,
    @Fecha_Salida DATE
AS
BEGIN
    INSERT INTO Animal_Potrero (ID_Animal, ID_Potrero, Fecha_Entrada, Fecha_Salida)
    VALUES (@ID_Animal, @ID_Potrero, @Fecha_Entrada, @Fecha_Salida);
    SELECT @ID_Animal AS ID_Animal, @ID_Potrero AS ID_Potrero;
END;
GO


CREATE PROCEDURE ListarAnimalPotrero
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Animal, ID_Potrero, Fecha_Entrada, Fecha_Salida FROM Animal_Potrero;
END;
GO


CREATE PROCEDURE ActualizarAnimalPotrero
    @ID_Animal INT,
    @ID_Potrero INT,
    @Fecha_Entrada DATE,
    @Fecha_Salida DATE
AS
BEGIN
    UPDATE Animal_Potrero
    SET Fecha_Salida = @Fecha_Salida
    WHERE ID_Animal = @ID_Animal AND ID_Potrero = @ID_Potrero AND Fecha_Entrada = @Fecha_Entrada;
    
    SELECT @ID_Animal AS ID_Animal, @ID_Potrero AS ID_Potrero;
END;
GO

CREATE PROCEDURE EliminarAnimalPotrero
    @ID_Animal INT,
    @ID_Potrero INT,
    @Fecha_Entrada DATE
AS
BEGIN
    DELETE FROM Animal_Potrero
    WHERE ID_Animal = @ID_Animal AND ID_Potrero = @ID_Potrero AND Fecha_Entrada = @Fecha_Entrada;
END;
GO


-- =========================================
-- 26. Muestreo_Suelo
-- =========================================
CREATE PROCEDURE InsertarMuestreoSuelo
    @ID_Muestreo INT,
    @ID_Potrero INT,
    @Fecha_Muestreo DATE,
    @pH DECIMAL(4,2),
    @Materia_Organica DECIMAL(5,2)
AS
BEGIN
    INSERT INTO Muestreo_Suelo (ID_Muestreo, ID_Potrero, Fecha_Muestreo, pH, Materia_Organica)
    VALUES (@ID_Muestreo, @ID_Potrero, @Fecha_Muestreo, @pH, @Materia_Organica);
    SELECT @ID_Muestreo AS ID_Muestreo;
END;
GO


CREATE PROCEDURE ListarMuestreoSuelo
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Muestreo, ID_Potrero, Fecha_Muestreo, pH, Materia_Organica FROM Muestreo_Suelo;
END;
GO


CREATE PROCEDURE EliminarMuestreoSuelo
    @ID_Muestreo INT
AS
BEGIN
    DELETE FROM Muestreo_Suelo WHERE ID_Muestreo = @ID_Muestreo;
END;
GO


-- =========================================
-- 27. Gestion_Agua
-- =========================================
CREATE PROCEDURE InsertarGestionAgua
    @ID_GestionAgua INT,
    @ID_Potrero INT,
    @Fecha_Revision DATE,
    @Estado_Bebedero VARCHAR(50)
AS
BEGIN
    INSERT INTO Gestion_Agua (ID_GestionAgua, ID_Potrero, Fecha_Revision, Estado_Bebedero)
    VALUES (@ID_GestionAgua, @ID_Potrero, @Fecha_Revision, @Estado_Bebedero);
    SELECT @ID_GestionAgua AS ID_GestionAgua;
END;
GO


CREATE PROCEDURE ListarGestionAgua
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_GestionAgua, ID_Potrero, Fecha_Revision, Estado_Bebedero FROM Gestion_Agua;
END;
GO


CREATE PROCEDURE ActualizarGestionAgua
    @ID_GestionAgua INT,
    @ID_Potrero INT,
    @Fecha_Revision DATE,
    @Estado_Bebedero VARCHAR(50)
AS
BEGIN
    UPDATE Gestion_Agua
    SET ID_Potrero = @ID_Potrero,
        Fecha_Revision = @Fecha_Revision,
        Estado_Bebedero = @Estado_Bebedero
    WHERE ID_GestionAgua = @ID_GestionAgua;
    SELECT @ID_GestionAgua AS ID_GestionAgua;
END;
GO

CREATE PROCEDURE EliminarGestionAgua
    @ID_GestionAgua INT
AS
BEGIN
    DELETE FROM Gestion_Agua WHERE ID_GestionAgua = @ID_GestionAgua;
END;
GO

-- =========================================
-- 28. Control_Ambiental
-- =========================================
CREATE PROCEDURE InsertarControlAmbiental
    @ID_ControlAmb INT,
    @ID_Potrero INT,
    @Fecha_Control DATE,
    @Observacion VARCHAR(200)
AS
BEGIN
    INSERT INTO Control_Ambiental (ID_ControlAmb, ID_Potrero, Fecha_Control, Observacion)
    VALUES (@ID_ControlAmb, @ID_Potrero, @Fecha_Control, @Observacion);
    SELECT @ID_ControlAmb AS ID_ControlAmb;
END;
GO


CREATE PROCEDURE ListarControlAmbiental
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_ControlAmb, ID_Potrero, Fecha_Control, Observacion FROM Control_Ambiental;
END;
GO


CREATE PROCEDURE ActualizarControlAmbiental
    @ID_ControlAmb INT,
    @ID_Potrero INT,
    @Fecha_Control DATE,
    @Observacion VARCHAR(200)
AS
BEGIN
    UPDATE Control_Ambiental
    SET ID_Potrero = @ID_Potrero,
        Fecha_Control = @Fecha_Control,
        Observacion = @Observacion
    WHERE ID_ControlAmb = @ID_ControlAmb;
    SELECT @ID_ControlAmb AS ID_ControlAmb;
END;
GO


CREATE PROCEDURE EliminarControlAmbiental
    @ID_ControlAmb INT
AS
BEGIN
    DELETE FROM Control_Ambiental WHERE ID_ControlAmb = @ID_ControlAmb;
END;
GO



-- =========================================
-- 29. Control_Lechero
-- =========================================
CREATE PROCEDURE InsertarControlLechero
    @ID_Control INT,
    @Fecha_Control DATE
AS
BEGIN
    INSERT INTO Control_Lechero (ID_Control, Fecha_Control)
    VALUES (@ID_Control, @Fecha_Control);
    SELECT @ID_Control AS ID_Control;
END;
GO


CREATE PROCEDURE ListarControlLechero
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Control, Fecha_Control FROM Control_Lechero;
END;
GO


CREATE PROCEDURE ActualizarControlLechero
    @ID_Control INT,
    @Fecha_Control DATE
AS
BEGIN
    UPDATE Control_Lechero
    SET Fecha_Control = @Fecha_Control
    WHERE ID_Control = @ID_Control;
    SELECT @ID_Control AS ID_Control, @Fecha_Control AS Fecha_Control;
END;
GO


CREATE PROCEDURE EliminarControlLechero
    @ID_Control INT
AS
BEGIN
    DELETE FROM Control_Lechero WHERE ID_Control = @ID_Control;
END;
GO


-- =========================================
-- 30. Periodo_Produccion
-- =========================================
CREATE PROCEDURE InsertarPeriodoProduccion
    @ID_Periodo INT,
    @Nombre_Periodo VARCHAR(60)
AS
BEGIN
    INSERT INTO Periodo_Produccion (ID_Periodo, Nombre_Periodo)
    VALUES (@ID_Periodo, @Nombre_Periodo);
    SELECT @ID_Periodo AS ID_Periodo;
END;
GO


CREATE PROCEDURE ListarPeriodoProduccion
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Periodo, Nombre_Periodo FROM Periodo_Produccion;
END;
GO


CREATE PROCEDURE ActualizarPeriodoProduccion
    @ID_Periodo INT,
    @Nombre_Periodo VARCHAR(60)
AS
BEGIN
    UPDATE Periodo_Produccion
    SET Nombre_Periodo = @Nombre_Periodo
    WHERE ID_Periodo = @ID_Periodo;
    SELECT @ID_Periodo AS ID_Periodo, @Nombre_Periodo AS Nombre_Periodo;
END;
GO


CREATE PROCEDURE EliminarPeriodoProduccion
    @ID_Periodo INT
AS
BEGIN
    DELETE FROM Periodo_Produccion WHERE ID_Periodo = @ID_Periodo;
END;
GO



-- =========================================
-- 31. Ordenador
-- =========================================
CREATE PROCEDURE InsertarOrdenador
    @ID_Ordenador INT,
    @Nombre_Ordenador VARCHAR(120)
AS
BEGIN
    INSERT INTO Ordenador (ID_Ordenador, Nombre_Ordenador)
    VALUES (@ID_Ordenador, @Nombre_Ordenador);
    SELECT @ID_Ordenador AS ID_Ordenador;
END;
GO


CREATE PROCEDURE ListarOrdenador
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Ordenador, Nombre_Ordenador FROM Ordenador;
END;
GO


CREATE PROCEDURE ActualizarOrdenador
    @ID_Ordenador INT,
    @Nombre_Ordenador VARCHAR(120)
AS
BEGIN
    UPDATE Ordenador
    SET Nombre_Ordenador = @Nombre_Ordenador
    WHERE ID_Ordenador = @ID_Ordenador;
    SELECT @ID_Ordenador AS ID_Ordenador, @Nombre_Ordenador AS Nombre_Ordenador;
END;
GO


CREATE PROCEDURE EliminarOrdenador
    @ID_Ordenador INT
AS
BEGIN
    DELETE FROM Ordenador WHERE ID_Ordenador = @ID_Ordenador;
END;
GO


-- =========================================
-- 32. Ordeno
-- =========================================
CREATE PROCEDURE InsertarOrdeno
    @ID_Ordeno INT,
    @ID_Ordenador INT,
    @Fecha_Ordeno DATE,
    @Turno VARCHAR(20)
AS
BEGIN
    INSERT INTO Ordeno (ID_Ordeno, ID_Ordenador, Fecha_Ordeno, Turno)
    VALUES (@ID_Ordeno, @ID_Ordenador, @Fecha_Ordeno, @Turno);
    SELECT @ID_Ordeno AS ID_Ordeno;
END;
GO


CREATE PROCEDURE ListarOrdeno
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Ordeno, ID_Ordenador, Fecha_Ordeno, Turno FROM Ordeno;
END;
GO


CREATE PROCEDURE ActualizarOrdeno
    @ID_Ordeno INT,
    @ID_Ordenador INT,
    @Fecha_Ordeno DATE,
    @Turno VARCHAR(20)
AS
BEGIN
    UPDATE Ordeno
    SET ID_Ordenador = @ID_Ordenador,
        Fecha_Ordeno = @Fecha_Ordeno,
        Turno = @Turno
    WHERE ID_Ordeno = @ID_Ordeno;
    SELECT @ID_Ordeno AS ID_Ordeno;
END;
GO


CREATE PROCEDURE EliminarOrdeno
    @ID_Ordeno INT
AS
BEGIN
    DELETE FROM Ordeno WHERE ID_Ordeno = @ID_Ordeno;
END;
GO



-- =========================================
-- 33. Animal_ControlLechero (M:N con valor)
-- =========================================
CREATE PROCEDURE InsertarAnimalControlLechero
    @ID_Animal INT,
    @ID_Control INT,
    @Litros_Leche DECIMAL(10,2)
AS
BEGIN
    INSERT INTO Animal_ControlLechero (ID_Animal, ID_Control, Litros_Leche)
    VALUES (@ID_Animal, @ID_Control, @Litros_Leche);
    SELECT @ID_Animal AS ID_Animal, @ID_Control AS ID_Control;
END;
GO


CREATE PROCEDURE ListarAnimalControlLechero
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Animal, ID_Control, Litros_Leche FROM Animal_ControlLechero;
END;
GO


CREATE PROCEDURE ActualizarAnimalControlLechero
    @ID_Animal INT,
    @ID_Control INT,
    @Litros_Leche DECIMAL(10,2)
AS
BEGIN
    UPDATE Animal_ControlLechero
    SET Litros_Leche = @Litros_Leche
    WHERE ID_Animal = @ID_Animal AND ID_Control = @ID_Control;
    SELECT @ID_Animal AS ID_Animal, @ID_Control AS ID_Control, @Litros_Leche AS Litros_Leche;
END;
GO


CREATE PROCEDURE EliminarAnimalControlLechero
    @ID_Animal INT,
    @ID_Control INT
AS
BEGIN
    DELETE FROM Animal_ControlLechero
    WHERE ID_Animal = @ID_Animal AND ID_Control = @ID_Control;
END;
GO

-- =========================================
-- 34. Empleado
-- =========================================
CREATE PROCEDURE InsertarEmpleado
    @ID_Empleado INT,
    @Nombre_Empleado VARCHAR(120),
    @Fecha_Contratacion DATE
AS
BEGIN
    INSERT INTO Empleado (ID_Empleado, Nombre_Empleado, Fecha_Contratacion)
    VALUES (@ID_Empleado, @Nombre_Empleado, @Fecha_Contratacion);
    SELECT @ID_Empleado AS ID_Empleado;
END;
GO


CREATE PROCEDURE ListarEmpleado
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Empleado, Nombre_Empleado, Fecha_Contratacion FROM Empleado;
END;
GO


CREATE PROCEDURE ActualizarEmpleado
    @ID_Empleado INT,
    @Nombre_Empleado VARCHAR(120),
    @Fecha_Contratacion DATE
AS
BEGIN
    UPDATE Empleado
    SET Nombre_Empleado = @Nombre_Empleado,
        Fecha_Contratacion = @Fecha_Contratacion
    WHERE ID_Empleado = @ID_Empleado;
    SELECT @ID_Empleado AS ID_Empleado;
END;
GO


CREATE PROCEDURE EliminarEmpleado
    @ID_Empleado INT
AS
BEGIN
    DELETE FROM Empleado WHERE ID_Empleado = @ID_Empleado;
END;
GO

-- =========================================
-- 35. Cargo
-- =========================================
CREATE PROCEDURE InsertarCargo
    @ID_Cargo INT,
    @Nombre_Cargo VARCHAR(80)
AS
BEGIN
    INSERT INTO Cargo (ID_Cargo, Nombre_Cargo)
    VALUES (@ID_Cargo, @Nombre_Cargo);
    SELECT @ID_Cargo AS ID_Cargo;
END;
GO


CREATE PROCEDURE ListarCargo
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Cargo, Nombre_Cargo FROM Cargo;
END;
GO


CREATE PROCEDURE ActualizarCargo
    @ID_Cargo INT,
    @Nombre_Cargo VARCHAR(80)
AS
BEGIN
    UPDATE Cargo
    SET Nombre_Cargo = @Nombre_Cargo
    WHERE ID_Cargo = @ID_Cargo;
    SELECT @ID_Cargo AS ID_Cargo, @Nombre_Cargo AS Nombre_Cargo;
END;
GO


CREATE PROCEDURE EliminarCargo
    @ID_Cargo INT
AS
BEGIN
    DELETE FROM Cargo WHERE ID_Cargo = @ID_Cargo;
END;
GO

-- =========================================
-- 36. Empleado_Cargo (M:N con fecha)
-- =========================================
CREATE PROCEDURE InsertarEmpleadoCargo
    @ID_Empleado INT,
    @ID_Cargo INT,
    @Fecha_Asignacion DATE
AS
BEGIN
    INSERT INTO Empleado_Cargo (ID_Empleado, ID_Cargo, Fecha_Asignacion)
    VALUES (@ID_Empleado, @ID_Cargo, @Fecha_Asignacion);
    SELECT @ID_Empleado AS ID_Empleado, @ID_Cargo AS ID_Cargo, @Fecha_Asignacion AS Fecha_Asignacion;
END;
GO


CREATE PROCEDURE ListarEmpleadoCargo
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Empleado, ID_Cargo, Fecha_Asignacion FROM Empleado_Cargo;
END;
GO


CREATE PROCEDURE EliminarEmpleadoCargo
    @ID_Empleado INT,
    @ID_Cargo INT,
    @Fecha_Asignacion DATE
AS
BEGIN
    DELETE FROM Empleado_Cargo
    WHERE ID_Empleado = @ID_Empleado
      AND ID_Cargo = @ID_Cargo
      AND Fecha_Asignacion = @Fecha_Asignacion;
END;
GO

-- =========================================
-- 37. Corral
-- =========================================
CREATE PROCEDURE InsertarCorral
    @ID_Corral INT,
    @Nombre_Corral VARCHAR(80),
    @Capacidad INT
AS
BEGIN
    INSERT INTO Corral (ID_Corral, Nombre_Corral, Capacidad)
    VALUES (@ID_Corral, @Nombre_Corral, @Capacidad);
    SELECT @ID_Corral AS ID_Corral;
END;
GO


CREATE PROCEDURE ListarCorral
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Corral, Nombre_Corral, Capacidad FROM Corral;
END;
GO


CREATE PROCEDURE ActualizarCorral
    @ID_Corral INT,
    @Nombre_Corral VARCHAR(80),
    @Capacidad INT
AS
BEGIN
    UPDATE Corral
    SET Nombre_Corral = @Nombre_Corral,
        Capacidad = @Capacidad
    WHERE ID_Corral = @ID_Corral;
    SELECT @ID_Corral AS ID_Corral;
END;
GO


CREATE PROCEDURE EliminarCorral
    @ID_Corral INT
AS
BEGIN
    DELETE FROM Corral WHERE ID_Corral = @ID_Corral;
END;
GO

-- =========================================
-- 38. Animal_Corral (M:N con fechas)
-- =========================================
CREATE PROCEDURE InsertarAnimalCorral
    @ID_Animal INT,
    @ID_Corral INT,
    @Fecha_Entrada DATE,
    @Fecha_Salida DATE
AS
BEGIN
    INSERT INTO Animal_Corral (ID_Animal, ID_Corral, Fecha_Entrada, Fecha_Salida)
    VALUES (@ID_Animal, @ID_Corral, @Fecha_Entrada, @Fecha_Salida);

    SELECT @ID_Animal AS ID_Animal, @ID_Corral AS ID_Corral, @Fecha_Entrada AS Fecha_Entrada;
END;
GO


CREATE PROCEDURE ListarAnimalCorral
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Animal, ID_Corral, Fecha_Entrada, Fecha_Salida
    FROM Animal_Corral;
END;
GO


CREATE PROCEDURE ActualizarAnimalCorral
    @ID_Animal INT,
    @ID_Corral INT,
    @Fecha_Entrada DATE,
    @Fecha_Salida DATE
AS
BEGIN
    UPDATE Animal_Corral
    SET Fecha_Salida = @Fecha_Salida
    WHERE ID_Animal = @ID_Animal
      AND ID_Corral = @ID_Corral
      AND Fecha_Entrada = @Fecha_Entrada;

    SELECT @ID_Animal AS ID_Animal, @ID_Corral AS ID_Corral, @Fecha_Entrada AS Fecha_Entrada, @Fecha_Salida AS Fecha_Salida;
END;
GO


CREATE PROCEDURE EliminarAnimalCorral
    @ID_Animal INT,
    @ID_Corral INT,
    @Fecha_Entrada DATE
AS
BEGIN
    DELETE FROM Animal_Corral
    WHERE ID_Animal = @ID_Animal
      AND ID_Corral = @ID_Corral
      AND Fecha_Entrada = @Fecha_Entrada;
END;
GO


-- =========================================
-- LOGIN: Rol
-- =========================================
CREATE PROCEDURE InsertarRol
    @ID_Rol INT,
    @Nombre_Rol VARCHAR(50)
AS
BEGIN
    INSERT INTO Rol (ID_Rol, Nombre_Rol)
    VALUES (@ID_Rol, @Nombre_Rol);

    SELECT @ID_Rol AS ID_Rol;
END;
GO


CREATE PROCEDURE ListarRol
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Rol, Nombre_Rol FROM Rol;
END;
GO


CREATE PROCEDURE ActualizarRol
    @ID_Rol INT,
    @Nombre_Rol VARCHAR(50)
AS
BEGIN
    UPDATE Rol
    SET Nombre_Rol = @Nombre_Rol
    WHERE ID_Rol = @ID_Rol;

    SELECT @ID_Rol AS ID_Rol, @Nombre_Rol AS Nombre_Rol;
END;
GO


CREATE PROCEDURE EliminarRol
    @ID_Rol INT
AS
BEGIN
    DELETE FROM Rol WHERE ID_Rol = @ID_Rol;
END;
GO


-- =========================================
-- LOGIN: Usuario
-- =========================================
CREATE PROCEDURE InsertarUsuario
    @ID_Usuario INT,
    @Nombre_Usuario VARCHAR(100),
    @Contrasena VARCHAR(255),
    @Correo VARCHAR(120),
    @ID_Rol INT
AS
BEGIN
    INSERT INTO Usuario (ID_Usuario, Nombre_Usuario, Contrasena, Correo, ID_Rol)
    VALUES (@ID_Usuario, @Nombre_Usuario, @Contrasena, @Correo, @ID_Rol);

    SELECT @ID_Usuario AS ID_Usuario;
END;
GO


CREATE PROCEDURE ListarUsuario
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID_Usuario, Nombre_Usuario, Contrasena, Correo, ID_Rol
    FROM Usuario;
END;
GO


CREATE PROCEDURE ActualizarUsuario
    @ID_Usuario INT,
    @Nombre_Usuario VARCHAR(100),
    @Contrasena VARCHAR(255),
    @Correo VARCHAR(120),
    @ID_Rol INT
AS
BEGIN
    UPDATE Usuario
    SET Nombre_Usuario = @Nombre_Usuario,
        Contrasena = @Contrasena,
        Correo = @Correo,
        ID_Rol = @ID_Rol
    WHERE ID_Usuario = @ID_Usuario;

    SELECT @ID_Usuario AS ID_Usuario;
END;
GO


CREATE PROCEDURE ActualizarContrasenaUsuario
    @ID_Usuario INT,
    @Contrasena VARCHAR(255)
AS
BEGIN
    UPDATE Usuario
    SET Contrasena = @Contrasena
    WHERE ID_Usuario = @ID_Usuario;

    SELECT @ID_Usuario AS ID_Usuario;
END;
GO


CREATE PROCEDURE EliminarUsuario
    @ID_Usuario INT
AS
BEGIN
    DELETE FROM Usuario WHERE ID_Usuario = @ID_Usuario;
END;
GO

-- =========================================
-- PROCEDIMIENTOS DE BITACORA 
-- =========================================


CREATE PROCEDURE InsertarBitacoraEvento
    @ID_Usuario INT,
    @Nombre_Usuario VARCHAR(100),
    @Tipo_Evento VARCHAR(50),
    @Entidad VARCHAR(100),
    @ID_Registro VARCHAR(200),
    @Valores_Antes VARCHAR(MAX),
    @Valores_Nuevos VARCHAR(MAX),
    @Mensaje VARCHAR(4000),
    @IP_Usuario VARCHAR(45)
AS
BEGIN
    INSERT INTO Bitacora_Evento (ID_Usuario, Nombre_Usuario, Tipo_Evento, Entidad, ID_Registro, Valores_Antes, Valores_Nuevos, Mensaje, IP_Usuario)
    VALUES (@ID_Usuario, @Nombre_Usuario, @Tipo_Evento, @Entidad, @ID_Registro, @Valores_Antes, @Valores_Nuevos, @Mensaje, @IP_Usuario);
    SELECT IDENT_CURRENT('Bitacora_Evento') AS ID_Bitacora;
END;
GO


CREATE PROCEDURE InsertarBitacoraError
    @ID_Usuario INT,
    @Nombre_Usuario VARCHAR(100),
    @Tipo_Error VARCHAR(200),
    @Mensaje VARCHAR(4000),
    @Detalle_Error VARCHAR(MAX),
    @Pagina VARCHAR(300),
    @IP_Usuario VARCHAR(45)
AS
BEGIN
    INSERT INTO Bitacora_Error (ID_Usuario, Nombre_Usuario, Tipo_Error, Mensaje, Detalle_Error, Pagina, IP_Usuario)
    VALUES (@ID_Usuario, @Nombre_Usuario, @Tipo_Error, @Mensaje, @Detalle_Error, @Pagina, @IP_Usuario);
    SELECT IDENT_CURRENT('Bitacora_Error') AS ID_Error;
END;
GO


CREATE PROCEDURE InsertarBitacoraRequest
    @Ruta VARCHAR(300),
    @Query_String VARCHAR(1000),
    @ID_Usuario INT,
    @Nombre_Usuario VARCHAR(100),
    @IP_Usuario VARCHAR(45)
AS
BEGIN
    INSERT INTO Bitacora_Request (Fecha_Inicio, Ruta, Query_String, ID_Usuario, Nombre_Usuario, IP_Usuario)
    VALUES (GETDATE(), @Ruta, @Query_String, @ID_Usuario, @Nombre_Usuario, @IP_Usuario);
    SELECT IDENT_CURRENT('Bitacora_Request') AS ID_Request;
END;
GO


CREATE PROCEDURE ActualizarBitacoraRequest
    @ID_Request INT,
    @Codigo_Estado INT
AS
BEGIN
    UPDATE Bitacora_Request
    SET Fecha_Fin = GETDATE(),
        Codigo_Estado = @Codigo_Estado,
        Tiempo_Ms = DATEDIFF(MILLISECOND, Fecha_Inicio, GETDATE())
    WHERE ID_Request = @ID_Request;
END;
GO


CREATE PROCEDURE ListarBitacoraEvento
AS
BEGIN
    SELECT TOP 100 * FROM Bitacora_Evento ORDER BY Fecha_Hora DESC;
END;
GO


CREATE PROCEDURE ListarBitacoraError
AS
BEGIN
    SELECT TOP 100 * FROM Bitacora_Error ORDER BY Fecha_Hora DESC;
END;
GO


CREATE PROCEDURE ListarBitacoraRequest
AS
BEGIN
    SELECT TOP 100 * FROM Bitacora_Request ORDER BY Fecha_Inicio DESC;
END;
GO


CREATE PROCEDURE LimpiarBitacoraAntigua
    @Dias_Retencion INT
AS
BEGIN
    DELETE FROM Bitacora_Evento WHERE Fecha_Hora < DATEADD(DAY, -@Dias_Retencion, GETDATE());
    DELETE FROM Bitacora_Error WHERE Fecha_Hora < DATEADD(DAY, -@Dias_Retencion, GETDATE());
    DELETE FROM Bitacora_Request WHERE Fecha_Inicio < DATEADD(DAY, -@Dias_Retencion, GETDATE());
END;
GO
