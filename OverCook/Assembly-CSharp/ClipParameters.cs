using System;
using UnityEngine;

// Token: 0x020004F3 RID: 1267
[Serializable]
public class ClipParameters
{
	// Token: 0x0400114B RID: 4427
	[Range(0f, 256f)]
	public int Priority = 128;

	// Token: 0x0400114C RID: 4428
	[Range(0f, 1f)]
	public float Volume = 0.5f;

	// Token: 0x0400114D RID: 4429
	[Range(-3f, 3f)]
	public float Pitch = 1f;

	// Token: 0x0400114E RID: 4430
	[Range(0f, 1f)]
	public float RandomPitchVariance;

	// Token: 0x0400114F RID: 4431
	public float m_fadeInTime;

	// Token: 0x04001150 RID: 4432
	public float m_fadeOutTime;
}
