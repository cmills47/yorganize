CREATE TABLE [dbo].[VideoSource] (
    [VideoSourceId] UNIQUEIDENTIFIER CONSTRAINT [DF_VideoSource_VideoSourceId] DEFAULT (newid()) NOT NULL,
    [VideoId]       UNIQUEIDENTIFIER NOT NULL,
    [Format]        VARCHAR (50)     NULL,
    [Url]           VARCHAR (256)    NULL,
    CONSTRAINT [PK_VideoSource] PRIMARY KEY CLUSTERED ([VideoSourceId] ASC),
    CONSTRAINT [FK_VideoSource_Video] FOREIGN KEY ([VideoId]) REFERENCES [dbo].[Video] ([VideoId])
);

