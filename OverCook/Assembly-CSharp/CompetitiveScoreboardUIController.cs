using System;
using System.Collections;
using Team17.Online;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B22 RID: 2850
public class CompetitiveScoreboardUIController : UIControllerBase
{
	// Token: 0x060039AD RID: 14765 RVA: 0x00111865 File Offset: 0x0010FC65
	protected void Awake()
	{
		this.m_FocusPlayersButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UIResultsToggleProfile, PlayerInputLookup.Player.One);
		this.m_BackButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UICancel, PlayerInputLookup.Player.One);
		this.m_uiPlayers.AllowSettingFocus = false;
		this.m_waitingForPlayers.SetActive(false);
	}

	// Token: 0x060039AE RID: 14766 RVA: 0x0011189C File Offset: 0x0010FC9C
	protected void Start()
	{
		this.UpdateLegend();
		if (T17EventSystemsManager.Instance != null)
		{
			T17EventSystem eventSystemForEngagementSlot = T17EventSystemsManager.Instance.GetEventSystemForEngagementSlot(EngagementSlot.One);
			if (eventSystemForEngagementSlot != null)
			{
				eventSystemForEngagementSlot.SetSelectedGameObject(null);
				eventSystemForEngagementSlot.SetSelectedGameObject(null);
			}
		}
	}

	// Token: 0x060039AF RID: 14767 RVA: 0x001118E0 File Offset: 0x0010FCE0
	protected void Update()
	{
		if (this.m_uiPlayers != null)
		{
			if (this.m_FocusPlayersButton.JustPressed())
			{
				this.m_FocusPlayersButton.ClaimPressEvent();
				if (T17EventSystemsManager.Instance != null)
				{
					T17EventSystem eventSystemForEngagementSlot = T17EventSystemsManager.Instance.GetEventSystemForEngagementSlot(EngagementSlot.One);
					if (eventSystemForEngagementSlot != null && eventSystemForEngagementSlot.currentSelectedGameObject == null)
					{
						this.m_uiPlayers.FocusOnFirstPlayer(true);
						this.m_LegendText.SetLocalisedTextCatchAll(CompetitiveScoreboardUIController.m_FocusedLegendText);
					}
				}
			}
			if (this.m_BackButton.JustPressed())
			{
				this.m_BackButton.ClaimPressEvent();
				if (T17EventSystemsManager.Instance != null)
				{
					T17EventSystem eventSystemForEngagementSlot2 = T17EventSystemsManager.Instance.GetEventSystemForEngagementSlot(EngagementSlot.One);
					if (eventSystemForEngagementSlot2 != null && eventSystemForEngagementSlot2.currentSelectedGameObject != null)
					{
						this.m_uiPlayers.CloseAllPlayerMenus();
						eventSystemForEngagementSlot2.SetSelectedGameObject(null);
						eventSystemForEngagementSlot2.SetSelectedGameObject(null);
						this.UpdateLegend();
					}
				}
			}
		}
	}

	// Token: 0x060039B0 RID: 14768 RVA: 0x001119D5 File Offset: 0x0010FDD5
	private void UpdateLegend()
	{
		if (!UserSystemUtils.AnySplitPadUsers())
		{
			this.m_LegendText.SetLocalisedTextCatchAll(this.m_LegendText_Emote_NoRestart);
		}
		else
		{
			this.m_LegendText.SetLocalisedTextCatchAll(this.m_LegendText_NoEmote_NoRestart);
		}
	}

	// Token: 0x060039B1 RID: 14769 RVA: 0x00111A08 File Offset: 0x0010FE08
	public void SetScoreData(object _scoreData)
	{
		CompetitiveScoreboardUIController.ScoreData scoreData = _scoreData as CompetitiveScoreboardUIController.ScoreData;
		GameSession gameSession = GameUtils.GetGameSession();
		SceneDirectoryData sceneDirectory = gameSession.Progress.GetSceneDirectory();
		int levelID = GameUtils.GetLevelID();
		GameProgress.GameProgressData.LevelProgress progress = gameSession.Progress.GetProgress(levelID);
		SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = sceneDirectory.Scenes[levelID];
		SceneDirectoryData.PerPlayerCountDirectoryEntry sceneDirectoryVarientEntry = gameSession.LevelSettings.SceneDirectoryVarientEntry;
		this.m_levelTitleText.SetLocalisedTextCatchAll(sceneDirectoryEntry.Label);
		this.m_winneris_noone.enabled = false;
		this.m_winneris_noone_Blue.enabled = false;
		this.m_winneris_one.enabled = false;
		this.m_winneris_two.enabled = false;
		if (this.m_uiPlayers != null)
		{
			GamepadUser user = GameUtils.RequestManager<PlayerManager>().GetUser(EngagementSlot.One);
			this.m_uiPlayers.m_canKickUsers = true;
			this.m_uiPlayers.Show(user, null, null, true);
		}
		this.m_teamOne.RegisterFinished(delegate
		{
			this.m_bteamOneDone = true;
			if (this.m_bteamTwoDone)
			{
				this.ShowWinner(scoreData.TeamOneData, scoreData.TeamTwoData);
			}
		});
		this.m_teamTwo.RegisterFinished(delegate
		{
			this.m_bteamTwoDone = true;
			if (this.m_bteamOneDone)
			{
				this.ShowWinner(scoreData.TeamOneData, scoreData.TeamTwoData);
			}
		});
		this.m_teamOne.ApplyScoreToUI(scoreData.TeamOneData, this);
		this.m_teamTwo.ApplyScoreToUI(scoreData.TeamTwoData, this);
	}

	// Token: 0x060039B2 RID: 14770 RVA: 0x00111B44 File Offset: 0x0010FF44
	protected void ShowWinner(TeamMonitor.TeamScoreStats _score1, TeamMonitor.TeamScoreStats _score2)
	{
		float num = (float)_score1.GetTotalScore();
		float num2 = (float)_score2.GetTotalScore();
		this.m_winneris_noone.enabled = (num == num2);
		this.m_winneris_noone_Blue.enabled = (num == num2);
		this.m_winneris_one.enabled = (num > num2);
		this.m_winneris_two.enabled = (num < num2);
	}

	// Token: 0x060039B3 RID: 14771 RVA: 0x00111B9D File Offset: 0x0010FF9D
	public bool IsButtonActive()
	{
		return this.m_buttonIcon.enabled;
	}

	// Token: 0x060039B4 RID: 14772 RVA: 0x00111BAC File Offset: 0x0010FFAC
	public bool AllowedToSkip()
	{
		if (T17EventSystemsManager.Instance != null)
		{
			T17EventSystem eventSystemForEngagementSlot = T17EventSystemsManager.Instance.GetEventSystemForEngagementSlot(EngagementSlot.One);
			if (eventSystemForEngagementSlot != null)
			{
				return eventSystemForEngagementSlot.currentSelectedGameObject == null;
			}
		}
		return true;
	}

	// Token: 0x04002E68 RID: 11880
	[SerializeField]
	private T17Text m_levelTitleText;

	// Token: 0x04002E69 RID: 11881
	[SerializeField]
	private Text m_winneris_noone;

	// Token: 0x04002E6A RID: 11882
	[SerializeField]
	private Text m_winneris_noone_Blue;

	// Token: 0x04002E6B RID: 11883
	[SerializeField]
	private Text m_winneris_one;

	// Token: 0x04002E6C RID: 11884
	[SerializeField]
	private Text m_winneris_two;

	// Token: 0x04002E6D RID: 11885
	[SerializeField]
	private UIPlayerRootMenu m_uiPlayers;

	// Token: 0x04002E6E RID: 11886
	[SerializeField]
	private CompetitiveScoreboardUIController.PerTeamUI m_teamOne;

	// Token: 0x04002E6F RID: 11887
	[SerializeField]
	private CompetitiveScoreboardUIController.PerTeamUI m_teamTwo;

	// Token: 0x04002E70 RID: 11888
	[SerializeField]
	private Image m_buttonIcon;

	// Token: 0x04002E71 RID: 11889
	[SerializeField]
	private T17Text m_LegendText;

	// Token: 0x04002E72 RID: 11890
	[SerializeField]
	public GameObject m_waitingForPlayers;

	// Token: 0x04002E73 RID: 11891
	public string m_LegendText_NoEmote_NoRestart = "Text.Menu.Legend03NoEmote";

	// Token: 0x04002E74 RID: 11892
	public string m_LegendText_Emote_NoRestart = "Text.Menu.Legend03";

	// Token: 0x04002E75 RID: 11893
	private static readonly string m_FocusedLegendText = "Text.Menu.RoundResultsCancel";

	// Token: 0x04002E76 RID: 11894
	private bool m_bteamOneDone;

	// Token: 0x04002E77 RID: 11895
	private bool m_bteamTwoDone;

	// Token: 0x04002E78 RID: 11896
	private ILogicalButton m_FocusPlayersButton;

	// Token: 0x04002E79 RID: 11897
	private ILogicalButton m_BackButton;

	// Token: 0x02000B23 RID: 2851
	public class ScoreData
	{
		// Token: 0x04002E7A RID: 11898
		public TeamMonitor.TeamScoreStats TeamOneData;

		// Token: 0x04002E7B RID: 11899
		public TeamMonitor.TeamScoreStats TeamTwoData;
	}

	// Token: 0x02000B24 RID: 2852
	[Serializable]
	private class PerTeamUI
	{
		// Token: 0x060039B8 RID: 14776 RVA: 0x00111C34 File Offset: 0x00110034
		public void ApplyScoreToUI(TeamMonitor.TeamScoreStats _scoreData, CompetitiveScoreboardUIController _uiController)
		{
			GameConfig gameConfig = GameUtils.GetGameConfig();
			this.m_SuccessfulDeliveriesScore.Value = 0;
			this.m_FailedDeliveriesScore.Value = 0;
			this.m_TipsScore.Value = 0;
			this.m_TotalScore.Value = 0;
			string nonLocalizedText = Localization.Get(CompetitiveScoreboardUIController.PerTeamUI.m_OrdersDeliveredText, new LocToken[]
			{
				new LocToken("Count", "0")
			});
			this.m_SuccessfulDeliveriesTitle.SetNonLocalizedText(nonLocalizedText);
			string nonLocalizedText2 = Localization.Get(CompetitiveScoreboardUIController.PerTeamUI.m_OrdersFailedText, new LocToken[]
			{
				new LocToken("Count", "0")
			});
			this.m_FailedDeliveriesTitle.SetNonLocalizedText(nonLocalizedText2);
			_uiController.StartCoroutine(this.TickUpScore(_scoreData));
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x00111CF8 File Offset: 0x001100F8
		private IEnumerator TickUpScore(TeamMonitor.TeamScoreStats _scoreData)
		{
			GameConfig gameConfig = GameUtils.GetGameConfig();
			float timePerElement = this.m_tickUpDuration / 4f;
			int nSuccessfulDeliveries = _scoreData.TotalSuccessfulDeliveries;
			for (int it = 1; it <= nSuccessfulDeliveries; it++)
			{
				this.m_SuccessfulDeliveriesScore.Value = _scoreData.TotalBaseScore / nSuccessfulDeliveries * it;
				string strText = Localization.Get(CompetitiveScoreboardUIController.PerTeamUI.m_OrdersDeliveredText, new LocToken[]
				{
					new LocToken("Count", it.ToString())
				});
				this.m_SuccessfulDeliveriesTitle.SetNonLocalizedText(strText);
				yield return null;
			}
			this.m_tickUpProgress = 0f;
			while (this.m_tickUpProgress < 1f)
			{
				this.m_tickUpProgress = Mathf.Min(this.m_tickUpProgress + Time.deltaTime / timePerElement, 1f);
				this.m_TipsScore.Value = Mathf.RoundToInt((float)_scoreData.TotalTipsScore * this.m_tickUpProgress);
				yield return null;
			}
			int nFailedDeliveries = _scoreData.TotalTimeExpireDeductions / gameConfig.RecipeTimeOutPointLoss;
			for (int it2 = 1; it2 <= nFailedDeliveries; it2++)
			{
				this.m_FailedDeliveriesScore.Value = -(_scoreData.TotalTimeExpireDeductions / nFailedDeliveries) * it2;
				string strText2 = Localization.Get(CompetitiveScoreboardUIController.PerTeamUI.m_OrdersFailedText, new LocToken[]
				{
					new LocToken("Count", it2.ToString())
				});
				this.m_FailedDeliveriesTitle.SetNonLocalizedText(strText2);
				yield return null;
			}
			this.m_tickUpProgress = 0f;
			int totalScore = _scoreData.GetTotalScore();
			while (this.m_tickUpProgress < 1f)
			{
				this.m_tickUpProgress = Mathf.Min(this.m_tickUpProgress + Time.deltaTime / timePerElement, 1f);
				this.m_TotalScore.Value = Mathf.RoundToInt((float)totalScore * this.m_tickUpProgress);
				yield return null;
			}
			this.m_SuccessfulDeliveriesScore.Value = _scoreData.TotalBaseScore;
			this.m_TipsScore.Value = _scoreData.TotalTipsScore;
			this.m_TotalScore.Value = totalScore;
			string strSuccesfulDeliveriesText = Localization.Get(CompetitiveScoreboardUIController.PerTeamUI.m_OrdersDeliveredText, new LocToken[]
			{
				new LocToken("Count", nSuccessfulDeliveries.ToString())
			});
			this.m_SuccessfulDeliveriesTitle.SetNonLocalizedText(strSuccesfulDeliveriesText);
			string strFailedDeliveriesText = Localization.Get(CompetitiveScoreboardUIController.PerTeamUI.m_OrdersFailedText, new LocToken[]
			{
				new LocToken("Count", nFailedDeliveries.ToString())
			});
			this.m_FailedDeliveriesTitle.SetNonLocalizedText(strFailedDeliveriesText);
			if (this.m_finishedCallback != null)
			{
				this.m_finishedCallback();
			}
			yield break;
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x00111D1A File Offset: 0x0011011A
		public void RegisterFinished(CallbackVoid _callback)
		{
			this.m_finishedCallback = (CallbackVoid)Delegate.Combine(this.m_finishedCallback, _callback);
		}

		// Token: 0x060039BB RID: 14779 RVA: 0x00111D33 File Offset: 0x00110133
		public void UnregisterFinished(CallbackVoid _callback)
		{
			this.m_finishedCallback = (CallbackVoid)Delegate.Remove(this.m_finishedCallback, _callback);
		}

		// Token: 0x04002E7C RID: 11900
		public T17Text m_SuccessfulDeliveriesTitle;

		// Token: 0x04002E7D RID: 11901
		public DisplayIntUIController m_SuccessfulDeliveriesScore;

		// Token: 0x04002E7E RID: 11902
		public T17Text m_FailedDeliveriesTitle;

		// Token: 0x04002E7F RID: 11903
		public DisplayIntUIController m_FailedDeliveriesScore;

		// Token: 0x04002E80 RID: 11904
		public DisplayIntUIController m_TipsScore;

		// Token: 0x04002E81 RID: 11905
		public DisplayIntUIController m_TotalScore;

		// Token: 0x04002E82 RID: 11906
		private static readonly string m_OrdersDeliveredText = "Text.Menu.OrdersDelivered";

		// Token: 0x04002E83 RID: 11907
		private static readonly string m_OrdersFailedText = "Text.Menu.OrdersFailed";

		// Token: 0x04002E84 RID: 11908
		[SerializeField]
		[Range(0.001f, 10f)]
		private float m_tickUpDuration = 1f;

		// Token: 0x04002E85 RID: 11909
		private float m_tickUpProgress;

		// Token: 0x04002E86 RID: 11910
		private CallbackVoid m_finishedCallback = delegate()
		{
		};
	}
}
