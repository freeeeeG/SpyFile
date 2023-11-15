using System;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class FogOfWarCtrl : MonoBehaviour
{
	// Token: 0x0600029E RID: 670 RVA: 0x0000B198 File Offset: 0x00009398
	private void Reset()
	{
		if (this.renderer_Area == null)
		{
			this.renderer_Area = base.GetComponent<Renderer>();
		}
	}

	// Token: 0x0600029F RID: 671 RVA: 0x0000B1B4 File Offset: 0x000093B4
	public bool IsValid()
	{
		return this.renderer_Area != null && this.radius > 0f;
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x0000B1D3 File Offset: 0x000093D3
	public void SetRadius(float radius)
	{
		this.radius = radius;
		base.transform.localScale = Vector3.one * radius;
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x0000B1F2 File Offset: 0x000093F2
	public float GetRadius()
	{
		return this.radius;
	}

	// Token: 0x04000321 RID: 801
	[SerializeField]
	private Renderer renderer_Area;

	// Token: 0x04000322 RID: 802
	[SerializeField]
	private float radius;
}
