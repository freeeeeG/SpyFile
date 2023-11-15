using System;
using GameModes;
using GameModes.Horde;
using UnityEngine;

// Token: 0x02000B34 RID: 2868
public class LoadingScreenUIController : MonoBehaviour
{
	// Token: 0x06003A24 RID: 14884 RVA: 0x00114B96 File Offset: 0x00112F96
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x06003A25 RID: 14885 RVA: 0x00114BA0 File Offset: 0x00112FA0
	private void Setup()
	{
		bool flag = false;
		this.m_loadingScreenFlow = base.gameObject.RequireComponent<LoadingScreenFlow>();
		this.m_gameSession = GameUtils.GetGameSession();
		if (this.m_gameSession != null)
		{
			GameProgress progress = this.m_gameSession.Progress;
			SceneDirectoryData sceneDirectory = progress.GetSceneDirectory();
			if (sceneDirectory != null)
			{
				SceneDirectoryData.SceneDirectoryEntry[] scenes = sceneDirectory.Scenes;
				GameSession.GameTypeSettings typeSettings = this.m_gameSession.TypeSettings;
				GameProgress.GameProgressData saveableData = progress.SaveableData;
				SceneDirectoryData.PerPlayerCountDirectoryEntry sceneDirectoryVarientEntry = this.m_gameSession.LevelSettings.SceneDirectoryVarientEntry;
				if (scenes != null && sceneDirectoryVarientEntry != null && !this.m_loadingScreenFlow.NextScene.Equals(typeSettings.WorldMapScene) && !this.m_loadingScreenFlow.NextScene.Equals("StartScreen"))
				{
					int levelID = GameUtils.GetLevelID();
					if (levelID >= 0 && levelID < scenes.Length && scenes[levelID].UseKitchenLoadingScreen)
					{
						bool isNewGamePlus = progress.SaveData.IsNGPEnabledForLevel(levelID) && progress.SaveData.NewGamePlusDialogShown;
						this.SetupKitchenLoad(scenes[levelID].Label, sceneDirectoryVarientEntry, saveableData.GetLevelProgress(levelID), isNewGamePlus);
						flag = true;
					}
				}
			}
		}
		if (!flag)
		{
			this.SetupGenericLoad();
		}
	}

	// Token: 0x06003A26 RID: 14886 RVA: 0x00114CDF File Offset: 0x001130DF
	private void SetupGenericLoad()
	{
		this.m_genericLoadingScreen.SetActive(true);
		this.m_levelLoadingScreen.SetActive(false);
	}

	// Token: 0x06003A27 RID: 14887 RVA: 0x00114CFC File Offset: 0x001130FC
	private void SetupKitchenLoad(string levelName, SceneDirectoryData.PerPlayerCountDirectoryEntry entry, GameProgress.GameProgressData.LevelProgress levelProgress, bool isNewGamePlus)
	{
		this.m_genericLoadingScreen.SetActive(false);
		this.m_levelLoadingScreen.SetActive(true);
		if (this.m_levelName != null)
		{
			this.m_levelName.SetLocalisedTextCatchAll(levelName);
		}
		if (this.m_previewImage != null)
		{
			this.m_previewImage.sprite = entry.Screenshot;
		}
		if (ClientGameSetup.Mode == GameMode.Campaign || ClientGameSetup.Mode == GameMode.Party)
		{
			Kind gameModeKind = this.m_gameSession.GameModeKind;
			if (gameModeKind != Kind.Campaign)
			{
				if (gameModeKind != Kind.Practice)
				{
					if (gameModeKind == Kind.Survival)
					{
						this.m_campaignModeUI.gameObject.SetActive(false);
						this.m_practiceModeUI.gameObject.SetActive(false);
						this.m_survivalModeUI.gameObject.SetActive(true);
						this.m_survivalModeTimeDisplay.Value = levelProgress.SurvivalModeTime;
					}
				}
				else
				{
					this.m_campaignModeUI.gameObject.SetActive(false);
					this.m_practiceModeUI.gameObject.SetActive(true);
					this.m_survivalModeUI.gameObject.SetActive(false);
				}
			}
			else
			{
				this.m_campaignModeUI.gameObject.SetActive(true);
				this.m_practiceModeUI.gameObject.SetActive(false);
				this.m_survivalModeUI.gameObject.SetActive(false);
				if (entry.LevelConfig != null && entry.LevelConfig as HordeLevelConfig != null)
				{
					if (this.m_highScore != null)
					{
						int highScore = levelProgress.HighScore;
						if (highScore == -2147483648)
						{
							this.m_highScore.gameObject.SetActive(false);
						}
						else
						{
							this.m_highScore.gameObject.SetActive(true);
							int requiredBitCount = GameUtils.GetRequiredBitCount(65535);
							float num = FloatUtils.FromUnorm(highScore, requiredBitCount);
							this.m_highScore.SetNonLocalizedText(Localization.Get("LoadingScreen.BestScore", new LocToken[]
							{
								new LocToken("[Score]", string.Format("{0:P0}", num))
							}));
						}
					}
				}
				else
				{
					if (this.m_highScore != null)
					{
						int num2 = levelProgress.HighScore;
						if (num2 == -2147483648)
						{
							num2 = 0;
						}
						this.m_highScore.SetNonLocalizedText(Localization.Get("LoadingScreen.BestScore", new LocToken[]
						{
							new LocToken("[Score]", num2.ToString())
						}));
					}
					if (this.m_stars == null && this.m_starsPrefab != null && this.m_starContainer != null)
					{
						GameObject gameObject = this.m_starsPrefab.InstantiateOnParent(this.m_starContainer, true);
						gameObject.transform.localScale = Vector3.one;
						this.m_stars = gameObject.gameObject.RequestComponentsRecursive<ScoreBoundaryStar>();
					}
					if (this.m_stars != null)
					{
						for (int i = 0; i < this.m_stars.Length; i++)
						{
							if (this.m_stars[i] != null)
							{
								this.m_stars[i].Score = entry.GetPointsForStar(this.m_stars[i].m_star);
								if (this.m_stars[i].m_star > 3)
								{
									this.m_stars[i].gameObject.SetActive(isNewGamePlus);
								}
							}
						}
						int scoreStars = levelProgress.ScoreStars;
						for (int j = 0; j < this.m_stars.Length; j++)
						{
							if (this.m_stars[j] != null)
							{
								this.m_stars[j].SetUnlocked(this.m_stars[j].m_star <= scoreStars);
							}
						}
					}
				}
			}
		}
		else
		{
			this.m_campaignModeUI.gameObject.SetActive(false);
			this.m_practiceModeUI.gameObject.SetActive(false);
			this.m_survivalModeUI.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003A28 RID: 14888 RVA: 0x00115103 File Offset: 0x00113503
	private void Update()
	{
		if (this.m_progressBar != null && this.m_loadingScreenFlow != null)
		{
			this.m_progressBar.SetValue(this.m_loadingScreenFlow.Progress);
		}
	}

	// Token: 0x04002F15 RID: 12053
	[SerializeField]
	private ProgressBarUI m_progressBar;

	// Token: 0x04002F16 RID: 12054
	[Header("Generic Scene Load")]
	[SerializeField]
	private GameObject m_genericLoadingScreen;

	// Token: 0x04002F17 RID: 12055
	[Header("Kitchen Scenes")]
	[SerializeField]
	private GameObject m_levelLoadingScreen;

	// Token: 0x04002F18 RID: 12056
	[SerializeField]
	private T17Text m_levelName;

	// Token: 0x04002F19 RID: 12057
	[SerializeField]
	private T17Text m_highScore;

	// Token: 0x04002F1A RID: 12058
	[SerializeField]
	private T17Image m_previewImage;

	// Token: 0x04002F1B RID: 12059
	[Header("Campaign Mode")]
	[SerializeField]
	private GameObject m_campaignModeUI;

	// Token: 0x04002F1C RID: 12060
	[SerializeField]
	[AssignChildRecursive("StarContainer", Editorbility.Editable)]
	private Transform m_starContainer;

	// Token: 0x04002F1D RID: 12061
	[SerializeField]
	[AssignResource("NGP_LoadingStars", Editorbility.Editable)]
	private GameObject m_starsPrefab;

	// Token: 0x04002F1E RID: 12062
	[Header("Practice Mode")]
	[SerializeField]
	private GameObject m_practiceModeUI;

	// Token: 0x04002F1F RID: 12063
	[Header("Survival Mode")]
	[SerializeField]
	private GameObject m_survivalModeUI;

	// Token: 0x04002F20 RID: 12064
	[SerializeField]
	private DisplayTimeUIController m_survivalModeTimeDisplay;

	// Token: 0x04002F21 RID: 12065
	private LoadingScreenFlow m_loadingScreenFlow;

	// Token: 0x04002F22 RID: 12066
	private ScoreBoundaryStar[] m_stars;

	// Token: 0x04002F23 RID: 12067
	private GameSession m_gameSession;
}
