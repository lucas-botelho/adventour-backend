CREATE PROCEDURE GetItineraryById
    @id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT id, id_user, title, createdAt
    FROM dbo.Itinerary
    WHERE id = @id;
END;
GO

CREATE PROCEDURE AddDay
    @itineraryId INT,
    @dayNumber INT,
    @insertedId INT OUTPUT
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

