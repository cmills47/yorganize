CREATE TABLE [dbo].[Member] (
    [MemberId] UNIQUEIDENTIFIER CONSTRAINT [DF_Member_MemberId] DEFAULT (newid()) NOT NULL,
    [LiveId]   VARCHAR (50)     NULL,
    CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED ([MemberId] ASC)
);



