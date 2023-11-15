using System;
using UnityEngine;

// Token: 0x0200079B RID: 1947
[ExecuteInEditMode]
public class MergedRendererInfo : RendererInfo
{
	// Token: 0x0600259C RID: 9628 RVA: 0x000B1E34 File Offset: 0x000B0234
	protected override void Start()
	{
		base.UpdateLighting();
	}
}
