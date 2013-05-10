CREATE TABLE [dbo].[Folder] (
    [FolderId]       UNIQUEIDENTIFIER CONSTRAINT [DF_Folder_FolderId] DEFAULT (newid()) NOT NULL,
    [FolderName]     NVARCHAR (256)   NULL,
    [FolderPosition] INT              NULL,
    [ParentFolderId] UNIQUEIDENTIFIER NULL,
    [MemberId]       UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([FolderId] ASC),
    FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId]),
    FOREIGN KEY ([ParentFolderId]) REFERENCES [dbo].[Folder] ([FolderId])
);







