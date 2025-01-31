INSERT INTO dbo.Person (id_user, name, username, email, verified, profile_picture, password)
VALUES 
(1, 'John Doe', 'johnd', 'johndoe@example.com', 1, 'john_profile.jpg', 'securePass123'),
(2, 'Alice Smith', 'alices', 'alice@example.com', 0, 'alice_pic.png', 'mypassword456'),
(3, 'Bob Johnson', 'bobj', 'bobjohnson@example.com', 1, NULL, 'password789'),
(4, 'Emma Brown', 'emmab', NULL, 1, 'emma_profile.png', 'emmaPass321');
