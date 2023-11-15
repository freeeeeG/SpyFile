using System;
using System.Collections;
using Team17.Online;
using UnityEngine;

// Token: 0x02000B53 RID: 2899
public class SurvivalModeRatingUIController : UIControllerBase
{
	// Token: 0x06003AE9 RID: 15081 RVA: 0x00118718 File Offset: 0x00116B18
	private void Awake()
	{
		this.m_focusPlayersButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UIResultsToggleProfile, PlayerInputLookup.Player.One);
		this.m_backButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UICancel, PlayerInputLookup.Player.One);
		this.m_uiPlayers.AllowSettingFocus = false;
		this.m_waitingForPlayers.SetActive(false);
	}

	// Token: 0x06003AEA RID: 15082 RVA: 0x00118750 File Offset: 0x00116B50
	private void Start()
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

	// Token: 0x06003AEB RID: 15083 RVA: 0x00118794 File Offset: 0x00116B94
	private void UpdateLegend()
	{
		if (!ConnectionStatus.IsHost() && ConnectionStatus.IsInSession())
		{
			this.m_legendText.SetLocalisedTextCatchAll(this.m_LegendText_Emote_NoRestart);
		}
		else if (!UserSystemUtils.AnySplitPadUsers())
		{
			this.m_legendText.SetLocalisedTextCatchAll((ClientGameSetup.Mode != GameMode.Campaign) ? this.m_LegendText_Emote_NoRestart : this.m_LegendText_Emote_Restart);
		}
		else
		{
			this.m_legendText.SetLocalisedTextCatchAll((ClientGameSetup.Mode != GameMode.Campaign) ? this.m_LegendText_NoEmote_NoRestart : this.m_LegendText_NoEmote_Restart);
		}
	}

	// Token: 0x06003AEC RID: 15084 RVA: 0x00118828 File Offset: 0x00116C28
	public void SetScoreData(object _scoreData)
	{
		SurvivalModeRatingUIController.ScoreData scoreData = (SurvivalModeRatingUIController.ScoreData)_scoreData;
		GameSession gameSession = GameUtils.GetGameSession();
		SceneDirectoryData sceneDirectory = gameSession.Progress.GetSceneDirectory();
		int levelID = GameUtils.GetLevelID();
		GameProgress.GameProgressData.LevelProgress progress = gameSession.Progress.GetProgress(levelID);
		SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = sceneDirectory.Scenes[levelID];
		SceneDirectoryData.PerPlayerCountDirectoryEntry sceneDirectoryVarientEntry = gameSession.LevelSettings.SceneDirectoryVarientEntry;
		this.m_levelTitleText.SetLocalisedTextCatchAll(sceneDirectoryEntry.Label);
		if (this.m_uiPlayers != null)
		{
			GamepadUser user = GameUtils.RequestManager<PlayerManager>().GetUser(EngagementSlot.One);
			this.m_uiPlayers.Show(user, null, null, true);
		}
		string nonLocalizedText = Localization.Get(SurvivalModeRatingUIController.m_ordersDeliveredText, new LocToken[]
		{
			new LocToken("Count", "0")
		});
		this.m_successfulDeliveriesTitle.SetNonLocalizedText(nonLocalizedText);
		string nonLocalizedText2 = Localization.Get(SurvivalModeRatingUIController.m_ordersFailedText, new LocToken[]
		{
			new LocToken("Count", "0")
		});
		this.m_failedDeliveriesTitle.SetNonLocalizedText(nonLocalizedText2);
		base.StartCoroutine(this.TickUpScore(scoreData));
	}

	// Token: 0x06003AED RID: 15085 RVA: 0x0011893C File Offset: 0x00116D3C
	private IEnumerator TickUpScore(SurvivalModeRatingUIController.ScoreData _scoreData)
	{
		GameConfig gameConfig = GameUtils.GetGameConfig();
		this.m_timeSurvived.Value = _scoreData.m_timeSurvived;
		float timePerElement = this.m_tickUpDuration / 4f;
		int nSuccessfulDeliveries = _scoreData.m_totalSuccessfulDeliveries;
		for (int it = 1; it <= nSuccessfulDeliveries; it++)
		{
			this.m_successfulDeliveriesScore.Value = _scoreData.m_successPoints / nSuccessfulDeliveries * it;
			string strText = Localization.Get(SurvivalModeRatingUIController.m_ordersDeliveredText, new LocToken[]
			{
				new LocToken("Count", it.ToString())
			});
			this.m_successfulDeliveriesTitle.SetNonLocalizedText(strText);
			yield return null;
		}
		this.m_tickUpProgress = 0f;
		while (this.m_tickUpProgress < 1f)
		{
			this.m_tickUpProgress = Mathf.Min(this.m_tickUpProgress + Time.deltaTime / timePerElement, 1f);
			this.m_tipsScore.Value = Mathf.RoundToInt((float)_scoreData.m_tips * this.m_tickUpProgress);
			yield return null;
		}
		int nFailedDeliveries = _scoreData.m_failDeductions / gameConfig.RecipeTimeOutPointLoss;
		for (int it2 = 1; it2 <= nFailedDeliveries; it2++)
		{
			this.m_failedDeliveriesScore.Value = -(_scoreData.m_failDeductions / nFailedDeliveries) * it2;
			string strText2 = Localization.Get(SurvivalModeRatingUIController.m_ordersFailedText, new LocToken[]
			{
				new LocToken("Count", it2.ToString())
			});
			this.m_failedDeliveriesTitle.SetNonLocalizedText(strText2);
			yield return null;
		}
		this.m_tickUpProgress = 0f;
		while (this.m_tickUpProgress < 1f)
		{
			this.m_tickUpProgress = Mathf.Min(this.m_tickUpProgress + Time.deltaTime / timePerElement, 1f);
			this.m_totalScore.Value = Mathf.RoundToInt((float)_scoreData.m_score * this.m_tickUpProgress);
			yield return null;
		}
		this.m_successfulDeliveriesScore.Value = _scoreData.m_successPoints;
		this.m_tipsScore.Value = _scoreData.m_tips;
		this.m_totalScore.Value = _scoreData.m_score;
		string strSuccesfulDeliveriesText = Localization.Get(SurvivalModeRatingUIController.m_ordersDeliveredText, new LocToken[]
		{
			new LocToken("Count", nSuccessfulDeliveries.ToString())
		});
		this.m_successfulDeliveriesTitle.SetNonLocalizedText(strSuccesfulDeliveriesText);
		string strFailedDeliveriesText = Localization.Get(SurvivalModeRatingUIController.m_ordersFailedText, new LocToken[]
		{
			new LocToken("Count", nFailedDeliveries.ToString())
		});
		this.m_failedDeliveriesTitle.SetNonLocalizedText(strFailedDeliveriesText);
		yield break;
	}

	// Token: 0x06003AEE RID: 15086 RVA: 0x00118960 File Offset: 0x00116D60
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

	// Token: 0x06003AEF RID: 15087 RVA: 0x0011899D File Offset: 0x00116D9D
	public bool AllowedToRestart()
	{
		return !this.m_focusedOnPlayers;
	}

	// Token: 0x06003AF0 RID: 15088 RVA: 0x001189A8 File Offset: 0x00116DA8
	private void Update()
	{
		if (this.m_uiPlayers != null)
		{
			if (this.m_focusPlayersButton.JustPressed())
			{
				this.m_focusPlayersButton.ClaimPressEvent();
				if (T17EventSystemsManager.Instance != null)
				{
					T17EventSystem eventSystemForEngagementSlot = T17EventSystemsManager.Instance.GetEventSystemForEngagementSlot(EngagementSlot.One);
					if (eventSystemForEngagementSlot != null && eventSystemForEngagementSlot.currentSelectedGameObject == null)
					{
						this.m_focusedOnPlayers = true;
						this.m_uiPlayers.FocusOnFirstPlayer(true);
						this.m_legendText.SetLocalisedTextCatchAll(SurvivalModeRatingUIController.m_focusedLegendText);
					}
				}
			}
			if (this.m_backButton.JustPressed())
			{
				this.m_backButton.ClaimPressEvent();
				if (T17EventSystemsManager.Instance != null)
				{
					T17EventSystem eventSystemForEngagementSlot2 = T17EventSystemsManager.Instance.GetEventSystemForEngagementSlot(EngagementSlot.One);
					if (eventSystemForEngagementSlot2 != null)
					{
						if (eventSystemForEngagementSlot2.currentSelectedGameObject != null)
						{
							this.m_uiPlayers.CloseAllPlayerMenus();
							eventSystemForEngagementSlot2.SetSelectedGameObject(null);
							eventSystemForEngagementSlot2.SetSelectedGameObject(null);
							this.UpdateLegend();
						}
						else
						{
							this.m_focusedOnPlayers = false;
						}
					}
				}
			}
		}
	}

	// Token: 0x04002FD3 RID: 12243
	[SerializeField]
	private T17Text m_levelTitleText;

	// Token: 0x04002FD4 RID: 12244
	[SerializeField]
	private UIPlayerRootMenu m_uiPlayers;

	// Token: 0x04002FD5 RID: 12245
	[SerializeField]
	public GameObject m_waitingForPlayers;

	// Token: 0x04002FD6 RID: 12246
	[SerializeField]
	private DisplayTimeUIController m_timeSurvived;

	// Token: 0x04002FD7 RID: 12247
	[SerializeField]
	private DisplayIntUIController m_successfulDeliveriesScore;

	// Token: 0x04002FD8 RID: 12248
	[SerializeField]
	private DisplayIntUIController m_failedDeliveriesScore;

	// Token: 0x04002FD9 RID: 12249
	[SerializeField]
	private DisplayIntUIController m_tipsScore;

	// Token: 0x04002FDA RID: 12250
	[SerializeField]
	private DisplayIntUIController m_totalScore;

	// Token: 0x04002FDB RID: 12251
	[SerializeField]
	private Animator m_onionKingAnimator;

	// Token: 0x04002FDC RID: 12252
	[SerializeField]
	private Animator m_kevinAnimator;

	// Token: 0x04002FDD RID: 12253
	[SerializeField]
	[Range(0.001f, 10f)]
	private float m_tickUpDuration = 1f;

	// Token: 0x04002FDE RID: 12254
	private float m_tickUpProgress;

	// Token: 0x04002FDF RID: 12255
	[SerializeField]
	private T17Text m_successfulDeliveriesTitle;

	// Token: 0x04002FE0 RID: 12256
	[SerializeField]
	private T17Text m_failedDeliveriesTitle;

	// Token: 0x04002FE1 RID: 12257
	[SerializeField]
	private T17Text m_legendText;

	// Token: 0x04002FE2 RID: 12258
	public string m_LegendText_Emote_NoRestart = "Text.Menu.Legend03";

	// Token: 0x04002FE3 RID: 12259
	public string m_LegendText_NoEmote_NoRestart = "Text.Menu.Legend03NoEmote";

	// Token: 0x04002FE4 RID: 12260
	public string m_LegendText_Emote_Restart = "Text.Menu.Legend03Restart";

	// Token: 0x04002FE5 RID: 12261
	public string m_LegendText_NoEmote_Restart = "Text.Menu.Legend03RestartNoEmote";

	// Token: 0x04002FE6 RID: 12262
	private static readonly string m_focusedLegendText = "Text.Menu.RoundResultsCancel";

	// Token: 0x04002FE7 RID: 12263
	private static readonly string m_ordersDeliveredText = "Text.Menu.OrdersDelivered";

	// Token: 0x04002FE8 RID: 12264
	private static readonly string m_ordersFailedText = "Text.Menu.OrdersFailed";

	// Token: 0x04002FE9 RID: 12265
	private ILogicalButton m_focusPlayersButton;

	// Token: 0x04002FEA RID: 12266
	private ILogicalButton m_backButton;

	// Token: 0x04002FEB RID: 12267
	private bool m_focusedOnPlayers;

	// Token: 0x02000B54 RID: 2900
	public struct ScoreData
	{
		// Token: 0x04002FEC RID: 12268
		public int m_timeSurvived;

		// Token: 0x04002FED RID: 12269
		public int m_successPoints;

		// Token: 0x04002FEE RID: 12270
		public int m_failDeductions;

		// Token: 0x04002FEF RID: 12271
		public int m_tips;

		// Token: 0x04002FF0 RID: 12272
		public int m_score;

		// Token: 0x04002FF1 RID: 12273
		public int m_totalSuccessfulDeliveries;
	}
}
