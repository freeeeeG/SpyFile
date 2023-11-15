using System;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public class ShakeCamera : MonoBehaviour
{
	// Token: 0x06000B31 RID: 2865 RVA: 0x0002B5D3 File Offset: 0x000297D3
	private void Start()
	{
		this.Shaking = false;
		this.OriginalPos = base.transform.localPosition;
		this.OriginalRot = base.transform.localRotation;
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x0002B600 File Offset: 0x00029800
	private void Update()
	{
		if (this.ShakeIntensity > 0f)
		{
			base.transform.localPosition = this.OriginalPos + Random.insideUnitSphere * this.ShakeIntensity;
			base.transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(-this.ShakeIntensity, this.ShakeIntensity)) * this.OriginalRot;
			this.ShakeIntensity -= this.ShakeDecay;
			return;
		}
		if (this.Shaking)
		{
			this.Shaking = false;
			base.transform.localPosition = this.OriginalPos;
			base.transform.localRotation = this.OriginalRot;
		}
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x0002B6BC File Offset: 0x000298BC
	public void DoShake(float intensity, float decay)
	{
		if (!this.Shaking)
		{
			this.OriginalPos = base.transform.localPosition;
			this.OriginalRot = base.transform.localRotation;
		}
		this.ShakeIntensity = intensity;
		this.ShakeDecay = decay;
		this.Shaking = true;
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x0002B708 File Offset: 0x00029908
	public void Stop()
	{
		this.Shaking = false;
		base.transform.localPosition = this.OriginalPos;
		base.transform.localRotation = this.OriginalRot;
	}

	// Token: 0x04000900 RID: 2304
	public bool Shaking;

	// Token: 0x04000901 RID: 2305
	private float ShakeDecay;

	// Token: 0x04000902 RID: 2306
	private float ShakeIntensity;

	// Token: 0x04000903 RID: 2307
	private Vector3 OriginalPos;

	// Token: 0x04000904 RID: 2308
	private Quaternion OriginalRot;
}
