using System;
using Team17.Online;

// Token: 0x0200088B RID: 2187
public static class NetworkErrors
{
	// Token: 0x06002A7B RID: 10875 RVA: 0x000C71B4 File Offset: 0x000C55B4
	public static string GetDisconnectionMessageText(OnlineMultiplayerSessionDisconnectionResult reason)
	{
		string result = string.Empty;
		switch (reason)
		{
		case OnlineMultiplayerSessionDisconnectionResult.eGeneric:
			result = "Online.Disconnection.Generic";
			break;
		case OnlineMultiplayerSessionDisconnectionResult.eLostNetwork:
			result = "Online.Disconnection.LostNetwork";
			break;
		case OnlineMultiplayerSessionDisconnectionResult.eGoneOffline:
			result = "Online.Disconnection.GoneOffline";
			break;
		case OnlineMultiplayerSessionDisconnectionResult.eLoggedOut:
			result = "Online.Disconnection.LoggedOut";
			break;
		case OnlineMultiplayerSessionDisconnectionResult.eKicked:
			result = "Online.Disconnection.Kicked";
			break;
		case OnlineMultiplayerSessionDisconnectionResult.eApplicationSuspended:
			result = "Online.Disconnection.ApplicationSuspended";
			break;
		case OnlineMultiplayerSessionDisconnectionResult.eHostDisconnected:
			result = "Online.Disconnection.HostDisconnected";
			break;
		default:
			result = "Online.Disconnection.Generic";
			break;
		}
		return result;
	}

	// Token: 0x06002A7C RID: 10876 RVA: 0x000C7247 File Offset: 0x000C5647
	public static string GetDisconnectionMessageText(OnlineMultiplayerConnectionModeErrorResult reason)
	{
		return "Online.Disconnection.ConnectionMode";
	}

	// Token: 0x04002181 RID: 8577
	public static string CachedErrorTitle;

	// Token: 0x04002182 RID: 8578
	public static string CachedErrorMessage;
}
