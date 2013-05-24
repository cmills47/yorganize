USE [Yorganize]
GO

/****** Object:  Trigger [Project_insert_trigger]    Script Date: 5/24/2013 12:32:42 PM ******/
DROP TRIGGER [dbo].[Project_insert_trigger]
GO

/****** Object:  Trigger [dbo].[Project_insert_trigger]    Script Date: 5/24/2013 12:32:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Project_insert_trigger] ON [dbo].[Project] FOR INSERT AS
SET NOCOUNT ON
UPDATE [side] SET [sync_row_is_tombstone] = 0, [local_update_peer_key] = 0, [local_update_peer_timestamp] = sync_row_version, [update_scope_local_id] = NULL, [last_change_datetime] = GETDATE(), [MemberId] = [i].[MemberId] FROM [Project_tracking] [side] JOIN (SELECT INSERTED.[ProjectId], INSERTED.[MemberId], get_new_rowversion() AS sync_row_version FROM INSERTED) AS [i] ON [side].[ProjectId] = [i].[ProjectId]
INSERT INTO [Project_tracking] ([i].[ProjectId], [create_scope_local_id], [local_create_peer_key], [local_create_peer_timestamp], [update_scope_local_id], [local_update_peer_key], [local_update_peer_timestamp], [sync_row_is_tombstone], [last_change_datetime], [i].[MemberId]) SELECT [i].[ProjectId], NULL, 0, sync_row_version, NULL, 0, sync_row_version, 0, GETDATE() , [i].[MemberId] FROM (SELECT INSERTED.[ProjectId], INSERTED.[MemberId], get_new_rowversion() AS sync_row_version FROM INSERTED) AS [i] LEFT JOIN [Project_tracking] [side] ON [side].[ProjectId] = [i].[ProjectId] WHERE [side].[ProjectId] IS NULL
SET NOCOUNT OFF
GO


