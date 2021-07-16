﻿/*
Deployment script for Telia

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "Telia"
:setvar DefaultFilePrefix "Telia"
:setvar DefaultDataPath "C:\Users\Marko Miloradovic\AppData\Local\Microsoft\VisualStudio\SSDT\Database\Telia"
:setvar DefaultLogPath "C:\Users\Marko Miloradovic\AppData\Local\Microsoft\VisualStudio\SSDT\Database\Telia"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Altering [dbo].[Nummer]...';


GO
ALTER TABLE [dbo].[Nummer]
    ADD [Katalogoppforing]            NVARCHAR (20) NULL,
        [Porteringsdatoog tid]        DATETIME      NULL,
        [Binding]                     NVARCHAR (20) NULL,
        [Postnummer]                  INT           NULL,
        [Antall TrillingSIM]          INT           NULL,
        [allDataSIM]                  INT           NULL,
        [Manuell Top-up]              NVARCHAR (15) NULL,
        [Sperre Top-up]               NVARCHAR (15) NULL,
        [Norden]                      NVARCHAR (20) NULL,
        [Tale og SMS til EU]          BIT           NULL,
        [TBN]                         NCHAR (15)    NULL,
        [HovedSIM]                    INT           NULL,
        [TrillingSIM1]                INT           NULL,
        [TrillingSIM2]                INT           NULL,
        [DataSIM1]                    INT           NULL,
        [DataSIM2]                    INT           NULL,
        [DataSIM3]                    INT           NULL,
        [DataSIM4]                    INT           NULL,
        [DataSIM5]                    INT           NULL,
        [DeliveryMethodCode]          NVARCHAR (20) NULL,
        [DeliveryStreetName]          NVARCHAR (20) NULL,
        [DeliveryStreetNumber]        NVARCHAR (10) NULL,
        [DeliveryStreetSuffix]        NVARCHAR (10) NULL,
        [DeliveryCity]                NVARCHAR (20) NULL,
        [DeliveryZIP]                 NVARCHAR (20) NULL,
        [DeliveryCountryCode]         NVARCHAR (20) NULL,
        [DeliveryContractEmail]       NVARCHAR (20) NULL,
        [DeliveryContractCountryCode] NVARCHAR (20) NULL,
        [DeliveryContractLocalNumber] NVARCHAR (20) NULL,
        [DeliveryIndividualFirstName] NVARCHAR (15) NULL,
        [DeliveryIndividualLastName]  NVARCHAR (20) NULL;


GO
PRINT N'Update complete.';


GO
