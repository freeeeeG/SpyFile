using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000005 RID: 5
[Serializable]
public class LightControlBehaviour : PlayableBehaviour
{
	// Token: 0x04000016 RID: 22
	public Color color = Color.white;

	// Token: 0x04000017 RID: 23
	public float intensity = 1f;

	// Token: 0x04000018 RID: 24
	public float bounceIntensity = 1f;

	// Token: 0x04000019 RID: 25
	public float range = 10f;
}
