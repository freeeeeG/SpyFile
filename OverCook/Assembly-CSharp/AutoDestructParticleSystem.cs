using System;
using UnityEngine;

// Token: 0x02000126 RID: 294
public class AutoDestructParticleSystem : MonoBehaviour
{
	// Token: 0x06000565 RID: 1381 RVA: 0x0002A1D7 File Offset: 0x000285D7
	private void Awake()
	{
		this.m_allParticles = base.gameObject.RequestComponentsRecursive<ParticleSystem>();
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x0002A1EA File Offset: 0x000285EA
	private void LateUpdate()
	{
		if (this.m_allParticles.FindIndex_Predicate((ParticleSystem x) => x != null && x.IsAlive()) == -1)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040004A0 RID: 1184
	private ParticleSystem[] m_allParticles;
}
