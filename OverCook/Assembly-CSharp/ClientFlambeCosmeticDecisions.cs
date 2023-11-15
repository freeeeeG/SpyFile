using System;
using UnityEngine;

// Token: 0x020003B5 RID: 949
public class ClientFlambeCosmeticDecisions : ClientFryingContentsCosmeticDecisions
{
	// Token: 0x060011B4 RID: 4532 RVA: 0x00065301 File Offset: 0x00063701
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_flambeCosmeticDecisions = (FlambeCosmeticDecisions)synchronisedObject;
		base.StartSynchronising(synchronisedObject);
	}

	// Token: 0x060011B5 RID: 4533 RVA: 0x00065316 File Offset: 0x00063716
	public override void OnCookingPropChanged(float newProp)
	{
		this.m_targetProp = 1f;
		base.OnCookingPropChanged(newProp);
	}

	// Token: 0x060011B6 RID: 4534 RVA: 0x0006532C File Offset: 0x0006372C
	protected void Update()
	{
		float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
		MathUtils.AdvanceToTarget_Sinusoidal(ref this.m_highlightProp, ref this.m_highlightPropGrad, this.m_targetProp, this.m_flambeCosmeticDecisions.m_gradLimit, this.m_flambeCosmeticDecisions.m_timeToMax, deltaTime);
		base.AccessRenderer.material.SetFloat(this.m_flambeCosmeticDecisions.m_materialFloatName, Mathf.Clamp01(this.m_highlightProp));
		this.m_targetProp = 0f;
	}

	// Token: 0x04000DCA RID: 3530
	private FlambeCosmeticDecisions m_flambeCosmeticDecisions;

	// Token: 0x04000DCB RID: 3531
	private float m_highlightProp;

	// Token: 0x04000DCC RID: 3532
	private float m_highlightPropGrad;

	// Token: 0x04000DCD RID: 3533
	private float m_targetProp;
}
