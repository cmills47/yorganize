CREATE TABLE [dbo].[BlogPost] (
    [BlogPostId]   UNIQUEIDENTIFIER CONSTRAINT [DF_BlogPost_BlogPostId] DEFAULT (newid()) NOT NULL,
    [MemberId]     UNIQUEIDENTIFIER NULL,
    [Title]        NVARCHAR (256)   NOT NULL,
    [Slug]         VARCHAR (512)    NOT NULL,
    [Header]       NVARCHAR (MAX)   NULL,
    [Content]      NVARCHAR (MAX)   NULL,
    [ImageUrl]     VARCHAR (256)    NULL,
    [ThumbnailUrl] VARCHAR (256)    NULL,
    [Created]      DATETIME         CONSTRAINT [DF_BlogPost_Created] DEFAULT (getdate()) NOT NULL,
    [Author]       NVARCHAR (50)    NULL,
    CONSTRAINT [PK_BlogPost] PRIMARY KEY CLUSTERED ([BlogPostId] ASC),
    CONSTRAINT [UK_BlogPost_Slug] UNIQUE NONCLUSTERED ([Slug] ASC)
);



