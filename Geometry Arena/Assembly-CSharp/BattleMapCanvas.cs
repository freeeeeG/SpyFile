using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020000A0 RID: 160
public class BattleMapCanvas : MonoBehaviour
{
	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x06000583 RID: 1411 RVA: 0x0001FC8D File Offset: 0x0001DE8D
	// (set) Token: 0x06000584 RID: 1412 RVA: 0x0001FC9F File Offset: 0x0001DE9F
	private bool ActiveUpgradeShop
	{
		get
		{
			return this.panel_UpgradeShop.gameObject.activeSelf;
		}
		set
		{
			this.panel_UpgradeShop.gameObject.SetActive(value);
		}
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000585 RID: 1413 RVA: 0x0001FCB2 File Offset: 0x0001DEB2
	// (set) Token: 0x06000586 RID: 1414 RVA: 0x0001FCC4 File Offset: 0x0001DEC4
	private bool ActiveChallengeShop
	{
		get
		{
			return this.panel_Challenge.gameObject.activeSelf;
		}
		set
		{
			this.panel_Challenge.gameObject.SetActive(value);
		}
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x0001FCD7 File Offset: 0x0001DED7
	private void Awake()
	{
		BattleMapCanvas.inst = this;
		this.SetPauseMenu(false);
		this.panel_BattleAward.gameObject.SetActive(false);
		this.CloseAllWindows();
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x0001FD00 File Offset: 0x0001DF00
	public void ApplySetting()
	{
		Setting setting = Setting.Inst;
		this.panel_Ability.gameObject.SetActive(setting.Show_Battle_Ability);
		this.panel_FactorBattle.gameObject.SetActive(setting.Show_Battle_FactorBattle);
		this.panel_UpgradeShow.gameObject.SetActive(setting.Show_Battle_Upgrade);
		HealthPointControl.inst.UpdateHpUnits();
		this.fpsShow.gameObject.SetActive(setting.Option_ShowFPS);
		this.playerPositionIndicator.gameObject.SetActive(setting.Option_PositionIndicator);
		this.UpdateLanguageText();
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x0001FD91 File Offset: 0x0001DF91
	private void Start()
	{
		this.ApplySetting();
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x0001FD9C File Offset: 0x0001DF9C
	private void Update()
	{
		if (MyInput.GetKeyDownWithSound(KeyCode.Escape))
		{
			if (!this.panel_BattleAward.gameObject.activeSelf && !this.panel_Pause.open && !this.IfAnyWindowActive())
			{
				this.SetPauseMenu(true);
			}
			else if (this.panel_Pause.open)
			{
				this.panel_Pause.TryClose();
			}
		}
		this.DetectWindowKeyInUpdate();
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x0001FE00 File Offset: 0x0001E000
	public void UpdateLanguageText()
	{
		LanguageText languageText = LanguageText.Inst;
		ResourceLibrary resourceLibrary = ResourceLibrary.Inst;
		this.icon_Score.sprite = resourceLibrary.Sprite_Score;
		this.icon_Fragment.sprite = resourceLibrary.Sprite_Fragment;
		this.UpdateTimeStageIndicator();
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x0001FE41 File Offset: 0x0001E041
	public bool IfAnyWindowActive()
	{
		return (this.panel_Challenge && this.ActiveChallengeShop) || (this.panel_UpgradeShop && this.ActiveUpgradeShop);
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x0001FE74 File Offset: 0x0001E074
	public void DetectWindowKeyInUpdate()
	{
		if (this.panel_Pause.open)
		{
			return;
		}
		if (TempData.inst.modeType == EnumModeType.WANDER || BattleManager.inst.timeStage == EnumTimeStage.REST)
		{
			if (MyInput.GetKeyDownWithSound(KeyCode.Q))
			{
				if (this.IfAnyWindowActive())
				{
					if (this.ActiveUpgradeShop)
					{
						this.CloseAllWindows();
					}
					else
					{
						this.CloseAllWindows();
						this.OpenUpgradeShop();
					}
				}
				else
				{
					this.OpenUpgradeShop();
				}
			}
			if (MyInput.GetKeyDownWithSound(KeyCode.E))
			{
				if (this.IfAnyWindowActive())
				{
					if (this.ActiveChallengeShop)
					{
						this.CloseAllWindows();
					}
					else
					{
						this.CloseAllWindows();
						this.panel_Challenge.Open();
					}
				}
				else
				{
					this.panel_Challenge.Open();
				}
			}
		}
		if (BattleManager.inst.timeStage == EnumTimeStage.REST && MyInput.GetKeyDownWithSound(KeyCode.G))
		{
			if (this.IfAnyWindowActive())
			{
				this.CloseAllWindows();
			}
			BattleManager.inst.TimeStage_Switch_Waving();
		}
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x0001FF4C File Offset: 0x0001E14C
	public void UpdateProcessPercent(EnumTimeStage timeStage, float percent)
	{
		percent = Mathf.Min(percent, 1f);
		if (timeStage == EnumTimeStage.REST)
		{
			this.trans_ProcessPercent.gameObject.SetActive(false);
			this.trans_ProcessPercentOutline.gameObject.SetActive(false);
			return;
		}
		this.trans_ProcessPercent.gameObject.SetActive(true);
		this.trans_ProcessPercentOutline.gameObject.SetActive(true);
		this.trans_ProcessPercent.localScale = new Vector3(percent, 1f, 1f);
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x0001FFC9 File Offset: 0x0001E1C9
	public void CloseAllWindows()
	{
		this.panel_Challenge.Close();
		this.panel_UpgradeShop.Close();
		if (UI_ToolTip.inst != null)
		{
			UI_ToolTip.inst.Close();
		}
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x0001FFF8 File Offset: 0x0001E1F8
	public void UpdateTimeStageIndicator()
	{
		LanguageText languageText = LanguageText.Inst;
		int level = Battle.inst.level;
		int wave = Battle.inst.wave;
		EnumTimeStage enumTimeStage = BattleManager.inst.timeStage;
		string text = "";
		switch (TempData.inst.modeType)
		{
		case EnumModeType.NORMAL:
			text = string.Concat(new object[]
			{
				languageText.wave,
				" ",
				level,
				"-",
				wave,
				"\n"
			});
			break;
		case EnumModeType.ENDLESS:
			text = string.Concat(new object[]
			{
				languageText.wave,
				" ",
				languageText.level_Infinity,
				" ",
				wave,
				"\n"
			});
			break;
		case EnumModeType.WANDER:
			text = string.Concat(new object[]
			{
				languageText.wave,
				" ",
				languageText.level_Wander,
				" ",
				wave,
				"\n"
			});
			break;
		}
		if (enumTimeStage == EnumTimeStage.UNINITED)
		{
			enumTimeStage = EnumTimeStage.REST;
		}
		text += languageText.waveStage[(int)enumTimeStage];
		this.text_StageIndicator.text = text;
		Sprite sprite = null;
		switch (Battle.inst.CurrentLevelType)
		{
		case EnumLevelType.TRIANGLE:
			sprite = ResourceLibrary.Inst.GetSprite_Shape(EnumShapeType.TRIANGLE, true);
			break;
		case EnumLevelType.CIRCLE:
			sprite = ResourceLibrary.Inst.GetSprite_Shape(EnumShapeType.CIRCLE, true);
			break;
		case EnumLevelType.SQUARE:
			sprite = ResourceLibrary.Inst.GetSprite_Shape(EnumShapeType.SQUARE, true);
			break;
		case EnumLevelType.FINAL:
			sprite = ResourceLibrary.Inst.Sprite_Star;
			break;
		}
		this.image_ThemeIndicator.sprite = sprite;
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x000201B3 File Offset: 0x0001E3B3
	public void Button_SaveBattleAndQuit()
	{
		SaveFile.SaveByJson(true);
		Time.timeScale = 1f;
		TempData.inst.currentSceneType = EnumSceneType.MAINMENU;
		SceneManager.LoadScene(0);
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x000201D6 File Offset: 0x0001E3D6
	public void Button_GameOverAndQuit()
	{
		SaveFile.SaveByJson(false);
		Time.timeScale = 1f;
		TempData.inst.currentSceneType = EnumSceneType.MAINMENU;
		SceneManager.LoadScene(0);
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x000201F9 File Offset: 0x0001E3F9
	public void SetPauseMenu(bool flag)
	{
		if (flag)
		{
			this.panel_Pause.Open();
			Debug.Log("暂停！");
			TimeManager.inst.PauseSet();
			return;
		}
		this.panel_Pause.Close();
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x0002022C File Offset: 0x0001E42C
	public void UpdateScoreNum()
	{
		this.text_ScoreNum.text = Battle.inst.Score.ToString();
		this.text_ScoreNum.gameObject.GetComponent<Animator>().SetTrigger("Exp");
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x00020270 File Offset: 0x0001E470
	public void UpdateFragmentNum()
	{
		this.text_FragNum.text = Battle.inst.Fragment.ToString();
		this.text_FragNum.gameObject.GetComponent<Animator>().SetTrigger("Exp");
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x000202B4 File Offset: 0x0001E4B4
	public void OpenUpgradeShop()
	{
		if (!BattleManager.inst.GameOn)
		{
			Debug.LogWarning("游戏已结束，无法打开商店");
			return;
		}
		this.ActiveUpgradeShop = true;
		this.panel_UpgradeShop.Init();
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x000202DF File Offset: 0x0001E4DF
	public void Button_RestartGame()
	{
		SaveFile.SaveByJson(false);
		Time.timeScale = 1f;
		TempData.inst.currentSceneType = EnumSceneType.BATTLE;
		TempData.inst.NewGame();
		SceneManager.LoadScene("Scene_BattleMap");
	}

	// Token: 0x04000474 RID: 1140
	public static BattleMapCanvas inst;

	// Token: 0x04000475 RID: 1141
	[Header("Show")]
	[SerializeField]
	public UI_Panel_Battle_AbilityShow panel_Ability;

	// Token: 0x04000476 RID: 1142
	public UI_Panel_Battle_FactorBattleShow panel_FactorBattle;

	// Token: 0x04000477 RID: 1143
	public UI_Panel_Battle_UpgradeShow panel_UpgradeShow;

	// Token: 0x04000478 RID: 1144
	public UI_Panel_Battle_DifficultyOptionShow panel_DifficultyOptionShow;

	// Token: 0x04000479 RID: 1145
	public UI_Panel_Main_SkillModule panel_SkillModShow;

	// Token: 0x0400047A RID: 1146
	public Text fpsShow;

	// Token: 0x0400047B RID: 1147
	[Header("Numbers")]
	[SerializeField]
	private Text text_ScoreNum;

	// Token: 0x0400047C RID: 1148
	[SerializeField]
	private Text text_FragNum;

	// Token: 0x0400047D RID: 1149
	[SerializeField]
	private Text text_StageIndicator;

	// Token: 0x0400047E RID: 1150
	[SerializeField]
	private Image image_ThemeIndicator;

	// Token: 0x0400047F RID: 1151
	[SerializeField]
	private Transform trans_ProcessPercent;

	// Token: 0x04000480 RID: 1152
	[SerializeField]
	private Transform trans_ProcessPercentOutline;

	// Token: 0x04000481 RID: 1153
	[Header("Icons")]
	[SerializeField]
	private Image icon_Score;

	// Token: 0x04000482 RID: 1154
	[SerializeField]
	private Image icon_Fragment;

	// Token: 0x04000483 RID: 1155
	[Header("Windows")]
	[SerializeField]
	public UI_Panel_Battle_UpgradeShop panel_UpgradeShop;

	// Token: 0x04000484 RID: 1156
	[SerializeField]
	private UI_Panel_Battle_DifficultyLevel panel_Challenge;

	// Token: 0x04000485 RID: 1157
	[SerializeField]
	public UI_Panel_Battle_Pause panel_Pause;

	// Token: 0x04000486 RID: 1158
	[SerializeField]
	public UI_Panel_Battle_BattleAward panel_BattleAward;

	// Token: 0x04000487 RID: 1159
	public UI_Panel_Battle_ConfirmDeleteUpgrade panel_ConfirmDeleteUpgrade;

	// Token: 0x04000488 RID: 1160
	[SerializeField]
	private PlayerPositionIndicator playerPositionIndicator;
}
