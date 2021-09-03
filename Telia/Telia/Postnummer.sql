CREATE TABLE [dbo].[Postnummer] (
    [ID]            INT           IDENTITY (1, 1)  NOT NULL PRIMARY KEY,
    [PostNr]        NVARCHAR (10) NULL,
    [Poststed]      NVARCHAR (10) NULL,
    [Kommunenummer] NVARCHAR (10) NULL,
    [Kommunenavn]   NVARCHAR (10) NULL,
    [Kategory]      CHAR (1)      NULL,
    
);

