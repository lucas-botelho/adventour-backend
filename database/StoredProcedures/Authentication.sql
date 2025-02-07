CREATE PROCEDURE CheckUserExistsByEmail
    @Email VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS UserCount
    FROM dbo.Person
    WHERE email = @Email
END;
GO

CREATE PROCEDURE CreateUser
	@id_user varchar(40),
    @name varchar(25),
    @email varchar(100),
    @password varchar(255)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Person (id_user, name, email, verified, password)
    OUTPUT INSERTED.id_user
    VALUES (@id_user, @name, @email, 0, @password);
END;
GO
