CREATE TABLE [dbo].[Attachment] (
    [AttachmentId]       UNIQUEIDENTIFIER NOT NULL,
    [Type]               VARCHAR (16)     NULL,
    [FilePath]           VARCHAR (512)    NULL,
    [MemberId]           UNIQUEIDENTIFIER NOT NULL,
    [IsProject]          BIT              NULL,
    [ProjectId]          UNIQUEIDENTIFIER NULL,
    [ActionId]           UNIQUEIDENTIFIER NULL,
    [AttachmentPosition] INT              NULL,
    [Name]               NVARCHAR (128)   NULL,
    PRIMARY KEY CLUSTERED ([AttachmentId] ASC),
    CHECK ([Type]='Other' OR [Type]='Video' OR [Type]='Image' OR [Type]='Audio' OR [Type]=NULL),
    FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId]),
    CONSTRAINT [FK_Attachment_0] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([ProjectId]),
    CONSTRAINT [FK_Attachment_1] FOREIGN KEY ([ActionId]) REFERENCES [dbo].[Action] ([ActionId])
);







