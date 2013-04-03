CREATE TABLE [dbo].[CustomViewFilter] (
    [CustomViewId]   UNIQUEIDENTIFIER NOT NULL,
    [FilterMethodId] UNIQUEIDENTIFIER NOT NULL,
    [MemberId]       UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([CustomViewId] ASC, [FilterMethodId] ASC),
    FOREIGN KEY ([CustomViewId]) REFERENCES [dbo].[CustomView] ([CustomViewId]),
    FOREIGN KEY ([FilterMethodId]) REFERENCES [dbo].[FilterMethod] ([FilterMethodId]),
    CONSTRAINT [FK_CustomViewFilter_0] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([MemberId])
);

