using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C8 RID: 200
public class UI_Panel_Main_NewGame : MonoBehaviour
{
	// Token: 0x060006E0 RID: 1760 RVA: 0x000267DB File Offset: 0x000249DB
	private void Awake()
	{
		UI_Panel_Main_NewGame.inst = this;
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x000267E3 File Offset: 0x000249E3
	private void OnDisable()
	{
		MainCanvas.inst.DestroyPlayerPrevew();
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x000267EF File Offset: 0x000249EF
	public void Open()
	{
		base.gameObject.SetActive(true);
		this.UpdatePanel();
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x00026804 File Offset: 0x00024A04
	public void UpdatePanel()
	{
		this.panelRune.gameObject.SetActive(false);
		this.UpdateLanguageTexts();
		this.panelTalents.InitIcons(null);
		this.panelColors.InitIcons(null);
		this.panelJobs.InitIcons(null);
		this.panelDifficultyOptions.InitIcons(null);
		TempData.inst.battle = new Battle();
		TempData.inst.battle.UpdateBattleFacs();
		this.panelAbility.InitIcons(null);
		this.panelSkillModule.InitIcons(null);
		this.iconStar.UpdateIcon();
		this.iconGeometryCoin.UpdateIcon();
		for (int i = 0; i < this.iconModes.Length; i++)
		{
			this.iconModes[i].Init(i);
		}
		int jobId = TempData.inst.jobId;
		int skillLevel = GameData.inst.jobs[jobId].skillLevel;
		this.iconSkill.Init(jobId, skillLevel);
		this.iconRuneButton.UpdateIcon();
		this.iconProficiency.Init(jobId);
		RectTransform[] array = this.rectsToReforce;
		for (int j = 0; j < array.Length; j++)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(array[j]);
		}
		MySteamAchievement.DetectRoleProficiencyLevel();
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x00026930 File Offset: 0x00024B30
	private void UpdateLanguageTexts()
	{
		LanguageText languageText = LanguageText.Inst;
		int main_TitleSize = UI_Setting.Inst.main_TitleSize;
		this.language_Job.text = languageText.main_Job;
		this.language_Job.fontSize = main_TitleSize;
		this.language_Color.text = languageText.main_Color;
		this.language_Color.fontSize = main_TitleSize;
		this.language_Ability.text = languageText.main_Ability;
		this.language_Ability.fontSize = main_TitleSize;
		this.language_Talents.text = languageText.main_Talents;
		this.language_Talents.fontSize = main_TitleSize;
		this.language_DifficultyOptions.text = languageText.main_DifficultyOptions;
		this.language_DifficultyOptions.fontSize = main_TitleSize;
		this.language_TitleSkill.text = languageText.newGamePanel.title_Skill;
		this.language_TitleMode.text = languageText.newGamePanel.title_Mode;
		this.language_TitleDaily.text = languageText.dailyChallenge.daily;
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Start()
	{
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x00026A21 File Offset: 0x00024C21
	private void Update()
	{
		if (MyInput.IfGetCloseButtonDown())
		{
			MainCanvas.inst.Panel_NewGame_Close();
			UI_ToolTip.inst.Close();
		}
	}

	// Token: 0x040005A8 RID: 1448
	public static UI_Panel_Main_NewGame inst;

	// Token: 0x040005A9 RID: 1449
	[SerializeField]
	private RectTransform[] rectsToReforce;

	// Token: 0x040005AA RID: 1450
	[Header("面板")]
	[SerializeField]
	public UI_Panel_Main_Ability panelAbility;

	// Token: 0x040005AB RID: 1451
	[SerializeField]
	private UI_Panel_Main_Talents panelTalents;

	// Token: 0x040005AC RID: 1452
	[SerializeField]
	private UI_Panel_Main_VarColors panelColors;

	// Token: 0x040005AD RID: 1453
	[SerializeField]
	private UI_Panel_Main_Jobs panelJobs;

	// Token: 0x040005AE RID: 1454
	[SerializeField]
	private UI_Panel_Main_DifficultyOptions panelDifficultyOptions;

	// Token: 0x040005AF RID: 1455
	[SerializeField]
	private UI_Panel_Main_RunePanel panelRune;

	// Token: 0x040005B0 RID: 1456
	[SerializeField]
	private UI_Panel_Main_SkillModule panelSkillModule;

	// Token: 0x040005B1 RID: 1457
	[Header("单独图标")]
	[SerializeField]
	private UI_Icon_Star iconStar;

	// Token: 0x040005B2 RID: 1458
	[SerializeField]
	private UI_Icon_GeometryCoin iconGeometryCoin;

	// Token: 0x040005B3 RID: 1459
	[SerializeField]
	private UI_Icon_Skill iconSkill;

	// Token: 0x040005B4 RID: 1460
	[SerializeField]
	private UI_Icon_RuneButton iconRuneButton;

	// Token: 0x040005B5 RID: 1461
	[SerializeField]
	private UI_Icon_ModeIcon[] iconModes;

	// Token: 0x040005B6 RID: 1462
	[SerializeField]
	private UI_Icon_Proficiency iconProficiency;

	// Token: 0x040005B7 RID: 1463
	[Header("Language")]
	[SerializeField]
	private Text language_Job;

	// Token: 0x040005B8 RID: 1464
	[SerializeField]
	private Text language_Color;

	// Token: 0x040005B9 RID: 1465
	[SerializeField]
	private Text language_Ability;

	// Token: 0x040005BA RID: 1466
	[SerializeField]
	private Text language_Talents;

	// Token: 0x040005BB RID: 1467
	[SerializeField]
	private Text language_DifficultyOptions;

	// Token: 0x040005BC RID: 1468
	[SerializeField]
	private Text language_TitleSkill;

	// Token: 0x040005BD RID: 1469
	[SerializeField]
	private Text language_TitleMode;

	// Token: 0x040005BE RID: 1470
	[SerializeField]
	private Text language_TitleDaily;
}
