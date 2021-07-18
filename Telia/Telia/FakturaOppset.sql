CREATE TABLE [dbo].[Fakturaoppsett](
    [NavnPaKostnadssted] NVARCHAR (50) NOT NULL,
    [Tileggsinfo kostnadssted] NVARCHAR(200),
    [Fakturaformat] NVARCHAR(50),
    [Fakturaadresse] NVARCHAR(50),
    [Husnr] INT,
    [Bokstav] NVARCHAR(200),
    [Postnummer] INT,
    [Sted] NVARCHAR(50),
    [Epost] NVARCHAR(50),
    [Kostnadssted] NVARCHAR(50) NOT NULL,
    [Orgnummer] NVARCHAR (50),
    [Date] DATE,
    PRIMARY KEY CLUSTERED ([Kostnadssted])
)
