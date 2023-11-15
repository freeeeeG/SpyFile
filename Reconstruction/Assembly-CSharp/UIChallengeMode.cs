using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200027E RID: 638
public class UIChallengeMode : MonoBehaviour
{
	// Token: 0x06000FD4 RID: 4052 RVA: 0x0002A623 File Offset: 0x00028823
	public void Initialize()
	{
		Singleton<GameEvents>.Instance.onChallengeLeaderBoardGet += this.GetLeaderboardCallback;
		this.SetArea(true);
	}

	// Token: 0x06000FD5 RID: 4053 RVA: 0x0002A642 File Offset: 0x00028842
	private void GetLeaderboardCallback(bool value)
	{
		this.SetArea(true);
	}

	// Token: 0x06000FD6 RID: 4054 RVA: 0x0002A64C File Offset: 0x0002884C
	private void SetArea(bool value)
	{
		this.reconnectArea.SetActive(!value);
		this.mainArea.SetActive(value);
		if (value)
		{
			this.levelAtt = this.daylyChallengeParam.ChallengeLevels[Singleton<PlayfabManager>.Instance.OnlineChallengeVersion % this.daylyChallengeParam.ChallengeLevels.Count];
			this.SetBossInfo();
			this.SetStarInfo();
		}
		this.playerScoreTxt.text = GameMultiLang.GetTraduction("PLAYERSCORE") + ":" + Singleton<PlayfabManager>.Instance.ChallngeScore.ToString();
	}

	// Token: 0x06000FD7 RID: 4055 RVA: 0x0002A6E8 File Offset: 0x000288E8
	private void SetBossInfo()
	{
		this.bossItemSlots[0].SetContent(this.levelAtt.Boss1[0], this.bossToggleGroup);
		this.bossItemSlots[1].SetContent(this.levelAtt.Boss2[0], this.bossToggleGroup);
		this.bossItemSlots[2].SetContent(this.levelAtt.Boss3[0], this.bossToggleGroup);
	}

	// Token: 0x06000FD8 RID: 4056 RVA: 0x0002A764 File Offset: 0x00028964
	private void SetStarInfo()
	{
		int challngeScore = Singleton<PlayfabManager>.Instance.ChallngeScore;
		this.highScore_Txt.text = challngeScore.ToString();
	}

	// Token: 0x06000FD9 RID: 4057 RVA: 0x0002A78E File Offset: 0x0002898E
	public void ReconnectBtnClick()
	{
		if (!this.isReconnecting)
		{
			base.StartCoroutine(this.ReconnectCor());
		}
	}

	// Token: 0x06000FDA RID: 4058 RVA: 0x0002A7A5 File Offset: 0x000289A5
	private IEnumerator ReconnectCor()
	{
		this.isReconnecting = true;
		this.connectTips.text = GameMultiLang.GetTraduction("LEADERBOARDTIPS");
		Singleton<Game>.Instance.InitializeNetworks();
		this.reConnectBtn.SetActive(false);
		yield return new WaitForSeconds(5f);
		this.SetArea(SteamManager.Initialized && Singleton<PlayfabManager>.Instance.ChallengeLeaderboardGot);
		this.connectTips.text = GameMultiLang.GetTraduction("LEADERBOARDTIPS2");
		this.reConnectBtn.SetActive(true);
		this.isReconnecting = false;
		yield break;
	}

	// Token: 0x06000FDB RID: 4059 RVA: 0x0002A7B4 File Offset: 0x000289B4
	public void ChallengeModeStart()
	{
		RuleFactory.Release();
		Singleton<LevelManager>.Instance.StartNewGame(this.levelAtt.ModeID);
	}

	// Token: 0x06000FDC RID: 4060 RVA: 0x0002A7D0 File Offset: 0x000289D0
	private void OnDestroy()
	{
		Singleton<GameEvents>.Instance.onChallengeLeaderBoardGet -= this.GetLeaderboardCallback;
	}

	// Token: 0x04000828 RID: 2088
	[SerializeField]
	private DailyChallengeParam daylyChallengeParam;

	// Token: 0x04000829 RID: 2089
	[SerializeField]
	private GameObject mainArea;

	// Token: 0x0400082A RID: 2090
	[SerializeField]
	private GameObject reconnectArea;

	// Token: 0x0400082B RID: 2091
	[SerializeField]
	private Text playerScoreTxt;

	// Token: 0x0400082C RID: 2092
	private bool isReconnecting;

	// Token: 0x0400082D RID: 2093
	[SerializeField]
	private Text connectTips;

	// Token: 0x0400082E RID: 2094
	[SerializeField]
	private GameObject reConnectBtn;

	// Token: 0x0400082F RID: 2095
	[SerializeField]
	private ToggleGroup bossToggleGroup;

	// Token: 0x04000830 RID: 2096
	[SerializeField]
	private ItemSlot[] bossItemSlots;

	// Token: 0x04000831 RID: 2097
	[SerializeField]
	private TextMeshProUGUI highScore_Txt;

	// Token: 0x04000832 RID: 2098
	private LevelAttribute levelAtt;
}
