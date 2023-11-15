using System;
using UnityEngine;

// Token: 0x0200003C RID: 60
[Serializable]
public class UpgradeType
{
	// Token: 0x170000BD RID: 189
	// (get) Token: 0x0600027B RID: 635 RVA: 0x0000EF9C File Offset: 0x0000D19C
	public string Language_TypeName
	{
		get
		{
			return this.typeNames[(int)Setting.Inst.language];
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x0600027C RID: 636 RVA: 0x0000EFAF File Offset: 0x0000D1AF
	public string Language_TypeInfo
	{
		get
		{
			return this.typeInfos[(int)Setting.Inst.language].GetColorfulInfoWithFacs(this.facs, false);
		}
	}

	// Token: 0x0400023F RID: 575
	public string dataName = "UNINITED";

	// Token: 0x04000240 RID: 576
	public int ID = -1;

	// Token: 0x04000241 RID: 577
	public Color typeColor = Color.white;

	// Token: 0x04000242 RID: 578
	public bool ifAvailableInRune;

	// Token: 0x04000243 RID: 579
	public string[] typeNames = new string[3];

	// Token: 0x04000244 RID: 580
	public string[] typeInfos = new string[3];

	// Token: 0x04000245 RID: 581
	public float[] facs;
}
