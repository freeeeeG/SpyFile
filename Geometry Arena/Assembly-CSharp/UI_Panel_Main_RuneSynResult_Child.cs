using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D1 RID: 209
public class UI_Panel_Main_RuneSynResult_Child : UI_Panel_Main_IconList
{
	// Token: 0x06000748 RID: 1864 RVA: 0x00029024 File Offset: 0x00027224
	public void InitWithRune(Rune theRune, float maxWidth = 1000f, UI_Icon_Rune.EnumIconRuneType iconType = UI_Icon_Rune.EnumIconRuneType.SYNRESULT, bool ifAutoFuse = false, bool ifAutoFuse_CanSelect = false)
	{
		this.ifAutoFuse = ifAutoFuse;
		this.ifAutoFuse_CanSelect = ifAutoFuse_CanSelect;
		this.maxWidth = maxWidth;
		LanguageText.RuneInfo runeInfo = LanguageText.Inst.runeInfo;
		this.theRune = theRune;
		this.iconRune.UpdateIcon_WithRune(theRune, iconType);
		this.iconRune.UpdateStateTip();
		this.iconRune.UpdateOutline();
		this.textRuneInfo.text = theRune.GetInfo_ExceptProp();
		this.textTitle.text = runeInfo.synResult_SmallTitles[this.index];
		this.InitIcons(null);
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x000290AD File Offset: 0x000272AD
	public void ChooseThis()
	{
		UI_Panel_Main_RuneSynResult.inst.Choose(this.index);
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool IfAvailable(int ID)
	{
		return true;
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x000290C0 File Offset: 0x000272C0
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		Rune_Property[] props_Sort = this.theRune.GetProps_Sort();
		obj.GetComponent<UI_Icon_Rune_SynResult_SingleProp>().InitIconWithProp(this.theRune, props_Sort[ID], this.maxWidth, this.ifAutoFuse, this.ifAutoFuse_CanSelect);
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x000290FF File Offset: 0x000272FF
	protected override int IconNum()
	{
		return this.theRune.props.Length;
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x00029110 File Offset: 0x00027310
	public float GetTextMaxWidth()
	{
		float num = 0f;
		foreach (GameObject gameObject in this.listIcons)
		{
			RectTransform component = gameObject.GetComponent<RectTransform>();
			LayoutRebuilder.ForceRebuildLayoutImmediate(component);
			float x = component.sizeDelta.x;
			if (x > num)
			{
				num = x;
			}
		}
		return num;
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x00029180 File Offset: 0x00027380
	public int GetRowCount()
	{
		return this.listIcons.Count;
	}

	// Token: 0x0400060B RID: 1547
	public Rune theRune;

	// Token: 0x0400060C RID: 1548
	[SerializeField]
	public int index;

	// Token: 0x0400060D RID: 1549
	[SerializeField]
	private UI_Icon_Rune iconRune;

	// Token: 0x0400060E RID: 1550
	[SerializeField]
	private Text textRuneInfo;

	// Token: 0x0400060F RID: 1551
	[SerializeField]
	private Text textTitle;

	// Token: 0x04000610 RID: 1552
	[SerializeField]
	private float maxWidth = 1000f;

	// Token: 0x04000611 RID: 1553
	[SerializeField]
	private bool ifAutoFuse;

	// Token: 0x04000612 RID: 1554
	[SerializeField]
	private bool ifAutoFuse_CanSelect;
}
