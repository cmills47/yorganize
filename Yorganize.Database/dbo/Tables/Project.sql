CREATE TABLE [dbo].[Project] (
    [ProjectId]               UNIQUEIDENTIFIER CONSTRAINT [DF_Project_ProjectId] DEFAULT (newid()) NOT NULL,
    [ProjectName]             NVARCHAR (256)   NULL,
    [Status]                  VARCHAR (16)     NULL,
    [Type]                    VARCHAR (16)     NULL,
    [ProjectPosition]         INT              NULL,
    [EstimatedCompletionTime] INT              NULL,
    [StartDate]               DATETIME         NULL,
    [DueDate]                 DATETIME         NULL,
    [FlagId]                  UNIQUEIDENTIFIER NULL,
    [LastReviewedDatetime]    DATETIME         NULL,
    [LastSavedTime]           DATETIME         NULL,
    [MemberId]                UNIQUEIDENTIFIER NOT NULL,
    [FolderId]                UNIQUEIDENTIFIER NULL,
    [ReferenceDate]           DATETIME         NULL,
    [RepeatBehavior]          VARCHAR (16)     NULL,
    [RepeatInterval]          INT              NULL,
    [RepeatUnit]              VARCHAR (16)     NULL,
    [EstimatedCompletionUnit] VARCHAR (16)     NULL,
    [SelectedNoteId]          UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([ProjectId] ASC),
    CHECK ([EstimatedCompletionUnit]='Years' OR [EstimatedCompletionUnit]='Months' OR [EstimatedCompletionUnit]='Weeks' OR [EstimatedCompletionUnit]='Days' OR [EstimatedCompletionUnit]='Hours' OR [EstimatedCompletionUnit]='Minutes' OR [EstimatedCompletionUnit]=NULL),
    CHECK ([RepeatBehavior]='StartAndDueAfter' OR [RepeatBehavior]='DueAfter' OR [RepeatBehavior]='StartAfter' OR [RepeatBehavior]='RepeatEvery' OR [RepeatBehavior]=NULL),
    CHECK ([RepeatUnit]='Year' OR [RepeatUnit]='Month' OR [RepeatUnit]='Day' OR [RepeatUnit]=NULL),
    CHECK ([Status]='Completed' OR [Status]='OnHold' OR [Status]='Dropped' OR [Status]='Active' OR [Status]=NULL),
    CHECK ([Type]='Independent' OR [Type]='Ordered' OR [Type]='Parallel' OR [Type]=NULL),
    FOREIGN KEY ([FlagId]) REFERENCES [dbo].[Flag] ([FlagId]),
    FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId]),
    FOREIGN KEY ([SelectedNoteId]) REFERENCES [dbo].[Note] ([NoteId]),
    CONSTRAINT [FK__Project__FolderI__68487DD7] FOREIGN KEY ([FolderId]) REFERENCES [dbo].[Folder] ([FolderId]) ON DELETE CASCADE
);







