CREATE TABLE [dbo].[ChecklistContext] (
    [ChecklistId] UNIQUEIDENTIFIER NOT NULL,
    [ContextId]   UNIQUEIDENTIFIER NOT NULL,
    [MemberId]    UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([ChecklistId] ASC, [ContextId] ASC),
    FOREIGN KEY ([ChecklistId]) REFERENCES [dbo].[Checklist] ([ChecklistId]),
    FOREIGN KEY ([ContextId]) REFERENCES [dbo].[Context] ([ContextId]),
    CONSTRAINT [FK_ChecklistContext_0] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId])
);

