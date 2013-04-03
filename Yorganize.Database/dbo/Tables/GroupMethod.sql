CREATE TABLE [dbo].[GroupMethod] (
    [GroupMethodId]  UNIQUEIDENTIFIER NOT NULL,
    [GroupName]      NVARCHAR (64)    NULL,
    [GroupShortName] NVARCHAR (16)    NULL,
    PRIMARY KEY CLUSTERED ([GroupMethodId] ASC)
);

