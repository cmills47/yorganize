CREATE TABLE [dbo].[Context] (
    [ContextId]       UNIQUEIDENTIFIER NOT NULL,
    [ContextName]     NVARCHAR (256)   NULL,
    [Status]          VARCHAR (16)     NULL,
    [ParentContextId] UNIQUEIDENTIFIER NULL,
    [ContextPosition] INT              NULL,
    [LastSavedTime]   DATETIME         NULL,
    [MemberId]        UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([ContextId] ASC),
    CHECK ([Status]='Deleted' OR [Status]='OnHold' OR [Status]='Dropped' OR [Status]='Active' OR [Status]=NULL),
    FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId]),
    FOREIGN KEY ([ParentContextId]) REFERENCES [dbo].[Context] ([ContextId])
);

