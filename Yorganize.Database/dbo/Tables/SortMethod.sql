CREATE TABLE [dbo].[SortMethod] (
    [SortMethodId]  UNIQUEIDENTIFIER NOT NULL,
    [SortName]      NVARCHAR (64)    NULL,
    [SortShortName] NVARCHAR (16)    NULL,
    PRIMARY KEY CLUSTERED ([SortMethodId] ASC)
);

