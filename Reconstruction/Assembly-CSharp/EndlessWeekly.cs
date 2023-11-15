using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200027D RID: 637
public class EndlessWeekly : MonoBehaviour
{
	// Token: 0x06000FCC RID: 4044 RVA: 0x0002A482 File Offset: 0x00028682
	public void Initialize()
	{
		this.m_BattleRecipe.Initialize();
		Singleton<GameEvents>.Instance.onEndlessLeaderBoardGet += this.GetLeaderboardCallback;
		this.SetArea(true);
	}

	// Token: 0x06000FCD RID: 4045 RVA: 0x0002A4AC File Offset: 0x000286AC
	private void SetArea(bool value)
	{
		this.reconnectArea.SetActive(!value);
		this.mainArea.SetActive(value);
		if (value)
		{
			List<Rule> list = new List<Rule>();
			EndlessParam endlessParam = this.EndlessParamData.EndlessParams[Singleton<PlayfabManager>.Instance.OnlineEndlessVersion % this.EndlessParamData.EndlessParams.Count];
			foreach (RuleName ruleName in endlessParam.RuleNames)
			{
				Rule rule = RuleFactory.GetRule((int)ruleName);
				list.Add(rule);
			}
			this.m_BattleRule.SetRules(list);
			this.m_BattleRecipe.SetRecipes(endlessParam.Recipes);
		}
		this.playerScoreTxt.text = GameMultiLang.GetTraduction("PLAYERSCORE") + ":" + Singleton<PlayfabManager>.Instance.EndlessWave.ToString() + GameMultiLang.GetTraduction("WAVE");
	}

	// Token: 0x06000FCE RID: 4046 RVA: 0x0002A5B0 File Offset: 0x000287B0
	private void GetLeaderboardCallback(bool value)
	{
		this.SetArea(true);
	}

	// Token: 0x06000FCF RID: 4047 RVA: 0x0002A5B9 File Offset: 0x000287B9
	public void ReconnectBtnClick()
	{
		if (!this.isReconnecting)
		{
			base.StartCoroutine(this.ReconnectCor());
		}
	}

	// Token: 0x06000FD0 RID: 4048 RVA: 0x0002A5D0 File Offset: 0x000287D0
	private IEnumerator ReconnectCor()
	{
		this.isReconnecting = true;
		this.connectTips.text = GameMultiLang.GetTraduction("LEADERBOARDTIPS");
		Singleton<Game>.Instance.InitializeNetworks();
		this.reConnectBtn.SetActive(false);
		yield return new WaitForSeconds(5f);
		this.SetArea(SteamManager.Initialized && Singleton<PlayfabManager>.Instance.EndlessLeaderboardGot);
		this.connectTips.text = GameMultiLang.GetTraduction("LEADERBOARDTIPS2");
		this.reConnectBtn.SetActive(true);
		this.isReconnecting = false;
		yield break;
	}

	// Token: 0x06000FD1 RID: 4049 RVA: 0x0002A5DF File Offset: 0x000287DF
	public void EndlessModeStart()
	{
		this.m_BattleRecipe.UpdateRecipes();
		this.m_BattleRule.UpdateRules();
		Singleton<LevelManager>.Instance.StartNewGame(12);
	}

	// Token: 0x06000FD2 RID: 4050 RVA: 0x0002A603 File Offset: 0x00028803
	private void OnDestroy()
	{
		Singleton<GameEvents>.Instance.onEndlessLeaderBoardGet -= this.GetLeaderboardCallback;
	}

	// Token: 0x0400081F RID: 2079
	[SerializeField]
	private BattleRecipe m_BattleRecipe;

	// Token: 0x04000820 RID: 2080
	[SerializeField]
	private BattleRule m_BattleRule;

	// Token: 0x04000821 RID: 2081
	[SerializeField]
	private GameObject reconnectArea;

	// Token: 0x04000822 RID: 2082
	[SerializeField]
	private GameObject mainArea;

	// Token: 0x04000823 RID: 2083
	[SerializeField]
	private Text connectTips;

	// Token: 0x04000824 RID: 2084
	[SerializeField]
	private GameObject reConnectBtn;

	// Token: 0x04000825 RID: 2085
	[SerializeField]
	private WeeklyParam_Endless EndlessParamData;

	// Token: 0x04000826 RID: 2086
	[SerializeField]
	private Text playerScoreTxt;

	// Token: 0x04000827 RID: 2087
	private bool isReconnecting;
}
