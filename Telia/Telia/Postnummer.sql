CREATE TABLE [dbo].[Postnummer] (
    [ID]            INT           IDENTITY (1, 1)  NOT NULL PRIMARY KEY,
    [PostNr]        NVARCHAR (MAX) NULL,
    [Poststed]      NVARCHAR (MAX) NULL,
    [Kommunenummer] NVARCHAR (MAX) NULL,
    [Kommunenavn]   NVARCHAR (MAX) NULL,
    [Kategory]      CHAR (1)      NULL,
    
);

