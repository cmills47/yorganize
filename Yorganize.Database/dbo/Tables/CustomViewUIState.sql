CREATE TABLE [dbo].[CustomViewUIState] (
    [CustomViewId]        UNIQUEIDENTIFIER NULL,
    [FolderId]            UNIQUEIDENTIFIER NULL,
    [ProjectId]           UNIQUEIDENTIFIER NULL,
    [ContextId]           UNIQUEIDENTIFIER NULL,
    [IsSelected]          BIT              NULL,
    [IsRoot]              BIT              NULL,
    [IsExpanded]          UNIQUEIDENTIFIER NULL,
    [Pane]                VARCHAR (16)     NULL,
    [Type]                VARCHAR (16)     NULL,
    [CustomViewUIStateId] UNIQUEIDENTIFIER NOT NULL,
    [MemberId]            UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([CustomViewUIStateId] ASC),
    CHECK ([Pane]='NavigationPane' OR [Pane]='ActionPane' OR [Pane]=NULL),
    CHECK ([Type]='Context' OR [Type]='Project' OR [Type]='Folder' OR [Type]=NULL),
    FOREIGN KEY ([ContextId]) REFERENCES [dbo].[Context] ([ContextId]),
    FOREIGN KEY ([CustomViewId]) REFERENCES [dbo].[CustomView] ([CustomViewId]),
    FOREIGN KEY ([FolderId]) REFERENCES [dbo].[Folder] ([FolderId]),
    FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    CONSTRAINT [FK_CustomViewUIState_0] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId])
);

