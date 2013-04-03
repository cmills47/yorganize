CREATE TABLE [dbo].[ActionTag] (
    [ActionId] UNIQUEIDENTIFIER NOT NULL,
    [TagId]    UNIQUEIDENTIFIER NOT NULL,
    [MemberId] UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([ActionId] ASC, [TagId] ASC),
    FOREIGN KEY ([ActionId]) REFERENCES [dbo].[Action] ([ActionId]),
    FOREIGN KEY ([TagId]) REFERENCES [dbo].[Tag] ([TagId]),
    CONSTRAINT [FK_ActionTag_0] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId])
);

