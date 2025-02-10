-- Create the database
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'Adventour')
BEGIN
    CREATE DATABASE Adventour;
END;
GO

-- Use the database
USE Adventour;
GO

-- Create the tables
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Country')
BEGIN
    CREATE TABLE Country (
        id_country INT PRIMARY KEY,
        name VARCHAR(50) NOT NULL
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'City')
BEGIN
    CREATE TABLE City (
        id_city INT PRIMARY KEY,
        id_country INT NOT NULL,
        name VARCHAR(255) NOT NULL,
        FOREIGN KEY (id_country) REFERENCES Country(id_country)
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Person')
BEGIN
    CREATE TABLE Person (
        id_user varchar(40) PRIMARY KEY,
        name VARCHAR(25) NOT NULL,
		username VARCHAR(25) NULL,
        email VARCHAR(100) NOT NULL,
        verified BIT NOT NULL,
        profile_picture_ref VARCHAR(255) NULL,
		password varchar(255) NULL,
    );
END;


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Attraction')
BEGIN
    CREATE TABLE Attraction (
        id_attraction INT PRIMARY KEY,
        id_city INT NOT NULL,
        name VARCHAR(255) NOT NULL,
        average_rating INT NULL,
        description VARCHAR(MAX),
        address_one VARCHAR(150) NULL,
		address_two VARCHAR(150) NULL,
        FOREIGN KEY (id_city) REFERENCES City(id_city)
    );
END;


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Attraction_Info_Type')
BEGIN
    CREATE TABLE Attraction_Info_Type (
        id_attraction_info_type INT PRIMARY KEY,
        type_title int
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Attraction_Info')
BEGIN
    CREATE TABLE Attraction_Info (
        id_attraction_info INT PRIMARY KEY,
        id_attraction INT NOT NULL,
        id_attraction_info_type INT NOT NULL,
        title VARCHAR(255),
        description VARCHAR(MAX),
        duration_seconds INT NULL,
        FOREIGN KEY (id_attraction) REFERENCES Attraction(id_attraction),
        FOREIGN KEY (id_attraction_info_type) REFERENCES Attraction_Info_Type(id_attraction_info_type)
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Rating')
BEGIN
    CREATE TABLE Rating (
        id_rating INT PRIMARY KEY,
        rating INT NOT NULL
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Review')
BEGIN
    CREATE TABLE Review (
        id_review INT PRIMARY KEY,
        id_rating INT NOT NULL,
        id_attraction INT NOT NULL,
        id_user varchar(40) NOT NULL,
        comment VARCHAR(255) NULL,
        FOREIGN KEY (id_rating) REFERENCES Rating(id_rating),
        FOREIGN KEY (id_attraction) REFERENCES Attraction(id_attraction),
        FOREIGN KEY (id_user) REFERENCES Person(id_user)
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Favorites')
BEGIN
    CREATE TABLE Favorites (
        id_favorite INT PRIMARY KEY,
        id_attraction INT NOT NULL,
        id_user varchar(40) NOT NULL,
        FOREIGN KEY (id_attraction) REFERENCES Attraction(id_attraction),
        FOREIGN KEY (id_user) REFERENCES Person(id_user)
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Itinerary')
BEGIN
    CREATE TABLE Itinerary (
        id_itinerary INT PRIMARY KEY,
        id_user varchar(40) NOT NULL,
        title VARCHAR(255) NOT NULL,
        created_at DATETIME NOT NULL,
        FOREIGN KEY (id_user) REFERENCES Person(id_user)
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Day')
BEGIN
    CREATE TABLE Day (
        id_day INT PRIMARY KEY,
        id_itinerary INT NOT NULL,
        day_number INT NOT NULL,
        FOREIGN KEY (id_itinerary) REFERENCES Itinerary(id_itinerary)
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Timeslot')
BEGIN
    CREATE TABLE Timeslot (
        id_time_slot INT PRIMARY KEY,
        id_attraction INT NOT NULL,
        id_day INT NOT NULL,
        start_time DATETIME NOT NULL,
        end_time DATETIME NOT NULL,
        FOREIGN KEY (id_attraction) REFERENCES Attraction(id_attraction),
        FOREIGN KEY (id_day) REFERENCES Day(id_day)
    );
END;

GO
