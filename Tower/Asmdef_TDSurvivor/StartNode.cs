using System;
using UnityEngine;

// Token: 0x020000AB RID: 171
public class StartNode : ABaseNode
{
	// Token: 0x0600038C RID: 908 RVA: 0x0000E416 File Offset: 0x0000C616
	public override string GetName()
	{
		return "Start";
	}

	// Token: 0x0600038D RID: 909 RVA: 0x0000E41D File Offset: 0x0000C61D
	public override void OnElementSelected()
	{
		Debug.Log("Start node selected");
	}
}
