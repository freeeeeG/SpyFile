using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E1 RID: 225
public class UI_FloatTextControl : MonoBehaviour
{
	// Token: 0x060007C8 RID: 1992 RVA: 0x0002B1DC File Offset: 0x000293DC
	private void Awake()
	{
		UI_FloatTextControl.inst = this;
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x0002B1E4 File Offset: 0x000293E4
	public void NewFloatText(string s)
	{
		Object.Instantiate<GameObject>(this.prefab_SingleFloatText, this.rectContainer).GetComponent<UI_SingleFloatText>().text.text = s;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectContainer);
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x0002B214 File Offset: 0x00029414
	public void Special_TalentLevelUp(int jobNo, int talentNo)
	{
		PlayerModel playerModel = DataBase.Inst.DataPlayerModels[jobNo];
		string language_JobName = DataBase.Inst.DataPlayerModels[jobNo].Language_JobName;
		string name = playerModel.talents[talentNo].Name;
		string talentLevelUp = LanguageText.Inst.floatText.talentLevelUp;
		this.NewFloatText(language_JobName + name + " " + talentLevelUp);
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x0002B270 File Offset: 0x00029470
	public void Special_TalentLevelMax(int jobNo, int talentNo)
	{
		PlayerModel playerModel = DataBase.Inst.DataPlayerModels[jobNo];
		string language_JobName = DataBase.Inst.DataPlayerModels[jobNo].Language_JobName;
		string name = playerModel.talents[talentNo].Name;
		string talent_LevelMax = LanguageText.Inst.floatText.talent_LevelMax;
		this.NewFloatText(language_JobName + name + " " + talent_LevelMax);
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x0002B2CC File Offset: 0x000294CC
	public void Special_TalentLackOfStar()
	{
		string talent_LackOfStar = LanguageText.Inst.floatText.talent_LackOfStar;
		this.NewFloatText(talent_LackOfStar);
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x0002B2F0 File Offset: 0x000294F0
	public void Special_BattleItemActive(BattleBuff buff)
	{
		string s = LanguageText.Inst.floatText.battleItemActive + " " + buff.Lang_Name;
		this.NewFloatText(s);
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x0002B324 File Offset: 0x00029524
	public void Special_DiffiOpiton(int DoId, bool ifOpen)
	{
		LanguageText.FloatText floatText = LanguageText.Inst.floatText;
		string str = ifOpen ? floatText.diffiOptionOpen : floatText.diffiOptionClose;
		string language_Name = DataBase.Inst.Data_DifficultyOptions[DoId].Language_Name;
		this.NewFloatText(language_Name + " " + str);
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x0002B374 File Offset: 0x00029574
	public void Special_SkillModule(SkillModule skillModule, bool ifOpen)
	{
		LanguageText.FloatText floatText = LanguageText.Inst.floatText;
		string str = ifOpen ? floatText.skillModuleOpen : floatText.skillModuleClose;
		string language_Name = skillModule.Language_Name;
		this.NewFloatText(language_Name + " " + str);
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x0002B3B8 File Offset: 0x000295B8
	public void Special_GainUpgrade(int upID)
	{
		string language_Name = DataBase.Inst.Data_Upgrades[upID].Language_Name;
		string upgrade_Gain = LanguageText.Inst.floatText.upgrade_Gain;
		this.NewFloatText(upgrade_Gain + " " + language_Name);
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x0002B3FC File Offset: 0x000295FC
	public void Special_DeleteUpgrade(int upID)
	{
		string language_Name = DataBase.Inst.Data_Upgrades[upID].Language_Name;
		string upgrade_Delete = LanguageText.Inst.floatText.upgrade_Delete;
		this.NewFloatText(upgrade_Delete + " " + language_Name);
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x0002B43D File Offset: 0x0002963D
	public void Special_AnyString(string s)
	{
		this.NewFloatText(s);
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x0002B448 File Offset: 0x00029648
	public void Special_SoldOut()
	{
		string upgrade_SoldOut = LanguageText.Inst.floatText.upgrade_SoldOut;
		this.NewFloatText(upgrade_SoldOut);
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x0002B46C File Offset: 0x0002966C
	public void Special_HpRecover()
	{
		string lifeRecover = LanguageText.Inst.floatText.lifeRecover;
		this.NewFloatText(lifeRecover);
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x0002B490 File Offset: 0x00029690
	public void Special_LackOfFragment()
	{
		string upgrade_LackOfFragment = LanguageText.Inst.floatText.upgrade_LackOfFragment;
		this.NewFloatText(upgrade_LackOfFragment);
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x0002B4B4 File Offset: 0x000296B4
	public void Special_GoStage()
	{
		bool timeStage = BattleManager.inst.timeStage != EnumTimeStage.REST;
		LanguageText.FloatText floatText = LanguageText.Inst.floatText;
		string s;
		if (!timeStage)
		{
			s = floatText.tip_EnterRestStage;
		}
		else
		{
			s = floatText.tip_EnterWaveStage;
		}
		this.NewFloatText(s);
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x0002B4F0 File Offset: 0x000296F0
	public void Special_Wander_LevelUp()
	{
		string wander_LevelUp = LanguageText.Inst.floatText.wander_LevelUp;
		this.NewFloatText(wander_LevelUp);
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x0002B514 File Offset: 0x00029714
	public void Special_ShopRefresh()
	{
		string shop_Refresh = LanguageText.Inst.floatText.shop_Refresh;
		this.NewFloatText(shop_Refresh);
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x0002B538 File Offset: 0x00029738
	public void Special_DiffiLevelUp()
	{
		string diffiLevelUp = LanguageText.Inst.floatText.diffiLevelUp;
		this.NewFloatText(diffiLevelUp);
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x0002B55C File Offset: 0x0002975C
	public void Special_DiffiLevelDown()
	{
		string diffiLevelDown = LanguageText.Inst.floatText.diffiLevelDown;
		this.NewFloatText(diffiLevelDown);
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x0002B580 File Offset: 0x00029780
	public void Special_DiffiLevelMax()
	{
		string diffiLevelMax = LanguageText.Inst.floatText.diffiLevelMax;
		this.NewFloatText(diffiLevelMax);
	}

	// Token: 0x04000694 RID: 1684
	public static UI_FloatTextControl inst;

	// Token: 0x04000695 RID: 1685
	[SerializeField]
	private GameObject prefab_SingleFloatText;

	// Token: 0x04000696 RID: 1686
	[SerializeField]
	private RectTransform rectContainer;
}
