using System;
using UnityEngine;

// Token: 0x020003AA RID: 938
public class CookingEffectsCosmeticDecisions : MonoBehaviour
{
	// Token: 0x0600118F RID: 4495 RVA: 0x000648BC File Offset: 0x00062CBC
	private void Awake()
	{
		this.m_animator = base.GetComponentInChildren<Animator>();
		this.SetEmissionRate(0f);
		if (this.m_animator != null)
		{
			this.m_hasOnParam = this.m_animator.HasParameter(CookingEffectsCosmeticDecisions.c_onParam);
			this.m_hasCookingParam = this.m_animator.HasParameter(CookingEffectsCosmeticDecisions.c_cookingParam);
		}
	}

	// Token: 0x06001190 RID: 4496 RVA: 0x00064920 File Offset: 0x00062D20
	public void SetEmissionRate(float _rate)
	{
		this.m_steamEffect.emission.rateOverTime = _rate;
	}

	// Token: 0x06001191 RID: 4497 RVA: 0x00064948 File Offset: 0x00062D48
	public void SetColor(Color _color)
	{
		this.m_steamEffect.main.startColor = _color;
	}

	// Token: 0x04000DAB RID: 3499
	[SerializeField]
	public ParticleSystem m_steamEffect;

	// Token: 0x04000DAC RID: 3500
	public Animator m_animator;

	// Token: 0x04000DAD RID: 3501
	public static int c_onParam = Animator.StringToHash("On");

	// Token: 0x04000DAE RID: 3502
	public static int c_cookingParam = Animator.StringToHash("Cooking");

	// Token: 0x04000DAF RID: 3503
	[HideInInspector]
	public bool m_hasOnParam;

	// Token: 0x04000DB0 RID: 3504
	[HideInInspector]
	public bool m_hasCookingParam;
}
