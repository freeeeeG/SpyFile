using System;
using UnityEngine;

// Token: 0x020000C3 RID: 195
[SelectionBase]
public class Obj_CorruptedGrid : APowerGrid
{
	// Token: 0x0600047E RID: 1150 RVA: 0x000121E4 File Offset: 0x000103E4
	public override string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("PowerGrid", "CORRUPTED", Array.Empty<object>());
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x000121FF File Offset: 0x000103FF
	public override string GetLocStatsString()
	{
		return LocalizationManager.Instance.GetString("PowerGrid", "CORRUPTED_EFFECT", Array.Empty<object>());
	}
}
