CREATE TABLE [dbo].[Member] (
    [MemberId] UNIQUEIDENTIFIER NOT NULL,
    [LiveId]   VARCHAR (50)     NULL,
    CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED ([MemberId] ASC)
);

