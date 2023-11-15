using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005B3 RID: 1459
public class ServerTriggerAnimationOnConveyor : ServerSynchroniserBase
{
	// Token: 0x06001BBC RID: 7100 RVA: 0x00087EF4 File Offset: 0x000862F4
	public override EntityType GetEntityType()
	{
		return EntityType.ConveyorAnimator;
	}

	// Token: 0x06001BBD RID: 7101 RVA: 0x00087EF8 File Offset: 0x000862F8
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_receiver = base.gameObject.RequestInterface<IConveyenceReceiver>();
		this.m_station = base.gameObject.RequireComponent<ServerConveyorStation>();
		if (this.m_triggerAnimationOnConveyor.m_stopWhileAnimating)
		{
			this.m_station.RegisterAllowConveyCallback(new Generic<bool>(this.AllowConvey));
			this.m_receiver.RegisterAllowConveyToCallback(new Generic<bool>(this.AllowConvey));
		}
		this.m_triggerAnimationOnConveyor.RegisterOnTriggerCallback(new GenericVoid<string>(this.OnTrigger));
	}

	// Token: 0x06001BBE RID: 7102 RVA: 0x00087F83 File Offset: 0x00086383
	private void SendStateChange()
	{
		this.m_data.Initialise(this.m_state);
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06001BBF RID: 7103 RVA: 0x00087FA2 File Offset: 0x000863A2
	private void Awake()
	{
		this.m_triggerAnimationOnConveyor = base.gameObject.RequireComponent<TriggerAnimationOnConveyor>();
		this.m_triggerAnimationOnConveyor.RegisterOnTriggerCallback(new GenericVoid<string>(this.OnTrigger));
	}

	// Token: 0x06001BC0 RID: 7104 RVA: 0x00087FCC File Offset: 0x000863CC
	private bool AllowConvey()
	{
		return !this.m_triggerAnimationOnConveyor.m_stopWhileAnimating || this.m_state == TriggerAnimationOnConveyor.State.Idle;
	}

	// Token: 0x06001BC1 RID: 7105 RVA: 0x00087FEC File Offset: 0x000863EC
	private void OnTrigger(string _trigger)
	{
		if (this.m_triggerAnimationOnConveyor.m_startTrigger == _trigger)
		{
			this.m_state = TriggerAnimationOnConveyor.State.Pending;
			this.SendStateChange();
		}
		if (this.m_triggerAnimationOnConveyor.m_animationFinishedTrigger == _trigger)
		{
			this.m_state = TriggerAnimationOnConveyor.State.Idle;
			this.SendStateChange();
		}
	}

	// Token: 0x06001BC2 RID: 7106 RVA: 0x00088040 File Offset: 0x00086440
	public override void UpdateSynchronising()
	{
		if (this.m_animator == null)
		{
			this.m_animator = base.gameObject.RequestComponent<Animator>();
		}
		if (this.m_state == TriggerAnimationOnConveyor.State.Pending && !this.m_station.IsConveying() && (this.m_receiver == null || !this.m_receiver.IsReceiving()))
		{
			this.m_state = TriggerAnimationOnConveyor.State.Animating;
			this.SendStateChange();
		}
	}

	// Token: 0x040015BD RID: 5565
	private TriggerAnimationOnConveyor m_triggerAnimationOnConveyor;

	// Token: 0x040015BE RID: 5566
	private ConveyorAnimationMessage m_data = new ConveyorAnimationMessage();

	// Token: 0x040015BF RID: 5567
	private ServerConveyorStation m_station;

	// Token: 0x040015C0 RID: 5568
	private IConveyenceReceiver m_receiver;

	// Token: 0x040015C1 RID: 5569
	private Animator m_animator;

	// Token: 0x040015C2 RID: 5570
	private TriggerAnimationOnConveyor.State m_state;
}
