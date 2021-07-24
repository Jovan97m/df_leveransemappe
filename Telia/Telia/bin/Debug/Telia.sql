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
PRINT N'Altering [dbo].[Fakturaoppsett]...';


GO
ALTER TABLE [dbo].[Fakturaoppsett]
    ADD [Id_client] INT NULL;


GO
PRINT N'Creating [dbo].[FK_dbo.Client_dbo.Client_Id]...';


GO
ALTER TABLE [dbo].[Fakturaoppsett] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.Client_dbo.Client_Id] FOREIGN KEY ([Id_client]) REFERENCES [dbo].[Client] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[Fakturaoppsett] WITH CHECK CHECK CONSTRAINT [FK_dbo.Client_dbo.Client_Id];


GO
PRINT N'Update complete.';


GO
