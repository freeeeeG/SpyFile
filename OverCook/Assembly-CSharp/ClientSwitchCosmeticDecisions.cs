using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003F8 RID: 1016
public class ClientSwitchCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x0600128C RID: 4748 RVA: 0x0006856B File Offset: 0x0006696B
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_decisions = (SwitchCosmeticDecisions)synchronisedObject;
		this.m_interactable = base.gameObject.RequireComponent<Interactable>();
		this.m_active = this.m_interactable.enabled;
		this.UpdateVisuals(false);
	}

	// Token: 0x0600128D RID: 4749 RVA: 0x000685A9 File Offset: 0x000669A9
	private void Update()
	{
		if (this.m_interactable == null)
		{
			return;
		}
		if (this.m_active != this.m_interactable.enabled)
		{
			this.UpdateVisuals(true);
		}
	}

	// Token: 0x0600128E RID: 4750 RVA: 0x000685DC File Offset: 0x000669DC
	protected void UpdateVisuals(bool _playAudio)
	{
		this.m_active = this.m_interactable.enabled;
		if (this.m_active)
		{
			this.m_decisions.m_buttonBit.sharedMaterial = this.m_decisions.m_activeMaterial;
			if (_playAudio)
			{
				GameUtils.TriggerAudio(GameOneShotAudioTag.SwitchOn, base.gameObject.layer);
			}
		}
		else
		{
			this.m_decisions.m_buttonBit.sharedMaterial = this.m_decisions.m_inactiveMaterial;
			if (_playAudio)
			{
				GameUtils.TriggerAudio(GameOneShotAudioTag.SwitchOff, base.gameObject.layer);
			}
		}
	}

	// Token: 0x0600128F RID: 4751 RVA: 0x00068678 File Offset: 0x00066A78
	protected override void OnEnable()
	{
		base.OnEnable();
		this.m_active = (this.m_interactable != null && this.m_interactable.enabled);
	}

	// Token: 0x04000E84 RID: 3716
	private SwitchCosmeticDecisions m_decisions;

	// Token: 0x04000E85 RID: 3717
	private Interactable m_interactable;

	// Token: 0x04000E86 RID: 3718
	private bool m_active;
}
