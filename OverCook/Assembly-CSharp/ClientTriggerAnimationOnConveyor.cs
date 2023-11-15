using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005B4 RID: 1460
public class ClientTriggerAnimationOnConveyor : ClientSynchroniserBase
{
	// Token: 0x06001BC4 RID: 7108 RVA: 0x000880BB File Offset: 0x000864BB
	public override EntityType GetEntityType()
	{
		return EntityType.ConveyorAnimator;
	}

	// Token: 0x06001BC5 RID: 7109 RVA: 0x000880BF File Offset: 0x000864BF
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerAnimationOnConveyor = (TriggerAnimationOnConveyor)synchronisedObject;
	}

	// Token: 0x06001BC6 RID: 7110 RVA: 0x000880D4 File Offset: 0x000864D4
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		ConveyorAnimationMessage conveyorAnimationMessage = (ConveyorAnimationMessage)serialisable;
		if (conveyorAnimationMessage.m_state != this.m_state)
		{
			this.m_state = conveyorAnimationMessage.m_state;
			if (this.m_state == TriggerAnimationOnConveyor.State.Animating)
			{
				this.m_animator.SetTrigger(this.m_triggerAnimationOnConveyor.m_animationStartTriggerHash);
			}
		}
	}

	// Token: 0x06001BC7 RID: 7111 RVA: 0x00088127 File Offset: 0x00086527
	public override void UpdateSynchronising()
	{
		if (this.m_animator == null)
		{
			this.m_animator = base.gameObject.RequestComponent<Animator>();
		}
	}

	// Token: 0x040015C3 RID: 5571
	private TriggerAnimationOnConveyor m_triggerAnimationOnConveyor;

	// Token: 0x040015C4 RID: 5572
	private Animator m_animator;

	// Token: 0x040015C5 RID: 5573
	private TriggerAnimationOnConveyor.State m_state;
}
