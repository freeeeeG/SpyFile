using System;
using UnityEngine;

// Token: 0x0200003E RID: 62
[Serializable]
public class VarColor
{
	// Token: 0x170000BF RID: 191
	// (get) Token: 0x06000281 RID: 641 RVA: 0x0000F182 File Offset: 0x0000D382
	public string Language_Name
	{
		get
		{
			return this.names[(int)Setting.Inst.language];
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x06000282 RID: 642 RVA: 0x0000F198 File Offset: 0x0000D398
	public Color ColorRGB
	{
		get
		{
			ColorSet colorSet_Unit = ResourceLibrary.Inst.colorSet_Unit;
			return Color.HSVToRGB(this.hue / 360f, colorSet_Unit.saturation / 100f, colorSet_Unit.value / 100f);
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x06000283 RID: 643 RVA: 0x0000F1DC File Offset: 0x0000D3DC
	public Color ColorUnit
	{
		get
		{
			ColorSet colorSet_Unit = ResourceLibrary.Inst.colorSet_Unit;
			return Color.HSVToRGB(this.hue / 360f, colorSet_Unit.saturation / 100f, colorSet_Unit.value / 100f);
		}
	}

	// Token: 0x06000284 RID: 644 RVA: 0x0000F220 File Offset: 0x0000D420
	public VarColor(VarColor target)
	{
		if (target == null)
		{
			return;
		}
		this.no = target.no;
		this.names = target.names;
		this.hue = target.hue;
		this.saturation = target.saturation;
		this.value = target.value;
		this.rank = target.rank;
		this.useRange = target.useRange;
	}

	// Token: 0x0400024A RID: 586
	public string dataName = "Uninited";

	// Token: 0x0400024B RID: 587
	public string[] names = new string[3];

	// Token: 0x0400024C RID: 588
	public int no = -1;

	// Token: 0x0400024D RID: 589
	public float hue;

	// Token: 0x0400024E RID: 590
	public float saturation;

	// Token: 0x0400024F RID: 591
	public float value;

	// Token: 0x04000250 RID: 592
	public EnumRank rank = EnumRank.UNINTED;

	// Token: 0x04000251 RID: 593
	public VarColor.EnumUseRange useRange = VarColor.EnumUseRange.UNINITED;

	// Token: 0x04000252 RID: 594
	public FactorMultis factorMultis;

	// Token: 0x04000253 RID: 595
	public bool avaiPlayer;

	// Token: 0x04000254 RID: 596
	public bool avaiEnemy;

	// Token: 0x0200014A RID: 330
	public enum EnumUseRange
	{
		// Token: 0x040009A7 RID: 2471
		UNINITED = -1,
		// Token: 0x040009A8 RID: 2472
		COMMON,
		// Token: 0x040009A9 RID: 2473
		ONLYSHOOT
	}
}
