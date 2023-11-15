using System;
using UnityEngine;

// Token: 0x0200045C RID: 1116
[RequireComponent(typeof(TriggerRecorder))]
public class CookingRegion : MonoBehaviour
{
	// Token: 0x060014B2 RID: 5298 RVA: 0x00070C3D File Offset: 0x0006F03D
	private void OnEnable()
	{
	}

	// Token: 0x04000FE3 RID: 4067
	[SerializeField]
	[AssignChildRecursive("glow", Editorbility.Editable)]
	public ParticleSystem m_glowEffect;

	// Token: 0x04000FE4 RID: 4068
	[SerializeField]
	public float m_flameOffRadius = 2f;

	// Token: 0x04000FE5 RID: 4069
	[SerializeField]
	public AnimationCurve m_heightCurve;

	// Token: 0x04000FE6 RID: 4070
	[SerializeField]
	public float m_fadeDuration = 1f;

	// Token: 0x04000FE7 RID: 4071
	[SerializeField]
	public Renderer m_burnerRenderer;

	// Token: 0x04000FE8 RID: 4072
	[SerializeField]
	[AssignComponentRecursive(Editorbility.Editable)]
	public ParticleSystem[] m_flameEffects;

	// Token: 0x04000FE9 RID: 4073
	public Collider m_TriggerArea;

	// Token: 0x04000FEA RID: 4074
	public CookingStationType m_StationType;
}
