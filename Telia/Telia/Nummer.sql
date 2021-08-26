﻿CREATE TABLE [dbo].[Nummer](
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Telefonnummer] NVARCHAR (20) NULL,
    [Abonnementstype] NVARCHAR (50),
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
    [Orgnummer] NVARCHAR (50),
    [Date] DATE,
    [Pending] BIT,
    [Katalogoppforing] NVARCHAR(20) NULL, 
    [Porteringsdatoog tid] DATETIME NULL, 
    [Binding] BIT NULL, 
    [Postnummer] INT NULL, 
    [Antall TrillingSIM] INT NULL, 
    [allDataSIM] INT NULL, 
    [Manuell Top-up] BIT NULL, 
    [Sperre Top-up] BIT NULL, 
    [Norden] BIT NULL, 
    [Tale og SMS til EU] BIT NULL, 
    [TBN] NCHAR(15) NULL, 
    [HovedSIM] INT NULL, 
    [TrillingSIM1] INT NULL, 
    [TrillingSIM2] INT NULL, 
    [DataSIM1] INT NULL, 
    [DataSIM2] INT NULL, 
    [DataSIM3] INT NULL, 
    [DataSIM4] INT NULL, 
    [DataSIM5] INT NULL, 
    [DeliveryMethodCode] NVARCHAR(20) NULL, 
    [DeliveryStreetName] NVARCHAR(20) NULL, 
    [DeliveryStreetNumber] NVARCHAR(10) NULL, 
    [DeliveryStreetSuffix] NVARCHAR(10) NULL, 
    [DeliveryCity] NVARCHAR(20) NULL, 
    [DeliveryZIP] NVARCHAR(20) NULL, 
    [DeliveryCountryCode] NVARCHAR(20) NULL, 
    [DeliveryContractEmail] NVARCHAR(20) NULL, 
    [DeliveryContractCountryCode] NVARCHAR(20) NULL, 
    [DeliveryContractLocalNumber] NVARCHAR(20) NULL, 
    [DeliveryIndividualFirstName] NVARCHAR(15) NULL, 
    [DeliveryIndividualLastName] NVARCHAR(20) NULL,
    
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Nummer_dbo.Fakturaoppsett_NavnPåKostnadssted] FOREIGN KEY ([Kostnadsted])
        REFERENCES [dbo].[Fakturaoppsett] ([Kostnadssted]) ON DELETE CASCADE
        
)
