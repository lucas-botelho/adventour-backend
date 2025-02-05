CREATE PROCEDURE CheckUserExistsByEmailAndUsername
    @Email VARCHAR(100),
    @Username VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS UserCount
    FROM dbo.Person
    WHERE email = @Email OR username = @Username;
END;