using System;
using Steamworks;

// Token: 0x0200074E RID: 1870
public class SteamAchievementManager : AchievementManager
{
	// Token: 0x060023D7 RID: 9175 RVA: 0x000A7020 File Offset: 0x000A5420
	public override void Init()
	{
		this.m_appId = SteamUtils.GetAppID();
		this.m_userStatsRecievedResult = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnUserStatsReceived));
		this.m_userStatsStoredResult = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.OnUserStatsStored));
		this.m_achievementResult = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.OnAchievementStored));
		SteamUserStats.RequestCurrentStats();
	}

	// Token: 0x060023D8 RID: 9176 RVA: 0x000A7083 File Offset: 0x000A5483
	private void OnUserStatsReceived(UserStatsReceived_t param)
	{
	}

	// Token: 0x060023D9 RID: 9177 RVA: 0x000A7085 File Offset: 0x000A5485
	private void OnUserStatsStored(UserStatsStored_t param)
	{
	}

	// Token: 0x060023DA RID: 9178 RVA: 0x000A7087 File Offset: 0x000A5487
	private void OnAchievementStored(UserAchievementStored_t param)
	{
	}

	// Token: 0x060023DB RID: 9179 RVA: 0x000A708C File Offset: 0x000A548C
	protected override void Unlock(int trophyId)
	{
		string trophyApiName = this.m_StatSystem.GetTrophyApiName(trophyId);
		SteamUserStats.SetAchievement(trophyApiName);
		SteamUserStats.StoreStats();
	}

	// Token: 0x04001B73 RID: 7027
	private AppId_t m_appId;

	// Token: 0x04001B74 RID: 7028
	protected Callback<UserStatsReceived_t> m_userStatsRecievedResult;

	// Token: 0x04001B75 RID: 7029
	protected Callback<UserStatsStored_t> m_userStatsStoredResult;

	// Token: 0x04001B76 RID: 7030
	protected Callback<UserAchievementStored_t> m_achievementResult;
}
