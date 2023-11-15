using System;
using UnityEngine;

// Token: 0x02000180 RID: 384
public class PathLine : ReusableObject
{
	// Token: 0x060009E1 RID: 2529 RVA: 0x0001AD33 File Offset: 0x00018F33
	private void Update()
	{
		if (this.showing)
		{
			this.lineSR.material.SetTextureOffset("_MainTex", new Vector2(-Time.time * this.pathSpeed, 0f));
		}
	}

	// Token: 0x060009E2 RID: 2530 RVA: 0x0001AD69 File Offset: 0x00018F69
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		this.showing = false;
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x0001AD78 File Offset: 0x00018F78
	public void ShowPath(Vector3[] points = null)
	{
		this.showing = true;
		this.lineSR.enabled = true;
		if (points != null)
		{
			this.lineSR.positionCount = points.Length;
			this.lineSR.SetPositions(points);
		}
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x0001ADAA File Offset: 0x00018FAA
	public void HidePath()
	{
		this.showing = false;
		this.lineSR.enabled = false;
	}

	// Token: 0x04000528 RID: 1320
	private bool showing;

	// Token: 0x04000529 RID: 1321
	[SerializeField]
	private LineRenderer lineSR;

	// Token: 0x0400052A RID: 1322
	[SerializeField]
	private float pathSpeed;
}
