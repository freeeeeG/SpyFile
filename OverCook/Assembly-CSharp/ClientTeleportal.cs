using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005C8 RID: 1480
public class ClientTeleportal : ClientSynchroniserBase
{
	// Token: 0x06001C26 RID: 7206 RVA: 0x00089597 File Offset: 0x00087997
	public override EntityType GetEntityType()
	{
		return EntityType.Teleportal;
	}

	// Token: 0x06001C27 RID: 7207 RVA: 0x0008959C File Offset: 0x0008799C
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_teleportal = (Teleportal)synchronisedObject;
		this.m_senders = base.gameObject.RequestInterfaces<IClientTeleportalSender>();
		this.m_selfReceivers = base.gameObject.RequestInterfaces<IClientTeleportalReceiver>();
		for (int i = 0; i < this.m_selfReceivers.Length; i++)
		{
			this.m_selfReceivers[i].RegisterStartedTeleportCallback(new ClientTeleportCallback(this.OnReceiverStartedTeleport));
			this.m_selfReceivers[i].RegisterCanTeleportToCallback(() => !this.IsTeleporting() && !this.IsReceiving());
		}
		if (this.m_teleportal.m_exitPortal != null)
		{
			this.m_exitReceivers = this.m_teleportal.m_exitPortal.RequestInterfaces<IClientTeleportalReceiver>();
		}
	}

	// Token: 0x06001C28 RID: 7208 RVA: 0x00089658 File Offset: 0x00087A58
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		TeleportalMessage teleportalMessage = (TeleportalMessage)serialisable;
		TeleportalMessage.MsgType msgType = teleportalMessage.m_msgType;
		if (msgType != TeleportalMessage.MsgType.PortalState)
		{
			if (msgType != TeleportalMessage.MsgType.TeleportStart)
			{
				if (msgType == TeleportalMessage.MsgType.TeleportEnd)
				{
					IClientTeleportable @object = teleportalMessage.m_object.RequireInterface<IClientTeleportable>();
					IEnumerator item = this.TeleportTo(@object, teleportalMessage.m_clientReceiver, teleportalMessage.m_clientSender);
					this.m_receiveRoutines.Add(item);
				}
			}
			else
			{
				IClientTeleportable object2 = teleportalMessage.m_object.RequireInterface<IClientTeleportable>();
				IEnumerator item2 = this.TeleportFrom(object2, teleportalMessage.m_clientSender, teleportalMessage.m_clientReceiver);
				this.m_teleportRoutines.Enqueue(item2);
			}
		}
		else
		{
			if (teleportalMessage.m_canTeleport != this.m_canTeleport)
			{
				this.m_canTeleport = teleportalMessage.m_canTeleport;
				this.m_canTeleportStateChanged(teleportalMessage.m_canTeleport);
			}
			if (teleportalMessage.m_isTeleporting != this.m_teleporting)
			{
				this.m_teleporting = teleportalMessage.m_isTeleporting;
				this.m_teleportStateChanged(teleportalMessage.m_isTeleporting);
			}
		}
	}

	// Token: 0x06001C29 RID: 7209 RVA: 0x00089753 File Offset: 0x00087B53
	public void RegisterCanTeleportChangedCallback(CallbackBool _callback)
	{
		this.m_canTeleportStateChanged = (CallbackBool)Delegate.Combine(this.m_canTeleportStateChanged, _callback);
	}

	// Token: 0x06001C2A RID: 7210 RVA: 0x0008976C File Offset: 0x00087B6C
	public void UnregisterCanTeleportChangedCallback(CallbackBool _callback)
	{
		this.m_canTeleportStateChanged = (CallbackBool)Delegate.Remove(this.m_canTeleportStateChanged, _callback);
	}

	// Token: 0x06001C2B RID: 7211 RVA: 0x00089785 File Offset: 0x00087B85
	public void RegisterTeleportStateChangedCallback(CallbackBool _callback)
	{
		this.m_teleportStateChanged = (CallbackBool)Delegate.Combine(this.m_teleportStateChanged, _callback);
	}

	// Token: 0x06001C2C RID: 7212 RVA: 0x0008979E File Offset: 0x00087B9E
	public void UnregisterTeleportStateChangedCallback(CallbackBool _callback)
	{
		this.m_teleportStateChanged = (CallbackBool)Delegate.Remove(this.m_teleportStateChanged, _callback);
	}

	// Token: 0x06001C2D RID: 7213 RVA: 0x000897B7 File Offset: 0x00087BB7
	public bool CanTeleport(IClientTeleportable _object)
	{
		return this.m_canTeleport;
	}

	// Token: 0x06001C2E RID: 7214 RVA: 0x000897BF File Offset: 0x00087BBF
	public bool IsTeleporting()
	{
		return this.m_teleporting;
	}

	// Token: 0x06001C2F RID: 7215 RVA: 0x000897C8 File Offset: 0x00087BC8
	public bool IsReceiving()
	{
		int num = this.m_selfReceivers.FindIndex_Generic((int x, IClientTeleportalReceiver y) => y.IsReceiving());
		return num >= 0;
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x00089805 File Offset: 0x00087C05
	private void Awake()
	{
		this.m_teleportal = base.gameObject.GetComponent<Teleportal>();
		this.m_animator = base.gameObject.GetComponent<Animator>();
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x0008982C File Offset: 0x00087C2C
	public override void UpdateSynchronising()
	{
		if (this.m_teleportRoutines.Count > 0)
		{
			IEnumerator enumerator = this.m_teleportRoutines.Peek();
			if (enumerator == null || !enumerator.MoveNext())
			{
				this.m_teleportRoutines.Dequeue();
			}
		}
		if (this.m_receiveRoutines.Count > 0)
		{
			Predicate<IEnumerator> match = (IEnumerator _routine) => !_routine.MoveNext();
			this.m_receiveRoutines.RemoveAll(match);
		}
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x000898B0 File Offset: 0x00087CB0
	private IEnumerator TeleportFrom(IClientTeleportable _object, IClientTeleportalSender _sender, IClientTeleportalReceiver _receiver)
	{
		while (_object.IsTeleporting() || !_object.CanTeleport(this))
		{
			yield return null;
		}
		GameUtils.TriggerAudio(GameOneShotAudioTag.TeleportIn, base.gameObject.layer);
		_object.StartTeleportFrom(_sender, _receiver);
		IEnumerator routine = _sender.TeleportFromMe(this, _receiver, _object);
		while (routine.MoveNext())
		{
			yield return null;
		}
		_object.EndTeleportFrom(_sender, _receiver);
		yield break;
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x000898E0 File Offset: 0x00087CE0
	private IEnumerator TeleportTo(IClientTeleportable _object, IClientTeleportalReceiver _receiver, IClientTeleportalSender _sender)
	{
		while (!_object.IsTeleported() || !_object.CanTeleport(this) || !_receiver.CanTeleportTo(_object))
		{
			yield return null;
		}
		GameUtils.TriggerAudio(GameOneShotAudioTag.TeleportOut, base.gameObject.layer);
		_object.StartTeleportTo(_receiver, _sender);
		IEnumerator routine = _receiver.TeleportToMe(this, _sender, _object);
		while (routine.MoveNext())
		{
			yield return null;
		}
		_object.EndTeleportTo(_receiver, _sender);
		yield break;
	}

	// Token: 0x06001C34 RID: 7220 RVA: 0x00089910 File Offset: 0x00087D10
	private void OnReceiverStartedTeleport(IClientTeleportable _object)
	{
	}

	// Token: 0x04001602 RID: 5634
	private Teleportal m_teleportal;

	// Token: 0x04001603 RID: 5635
	private IClientTeleportalSender[] m_senders = new IClientTeleportalSender[0];

	// Token: 0x04001604 RID: 5636
	private IClientTeleportalReceiver[] m_selfReceivers = new IClientTeleportalReceiver[0];

	// Token: 0x04001605 RID: 5637
	private IClientTeleportalReceiver[] m_exitReceivers = new IClientTeleportalReceiver[0];

	// Token: 0x04001606 RID: 5638
	private Animator m_animator;

	// Token: 0x04001607 RID: 5639
	private Queue<IEnumerator> m_teleportRoutines = new Queue<IEnumerator>();

	// Token: 0x04001608 RID: 5640
	private List<IEnumerator> m_receiveRoutines = new List<IEnumerator>();

	// Token: 0x04001609 RID: 5641
	private CallbackBool m_canTeleportStateChanged = delegate(bool _canTeleport)
	{
	};

	// Token: 0x0400160A RID: 5642
	private CallbackBool m_teleportStateChanged = delegate(bool _teleporting)
	{
	};

	// Token: 0x0400160B RID: 5643
	private bool m_teleporting;

	// Token: 0x0400160C RID: 5644
	private bool m_canTeleport;
}
