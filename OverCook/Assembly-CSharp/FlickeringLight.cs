using System;
using UnityEngine;

// Token: 0x020003B6 RID: 950
[RequireComponent(typeof(Light))]
public class FlickeringLight : LightModifier
{
	// Token: 0x060011B8 RID: 4536 RVA: 0x00065491 File Offset: 0x00063891
	protected override void Awake()
	{
		base.Awake();
		this.m_initialIntensity = base.BaseLight.intensity;
		this.m_initialRange = base.BaseLight.range;
	}

	// Token: 0x060011B9 RID: 4537 RVA: 0x000654BC File Offset: 0x000638BC
	protected override void ModifyLight(Light _light)
	{
		this.m_timer += TimeManager.GetDeltaTime(base.gameObject);
		float num = (Mathf.Sin(3.1f * this.m_timer) + Mathf.Sin(7f * this.m_timer) + Mathf.Sin(12f * this.m_timer)) / 3f;
		this.m_timer %= 1636.1415f;
		_light.intensity = this.m_initialIntensity + num * this.m_intensityVariation;
		_light.range = this.m_initialRange + num * this.m_rangeVariation;
	}

	// Token: 0x04000DCE RID: 3534
	[SerializeField]
	private float m_intensityVariation = 2f;

	// Token: 0x04000DCF RID: 3535
	[SerializeField]
	private float m_rangeVariation = 1f;

	// Token: 0x04000DD0 RID: 3536
	private float m_timer;

	// Token: 0x04000DD1 RID: 3537
	private float m_initialIntensity;

	// Token: 0x04000DD2 RID: 3538
	private float m_initialRange;
}
