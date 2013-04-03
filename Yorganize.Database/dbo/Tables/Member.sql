CREATE TABLE [dbo].[Member] (
    [MemberId] UNIQUEIDENTIFIER NOT NULL,
    [LiveId]   VARCHAR (64)     NULL,
    PRIMARY KEY CLUSTERED ([MemberId] ASC)
);

