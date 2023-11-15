using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using Steamworks;
using UnityEngine;

// Token: 0x0200017A RID: 378
public class PlayfabManager : Singleton<PlayfabManager>
{
	// Token: 0x17000361 RID: 865
	// (get) Token: 0x0600098B RID: 2443 RVA: 0x0001949F File Offset: 0x0001769F
	// (set) Token: 0x0600098C RID: 2444 RVA: 0x000194AC File Offset: 0x000176AC
	public int LocalEndlessVersion
	{
		get
		{
			return PlayerPrefs.GetInt("EndlessDayVersion", 0);
		}
		set
		{
			PlayerPrefs.SetInt("EndlessDayVersion", value);
		}
	}

	// Token: 0x17000362 RID: 866
	// (get) Token: 0x0600098D RID: 2445 RVA: 0x000194B9 File Offset: 0x000176B9
	// (set) Token: 0x0600098E RID: 2446 RVA: 0x000194C6 File Offset: 0x000176C6
	public int EndlessWave
	{
		get
		{
			return PlayerPrefs.GetInt("EndlessDay", 0);
		}
		set
		{
			if (value > PlayerPrefs.GetInt("EndlessDay", 0))
			{
				PlayerPrefs.SetInt("EndlessDay", value);
			}
		}
	}

	// Token: 0x17000363 RID: 867
	// (get) Token: 0x0600098F RID: 2447 RVA: 0x000194E1 File Offset: 0x000176E1
	// (set) Token: 0x06000990 RID: 2448 RVA: 0x000194EE File Offset: 0x000176EE
	public int LocalChallengeVersion
	{
		get
		{
			return PlayerPrefs.GetInt("Challenge_DailyVersion", 0);
		}
		set
		{
			PlayerPrefs.SetInt("Challenge_DailyVersion", value);
		}
	}

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x06000991 RID: 2449 RVA: 0x000194FB File Offset: 0x000176FB
	// (set) Token: 0x06000992 RID: 2450 RVA: 0x00019508 File Offset: 0x00017708
	public int ChallngeScore
	{
		get
		{
			return PlayerPrefs.GetInt("Challenge_Daily", 0);
		}
		set
		{
			if (value > PlayerPrefs.GetInt("Challenge_Daily", 0))
			{
				PlayerPrefs.SetInt("Challenge_Daily", value);
			}
		}
	}

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x06000993 RID: 2451 RVA: 0x00019523 File Offset: 0x00017723
	// (set) Token: 0x06000994 RID: 2452 RVA: 0x0001952B File Offset: 0x0001772B
	public bool EndlessLeaderboardGot { get; set; }

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x06000995 RID: 2453 RVA: 0x00019534 File Offset: 0x00017734
	// (set) Token: 0x06000996 RID: 2454 RVA: 0x0001953C File Offset: 0x0001773C
	public bool ChallengeLeaderboardGot { get; set; }

	// Token: 0x06000997 RID: 2455 RVA: 0x00019548 File Offset: 0x00017748
	public void Login()
	{
		if (!SteamManager.Initialized)
		{
			Debug.Log("Steam is not initilized");
			return;
		}
		if (this.FirstLogin)
		{
			PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
			{
				CustomId = SteamFriends.GetPersonaName(),
				CreateAccount = new bool?(true),
				InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
				{
					GetPlayerProfile = true
				}
			}, new Action<LoginResult>(this.OnLoginSuccess), new Action<PlayFabError>(this.OnError), null, null);
			return;
		}
		this.UpdateLoacalScore();
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x000195C3 File Offset: 0x000177C3
	private void OnLoginSuccess(LoginResult result)
	{
		Debug.Log("Successfully Login" + result.PlayFabId);
		this.SetPlayerDisplayName(result);
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x000195E4 File Offset: 0x000177E4
	private void SetPlayerDisplayName(LoginResult result)
	{
		if (result.InfoResultPayload.PlayerProfile != null)
		{
			if (result.InfoResultPayload.PlayerProfile.DisplayName == null)
			{
				PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
				{
					DisplayName = SteamFriends.GetPersonaName()
				}, new Action<UpdateUserTitleDisplayNameResult>(this.OnDisplayNameUpdate), new Action<PlayFabError>(this.OnError), null, null);
				return;
			}
			this.GetEndlessVersion();
			this.GetChallengeVersion();
		}
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x0001964C File Offset: 0x0001784C
	private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult obj)
	{
		this.GetEndlessVersion();
		this.GetChallengeVersion();
		Debug.Log("Update display name");
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x00019664 File Offset: 0x00017864
	private void OnError(PlayFabError error)
	{
		Debug.Log(error.Error);
		Debug.Log(error.GenerateErrorReport());
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x00019684 File Offset: 0x00017884
	public void SendLeaderBoard(string leaderBoard, int score)
	{
		PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
		{
			Statistics = new List<StatisticUpdate>
			{
				new StatisticUpdate
				{
					StatisticName = leaderBoard,
					Value = score
				}
			}
		}, new Action<UpdatePlayerStatisticsResult>(this.OnLeaderboardUpdate), new Action<PlayFabError>(this.OnError), null, null);
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x000196D9 File Offset: 0x000178D9
	private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult obj)
	{
		Debug.Log("Successful Leader board Update");
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x000196E5 File Offset: 0x000178E5
	public void GetEndlessVersion()
	{
		this.EndlessLeaderboardGot = false;
		PlayFabClientAPI.GetPlayerStatisticVersions(new GetPlayerStatisticVersionsRequest
		{
			StatisticName = "EndlessDay"
		}, new Action<GetPlayerStatisticVersionsResult>(this.OnEndlessVersionGet), new Action<PlayFabError>(this.OnError), null, null);
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x0001971D File Offset: 0x0001791D
	public void GetChallengeVersion()
	{
		this.ChallengeLeaderboardGot = false;
		PlayFabClientAPI.GetPlayerStatisticVersions(new GetPlayerStatisticVersionsRequest
		{
			StatisticName = "Challenge_Daily"
		}, new Action<GetPlayerStatisticVersionsResult>(this.OnChallengeVersionGet), new Action<PlayFabError>(this.OnError), null, null);
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x00019758 File Offset: 0x00017958
	private void OnEndlessVersionGet(GetPlayerStatisticVersionsResult result)
	{
		this.OnlineEndlessVersion = (int)result.StatisticVersions[result.StatisticVersions.Count - 1].Version;
		this.GetLeaderBoard("EndlessDay", this.OnlineEndlessVersion);
		if (this.OnlineEndlessVersion > 0)
		{
			this.GetLeaderBoard("EndlessDay", this.OnlineEndlessVersion - 1);
		}
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x000197B8 File Offset: 0x000179B8
	private void OnChallengeVersionGet(GetPlayerStatisticVersionsResult result)
	{
		this.OnlineChallengeVersion = (int)result.StatisticVersions[result.StatisticVersions.Count - 1].Version;
		this.GetLeaderBoard("Challenge_Daily", this.OnlineChallengeVersion);
		if (this.OnlineChallengeVersion > 0)
		{
			this.GetLeaderBoard("Challenge_Daily", this.OnlineChallengeVersion - 1);
		}
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x00019818 File Offset: 0x00017A18
	private void GetLeaderBoard(string leaderBoardName, int version)
	{
		if (leaderBoardName == "EndlessDay")
		{
			PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
			{
				StatisticName = leaderBoardName,
				StartPosition = 0,
				MaxResultsCount = new int?(50),
				Version = new int?(version)
			}, new Action<GetLeaderboardResult>(this.OnEndlessLeaderBoardGet), new Action<PlayFabError>(this.OnError), null, null);
			return;
		}
		if (!(leaderBoardName == "Challenge_Daily"))
		{
			return;
		}
		PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
		{
			StatisticName = leaderBoardName,
			StartPosition = 0,
			MaxResultsCount = new int?(50),
			Version = new int?(version)
		}, new Action<GetLeaderboardResult>(this.OnChallengeLeaderBoardGet), new Action<PlayFabError>(this.OnError), null, null);
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x000198D8 File Offset: 0x00017AD8
	private void OnChallengeLeaderBoardGet(GetLeaderboardResult result)
	{
		this.ChallengeResults[this.OnlineChallengeVersion - result.Version].LeaderBoardResult = result;
		this.ChallengeLeaderboardGot = true;
		Singleton<GameEvents>.Instance.ChallengeLeaderboardGet(true);
		if (this.FirstLogin)
		{
			this.UpdateLoacalScore();
			this.FirstLogin = false;
		}
		Singleton<GameEvents>.Instance.ChallengeLeaderboardGet(true);
		Debug.Log("Successfully get leaderboard：Challenge_Daily");
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x00019940 File Offset: 0x00017B40
	private void OnEndlessLeaderBoardGet(GetLeaderboardResult result)
	{
		this.EndlessResult[this.OnlineEndlessVersion - result.Version].LeaderBoardResult = result;
		this.EndlessLeaderboardGot = true;
		if (this.FirstLogin)
		{
			this.UpdateLoacalScore();
			this.FirstLogin = false;
		}
		Singleton<GameEvents>.Instance.EndlessLeaderboardGet(true);
		Debug.Log("Successfully get leaderboard：EndlessDay");
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x0001999C File Offset: 0x00017B9C
	public void UpdateLoacalScore()
	{
		if (this.OnlineEndlessVersion == this.LocalEndlessVersion)
		{
			if (this.EndlessWave > 0)
			{
				this.SendLeaderBoard("EndlessDay", this.EndlessWave);
			}
		}
		else
		{
			this.LocalEndlessVersion = this.OnlineEndlessVersion;
			PlayerPrefs.SetInt("EndlessDay", 0);
		}
		if (this.OnlineChallengeVersion == this.LocalChallengeVersion)
		{
			if (this.ChallngeScore > 0)
			{
				this.SendLeaderBoard("Challenge_Daily", this.ChallngeScore);
				return;
			}
		}
		else
		{
			this.LocalChallengeVersion = this.OnlineChallengeVersion;
			PlayerPrefs.SetInt("Challenge_Daily", 0);
		}
	}

	// Token: 0x040004E4 RID: 1252
	public const string EndlessName = "EndlessDay";

	// Token: 0x040004E5 RID: 1253
	public LeaderBoardInfo[] EndlessResult = new LeaderBoardInfo[2];

	// Token: 0x040004E6 RID: 1254
	public int OnlineEndlessVersion;

	// Token: 0x040004E7 RID: 1255
	public const string ChallengeDailyName = "Challenge_Daily";

	// Token: 0x040004E8 RID: 1256
	public LeaderBoardInfo[] ChallengeResults = new LeaderBoardInfo[2];

	// Token: 0x040004E9 RID: 1257
	public int OnlineChallengeVersion;

	// Token: 0x040004EC RID: 1260
	private bool FirstLogin = true;
}
