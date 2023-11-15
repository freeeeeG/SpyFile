using System;
using GameModes;
using GameModes.Horde;
using Team17.Online;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B56 RID: 2902
[RequireComponent(typeof(Animator))]
public class WorldMapKitchenLevelIconUI : WorldMapLevelIconUI
{
	// Token: 0x06003AF8 RID: 15096 RVA: 0x001190B5 File Offset: 0x001174B5
	protected override void Awake()
	{
		base.Awake();
		this.m_stars = base.gameObject.RequestComponentsRecursive<ScoreBoundaryStar>();
	}

	// Token: 0x06003AF9 RID: 15097 RVA: 0x001190D0 File Offset: 0x001174D0
	public void Setup(SceneDirectoryData.SceneDirectoryEntry _sceneData, GameProgress.GameProgressData.LevelProgress _levelProgress, WorldMapKitchenLevelIconUI.State _state)
	{
		this.m_sceneData = _sceneData;
		this.SetState(_state);
		this.UpdateStarVisibility(_levelProgress);
		this.SetUserScores(_levelProgress.LevelId);
		if (_sceneData != null)
		{
			base.SetTitle(_sceneData.Label);
			this.SetCost(_sceneData.StarCost);
			for (int i = 0; i < this.m_levelImages.Length; i++)
			{
				Image image = this.m_levelImages[i];
				if (image != null)
				{
					image.sprite = _sceneData.LoadScreenOverride;
				}
			}
		}
	}

	// Token: 0x06003AFA RID: 15098 RVA: 0x00119158 File Offset: 0x00117558
	public void SetCost(int _cost)
	{
		for (int i = 0; i < this.m_costText.Length; i++)
		{
			Text text = this.m_costText[i];
			if (!(text == null))
			{
				text.text = _cost.ToString();
			}
		}
	}

	// Token: 0x06003AFB RID: 15099 RVA: 0x001191AC File Offset: 0x001175AC
	public void UpdateStarVisibility(GameProgress.GameProgressData.LevelProgress _levelProgress)
	{
		ScoreBoundaryStar uistar = this.GetUIStar(1);
		ScoreBoundaryStar uistar2 = this.GetUIStar(2);
		ScoreBoundaryStar uistar3 = this.GetUIStar(3);
		if (uistar == null || uistar2 == null || uistar3 == null)
		{
			return;
		}
		ScoreBoundaryStar uistar4 = this.GetUIStar(4);
		if (uistar4 == null)
		{
			return;
		}
		SceneDirectoryData.PerPlayerCountDirectoryEntry sceneVarient = this.m_sceneData.GetSceneVarient(ClientUserSystem.m_Users.Count);
		if (sceneVarient != null)
		{
			uistar.Score = sceneVarient.OneStarScore;
			uistar2.Score = sceneVarient.TwoStarScore;
			uistar3.Score = sceneVarient.ThreeStarScore;
			for (int i = 0; i < this.m_stars.Length; i++)
			{
				ScoreBoundaryStar scoreBoundaryStar = this.m_stars[i];
				if (scoreBoundaryStar != null)
				{
					scoreBoundaryStar.SetUnlocked(scoreBoundaryStar.m_star <= _levelProgress.ScoreStars);
				}
			}
			GameSession gameSession = GameUtils.GetGameSession();
			if (gameSession != null)
			{
				GameProgress progress = gameSession.Progress;
				if (progress != null)
				{
					uistar4.gameObject.SetActive(_levelProgress.NGPEnabled);
					if (_levelProgress.NGPEnabled)
					{
						uistar4.Score = sceneVarient.FourStarScore;
					}
				}
			}
		}
	}

	// Token: 0x06003AFC RID: 15100 RVA: 0x001192F4 File Offset: 0x001176F4
	private void SetUserScores(int _levelID)
	{
		int count = ClientUserSystem.m_Users.Count;
		GameSession gameSession = GameUtils.GetGameSession();
		HighScoreRepository highScoreRepository = gameSession.HighScoreRepository;
		for (int i = 0; i < this.m_userScoreUis.Length; i++)
		{
			WorldMapKitchenLevelIconUI.UserScoreUI userScoreUI = this.m_userScoreUis[i];
			if (i < count)
			{
				User user = ClientUserSystem.m_Users._items[i];
				userScoreUI.SetName(user.DisplayName);
				if (user.Engagement != EngagementSlot.One || user.Split == User.SplitStatus.SplitPadGuest)
				{
					userScoreUI.SetScoreText(this.m_noScoreText);
				}
				else
				{
					LevelConfigBase levelConfig = this.m_sceneData.SceneVarients[0].LevelConfig;
					GameProgress.HighScores.Score score = null;
					if (highScoreRepository.GetScore(user.Machine, _levelID, ref score))
					{
						Kind gameModeKind = gameSession.GameModeKind;
						if (gameModeKind != Kind.Campaign)
						{
							if (gameModeKind == Kind.Survival)
							{
								if (score.iSurvivalModeTime != 0)
								{
									userScoreUI.SetScoreText(this.FormatScore(score.iSurvivalModeTime, WorldMapKitchenLevelIconUI.ScoreFormat.Time));
								}
								else
								{
									userScoreUI.SetScoreText(this.m_noScoreText);
								}
							}
						}
						else if (score.iHighScore != 65535 && score.iHighScore != -2147483648)
						{
							if (levelConfig.GetType() == typeof(HordeLevelConfig))
							{
								int requiredBitCount = GameUtils.GetRequiredBitCount(65535);
								float num = FloatUtils.FromUnorm(score.iHighScore, requiredBitCount);
								userScoreUI.SetScoreText(string.Format("{0:P0}", num));
							}
							else
							{
								userScoreUI.SetScoreText(this.FormatScore(score.iHighScore, WorldMapKitchenLevelIconUI.ScoreFormat.Number));
							}
						}
						else
						{
							userScoreUI.SetScoreText(this.m_noScoreText);
						}
					}
					else
					{
						userScoreUI.SetScoreText(this.m_noScoreText);
					}
				}
			}
			userScoreUI.SetActive(i < count);
		}
	}

	// Token: 0x06003AFD RID: 15101 RVA: 0x001194C4 File Offset: 0x001178C4
	protected virtual string FormatScore(int _score, WorldMapKitchenLevelIconUI.ScoreFormat _format)
	{
		if (_format == WorldMapKitchenLevelIconUI.ScoreFormat.Number)
		{
			return _score.ToString();
		}
		if (_format != WorldMapKitchenLevelIconUI.ScoreFormat.Time)
		{
			return string.Empty;
		}
		return _score.ToTimeString();
	}

	// Token: 0x06003AFE RID: 15102 RVA: 0x001194F4 File Offset: 0x001178F4
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

	// Token: 0x06003AFF RID: 15103 RVA: 0x00119549 File Offset: 0x00117949
	public void SetState(WorldMapKitchenLevelIconUI.State _state)
	{
		this.m_animator.SetInteger(WorldMapKitchenLevelIconUI.m_iPurchaseState, (int)_state);
	}

	// Token: 0x04002FF5 RID: 12277
	[SerializeField]
	private Text[] m_costText;

	// Token: 0x04002FF6 RID: 12278
	private static readonly int m_iPurchaseState = Animator.StringToHash("PurchaseState");

	// Token: 0x04002FF7 RID: 12279
	[HideInInspector]
	private ScoreBoundaryStar[] m_stars;

	// Token: 0x04002FF8 RID: 12280
	private SceneDirectoryData.SceneDirectoryEntry m_sceneData;

	// Token: 0x04002FF9 RID: 12281
	[SerializeField]
	private Image[] m_levelImages;

	// Token: 0x04002FFA RID: 12282
	[SerializeField]
	private string m_noScoreText = "----";

	// Token: 0x04002FFB RID: 12283
	[SerializeField]
	private WorldMapKitchenLevelIconUI.UserScoreUI[] m_userScoreUis;

	// Token: 0x02000B57 RID: 2903
	[Serializable]
	public enum ScoreFormat
	{
		// Token: 0x04002FFD RID: 12285
		Number,
		// Token: 0x04002FFE RID: 12286
		Time
	}

	// Token: 0x02000B58 RID: 2904
	[Serializable]
	private class UserScoreUI
	{
		// Token: 0x06003B02 RID: 15106 RVA: 0x00119575 File Offset: 0x00117975
		public void SetActive(bool _active)
		{
			this.m_container.SetActive(_active);
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x00119584 File Offset: 0x00117984
		public void SetName(string _name)
		{
			string text = _name;
			if (_name.Length > 15)
			{
				text = text.Substring(0, 12) + "…";
			}
			this.m_name.text = text;
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x001195C0 File Offset: 0x001179C0
		public void SetScoreText(string _text)
		{
			if (this.m_score == null)
			{
				return;
			}
			this.m_score.text = _text;
		}

		// Token: 0x04002FFF RID: 12287
		[SerializeField]
		private GameObject m_container;

		// Token: 0x04003000 RID: 12288
		[SerializeField]
		private Text m_name;

		// Token: 0x04003001 RID: 12289
		[SerializeField]
		private Text m_score;
	}

	// Token: 0x02000B59 RID: 2905
	public enum State
	{
		// Token: 0x04003003 RID: 12291
		UnAffordable,
		// Token: 0x04003004 RID: 12292
		Affordable,
		// Token: 0x04003005 RID: 12293
		Purchased,
		// Token: 0x04003006 RID: 12294
		UnSupported
	}
}
