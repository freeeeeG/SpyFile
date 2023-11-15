using System;
using UnityEngine;

// Token: 0x0200079D RID: 1949
public class RendererInfo : MonoBehaviour
{
	// Token: 0x060025A2 RID: 9634 RVA: 0x000B1DE9 File Offset: 0x000B01E9
	protected virtual void Start()
	{
		this.UpdateLighting();
	}

	// Token: 0x060025A3 RID: 9635 RVA: 0x000B1DF4 File Offset: 0x000B01F4
	protected void UpdateLighting()
	{
		MeshRenderer component = base.GetComponent<MeshRenderer>();
		if (component != null)
		{
			component.lightmapIndex = this.lightmapIndex;
			component.lightmapScaleOffset = this.lightmapScaleOffset;
		}
	}

	// Token: 0x04001D2C RID: 7468
	public int lightmapIndex = -1;

	// Token: 0x04001D2D RID: 7469
	public Vector4 lightmapScaleOffset = default(Vector4);
}
