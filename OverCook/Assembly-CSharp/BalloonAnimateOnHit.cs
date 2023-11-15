using System;
using UnityEngine;

// Token: 0x02000B9F RID: 2975
[RequireComponent(typeof(Collider))]
public class BalloonAnimateOnHit : MonoBehaviour
{
	// Token: 0x06003CFA RID: 15610 RVA: 0x00123672 File Offset: 0x00121A72
	protected virtual void Awake()
	{
		this.m_triggerHash = Animator.StringToHash(this.m_trigger);
		this.m_AudioManager = GameUtils.RequireManager<AudioManager>();
	}

	// Token: 0x06003CFB RID: 15611 RVA: 0x00123690 File Offset: 0x00121A90
	private void OnTriggerEnter(Collider other)
	{
		if (this.m_targetAnimator != null)
		{
			this.m_targetAnimator.SetTrigger(this.m_triggerHash);
		}
		if (this.m_popParticles != null)
		{
			this.m_popParticles.SetActive(true);
		}
	}

	// Token: 0x06003CFC RID: 15612 RVA: 0x001236DC File Offset: 0x00121ADC
	public void OnTrigger(string _trigger)
	{
		if (_trigger == "animationFinished")
		{
			if (this.m_popParticles != null)
			{
				this.m_popParticles.SetActive(false);
			}
			this.m_AudioManager.TriggerAudio(GameOneShotAudioTag.UIPop, base.gameObject.layer);
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0400310D RID: 12557
	private AudioManager m_AudioManager;

	// Token: 0x0400310E RID: 12558
	[SerializeField]
	public GameObject m_popParticles;

	// Token: 0x0400310F RID: 12559
	[SerializeField]
	public string m_trigger = string.Empty;

	// Token: 0x04003110 RID: 12560
	[SerializeField]
	public Animator m_targetAnimator;

	// Token: 0x04003111 RID: 12561
	private int m_triggerHash;
}
