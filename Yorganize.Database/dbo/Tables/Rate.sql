CREATE TABLE [dbo].[Rate] (
    [Comment]     TEXT             NULL,
    [RateValue]   FLOAT (53)       NULL,
    [ChecklistId] UNIQUEIDENTIFIER NOT NULL,
    [MemberId]    UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([ChecklistId] ASC, [MemberId] ASC),
    FOREIGN KEY ([ChecklistId]) REFERENCES [dbo].[Checklist] ([ChecklistId]),
    CONSTRAINT [FK__Rate__MemberId__70DDC3D8] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId])
);



