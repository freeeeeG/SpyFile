using System;
using UnityEngine;

// Token: 0x020003E1 RID: 993
[RequireComponent(typeof(Light))]
public class ParticleLightIntensity : LightModifier
{
	// Token: 0x06001253 RID: 4691 RVA: 0x00067450 File Offset: 0x00065850
	protected override void ModifyLight(Light _light)
	{
		float num = (float)this.m_particleSystem.particleCount / (float)this.m_particleSystem.maxParticles;
		_light.intensity = num * _light.intensity;
	}

	// Token: 0x04000E4E RID: 3662
	[SerializeField]
	private ParticleSystem m_particleSystem;
}
