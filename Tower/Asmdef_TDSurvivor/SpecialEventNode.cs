using System;
using UnityEngine;

// Token: 0x020000AA RID: 170
public class SpecialEventNode : ABaseNode
{
	// Token: 0x06000389 RID: 905 RVA: 0x0000E3EF File Offset: 0x0000C5EF
	public override string GetName()
	{
		return this.eventName;
	}

	// Token: 0x0600038A RID: 906 RVA: 0x0000E3F7 File Offset: 0x0000C5F7
	public override void OnElementSelected()
	{
		Debug.Log("Special event selected: " + this.eventName);
	}

	// Token: 0x040003B6 RID: 950
	public string eventName;
}
