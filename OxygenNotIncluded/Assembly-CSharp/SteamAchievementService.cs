using System;
using System.Diagnostics;
using Steamworks;
using UnityEngine;

// Token: 0x02000A3B RID: 2619
public class SteamAchievementService : MonoBehaviour
{
	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x06004ED7 RID: 20183 RVA: 0x001BCA01 File Offset: 0x001BAC01
	public static SteamAchievementService Instance
	{
		get
		{
			return SteamAchievementService.instance;
		}
	}

	// Token: 0x06004ED8 RID: 20184 RVA: 0x001BCA08 File Offset: 0x001BAC08
	public static void Initialize()
	{
		if (SteamAchievementService.instance == null)
		{
			GameObject gameObject = GameObject.Find("/SteamManager");
			SteamAchievementService.instance = gameObject.GetComponent<SteamAchievementService>();
			if (SteamAchievementService.instance == null)
			{
				SteamAchievementService.instance = gameObject.AddComponent<SteamAchievementService>();
			}
		}
	}

	// Token: 0x06004ED9 RID: 20185 RVA: 0x001BCA50 File Offset: 0x001BAC50
	public void Awake()
	{
		this.setupComplete = false;
		global::Debug.Assert(SteamAchievementService.instance == null);
		SteamAchievementService.instance = this;
	}

	// Token: 0x06004EDA RID: 20186 RVA: 0x001BCA6F File Offset: 0x001BAC6F
	private void OnDestroy()
	{
		global::Debug.Assert(SteamAchievementService.instance == this);
		SteamAchievementService.instance = null;
	}

	// Token: 0x06004EDB RID: 20187 RVA: 0x001BCA87 File Offset: 0x001BAC87
	private void Update()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (Game.Instance != null)
		{
			return;
		}
		if (!this.setupComplete && DistributionPlatform.Initialized)
		{
			this.Setup();
		}
	}

	// Token: 0x06004EDC RID: 20188 RVA: 0x001BCAB4 File Offset: 0x001BACB4
	private void Setup()
	{
		this.cbUserStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnUserStatsReceived));
		this.cbUserStatsStored = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.OnUserStatsStored));
		this.cbUserAchievementStored = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.OnUserAchievementStored));
		this.setupComplete = true;
		this.RefreshStats();
	}

	// Token: 0x06004EDD RID: 20189 RVA: 0x001BCB13 File Offset: 0x001BAD13
	private void RefreshStats()
	{
		SteamUserStats.RequestCurrentStats();
	}

	// Token: 0x06004EDE RID: 20190 RVA: 0x001BCB1B File Offset: 0x001BAD1B
	private void OnUserStatsReceived(UserStatsReceived_t data)
	{
		if (data.m_eResult != EResult.k_EResultOK)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"OnUserStatsReceived",
				data.m_eResult,
				data.m_steamIDUser
			});
			return;
		}
	}

	// Token: 0x06004EDF RID: 20191 RVA: 0x001BCB56 File Offset: 0x001BAD56
	private void OnUserStatsStored(UserStatsStored_t data)
	{
		if (data.m_eResult != EResult.k_EResultOK)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"OnUserStatsStored",
				data.m_eResult
			});
			return;
		}
	}

	// Token: 0x06004EE0 RID: 20192 RVA: 0x001BCB83 File Offset: 0x001BAD83
	private void OnUserAchievementStored(UserAchievementStored_t data)
	{
	}

	// Token: 0x06004EE1 RID: 20193 RVA: 0x001BCB88 File Offset: 0x001BAD88
	public void Unlock(string achievement_id)
	{
		bool flag = SteamUserStats.SetAchievement(achievement_id);
		global::Debug.LogFormat("SetAchievement {0} {1}", new object[]
		{
			achievement_id,
			flag
		});
		bool flag2 = SteamUserStats.StoreStats();
		global::Debug.LogFormat("StoreStats {0}", new object[]
		{
			flag2
		});
	}

	// Token: 0x06004EE2 RID: 20194 RVA: 0x001BCBD8 File Offset: 0x001BADD8
	[Conditional("UNITY_EDITOR")]
	[ContextMenu("Reset All Achievements")]
	private void ResetAllAchievements()
	{
		bool flag = SteamUserStats.ResetAllStats(true);
		global::Debug.LogFormat("ResetAllStats {0}", new object[]
		{
			flag
		});
		if (flag)
		{
			this.RefreshStats();
		}
	}

	// Token: 0x04003347 RID: 13127
	private Callback<UserStatsReceived_t> cbUserStatsReceived;

	// Token: 0x04003348 RID: 13128
	private Callback<UserStatsStored_t> cbUserStatsStored;

	// Token: 0x04003349 RID: 13129
	private Callback<UserAchievementStored_t> cbUserAchievementStored;

	// Token: 0x0400334A RID: 13130
	private bool setupComplete;

	// Token: 0x0400334B RID: 13131
	private static SteamAchievementService instance;
}
