CREATE TABLE [dbo].[Client]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
    [Orgnummer]       NVARCHAR (50) NOT NULL,
    [Password]      NVARCHAR (50) NOT NULL,
    [Id_admin] INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Admin.Id_Admin] FOREIGN KEY ([Id_admin])
        REFERENCES [dbo].[Admin] ([Id]) ON DELETE CASCADE
)
