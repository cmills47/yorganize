CREATE TABLE [dbo].[Folder] (
    [FolderId]       UNIQUEIDENTIFIER NOT NULL,
    [FolderName]     NVARCHAR (256)   NULL,
    [FolderPosition] INT              NULL,
    [ParentFolderId] UNIQUEIDENTIFIER NULL,
    [MemberId]       UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([FolderId] ASC),
    FOREIGN KEY ([ParentFolderId]) REFERENCES [dbo].[Folder] ([FolderId]),
    CONSTRAINT [FK__Folder__MemberId__5FB337D6] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId])
);



