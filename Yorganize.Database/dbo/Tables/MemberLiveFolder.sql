CREATE TABLE [dbo].[MemberLiveFolder] (
    [MemberId]           UNIQUEIDENTIFIER NULL,
    [FolderName]         NVARCHAR (64)    NULL,
    [FolderId]           NVARCHAR (64)    NULL,
    [FolderType]         NVARCHAR (16)    NULL,
    [MemberLiveFolderId] UNIQUEIDENTIFIER NOT NULL,
    [FolderUrl]          NVARCHAR (1024)  NULL,
    PRIMARY KEY CLUSTERED ([MemberLiveFolderId] ASC)
);

