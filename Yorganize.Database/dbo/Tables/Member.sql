CREATE TABLE [dbo].[Member] (
    [MemberId]            UNIQUEIDENTIFIER NOT NULL,
    [LiveId]              VARCHAR (64)     NOT NULL,
    [FirstName]           NVARCHAR (256)   NULL,
    [LastName]            NVARCHAR (256)   NULL,
    [Locale]              NVARCHAR (32)    NULL,
    [PreferredEmail]      NVARCHAR (256)   NULL,
    [AccountEmail]        NVARCHAR (256)   NULL,
    [PersonalEmail]       NVARCHAR (256)   NULL,
    [BusinessEmail]       NVARCHAR (256)   NULL,
    [OtherEmail]          NVARCHAR (256)   NULL,
    [RefreshToken]        NVARCHAR (128)   NULL,
    [AuthenticationToken] NVARCHAR (128)   NULL,
    [AccessToken]         VARCHAR (1024)   NULL,
    [YorganizeEmail]      NVARCHAR (256)   NULL,
    CONSTRAINT [PK__Member__0CF04B186A6939DD] PRIMARY KEY CLUSTERED ([MemberId] ASC)
);



