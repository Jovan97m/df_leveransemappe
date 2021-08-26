CREATE TABLE [dbo].[Postnummer]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[PostNr] NVARCHAR(10),
	[Poststed] NVARCHAR(10),
	[Kommunenummer] NVARCHAR(10),
	[Kommunenavn] NVARCHAR(10),
	[Kategory] char ,
	[Land] NVARCHAR(10),

)
