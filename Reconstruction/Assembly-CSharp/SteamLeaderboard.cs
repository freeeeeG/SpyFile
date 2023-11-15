using System;
using System.Collections.Generic;
using System.Threading;
using Steamworks;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class SteamLeaderboard : MonoBehaviour
{
	// Token: 0x06000883 RID: 2179 RVA: 0x00016E48 File Offset: 0x00015048
	public static void UpdateScore(int score)
	{
		if (!SteamLeaderboard.s_initialized)
		{
			Debug.Log("Can't upload to the leaderboard because isn't loadded yet");
			return;
		}
		Debug.Log("uploading score(" + score.ToString() + ") to steam leaderboard(EndlessMode)");
		SteamAPICall_t hAPICall = SteamUserStats.UploadLeaderboardScore(SteamLeaderboard.s_currentLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, score, null, 0);
		SteamLeaderboard.m_uploadResult.Set(hAPICall, new CallResult<LeaderboardScoreUploaded_t>.APIDispatchDelegate(SteamLeaderboard.OnLeaderboardUploadResult));
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x00016EA8 File Offset: 0x000150A8
	public static void DownloadScore()
	{
		if (!SteamLeaderboard.s_initialized)
		{
			Debug.Log("Can't download the leaderboard because isn't loadded yet");
			return;
		}
		Debug.Log("download(" + 50.ToString() + ") to steam leaderboard(EndlessMode)");
		SteamLeaderboard.EndlessLeaderBoard.Clear();
		SteamAPICall_t hAPICall = SteamUserStats.DownloadLeaderboardEntries(SteamLeaderboard.s_currentLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 50);
		SteamLeaderboard.m_downloadResult.Set(hAPICall, new CallResult<LeaderboardScoresDownloaded_t>.APIDispatchDelegate(SteamLeaderboard.OnLeaderboardDownloadResult));
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x00016F18 File Offset: 0x00015118
	public static void Init()
	{
		SteamLeaderboard.EndlessLeaderBoard.Clear();
		SteamAPICall_t hAPICall = SteamUserStats.FindLeaderboard("EndlessMode");
		SteamLeaderboard.m_findResult.Set(hAPICall, new CallResult<LeaderboardFindResult_t>.APIDispatchDelegate(SteamLeaderboard.OnLeaderboardFindResult));
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x00016F54 File Offset: 0x00015154
	private static void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool failure)
	{
		Debug.Log("STEAM LEADERBOARDS: Found - " + pCallback.m_bLeaderboardFound.ToString() + " leaderboardID - " + pCallback.m_hSteamLeaderboard.m_SteamLeaderboard.ToString());
		SteamLeaderboard.s_currentLeaderboard = pCallback.m_hSteamLeaderboard;
		SteamLeaderboard.s_initialized = true;
		SteamLeaderboard.DownloadScore();
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x00016FA8 File Offset: 0x000151A8
	private static void OnLeaderboardUploadResult(LeaderboardScoreUploaded_t pCallback, bool failure)
	{
		Debug.Log(string.Concat(new string[]
		{
			"STEAM LEADERBOARDS: failure - ",
			failure.ToString(),
			" Completed - ",
			pCallback.m_bSuccess.ToString(),
			" NewScore: ",
			pCallback.m_nGlobalRankNew.ToString(),
			" Score ",
			pCallback.m_nScore.ToString(),
			" HasChanged - ",
			pCallback.m_bScoreChanged.ToString()
		}));
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x00017038 File Offset: 0x00015238
	private static void OnLeaderboardDownloadResult(LeaderboardScoresDownloaded_t pCallback, bool failure)
	{
		Debug.Log("STEAM LEADERBOARDS: failure - " + failure.ToString() + " Completed - " + pCallback.m_cEntryCount.ToString());
		if (!failure)
		{
			int num = Mathf.Min(pCallback.m_cEntryCount, 50);
			for (int i = 0; i < num; i++)
			{
				LeaderboardEntry_t item;
				SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, i, out item, null, 0);
				SteamLeaderboard.EndlessLeaderBoard.Add(item);
			}
		}
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x000170A5 File Offset: 0x000152A5
	public static void InitTimer()
	{
		SteamLeaderboard.timer1 = new Timer(new TimerCallback(SteamLeaderboard.timer1_Tick), null, 0, 1000);
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x000170C4 File Offset: 0x000152C4
	private static void timer1_Tick(object state)
	{
		SteamAPI.RunCallbacks();
		Debug.Log("Yes");
	}

	// Token: 0x0400046D RID: 1133
	private const string s_leaderboardName = "EndlessMode";

	// Token: 0x0400046E RID: 1134
	private const ELeaderboardUploadScoreMethod s_leaderboardMethod = ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest;

	// Token: 0x0400046F RID: 1135
	private static SteamLeaderboard_t s_currentLeaderboard;

	// Token: 0x04000470 RID: 1136
	private static bool s_initialized = false;

	// Token: 0x04000471 RID: 1137
	private static CallResult<LeaderboardFindResult_t> m_findResult = new CallResult<LeaderboardFindResult_t>(null);

	// Token: 0x04000472 RID: 1138
	private static CallResult<LeaderboardScoreUploaded_t> m_uploadResult = new CallResult<LeaderboardScoreUploaded_t>(null);

	// Token: 0x04000473 RID: 1139
	private static CallResult<LeaderboardScoresDownloaded_t> m_downloadResult = new CallResult<LeaderboardScoresDownloaded_t>(null);

	// Token: 0x04000474 RID: 1140
	public static List<LeaderboardEntry_t> EndlessLeaderBoard = new List<LeaderboardEntry_t>();

	// Token: 0x04000475 RID: 1141
	private static Timer timer1;
}
