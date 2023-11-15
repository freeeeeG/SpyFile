using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000093 RID: 147
public class UI_Icon_RuneButton : UI_Icon
{
	// Token: 0x06000524 RID: 1316 RVA: 0x0001E1E3 File Offset: 0x0001C3E3
	private bool IfBattle()
	{
		return TempData.inst.currentSceneType == EnumSceneType.BATTLE;
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x0001E1F2 File Offset: 0x0001C3F2
	public override void OnPointerClick(PointerEventData eventData)
	{
		if (this.IfBattle())
		{
			return;
		}
		if (!this.ifUnlocked())
		{
			return;
		}
		this.panelRune.Open();
		UI_ToolTip.inst.Close();
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x0001E21C File Offset: 0x0001C41C
	public override void OnPointerEnter(PointerEventData eventData)
	{
		LanguageText inst = LanguageText.Inst;
		LanguageText.RuneInfo runeInfo = inst.runeInfo;
		UI_Setting inst2 = UI_Setting.Inst;
		UI_Setting.InfinityMode infinityMode = inst2.infinityMode;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(runeInfo.rune.TextSet(infinityMode.setTitle)).AppendLine();
		if (this.ifUnlocked())
		{
			if (!this.IfBattle())
			{
				stringBuilder.Append(runeInfo.outButton_ClickTip.TextSet(infinityMode.open));
				stringBuilder.AppendLine().AppendLine();
			}
		}
		else
		{
			if (GameParameters.Inst.ifDemo)
			{
				stringBuilder.Append(inst.demo.lockTip.TextSet(infinityMode.lockedOrOff));
			}
			else
			{
				stringBuilder.Append(runeInfo.outButton_Locked.TextSet(infinityMode.lockedOrOff)).AppendLine();
				stringBuilder.Append(runeInfo.outButton_LockTip.TextSet(infinityMode.lockedOrOff));
			}
			stringBuilder.AppendLine().AppendLine();
		}
		stringBuilder.Append(runeInfo.outButton_Info);
		if (this.ifUnlocked())
		{
			stringBuilder.AppendLine().AppendLine();
			Rune currentRune = GameData.inst.CurrentRune;
			if (currentRune == null)
			{
				stringBuilder.Append(runeInfo.outButton_CurrentRuneEmpty.TextSet(infinityMode.info));
			}
			else
			{
				stringBuilder.Append(runeInfo.outButton_CurrentRune.Colored(inst2.skill.Color_SmallTitle).Sized((float)inst2.skill.size_SmallTile)).AppendLine().Append(currentRune.GetInfo_Total().TextSet(infinityMode.info));
			}
		}
		UI_ToolTip.inst.ShowWithString(stringBuilder.ToString());
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x0001E3C5 File Offset: 0x0001C5C5
	public override void OnPointerExit(PointerEventData eventData)
	{
		UI_ToolTip.inst.Close();
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x0001E3D4 File Offset: 0x0001C5D4
	public void UpdateIcon()
	{
		LanguageText.RuneInfo runeInfo = LanguageText.Inst.runeInfo;
		UI_Setting inst = UI_Setting.Inst;
		SpriteList spList_Icon_Rune = ResourceLibrary.Inst.SpList_Icon_Rune;
		this.lang_Rune.text = runeInfo.rune;
		Rune currentRune = GameData.inst.CurrentRune;
		if (currentRune == null || currentRune.typeID < 0 || currentRune.rank == EnumRank.UNINTED)
		{
			this.imageOutline.color = Color.white;
			this.iconRune.sprite = spList_Icon_Rune.GetSprite_Default();
			this.iconRune.color = Color.white;
		}
		else
		{
			this.imageOutline.color = inst.rankColors[(int)currentRune.rank];
			this.iconRune.sprite = spList_Icon_Rune.GetSpriteWithId(currentRune.typeID);
			VarColor varColor = DataBase.Inst.Data_VarColors[currentRune.varColorID];
			this.iconRune.color = varColor.ColorRGB.ApplyColorSet(ResourceLibrary.Inst.colorSet_UI);
			if (currentRune.typeID == 12 || currentRune.typeID == 13)
			{
				this.iconRune.color = Color.white;
			}
		}
		base.UpdateLockIcon();
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x0001E4F5 File Offset: 0x0001C6F5
	protected override bool ifUnlocked()
	{
		return !GameParameters.Inst.ifDemo && GameData.inst.ifFinished;
	}

	// Token: 0x0400043F RID: 1087
	public Image iconRune;

	// Token: 0x04000440 RID: 1088
	public Text lang_Rune;

	// Token: 0x04000441 RID: 1089
	[SerializeField]
	private UI_Panel_Main_RunePanel panelRune;
}
