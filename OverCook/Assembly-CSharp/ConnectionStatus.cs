using System;
using Team17.Online;

// Token: 0x02000882 RID: 2178
internal class ConnectionStatus
{
	// Token: 0x06002A3A RID: 10810 RVA: 0x000C543C File Offset: 0x000C383C
	public void Initialise()
	{
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		ConnectionStatus.m_iOnlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		ConnectionStatus.m_connectionModeCoordinator = onlinePlatformManager.OnlineMultiplayerConnectionModeCoordinator();
	}

	// Token: 0x06002A3B RID: 10811 RVA: 0x000C5465 File Offset: 0x000C3865
	public void Shutdown()
	{
		ConnectionStatus.m_iOnlineMultiplayerSessionCoordinator = null;
	}

	// Token: 0x06002A3C RID: 10812 RVA: 0x000C546D File Offset: 0x000C386D
	public static bool IsInSession()
	{
		return ConnectionStatus.m_iOnlineMultiplayerSessionCoordinator != null && null != ConnectionStatus.m_iOnlineMultiplayerSessionCoordinator.Members();
	}

	// Token: 0x06002A3D RID: 10813 RVA: 0x000C548C File Offset: 0x000C388C
	public static bool IsHost()
	{
		return ConnectionStatus.m_iOnlineMultiplayerSessionCoordinator != null && ConnectionStatus.m_iOnlineMultiplayerSessionCoordinator.IsHost();
	}

	// Token: 0x06002A3E RID: 10814 RVA: 0x000C54A8 File Offset: 0x000C38A8
	public static OnlineMultiplayerConnectionMode CurrentConnectionMode()
	{
		OnlineMultiplayerConnectionMode result = OnlineMultiplayerConnectionMode.eNone;
		if (ConnectionStatus.m_connectionModeCoordinator != null)
		{
			result = ConnectionStatus.m_connectionModeCoordinator.Mode();
		}
		return result;
	}

	// Token: 0x06002A3F RID: 10815 RVA: 0x000C54CD File Offset: 0x000C38CD
	public static bool HasConnectionModes()
	{
		return ConnectionStatus.m_connectionModeCoordinator != null;
	}

	// Token: 0x04002142 RID: 8514
	private static IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator;

	// Token: 0x04002143 RID: 8515
	private static IOnlineMultiplayerConnectionModeCoordinator m_connectionModeCoordinator;
}
