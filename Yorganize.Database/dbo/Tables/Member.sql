CREATE TABLE [dbo].[Member] (
    [MemberId]            UNIQUEIDENTIFIER NOT NULL,
    [LiveId]              VARCHAR (64)     NULL,
    [FirstName]           NVARCHAR (256)   NULL,
    [LastName]            NVARCHAR (256)   NULL,
    [Locale]              NVARCHAR (32)    NULL,
    [PreferredEmail]      NVARCHAR (320)   NULL,
    [AccountEmail]        NVARCHAR (320)   NULL,
    [PersonalEmail]       NVARCHAR (320)   NULL,
    [BusinessEmail]       NVARCHAR (320)   NULL,
    [OtherEmail]          NVARCHAR (320)   NULL,
    [RefreshToken]        VARCHAR (1024)   NULL,
    [AuthenticationToken] VARCHAR (1024)   NULL,
    [AccessToken]         VARCHAR (1024)   NULL,
    [YorganizeEmail]      NVARCHAR (320)   NULL,
    PRIMARY KEY CLUSTERED ([MemberId] ASC)
);





