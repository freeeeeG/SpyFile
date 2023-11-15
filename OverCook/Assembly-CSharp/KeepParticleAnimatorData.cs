using System;
using UnityEngine;

// Token: 0x020003CB RID: 971
[Serializable]
public class KeepParticleAnimatorData : MonoBehaviour
{
	// Token: 0x04000E05 RID: 3589
	[SerializeField]
	public float simulationSpeed = 1f;

	// Token: 0x04000E06 RID: 3590
	[SerializeField]
	public float startLifetime = 1f;

	// Token: 0x04000E07 RID: 3591
	[SerializeField]
	public int rateOverTime = 100;

	// Token: 0x04000E08 RID: 3592
	[SerializeField]
	public Color startColor = Color.white;
}
