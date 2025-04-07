USE Adventour;
GO

-- Cities (Linked to Countries with IDs starting from 1)
INSERT INTO City (id_country, name) VALUES (1, 'Lisbon');
INSERT INTO City (id_country, name) VALUES (1, 'Porto');
INSERT INTO City (id_country, name) VALUES (2, 'Madrid');
INSERT INTO City (id_country, name) VALUES (2, 'Barcelona');
INSERT INTO City (id_country, name) VALUES (3, 'Paris');
GO

-- Attractions (Linked to Cities)
INSERT INTO Attraction (id_city, name, average_rating, description, address_one, address_two)
VALUES (1, 'Belem Tower', 5, 'A historic tower on the riverbank.', 'Avenida Brasilia', 'Lisbon');

INSERT INTO Attraction (id_city, name, average_rating, description, address_one, address_two)
VALUES (2, 'Clerigos Tower', 4, 'Iconic Baroque tower with stunning views.', 'Rua dos Clérigos', 'Porto');

INSERT INTO Attraction (id_city, name, average_rating, description, address_one, address_two)
VALUES (3, 'Prado Museum', 5, 'Famous museum with European art.', 'Calle Ruiz de Alarcon', 'Madrid');

INSERT INTO Attraction (id_city, name, average_rating, description, address_one, address_two)
VALUES (4, 'Sagrada Familia', 5, 'Magnificent basilica designed by Gaudi.', 'Carrer de Mallorca', 'Barcelona');

INSERT INTO Attraction (id_city, name, average_rating, description, address_one, address_two)
VALUES (5, 'Eiffel Tower', 5, 'Famous iron tower with city views.', 'Champ de Mars', 'Paris');
GO

-- Attraction_Info_Type
INSERT INTO Attraction_Info_Type (type_title) VALUES (1);
INSERT INTO Attraction_Info_Type (type_title) VALUES (2);
INSERT INTO Attraction_Info_Type (type_title) VALUES (3);
GO

-- Attraction_Info
INSERT INTO Attraction_Info (id_attraction, id_attraction_info_type, title, description, duration_seconds)
VALUES (1, 1, 'History', 'A brief history of the Belem Tower.', 3600);

INSERT INTO Attraction_Info (id_attraction, id_attraction_info_type, title, description, duration_seconds)
VALUES (2, 2, 'Views', 'Best spots to see the city from the top.', 1800);

INSERT INTO Attraction_Info (id_attraction, id_attraction_info_type, title, description, duration_seconds)
VALUES (3, 3, 'Artworks', 'Famous paintings and sculptures.', 5400);
GO

-- Rating
INSERT INTO Rating (rating) VALUES (1);
INSERT INTO Rating (rating) VALUES (2);
INSERT INTO Rating (rating) VALUES (3);
INSERT INTO Rating (rating) VALUES (4);
INSERT INTO Rating (rating) VALUES (5);
GO

-- Reviews (Linked to Person and Attraction)
INSERT INTO Review (id_rating, id_attraction, id_user, comment)
VALUES (5, 1, '3032E381-D2EA-4C37-B369-08DD75BAC44C', 'Amazing experience, must visit!');

INSERT INTO Review (id_rating, id_attraction, id_user, comment)
VALUES (4, 2, '3032E381-D2EA-4C37-B369-08DD75BAC44C', 'Beautiful views from the top.');

INSERT INTO Review (id_rating, id_attraction, id_user, comment)
VALUES (5, 3, '3032E381-D2EA-4C37-B369-08DD75BAC44C', 'Incredible art collection.');
GO

-- Favorites (Linked to Person and Attraction)
INSERT INTO Favorites (id_attraction, id_user)
VALUES (1, '3032E381-D2EA-4C37-B369-08DD75BAC44C');

INSERT INTO Favorites (id_attraction, id_user)
VALUES (3, '3032E381-D2EA-4C37-B369-08DD75BAC44C');

INSERT INTO Favorites (id_attraction, id_user)
VALUES (5, '3032E381-D2EA-4C37-B369-08DD75BAC44C');
GO

-- Itinerary (Linked to Person)
INSERT INTO Itinerary (id_user, title, created_at)
VALUES ('3032E381-D2EA-4C37-B369-08DD75BAC44C', 'Lisbon & Porto Trip', GETDATE());

INSERT INTO Itinerary (id_user, title, created_at)
VALUES ('3032E381-D2EA-4C37-B369-08DD75BAC44C', 'Paris Adventure', GETDATE());
GO

-- Day (Linked to Itinerary)
INSERT INTO Day (id_itinerary, day_number) VALUES (1, 1);
INSERT INTO Day (id_itinerary, day_number) VALUES (1, 2);
INSERT INTO Day (id_itinerary, day_number) VALUES (2, 1);
GO

-- Timeslot (Linked to Attraction and Day)
INSERT INTO Timeslot (id_attraction, id_day, start_time, end_time)
VALUES (1, 1, '2025-04-08 09:00:00', '2025-04-08 11:00:00');

INSERT INTO Timeslot (id_attraction, id_day, start_time, end_time)
VALUES (2, 2, '2025-04-08 12:00:00', '2025-04-08 14:00:00');
GO

-- Attraction_Images (Linked to Attraction)
INSERT INTO Attraction_Images (id_attraction, is_main, picture_ref)
VALUES (1, 1, 'belem_tower.jpg');

INSERT INTO Attraction_Images (id_attraction, is_main, picture_ref)
VALUES (2, 1, 'clerigos_tower.jpg');

INSERT INTO Attraction_Images (id_attraction, is_main, picture_ref)
VALUES (5, 1, 'eiffel_tower.jpg');
GO

-- Review_Images (Linked to Review)
INSERT INTO Review_Images (id_review, is_main, picture_ref)
VALUES (1, 1, 'belem_tower_review.jpg');

INSERT INTO Review_Images (id_review, is_main, picture_ref)
VALUES (2, 0, 'clerigos_tower_review.jpg');

INSERT INTO Review_Images (id_review, is_main, picture_ref)
VALUES (3, 1, 'prado_museum_review.jpg');
GO
