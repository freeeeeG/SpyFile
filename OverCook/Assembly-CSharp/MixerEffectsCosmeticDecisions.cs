using System;
using UnityEngine;

// Token: 0x020003D8 RID: 984
public class MixerEffectsCosmeticDecisions : MonoBehaviour, IClientMixingNotifed
{
	// Token: 0x0600122D RID: 4653 RVA: 0x00066F90 File Offset: 0x00065390
	private void Awake()
	{
		this.SetActiveEffectOnOff(false);
		this.SetOvermixedEffectOnOff(false);
	}

	// Token: 0x0600122E RID: 4654 RVA: 0x00066FA0 File Offset: 0x000653A0
	private void OnEnable()
	{
		this.SetActiveEffectOnOff(false);
		this.SetOvermixedEffectOnOff(false);
	}

	// Token: 0x0600122F RID: 4655 RVA: 0x00066FB0 File Offset: 0x000653B0
	private void OnDisable()
	{
		this.SetActiveEffectOnOff(false);
		this.SetOvermixedEffectOnOff(false);
	}

	// Token: 0x06001230 RID: 4656 RVA: 0x00066FC0 File Offset: 0x000653C0
	public void OnMixingStarted()
	{
		this.SetActiveEffectOnOff(true);
	}

	// Token: 0x06001231 RID: 4657 RVA: 0x00066FC9 File Offset: 0x000653C9
	public void OnMixingFinished()
	{
		this.SetActiveEffectOnOff(false);
	}

	// Token: 0x06001232 RID: 4658 RVA: 0x00066FD2 File Offset: 0x000653D2
	public void OnMixingPropChanged(float newProp)
	{
		this.SetOvermixedEffectOnOff(newProp >= 2f);
	}

	// Token: 0x06001233 RID: 4659 RVA: 0x00066FE5 File Offset: 0x000653E5
	private void SetActiveEffectOnOff(bool bOn)
	{
		if (this.m_activeEffect != null)
		{
			if (bOn)
			{
				this.m_activeEffect.Play();
			}
			else
			{
				this.m_activeEffect.Stop();
			}
		}
	}

	// Token: 0x06001234 RID: 4660 RVA: 0x00067019 File Offset: 0x00065419
	private void SetOvermixedEffectOnOff(bool bOn)
	{
		if (this.m_overmixedEffect != null)
		{
			if (bOn)
			{
				this.m_overmixedEffect.Play();
			}
			else
			{
				this.m_overmixedEffect.Stop();
			}
		}
	}

	// Token: 0x04000E3D RID: 3645
	[SerializeField]
	private ParticleSystem m_activeEffect;

	// Token: 0x04000E3E RID: 3646
	[SerializeField]
	private ParticleSystem m_overmixedEffect;
}
