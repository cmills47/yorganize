CREATE TABLE [dbo].[Note] (
    [NoteId]       UNIQUEIDENTIFIER NOT NULL,
    [Type]         VARCHAR (16)     NULL,
    [Content]      NVARCHAR (512)   NULL,
    [MemberId]     UNIQUEIDENTIFIER NOT NULL,
    [IsProject]    BIT              NULL,
    [ProjectId]    UNIQUEIDENTIFIER NULL,
    [ActionId]     UNIQUEIDENTIFIER NULL,
    [NotePosition] INT              NULL,
    PRIMARY KEY CLUSTERED ([NoteId] ASC),
    CHECK ([Type]='Image' OR [Type]='Text' OR [Type]=NULL),
    FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId]),
    CONSTRAINT [FK_Note_0] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    CONSTRAINT [FK_Note_1] FOREIGN KEY ([ActionId]) REFERENCES [dbo].[Action] ([ActionId])
);

