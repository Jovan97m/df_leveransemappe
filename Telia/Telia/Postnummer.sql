CREATE TABLE [dbo].[Postnummer] (
    [ID]            INT           IDENTITY (1, 1)  NOT NULL PRIMARY KEY,
    [PostNr]        NVARCHAR (Max) NULL,
    [Poststed]      NVARCHAR (Max) NULL,
    [Kommunenummer] NVARCHAR (Max) NULL,
    [Kommunenavn]   NVARCHAR (Max) NULL,
    [Kategory]      CHAR (1)      NULL,
    
);

