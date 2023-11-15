using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C1 RID: 193
public class UI_Panel_Main_DifficultyOptions : UI_Panel_Main_IconList
{
	// Token: 0x060006AC RID: 1708 RVA: 0x00025C74 File Offset: 0x00023E74
	public override void InitIcons(Transform transformParent = null)
	{
		base.InitIcons(null);
		this.starMulti.text = MyTool.DecimalToMultiPercentString((double)Battle.inst.factorBattle_FromDifficultyOption.StarGain);
		this.textSelectAll.text = LanguageText.Inst.newGamePanel.selectAll;
		this.textClearAll.text = LanguageText.Inst.newGamePanel.clearAll;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectButtons);
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x00021E5A File Offset: 0x0002005A
	protected override int IconNum()
	{
		return DataBase.Inst.Data_DifficultyOptions.Length;
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool IfAvailable(int ID)
	{
		return true;
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x00025CE7 File Offset: 0x00023EE7
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		obj.GetComponent<UI_Icon_DifficultyOption>().Init(ID);
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x00025CF8 File Offset: 0x00023EF8
	public void Button_SelectAll()
	{
		if (TempData.inst.daily_Open)
		{
			UI_FloatTextControl.inst.NewFloatText(LanguageText.Inst.floatText.dailyChallenge_LockDO);
			return;
		}
		for (int i = 1; i < TempData.inst.diffiOptFlag.Length; i++)
		{
			TempData.inst.diffiOptFlag[i] = true;
		}
		MainCanvas.inst.Panel_NewGame_Update();
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x00025D5C File Offset: 0x00023F5C
	public void Button_ClearAll()
	{
		if (TempData.inst.daily_Open)
		{
			UI_FloatTextControl.inst.NewFloatText(LanguageText.Inst.floatText.dailyChallenge_LockDO);
			return;
		}
		for (int i = 0; i < TempData.inst.diffiOptFlag.Length; i++)
		{
			TempData.inst.diffiOptFlag[i] = false;
		}
		MainCanvas.inst.Panel_NewGame_Update();
	}

	// Token: 0x04000588 RID: 1416
	[SerializeField]
	private Text starMulti;

	// Token: 0x04000589 RID: 1417
	[SerializeField]
	private Text textSelectAll;

	// Token: 0x0400058A RID: 1418
	[SerializeField]
	private Text textClearAll;

	// Token: 0x0400058B RID: 1419
	[SerializeField]
	private RectTransform rectButtons;
}
