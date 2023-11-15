using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200019B RID: 411
public class ClientTriggerToggleOnAnimator : ClientSynchroniserBase
{
	// Token: 0x060006FC RID: 1788 RVA: 0x0002E1DE File Offset: 0x0002C5DE
	public override EntityType GetEntityType()
	{
		return EntityType.TriggerToggleOnAnimator;
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x0002E1E4 File Offset: 0x0002C5E4
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerOnAnimator = (TriggerToggleOnAnimator)synchronisedObject;
		if (this.m_triggerOnAnimator.m_targetAnimator != null)
		{
			this.m_currentValue = this.m_triggerOnAnimator.m_initialValue;
			this.m_triggerOnAnimator.m_targetAnimator.SetBool(this.m_triggerOnAnimator.m_targetParameterHash, this.m_currentValue);
		}
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x0002E24C File Offset: 0x0002C64C
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		TriggerToggleOnAnimatorMessage triggerToggleOnAnimatorMessage = (TriggerToggleOnAnimatorMessage)serialisable;
		this.SetAnimatorState(triggerToggleOnAnimatorMessage.m_value);
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x060006FF RID: 1791 RVA: 0x0002E26C File Offset: 0x0002C66C
	public bool CurrentState
	{
		get
		{
			if (this.m_triggerOnAnimator.m_targetAnimator != null)
			{
				return this.m_triggerOnAnimator.m_targetAnimator.GetBool(this.m_triggerOnAnimator.m_targetParameterHash);
			}
			return this.m_currentValue;
		}
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x0002E2A8 File Offset: 0x0002C6A8
	private void SetAnimatorState(bool _state)
	{
		if (this.m_triggerOnAnimator.enabled && this.m_triggerOnAnimator.m_targetAnimator != null)
		{
			this.m_currentValue = _state;
			this.m_triggerOnAnimator.m_targetAnimator.SetBool(this.m_triggerOnAnimator.m_targetParameterHash, _state);
		}
	}

	// Token: 0x040005D0 RID: 1488
	private TriggerToggleOnAnimator m_triggerOnAnimator;

	// Token: 0x040005D1 RID: 1489
	private bool m_currentValue;
}
