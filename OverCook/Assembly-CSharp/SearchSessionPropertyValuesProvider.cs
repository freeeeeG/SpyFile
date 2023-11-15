using System;
using System.Collections.Generic;
using Team17.Online;

// Token: 0x02000850 RID: 2128
public static class SearchSessionPropertyValuesProvider
{
	// Token: 0x06002904 RID: 10500 RVA: 0x000C0544 File Offset: 0x000BE944
	public static bool Initialise(IOnlineMultiplayerSessionPropertyCoordinator sessionPropertyCoordinator)
	{
		if (!SearchSessionPropertyValuesProvider.m_bInit && sessionPropertyCoordinator != null)
		{
			if (sessionPropertyCoordinator.IsInitialized())
			{
				SearchSessionPropertyValuesProvider.m_versionPropertyValue.m_value = OnlineMultiplayerConfig.CodeVersion;
				SearchSessionPropertyValuesProvider.m_versionPropertyValue.m_valueMinRange = OnlineMultiplayerConfig.CodeVersion;
				SearchSessionPropertyValuesProvider.m_versionPropertyValue.m_valueMaxRange = OnlineMultiplayerConfig.CodeVersion;
				SearchSessionPropertyValuesProvider.m_versionPropertyValue.m_property = sessionPropertyCoordinator.FindProperty(OnlineMultiplayerSessionPropertyId.eVersion);
				SearchSessionPropertyValuesProvider.m_versionPropertyValue.m_operator = OnlineMultiplayerSessionPropertySearchValue.Operator.eEquals;
				SearchSessionPropertyValuesProvider.m_searchPropertyValues.Add(SearchSessionPropertyValuesProvider.m_versionPropertyValue);
				SearchSessionPropertyValuesProvider.m_gamemodePropertyValue.m_value = 4U;
				SearchSessionPropertyValuesProvider.m_gamemodePropertyValue.m_valueMinRange = 4U;
				SearchSessionPropertyValuesProvider.m_gamemodePropertyValue.m_valueMaxRange = 4U;
				SearchSessionPropertyValuesProvider.m_gamemodePropertyValue.m_property = sessionPropertyCoordinator.FindProperty(OnlineMultiplayerSessionPropertyId.eGameMode);
				SearchSessionPropertyValuesProvider.m_gamemodePropertyValue.m_operator = OnlineMultiplayerSessionPropertySearchValue.Operator.eEquals;
				SearchSessionPropertyValuesProvider.m_searchPropertyValues.Add(SearchSessionPropertyValuesProvider.m_gamemodePropertyValue);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002905 RID: 10501 RVA: 0x000C0618 File Offset: 0x000BEA18
	public static void SetGameMode(GameMode mode)
	{
		if (!SearchSessionPropertyValuesProvider.m_bInit)
		{
			IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
			SearchSessionPropertyValuesProvider.m_bInit = SearchSessionPropertyValuesProvider.Initialise(onlinePlatformManager.OnlineMultiplayerSessionPropertyCoordinator());
		}
		SearchSessionPropertyValuesProvider.m_gamemodePropertyValue.m_value = (uint)mode;
		SearchSessionPropertyValuesProvider.m_gamemodePropertyValue.m_valueMinRange = (uint)mode;
		SearchSessionPropertyValuesProvider.m_gamemodePropertyValue.m_valueMaxRange = (uint)mode;
	}

	// Token: 0x06002906 RID: 10502 RVA: 0x000C0668 File Offset: 0x000BEA68
	public static GameMode GetGameMode()
	{
		if (!SearchSessionPropertyValuesProvider.m_bInit)
		{
			IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
			SearchSessionPropertyValuesProvider.m_bInit = SearchSessionPropertyValuesProvider.Initialise(onlinePlatformManager.OnlineMultiplayerSessionPropertyCoordinator());
		}
		return (GameMode)SearchSessionPropertyValuesProvider.m_gamemodePropertyValue.m_value;
	}

	// Token: 0x06002907 RID: 10503 RVA: 0x000C06A0 File Offset: 0x000BEAA0
	public static List<OnlineMultiplayerSessionPropertySearchValue> GetValues()
	{
		if (!SearchSessionPropertyValuesProvider.m_bInit)
		{
			IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
			SearchSessionPropertyValuesProvider.m_bInit = SearchSessionPropertyValuesProvider.Initialise(onlinePlatformManager.OnlineMultiplayerSessionPropertyCoordinator());
		}
		return SearchSessionPropertyValuesProvider.m_searchPropertyValues;
	}

	// Token: 0x04002076 RID: 8310
	private static bool m_bInit = false;

	// Token: 0x04002077 RID: 8311
	private static OnlineMultiplayerSessionPropertySearchValue m_versionPropertyValue = new OnlineMultiplayerSessionPropertySearchValue();

	// Token: 0x04002078 RID: 8312
	private static OnlineMultiplayerSessionPropertySearchValue m_gamemodePropertyValue = new OnlineMultiplayerSessionPropertySearchValue();

	// Token: 0x04002079 RID: 8313
	private static List<OnlineMultiplayerSessionPropertySearchValue> m_searchPropertyValues = new List<OnlineMultiplayerSessionPropertySearchValue>(Enum.GetNames(typeof(OnlineMultiplayerSessionPropertyId)).Length);
}
