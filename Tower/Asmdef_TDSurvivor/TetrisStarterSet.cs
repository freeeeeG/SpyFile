using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000038 RID: 56
[Serializable]
public class TetrisStarterSet
{
	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000115 RID: 277 RVA: 0x0000535C File Offset: 0x0000355C
	public bool CanShowBeforeTutorial
	{
		get
		{
			return this.canShowBeforeTutorial;
		}
	}

	// Token: 0x06000116 RID: 278 RVA: 0x00005364 File Offset: 0x00003564
	public TetrisStarterSet()
	{
		this.list_TetrisTypes = new List<eItemType>
		{
			eItemType._2005_PANEL_I,
			eItemType._2005_PANEL_I,
			eItemType._2005_PANEL_I,
			eItemType._2005_PANEL_I,
			eItemType._2005_PANEL_I,
			eItemType._2005_PANEL_I,
			eItemType._2005_PANEL_I
		};
	}

	// Token: 0x040000C1 RID: 193
	[SerializeField]
	private string note;

	// Token: 0x040000C2 RID: 194
	[SerializeField]
	private bool canShowBeforeTutorial = true;

	// Token: 0x040000C3 RID: 195
	public int weight = 1;

	// Token: 0x040000C4 RID: 196
	public List<eItemType> list_TetrisTypes;
}
