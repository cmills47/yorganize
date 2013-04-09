CREATE TABLE [dbo].[Video] (
    [VideoId]         UNIQUEIDENTIFIER CONSTRAINT [DF_Video_VideoId] DEFAULT (newid()) NOT NULL,
    [VideoCategoryId] INT              NOT NULL,
    [Title]           NVARCHAR (256)   NOT NULL,
    [Description]     NVARCHAR (MAX)   NULL,
    [Order]           SMALLINT         CONSTRAINT [DF_Video_Order] DEFAULT ((0)) NOT NULL,
    [SourceMP4Url]    VARCHAR (256)    NULL,
    [SourceOGGUrl]    VARCHAR (256)    NULL,
    [SourceWEBMUrl]   VARCHAR (256)    NULL,
    CONSTRAINT [PK_Video] PRIMARY KEY CLUSTERED ([VideoId] ASC),
    CONSTRAINT [FK_Video_VideoCategory] FOREIGN KEY ([VideoCategoryId]) REFERENCES [dbo].[VideoCategory] ([VideoCategoryId])
);



