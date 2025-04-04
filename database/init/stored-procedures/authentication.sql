USE Adventour;
GO
-- Check and create CheckUserExistsByEmail procedure if not exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'CheckUserExistsByEmail')
BEGIN
    EXEC('
        CREATE PROCEDURE CheckUserExistsByEmail
            @email NVARCHAR(100)
        AS
        BEGIN
            SET NOCOUNT ON;

            SELECT COUNT(*) AS UserCount
            FROM dbo.Person
            WHERE email = @email AND dbo.Person.verified = 1
        END;
    ');
END;
GO

-- Check and create CheckUserExistsById procedure if not exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'CheckUserExistsById')
BEGIN
    EXEC('
        CREATE PROCEDURE CheckUserExistsById
            @userId uniqueidentifier
        AS
        BEGIN
            SET NOCOUNT ON;

            SELECT COUNT(*) AS UserCount
            FROM dbo.Person
            WHERE id = @userId AND dbo.Person.verified = 1
        END;
    ');
END;
GO

-- Check and create CreateUser procedure if not exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'CreateUser')
BEGIN
    EXEC('
        CREATE PROCEDURE CreateUser
            @name NVARCHAR(200),
            @email NVARCHAR(200),
            @oAuthId NVARCHAR(255),
            @photoUrl NVARCHAR(255)
        AS
        BEGIN
            SET NOCOUNT ON;

            INSERT INTO dbo.Person (id, oauth_id, name, email, verified, photo_url)
            OUTPUT INSERTED.id
            VALUES (NEWID(), @oAuthId, @name, @email, 0, @photoUrl);
        END;
    ');
END;
GO

-- Check and create UpdateUserPublicData procedure if not exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'UpdateUserPublicData')
BEGIN
    EXEC('
        CREATE PROCEDURE UpdateUserPublicData
            @username NVARCHAR(25),
            @photoUrl NVARCHAR(255),
            @userId uniqueidentifier
        AS
        BEGIN
            UPDATE dbo.Person SET username = @username, photo_url = @photoUrl
            WHERE id = @userId
        END;
    ');
END;
GO

-- Check and create ConfirmEmail procedure if not exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ConfirmEmail')
BEGIN
    EXEC('
        CREATE PROCEDURE ConfirmEmail
            @userId uniqueidentifier
        AS
        BEGIN
            SET NOCOUNT ON;
            UPDATE dbo.Person SET verified = 1
            WHERE id = @userId
        END;
    ');
END;
GO

-- Check and create GetPersonByOAuthId procedure if not exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetPersonByOAuthId')
BEGIN
    EXEC('
        CREATE PROCEDURE GetPersonByOAuthId
            @oAuthId NVARCHAR(255)
        AS
        BEGIN
            SET NOCOUNT ON;

            SELECT 
                * 
            FROM dbo.Person 
            WHERE oauth_id = @OAuthId;
        END;
    ');
END;
GO
