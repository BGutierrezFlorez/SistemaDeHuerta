-- ============================================
-- PROCEDIMIENTOS ALMACENADOS - AgroDB
-- ============================================

USE AgroDB;
GO

-- ============================================
-- 1. LISTAR TODOS LOS USUARIOS
-- ============================================
CREATE PROCEDURE ListarUsuario
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        idUsuario,
        nombre,
        tipoUsuario,
        cedula,
        correo,
        contraseña
    FROM Usuario
    ORDER BY nombre ASC;
END;
GO

-- ============================================
-- 2. REGISTRAR USUARIO
-- ============================================
CREATE PROCEDURE RegistrarUsuario
    @nombre         VARCHAR(100),
    @tipoUsuario    VARCHAR(50),
    @cedula         VARCHAR(20),
    @correo         VARCHAR(100),
    @contraseña     VARCHAR(200),
    @Resultado      INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Validar que no exista el correo
        IF EXISTS(SELECT 1 FROM Usuario WHERE correo = @correo)
        BEGIN
            SET @Resultado = -1;
            RAISERROR('El correo ya está registrado.', 16, 1);
            RETURN;
        END

        -- Validar que no exista la cédula
        IF EXISTS(SELECT 1 FROM Usuario WHERE cedula = @cedula)
        BEGIN
            SET @Resultado = -1;
            RAISERROR('La cédula ya está registrada.', 16, 1);
            RETURN;
        END

        -- Insertar nuevo usuario
        INSERT INTO Usuario (nombre, tipoUsuario, cedula, correo, contraseña)
        VALUES (@nombre, @tipoUsuario, @cedula, @correo, @contraseña);

        SET @Resultado = 1;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000);
        SET @ErrorMessage = ERROR_MESSAGE();
        SET @Resultado = -1;
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO

-- ============================================
-- 3. ACTUALIZAR USUARIO
-- ============================================
CREATE PROCEDURE ActualizarUsuario
    @idUsuario      INT,
    @nombre         VARCHAR(100),
    @tipoUsuario    VARCHAR(50),
    @cedula         VARCHAR(20),
    @correo         VARCHAR(100),
    @contraseña     VARCHAR(200),
    @Resultado      INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Verificar que el usuario existe
        IF NOT EXISTS(SELECT 1 FROM Usuario WHERE idUsuario = @idUsuario)
        BEGIN
            SET @Resultado = -1;
            RAISERROR('El usuario no existe.', 16, 1);
            RETURN;
        END

        -- Validar que el correo no esté asignado a otro usuario
        IF EXISTS(SELECT 1 FROM Usuario 
                  WHERE correo = @correo AND idUsuario <> @idUsuario)
        BEGIN
            SET @Resultado = -1;
            RAISERROR('El correo ya está registrado por otro usuario.', 16, 1);
            RETURN;
        END

        -- Validar que la cédula no esté asignada a otro usuario
        IF EXISTS(SELECT 1 FROM Usuario 
                  WHERE cedula = @cedula AND idUsuario <> @idUsuario)
        BEGIN
            SET @Resultado = -1;
            RAISERROR('La cédula ya está registrada por otro usuario.', 16, 1);
            RETURN;
        END

        -- Actualizar el usuario
        UPDATE Usuario
        SET nombre = @nombre,
            tipoUsuario = @tipoUsuario,
            cedula = @cedula,
            correo = @correo,
            contraseña = @contraseña
        WHERE idUsuario = @idUsuario;

        SET @Resultado = 1;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000);
        SET @ErrorMessage = ERROR_MESSAGE();
        SET @Resultado = -1;
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO

-- ============================================
-- 4. OBTENER USUARIO POR ID
-- ============================================
CREATE PROCEDURE ObtenerUsuarioID
    @idUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        idUsuario,
        nombre,
        tipoUsuario,
        cedula,
        correo,
        contraseña
    FROM Usuario
    WHERE idUsuario = @idUsuario;
END;
GO

-- ============================================
-- 5. OBTENER USUARIO POR CORREO
-- ============================================
CREATE PROCEDURE ObtenerUsuarioPorCorreo
    @correo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        idUsuario,
        nombre,
        tipoUsuario,
        cedula,
        correo,
        contraseña
    FROM Usuario
    WHERE correo = @correo;
END;
GO

-- ============================================
-- 6. ELIMINAR USUARIO
-- ============================================
CREATE PROCEDURE EliminarUsuario
    @idUsuario INT,
    @Resultado INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Verificar que el usuario existe
        IF NOT EXISTS(SELECT 1 FROM Usuario WHERE idUsuario = @idUsuario)
        BEGIN
            SET @Resultado = -1;
            RAISERROR('El usuario no existe.', 16, 1);
            RETURN;
        END

        -- Eliminar usuario
        DELETE FROM Usuario WHERE idUsuario = @idUsuario;
        
        SET @Resultado = 1;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000);
        SET @ErrorMessage = ERROR_MESSAGE();
        SET @Resultado = -1;
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO
