using System;
using UnityEngine;

// Token: 0x020001BD RID: 445
[RequireComponent(typeof(ParticleSystem))]
public class AnimatedParticleEmission : MonoBehaviour
{
	// Token: 0x060007AD RID: 1965 RVA: 0x0002FF0B File Offset: 0x0002E30B
	private void LateUpdate()
	{
		this.m_particleSystem.enableEmission = this.m_emit;
	}

	// Token: 0x04000614 RID: 1556
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private ParticleSystem m_particleSystem;

	// Token: 0x04000615 RID: 1557
	[SerializeField]
	private bool m_emit;
}
