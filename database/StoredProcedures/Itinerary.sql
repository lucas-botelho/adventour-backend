IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetItineraryById')
BEGIN
    EXEC('
CREATE PROCEDURE [dbo].[GetItineraryById]
    @itineraryId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        i.Id, 
        i.title,
        i.created_at,
        d.Id, 
        d.id_itinerary, 
        d.day_number
    FROM Itinerary i
    LEFT JOIN Day d ON i.Id = d.id_itinerary
    WHERE i.Id = @itineraryId;
END;')
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddDay')
BEGIN
    EXEC('
CREATE PROCEDURE AddDay
    @itineraryId INT,
    @dayNumber INT,
    @insertedId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Day (id_itinerary, day_number)
    VALUES (@itineraryId, @dayNumber);
    
    SET @insertedId = SCOPE_IDENTITY();
END;')
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteDayById')
BEGIN
    EXEC('
CREATE PROCEDURE DeleteDayById
    @dayId INT
AS
BEGIN
    DELETE FROM Day
    WHERE Id = @dayId;
END;')
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetDayById')
BEGIN
    EXEC('
CREATE PROCEDURE GetDayById
    @dayId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        id_itinerary,
        day_number
    FROM Day
    WHERE Id = @dayId;
END;')
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetNextDayNumber')
BEGIN
    EXEC('
CREATE PROCEDURE [dbo].[GetNextDayNumber]
    @itineraryId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        MAX(d.day_number) + 1 AS HighestDayNumber
    FROM Day d
    WHERE d.id_itinerary = @itineraryId;
END;')
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetAttractionById')
BEGIN
    EXEC('
CREATE PROCEDURE GetAttractionById
    @AttractionId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        id, 
        id_city, 
        name, 
        description, 
        address_one, 
        address_two
    FROM Attraction
    WHERE id = @AttractionId;
END;')
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddAttraction')
BEGIN
    EXEC('
CREATE PROCEDURE AddAttraction
    @cityId INT,
    @name NVARCHAR(255),
    @description NVARCHAR(MAX),
    @addressOne NVARCHAR(150),
    @addressTwo NVARCHAR(150),
    @insertedId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Attraction (id_city, name, description, address_one, address_two)
    VALUES (@CityId, @Name, @Description, @AddressOne, @AddressTwo);
    
    SET @insertedId = SCOPE_IDENTITY();
END;')
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetCityById')
BEGIN
    EXEC('
CREATE PROCEDURE GetCityById
    @cityId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        id_country,
        name
    FROM City
    WHERE Id = @cityId;
END;')
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteAttractionById')
BEGIN
    EXEC('
CREATE PROCEDURE DeleteAttractionById
    @attractionId INT
AS
BEGIN
    DELETE FROM Attraction
    WHERE Id = @attractionId;
END;')
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'UpdateAttraction')
BEGIN
    EXEC('
CREATE PROCEDURE UpdateAttraction
    @attractionId INT,
    @cityId INT,
    @name NVARCHAR(255),
    @description NVARCHAR(MAX),
    @addressOne NVARCHAR(255),
    @addressTwo NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Attraction
    SET 
        id_city = @cityId,
        name = @name,
        description = @description,
        address_one = @addressOne,
        address_two = @addressTwo
    WHERE Id = @attractionId;
END;')
END
Go

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetTimeSlotsByDayId')
BEGIN
    EXEC('
CREATE PROCEDURE [dbo].[GetTimeSlotsByDayId]
    @dayId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id,
        id_attraction,
        id_day,
        start_time,
        end_time
    FROM TimeSlot
    WHERE id_day = @dayId;
END;')
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddTimeSlot')
BEGIN
    EXEC('
CREATE PROCEDURE AddTimeSlot
    @attractionId INT,
    @dayId INT,
    @startTime DATETIME,
    @endTime DATETIME,
    @insertedId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO TimeSlot (id_attraction, id_day, start_time, end_time)
    VALUES (@attractionId, @dayId, @startTime, @endTime);
    
    SET @insertedId = SCOPE_IDENTITY();
END;')
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteTimeSlot')
BEGIN
    EXEC('
CREATE PROCEDURE DeleteTimeSlot
    @timeSlotId INT
AS
BEGIN
    DELETE FROM TimeSlot
    WHERE Id = @timeSlotId;
END;')
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetTimeSlotById')
BEGIN
    EXEC('
CREATE PROCEDURE [dbo].[GetTimeSlotById]
    @timeSlotId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id,
        id_attraction,
        id_day,
        start_time,
        end_time
    FROM TimeSlot
    WHERE Id = @timeSlotId;
END;')
END
GO


