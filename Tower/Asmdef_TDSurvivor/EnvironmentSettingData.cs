using System;
using UnityEngine;

// Token: 0x0200007B RID: 123
[Serializable]
public class EnvironmentSettingData
{
	// Token: 0x0400030D RID: 781
	[Header("燈光顏色")]
	public Gradient gradient_LightColor;

	// Token: 0x0400030E RID: 782
	[Header("燈光強度")]
	public AnimationCurve curve_LightIntensity;

	// Token: 0x0400030F RID: 783
	[Header("影子強度")]
	public AnimationCurve curve_ShadowStrength;

	// Token: 0x04000310 RID: 784
	[Header("燈光方向_開始")]
	public Vector3 lightOrientation_Start;

	// Token: 0x04000311 RID: 785
	[Header("燈光方向_結束")]
	public Vector3 lightOrientation_End;
}
