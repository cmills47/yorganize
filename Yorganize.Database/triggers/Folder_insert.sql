USE [Yorganize]
GO

/****** Object:  Trigger [Folder_insert_trigger]    Script Date: 5/24/2013 12:09:10 PM ******/
DROP TRIGGER [dbo].[Folder_insert_trigger]
GO

/****** Object:  Trigger [dbo].[Folder_insert_trigger]    Script Date: 5/24/2013 12:09:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Folder_insert_trigger] ON [dbo].[Folder] FOR INSERT AS
SET NOCOUNT ON
UPDATE [side] SET [sync_row_is_tombstone] = 0, [local_update_peer_key] = 0, [local_update_peer_timestamp] = sync_row_version, [update_scope_local_id] = NULL, [last_change_datetime] = GETDATE(), [MemberId] = [i].[MemberId] FROM [Folder_tracking] [side] JOIN (SELECT INSERTED.[FolderId], INSERTED.[MemberId], get_new_rowversion() AS sync_row_version FROM INSERTED) AS [i] ON [side].[FolderId] = [i].[FolderId]
INSERT INTO [Folder_tracking] ([i].[FolderId], [create_scope_local_id], [local_create_peer_key], [local_create_peer_timestamp], [update_scope_local_id], [local_update_peer_key], [local_update_peer_timestamp], [sync_row_is_tombstone], [last_change_datetime], [i].[MemberId]) SELECT [i].[FolderId], NULL, 0, sync_row_version, NULL, 0, sync_row_version, 0, GETDATE() , [i].[MemberId] FROM (SELECT INSERTED.[FolderId], INSERTED.[MemberId], get_new_rowversion() AS sync_row_version FROM INSERTED) AS [i] LEFT JOIN [Folder_tracking] [side] ON [side].[FolderId] = [i].[FolderId] WHERE [side].[FolderId] IS NULL
SET NOCOUNT OFF
GO


