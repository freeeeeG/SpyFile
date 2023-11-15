using System;
using System.Threading.Tasks;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000C1 RID: 193
	public class SteamIntegration : MonoBehaviour
	{
		// Token: 0x06000633 RID: 1587 RVA: 0x0001C890 File Offset: 0x0001AA90
		private void Awake()
		{
			if (SteamIntegration.Instance == null)
			{
				Object.DontDestroyOnLoad(base.gameObject);
				SteamIntegration.Instance = this;
				try
				{
					SteamClient.Init(1966900U, true);
					return;
				}
				catch (Exception message)
				{
					Debug.Log(message);
					return;
				}
			}
			if (SteamIntegration.Instance != null)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0001C8F8 File Offset: 0x0001AAF8
		private void Update()
		{
			SteamClient.RunCallbacks();
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001C8FF File Offset: 0x0001AAFF
		private void OnApplicationQuit()
		{
			SteamClient.Shutdown();
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001C908 File Offset: 0x0001AB08
		public static void UnlockAchievement(string id)
		{
			Achievement achievement = new Achievement(id);
			achievement.Trigger(true);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001C928 File Offset: 0x0001AB28
		public static void ClearAchievement(string id)
		{
			Achievement achievement = new Achievement(id);
			achievement.Clear();
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0001C948 File Offset: 0x0001AB48
		public async Task<Leaderboard> GetLeaderboardAsync(string leaderboardName)
		{
			Leaderboard result;
			try
			{
				result = (await SteamUserStats.FindOrCreateLeaderboardAsync(leaderboardName, LeaderboardSort.Descending, LeaderboardDisplay.Numeric)).Value;
			}
			catch (Exception message)
			{
				Debug.Log(message);
				result = default(Leaderboard);
			}
			return result;
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0001C98D File Offset: 0x0001AB8D
		private void ClearAll()
		{
			SteamIntegration.ClearAchievement("ACH_SURVIVE20");
			SteamIntegration.ClearAchievement("ACH_DARKNESS1");
			SteamIntegration.ClearAchievement("ACH_DARKNESS5");
			SteamIntegration.ClearAchievement("ACH_DARKNESS10");
			SteamIntegration.ClearAchievement("ACH_DARKNESS15");
			SteamIntegration.ClearAchievement("ACH_RECKLESS");
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001C9CC File Offset: 0x0001ABCC
		private async void SubmitScore(int score)
		{
			try
			{
				SteamClient.Init(1966900U, true);
			}
			catch (Exception)
			{
			}
			LeaderboardUpdate? leaderboardUpdate = await(await this.GetLeaderboardAsync("Endless Mode")).SubmitScoreAsync(score, null);
			Debug.Log(string.Concat(new object[]
			{
				"Submitted: ",
				leaderboardUpdate.Value.Changed.ToString(),
				", Score: ",
				leaderboardUpdate.Value.Score
			}));
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0001CA10 File Offset: 0x0001AC10
		private async void ReplaceScore(int score)
		{
			try
			{
				SteamClient.Init(1966900U, true);
			}
			catch (Exception)
			{
			}
			LeaderboardUpdate? leaderboardUpdate = await(await this.GetLeaderboardAsync("Endless Mode")).ReplaceScore(score, null);
			Debug.Log(string.Concat(new object[]
			{
				"Submitted: ",
				leaderboardUpdate.Value.Changed.ToString(),
				", Score: ",
				leaderboardUpdate.Value.Score
			}));
		}

		// Token: 0x04000400 RID: 1024
		public static SteamIntegration Instance;
	}
}
