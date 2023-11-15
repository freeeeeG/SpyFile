using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000AD RID: 173
public class UI_Panel_Battle_Pause : MonoBehaviour
{
	// Token: 0x060005F4 RID: 1524 RVA: 0x00021EC0 File Offset: 0x000200C0
	public void Open()
	{
		this.open = true;
		this.UpdateLanguage();
		this.SetConfirmWindow(0, false);
		this.SetConfirmWindow(1, false);
		this.SetConfirmWindow(2, false);
		this.SetConfirmWindow(3, false);
		this.confirm_DeleteUpgrade.SetActive(false);
		base.gameObject.SetActive(true);
		this.obj_PanelMain.SetActive(true);
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.buttonList);
		BattleMapCanvas.inst.panel_FactorBattle.InitIcons(null);
		BattleMapCanvas.inst.panel_UpgradeShow.InitIcons(null);
		BattleMapCanvas.inst.panel_DifficultyOptionShow.InitIcons(null);
		BattleMapCanvas.inst.panel_SkillModShow.InitIcons(null);
		int int_Row = BattleMapCanvas.inst.panel_DifficultyOptionShow.GetInt_Row();
		BattleMapCanvas.inst.panel_SkillModShow.transform.localPosition = BattleMapCanvas.inst.panel_DifficultyOptionShow.transform.localPosition - new Vector3(0f, (float)(int_Row * 48 + 24), 0f);
		this.UpdateButtonColor();
		int jobId = TempData.inst.jobId;
		int skillLevel = GameData.inst.jobs[jobId].skillLevel;
		this.iconSkill.Init(jobId, skillLevel);
		this.iconRuneButton.UpdateIcon();
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Update()
	{
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x00021FF8 File Offset: 0x000201F8
	public void TryClose()
	{
		if (this.panelSetting.gameObject.activeSelf)
		{
			this.ClosePanel_Settings();
			return;
		}
		if (this.confirm_GiveUp.gameObject.activeSelf)
		{
			this.CloseConfirmWindow(0);
			return;
		}
		if (this.confirm_Save.gameObject.activeSelf)
		{
			this.CloseConfirmWindow(1);
			return;
		}
		if (this.tips_CantSave.gameObject.activeSelf)
		{
			this.CloseConfirmWindow(2);
			return;
		}
		if (this.tips_CantSave_PlayerDie.gameObject.activeSelf)
		{
			this.CloseConfirmWindow(3);
			return;
		}
		this.Close();
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0002208C File Offset: 0x0002028C
	public void WantoSaveAndQuit()
	{
		if (!BattleManager.inst.GameOn)
		{
			this.OpenConfirmWindow(3);
			return;
		}
		if (TempData.inst.modeType == EnumModeType.WANDER)
		{
			if (BattleManager.inst.GameOn)
			{
				this.OpenConfirmWindow(1);
				return;
			}
			Debug.LogWarning("玩家已死亡提示待补充");
			this.OpenConfirmWindow(2);
			return;
		}
		else
		{
			if (BattleManager.inst.timeStage == EnumTimeStage.REST)
			{
				this.OpenConfirmWindow(1);
				return;
			}
			this.OpenConfirmWindow(2);
			return;
		}
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x000220FC File Offset: 0x000202FC
	public void Close()
	{
		Debug.Log("关闭暂停面板");
		TimeManager.inst.PauseRecover();
		this.open = false;
		this.obj_PanelMain.SetActive(false);
		this.confirm_GiveUp.SetActive(false);
		this.confirm_Save.SetActive(false);
		this.tips_CantSave.SetActive(false);
		this.tips_CantSave_PlayerDie.SetActive(false);
		this.confirm_DeleteUpgrade.SetActive(false);
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0002216C File Offset: 0x0002036C
	private void OnEnable()
	{
		TutorialMaster.inst.DeActivate();
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x00022178 File Offset: 0x00020378
	private void OnDisable()
	{
		UI_ToolTip.inst.Close();
		TutorialMaster.inst.Activate();
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x00022190 File Offset: 0x00020390
	private void UpdateButtonColor()
	{
		Color button_Highlight = UI_Setting.Inst.button_Highlight;
		foreach (Button button in base.GetComponentsInChildren<Button>())
		{
			ColorBlock colors = button.colors;
			colors.highlightedColor = button_Highlight;
			button.colors = colors;
		}
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x000221D5 File Offset: 0x000203D5
	public void OpenPanel_Settings()
	{
		this.obj_PanelMain.SetActive(false);
		this.panelSetting.Open();
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x000221EE File Offset: 0x000203EE
	public void ClosePanel_Settings()
	{
		this.panelSetting.Close();
		this.obj_PanelMain.SetActive(true);
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x00022208 File Offset: 0x00020408
	private void SetConfirmWindow(int type, bool openFlag)
	{
		GameObject gameObject = null;
		switch (type)
		{
		case 0:
			gameObject = this.confirm_GiveUp;
			break;
		case 1:
			gameObject = this.confirm_Save;
			break;
		case 2:
			gameObject = this.tips_CantSave;
			break;
		case 3:
			gameObject = this.tips_CantSave_PlayerDie;
			break;
		default:
			Debug.LogError("TypeError!");
			break;
		}
		gameObject.SetActive(openFlag);
		this.UpdateButtonColor();
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x0002226A File Offset: 0x0002046A
	public void OpenConfirmWindow(int type)
	{
		this.SetConfirmWindow(type, true);
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x00022274 File Offset: 0x00020474
	public void CloseConfirmWindow(int type)
	{
		this.SetConfirmWindow(type, false);
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x00022280 File Offset: 0x00020480
	public void UpdateLanguage()
	{
		LanguageText inst = LanguageText.Inst;
		LanguageText.PauseMenu pauseMenu = inst.pauseMenu;
		this.button_Continue.text = pauseMenu.continueGame;
		this.button_GiveUp.text = pauseMenu.giveUp;
		this.button_SaveQuit.text = pauseMenu.saveAndExit;
		this.info_Confirm_GiveUp.text = pauseMenu.confirm_GiveUp;
		this.info_Confirm_Save.text = pauseMenu.confirm_Save;
		this.title_Pause.text = pauseMenu.title_Pause;
		Text[] array = this.titles_Tips;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].text = inst.confirm_Tips;
		}
		array = this.buttons_Confirm;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].text = inst.confirm_Confirm;
		}
		this.info_Tips_CantSave.text = pauseMenu.info_CantSave;
		this.info_Tips_CantSave_PlayerDie.text = pauseMenu.info_CantSave_PlayerDie;
		this.button_Setting.text = inst.mainMenu.main_Setting;
		array = this.title_Confirm;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].text = inst.confirm_Title;
		}
		array = this.button_Confirm_Yes;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].text = inst.confirm_Yes;
		}
		array = this.button_Confirm_No;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].text = inst.confirm_No;
		}
	}

	// Token: 0x040004E5 RID: 1253
	public bool open;

	// Token: 0x040004E6 RID: 1254
	[SerializeField]
	private Text button_Continue;

	// Token: 0x040004E7 RID: 1255
	[SerializeField]
	private Text button_GiveUp;

	// Token: 0x040004E8 RID: 1256
	[SerializeField]
	private Text button_SaveQuit;

	// Token: 0x040004E9 RID: 1257
	[SerializeField]
	private GameObject confirm_GiveUp;

	// Token: 0x040004EA RID: 1258
	[SerializeField]
	private GameObject confirm_Save;

	// Token: 0x040004EB RID: 1259
	[SerializeField]
	private GameObject tips_CantSave;

	// Token: 0x040004EC RID: 1260
	[SerializeField]
	private GameObject tips_CantSave_PlayerDie;

	// Token: 0x040004ED RID: 1261
	[SerializeField]
	private RectTransform buttonList;

	// Token: 0x040004EE RID: 1262
	[SerializeField]
	private GameObject confirm_DeleteUpgrade;

	// Token: 0x040004EF RID: 1263
	[Header("Panels")]
	[SerializeField]
	public GameObject obj_PanelMain;

	// Token: 0x040004F0 RID: 1264
	[SerializeField]
	public UI_Panel_Setting panelSetting;

	// Token: 0x040004F1 RID: 1265
	[Header("Icons")]
	[SerializeField]
	private UI_Icon_Skill iconSkill;

	// Token: 0x040004F2 RID: 1266
	[SerializeField]
	private UI_Icon_RuneButton iconRuneButton;

	// Token: 0x040004F3 RID: 1267
	[Header("Languages")]
	[SerializeField]
	private Text[] title_Confirm;

	// Token: 0x040004F4 RID: 1268
	[SerializeField]
	private Text button_Setting;

	// Token: 0x040004F5 RID: 1269
	[SerializeField]
	private Text info_Confirm_GiveUp;

	// Token: 0x040004F6 RID: 1270
	[SerializeField]
	private Text info_Confirm_Save;

	// Token: 0x040004F7 RID: 1271
	[SerializeField]
	private Text[] button_Confirm_Yes;

	// Token: 0x040004F8 RID: 1272
	[SerializeField]
	private Text[] button_Confirm_No;

	// Token: 0x040004F9 RID: 1273
	[SerializeField]
	private Text title_Pause;

	// Token: 0x040004FA RID: 1274
	[Header("提示的标题与确认们")]
	[SerializeField]
	private Text[] titles_Tips;

	// Token: 0x040004FB RID: 1275
	[SerializeField]
	private Text[] buttons_Confirm;

	// Token: 0x040004FC RID: 1276
	[Header("不能保存 非准备阶段")]
	[SerializeField]
	private Text info_Tips_CantSave;

	// Token: 0x040004FD RID: 1277
	[Header("不能保存 玩家已死亡")]
	[SerializeField]
	private Text info_Tips_CantSave_PlayerDie;
}
