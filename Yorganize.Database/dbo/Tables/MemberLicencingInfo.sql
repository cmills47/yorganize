CREATE TABLE [dbo].[MemberLicencingInfo] (
    [MemberId]              UNIQUEIDENTIFIER NULL,
    [PaymentId]             UNIQUEIDENTIFIER NULL,
    [LicenceKey]            NVARCHAR (64)    NULL,
    [ExpiringDate]          DATETIME         NULL,
    [LicencedDate]          DATETIME         NULL,
    [Mode]                  VARCHAR (16)     NULL,
    [MemberLicencingInfoId] UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([MemberLicencingInfoId] ASC)
);

