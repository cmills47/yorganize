USE [Yorganize]
GO

/****** Object:  Trigger [Action_insert_trigger]    Script Date: 5/24/2013 12:33:58 PM ******/
DROP TRIGGER [dbo].[Action_insert_trigger]
GO

/****** Object:  Trigger [dbo].[Action_insert_trigger]    Script Date: 5/24/2013 12:33:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Action_insert_trigger] ON [dbo].[Action] FOR INSERT AS
SET NOCOUNT ON
UPDATE [side] SET [sync_row_is_tombstone] = 0, [local_update_peer_key] = 0, [local_update_peer_timestamp] = sync_row_version, [update_scope_local_id] = NULL, [last_change_datetime] = GETDATE(), [MemberId] = [i].[MemberId] FROM [Action_tracking] [side] JOIN (SELECT INSERTED.[ActionId], INSERTED.[MemberId], get_new_rowversion() AS sync_row_version FROM INSERTED) AS [i] ON [side].[ActionId] = [i].[ActionId]
INSERT INTO [Action_tracking] ([i].[ActionId], [create_scope_local_id], [local_create_peer_key], [local_create_peer_timestamp], [update_scope_local_id], [local_update_peer_key], [local_update_peer_timestamp], [sync_row_is_tombstone], [last_change_datetime], [i].[MemberId]) SELECT [i].[ActionId], NULL, 0, sync_row_version, NULL, 0, sync_row_version, 0, GETDATE() , [i].[MemberId] FROM (SELECT INSERTED.[ActionId], INSERTED.[MemberId], get_new_rowversion() AS sync_row_version FROM INSERTED) AS [i] LEFT JOIN [Action_tracking] [side] ON [side].[ActionId] = [i].[ActionId] WHERE [side].[ActionId] IS NULL
SET NOCOUNT OFF
GO


