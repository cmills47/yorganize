CREATE TABLE [dbo].[CustomView] (
    [CustomViewId]  UNIQUEIDENTIFIER NOT NULL,
    [GroupMethodId] UNIQUEIDENTIFIER NULL,
    [GroupValue]    NVARCHAR (128)   NULL,
    [SortMethodId]  UNIQUEIDENTIFIER NULL,
    [SortValue]     NVARCHAR (128)   NULL,
    [IconUrl]       VARCHAR (256)    NULL,
    [MemberId]      UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([CustomViewId] ASC),
    CONSTRAINT [FK_CustomView_0] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId])
);

