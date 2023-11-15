using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000092 RID: 146
public class UI_Icon_Rune : UI_Icon
{
	// Token: 0x06000516 RID: 1302 RVA: 0x0001DCC6 File Offset: 0x0001BEC6
	private void Update()
	{
		if (this.flagSelected)
		{
			this.UpdateOutline();
		}
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x0001DCD6 File Offset: 0x0001BED6
	public void SetSelect(bool flag)
	{
		if (flag)
		{
			Debug.LogWarning("符文选中情况还没做，打算做：装备就是圆形、融合中就是菱形");
			this.OutlineNew_Show_Selected();
			return;
		}
		this.OutlineNew_Close();
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x0001DCF4 File Offset: 0x0001BEF4
	public void UpdateIcon_WithRune(Rune rune, UI_Icon_Rune.EnumIconRuneType iconType)
	{
		this.iconType = iconType;
		this.rune = rune;
		UI_Setting inst = UI_Setting.Inst;
		SpriteList spList_Icon_Rune = ResourceLibrary.Inst.SpList_Icon_Rune;
		if (rune == null || rune.typeID < 0 || rune.rank == EnumRank.UNINTED)
		{
			this.imageOutline.color = Color.white;
			this.iconRune.sprite = null;
			this.iconRune.color = Color.black;
			this.imageFavorite.gameObject.SetActive(false);
			return;
		}
		this.imageFavorite.gameObject.SetActive(rune.ifFavorite);
		this.imageOutline.color = inst.rankColors[(int)rune.rank];
		this.iconRune.sprite = spList_Icon_Rune.GetSpriteWithId(rune.typeID);
		VarColor varColor = DataBase.Inst.Data_VarColors[rune.varColorID];
		this.iconRune.color = varColor.ColorRGB.ApplyColorSet(ResourceLibrary.Inst.colorSet_UI);
		if (rune.typeID == 12 || rune.typeID == 13)
		{
			this.iconRune.color = Color.white;
		}
		UI_Panel_Main_RunePanel inst2 = UI_Panel_Main_RunePanel.inst;
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x0001DE18 File Offset: 0x0001C018
	public void UpdateOutline()
	{
		if (this.iconType == UI_Icon_Rune.EnumIconRuneType.RUNESTORE)
		{
			if (!UI_Panel_Rune_RuneDetail.inst.ifRuneGoodPreview(this.rune))
			{
				this.OutlineNew_Close();
			}
			return;
		}
		if (this.iconType != UI_Icon_Rune.EnumIconRuneType.LIST && this.iconType != UI_Icon_Rune.EnumIconRuneType.DETAIL)
		{
			this.OutlineNew_Close();
			return;
		}
		if (UI_Panel_Rune_RuneDetail.inst.ifRuneSelected(this.rune) || UI_Panel_Rune_RuneDetail.inst.ifRuneGoodPreview(this.rune))
		{
			this.OutlineNew_Show_Selected();
			return;
		}
		if (UI_Panel_Rune_RuneDetail.inst.ifTempAbove(this.rune))
		{
			this.OutlineNew_Show_Above();
			return;
		}
		this.OutlineNew_Close();
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x0001DEAC File Offset: 0x0001C0AC
	public void UpdateIcon(int index, UI_Icon_Rune.EnumIconRuneType iconType)
	{
		this.index = index;
		if (index < 0)
		{
			this.rune = null;
		}
		else
		{
			this.rune = GameData.inst.runes[index];
		}
		this.UpdateIcon_WithRune(this.rune, iconType);
		this.UpdateStateTip();
		this.UpdateOutline();
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x0001DEFC File Offset: 0x0001C0FC
	public void UpdateStateTip()
	{
		LanguageText.RuneInfo runeInfo = LanguageText.Inst.runeInfo;
		if (this.rune == null)
		{
			this.stateTip.gameObject.SetActive(false);
			return;
		}
		if (this.rune.ifNew)
		{
			this.stateTip.text = runeInfo.runeIcon_New;
			this.stateTip.gameObject.SetActive(true);
			this.stateTip.fontSize = 30;
			return;
		}
		this.stateTip.fontSize = 20;
		int[] runeFusion_MaterialIndexs = GameData.inst.runeFusion_MaterialIndexs;
		if (this.rune == GameData.inst.CurrentRune)
		{
			this.stateTip.text = runeInfo.runeIcon_InEquip.ReplaceLineBreak();
			this.stateTip.gameObject.SetActive(true);
			return;
		}
		int num = GameData.inst.runes.IndexOf(this.rune);
		for (int i = 0; i <= 1; i++)
		{
			if (runeFusion_MaterialIndexs[i] >= 0 && runeFusion_MaterialIndexs[i] == num)
			{
				this.stateTip.text = runeInfo.runeIcon_ToFuse.ReplaceLineBreak();
				this.stateTip.gameObject.SetActive(true);
				return;
			}
		}
		this.stateTip.gameObject.SetActive(false);
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x0001E024 File Offset: 0x0001C224
	public override void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerId != -1)
		{
			return;
		}
		if (this.rune == null)
		{
			Debug.LogWarning("Warning_空图标被点击!");
			return;
		}
		UI_Panel_Main_RunePanel inst = UI_Panel_Main_RunePanel.inst;
		switch (this.iconType)
		{
		case UI_Icon_Rune.EnumIconRuneType.LIST:
			UI_Panel_Rune_RuneDetail.inst.SetLockedPreview(this.rune);
			break;
		case UI_Icon_Rune.EnumIconRuneType.EQUIP:
			inst.RuneEquip_TakeOff();
			break;
		case UI_Icon_Rune.EnumIconRuneType.SYNTHESIZE:
			inst.RuneSyn_Remove(this.index);
			break;
		case UI_Icon_Rune.EnumIconRuneType.NEWVIEW:
		case UI_Icon_Rune.EnumIconRuneType.SYNRESULT:
		case UI_Icon_Rune.EnumIconRuneType.RUNESTORE:
		case UI_Icon_Rune.EnumIconRuneType.DETAIL:
			break;
		default:
			Debug.LogError("Error_RuneIconTypeError!");
			break;
		}
		this.UpdateOutline();
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x0001E0B8 File Offset: 0x0001C2B8
	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (this.rune == null)
		{
			return;
		}
		if (this.rune.ifNew)
		{
			this.rune.ifNew = false;
			this.UpdateStateTip();
		}
		switch (this.iconType)
		{
		case UI_Icon_Rune.EnumIconRuneType.LIST:
			UI_Panel_Rune_RuneDetail.inst.SetTempPreview(this.rune);
			break;
		case UI_Icon_Rune.EnumIconRuneType.EQUIP:
			UI_Panel_Rune_RuneDetail.inst.SetTempPreview(this.rune);
			break;
		case UI_Icon_Rune.EnumIconRuneType.SYNTHESIZE:
			UI_Panel_Rune_RuneDetail.inst.SetTempPreview(this.rune);
			break;
		case UI_Icon_Rune.EnumIconRuneType.NEWVIEW:
			UI_Panel_Rune_RuneDetail.inst.SetTempPreview(this.rune);
			break;
		case UI_Icon_Rune.EnumIconRuneType.SYNRESULT:
		case UI_Icon_Rune.EnumIconRuneType.DETAIL:
			break;
		case UI_Icon_Rune.EnumIconRuneType.RUNESTORE:
			UI_Panel_Rune_RuneDetail.inst.SetTempPreview(this.rune);
			break;
		default:
			Debug.LogError("Error_RuneIconTypeError!");
			break;
		}
		this.UpdateOutline();
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x0001E182 File Offset: 0x0001C382
	public override void OnPointerExit(PointerEventData eventData)
	{
		UI_Panel_Rune_RuneDetail.inst.SetTempPreview(null);
		this.UpdateOutline();
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x0001E195 File Offset: 0x0001C395
	protected override void OutlineNew_Close()
	{
		this.flagSelected = false;
		this.imageOutline.sprite = this.spriteSquare;
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x0001E1AF File Offset: 0x0001C3AF
	protected override void OutlineNew_Show_Above()
	{
		this.flagSelected = false;
		this.imageOutline.sprite = this.spriteSquareR;
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x0001E1C9 File Offset: 0x0001C3C9
	public new void OutlineNew_Show_Selected()
	{
		this.flagSelected = true;
		this.imageOutline.sprite = this.spriteCircle;
	}

	// Token: 0x04000435 RID: 1077
	public int index;

	// Token: 0x04000436 RID: 1078
	public Rune rune;

	// Token: 0x04000437 RID: 1079
	public UI_Icon_Rune.EnumIconRuneType iconType;

	// Token: 0x04000438 RID: 1080
	[SerializeField]
	private bool flagSelected;

	// Token: 0x04000439 RID: 1081
	[Header("UI_MainAndExtra")]
	public Image iconRune;

	// Token: 0x0400043A RID: 1082
	[SerializeField]
	private Image imageFavorite;

	// Token: 0x0400043B RID: 1083
	[SerializeField]
	private Text stateTip;

	// Token: 0x0400043C RID: 1084
	[Header("Sprites_Outline")]
	[SerializeField]
	private Sprite spriteSquare;

	// Token: 0x0400043D RID: 1085
	[SerializeField]
	private Sprite spriteCircle;

	// Token: 0x0400043E RID: 1086
	[SerializeField]
	private Sprite spriteSquareR;

	// Token: 0x02000153 RID: 339
	public enum EnumIconRuneType
	{
		// Token: 0x040009D1 RID: 2513
		LIST,
		// Token: 0x040009D2 RID: 2514
		EQUIP,
		// Token: 0x040009D3 RID: 2515
		SYNTHESIZE,
		// Token: 0x040009D4 RID: 2516
		NEWVIEW,
		// Token: 0x040009D5 RID: 2517
		SYNRESULT,
		// Token: 0x040009D6 RID: 2518
		RUNESTORE,
		// Token: 0x040009D7 RID: 2519
		DETAIL
	}
}
