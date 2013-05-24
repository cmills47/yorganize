USE [Yorganize]
GO

/****** Object:  Trigger [Project_update_trigger]    Script Date: 5/24/2013 12:28:07 PM ******/
DROP TRIGGER [dbo].[Project_update_trigger]
GO

/****** Object:  Trigger [dbo].[Project_update_trigger]    Script Date: 5/24/2013 12:28:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Project_update_trigger] ON [dbo].[Project] FOR UPDATE AS
SET NOCOUNT ON
UPDATE [side] SET [local_update_peer_key] = 0, [local_update_peer_timestamp] = sync_row_version, [update_scope_local_id] = NULL, [last_change_datetime] = GETDATE(), [MemberId] = [i].[MemberId] FROM [Project_tracking] [side] JOIN (SELECT INSERTED.[ProjectId], INSERTED.[MemberId], get_new_rowversion() AS sync_row_version FROM INSERTED) AS [i] ON [side].[ProjectId] = [i].[ProjectId]
SET NOCOUNT OFF
GO


