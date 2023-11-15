using System;
using System.Runtime.CompilerServices;
using Team17.Online;

// Token: 0x0200088C RID: 2188
public class NetworkErrorDialog
{
	// Token: 0x06002A7F RID: 10879 RVA: 0x000C7258 File Offset: 0x000C5658
	public void Enable(T17DialogBox.DialogEvent onNetworkErrorDialogDismissed)
	{
		this.m_LocalConfirmCallbacks = onNetworkErrorDialogDismissed;
		NetworkErrorDialog.OnNetworkErrorDialogDismissed = (T17DialogBox.DialogEvent)Delegate.Combine(NetworkErrorDialog.OnNetworkErrorDialogDismissed, this.m_LocalConfirmCallbacks);
		DisconnectionHandler.SessionConnectionLostEvent = (GenericVoid)Delegate.Combine(DisconnectionHandler.SessionConnectionLostEvent, new GenericVoid(this.OnSessionConnectionLost));
		DisconnectionHandler.ConnectionModeErrorEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>)Delegate.Combine(DisconnectionHandler.ConnectionModeErrorEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>(this.OnConnectionModeError));
		DisconnectionHandler.KickedFromSessionEvent = (GenericVoid)Delegate.Combine(DisconnectionHandler.KickedFromSessionEvent, new GenericVoid(this.OnKickedFromSession));
		DisconnectionHandler.LocalDisconnectionEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>)Delegate.Combine(DisconnectionHandler.LocalDisconnectionEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>(this.OnLocalDisconnection));
	}

	// Token: 0x06002A80 RID: 10880 RVA: 0x000C7308 File Offset: 0x000C5708
	public void Disable()
	{
		DisconnectionHandler.SessionConnectionLostEvent = (GenericVoid)Delegate.Remove(DisconnectionHandler.SessionConnectionLostEvent, new GenericVoid(this.OnSessionConnectionLost));
		DisconnectionHandler.ConnectionModeErrorEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>)Delegate.Remove(DisconnectionHandler.ConnectionModeErrorEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>(this.OnConnectionModeError));
		DisconnectionHandler.KickedFromSessionEvent = (GenericVoid)Delegate.Remove(DisconnectionHandler.KickedFromSessionEvent, new GenericVoid(this.OnKickedFromSession));
		DisconnectionHandler.LocalDisconnectionEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>)Delegate.Remove(DisconnectionHandler.LocalDisconnectionEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>(this.OnLocalDisconnection));
		NetworkErrorDialog.OnNetworkErrorDialogDismissed = (T17DialogBox.DialogEvent)Delegate.Remove(NetworkErrorDialog.OnNetworkErrorDialogDismissed, this.m_LocalConfirmCallbacks);
		this.m_LocalConfirmCallbacks = null;
	}

	// Token: 0x06002A81 RID: 10881 RVA: 0x000C73B6 File Offset: 0x000C57B6
	public void OnDestroy()
	{
		this.Disable();
	}

	// Token: 0x06002A82 RID: 10882 RVA: 0x000C73BE File Offset: 0x000C57BE
	private void OnSessionConnectionLost()
	{
		NetworkErrorDialog.ShowDialog(NetworkErrors.GetDisconnectionMessageText(OnlineMultiplayerSessionDisconnectionResult.eGeneric), true);
	}

	// Token: 0x06002A83 RID: 10883 RVA: 0x000C73CC File Offset: 0x000C57CC
	private void OnConnectionModeError(OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult> result)
	{
		NetworkErrorDialog.ShowDialog(result);
	}

	// Token: 0x06002A84 RID: 10884 RVA: 0x000C73D4 File Offset: 0x000C57D4
	private void OnKickedFromSession()
	{
		NetworkErrorDialog.ShowDialog(NetworkErrors.GetDisconnectionMessageText(OnlineMultiplayerSessionDisconnectionResult.eKicked), true);
	}

	// Token: 0x06002A85 RID: 10885 RVA: 0x000C73E2 File Offset: 0x000C57E2
	private void OnLocalDisconnection(OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult> result)
	{
		NetworkErrorDialog.ShowDialog(result);
	}

	// Token: 0x06002A86 RID: 10886 RVA: 0x000C73EA File Offset: 0x000C57EA
	public static void ShowDialog(OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult> result)
	{
		result.DisplayPlatformSpecificError(false);
		NetworkErrorDialog.ShowDialog(NetworkErrors.GetDisconnectionMessageText(result.m_returnCode), true);
	}

	// Token: 0x06002A87 RID: 10887 RVA: 0x000C7405 File Offset: 0x000C5805
	public static void ShowDialog(OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult> result)
	{
		result.DisplayPlatformSpecificError(false);
		NetworkErrorDialog.ShowDialog(NetworkErrors.GetDisconnectionMessageText(result.m_returnCode), true);
	}

	// Token: 0x06002A88 RID: 10888 RVA: 0x000C7420 File Offset: 0x000C5820
	public static void ShowDialog(IConnectionModeSwitchStatus status)
	{
		status.DisplayPlatformDialog();
		NetworkErrorDialog.ShowDialog(status.GetLocalisedResultDescription(), false);
	}

	// Token: 0x06002A89 RID: 10889 RVA: 0x000C7435 File Offset: 0x000C5835
	public static void ShowDialog(JoinSessionStatus joinStatus, bool bShowPlatformSpecificError)
	{
		if (bShowPlatformSpecificError)
		{
			joinStatus.DisplayPlatformDialog();
		}
		NetworkErrorDialog.ShowDialog(joinStatus.GetLocalisedResultDescription(), false);
	}

	// Token: 0x06002A8A RID: 10890 RVA: 0x000C7450 File Offset: 0x000C5850
	private static void ShowDialog(string message, bool localiseMessage)
	{
		if (NetworkErrorDialog.m_dialogBox == null)
		{
			NetworkErrorDialog.m_dialogBox = T17DialogBoxManager.GetDialog(false);
			if (NetworkErrorDialog.m_dialogBox != null)
			{
				T17DialogBox dialogBox = NetworkErrorDialog.m_dialogBox;
				string title = "Text.Warning";
				string confirmBtn = "Text.Button.Confirm";
				string declineBtn = null;
				string cancelBtn = null;
				dialogBox.Initialize(title, message, confirmBtn, declineBtn, cancelBtn, T17DialogBox.Symbols.Warning, true, localiseMessage, false);
				T17DialogBox dialogBox2 = NetworkErrorDialog.m_dialogBox;
				Delegate onConfirm = dialogBox2.OnConfirm;
				if (NetworkErrorDialog.<>f__mg$cache0 == null)
				{
					NetworkErrorDialog.<>f__mg$cache0 = new T17DialogBox.DialogEvent(NetworkErrorDialog.OnConfirmed);
				}
				dialogBox2.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(onConfirm, NetworkErrorDialog.<>f__mg$cache0);
				NetworkErrorDialog.m_dialogBox.Show();
			}
		}
	}

	// Token: 0x06002A8B RID: 10891 RVA: 0x000C74F4 File Offset: 0x000C58F4
	private static void OnConfirmed()
	{
		NetworkErrorDialog.m_dialogBox = null;
		if (NetworkErrorDialog.OnNetworkErrorDialogDismissed != null)
		{
			NetworkErrorDialog.OnNetworkErrorDialogDismissed();
		}
	}

	// Token: 0x04002183 RID: 8579
	private static T17DialogBox m_dialogBox;

	// Token: 0x04002184 RID: 8580
	private static T17DialogBox.DialogEvent OnNetworkErrorDialogDismissed;

	// Token: 0x04002185 RID: 8581
	private T17DialogBox.DialogEvent m_LocalConfirmCallbacks;

	// Token: 0x04002186 RID: 8582
	[CompilerGenerated]
	private static T17DialogBox.DialogEvent <>f__mg$cache0;
}
