CREATE TABLE [dbo].[ProjectTag] (
    [ProjectId] UNIQUEIDENTIFIER NOT NULL,
    [TagId]     UNIQUEIDENTIFIER NOT NULL,
    [MemberId]  UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([ProjectId] ASC, [TagId] ASC),
    FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    FOREIGN KEY ([TagId]) REFERENCES [dbo].[Tag] ([TagId]),
    CONSTRAINT [FK_ProjectTag_0] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId])
);

