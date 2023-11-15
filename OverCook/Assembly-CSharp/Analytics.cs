using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Team17.Online;
using UnityEngine;

// Token: 0x020000F3 RID: 243
public class Analytics : Manager
{
	// Token: 0x06000492 RID: 1170 RVA: 0x000276A3 File Offset: 0x00025AA3
	private void Awake()
	{
		this.StartupAnalytics();
		Analytics.StartupT17Analytics();
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x000276B0 File Offset: 0x00025AB0
	private void OnDestroy()
	{
		this.ShutdownAnalytics();
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x000276B8 File Offset: 0x00025AB8
	private void StartupAnalytics()
	{
		if (!string.IsNullOrEmpty("Analytics/GAv3_live"))
		{
			GameObject gameObject = null;
			if (this.m_AnalyticsPrefabs != null)
			{
				int i = 0;
				int num = this.m_AnalyticsPrefabs.Length;
				while (i < num)
				{
					if ("Analytics/GAv3_live".EndsWith(this.m_AnalyticsPrefabs[i].name))
					{
						gameObject = this.m_AnalyticsPrefabs[i];
						break;
					}
					i++;
				}
			}
			else
			{
				gameObject = (Resources.Load("Analytics/GAv3_live") as GameObject);
			}
			if (gameObject == null)
			{
				return;
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			if (gameObject2 == null)
			{
				return;
			}
			gameObject2.transform.parent = base.transform;
			this.m_GoogleAnalytics = gameObject2.GetComponent<GoogleAnalyticsV3>();
			if (this.m_GoogleAnalytics != null)
			{
				string str = "UNKNOWN";
				IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
				if (onlinePlatformManager != null)
				{
					str = onlinePlatformManager.Name();
				}
				this.m_GoogleAnalytics.bundleVersion = str + "." + BuildVersion.m_VersionString;
				this.m_GoogleAnalytics.SetAppLevelOptOut(false);
				GoogleAnalyticsV3 googleAnalytics = this.m_GoogleAnalytics;
				if (Analytics.<>f__mg$cache0 == null)
				{
					Analytics.<>f__mg$cache0 = new GoogleAnalyticsV3.GetUserData(GameUtils.DebugGetState);
				}
				googleAnalytics.getUserData = Analytics.<>f__mg$cache0;
				this.m_GoogleAnalytics.Initialize();
				this.m_GoogleAnalytics.StartSession();
			}
		}
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x0002780E File Offset: 0x00025C0E
	private void ShutdownAnalytics()
	{
		if (this.m_GoogleAnalytics != null)
		{
			this.m_GoogleAnalytics.StopSession();
			this.m_GoogleAnalytics = null;
		}
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x00027833 File Offset: 0x00025C33
	[Conditional("ANALYTICS")]
	public void LogAnException(string log, string stackTrace, string userData)
	{
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x00027835 File Offset: 0x00025C35
	[Conditional("ANALYTICS")]
	private void LogEventInternal(string category, string action, string label, long value)
	{
		if (this.m_GoogleAnalytics != null)
		{
			this.m_GoogleAnalytics.LogEvent(category, action, label, value);
		}
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00027858 File Offset: 0x00025C58
	[Conditional("ANALYTICS")]
	public static void LogEvent(string category, string action, string label, long value)
	{
		Analytics analytics = GameUtils.RequestManager<Analytics>();
		if (analytics)
		{
			analytics.LogEventInternal(category, action, label, value);
		}
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x00027880 File Offset: 0x00025C80
	private static string GetCurrentLevelName()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		if (gameSession != null)
		{
			SceneDirectoryData.PerPlayerCountDirectoryEntry sceneDirectoryVarientEntry = gameSession.LevelSettings.SceneDirectoryVarientEntry;
			if (sceneDirectoryVarientEntry != null)
			{
				return sceneDirectoryVarientEntry.SceneName;
			}
		}
		return "<Unknown Level>";
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x000278BD File Offset: 0x00025CBD
	private static bool IsServer()
	{
		return ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession();
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x000278D4 File Offset: 0x00025CD4
	private static GameMode GetCurrentGameMode()
	{
		return (!Analytics.IsServer()) ? ClientGameSetup.Mode : ServerGameSetup.Mode;
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x000278EF File Offset: 0x00025CEF
	private static int GetCurrentPlayerCount()
	{
		return (!Analytics.IsServer()) ? ClientUserSystem.m_Users.Count : ServerUserSystem.m_Users.Count;
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x00027914 File Offset: 0x00025D14
	private static void ProcessFlags(ref string category, ref string action, ref string label, ref long value, Analytics.Flags flags)
	{
		if ((flags & Analytics.Flags.LevelName) != (Analytics.Flags)0)
		{
			action = ((!string.IsNullOrEmpty(action)) ? (action + " ") : string.Empty) + Analytics.GetCurrentLevelName();
		}
		if ((flags & Analytics.Flags.PlayerCount) != (Analytics.Flags)0)
		{
			label = ((!string.IsNullOrEmpty(label)) ? (label + " ") : string.Empty) + Analytics.GetCurrentPlayerCount() + " Player";
		}
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x0002799C File Offset: 0x00025D9C
	[Conditional("ANALYTICS")]
	public static void LogEvent(string category, string action, long value = 0L, Analytics.Flags flags = (Analytics.Flags)0)
	{
		string label = (!ConnectionStatus.IsInSession()) ? "Offline" : "Online";
		Analytics.ProcessFlags(ref category, ref action, ref label, ref value, flags);
		Analytics.LogEvent(category, action, label, value);
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x000279DC File Offset: 0x00025DDC
	[Conditional("ANALYTICS")]
	public static void LogEvent(string action, long value = 0L, Analytics.Flags flags = (Analytics.Flags)0)
	{
		string category = Analytics.GetCurrentGameMode().ToString();
		string label = (!ConnectionStatus.IsInSession()) ? "Offline" : "Online";
		Analytics.ProcessFlags(ref category, ref action, ref label, ref value, flags);
		Analytics.LogEvent(category, action, label, value);
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x00027A30 File Offset: 0x00025E30
	public static void StartupT17Analytics()
	{
		if (GameUtils.GetT17AnalyticsHelper() == null)
		{
			string text = Environment.MachineName;
			if (string.IsNullOrEmpty(text))
			{
				text = "noname";
			}
			GameUtils.InitialiseAnalytics("https://api.gameanalytics.com", "dce9689b8a79396089a774e5b0efdc89", "38fdf3b708b9f478dc890bd7566d23706a2ebea6", text, BuildVersion.m_VersionString);
			string eventName = "Steam";
			GameUtils.SendDiagnosticEvent(eventName);
		}
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x00027A8A File Offset: 0x00025E8A
	public static void ShutdownT17Analytics()
	{
		GameUtils.ShutdownAnalytics();
	}

	// Token: 0x04000407 RID: 1031
	[SerializeField]
	private GameObject[] m_AnalyticsPrefabs;

	// Token: 0x04000408 RID: 1032
	private const string m_googleAnalyticsPrefabName = "Analytics/GAv3_live";

	// Token: 0x04000409 RID: 1033
	private GoogleAnalyticsV3 m_GoogleAnalytics;

	// Token: 0x0400040A RID: 1034
	[CompilerGenerated]
	private static GoogleAnalyticsV3.GetUserData <>f__mg$cache0;

	// Token: 0x020000F4 RID: 244
	[Flags]
	public enum Flags
	{
		// Token: 0x0400040C RID: 1036
		LevelName = 1,
		// Token: 0x0400040D RID: 1037
		PlayerCount = 2
	}
}
