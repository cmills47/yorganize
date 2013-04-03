CREATE TABLE [dbo].[FilterMethod] (
    [FilterMethodId]  UNIQUEIDENTIFIER NOT NULL,
    [FilterName]      NVARCHAR (64)    NULL,
    [FilterShortName] NVARCHAR (16)    NULL,
    PRIMARY KEY CLUSTERED ([FilterMethodId] ASC)
);

