using System;

// Token: 0x0200092B RID: 2347
internal static class NetworkDialogHelper
{
	// Token: 0x06002E47 RID: 11847 RVA: 0x000DAEC4 File Offset: 0x000D92C4
	public static void ShowRemoveSplitPadUsersDialog(T17DialogBox.DialogEvent OnConfirm, T17DialogBox.DialogEvent OnCancel)
	{
		T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
		if (dialog != null)
		{
			dialog.Initialize("Text.Warning", "MainMenu.Kitchen.Invite.WarnRemoveLocals", "Text.Button.Continue", null, "Text.Button.Cancel", T17DialogBox.Symbols.Warning, true, true, false);
			T17DialogBox t17DialogBox = dialog;
			t17DialogBox.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(t17DialogBox.OnConfirm, OnConfirm);
			T17DialogBox t17DialogBox2 = dialog;
			t17DialogBox2.OnCancel = (T17DialogBox.DialogEvent)Delegate.Combine(t17DialogBox2.OnCancel, OnCancel);
			dialog.Show();
		}
	}

	// Token: 0x06002E48 RID: 11848 RVA: 0x000DAF38 File Offset: 0x000D9338
	public static void ShowNoOnlineUsersDialog(T17DialogBox.DialogEvent OnConfirm)
	{
		T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
		if (dialog != null)
		{
			dialog.Initialize("Text.Warning", "MainMenu.Kitchen.NoOnlineUsers", "Text.Button.Continue", string.Empty, string.Empty, T17DialogBox.Symbols.Warning, true, true, false);
			T17DialogBox t17DialogBox = dialog;
			t17DialogBox.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(t17DialogBox.OnConfirm, OnConfirm);
			dialog.Show();
		}
	}

	// Token: 0x06002E49 RID: 11849 RVA: 0x000DAF98 File Offset: 0x000D9398
	public static void ShowFullLobbyDialog()
	{
		T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
		if (dialog != null)
		{
			dialog.Initialize("Text.Warning", "MainMenu.Kitchen.FullKitchen", "Text.Button.Continue", string.Empty, string.Empty, T17DialogBox.Symbols.Warning, true, true, false);
			dialog.Show();
		}
	}

	// Token: 0x06002E4A RID: 11850 RVA: 0x000DAFE4 File Offset: 0x000D93E4
	public static void ShowGoingOfflineDialog(T17DialogBox.DialogEvent OnConfirm)
	{
		T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
		if (dialog != null)
		{
			dialog.Initialize("Text.Warning", "MainMenu.Kitchen.WarnGoingOffline", "Text.Button.Continue", null, "Text.Button.Cancel", T17DialogBox.Symbols.Warning, true, true, false);
			T17DialogBox t17DialogBox = dialog;
			t17DialogBox.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(t17DialogBox.OnConfirm, OnConfirm);
			dialog.Show();
		}
	}

	// Token: 0x06002E4B RID: 11851 RVA: 0x000DB040 File Offset: 0x000D9440
	public static void ShowMoreUsersRequiredDialog()
	{
		T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
		if (dialog != null)
		{
			dialog.Initialize("Text.Warning", "MainMenu.Kitchen.MoreUsersNeeded", "Text.Button.Okay", string.Empty, string.Empty, T17DialogBox.Symbols.Warning, true, true, false);
			dialog.Show();
		}
	}

	// Token: 0x06002E4C RID: 11852 RVA: 0x000DB08C File Offset: 0x000D948C
	public static void ShowNoWirelessUsersDialog()
	{
		T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
		if (dialog != null)
		{
			dialog.Initialize("Text.Warning", "MainMenu.Kitchen.NoWirelessUsers", "Text.Button.Continue", string.Empty, string.Empty, T17DialogBox.Symbols.Warning, true, true, false);
			dialog.Show();
		}
	}

	// Token: 0x06002E4D RID: 11853 RVA: 0x000DB0D8 File Offset: 0x000D94D8
	public static void ShowTooManyLocalUsersForJoining()
	{
		T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
		if (dialog != null)
		{
			dialog.Initialize("Text.Warning", "MainMenu.Kitchen.TooManyLocalUsersForJoining", "Text.Button.Continue", string.Empty, string.Empty, T17DialogBox.Symbols.Warning, true, true, false);
			dialog.Show();
		}
	}
}
