CREATE TABLE [dbo].[Rate] (
    [Comment]     TEXT             NULL,
    [RateValue]   FLOAT (53)       NULL,
    [ChecklistId] UNIQUEIDENTIFIER NOT NULL,
    [MemberId]    UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([ChecklistId] ASC, [MemberId] ASC),
    FOREIGN KEY ([ChecklistId]) REFERENCES [dbo].[Checklist] ([ChecklistId]),
    FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId])
);





