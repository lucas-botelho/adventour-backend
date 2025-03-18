CREATE PROCEDURE AddDay
    @ItineraryId INT,
    @DayNumber INT,
    @InsertedId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Day (id_itinerary, day_number)
    VALUES (@ItineraryId, @DayNumber);

    SET @InsertedId = SCOPE_IDENTITY();
END;
