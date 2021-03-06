USE [Pyrros]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 03-04-2022 22:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[Direction] [varchar](10) NOT NULL,
	[Account] [int] NOT NULL,
	[CreatedTimestamp] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Wallet]    Script Date: 03-04-2022 22:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wallet](
	[WalletId] [int] IDENTITY(1,1) NOT NULL,
	[Account] [int] NOT NULL,
	[FirstName] [varchar](20) NOT NULL,
	[LastName] [varchar](20) NOT NULL,
	[Balance] [int] NOT NULL,
	[CreatedTimestamp] [datetime] NOT NULL,
	[ModifiedTimestamp] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[WalletId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Wallet] ON 

INSERT [dbo].[Wallet] ([WalletId], [Account], [FirstName], [LastName], [Balance], [CreatedTimestamp], [ModifiedTimestamp]) VALUES (1, 1001, N'Prince', N'Painadath', 2000, CAST(N'2022-04-03T06:20:28.277' AS DateTime), NULL)
INSERT [dbo].[Wallet] ([WalletId], [Account], [FirstName], [LastName], [Balance], [CreatedTimestamp], [ModifiedTimestamp]) VALUES (2, 1002, N'Bill', N'Gates', 5000, CAST(N'2022-04-03T06:55:20.180' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Wallet] OFF
/****** Object:  Index [UQ__Wallet__B0C3AC461C86B2B7]    Script Date: 03-04-2022 22:23:03 ******/
ALTER TABLE [dbo].[Wallet] ADD UNIQUE NONCLUSTERED 
(
	[Account] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [Fk_TransactionAccount] FOREIGN KEY([Account])
REFERENCES [dbo].[Wallet] ([Account])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [Fk_TransactionAccount]
GO
/****** Object:  StoredProcedure [dbo].[UspInsertTransaction]    Script Date: 03-04-2022 22:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[UspInsertTransaction]
	@id int,
	@amount int,
	@direction varchar(10),
	@account int
AS
BEGIN
	INSERT INTO [dbo].[Transaction] (
		[Id],
		[Amount],
		[Direction],
		[Account],
		[CreatedTimestamp])
	VALUES (
		@id,
		@amount,
		LOWER(@direction),
		@account,
		GETUTCDATE());
END
GO
/****** Object:  StoredProcedure [dbo].[UspSelectWallet]    Script Date: 03-04-2022 22:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[UspSelectWallet]
	@account int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[WalletId],
		[Account],
		[FirstName],
		[LastName],
		[Balance]
	FROM [dbo].[Wallet]
	WHERE 
		[Account] = @account;
END

GO
/****** Object:  Trigger [dbo].[TgrInsertTransaction]    Script Date: 03-04-2022 22:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TgrInsertTransaction]
	ON [dbo].[Transaction]
	AFTER INSERT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE
		@direction varchar(10),
		@amount int,
		@account int;

	SELECT 
		@direction = INSERTED.[Direction],
		@amount = INSERTED.[Amount],
		@account = INSERTED.[Account]
	FROM INSERTED;

	UPDATE A SET
		A.[Balance] = CASE WHEN @direction = 'Debit' THEN
							[Balance] - @amount
						ELSE
							[Balance] + @amount
						END,
		A.[ModifiedTimestamp] = GETUTCDATE()
	FROM [dbo].[Wallet] A
	WHERE
		A.[Account] = @account;
END

GO
ALTER TABLE [dbo].[Transaction] ENABLE TRIGGER [TgrInsertTransaction]
GO
