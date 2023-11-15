using System;
using System.Collections;
using Team17.Online;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B27 RID: 2855
public class CoopStarRatingUIController : StarRatingUIController
{
	// Token: 0x060039CA RID: 14794 RVA: 0x001127FC File Offset: 0x00110BFC
	protected override void Awake()
	{
		base.Awake();
		this.m_FocusPlayersButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UIResultsToggleProfile, PlayerInputLookup.Player.One);
		this.m_BackButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UICancel, PlayerInputLookup.Player.One);
		this.m_uiPlayers.AllowSettingFocus = false;
		this.m_waitingForPlayers.SetActive(false);
		if (this.m_scoreboardStarsPrefab != null && this.m_contentBacker != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_scoreboardStarsPrefab, this.m_contentBacker);
			gameObject.transform.localScale = Vector3.one;
			this.m_stars = gameObject.gameObject.RequestComponentsRecursive<ScoreBoundaryStar>();
			while (gameObject.transform.childCount > 0)
			{
				Transform child = gameObject.transform.GetChild(0);
				child.SetParent(this.m_contentBacker);
			}
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	// Token: 0x060039CB RID: 14795 RVA: 0x001128D0 File Offset: 0x00110CD0
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

	// Token: 0x060039CC RID: 14796 RVA: 0x00112914 File Offset: 0x00110D14
	public override void SetScoreData(object _scoreData)
	{
		CoopStarRatingUIController.ScoreData scoreData = _scoreData as CoopStarRatingUIController.ScoreData;
		GameSession gameSession = GameUtils.GetGameSession();
		SceneDirectoryData sceneDirectory = gameSession.Progress.GetSceneDirectory();
		int levelID = GameUtils.GetLevelID();
		GameProgress.GameProgressData.LevelProgress progress = gameSession.Progress.GetProgress(levelID);
		SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = sceneDirectory.Scenes[levelID];
		SceneDirectoryData.PerPlayerCountDirectoryEntry sceneDirectoryVarientEntry = gameSession.LevelSettings.SceneDirectoryVarientEntry;
		this.m_levelTitleText.SetLocalisedTextCatchAll(sceneDirectoryEntry.Label);
		GameConfig gameConfig = GameUtils.GetGameConfig();
		this.m_failedDeliveriesScore.Value = 0;
		for (int i = 1; i <= 4; i++)
		{
			ScoreBoundaryStar uistar = this.GetUIStar(i);
			if (uistar != null)
			{
				switch (i)
				{
				case 1:
					uistar.Score = sceneDirectoryVarientEntry.OneStarScore;
					break;
				case 2:
					uistar.Score = sceneDirectoryVarientEntry.TwoStarScore;
					break;
				case 3:
					uistar.Score = sceneDirectoryVarientEntry.ThreeStarScore;
					break;
				case 4:
				{
					bool flag = !scoreData.JustUnlockedNGP && gameSession.Progress.SaveData.IsNGPEnabledForLevel(GameUtils.GetLevelID()) && gameSession.Progress.SaveData.NewGamePlusDialogShown;
					uistar.gameObject.SetActive(flag);
					if (flag)
					{
						uistar.Score = sceneDirectoryVarientEntry.FourStarScore;
					}
					break;
				}
				}
			}
		}
		if (this.m_highScoreLabel != null && this.m_highScore != null)
		{
			if (progress.Completed)
			{
				this.m_highScore.Value = Mathf.Max(progress.HighScore, scoreData.Score);
			}
			else
			{
				this.m_highScore.gameObject.SetActive(false);
				this.m_highScoreLabel.gameObject.SetActive(false);
			}
		}
		if (this.m_nextStarLabel != null && this.m_nextStarScore != null)
		{
			if (scoreData.StarRating < 4 && scoreData.StarRating >= 0)
			{
				this.m_nextStarScore.Value = sceneDirectoryVarientEntry.GetPointsForStar(scoreData.StarRating + 1);
			}
			else
			{
				this.m_nextStarLabel.gameObject.SetActive(false);
				this.m_nextStarScore.gameObject.SetActive(false);
			}
		}
		if (this.m_uiPlayers != null)
		{
			GamepadUser user = GameUtils.RequestManager<PlayerManager>().GetUser(EngagementSlot.One);
			this.m_uiPlayers.Show(user, null, null, true);
		}
		if (this.m_OnionKingAnimator != null)
		{
			this.m_OnionKingAnimator.SetInteger(CoopStarRatingUIController.m_iScore, scoreData.StarRating);
			this.m_OnionKingAnimator.Update(0f);
		}
		if (this.m_KevinAnimator != null)
		{
			this.m_KevinAnimator.SetInteger(CoopStarRatingUIController.m_iScore, scoreData.StarRating);
			this.m_KevinAnimator.Update(0f);
		}
		string nonLocalizedText = Localization.Get(CoopStarRatingUIController.m_OrdersDeliveredText, new LocToken[]
		{
			new LocToken("Count", "0")
		});
		this.m_SuccessfulDeliveriesTitle.SetNonLocalizedText(nonLocalizedText);
		string nonLocalizedText2 = Localization.Get(CoopStarRatingUIController.m_OrdersFailedText, new LocToken[]
		{
			new LocToken("Count", "0")
		});
		this.m_FailedDeliveriesTitle.SetNonLocalizedText(nonLocalizedText2);
		base.StartCoroutine(this.TickUpScore(scoreData));
	}

	// Token: 0x060039CD RID: 14797 RVA: 0x00112C7C File Offset: 0x0011107C
	public override bool AllowedToSkip()
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

	// Token: 0x060039CE RID: 14798 RVA: 0x00112CB9 File Offset: 0x001110B9
	public override bool AllowedToRestart()
	{
		return !this.m_focusedOnPlayers;
	}

	// Token: 0x060039CF RID: 14799 RVA: 0x00112CC4 File Offset: 0x001110C4
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
						this.m_focusedOnPlayers = true;
						this.m_uiPlayers.FocusOnFirstPlayer(true);
						this.m_LegendText.SetLocalisedTextCatchAll(CoopStarRatingUIController.m_FocusedLegendText);
					}
				}
			}
			if (this.m_BackButton.JustPressed())
			{
				this.m_BackButton.ClaimPressEvent();
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

	// Token: 0x060039D0 RID: 14800 RVA: 0x00112DCC File Offset: 0x001111CC
	private void UpdateLegend()
	{
		if (!ConnectionStatus.IsHost() && ConnectionStatus.IsInSession())
		{
			this.m_LegendText.SetLocalisedTextCatchAll(this.m_LegendText_Emote_NoRestart);
		}
		else if (!UserSystemUtils.AnySplitPadUsers())
		{
			this.m_LegendText.SetLocalisedTextCatchAll((ClientGameSetup.Mode != GameMode.Campaign) ? this.m_LegendText_Emote_NoRestart : this.m_LegendText_Emote_Restart);
		}
		else
		{
			this.m_LegendText.SetLocalisedTextCatchAll((ClientGameSetup.Mode != GameMode.Campaign) ? this.m_LegendText_NoEmote_NoRestart : this.m_LegendText_NoEmote_Restart);
		}
	}

	// Token: 0x060039D1 RID: 14801 RVA: 0x00112E60 File Offset: 0x00111260
	private IEnumerator TickUpScore(CoopStarRatingUIController.ScoreData _scoreData)
	{
		GameConfig gameConfig = GameUtils.GetGameConfig();
		float timePerElement = this.m_tickUpDuration / 4f;
		int nSuccessfulDeliveries = _scoreData.TotalSuccessfulDeliveries;
		for (int it = 1; it <= nSuccessfulDeliveries; it++)
		{
			this.m_successfulDeliveriesScore.Value = _scoreData.SuccessPoints / nSuccessfulDeliveries * it;
			string strText = Localization.Get(CoopStarRatingUIController.m_OrdersDeliveredText, new LocToken[]
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
			this.m_tipsScore.Value = Mathf.RoundToInt((float)_scoreData.Tips * this.m_tickUpProgress);
			yield return null;
		}
		int nFailedDeliveries = _scoreData.FailDeductions / gameConfig.RecipeTimeOutPointLoss;
		for (int it2 = 1; it2 <= nFailedDeliveries; it2++)
		{
			this.m_failedDeliveriesScore.Value = -(_scoreData.FailDeductions / nFailedDeliveries) * it2;
			string strText2 = Localization.Get(CoopStarRatingUIController.m_OrdersFailedText, new LocToken[]
			{
				new LocToken("Count", it2.ToString())
			});
			this.m_FailedDeliveriesTitle.SetNonLocalizedText(strText2);
			yield return null;
		}
		this.m_tickUpProgress = 0f;
		while (this.m_tickUpProgress < 1f)
		{
			this.m_tickUpProgress = Mathf.Min(this.m_tickUpProgress + Time.deltaTime / timePerElement, 1f);
			this.m_totalScore.Value = Mathf.RoundToInt((float)_scoreData.Score * this.m_tickUpProgress);
			yield return null;
		}
		base.SetScoreData(_scoreData.StarRating);
		this.m_successfulDeliveriesScore.Value = _scoreData.SuccessPoints;
		this.m_tipsScore.Value = _scoreData.Tips;
		this.m_totalScore.Value = _scoreData.Score;
		string strSuccesfulDeliveriesText = Localization.Get(CoopStarRatingUIController.m_OrdersDeliveredText, new LocToken[]
		{
			new LocToken("Count", nSuccessfulDeliveries.ToString())
		});
		this.m_SuccessfulDeliveriesTitle.SetNonLocalizedText(strSuccesfulDeliveriesText);
		string strFailedDeliveriesText = Localization.Get(CoopStarRatingUIController.m_OrdersFailedText, new LocToken[]
		{
			new LocToken("Count", nFailedDeliveries.ToString())
		});
		this.m_FailedDeliveriesTitle.SetNonLocalizedText(strFailedDeliveriesText);
		yield break;
	}

	// Token: 0x060039D2 RID: 14802 RVA: 0x00112E84 File Offset: 0x00111284
	private ScoreBoundaryStar GetUIStar(int _star)
	{
		if (this.m_stars != null)
		{
			for (int i = 0; i < this.m_stars.Length; i++)
			{
				ScoreBoundaryStar scoreBoundaryStar = this.m_stars[i];
				if (scoreBoundaryStar != null && scoreBoundaryStar.m_star == _star)
				{
					return scoreBoundaryStar;
				}
			}
		}
		return null;
	}

	// Token: 0x04002E95 RID: 11925
	[SerializeField]
	private T17Text m_levelTitleText;

	// Token: 0x04002E96 RID: 11926
	[SerializeField]
	private DisplayIntUIController m_successfulDeliveriesScore;

	// Token: 0x04002E97 RID: 11927
	[SerializeField]
	private DisplayIntUIController m_failedDeliveriesScore;

	// Token: 0x04002E98 RID: 11928
	[SerializeField]
	private DisplayIntUIController m_tipsScore;

	// Token: 0x04002E99 RID: 11929
	[SerializeField]
	private DisplayIntUIController m_totalScore;

	// Token: 0x04002E9A RID: 11930
	[SerializeField]
	private Text m_highScoreLabel;

	// Token: 0x04002E9B RID: 11931
	[SerializeField]
	private DisplayIntUIController m_highScore;

	// Token: 0x04002E9C RID: 11932
	[SerializeField]
	private Text m_nextStarLabel;

	// Token: 0x04002E9D RID: 11933
	[SerializeField]
	private DisplayIntUIController m_nextStarScore;

	// Token: 0x04002E9E RID: 11934
	[SerializeField]
	[Range(0.001f, 10f)]
	private float m_tickUpDuration = 1f;

	// Token: 0x04002E9F RID: 11935
	private float m_tickUpProgress;

	// Token: 0x04002EA0 RID: 11936
	private CallbackVoid m_finishedCallback = delegate()
	{
	};

	// Token: 0x04002EA1 RID: 11937
	[SerializeField]
	private UIPlayerRootMenu m_uiPlayers;

	// Token: 0x04002EA2 RID: 11938
	[SerializeField]
	private Animator m_OnionKingAnimator;

	// Token: 0x04002EA3 RID: 11939
	[SerializeField]
	private Animator m_KevinAnimator;

	// Token: 0x04002EA4 RID: 11940
	[SerializeField]
	private T17Text m_SuccessfulDeliveriesTitle;

	// Token: 0x04002EA5 RID: 11941
	[SerializeField]
	private T17Text m_FailedDeliveriesTitle;

	// Token: 0x04002EA6 RID: 11942
	[SerializeField]
	private T17Text m_LegendText;

	// Token: 0x04002EA7 RID: 11943
	[SerializeField]
	public GameObject m_waitingForPlayers;

	// Token: 0x04002EA8 RID: 11944
	[SerializeField]
	[AssignChild("ContentBacker", Editorbility.Editable)]
	private Transform m_contentBacker;

	// Token: 0x04002EA9 RID: 11945
	private ScoreBoundaryStar[] m_stars;

	// Token: 0x04002EAA RID: 11946
	[SerializeField]
	[AssignResource("NGPScoreBoardStars", Editorbility.Editable)]
	private GameObject m_scoreboardStarsPrefab;

	// Token: 0x04002EAB RID: 11947
	private static readonly string m_OrdersDeliveredText = "Text.Menu.OrdersDelivered";

	// Token: 0x04002EAC RID: 11948
	private static readonly string m_OrdersFailedText = "Text.Menu.OrdersFailed";

	// Token: 0x04002EAD RID: 11949
	public string m_LegendText_Emote_NoRestart = "Text.Menu.Legend03";

	// Token: 0x04002EAE RID: 11950
	public string m_LegendText_Emote_Restart = "Text.Menu.Legend03Restart";

	// Token: 0x04002EAF RID: 11951
	public string m_LegendText_NoEmote_NoRestart = "Text.Menu.Legend03NoEmote";

	// Token: 0x04002EB0 RID: 11952
	public string m_LegendText_NoEmote_Restart = "Text.Menu.Legend03RestartNoEmote";

	// Token: 0x04002EB1 RID: 11953
	private static readonly string m_FocusedLegendText = "Text.Menu.RoundResultsCancel";

	// Token: 0x04002EB2 RID: 11954
	private static readonly int m_iScore = Animator.StringToHash("Score");

	// Token: 0x04002EB3 RID: 11955
	private ILogicalButton m_FocusPlayersButton;

	// Token: 0x04002EB4 RID: 11956
	private ILogicalButton m_BackButton;

	// Token: 0x04002EB5 RID: 11957
	private bool m_focusedOnPlayers;

	// Token: 0x02000B28 RID: 2856
	public class ScoreData
	{
		// Token: 0x04002EB7 RID: 11959
		public int StarRating;

		// Token: 0x04002EB8 RID: 11960
		public bool StarRatingIncreased;

		// Token: 0x04002EB9 RID: 11961
		public int SuccessPoints;

		// Token: 0x04002EBA RID: 11962
		public int FailDeductions;

		// Token: 0x04002EBB RID: 11963
		public int Tips;

		// Token: 0x04002EBC RID: 11964
		public int Score;

		// Token: 0x04002EBD RID: 11965
		public int TotalSuccessfulDeliveries;

		// Token: 0x04002EBE RID: 11966
		public bool JustUnlockedNGP;
	}
}
