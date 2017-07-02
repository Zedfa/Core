Begin Transaction;
begin try
IF object_id(N'[core].[FK_core.Constants_core.ConstantCategories_ConstantCategoryID]', N'F') IS NOT NULL
    ALTER TABLE [core].[Constants] DROP CONSTRAINT [FK_core.Constants_core.ConstantCategories_ConstantCategoryID]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_ConstantCategoryID' AND object_id = object_id(N'[core].[Constants]', N'U'))
    DROP INDEX [IX_ConstantCategoryID] ON [core].[Constants]
DROP TABLE [core].[Constants]
DROP TABLE [core].[ConstantCategories]
DELETE [core].[__MigrationHistory]
WHERE (([MigrationId] = N'201508211346085_Core_Ver3') AND ([ContextKey] = N'Core.Ef.Migrations.Configuration'))
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'core.Logs')
AND col_name(parent_object_id, parent_column_id) = 'LogType';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [core].[Logs] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [core].[Logs] DROP COLUMN [LogType]
DELETE [core].[__MigrationHistory]
WHERE (([MigrationId] = N'201503081448361_Core_Ver2') AND ([ContextKey] = N'Core.Ef.Migrations.Configuration'))
IF object_id(N'[core].[FK_core.Logs_core.ExceptionLogs_ExceptionLog_ID]', N'F') IS NOT NULL
    ALTER TABLE [core].[Logs] DROP CONSTRAINT [FK_core.Logs_core.ExceptionLogs_ExceptionLog_ID]
IF object_id(N'[core].[FK_core.ExceptionLogs_core.ExceptionLogs_InnerException_ID]', N'F') IS NOT NULL
    ALTER TABLE [core].[ExceptionLogs] DROP CONSTRAINT [FK_core.ExceptionLogs_core.ExceptionLogs_InnerException_ID]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_ExceptionLog_ID' AND object_id = object_id(N'[core].[Logs]', N'U'))
    DROP INDEX [IX_ExceptionLog_ID] ON [core].[Logs]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_InnerException_ID' AND object_id = object_id(N'[core].[ExceptionLogs]', N'U'))
    DROP INDEX [IX_InnerException_ID] ON [core].[ExceptionLogs]
DROP TABLE [core].[Logs]
DROP TABLE [core].[ExceptionLogs]
DROP TABLE [core].[__MigrationHistory]
END TRY
BEGIN CATCH
    SELECT 
        ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0
    COMMIT TRANSACTION;
GO