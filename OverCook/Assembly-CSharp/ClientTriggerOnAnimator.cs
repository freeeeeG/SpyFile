using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000187 RID: 391
public class ClientTriggerOnAnimator : ClientSynchroniserBase
{
	// Token: 0x060006C1 RID: 1729 RVA: 0x0002D8B1 File Offset: 0x0002BCB1
	public override EntityType GetEntityType()
	{
		return EntityType.TriggerOnAnimator;
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x0002D8B5 File Offset: 0x0002BCB5
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerOnAnimator = (TriggerOnAnimator)synchronisedObject;
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x0002D8CA File Offset: 0x0002BCCA
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		this.DoEvent();
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x0002D8D4 File Offset: 0x0002BCD4
	private void DoEvent()
	{
		if (this.m_triggerOnAnimator.enabled && this.m_triggerOnAnimator.m_targetAnimator != null)
		{
			this.m_triggerOnAnimator.m_targetAnimator.SetTrigger(this.m_triggerOnAnimator.m_triggerToFireHash);
		}
	}

	// Token: 0x0400059A RID: 1434
	private TriggerOnAnimator m_triggerOnAnimator;
}
