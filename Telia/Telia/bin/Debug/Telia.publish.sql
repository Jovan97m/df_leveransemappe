/*
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
:setvar DefaultDataPath "C:\Data\"
:setvar DefaultLogPath "C:\Data\"

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
USE [master];


GO

IF (DB_ID(N'$(DatabaseName)') IS NOT NULL) 
BEGIN
    ALTER DATABASE [$(DatabaseName)]
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$(DatabaseName)];
END

GO
PRINT N'Creating $(DatabaseName)...'
GO
CREATE DATABASE [$(DatabaseName)]
    ON 
    PRIMARY(NAME = [$(DatabaseName)], FILENAME = N'$(DefaultDataPath)$(DefaultFilePrefix)_Primary.mdf')
    LOG ON (NAME = [$(DatabaseName)_log], FILENAME = N'$(DefaultLogPath)$(DefaultFilePrefix)_Primary.ldf') COLLATE SQL_Latin1_General_CP1_CI_AS
GO
USE [$(DatabaseName)];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                NUMERIC_ROUNDABORT OFF,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL,
                RECOVERY FULL,
                CURSOR_CLOSE_ON_COMMIT OFF,
                AUTO_CREATE_STATISTICS ON,
                AUTO_SHRINK OFF,
                AUTO_UPDATE_STATISTICS ON,
                RECURSIVE_TRIGGERS OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ALLOW_SNAPSHOT_ISOLATION OFF;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET READ_COMMITTED_SNAPSHOT OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_UPDATE_STATISTICS_ASYNC OFF,
                PAGE_VERIFY NONE,
                DATE_CORRELATION_OPTIMIZATION OFF,
                DISABLE_BROKER,
                PARAMETERIZATION SIMPLE,
                SUPPLEMENTAL_LOGGING OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET TRUSTWORTHY OFF,
        DB_CHAINING OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'The database settings cannot be modified. You must be a SysAdmin to apply these settings.';
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET HONOR_BROKER_PRIORITY OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'The database settings cannot be modified. You must be a SysAdmin to apply these settings.';
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET TARGET_RECOVERY_TIME = 0 SECONDS 
    WITH ROLLBACK IMMEDIATE;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET FILESTREAM(NON_TRANSACTED_ACCESS = OFF),
                CONTAINMENT = NONE 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_CREATE_STATISTICS ON(INCREMENTAL = OFF),
                MEMORY_OPTIMIZED_ELEVATE_TO_SNAPSHOT = OFF,
                DELAYED_DURABILITY = DISABLED 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET QUERY_STORE (QUERY_CAPTURE_MODE = ALL, DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_PLANS_PER_QUERY = 200, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 367), MAX_STORAGE_SIZE_MB = 100) 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET QUERY_STORE = OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
        ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
        ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
        ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
        ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
        ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
        ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
        ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET TEMPORAL_HISTORY_RETENTION ON 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF fulltextserviceproperty(N'IsFulltextInstalled') = 1
    EXECUTE sp_fulltext_database 'enable';


GO
PRINT N'Creating [dbo].[Postnummer]...';


GO
CREATE TABLE [dbo].[Postnummer] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [PostNr]        NVARCHAR (MAX) NULL,
    [Poststed]      NVARCHAR (MAX) NULL,
    [Kommunenummer] NVARCHAR (MAX) NULL,
    [Kommunenavn]   NVARCHAR (MAX) NULL,
    [Kategory]      CHAR (1)       NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[Type]...';


GO
CREATE TABLE [dbo].[Type] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (35) NULL,
    [Reference_code] NVARCHAR (15) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[ConnectionType]...';


GO
CREATE TABLE [dbo].[ConnectionType] (
    [Id]      INT IDENTITY (1, 1) NOT NULL,
    [Id_abom] INT NOT NULL,
    [Id_type] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Abonementype]...';


GO
CREATE TABLE [dbo].[Abonementype] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (25) NULL,
    [Num_type] CHAR (1)      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Client]...';


GO
CREATE TABLE [dbo].[Client] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [Orgnummer]           NVARCHAR (50) NOT NULL,
    [Password]            NVARCHAR (50) NOT NULL,
    [FirmaNavn]           NVARCHAR (50) NULL,
    [GateNavn]            NVARCHAR (50) NULL,
    [HusNummer]           INT           NULL,
    [HusBokStav]          NVARCHAR (20) NULL,
    [PostNummer]          INT           NULL,
    [Sted]                NVARCHAR (50) NULL,
    [Epost]               NVARCHAR (20) NULL,
    [KontaktNavn]         NVARCHAR (30) NULL,
    [KontaktEpost]        NVARCHAR (20) NULL,
    [KontaktTlfnr]        NVARCHAR (30) NULL,
    [TekniskKontaktNavn]  NVARCHAR (20) NULL,
    [TekniskKontaktEpost] NVARCHAR (30) NULL,
    [TekniskKontaktTlfnr] NVARCHAR (30) NULL,
    [Id_abonementype]     INT           NOT NULL,
    [Id_abonemetypeF]     INT           NOT NULL,
    [Id_abonementypeI]    INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Fakturaoppsett]...';


GO
CREATE TABLE [dbo].[Fakturaoppsett] (
    [NavnPaKostnadssted]       NVARCHAR (50)  NOT NULL,
    [Tileggsinfo kostnadssted] NVARCHAR (200) NULL,
    [Fakturaformat]            NVARCHAR (50)  NULL,
    [Fakturaadresse]           NVARCHAR (50)  NULL,
    [Husnr]                    INT            NULL,
    [Bokstav]                  NVARCHAR (200) NULL,
    [Postnummer]               INT            NULL,
    [Sted]                     NVARCHAR (50)  NULL,
    [Epost]                    NVARCHAR (50)  NULL,
    [Kostnadssted]             NVARCHAR (50)  NOT NULL,
    [Orgnummer]                NVARCHAR (50)  NULL,
    [Date]                     DATE           NULL,
    [Id_client]                INT            NULL,
    PRIMARY KEY CLUSTERED ([Kostnadssted] ASC)
);


GO
PRINT N'Creating [dbo].[Nummer]...';


GO
CREATE TABLE [dbo].[Nummer] (
    [Id]                              INT           IDENTITY (1, 1) NOT NULL,
    [Telefonnummer]                   NVARCHAR (20) NULL,
    [Abonnementstype]                 NVARCHAR (50) NULL,
    [Fornavn]                         NVARCHAR (50) NULL,
    [Etternavn]                       NVARCHAR (50) NULL,
    [Bedrift som skal faktureres]     NVARCHAR (50) NULL,
    [c/o adresse for SIM levering]    NVARCHAR (50) NULL,
    [Gateadresse SIM Skal sendes til] NVARCHAR (50) NULL,
    [Hus nummer]                      INT           NULL,
    [Hus bokstav]                     NVARCHAR (50) NULL,
    [post nr.]                        INT           NULL,
    [Post sted]                       NVARCHAR (50) NULL,
    [Epost for sporings informasjon]  NVARCHAR (50) NULL,
    [Epost]                           NVARCHAR (50) NULL,
    [Kostnadsted]                     NVARCHAR (50) NULL,
    [Tilleggsinfo/ansatt ID]          INT           NULL,
    [Ekstra talesim ]                 INT           NULL,
    [Ekstra datasim]                  INT           NULL,
    [Orgnummer]                       NVARCHAR (50) NULL,
    [Date]                            DATE          NULL,
    [Pending]                         BIT           NULL,
    [Katalogoppforing]                NVARCHAR (45) NULL,
    [Porteringsdatoog tid]            DATETIME      NULL,
    [Binding]                         BIT           NULL,
    [Postnummer]                      INT           NULL,
    [Antall TrillingSIM]              INT           NULL,
    [allDataSIM]                      INT           NULL,
    [Manuell Top-up]                  BIT           NULL,
    [Sperre Top-up]                   BIT           NULL,
    [Norden]                          BIT           NULL,
    [Tale og SMS til EU]              BIT           NULL,
    [TBN]                             NVARCHAR (15) NULL,
    [HovedSIM]                        INT           NULL,
    [TrillingSIM1]                    INT           NULL,
    [TrillingSIM2]                    INT           NULL,
    [DataSIM1]                        INT           NULL,
    [DataSIM2]                        INT           NULL,
    [DataSIM3]                        INT           NULL,
    [DataSIM4]                        INT           NULL,
    [DataSIM5]                        INT           NULL,
    [DeliveryMethodCode]              NVARCHAR (20) NULL,
    [DeliveryStreetName]              NVARCHAR (20) NULL,
    [DeliveryStreetNumber]            NVARCHAR (10) NULL,
    [DeliveryStreetSuffix]            NVARCHAR (10) NULL,
    [DeliveryCity]                    NVARCHAR (20) NULL,
    [DeliveryZIP]                     NVARCHAR (20) NULL,
    [DeliveryCountryCode]             NVARCHAR (20) NULL,
    [DeliveryContractEmail]           NVARCHAR (20) NULL,
    [DeliveryContractCountryCode]     NVARCHAR (20) NULL,
    [DeliveryContractLocalNumber]     NVARCHAR (20) NULL,
    [DeliveryIndividualFirstName]     NVARCHAR (15) NULL,
    [DeliveryIndividualLastName]      NVARCHAR (20) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Admin]...';


GO
CREATE TABLE [dbo].[Admin] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (50) NOT NULL,
    [Password] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating unnamed constraint on [dbo].[ConnectionType]...';


GO
ALTER TABLE [dbo].[ConnectionType]
    ADD FOREIGN KEY ([Id_abom]) REFERENCES [dbo].[Abonementype] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating unnamed constraint on [dbo].[ConnectionType]...';


GO
ALTER TABLE [dbo].[ConnectionType]
    ADD FOREIGN KEY ([Id_type]) REFERENCES [dbo].[Type] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating unnamed constraint on [dbo].[Client]...';


GO
ALTER TABLE [dbo].[Client]
    ADD FOREIGN KEY ([Id_abonementype]) REFERENCES [dbo].[Abonementype] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.Client_dbo.Client_Id]...';


GO
ALTER TABLE [dbo].[Fakturaoppsett]
    ADD CONSTRAINT [FK_dbo.Client_dbo.Client_Id] FOREIGN KEY ([Id_client]) REFERENCES [dbo].[Client] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.Nummer_dbo.Fakturaoppsett_NavnPåKostnadssted]...';


GO
ALTER TABLE [dbo].[Nummer]
    ADD CONSTRAINT [FK_dbo.Nummer_dbo.Fakturaoppsett_NavnPåKostnadssted] FOREIGN KEY ([Kostnadsted]) REFERENCES [dbo].[Fakturaoppsett] ([Kostnadssted]) ON DELETE CASCADE;


GO
-- Refactoring step to update target server with deployed transaction logs

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END
GO
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '3612fca1-4f17-4b39-b934-d88c654b39d1')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('3612fca1-4f17-4b39-b934-d88c654b39d1')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'c2f7a30c-baad-407f-8bd7-7b012d77f648')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('c2f7a30c-baad-407f-8bd7-7b012d77f648')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'b197c2cd-7dba-4677-b31a-6305baa47e1e')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('b197c2cd-7dba-4677-b31a-6305baa47e1e')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '75ddc9fd-fe11-4032-bb4a-b135747e2507')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('75ddc9fd-fe11-4032-bb4a-b135747e2507')

GO

GO
DECLARE @VarDecimalSupported AS BIT;

SELECT @VarDecimalSupported = 0;

IF ((ServerProperty(N'EngineEdition') = 3)
    AND (((@@microsoftversion / power(2, 24) = 9)
          AND (@@microsoftversion & 0xffff >= 3024))
         OR ((@@microsoftversion / power(2, 24) = 10)
             AND (@@microsoftversion & 0xffff >= 1600))))
    SELECT @VarDecimalSupported = 1;

IF (@VarDecimalSupported > 0)
    BEGIN
        EXECUTE sp_db_vardecimal_storage_format N'$(DatabaseName)', 'ON';
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET MULTI_USER 
    WITH ROLLBACK IMMEDIATE;


GO
PRINT N'Update complete.';


GO
