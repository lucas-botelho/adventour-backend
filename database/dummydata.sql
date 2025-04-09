USE Adventour;
GO

-- Attractions (Directly linked to Countries instead of Cities)
INSERT INTO Attraction (id_country, name, average_rating, description, address_one, address_two)
VALUES (180, 'Belem Tower', 5, 'A historic tower on the riverbank.', 'Avenida Brasilia', 'Lisbon');

INSERT INTO Attraction (id_country, name, average_rating, description, address_one, address_two)
VALUES (180, 'Clerigos Tower', 4, 'Iconic Baroque tower with stunning views.', 'Rua dos Clérigos', 'Porto');

INSERT INTO Attraction (id_country, name, average_rating, description, address_one, address_two)
VALUES (180, 'Prado Museum', 5, 'Famous museum with European art.', 'Calle Ruiz de Alarcon', 'Madrid');

INSERT INTO Attraction (id_country, name, average_rating, description, address_one, address_two)
VALUES (180, 'Sagrada Familia', 5, 'Magnificent basilica designed by Gaudi.', 'Carrer de Mallorca', 'Barcelona');

INSERT INTO Attraction (id_country, name, average_rating, description, address_one, address_two)
VALUES (180, 'Eiffel Tower', 5, 'Famous iron tower with city views.', 'Champ de Mars', 'Paris');

-- Additional Attractions for Portugal
INSERT INTO Attraction (id_country, name, average_rating, description, address_one, address_two)
VALUES (180, 'Sintra Palace', 5, 'Beautiful palace surrounded by lush gardens.', 'Largo Rainha Dona Amélia', 'Sintra');

INSERT INTO Attraction (id_country, name, average_rating, description, address_one, address_two)
VALUES (180, 'Quinta da Regaleira', 5, 'Mystical palace with underground tunnels.', 'Rua Barbosa du Bocage', 'Sintra');

INSERT INTO Attraction (id_country, name, average_rating, description, address_one, address_two)
VALUES (180, 'Dom Luis I Bridge', 4, 'Famous double-deck iron bridge in Porto.', 'Ponte Luiz I', 'Porto');

GO

-- Attraction_Info_Type
INSERT INTO Attraction_Info_Type (type_title) VALUES ('History');
INSERT INTO Attraction_Info_Type (type_title) VALUES ('Views');
INSERT INTO Attraction_Info_Type (type_title) VALUES ('Artworks');
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

-- Attraction_Images
INSERT INTO Attraction_Images (id_attraction, is_main, picture_ref)
VALUES (1, 1, 'https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_92x30dp.png');

INSERT INTO Attraction_Images (id_attraction, is_main, picture_ref)
VALUES (2, 1, 'https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_92x30dp.png');

INSERT INTO Attraction_Images (id_attraction, is_main, picture_ref)
VALUES (5, 1, 'https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_92x30dp.png');
GO

---- Review_Images
--INSERT INTO Review_Images (id_review, is_main, picture_ref)
--VALUES (1, 1, 'https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_92x30dp.png');

--INSERT INTO Review_Images (id_review, is_main, picture_ref)
--VALUES (2, 0, 'https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_92x30dp.png');

--INSERT INTO Review_Images (id_review, is_main, picture_ref)
--VALUES (3, 1, 'https://www.google.com/images/branding/googlelogo/2x/googlelogo_color_92x30dp.png');
GO
