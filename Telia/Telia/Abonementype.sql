CREATE TABLE [dbo].[Abonementype]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(25),
	[Reference_code] NVARCHAR(15) NULL,
)
