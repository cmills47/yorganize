CREATE TABLE [dbo].[AdminNotification] (
    [AdminNotificationId] UNIQUEIDENTIFIER NOT NULL,
    [Message]             NVARCHAR (MAX)   NULL,
    [StartDate]           DATETIME         NULL,
    [DueDate]             DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([AdminNotificationId] ASC)
);

