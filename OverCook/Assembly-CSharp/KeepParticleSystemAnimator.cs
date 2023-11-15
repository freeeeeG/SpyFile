using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003CC RID: 972
public class KeepParticleSystemAnimator : MonoBehaviour
{
	// Token: 0x04000E09 RID: 3593
	private const float c_updateInterval = 0.25f;

	// Token: 0x04000E0A RID: 3594
	private const float c_updateIntervalRandomness = 0.05f;

	// Token: 0x04000E0B RID: 3595
	private KeepParticleAnimatorData[] m_datas = new KeepParticleAnimatorData[0];

	// Token: 0x04000E0C RID: 3596
	private ParticleSystem[] m_particleSystems = new ParticleSystem[0];

	// Token: 0x04000E0D RID: 3597
	private float m_updateInterval = 0.25f;

	// Token: 0x04000E0E RID: 3598
	private IEnumerator m_updateRoutine;
}
