using System;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public class BossNode : ABaseNode
{
	// Token: 0x06000380 RID: 896 RVA: 0x0000E386 File Offset: 0x0000C586
	public override string GetName()
	{
		return "Boss";
	}

	// Token: 0x06000381 RID: 897 RVA: 0x0000E38D File Offset: 0x0000C58D
	public override void OnElementSelected()
	{
		Debug.Log("Boss node selected");
	}
}
