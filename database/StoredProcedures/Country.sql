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

CREATE PROCEDURE GetCountriesByContinent
    @continentName NVARCHAR(100),
    @pageNumber INT,
    @fetchRows INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @offsetRows INT = (@PageNumber - 1) * @FetchRows;

    SELECT * 
    FROM Country 
    WHERE continent_name LIKE @continentName
    ORDER BY id
    OFFSET @offsetRows ROWS
    FETCH NEXT @fetchRows ROWS ONLY;
END;