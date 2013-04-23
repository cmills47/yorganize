CREATE TABLE [dbo].[Tag] (
    [TagId]    UNIQUEIDENTIFIER NOT NULL,
    [MemberId] UNIQUEIDENTIFIER NOT NULL,
    [Text]     NVARCHAR (256)   NULL,
    PRIMARY KEY CLUSTERED ([TagId] ASC),
    CONSTRAINT [FK__Tag__MemberId__71D1E811] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId])
);



