CREATE PROCEDURE GetCountryByCode
    @code VARCHAR(2)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT id, name, continent_name
    FROM dbo.Country
    WHERE code = @code;
END;
GO