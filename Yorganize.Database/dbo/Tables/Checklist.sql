CREATE TABLE [dbo].[Checklist] (
    [ChecklistId]             UNIQUEIDENTIFIER NOT NULL,
    [ChecklistName]           NVARCHAR (256)   NULL,
    [Status]                  VARCHAR (16)     NULL,
    [ChecklistPosition]       INT              NULL,
    [EstimatedCompletionTime] INT              NULL,
    [LastSavedTime]           DATETIME         NULL,
    [IsShared]                BIT              NULL,
    [MemberId]                UNIQUEIDENTIFIER NOT NULL,
    [EstimatedCompletionUnit] VARCHAR (16)     NULL,
    PRIMARY KEY CLUSTERED ([ChecklistId] ASC),
    CHECK ([EstimatedCompletionUnit]='Years' OR [EstimatedCompletionUnit]='Months' OR [EstimatedCompletionUnit]='Weeks' OR [EstimatedCompletionUnit]='Days' OR [EstimatedCompletionUnit]='Hours' OR [EstimatedCompletionUnit]='Minutes' OR [EstimatedCompletionUnit]=NULL),
    CHECK ([Status]='Completed' OR [Status]='OnHold' OR [Status]='Dropped' OR [Status]='Active' OR [Status]=NULL),
    CONSTRAINT [FK__Checklist__Membe__4E88ABD4] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId])
);



