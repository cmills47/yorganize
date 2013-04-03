CREATE TABLE [dbo].[ProjectContext] (
    [ProjectId] UNIQUEIDENTIFIER NOT NULL,
    [ContextId] UNIQUEIDENTIFIER NOT NULL,
    [MemberId]  UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([ProjectId] ASC, [ContextId] ASC),
    FOREIGN KEY ([ContextId]) REFERENCES [dbo].[Context] ([ContextId]),
    FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    CONSTRAINT [FK_ProjectContext_0] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId])
);

