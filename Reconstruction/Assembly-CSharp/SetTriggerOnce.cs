using System;
using UnityEngine;

// Token: 0x0200025C RID: 604
public class SetTriggerOnce : GuideEvent
{
	// Token: 0x06000F1F RID: 3871 RVA: 0x00027E85 File Offset: 0x00026085
	public override void Trigger()
	{
		PlayerPrefs.SetInt(this.triggerKey, this.value);
	}

	// Token: 0x04000786 RID: 1926
	public string triggerKey;

	// Token: 0x04000787 RID: 1927
	public int value;
}
