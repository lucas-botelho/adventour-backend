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