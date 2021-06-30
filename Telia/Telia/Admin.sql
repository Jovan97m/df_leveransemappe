CREATE TABLE [dbo].[Admin]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
    [UserName]       NVARCHAR (50) NOT NULL,
    [Password]      NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
