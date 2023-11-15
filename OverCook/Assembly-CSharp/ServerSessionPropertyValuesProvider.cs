using System;
using System.Collections.Generic;
using Team17.Online;

// Token: 0x02000851 RID: 2129
public static class ServerSessionPropertyValuesProvider
{
	// Token: 0x06002909 RID: 10505 RVA: 0x000C070C File Offset: 0x000BEB0C
	public static bool Initialise(IOnlineMultiplayerSessionPropertyCoordinator propertyCoordinator)
	{
		if (!ServerSessionPropertyValuesProvider.m_bInit && propertyCoordinator != null)
		{
			if (propertyCoordinator.IsInitialized())
			{
				ServerSessionPropertyValuesProvider.m_versionPropertyValue.m_value = OnlineMultiplayerConfig.CodeVersion;
				ServerSessionPropertyValuesProvider.m_versionPropertyValue.m_property = propertyCoordinator.FindProperty(OnlineMultiplayerSessionPropertyId.eVersion);
				ServerSessionPropertyValuesProvider.m_sessionPropertyValues.Add(ServerSessionPropertyValuesProvider.m_versionPropertyValue);
				ServerSessionPropertyValuesProvider.m_gamemodePropertyValue.m_value = 4U;
				ServerSessionPropertyValuesProvider.m_gamemodePropertyValue.m_property = propertyCoordinator.FindProperty(OnlineMultiplayerSessionPropertyId.eGameMode);
				ServerSessionPropertyValuesProvider.m_sessionPropertyValues.Add(ServerSessionPropertyValuesProvider.m_gamemodePropertyValue);
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600290A RID: 10506 RVA: 0x000C0798 File Offset: 0x000BEB98
	public static void SetGameMode(GameMode mode)
	{
		if (!ServerSessionPropertyValuesProvider.m_bInit)
		{
			IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
			ServerSessionPropertyValuesProvider.m_bInit = ServerSessionPropertyValuesProvider.Initialise(onlinePlatformManager.OnlineMultiplayerSessionPropertyCoordinator());
		}
		ServerSessionPropertyValuesProvider.m_gamemodePropertyValue.m_value = (uint)mode;
	}

	// Token: 0x0600290B RID: 10507 RVA: 0x000C07D0 File Offset: 0x000BEBD0
	public static GameMode GetGameMode()
	{
		if (!ServerSessionPropertyValuesProvider.m_bInit)
		{
			IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
			ServerSessionPropertyValuesProvider.m_bInit = ServerSessionPropertyValuesProvider.Initialise(onlinePlatformManager.OnlineMultiplayerSessionPropertyCoordinator());
		}
		return (GameMode)ServerSessionPropertyValuesProvider.m_gamemodePropertyValue.m_value;
	}

	// Token: 0x0600290C RID: 10508 RVA: 0x000C0808 File Offset: 0x000BEC08
	public static List<OnlineMultiplayerSessionPropertyValue> GetValues()
	{
		if (!ServerSessionPropertyValuesProvider.m_bInit)
		{
			IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
			ServerSessionPropertyValuesProvider.m_bInit = ServerSessionPropertyValuesProvider.Initialise(onlinePlatformManager.OnlineMultiplayerSessionPropertyCoordinator());
		}
		return ServerSessionPropertyValuesProvider.m_sessionPropertyValues;
	}

	// Token: 0x0400207A RID: 8314
	private static bool m_bInit = false;

	// Token: 0x0400207B RID: 8315
	private static OnlineMultiplayerSessionPropertyValue m_versionPropertyValue = new OnlineMultiplayerSessionPropertyValue();

	// Token: 0x0400207C RID: 8316
	private static OnlineMultiplayerSessionPropertyValue m_gamemodePropertyValue = new OnlineMultiplayerSessionPropertyValue();

	// Token: 0x0400207D RID: 8317
	private static List<OnlineMultiplayerSessionPropertyValue> m_sessionPropertyValues = new List<OnlineMultiplayerSessionPropertyValue>(Enum.GetNames(typeof(OnlineMultiplayerSessionPropertyId)).Length);
}
