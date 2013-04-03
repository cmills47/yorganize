CREATE TABLE [dbo].[MemberNotificationStatus] (
    [AdminNotificationId] UNIQUEIDENTIFIER NOT NULL,
    [MemberId]            UNIQUEIDENTIFIER NOT NULL,
    [Status]              VARCHAR (16)     NULL,
    PRIMARY KEY CLUSTERED ([AdminNotificationId] ASC, [MemberId] ASC),
    CHECK ([Status]='Deleted' OR [Status]='Viewed' OR [Status]='New' OR [Status]=NULL),
    FOREIGN KEY ([AdminNotificationId]) REFERENCES [dbo].[AdminNotification] ([AdminNotificationId]),
    FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId])
);

