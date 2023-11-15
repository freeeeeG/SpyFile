using System;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class LevelNode : ABaseNode
{
	// Token: 0x06000383 RID: 899 RVA: 0x0000E3A1 File Offset: 0x0000C5A1
	public override string GetName()
	{
		return this.levelName;
	}

	// Token: 0x06000384 RID: 900 RVA: 0x0000E3A9 File Offset: 0x0000C5A9
	public override void OnElementSelected()
	{
		Debug.Log("Level selected: " + this.levelName);
	}

	// Token: 0x040003B4 RID: 948
	public string levelName;
}
