SET IDENTITY_INSERT [dbo].[Accounts] ON
INSERT INTO [dbo].[Accounts] ([AccountID], [IsModerator], [Email], [Password], [Address], [PhoneNumber]) VALUES (1, 1, N'admin2@gmail.com', N'admin2', N'Elie Wiesel St 2, Rishon LeTsiyon	', N'050-9878746')
INSERT INTO [dbo].[Accounts] ([AccountID], [IsModerator], [Email], [Password], [Address], [PhoneNumber]) VALUES (3, 0, N'user1@gmail.com', N'user1', N'Ashdod', N'052-1234567')
INSERT INTO [dbo].[Accounts] ([AccountID], [IsModerator], [Email], [Password], [Address], [PhoneNumber]) VALUES (4, 1, N'admin1@gmail.com', N'admin1', N'Ehud Manor Street 1, Holon', N'052-2445089')
INSERT INTO [dbo].[Accounts] ([AccountID], [IsModerator], [Email], [Password], [Address], [PhoneNumber]) VALUES (6, 0, N'user2@gmail.com', N'user2', N'Ashdod', N'052-1234564')
INSERT INTO [dbo].[Accounts] ([AccountID], [IsModerator], [Email], [Password], [Address], [PhoneNumber]) VALUES (7, 0, N'user3@gmail.com', N'user3', N'Holon', N'053-1234567')
INSERT INTO [dbo].[Accounts] ([AccountID], [IsModerator], [Email], [Password], [Address], [PhoneNumber]) VALUES (8, 1, N'admin3@gmail.com', N'admin3', N'Tel Aviv', N'052-3456789')
INSERT INTO [dbo].[Accounts] ([AccountID], [IsModerator], [Email], [Password], [Address], [PhoneNumber]) VALUES (10, 0, N'user4@gmail.com', N'user4', N'Eilat', N'052-4567891')
INSERT INTO [dbo].[Accounts] ([AccountID], [IsModerator], [Email], [Password], [Address], [PhoneNumber]) VALUES (12, 0, N'user5@gmail.com', N'user5', N'Holon', N'054-8556233')
INSERT INTO [dbo].[Accounts] ([AccountID], [IsModerator], [Email], [Password], [Address], [PhoneNumber]) VALUES (47, 0, N'noalecohen1@gmail.com', N'123456', N'Holon', N'052-2445089')
SET IDENTITY_INSERT [dbo].[Accounts] OFF
