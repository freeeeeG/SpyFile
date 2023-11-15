using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000405 RID: 1029
public class ClientToggleSwitchCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x060012B2 RID: 4786 RVA: 0x00068DC0 File Offset: 0x000671C0
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_cosmeticDecisions = (ToggleSwitchCosmeticDecisions)synchronisedObject;
		this.m_interactable = base.gameObject.RequireComponent<Interactable>();
		this.m_toggleOnAnimator = base.gameObject.RequireComponent<ClientTriggerToggleOnAnimator>();
		this.m_stateParameterHash = Animator.StringToHash(this.m_cosmeticDecisions.m_stateParameter);
		this.m_active = this.m_interactable.enabled;
		this.m_toggleState = this.m_toggleOnAnimator.CurrentState;
		this.UpdateVisuals(false);
	}

	// Token: 0x060012B3 RID: 4787 RVA: 0x00068E44 File Offset: 0x00067244
	private void Update()
	{
		if (this.m_interactable == null)
		{
			return;
		}
		if (this.m_active != this.m_interactable.enabled || this.m_toggleState != this.m_toggleOnAnimator.CurrentState)
		{
			this.m_active = this.m_interactable.enabled;
			this.m_toggleState = this.m_toggleOnAnimator.CurrentState;
			this.UpdateVisuals(true);
		}
	}

	// Token: 0x060012B4 RID: 4788 RVA: 0x00068EB8 File Offset: 0x000672B8
	protected void UpdateVisuals(bool _playAudio)
	{
		if (this.m_toggleState)
		{
			this.m_cosmeticDecisions.m_animator.SetBool(this.m_stateParameterHash, true);
		}
		else
		{
			this.m_cosmeticDecisions.m_animator.SetBool(this.m_stateParameterHash, false);
		}
	}

	// Token: 0x04000EA9 RID: 3753
	private ToggleSwitchCosmeticDecisions m_cosmeticDecisions;

	// Token: 0x04000EAA RID: 3754
	private Interactable m_interactable;

	// Token: 0x04000EAB RID: 3755
	private ClientTriggerToggleOnAnimator m_toggleOnAnimator;

	// Token: 0x04000EAC RID: 3756
	private int m_stateParameterHash;

	// Token: 0x04000EAD RID: 3757
	private bool m_active;

	// Token: 0x04000EAE RID: 3758
	private bool m_toggleState;
}
