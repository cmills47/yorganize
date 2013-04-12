CREATE TABLE [dbo].[VideoCategory] (
    [VideoCategoryId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_VideoCategory] PRIMARY KEY CLUSTERED ([VideoCategoryId] ASC),
    CONSTRAINT [UK_VideoCategory] UNIQUE NONCLUSTERED ([Name] ASC)
);



