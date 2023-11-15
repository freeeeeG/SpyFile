using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000AA RID: 170
public class UI_Panel_Battle_DifficultyLevel : MonoBehaviour
{
	// Token: 0x060005E1 RID: 1505 RVA: 0x00021B47 File Offset: 0x0001FD47
	private void Update()
	{
		this.UpdateInput();
		if (MyInput.IfGetCloseButtonDown())
		{
			this.Close();
		}
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x00021B5C File Offset: 0x0001FD5C
	public void Open()
	{
		if (!BattleManager.inst.GameOn)
		{
			Debug.LogWarning("游戏已结束，无法打开DL");
			return;
		}
		base.gameObject.SetActive(true);
		this.UpdateListPos();
		this.UpdateInfo();
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x00008887 File Offset: 0x00006A87
	public void Close()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x00021B90 File Offset: 0x0001FD90
	private void UpdateListPos()
	{
		if (Battle.inst.diffiLevel < Battle.inst.DiffiLevelMax)
		{
			this.listNow.SetActive(true);
			this.listNext.SetActive(true);
			this.listNow.transform.localPosition = new Vector2(-this.listX, this.listY);
			this.listNext.transform.localPosition = new Vector2(this.listX, this.listY);
			return;
		}
		this.listNow.SetActive(true);
		this.listNext.SetActive(false);
		this.listNow.transform.localPosition = new Vector2(0f, this.listY);
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x00021C56 File Offset: 0x0001FE56
	private void OnEnable()
	{
		if (TempData.inst.modeType == EnumModeType.WANDER && BattleManager.inst.wander_On)
		{
			TimeManager.inst.PauseSet();
		}
		TutorialMaster.inst.Activate();
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x00021C85 File Offset: 0x0001FE85
	private void OnDisable()
	{
		if (TempData.inst.modeType == EnumModeType.WANDER && BattleManager.inst.wander_On)
		{
			TimeManager.inst.PauseRecover();
		}
		TutorialMaster.inst.Activate();
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x00021CB4 File Offset: 0x0001FEB4
	private void UpdateInput()
	{
		if (MyInput.GetKeyDownWithSound(KeyCode.R))
		{
			this.DiffiLevelUp();
		}
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x00021CC8 File Offset: 0x0001FEC8
	private void UpdateInfo()
	{
		int diffiLevel = Battle.inst.diffiLevel;
		int diffiLevelMax = Battle.inst.DiffiLevelMax;
		string name = GameParameters.Inst.difficultyLevel_FactorBattle[diffiLevel].name;
		LanguageText.ChallengeMenu challengeMenu = LanguageText.Inst.challengeMenu;
		this.panel_Title.text = challengeMenu.panel_challenge;
		this.now_Title.text = challengeMenu.title_CurrentDiffilevel + "\n" + name;
		this.now_Info.text = UI_ToolTipInfo.GetInfo_DifficultyLevel(diffiLevel, false);
		this.button_Return.text = challengeMenu.button_Return.AppendKeycode("E");
		if (diffiLevel < diffiLevelMax)
		{
			string name2 = GameParameters.Inst.difficultyLevel_FactorBattle[diffiLevel + 1].name;
			this.next_Title.text = challengeMenu.title_DiffiLevelUpPreview + "\n" + name2;
			this.next_Info.text = UI_ToolTipInfo.GetInfo_DifficultyLevel(diffiLevel + 1, true);
			this.button_LevelUp.text = challengeMenu.button_LevelUp.AppendKeycode("R");
			return;
		}
		this.button_LevelUp.text = challengeMenu.button_LevelMax;
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x00021DDC File Offset: 0x0001FFDC
	public void DiffiLevelUp()
	{
		if (Battle.inst.diffiLevel < Battle.inst.DiffiLevelMax)
		{
			Battle.inst.diffiLevel++;
			this.UpdateListPos();
			this.UpdateInfo();
			UI_FloatTextControl.inst.Special_DiffiLevelUp();
			return;
		}
		UI_FloatTextControl.inst.Special_DiffiLevelMax();
		Debug.LogWarning("已满级！");
	}

	// Token: 0x040004DA RID: 1242
	[SerializeField]
	private float listX = 300f;

	// Token: 0x040004DB RID: 1243
	[SerializeField]
	private float listY = 250f;

	// Token: 0x040004DC RID: 1244
	[SerializeField]
	private GameObject listNow;

	// Token: 0x040004DD RID: 1245
	[SerializeField]
	private GameObject listNext;

	// Token: 0x040004DE RID: 1246
	[SerializeField]
	private Text panel_Title;

	// Token: 0x040004DF RID: 1247
	[SerializeField]
	private Text now_Title;

	// Token: 0x040004E0 RID: 1248
	[SerializeField]
	private Text now_Info;

	// Token: 0x040004E1 RID: 1249
	[SerializeField]
	private Text next_Title;

	// Token: 0x040004E2 RID: 1250
	[SerializeField]
	private Text next_Info;

	// Token: 0x040004E3 RID: 1251
	[SerializeField]
	private Text button_LevelUp;

	// Token: 0x040004E4 RID: 1252
	[SerializeField]
	private Text button_Return;
}
