using System;
using Team17.Online;
using UnityEngine;

// Token: 0x02000A9D RID: 2717
public class KitchenSendInviteTask : KitchenTask
{
	// Token: 0x060035BA RID: 13754 RVA: 0x000FB3D6 File Offset: 0x000F97D6
	public override void Start()
	{
		base.Start();
		this.m_RequestOnlineMode = false;
		if (!ConnectionStatus.IsInSession())
		{
			if (!this.CheckForInvalidLocalPlayers())
			{
				this.m_RequestOnlineMode = true;
			}
			this.m_status = KitchenTaskStatus.Running;
		}
		else
		{
			this.ShowInviteUI();
		}
	}

	// Token: 0x060035BB RID: 13755 RVA: 0x000FB414 File Offset: 0x000F9814
	public override void CleanUp()
	{
		base.CleanUp();
		ConnectionModeSwitcher.InvalidateCallback(new GenericVoid<IConnectionModeSwitchStatus>(this.OnConnectionStateRequestComplete));
	}

	// Token: 0x060035BC RID: 13756 RVA: 0x000FB430 File Offset: 0x000F9830
	public override void Update()
	{
		base.Update();
		if (this.m_RequestOnlineMode)
		{
			this.RequestOnlineMode();
		}
		if (this.m_progressBox != null)
		{
			string localisedProgressDescription = ConnectionModeSwitcher.GetStatus().GetLocalisedProgressDescription();
			this.m_progressBox.SetMessage(localisedProgressDescription, false);
		}
	}

	// Token: 0x060035BD RID: 13757 RVA: 0x000FB47D File Offset: 0x000F987D
	private bool CheckForInvalidLocalPlayers()
	{
		if (UserSystemUtils.AnySplitPadUsers())
		{
			NetworkDialogHelper.ShowRemoveSplitPadUsersDialog(new T17DialogBox.DialogEvent(this.RemoveLocalGuestsBeforeInvite), new T17DialogBox.DialogEvent(this.CancelInvite));
			return true;
		}
		return false;
	}

	// Token: 0x060035BE RID: 13758 RVA: 0x000FB4A9 File Offset: 0x000F98A9
	private void RemoveLocalGuestsBeforeInvite()
	{
		UserSystemUtils.RemoveAllSplitPadGuestUsers();
		this.m_RequestOnlineMode = true;
	}

	// Token: 0x060035BF RID: 13759 RVA: 0x000FB4B7 File Offset: 0x000F98B7
	private void CancelInvite()
	{
		this.TaskComplete(KitchenTaskResult.Success);
	}

	// Token: 0x060035C0 RID: 13760 RVA: 0x000FB4C0 File Offset: 0x000F98C0
	private void RequestOnlineMode()
	{
		this.m_RequestOnlineMode = false;
		this.m_ServerOptions.gameMode = GameMode.OnlineKitchen;
		this.m_ServerOptions.visibility = OnlineMultiplayerSessionVisibility.ePrivate;
		this.m_ServerOptions.hostUser = this.m_IPlayerManager.GetUser(EngagementSlot.One);
		this.m_ServerOptions.connectionMode = OnlineMultiplayerConnectionMode.eInternet;
		try
		{
			this.m_progressBox = T17DialogBoxManager.GetDialog(false);
			if (this.m_progressBox != null)
			{
				this.m_progressBox.Initialize("Text.PleaseWait", string.Empty, null, null, null, T17DialogBox.Symbols.Spinner, true, true, false);
				this.m_progressBox.Show();
			}
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Server, this.m_ServerOptions, new GenericVoid<IConnectionModeSwitchStatus>(this.OnConnectionStateRequestComplete));
		}
		catch (UnityException ex)
		{
			this.TaskComplete(KitchenTaskResult.Failure);
		}
	}

	// Token: 0x060035C1 RID: 13761 RVA: 0x000FB594 File Offset: 0x000F9994
	private void OnConnectionStateRequestComplete(IConnectionModeSwitchStatus status)
	{
		if (status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			this.ShowInviteUI();
		}
		else
		{
			this.TaskComplete(KitchenTaskResult.Failure);
		}
	}

	// Token: 0x060035C2 RID: 13762 RVA: 0x000FB5B8 File Offset: 0x000F99B8
	private bool ShowInviteUI()
	{
		bool result = false;
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		IOnlineMultiplayerSessionCoordinator onlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		if (onlineMultiplayerSessionCoordinator != null)
		{
			string msg = Localization.Get("Online.GameInvite.Message", new LocToken[0]);
			onlineMultiplayerSessionCoordinator.ShowSendInviteDialog(msg);
			result = true;
		}
		this.TaskComplete(KitchenTaskResult.Success);
		return result;
	}

	// Token: 0x060035C3 RID: 13763 RVA: 0x000FB5FC File Offset: 0x000F99FC
	protected override void TaskComplete(KitchenTaskResult result)
	{
		if (this.m_progressBox != null)
		{
			this.m_progressBox.Hide();
			this.m_progressBox = null;
		}
		base.TaskComplete(result);
	}

	// Token: 0x04002B3F RID: 11071
	private ServerOptions m_ServerOptions = default(ServerOptions);

	// Token: 0x04002B40 RID: 11072
	private T17DialogBox m_progressBox;

	// Token: 0x04002B41 RID: 11073
	private bool m_RequestOnlineMode;
}
