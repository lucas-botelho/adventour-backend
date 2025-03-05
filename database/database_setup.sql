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
         id INT PRIMARY KEY IDENTITY(1,1),
        name NVARCHAR(50) NOT NULL,
        code NVARCHAR(2) NOT NULL,
        continent_name NVARCHAR(50) NOT NULL
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'City')
BEGIN
    CREATE TABLE City (
         id INT PRIMARY KEY IDENTITY(1,1),
        id_country INT NOT NULL,
        name NVARCHAR(255) NOT NULL,
        FOREIGN KEY (id_country) REFERENCES Country(id)
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Person')
BEGIN
	CREATE TABLE Person (
        id uniqueidentifier PRIMARY KEY,
		oauth_id NVARCHAR(255) NULL,
        name NVARCHAR(200) NOT NULL,
        username NVARCHAR(25) NULL,
        email NVARCHAR(200) NOT NULL,
		verified BIT DEFAULT 0,
		profile_picture_ref NVARCHAR(255) NULL 
	);
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Attraction')
BEGIN
    CREATE TABLE Attraction (
         id INT PRIMARY KEY IDENTITY(1,1),
        id_city INT NULL,
        name NVARCHAR(255) NOT NULL,
        average_rating INT NULL,
        description NVARCHAR(MAX),
        address_one NVARCHAR(150) NULL,
        address_two NVARCHAR(150) NULL,
        FOREIGN KEY (id_city) REFERENCES City(id)
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Attraction_Info_Type')
BEGIN
    CREATE TABLE Attraction_Info_Type (
         id INT PRIMARY KEY IDENTITY(1,1),
        type_title INT
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Attraction_Info')
BEGIN
    CREATE TABLE Attraction_Info (
         id INT PRIMARY KEY IDENTITY(1,1),
        id_attraction INT NOT NULL,
        id_attraction_info_type INT NOT NULL,
        title NVARCHAR(255),
        description NVARCHAR(MAX),
        duration_seconds INT NULL,
        FOREIGN KEY (id_attraction) REFERENCES Attraction(id) ON DELETE CASCADE,
        FOREIGN KEY (id_attraction_info_type) REFERENCES Attraction_Info_Type(id)
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Rating')
BEGIN
    CREATE TABLE Rating (
         id INT PRIMARY KEY IDENTITY(1,1),
        rating INT NOT NULL
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Review')
BEGIN
    CREATE TABLE Review (
         id INT PRIMARY KEY IDENTITY(1,1),
        id_rating INT NOT NULL,
        id_attraction INT NOT NULL,
        id_user uniqueidentifier NOT NULL,
        comment NVARCHAR(255) NULL,
        FOREIGN KEY (id_rating) REFERENCES Rating(id),
        FOREIGN KEY (id_attraction) REFERENCES Attraction(id) ON DELETE CASCADE,
        FOREIGN KEY (id_user) REFERENCES Person(id) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Favorites')
BEGIN
    CREATE TABLE Favorites (
         id INT PRIMARY KEY IDENTITY(1,1),
        id_attraction INT NOT NULL,
        id_user uniqueidentifier NOT NULL,
        FOREIGN KEY (id_attraction) REFERENCES Attraction(id) ON DELETE CASCADE,
        FOREIGN KEY (id_user) REFERENCES Person(id) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Itinerary')
BEGIN
    CREATE TABLE Itinerary (
         id INT PRIMARY KEY IDENTITY(1,1),
        id_user uniqueidentifier NOT NULL,
        title NVARCHAR(255) NOT NULL,
        created_at DATETIME NOT NULL,
        FOREIGN KEY (id_user) REFERENCES Person(id) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Day')
BEGIN
    CREATE TABLE Day (
         id INT PRIMARY KEY IDENTITY(1,1),
        id_itinerary INT NOT NULL,
        day_number INT NOT NULL,
        FOREIGN KEY (id_itinerary) REFERENCES Itinerary(id) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Timeslot')
BEGIN
    CREATE TABLE Timeslot (
         id INT PRIMARY KEY IDENTITY(1,1),
        id_attraction INT NOT NULL,
        id_day INT NOT NULL,
        start_time DATETIME NOT NULL,
        end_time DATETIME NOT NULL,
        FOREIGN KEY (id_attraction) REFERENCES Attraction(id) ON DELETE CASCADE,
        FOREIGN KEY (id_day) REFERENCES Day(id) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Attraction_Images')
BEGIN
    CREATE TABLE Attraction_Images (
         id INT PRIMARY KEY IDENTITY(1,1),
        id_attraction INT NOT NULL,
        is_main BIT DEFAULT 0,
        picture_ref NVARCHAR(255) NOT NULL,
        FOREIGN KEY (id_attraction) REFERENCES Attraction(id) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Review_Images')
BEGIN
    CREATE TABLE Review_Images (
         id INT PRIMARY KEY IDENTITY(1,1),
        id_review INT NOT NULL,
        is_main BIT DEFAULT 0,
        picture_ref NVARCHAR(255) NOT NULL,
        FOREIGN KEY (id_review) REFERENCES Review(id) ON DELETE CASCADE
    );
END;

GO
