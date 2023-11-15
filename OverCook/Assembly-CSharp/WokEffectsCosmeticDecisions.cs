using System;
using UnityEngine;

// Token: 0x02000417 RID: 1047
public class WokEffectsCosmeticDecisions : MonoBehaviour
{
	// Token: 0x04000EE6 RID: 3814
	[SerializeField]
	[AssignChildRecursive("wok_flame", Editorbility.Editable)]
	public Renderer m_flameRenderer;

	// Token: 0x04000EE7 RID: 3815
	[SerializeField]
	[AssignComponentRecursive(Editorbility.Editable)]
	public ParticleSystem[] m_particleSystems;

	// Token: 0x04000EE8 RID: 3816
	[SerializeField]
	public AnimationCurve m_emissionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	// Token: 0x04000EE9 RID: 3817
	[SerializeField]
	public float m_transitionDuration = 1f;
}
