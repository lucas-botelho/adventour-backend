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

    RETURN @@ROWCOUNT;
END;
