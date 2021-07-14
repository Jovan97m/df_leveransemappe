CREATE TABLE [dbo].[Client]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
    [Orgnummer]       NVARCHAR (50) NOT NULL,
    [Password]      NVARCHAR (50) NOT NULL,
    [Id_admin] INT NULL,
    [FirmaNavn] NVARCHAR(100),
    [GateNavn] NVARCHAR(100),
    [HusNummer] INT,
    [HusBokStav] NVARCHAR(1),
    [PostNummer] INT,
    [Sted] NVARCHAR(100),
    [Epost] NVARCHAR(100),
    [KontaktNavn] NVARCHAR(100),
    [KontaktEpost] NVARCHAR(100),
    [KontaktTlfnr] NVARCHAR(100),
    [TekniskKontaktNavn] NVARCHAR(100),
    [TekniskKontaktEpost] NVARCHAR(100),
    [TekniskKontaktTlfnr] NVARCHAR(100),
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Admin.Id_Admin] FOREIGN KEY ([Id_admin])
        REFERENCES [dbo].[Admin] ([Id]) ON DELETE CASCADE
)
