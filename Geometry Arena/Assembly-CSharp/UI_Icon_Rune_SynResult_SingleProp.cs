using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000095 RID: 149
public class UI_Icon_Rune_SynResult_SingleProp : UI_Icon
{
	// Token: 0x06000534 RID: 1332 RVA: 0x0001E94C File Offset: 0x0001CB4C
	public void InitIconWithProp(Rune rune, Rune_Property prop, float maxWidth, bool ifAutoFuse, bool ifAutoFuse_CanSelect)
	{
		this.ifAutoFuse = ifAutoFuse;
		this.ifAutoFuse_CanSelect = ifAutoFuse_CanSelect;
		this.prop = prop;
		this.textInfo.text = prop.GetInfo(rune);
		Color color = UI_Setting.Inst.rankColors[(int)prop.rank];
		this.textNormalColor = color;
		this.textHighlightColor = color;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.textInfo.rectTransform);
		if (this.textInfo.rectTransform.sizeDelta.x > maxWidth)
		{
			float d = maxWidth / this.textInfo.rectTransform.sizeDelta.x;
			this.textInfo.rectTransform.localScale = Vector2.one * d;
		}
		else
		{
			this.textInfo.rectTransform.localScale = Vector2.one;
		}
		this.UpdateOutline();
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x000051D0 File Offset: 0x000033D0
	protected override void Start()
	{
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x0001EA28 File Offset: 0x0001CC28
	public override void OnPointerClick(PointerEventData eventData)
	{
		if (!this.ifAutoFuse)
		{
			return;
		}
		if (!this.ifAutoFuse_CanSelect)
		{
			return;
		}
		UI_Panel_Main_RuneSynResult.inst.AutoFuse_AddProp(this.prop);
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x0001EA4C File Offset: 0x0001CC4C
	public override void OnPointerEnter(PointerEventData eventData)
	{
		this.ifAbove = true;
		base.TextSetHighlight();
		switch (this.prop.bigType)
		{
		case EnumRunePropertyType.FACTORPLAYER:
			UI_ToolTip.inst.ShowWithAbilityMain(this.prop.smallType);
			break;
		case EnumRunePropertyType.ORIGINUPGRADE:
			UI_ToolTip.inst.ShowWithString(UI_ToolTipInfo.GetInfo_UpgradeLibrary(this.prop.smallType));
			break;
		case EnumRunePropertyType.UPGRADETYPEBONUS:
		case EnumRunePropertyType.BLOCKUPGRADETYPE:
		{
			UpgradeType upgradeType = DataBase.Inst.Data_UpgradeTypes[this.prop.smallType];
			string text = upgradeType.Language_TypeName.TextSet(UI_Setting.Inst.commonSets.blueSmallTile) + "\n";
			text += upgradeType.Language_TypeInfo;
			UI_ToolTip.inst.ShowWithString(text);
			break;
		}
		}
		if (!this.ifAutoFuse)
		{
			return;
		}
		if (!this.ifAutoFuse_CanSelect)
		{
			return;
		}
		this.UpdateOutline();
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x0001EB2C File Offset: 0x0001CD2C
	public override void OnPointerExit(PointerEventData eventData)
	{
		this.ifAbove = false;
		base.TextSetNormal();
		UI_ToolTip.inst.Close();
		if (!this.ifAutoFuse)
		{
			return;
		}
		this.UpdateOutline();
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x0001EB54 File Offset: 0x0001CD54
	private bool AutoFuse_IfEqual()
	{
		using (List<Rune_Property>.Enumerator enumerator = UI_Panel_Main_RuneSynResult.inst.autoFuse_ListProps.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (Rune_Property.AutoFuse_IfPropEqual(enumerator.Current, this.prop))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x0001EBB8 File Offset: 0x0001CDB8
	private void UpdateOutline()
	{
		if (!this.ifAutoFuse)
		{
			this.imageOutline.gameObject.SetActive(false);
			return;
		}
		if (this.AutoFuse_IfEqual())
		{
			this.imageOutline.gameObject.SetActive(true);
			this.imageOutline.rectTransform.sizeDelta = new Vector2(this.textInfo.rectTransform.sizeDelta.x + 21f, 39.5f);
			this.imageOutline.color = this.colorSelected;
			return;
		}
		if (this.ifAbove)
		{
			this.imageOutline.gameObject.SetActive(true);
			this.imageOutline.rectTransform.sizeDelta = new Vector2(this.textInfo.rectTransform.sizeDelta.x + 21f, 39.5f);
			this.imageOutline.color = this.colorAbove;
			return;
		}
		this.imageOutline.gameObject.SetActive(false);
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x0400044B RID: 1099
	[SerializeField]
	private Rune_Property prop;

	// Token: 0x0400044C RID: 1100
	[SerializeField]
	private Text textInfo;

	// Token: 0x0400044D RID: 1101
	[SerializeField]
	private bool ifAutoFuse;

	// Token: 0x0400044E RID: 1102
	[SerializeField]
	private bool ifAutoFuse_CanSelect;

	// Token: 0x0400044F RID: 1103
	[SerializeField]
	private bool ifAbove;

	// Token: 0x04000450 RID: 1104
	[SerializeField]
	private Color colorAbove = Color.white;

	// Token: 0x04000451 RID: 1105
	[SerializeField]
	private Color colorSelected = Color.white;
}
