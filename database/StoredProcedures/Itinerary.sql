ALTER PROCEDURE [dbo].[GetItineraryById]
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

END;

Go

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
END;

Go

CREATE PROCEDURE DeleteDayById
    @dayId INT
AS
BEGIN

    DELETE FROM Day
    WHERE Id = @dayId;
END;

Go

CREATE PROCEDURE GetDayById
    @dayId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        id_itinerary,
        day_number
    FROM 
        Day
    WHERE 
        Id = @dayId;
END

Go


