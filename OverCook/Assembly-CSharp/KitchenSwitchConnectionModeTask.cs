using System;
using System.Collections;
using Team17.Online;
using UnityEngine;

// Token: 0x02000A9E RID: 2718
public class KitchenSwitchConnectionModeTask : KitchenTask
{
	// Token: 0x060035C5 RID: 13765 RVA: 0x000FB6C8 File Offset: 0x000F9AC8
	public override void Start()
	{
		this.m_successMessage = string.Empty;
		this.m_DelayedSuccessEnumerator = null;
		switch (this.connectionMode)
		{
		case KitchenSwitchConnectionModeTask.Mode.Offline:
			this.RequestOfflineMode();
			break;
		case KitchenSwitchConnectionModeTask.Mode.Internet:
			if (this.internetSubmode == KitchenSwitchConnectionModeTask.SubMode.Search)
			{
				this.RequestOfflineMode(OnlineMultiplayerConnectionMode.eInternet);
			}
			else
			{
				this.RequestOnlineMode();
			}
			break;
		case KitchenSwitchConnectionModeTask.Mode.Wireless:
			this.RequestAdhocMode();
			break;
		case KitchenSwitchConnectionModeTask.Mode.JoinRoom:
			this.JoinEnumeratedRoom();
			break;
		}
		this.m_status = KitchenTaskStatus.Running;
	}

	// Token: 0x060035C6 RID: 13766 RVA: 0x000FB754 File Offset: 0x000F9B54
	public override void CleanUp()
	{
		base.CleanUp();
		ConnectionModeSwitcher.InvalidateCallback(new GenericVoid<IConnectionModeSwitchStatus>(this.OnRequestOfflineConnectionStateComplete));
		ConnectionModeSwitcher.InvalidateCallback(new GenericVoid<IConnectionModeSwitchStatus>(this.OnHostAdhocSessionComplete));
		ConnectionModeSwitcher.InvalidateCallback(new GenericVoid<IConnectionModeSwitchStatus>(this.OnAdhocSearchComplete));
		ConnectionModeSwitcher.InvalidateCallback(new GenericVoid<IConnectionModeSwitchStatus>(this.OnRequestOnlineConnectionStateComplete));
	}

	// Token: 0x060035C7 RID: 13767 RVA: 0x000FB7AC File Offset: 0x000F9BAC
	private IEnumerator ShowSuccessMessageThenComplete()
	{
		if (this.m_progressBox != null)
		{
			while (Time.time - this.m_startTime < this.c_messageMinDisplayTime)
			{
				yield return null;
			}
			if (this.c_showModeChangeSuccessMessages)
			{
				this.m_progressBox.SetMessage(this.m_successMessage, true);
				this.m_startTime = Time.time;
				while (Time.time - this.m_startTime < this.c_messageMinDisplayTime)
				{
					yield return null;
				}
			}
		}
		this.TaskComplete(KitchenTaskResult.Success);
		yield break;
	}

	// Token: 0x060035C8 RID: 13768 RVA: 0x000FB7C8 File Offset: 0x000F9BC8
	public override void Update()
	{
		base.Update();
		if (this.m_DelayedSuccessEnumerator != null && !this.m_DelayedSuccessEnumerator.MoveNext())
		{
			this.m_DelayedSuccessEnumerator = null;
		}
		if (this.m_joiningEnumeratedRoom && ConnectionStatus.CurrentConnectionMode() != OnlineMultiplayerConnectionMode.eAdhoc)
		{
			this.m_joiningEnumeratedRoom = false;
			this.TaskComplete(KitchenTaskResult.Failure);
		}
	}

	// Token: 0x060035C9 RID: 13769 RVA: 0x000FB824 File Offset: 0x000F9C24
	private void RequestOfflineMode()
	{
		OnlineMultiplayerConnectionMode onlineMultiplayerConnectionMode;
		if (ConnectionStatus.CurrentConnectionMode() == OnlineMultiplayerConnectionMode.eInternet)
		{
			onlineMultiplayerConnectionMode = OnlineMultiplayerConnectionMode.eInternet;
		}
		else
		{
			onlineMultiplayerConnectionMode = OnlineMultiplayerConnectionMode.eNone;
		}
		this.RequestOfflineMode(onlineMultiplayerConnectionMode);
	}

	// Token: 0x060035CA RID: 13770 RVA: 0x000FB84C File Offset: 0x000F9C4C
	private void RequestOfflineMode(OnlineMultiplayerConnectionMode connectionMode)
	{
		try
		{
			this.m_progressBox = T17DialogBoxManager.GetDialog(false);
			if (this.m_progressBox != null)
			{
				this.m_progressBox.Initialize("Text.PleaseWait", this.c_OfflineProgressMessage, null, null, null, T17DialogBox.Symbols.Spinner, true, true, false);
				this.m_progressBox.Show();
			}
			this.m_startTime = Time.time;
			this.m_successMessage = this.c_OfflineSuccessMessage;
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, new OfflineOptions
			{
				hostUser = this.m_IPlayerManager.GetUser(EngagementSlot.One),
				connectionMode = new OnlineMultiplayerConnectionMode?(connectionMode)
			}, new GenericVoid<IConnectionModeSwitchStatus>(this.OnRequestOfflineConnectionStateComplete));
		}
		catch (UnityException ex)
		{
			this.TaskComplete(KitchenTaskResult.Failure);
		}
	}

	// Token: 0x060035CB RID: 13771 RVA: 0x000FB918 File Offset: 0x000F9D18
	private void OnRequestOfflineConnectionStateComplete(IConnectionModeSwitchStatus status)
	{
		if (status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			this.m_DelayedSuccessEnumerator = this.ShowSuccessMessageThenComplete();
		}
		else
		{
			CompositeStatus compositeStatus = status as CompositeStatus;
			ConnectionModeStatus connectionModeStatus = null;
			if (compositeStatus != null)
			{
				connectionModeStatus = (compositeStatus.m_TaskSubStatus as ConnectionModeStatus);
			}
			if (connectionModeStatus != null && connectionModeStatus.m_Result.m_returnCode == OnlineMultiplayerConnectionModeConnectResult.eCancelledByUser)
			{
				this.TaskComplete(KitchenTaskResult.Cancelled);
			}
			else
			{
				this.TaskComplete(KitchenTaskResult.Failure);
			}
		}
	}

	// Token: 0x060035CC RID: 13772 RVA: 0x000FB988 File Offset: 0x000F9D88
	private void RequestAdhocMode()
	{
		KitchenSwitchConnectionModeTask.SubMode subMode = this.adhocSubmode;
		if (subMode != KitchenSwitchConnectionModeTask.SubMode.Host)
		{
			if (subMode != KitchenSwitchConnectionModeTask.SubMode.Search)
			{
				if (subMode == KitchenSwitchConnectionModeTask.SubMode.AskUser)
				{
					T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
					if (dialog != null)
					{
						dialog.Initialize("MainMenu.Kitchen.AdHoc.HostOrSearch.Title", "MainMenu.Kitchen.AdHoc.HostOrSearch.Body", "Text.Button.Host", "Text.Button.Search", "Text.Button.Cancel", T17DialogBox.Symbols.Warning, true, true, false);
						T17DialogBox t17DialogBox = dialog;
						t17DialogBox.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(t17DialogBox.OnConfirm, new T17DialogBox.DialogEvent(this.HostAdhocSession));
						T17DialogBox t17DialogBox2 = dialog;
						t17DialogBox2.OnDecline = (T17DialogBox.DialogEvent)Delegate.Combine(t17DialogBox2.OnDecline, new T17DialogBox.DialogEvent(this.SearchForAdhocSession));
						T17DialogBox t17DialogBox3 = dialog;
						t17DialogBox3.OnCancel = (T17DialogBox.DialogEvent)Delegate.Combine(t17DialogBox3.OnCancel, new T17DialogBox.DialogEvent(this.CancelConnectionModeSwitch));
						dialog.Show();
					}
				}
			}
			else
			{
				this.SearchForAdhocSession();
			}
		}
		else
		{
			this.HostAdhocSession();
		}
	}

	// Token: 0x060035CD RID: 13773 RVA: 0x000FBA74 File Offset: 0x000F9E74
	private void HostAdhocSession()
	{
		this.m_hosting = true;
		this.m_ServerOptions.gameMode = GameMode.OnlineKitchen;
		this.m_ServerOptions.visibility = OnlineMultiplayerSessionVisibility.ePrivate;
		this.m_ServerOptions.hostUser = this.m_IPlayerManager.GetUser(EngagementSlot.One);
		this.m_ServerOptions.connectionMode = OnlineMultiplayerConnectionMode.eAdhoc;
		try
		{
			this.m_progressBox = T17DialogBoxManager.GetDialog(false);
			if (this.m_progressBox != null)
			{
				this.m_progressBox.Initialize("Text.PleaseWait", this.c_AdhocHostProgressMessage, null, null, null, T17DialogBox.Symbols.Spinner, true, true, false);
				this.m_progressBox.Show();
			}
			this.m_startTime = Time.time;
			this.m_successMessage = this.c_AdhocHostSuccessMessage;
			DisconnectionHandler.ConnectionModeErrorEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>)Delegate.Combine(DisconnectionHandler.ConnectionModeErrorEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>(this.OnConnectionModeError));
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Server, this.m_ServerOptions, new GenericVoid<IConnectionModeSwitchStatus>(this.OnHostAdhocSessionComplete));
		}
		catch (UnityException ex)
		{
			this.TaskComplete(KitchenTaskResult.Failure);
		}
	}

	// Token: 0x060035CE RID: 13774 RVA: 0x000FBB80 File Offset: 0x000F9F80
	private void OnHostAdhocSessionComplete(IConnectionModeSwitchStatus status)
	{
		DisconnectionHandler.ConnectionModeErrorEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>)Delegate.Remove(DisconnectionHandler.ConnectionModeErrorEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>(this.OnConnectionModeError));
		if (status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			this.m_DelayedSuccessEnumerator = this.ShowSuccessMessageThenComplete();
		}
		else
		{
			this.TaskComplete(KitchenTaskResult.Failure);
		}
	}

	// Token: 0x060035CF RID: 13775 RVA: 0x000FBBD4 File Offset: 0x000F9FD4
	private void SearchForAdhocSession()
	{
		this.m_hosting = false;
		if (this.c_showProgressSpinnerForAdhocSearch)
		{
			this.m_progressBox = T17DialogBoxManager.GetDialog(false);
			if (this.m_progressBox != null)
			{
				this.m_progressBox.Initialize("Text.PleaseWait", this.c_AdhocSearchProgressMessage, null, null, null, T17DialogBox.Symbols.Spinner, true, true, false);
				this.m_progressBox.Show();
			}
		}
		GamepadUser user = this.m_IPlayerManager.GetUser(EngagementSlot.One);
		ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, new OfflineOptions
		{
			hostUser = user,
			searchGameMode = GameMode.OnlineKitchen,
			eAdditionalAction = OfflineOptions.AdditionalAction.PrivilegeCheckAllUsersAndSearchForGames,
			connectionMode = new OnlineMultiplayerConnectionMode?(OnlineMultiplayerConnectionMode.eAdhoc)
		}, new GenericVoid<IConnectionModeSwitchStatus>(this.OnAdhocSearchComplete));
	}

	// Token: 0x060035D0 RID: 13776 RVA: 0x000FBC8C File Offset: 0x000FA08C
	private void OnAdhocSearchComplete(IConnectionModeSwitchStatus status)
	{
		if (status != null && status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			SearchTask.SearchResultData searchResultData = ConnectionModeSwitcher.GetAgentData() as SearchTask.SearchResultData;
			if (searchResultData != null && searchResultData.m_AvailableSessions != null)
			{
				if (this.onResults != null)
				{
					this.onResults(searchResultData);
				}
			}
			this.TaskComplete(KitchenTaskResult.Success);
		}
		else
		{
			this.TaskComplete(KitchenTaskResult.Failure);
		}
	}

	// Token: 0x060035D1 RID: 13777 RVA: 0x000FBCF6 File Offset: 0x000FA0F6
	private void CancelConnectionModeSwitch()
	{
		this.TaskComplete(KitchenTaskResult.Cancelled);
	}

	// Token: 0x060035D2 RID: 13778 RVA: 0x000FBD00 File Offset: 0x000FA100
	private void RequestOnlineMode()
	{
		this.m_ServerOptions.gameMode = GameMode.OnlineKitchen;
		this.m_ServerOptions.visibility = OnlineMultiplayerSessionVisibility.ePrivate;
		this.m_ServerOptions.hostUser = this.m_IPlayerManager.GetUser(EngagementSlot.One);
		this.m_ServerOptions.connectionMode = OnlineMultiplayerConnectionMode.eInternet;
		try
		{
			this.m_progressBox = T17DialogBoxManager.GetDialog(false);
			if (this.m_progressBox != null)
			{
				this.m_progressBox.Initialize("Text.PleaseWait", this.c_OnlineProgressMessage, null, null, null, T17DialogBox.Symbols.Spinner, true, true, false);
				this.m_progressBox.Show();
			}
			this.m_startTime = Time.time;
			this.m_successMessage = this.c_OnlineSuccessMessage;
			DisconnectionHandler.ConnectionModeErrorEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>)Delegate.Combine(DisconnectionHandler.ConnectionModeErrorEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>(this.OnConnectionModeError));
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Server, this.m_ServerOptions, new GenericVoid<IConnectionModeSwitchStatus>(this.OnRequestOnlineConnectionStateComplete));
		}
		catch (UnityException ex)
		{
			this.TaskComplete(KitchenTaskResult.Failure);
		}
	}

	// Token: 0x060035D3 RID: 13779 RVA: 0x000FBE04 File Offset: 0x000FA204
	private void OnRequestOnlineConnectionStateComplete(IConnectionModeSwitchStatus status)
	{
		DisconnectionHandler.ConnectionModeErrorEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>)Delegate.Remove(DisconnectionHandler.ConnectionModeErrorEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>(this.OnConnectionModeError));
		if (status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			this.m_DelayedSuccessEnumerator = this.ShowSuccessMessageThenComplete();
		}
		else
		{
			CompositeStatus compositeStatus = status as CompositeStatus;
			ConnectionModeStatus connectionModeStatus = null;
			if (compositeStatus != null)
			{
				connectionModeStatus = (compositeStatus.m_TaskSubStatus as ConnectionModeStatus);
			}
			if (connectionModeStatus != null && connectionModeStatus.m_Result.m_returnCode == OnlineMultiplayerConnectionModeConnectResult.eCancelledByUser)
			{
				this.TaskComplete(KitchenTaskResult.Cancelled);
			}
			else
			{
				this.TaskComplete(KitchenTaskResult.Failure);
			}
		}
	}

	// Token: 0x060035D4 RID: 13780 RVA: 0x000FBE94 File Offset: 0x000FA294
	private void JoinEnumeratedRoom()
	{
		if (this.joinOptions == null)
		{
			this.TaskComplete(KitchenTaskResult.Failure);
			return;
		}
		this.m_progressBox = T17DialogBoxManager.GetDialog(false);
		if (this.m_progressBox != null)
		{
			this.m_progressBox.Initialize("Text.PleaseWait", this.joinProgressText, null, null, null, T17DialogBox.Symbols.Spinner, true, false, false);
			this.m_progressBox.Show();
		}
		this.m_startTime = Time.time;
		this.m_successMessage = string.Empty;
		this.m_joiningEnumeratedRoom = ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.JoinEnumeratedRoom, this.joinOptions, new GenericVoid<IConnectionModeSwitchStatus>(this.OnJoinEnumeratedRoomComplete));
	}

	// Token: 0x060035D5 RID: 13781 RVA: 0x000FBF2D File Offset: 0x000FA32D
	private void OnJoinEnumeratedRoomComplete(IConnectionModeSwitchStatus status)
	{
		this.joinOptions = null;
		this.joinProgressText = string.Empty;
		this.m_joiningEnumeratedRoom = false;
		if (status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			this.m_DelayedSuccessEnumerator = this.ShowSuccessMessageThenComplete();
		}
		else
		{
			this.TaskComplete(KitchenTaskResult.Failure);
		}
	}

	// Token: 0x060035D6 RID: 13782 RVA: 0x000FBF6C File Offset: 0x000FA36C
	private void OnConnectionModeError(OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult> result)
	{
		DisconnectionHandler.ConnectionModeErrorEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>)Delegate.Remove(DisconnectionHandler.ConnectionModeErrorEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>(this.OnConnectionModeError));
		this.TaskComplete(KitchenTaskResult.Failure);
	}

	// Token: 0x060035D7 RID: 13783 RVA: 0x000FBF95 File Offset: 0x000FA395
	protected override void TaskComplete(KitchenTaskResult result)
	{
		if (this.m_progressBox != null)
		{
			this.m_progressBox.Hide();
			this.m_progressBox = null;
		}
		base.TaskComplete(result);
	}

	// Token: 0x04002B42 RID: 11074
	public KitchenSwitchConnectionModeTask.Mode connectionMode;

	// Token: 0x04002B43 RID: 11075
	public KitchenSwitchConnectionModeTask.SubMode internetSubmode = KitchenSwitchConnectionModeTask.SubMode.Host;

	// Token: 0x04002B44 RID: 11076
	public KitchenSwitchConnectionModeTask.SubMode adhocSubmode;

	// Token: 0x04002B45 RID: 11077
	public JoinEnumeratedRoomOptions joinOptions;

	// Token: 0x04002B46 RID: 11078
	public string joinProgressText = string.Empty;

	// Token: 0x04002B47 RID: 11079
	public bool m_hosting = true;

	// Token: 0x04002B48 RID: 11080
	public KitchenSwitchConnectionModeTask.OnResults onResults;

	// Token: 0x04002B49 RID: 11081
	private ServerOptions m_ServerOptions = default(ServerOptions);

	// Token: 0x04002B4A RID: 11082
	private T17DialogBox m_progressBox;

	// Token: 0x04002B4B RID: 11083
	private string m_successMessage = string.Empty;

	// Token: 0x04002B4C RID: 11084
	private IEnumerator m_DelayedSuccessEnumerator;

	// Token: 0x04002B4D RID: 11085
	private float m_startTime;

	// Token: 0x04002B4E RID: 11086
	private bool m_joiningEnumeratedRoom;

	// Token: 0x04002B4F RID: 11087
	private readonly bool c_showModeChangeSuccessMessages;

	// Token: 0x04002B50 RID: 11088
	private readonly bool c_showProgressSpinnerForAdhocSearch;

	// Token: 0x04002B51 RID: 11089
	private readonly float c_messageMinDisplayTime = 3f;

	// Token: 0x04002B52 RID: 11090
	private readonly string c_OfflineProgressMessage = "MainMenu.Kitchen.ChangingMode.Offline";

	// Token: 0x04002B53 RID: 11091
	private readonly string c_AdhocHostProgressMessage = "MainMenu.Kitchen.ChangingMode.Wireless.Host";

	// Token: 0x04002B54 RID: 11092
	private readonly string c_AdhocSearchProgressMessage = "MainMenu.Kitchen.ChangingMode.Wireless.Search";

	// Token: 0x04002B55 RID: 11093
	private readonly string c_OnlineProgressMessage = "MainMenu.Kitchen.ChangingMode.Online";

	// Token: 0x04002B56 RID: 11094
	private readonly string c_OfflineSuccessMessage = "MainMenu.Kitchen.ModeChanged.Offline";

	// Token: 0x04002B57 RID: 11095
	private readonly string c_AdhocHostSuccessMessage = "MainMenu.Kitchen.ModeChanged.Wireless.Host";

	// Token: 0x04002B58 RID: 11096
	private readonly string c_OnlineSuccessMessage = "MainMenu.Kitchen.ModeChanged.Online";

	// Token: 0x02000A9F RID: 2719
	public enum Mode
	{
		// Token: 0x04002B5A RID: 11098
		Offline,
		// Token: 0x04002B5B RID: 11099
		Internet,
		// Token: 0x04002B5C RID: 11100
		Wireless,
		// Token: 0x04002B5D RID: 11101
		JoinRoom
	}

	// Token: 0x02000AA0 RID: 2720
	public enum SubMode
	{
		// Token: 0x04002B5F RID: 11103
		AskUser,
		// Token: 0x04002B60 RID: 11104
		Host,
		// Token: 0x04002B61 RID: 11105
		Search
	}

	// Token: 0x02000AA1 RID: 2721
	// (Invoke) Token: 0x060035D9 RID: 13785
	public delegate void OnResults(SearchTask.SearchResultData results);
}
