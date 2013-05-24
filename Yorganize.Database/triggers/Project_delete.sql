USE [Yorganize]
GO

/****** Object:  Trigger [Project_delete_trigger]    Script Date: 5/24/2013 12:28:17 PM ******/
DROP TRIGGER [dbo].[Project_delete_trigger]
GO

/****** Object:  Trigger [dbo].[Project_delete_trigger]    Script Date: 5/24/2013 12:28:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Project_delete_trigger] ON [dbo].[Project] FOR DELETE AS
SET NOCOUNT ON
UPDATE [side] SET [sync_row_is_tombstone] = 1, [local_update_peer_key] = 0, [local_update_peer_timestamp] = sync_row_version, [update_scope_local_id] = NULL, [last_change_datetime] = GETDATE(), [MemberId] = [d].[MemberId] FROM [Project_tracking] [side] JOIN (SELECT DELETED.[ProjectId], DELETED.[MemberId], get_new_rowversion() AS sync_row_version FROM DELETED) AS [d] ON [side].[ProjectId] = [d].[ProjectId]
SET NOCOUNT OFF
GO


