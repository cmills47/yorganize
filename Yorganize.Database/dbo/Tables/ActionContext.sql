CREATE TABLE [dbo].[ActionContext] (
    [ActionId]  UNIQUEIDENTIFIER NOT NULL,
    [ContextId] UNIQUEIDENTIFIER NOT NULL,
    [MemberId]  UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([ActionId] ASC, [ContextId] ASC),
    FOREIGN KEY ([ActionId]) REFERENCES [dbo].[Action] ([ActionId]),
    FOREIGN KEY ([ContextId]) REFERENCES [dbo].[Context] ([ContextId]),
    CONSTRAINT [FK_ActionContext_0] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId])
);

