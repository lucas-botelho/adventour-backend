use adventour
go
CREATE PROCEDURE CheckUserExistsByEmail
    @email VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS UserCount
    FROM dbo.Person
    WHERE email = @email AND dbo.Person.verified = 1
END;
GO

CREATE PROCEDURE CheckUserExistsById
    @userId uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS UserCount
    FROM dbo.Person
    WHERE id = @userId AND dbo.Person.verified = 1
END;
GO

CREATE PROCEDURE CreateUser
    @name varchar(25),
    @email varchar(100),
    @password varchar(255)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Person (id, name, email, verified, password)
    OUTPUT INSERTED.id
    VALUES (NEWID(), @name, @email, 0, @password);
END;
GO


CREATE PROCEDURE UpdateUserPublicData
	@username varchar(25),
    @profilePictureRef varchar(255),
    @userId uniqueidentifier
AS
BEGIN
	UPDATE dbo.Person SET username = @username, profile_picture_ref = @profilePictureRef
	WHERE id = @userId
END;
GO

CREATE PROCEDURE ConfirmEmail
    @userId uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;
	UPDATE dbo.Person SET verified = 1
	WHERE id = @userId
END;
GO



