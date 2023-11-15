using System;
using UnityEngine;

// Token: 0x02000249 RID: 585
public class FirstTime : GuideCondition
{
	// Token: 0x06000EF9 RID: 3833 RVA: 0x00027B30 File Offset: 0x00025D30
	public override bool Judge()
	{
		return PlayerPrefs.GetInt(this.trigger, 0) != this.value;
	}

	// Token: 0x04000763 RID: 1891
	public string trigger;

	// Token: 0x04000764 RID: 1892
	public int value;
}
