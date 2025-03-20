CREATE PROCEDURE GetCountryByCode
    @code VARCHAR(2)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Country
    WHERE code = @code;
END;
GO