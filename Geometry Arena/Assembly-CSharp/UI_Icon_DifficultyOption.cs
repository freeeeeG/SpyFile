using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200008A RID: 138
public class UI_Icon_DifficultyOption : UI_Icon
{
	// Token: 0x170000DA RID: 218
	// (get) Token: 0x060004DE RID: 1246 RVA: 0x0001D0A0 File Offset: 0x0001B2A0
	// (set) Token: 0x060004DF RID: 1247 RVA: 0x0001D0B3 File Offset: 0x0001B2B3
	public bool OpenFlag
	{
		get
		{
			return TempData.inst.diffiOptFlag[this.doId];
		}
		set
		{
			TempData.inst.diffiOptFlag[this.doId] = value;
		}
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x0001D0C8 File Offset: 0x0001B2C8
	public void Init(int ID)
	{
		this.doId = ID;
		Sprite spriteWithId = ResourceLibrary.Inst.SpList_Icon_DifficultyOptions.GetSpriteWithId(ID);
		Sprite spriteWithId2 = ResourceLibrary.Inst.SpList_Icon_DifficultyOptionsOnBottomColor.GetSpriteWithId(ID);
		Sprite spriteWithId3 = ResourceLibrary.Inst.SpList_Icon_DifficultyOptionsOnTopWhite.GetSpriteWithId(ID);
		Color colorWithHue = ResourceLibrary.Inst.colorSet_UI.GetColorWithHue((float)ID / (float)DataBase.Inst.Data_DifficultyOptions.Length);
		this.imageOff.sprite = spriteWithId;
		this.imageOff.color = colorWithHue;
		this.imageOnBottomColor.sprite = spriteWithId2;
		this.imageOnBottomColor.color = colorWithHue;
		this.imageOnTopWhite.sprite = spriteWithId3;
		this.imageOnTopWhite.color = Color.white;
		this.UpdateOutline();
		this.UpdateImage();
		base.UpdateLockIcon();
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x0001D190 File Offset: 0x0001B390
	private void UpdateImage()
	{
		if (this.OpenFlag)
		{
			this.imageOnBottomColor.gameObject.SetActive(true);
			this.imageOnTopWhite.gameObject.SetActive(true);
			this.imageOff.gameObject.SetActive(false);
			return;
		}
		this.imageOnBottomColor.gameObject.SetActive(false);
		this.imageOnTopWhite.gameObject.SetActive(false);
		this.imageOff.gameObject.SetActive(true);
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x0001D20C File Offset: 0x0001B40C
	private void UpdateOutline()
	{
		if (TempData.inst.currentSceneType != EnumSceneType.MAINMENU)
		{
			this.OutlineNew_Close();
			return;
		}
		if (this.OpenFlag)
		{
			this.OutlineNew_Show_Selected();
			return;
		}
		this.OutlineNew_Close();
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x0001D238 File Offset: 0x0001B438
	public override void OnPointerClick(PointerEventData eventData)
	{
		if (!this.ifUnlocked())
		{
			return;
		}
		if (TempData.inst.currentSceneType != EnumSceneType.MAINMENU)
		{
			return;
		}
		if (TempData.inst.daily_Open)
		{
			UI_FloatTextControl.inst.NewFloatText(LanguageText.Inst.floatText.dailyChallenge_LockDO);
			return;
		}
		this.OpenFlag = !this.OpenFlag;
		UI_FloatTextControl.inst.Special_DiffiOpiton(this.doId, this.OpenFlag);
		TempData.inst.battle = new Battle();
		TempData.inst.battle.UpdateBattleFacs();
		UI_ToolTip.inst.TryClose();
		MainCanvas.inst.Panel_NewGame_Update();
		MainCanvas.inst.Obj_Preview_UpdateColor();
		this.UpdateOutline();
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x0001D2E8 File Offset: 0x0001B4E8
	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (TempData.inst.currentSceneType != EnumSceneType.MAINMENU)
		{
			return;
		}
		UI_ToolTip.inst.ShowWithString(UI_ToolTipInfo.GetInfo_DifficultyOptions(this.doId, this.ifUnlocked()));
		if (!this.OpenFlag)
		{
			this.OutlineNew_Show_Above();
		}
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x0001D320 File Offset: 0x0001B520
	public override void OnPointerExit(PointerEventData eventData)
	{
		if (TempData.inst.currentSceneType != EnumSceneType.MAINMENU)
		{
			return;
		}
		UI_ToolTip.inst.Close();
		if (!this.OpenFlag)
		{
			this.OutlineNew_Close();
		}
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x0001D347 File Offset: 0x0001B547
	protected override bool ifUnlocked()
	{
		return !GameParameters.Inst.ifDemo || this.doId < 14;
	}

	// Token: 0x04000414 RID: 1044
	public int doId;

	// Token: 0x04000415 RID: 1045
	public Image imageOff;

	// Token: 0x04000416 RID: 1046
	public Image imageOnBottomColor;

	// Token: 0x04000417 RID: 1047
	public Image imageOnTopWhite;
}
