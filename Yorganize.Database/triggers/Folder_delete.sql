USE [Yorganize]
GO

/****** Object:  Trigger [Folder_delete_trigger]    Script Date: 5/24/2013 12:07:18 PM ******/
DROP TRIGGER [dbo].[Folder_delete_trigger]
GO

/****** Object:  Trigger [dbo].[Folder_delete_trigger]    Script Date: 5/24/2013 12:07:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Folder_delete_trigger] ON [dbo].[Folder] FOR DELETE AS
SET NOCOUNT ON
UPDATE [side] SET [sync_row_is_tombstone] = 1, [local_update_peer_key] = 0, [local_update_peer_timestamp] = sync_row_version, [update_scope_local_id] = NULL, [last_change_datetime] = GETDATE(), [MemberId] = [d].[MemberId] FROM [Folder_tracking] [side] JOIN (SELECT DELETED.[FolderId], DELETED.[MemberId], get_new_rowversion() AS sync_row_version FROM DELETED) AS [d] ON [side].[FolderId] = [d].[FolderId]
SET NOCOUNT OFF
GO


