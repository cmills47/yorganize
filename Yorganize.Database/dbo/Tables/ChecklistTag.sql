CREATE TABLE [dbo].[ChecklistTag] (
    [ChecklistId] UNIQUEIDENTIFIER NOT NULL,
    [TagId]       UNIQUEIDENTIFIER NOT NULL,
    [MemberId]    UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([ChecklistId] ASC, [TagId] ASC),
    FOREIGN KEY ([ChecklistId]) REFERENCES [dbo].[Checklist] ([ChecklistId]),
    FOREIGN KEY ([TagId]) REFERENCES [dbo].[Tag] ([TagId]),
    CONSTRAINT [FK_ChecklistTag_0] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId])
);

