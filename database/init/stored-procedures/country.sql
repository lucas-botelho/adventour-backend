USE Adventour;
GO
-- Check and create GetCountryByCode procedure if not exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetCountryByCode')
BEGIN
    EXEC('
        CREATE PROCEDURE GetCountryByCode
            @code VARCHAR(2)
        AS
        BEGIN
            SET NOCOUNT ON;

            SELECT *
            FROM dbo.Country
            WHERE code = @code;
        END;
    ');
END;
GO

-- Check and create GetCountriesByContinent procedure if not exists
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetCountriesByContinent')
BEGIN
    EXEC('
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
    ');
END;
GO
