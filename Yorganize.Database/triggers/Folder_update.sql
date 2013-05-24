USE [Yorganize]
GO

/****** Object:  Trigger [Folder_update_trigger]    Script Date: 5/24/2013 12:10:03 PM ******/
DROP TRIGGER [dbo].[Folder_update_trigger]
GO

/****** Object:  Trigger [dbo].[Folder_update_trigger]    Script Date: 5/24/2013 12:10:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Folder_update_trigger] ON [dbo].[Folder] FOR UPDATE AS
SET NOCOUNT ON
UPDATE [side] SET [local_update_peer_key] = 0, [local_update_peer_timestamp] = sync_row_version, [update_scope_local_id] = NULL, [last_change_datetime] = GETDATE(), [MemberId] = [i].[MemberId] FROM [Folder_tracking] [side] JOIN (SELECT INSERTED.[FolderId], INSERTED.[MemberId], get_new_rowversion() AS sync_row_version FROM INSERTED) AS [i] ON [side].[FolderId] = [i].[FolderId]
SET NOCOUNT OFF
GO


