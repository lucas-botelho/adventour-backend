USE [Adventour]
GO
INSERT [dbo].[Person] ([id], [oauth_id], [name], [username], [email], [verified], [photo_url]) VALUES (N'40effef5-1449-404e-51f1-08dd7c4dacc0', N'SEtuxdaKHsXjKRHR31T2cXVxp1h1', N'Adventour', N'botelho', N'adventour.helpcenter@gmail.com', 1, N'https://res.cloudinary.com/dgskluspn/image/upload/v1744702589/33.jpg')
GO
SET IDENTITY_INSERT [dbo].[Attraction] ON 
GO
INSERT [dbo].[Attraction] ([id], [id_country], [name], [average_rating], [short_description], [address_one], [address_two], [long_description], [duration_minutes]) VALUES (1, 180, N'Belem Tower', 5, N'A historic tower on the riverbank.', N'Avenida Brasilia', N'Lisbon', N'A Torre de Belém, antigamente Torre de São Vicente a Par de Belém, oficialmente Torre de São Vicente,[1] é uma fortificação localizada na freguesia de Belém, Município e Distrito de Lisboa, em Portugal. Na margem direita do rio Tejo, onde existiu outrora a praia de Belém, era primitivamente cercada pelas águas em todo o seu perímetro. Ao longo dos séculos foi envolvida pela praia, até se incorporar hoje à terra firme. Um dos ex libris da cidade, o monumento é um ícone da arquitetura do reinado de D. Manuel I, numa síntese entre a torre de menagem de tradição medieval e o baluarte moderno, onde se dispunham peças de artilharia.', 15)
GO
INSERT [dbo].[Attraction] ([id], [id_country], [name], [average_rating], [short_description], [address_one], [address_two], [long_description], [duration_minutes]) VALUES (2, 180, N'Clerigos Tower', 4, N'Iconic Baroque tower with stunning views.', N'Rua dos Clérigos', N'Porto', NULL, 15)
GO
INSERT [dbo].[Attraction] ([id], [id_country], [name], [average_rating], [short_description], [address_one], [address_two], [long_description], [duration_minutes]) VALUES (3, 180, N'Prado Museum', 5, N'Famous museum with European art.', N'Calle Ruiz de Alarcon', N'Madrid', NULL, 15)
GO
INSERT [dbo].[Attraction] ([id], [id_country], [name], [average_rating], [short_description], [address_one], [address_two], [long_description], [duration_minutes]) VALUES (4, 180, N'Sagrada Familia', 5, N'Magnificent basilica designed by Gaudi.', N'Carrer de Mallorca', N'Barcelona', NULL, 15)
GO
INSERT [dbo].[Attraction] ([id], [id_country], [name], [average_rating], [short_description], [address_one], [address_two], [long_description], [duration_minutes]) VALUES (5, 180, N'Eiffel Tower', 5, N'Famous iron tower with city views.', N'Champ de Mars', N'Paris', NULL, 15)
GO
INSERT [dbo].[Attraction] ([id], [id_country], [name], [average_rating], [short_description], [address_one], [address_two], [long_description], [duration_minutes]) VALUES (6, 180, N'Sintra Palace', 5, N'Beautiful palace surrounded by lush gardens.', N'Largo Rainha Dona Amélia', N'Sintra', NULL, 15)
GO
INSERT [dbo].[Attraction] ([id], [id_country], [name], [average_rating], [short_description], [address_one], [address_two], [long_description], [duration_minutes]) VALUES (7, 180, N'Quinta da Regaleira', 5, N'Mystical palace with underground tunnels.', N'Rua Barbosa du Bocage', N'Sintra', NULL, 15)
GO
INSERT [dbo].[Attraction] ([id], [id_country], [name], [average_rating], [short_description], [address_one], [address_two], [long_description], [duration_minutes]) VALUES (8, 180, N'Dom Luis I Bridge', 4, N'Famous double-deck iron bridge in Porto.', N'Ponte Luiz I', N'Porto', NULL, 15)
GO
SET IDENTITY_INSERT [dbo].[Attraction] OFF
GO
SET IDENTITY_INSERT [dbo].[Review] ON 
GO
INSERT [dbo].[Review] ([id], [id_rating], [id_attraction], [id_user], [comment], [title]) VALUES (10, 3, 1, N'40effef5-1449-404e-51f1-08dd7c4dacc0', N'asdfasdfasdfasdfasdfasdf', N'')
GO
INSERT [dbo].[Review] ([id], [id_rating], [id_attraction], [id_user], [comment], [title]) VALUES (11, 5, 1, N'40effef5-1449-404e-51f1-08dd7c4dacc0', N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam.', N'by Cicero in 45 BC')
GO
INSERT [dbo].[Review] ([id], [id_rating], [id_attraction], [id_user], [comment], [title]) VALUES (12, 2, 1, N'40effef5-1449-404e-51f1-08dd7c4dacc0', N'commentr255', N'test')
GO
INSERT [dbo].[Review] ([id], [id_rating], [id_attraction], [id_user], [comment], [title]) VALUES (13, 2, 1, N'40effef5-1449-404e-51f1-08dd7c4dacc0', N'commentr255', N'test')
GO
INSERT [dbo].[Review] ([id], [id_rating], [id_attraction], [id_user], [comment], [title]) VALUES (1011, 4, 1, N'40effef5-1449-404e-51f1-08dd7c4dacc0', N'review maluca', N'work flow completo')
GO
INSERT [dbo].[Review] ([id], [id_rating], [id_attraction], [id_user], [comment], [title]) VALUES (1012, 2, 1, N'40effef5-1449-404e-51f1-08dd7c4dacc0', N'ok mano maluco loko', N'teste se funciona')
GO
INSERT [dbo].[Review] ([id], [id_rating], [id_attraction], [id_user], [comment], [title]) VALUES (1013, 4, 1, N'40effef5-1449-404e-51f1-08dd7c4dacc0', N'b', N'a')
GO
INSERT [dbo].[Review] ([id], [id_rating], [id_attraction], [id_user], [comment], [title]) VALUES (1014, 1, 1, N'40effef5-1449-404e-51f1-08dd7c4dacc0', N'2', N'1')
GO
INSERT [dbo].[Review] ([id], [id_rating], [id_attraction], [id_user], [comment], [title]) VALUES (1015, 3, 1, N'40effef5-1449-404e-51f1-08dd7c4dacc0', N'4', N'3')
GO
INSERT [dbo].[Review] ([id], [id_rating], [id_attraction], [id_user], [comment], [title]) VALUES (1016, 3, 1, N'40effef5-1449-404e-51f1-08dd7c4dacc0', N'5', N'5')
GO
INSERT [dbo].[Review] ([id], [id_rating], [id_attraction], [id_user], [comment], [title]) VALUES (1017, 2, 1, N'40effef5-1449-404e-51f1-08dd7c4dacc0', N'1', N'1')
GO
SET IDENTITY_INSERT [dbo].[Review] OFF
GO
SET IDENTITY_INSERT [dbo].[Favorites] ON 
GO
INSERT [dbo].[Favorites] ([id], [id_attraction], [id_user]) VALUES (1, 1, N'40effef5-1449-404e-51f1-08dd7c4dacc0')
GO
INSERT [dbo].[Favorites] ([id], [id_attraction], [id_user]) VALUES (2, 1, N'40effef5-1449-404e-51f1-08dd7c4dacc0')
GO
SET IDENTITY_INSERT [dbo].[Favorites] OFF
GO
SET IDENTITY_INSERT [dbo].[Attraction_Info_Type] ON 
GO
SET IDENTITY_INSERT [dbo].[Attraction_Info_Type] OFF
GO
SET IDENTITY_INSERT [dbo].[Attraction_Info] ON 
GO
INSERT [dbo].[Attraction_Info] ([id], [id_attraction], [id_attraction_info_type], [title], [description]) VALUES (4, 1, 1, N'History', N'A Torre de Belém, antigamente Torre de São Vicente a Par de Belém, oficialmente Torre de São Vicente,[1] é uma fortificação localizada na freguesia de Belém, Município e Distrito de Lisboa, em Portugal. Na margem direita do rio Tejo, onde existiu outrora a praia de Belém, era primitivamente cercada pelas águas em todo o seu perímetro. Ao longo dos séculos foi envolvida pela praia, até se incorporar hoje à terra firme. Um dos ex libris da cidade, o monumento é um ícone da arquitetura do reinado de D. Manuel I, numa síntese entre a torre de menagem de tradição medieval e o baluarte moderno, onde se dispunham peças de artilharia.')
GO
INSERT [dbo].[Attraction_Info] ([id], [id_attraction], [id_attraction_info_type], [title], [description]) VALUES (5, 1, 2, N'Views', N'Best spots to see the city from the top.')
GO
INSERT [dbo].[Attraction_Info] ([id], [id_attraction], [id_attraction_info_type], [title], [description]) VALUES (6, 1, 3, N'Artworks', N'Famous paintings and sculptures.')
GO
SET IDENTITY_INSERT [dbo].[Attraction_Info] OFF
GO
SET IDENTITY_INSERT [dbo].[Attraction_Images] ON 
GO
INSERT [dbo].[Attraction_Images] ([id], [id_attraction], [is_main], [picture_ref]) VALUES (4, 1, 1, N'https://res.cloudinary.com/dgskluspn/image/upload/v1739805925/1000000033.jpg')
GO
INSERT [dbo].[Attraction_Images] ([id], [id_attraction], [is_main], [picture_ref]) VALUES (5, 2, 1, N'https://res.cloudinary.com/dgskluspn/image/upload/v1739805925/1000000033.jpg')
GO
INSERT [dbo].[Attraction_Images] ([id], [id_attraction], [is_main], [picture_ref]) VALUES (6, 5, 1, N'https://res.cloudinary.com/dgskluspn/image/upload/v1739805925/1000000033.jpg')
GO
INSERT [dbo].[Attraction_Images] ([id], [id_attraction], [is_main], [picture_ref]) VALUES (8, 1, 0, N'https://res.cloudinary.com/dgskluspn/image/upload/v1744702589/33.jpg')
GO
SET IDENTITY_INSERT [dbo].[Attraction_Images] OFF
GO
SET IDENTITY_INSERT [dbo].[Review_Images] ON 
GO
INSERT [dbo].[Review_Images] ([id], [id_review], [is_main], [picture_ref]) VALUES (5, 10, 0, N'https://res.cloudinary.com/dgskluspn/image/upload/v1739805925/1000000033.jpg')
GO
INSERT [dbo].[Review_Images] ([id], [id_review], [is_main], [picture_ref]) VALUES (6, 10, 0, N'https://res.cloudinary.com/dgskluspn/image/upload/v1739805925/1000000033.jpg')
GO
INSERT [dbo].[Review_Images] ([id], [id_review], [is_main], [picture_ref]) VALUES (7, 10, 0, N'https://res.cloudinary.com/dgskluspn/image/upload/v1739805925/1000000033.jpg')
GO
INSERT [dbo].[Review_Images] ([id], [id_review], [is_main], [picture_ref]) VALUES (8, 10, 0, N'https://res.cloudinary.com/dgskluspn/image/upload/v1739805925/1000000033.jpg')
GO
INSERT [dbo].[Review_Images] ([id], [id_review], [is_main], [picture_ref]) VALUES (9, 11, 0, N'https://res.cloudinary.com/dgskluspn/image/upload/v1744702589/33.jpg')
GO
INSERT [dbo].[Review_Images] ([id], [id_review], [is_main], [picture_ref]) VALUES (10, 11, 0, N'https://res.cloudinary.com/dgskluspn/image/upload/v1744702589/33.jpg')
GO
SET IDENTITY_INSERT [dbo].[Review_Images] OFF
GO
