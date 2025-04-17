USE Adventour;
GO

INSERT INTO Person (
    id,
    oauth_id,
    name,
    username,
    email,
    verified,
    photo_url
)
VALUES (
    '40EFFEF5-1449-404E-51F1-08DD7C4DACC0',
    'SEtuxdaKHsXjKRHR31T2cXVxp1h1',
    'Adventour',
    'botelho',
    'adventour.helpcenter@gmail.com',
    1,
    'https://res.cloudinary.com/dgskluspn/image/upload/v1744702589/33.jpg'
);


-- Attractions (Directly linked to Countries instead of Cities)
INSERT INTO Attraction (id_country, name, average_rating, short_description, address_one, address_two, long_description)
VALUES (180, 'Belem Tower', 5, 'A historic tower on the riverbank.', 'Avenida Brasilia', 'Lisbon', 'A Torre de Belém, antigamente Torre de São Vicente a Par de Belém, oficialmente Torre de São Vicente,[1] é uma fortificação localizada na freguesia de Belém, Município e Distrito de Lisboa, em Portugal. Na margem direita do rio Tejo, onde existiu outrora a praia de Belém, era primitivamente cercada pelas águas em todo o seu perímetro. Ao longo dos séculos foi envolvida pela praia, até se incorporar hoje à terra firme. Um dos ex libris da cidade, o monumento é um ícone da arquitetura do reinado de D. Manuel I, numa síntese entre a torre de menagem de tradição medieval e o baluarte moderno, onde se dispunham peças de artilharia.');

INSERT INTO Attraction (id_country, name, average_rating, short_description, address_one, address_two)
VALUES (180, 'Clerigos Tower', 4, 'Iconic Baroque tower with stunning views.', 'Rua dos Clérigos', 'Porto');

INSERT INTO Attraction (id_country, name, average_rating, short_description, address_one, address_two)
VALUES (180, 'Prado Museum', 5, 'Famous museum with European art.', 'Calle Ruiz de Alarcon', 'Madrid');

INSERT INTO Attraction (id_country, name, average_rating, short_description, address_one, address_two)
VALUES (180, 'Sagrada Familia', 5, 'Magnificent basilica designed by Gaudi.', 'Carrer de Mallorca', 'Barcelona');

INSERT INTO Attraction (id_country, name, average_rating, short_description, address_one, address_two)
VALUES (180, 'Eiffel Tower', 5, 'Famous iron tower with city views.', 'Champ de Mars', 'Paris');

-- Additional Attractions for Portugal
INSERT INTO Attraction (id_country, name, average_rating, short_description, address_one, address_two)
VALUES (180, 'Sintra Palace', 5, 'Beautiful palace surrounded by lush gardens.', 'Largo Rainha Dona Amélia', 'Sintra');

INSERT INTO Attraction (id_country, name, average_rating, short_description, address_one, address_two)
VALUES (180, 'Quinta da Regaleira', 5, 'Mystical palace with underground tunnels.', 'Rua Barbosa du Bocage', 'Sintra');

INSERT INTO Attraction (id_country, name, average_rating, short_description, address_one, address_two)
VALUES (180, 'Dom Luis I Bridge', 4, 'Famous double-deck iron bridge in Porto.', 'Ponte Luiz I', 'Porto');

GO

-- Attraction_Info
INSERT INTO Attraction_Info (id_attraction, id_attraction_info_type, title, description, duration_seconds)
VALUES (1, 1, 'History', 'A brief history of the Belem Tower.', 3600);

INSERT INTO Attraction_Info (id_attraction, id_attraction_info_type, title, description, duration_seconds)
VALUES (2, 2, 'Views', 'Best spots to see the city from the top.', 1800);

INSERT INTO Attraction_Info (id_attraction, id_attraction_info_type, title, description, duration_seconds)
VALUES (3, 3, 'Artworks', 'Famous paintings and sculptures.', 5400);
GO


-- Attraction_Images
INSERT INTO Attraction_Images (id_attraction, is_main, picture_ref)
VALUES (1, 1, 'https://res.cloudinary.com/dgskluspn/image/upload/v1739805925/1000000033.jpg');

INSERT INTO Attraction_Images (id_attraction, is_main, picture_ref)
VALUES (1, 0, 'https://res.cloudinary.com/dgskluspn/image/upload/v1744702589/33.jpg');

INSERT INTO Attraction_Images (id_attraction, is_main, picture_ref)
VALUES (1, 0, 'https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.self.com%2Fstory%2Fcoronavirus-travel-safety&psig=AOvVaw1-3NGGUpCu5xm-rPrP_LMm&ust=1744978854317000&source=images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCKCq7_eG34wDFQAAAAAdAAAAABAE');


INSERT INTO Attraction_Images (id_attraction, is_main, picture_ref)
VALUES (2, 1, 'https://res.cloudinary.com/dgskluspn/image/upload/v1739805925/1000000033.jpg');

INSERT INTO Attraction_Images (id_attraction, is_main, picture_ref)
VALUES (5, 1, 'https://res.cloudinary.com/dgskluspn/image/upload/v1739805925/1000000033.jpg');
GO

INSERT INTO Review (
    id_rating,
    id_attraction,
    id_user,
    comment
)
VALUES (
    3,
    1,
    '40EFFEF5-1449-404E-51F1-08DD7C4DACC0',
    'asdfasdfasdfasdfasdfasdf'
);

go
INSERT INTO Review_Images (
    id_review,
    is_main,
    picture_ref
)
VALUES
    (1, 0, 'https://res.cloudinary.com/dgskluspn/image/upload/v1739805925/1000000033.jpg'),
    (1, 0, 'https://res.cloudinary.com/dgskluspn/image/upload/v1739805925/1000000033.jpg');

GO
INSERT INTO Favorites (
id_attraction,
id_user
)
VALUES (
    1,
    '40EFFEF5-1449-404E-51F1-08DD7C4DACC0'
);


GO



