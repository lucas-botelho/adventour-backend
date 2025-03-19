use adventour
go
CREATE PROCEDURE CheckUserExistsByEmail
    @email NVARCHAR(100)
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
GO


CREATE PROCEDURE UpdateUserPublicData
	@username NVARCHAR(25),
    @photoUrl NVARCHAR(255),
    @userId uniqueidentifier
AS
BEGIN
	UPDATE dbo.Person SET username = @username, photo_url = @photoUrl
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

CREATE PROCEDURE GetPersonByOAuthId
    @oAuthId NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        name, 
        email, 
        photo_url 
    FROM dbo.Person 
    WHERE oauth_id = @OAuthId;
END;


