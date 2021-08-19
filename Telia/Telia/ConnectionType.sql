CREATE TABLE [dbo].[ConnectionType]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Id_abom] int not null,
	[Id_type] int not null,
	FOREIGN KEY ([Id_abom]) REFERENCES [dbo].[Abonementype](Id) ON DELETE CASCADE,
	FOREIGN KEY ([Id_type]) REFERENCES [dbo].[Type](Id) ON DELETE CASCADE,
)
