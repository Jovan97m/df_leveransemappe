CREATE TABLE [dbo].[Nummer](
    [Telefonnummer] INT IDENTITY (1, 1) NOT NULL,
    [Abonnementstype] NVARCHAR (20),
    [Fornavn] NVARCHAR (50),
    [Etternavn] NVARCHAR (50),
    [Bedrift som skal faktureres] NVARCHAR (50),
    [c/o adresse for SIM levering] NVARCHAR (50),
    [Gateadresse SIM Skal sendes til] NVARCHAR (50),
    [Hus nummer] INT ,
    [Hus bokstav]  NVARCHAR (50),
    [post nr.] INT,
    [Post sted] NVARCHAR (50),
    [Epost for sporings informasjon] NVARCHAR (50),
    [Epost] NVARCHAR (50),
    [Kostnadsted] NVARCHAR (50),
    [Tilleggsinfo/ansatt ID] INT,
    [Ekstra talesim ] INT ,
    [Ekstra datasim] INT ,
    PRIMARY KEY CLUSTERED ([Telefonnummer] ASC),
    CONSTRAINT [FK_dbo.Nummer_dbo.Fakturaoppsett_NavnPåKostnadssted] FOREIGN KEY ([Kostnadsted])
        REFERENCES [dbo].[Fakturaoppsett] ([Kostnadssted]) ON DELETE CASCADE
)
