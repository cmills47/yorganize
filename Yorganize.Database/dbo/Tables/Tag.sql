CREATE TABLE [dbo].[Tag] (
    [TagId]    UNIQUEIDENTIFIER NOT NULL,
    [MemberId] UNIQUEIDENTIFIER NOT NULL,
    [Text]     NVARCHAR (256)   NULL,
    PRIMARY KEY CLUSTERED ([TagId] ASC),
    FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId])
);





