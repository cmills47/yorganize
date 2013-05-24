CREATE TABLE [dbo].[Action] (
    [ActionId]                UNIQUEIDENTIFIER CONSTRAINT [DF_Action_ActionId] DEFAULT (newid()) NOT NULL,
    [ActionName]              NVARCHAR (256)   NULL,
    [Status]                  VARCHAR (16)     NULL,
    [ActionPosition]          INT              NULL,
    [EstimatedCompletionTime] INT              NULL,
    [StartDate]               DATETIME         NULL,
    [DueDate]                 DATETIME         NULL,
    [LastSavedTime]           DATETIME         NULL,
    [FlagId]                  UNIQUEIDENTIFIER NULL,
    [ProjectId]               UNIQUEIDENTIFIER NULL,
    [MemberId]                UNIQUEIDENTIFIER NOT NULL,
    [ReferenceDate]           DATETIME         NULL,
    [RepeatBehavior]          VARCHAR (16)     NULL,
    [RepeatInterval]          INT              NULL,
    [RepeatUnit]              VARCHAR (16)     NULL,
    [EstimatedCompletionUnit] VARCHAR (16)     NULL,
    [Type]                    VARCHAR (16)     NULL,
    [ChecklistId]             UNIQUEIDENTIFIER NULL,
    [SelectedNoteId]          UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([ActionId] ASC),
    CHECK ([EstimatedCompletionUnit]='Years' OR [EstimatedCompletionUnit]='Months' OR [EstimatedCompletionUnit]='Weeks' OR [EstimatedCompletionUnit]='Days' OR [EstimatedCompletionUnit]='Hours' OR [EstimatedCompletionUnit]='Minutes' OR [EstimatedCompletionUnit]=NULL),
    CHECK ([RepeatBehavior]='StartAndDueAfter' OR [RepeatBehavior]='DueAfter' OR [RepeatBehavior]='StartAfter' OR [RepeatBehavior]='RepeatEvery' OR [RepeatBehavior]=NULL),
    CHECK ([RepeatUnit]='Year' OR [RepeatUnit]='Month' OR [RepeatUnit]='Day' OR [RepeatUnit]=NULL),
    CHECK ([Status]='Completed' OR [Status]='Active' OR [Status]=NULL),
    CHECK ([Type]='ChecklistItem' OR [Type]='InboxItem' OR [Type]='Default' OR [Type]=NULL),
    FOREIGN KEY ([FlagId]) REFERENCES [dbo].[Flag] ([FlagId]),
    FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId]),
    FOREIGN KEY ([SelectedNoteId]) REFERENCES [dbo].[Note] ([NoteId]),
    CONSTRAINT [FK__Action__ProjectI__1B9317B3] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Action_0] FOREIGN KEY ([ChecklistId]) REFERENCES [dbo].[Checklist] ([ChecklistId])
);











