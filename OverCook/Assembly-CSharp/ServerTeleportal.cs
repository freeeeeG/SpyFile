using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005C7 RID: 1479
public class ServerTeleportal : ServerSynchroniserBase
{
	// Token: 0x06001C01 RID: 7169 RVA: 0x00088997 File Offset: 0x00086D97
	public override EntityType GetEntityType()
	{
		return EntityType.Teleportal;
	}

	// Token: 0x06001C02 RID: 7170 RVA: 0x0008899C File Offset: 0x00086D9C
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_teleportal = (Teleportal)synchronisedObject;
		this.m_senders = base.gameObject.RequestInterfaces<ITeleportalSender>();
		this.m_selfReceivers = base.gameObject.RequestInterfaces<ITeleportalReceiver>();
		for (int i = 0; i < this.m_selfReceivers.Length; i++)
		{
			this.m_selfReceivers[i].RegisterAllowTeleportCallback(() => !this.IsTeleporting() && !this.IsReceiving());
			this.m_selfReceivers[i].RegisterStartedTeleportCallback(new TeleportCallback(this.OnReceiverStartedTeleport));
			this.m_selfReceivers[i].RegisterFinishedTeleportCallback(new TeleportCallback(this.OnReceiverFinishedTeleport));
		}
		if (this.m_teleportal.m_exitPortal != null)
		{
			this.m_exitReceivers = this.m_teleportal.m_exitPortal.RequestInterfaces<ITeleportalReceiver>();
		}
	}

	// Token: 0x06001C03 RID: 7171 RVA: 0x00088A6E File Offset: 0x00086E6E
	private void SynchroniseTeleportalState()
	{
		if (!MultiplayerController.IsSynchronisationActive())
		{
			return;
		}
		ServerTeleportal.m_data.Initialise_State(base.enabled, this.m_teleporting);
		this.SendServerEvent(ServerTeleportal.m_data);
	}

	// Token: 0x06001C04 RID: 7172 RVA: 0x00088A9C File Offset: 0x00086E9C
	private void SendTeleportFrom(ITeleportalSender _sender, ITeleportalReceiver _receiver, ITeleportable _object)
	{
		ServerTeleportal.m_data.Initialise_StartTeleport(_sender, _receiver, _object);
		this.SendServerEvent(ServerTeleportal.m_data);
	}

	// Token: 0x06001C05 RID: 7173 RVA: 0x00088AB6 File Offset: 0x00086EB6
	private void SendTeleportTo(ITeleportalReceiver _receiver, ITeleportalSender _sender, ITeleportable _object)
	{
		ServerTeleportal.m_data.Initialise_EndTeleport(_receiver, _sender, _object);
		this.SendServerEvent(ServerTeleportal.m_data);
	}

	// Token: 0x06001C06 RID: 7174 RVA: 0x00088AD0 File Offset: 0x00086ED0
	public void RegisterCanTeleportChangedCallback(CallbackBool _callback)
	{
		this.m_canTeleportStateChanged = (CallbackBool)Delegate.Combine(this.m_canTeleportStateChanged, _callback);
	}

	// Token: 0x06001C07 RID: 7175 RVA: 0x00088AE9 File Offset: 0x00086EE9
	public void UnregisterCanTeleportChangedCallback(CallbackBool _callback)
	{
		this.m_canTeleportStateChanged = (CallbackBool)Delegate.Remove(this.m_canTeleportStateChanged, _callback);
	}

	// Token: 0x06001C08 RID: 7176 RVA: 0x00088B02 File Offset: 0x00086F02
	public void RegisterTeleportStateChangedCallback(CallbackBool _callback)
	{
		this.m_teleportStateChanged = (CallbackBool)Delegate.Combine(this.m_teleportStateChanged, _callback);
	}

	// Token: 0x06001C09 RID: 7177 RVA: 0x00088B1B File Offset: 0x00086F1B
	public void UnregisterTeleportStateChangedCallback(CallbackBool _callback)
	{
		this.m_teleportStateChanged = (CallbackBool)Delegate.Remove(this.m_teleportStateChanged, _callback);
	}

	// Token: 0x06001C0A RID: 7178 RVA: 0x00088B34 File Offset: 0x00086F34
	public bool CanTeleport(ITeleportable _object)
	{
		if (!this.m_teleporting && !this.m_cooldown && (this.m_teleportal.m_allowImmediateReteleport || !this.m_recentlyTeleported.Contains(_object)))
		{
			ITeleportalSender senderForObject = this.GetSenderForObject(_object);
			ITeleportalReceiver receiverForObject = this.GetReceiverForObject(_object);
			return receiverForObject != null && senderForObject != null && senderForObject.CanTeleport(_object);
		}
		return false;
	}

	// Token: 0x06001C0B RID: 7179 RVA: 0x00088BA0 File Offset: 0x00086FA0
	public bool IsTeleporting()
	{
		return this.m_teleporting;
	}

	// Token: 0x06001C0C RID: 7180 RVA: 0x00088BA8 File Offset: 0x00086FA8
	public bool IsReceiving()
	{
		int num = this.m_selfReceivers.FindIndex_Generic((int x, ITeleportalReceiver y) => y.IsReceiving());
		return num >= 0;
	}

	// Token: 0x06001C0D RID: 7181 RVA: 0x00088BE8 File Offset: 0x00086FE8
	private ITeleportalSender GetSenderForObject(ITeleportable _object)
	{
		Predicate<ITeleportalSender> matchFunction = (ITeleportalSender _sender) => _sender.CanHandleTeleport(_object);
		int num = this.m_senders.FindIndex_Predicate(matchFunction);
		return (num < 0) ? null : this.m_senders[num];
	}

	// Token: 0x06001C0E RID: 7182 RVA: 0x00088C34 File Offset: 0x00087034
	private ITeleportalReceiver GetReceiverForObject(ITeleportable _object)
	{
		Predicate<ITeleportalReceiver> matchFunction = (ITeleportalReceiver _receiver) => _receiver.CanHandleTeleport(_object);
		int num = this.m_exitReceivers.FindIndex_Predicate(matchFunction);
		return (num < 0) ? null : this.m_exitReceivers[num];
	}

	// Token: 0x06001C0F RID: 7183 RVA: 0x00088C7D File Offset: 0x0008707D
	private void Awake()
	{
		this.m_teleportal = base.gameObject.GetComponent<Teleportal>();
		this.m_collider = base.gameObject.GetComponent<Collider>();
		this.m_triggerRecorder = base.gameObject.RequireComponent<TriggerRecorder>();
	}

	// Token: 0x06001C10 RID: 7184 RVA: 0x00088CB2 File Offset: 0x000870B2
	protected override void OnEnable()
	{
		base.OnEnable();
		this.m_canTeleportStateChanged(true);
		this.SynchroniseTeleportalState();
	}

	// Token: 0x06001C11 RID: 7185 RVA: 0x00088CCC File Offset: 0x000870CC
	protected override void OnDisable()
	{
		base.OnDisable();
		this.m_canTeleportStateChanged(false);
		this.SynchroniseTeleportalState();
	}

	// Token: 0x06001C12 RID: 7186 RVA: 0x00088CE8 File Offset: 0x000870E8
	public override void UpdateSynchronising()
	{
		if (this.m_recentlyTeleported.Count > 0)
		{
			List<Collider> collisions = this.m_triggerRecorder.GetRecentCollisions();
			Predicate<ITeleportable> match = delegate(ITeleportable _object)
			{
				if (_object == null || (MonoBehaviour)_object == null)
				{
					return true;
				}
				GameObject obj = ((MonoBehaviour)_object).gameObject;
				return this.IsFacingPortal(obj) || !collisions.Exists((Collider x) => x.transform.IsChildOf(obj.transform));
			};
			this.m_recentlyTeleported.RemoveAll(match);
		}
		if (this.m_receiveRoutines.Count > 0)
		{
			Predicate<IEnumerator> match2 = (IEnumerator _teleportRoutine) => !_teleportRoutine.MoveNext();
			this.m_receiveRoutines.RemoveAll(match2);
		}
	}

	// Token: 0x06001C13 RID: 7187 RVA: 0x00088D7C File Offset: 0x0008717C
	public void Teleport(ITeleportable _object)
	{
		ITeleportalSender senderForObject = this.GetSenderForObject(_object);
		ITeleportalReceiver receiverForObject = this.GetReceiverForObject(_object);
		_object.StartTeleport(senderForObject, receiverForObject);
		this.m_teleportRoutine = this.TeleportFrom(_object, senderForObject, receiverForObject);
		base.StartCoroutine(this.m_teleportRoutine);
		this.m_teleporting = true;
		this.m_teleportStateChanged(true);
		this.SynchroniseTeleportalState();
	}

	// Token: 0x06001C14 RID: 7188 RVA: 0x00088DD8 File Offset: 0x000871D8
	private IEnumerator TeleportFrom(ITeleportable _object, ITeleportalSender _sender, ITeleportalReceiver _receiver)
	{
		this.SendTeleportFrom(_sender, _receiver, _object);
		IEnumerator routine = _sender.TeleportFromMe(this, _receiver, _object);
		while (routine.MoveNext())
		{
			yield return null;
		}
		this.m_receiveRoutines.Add(this.TeleportTo(_object, _sender, _receiver));
		this.EndTeleport(_object);
		yield break;
	}

	// Token: 0x06001C15 RID: 7189 RVA: 0x00088E08 File Offset: 0x00087208
	private IEnumerator TeleportTo(ITeleportable _object, ITeleportalSender _sender, ITeleportalReceiver _receiver)
	{
		IEnumerator routine = CoroutineUtils.TimerRoutine(this.m_teleportal.m_receiveDelay, base.gameObject.layer);
		while (routine.MoveNext())
		{
			yield return null;
		}
		while (!_receiver.CanTeleportTo(_object))
		{
			yield return null;
		}
		this.SendTeleportTo(_receiver, _sender, _object);
		routine = _receiver.TeleportToMe(this, _sender, _object);
		while (routine.MoveNext())
		{
			yield return null;
		}
		_object.EndTeleport(_receiver, _sender);
		yield break;
	}

	// Token: 0x06001C16 RID: 7190 RVA: 0x00088E38 File Offset: 0x00087238
	public void EndTeleport(ITeleportable _object)
	{
		if (this.m_teleportRoutine != null)
		{
			base.StopCoroutine(this.m_teleportRoutine);
		}
		this.m_teleporting = false;
		this.m_teleportStateChanged(false);
		this.SynchroniseTeleportalState();
		base.StartCoroutine(this.PostTeleportCooldown());
		if (!this.m_teleportal.m_allowImmediateReteleport)
		{
			this.m_recentlyTeleported.Add(_object);
		}
		this.m_teleportRoutine = null;
	}

	// Token: 0x06001C17 RID: 7191 RVA: 0x00088EA8 File Offset: 0x000872A8
	private IEnumerator PostTeleportCooldown()
	{
		this.m_cooldown = true;
		IEnumerator timer = CoroutineUtils.TimerRoutine(this.m_teleportal.m_cooldownTime, base.gameObject.layer);
		while (timer.MoveNext())
		{
			yield return null;
		}
		this.m_cooldown = false;
		yield break;
	}

	// Token: 0x06001C18 RID: 7192 RVA: 0x00088EC4 File Offset: 0x000872C4
	private bool IsPointWithinConeXZ(Vector3 _point, Vector3 _coneOrigin, Vector3 _coneDirection, float _coneAngle)
	{
		Vector3 a = (_coneOrigin - _point).WithY(0f).SafeNormalised(Vector3.zero);
		if (a.sqrMagnitude > 0f)
		{
			float num = Vector3.Angle(_coneDirection, -a);
			return num < _coneAngle;
		}
		return false;
	}

	// Token: 0x06001C19 RID: 7193 RVA: 0x00088F14 File Offset: 0x00087314
	private bool IsInTeleportArc(GameObject _object)
	{
		return this.IsPointWithinConeXZ(_object.transform.position, this.m_collider.bounds.center, base.transform.right, this.m_teleportal.m_teleportArc);
	}

	// Token: 0x06001C1A RID: 7194 RVA: 0x00088F5C File Offset: 0x0008735C
	private bool IsFacingPortal(GameObject _object)
	{
		return this.IsPointWithinConeXZ(this.m_collider.bounds.center, _object.transform.position, _object.transform.forward, 45f);
	}

	// Token: 0x06001C1B RID: 7195 RVA: 0x00088F9D File Offset: 0x0008739D
	private void OnReceiverStartedTeleport(ITeleportable _object)
	{
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x00088F9F File Offset: 0x0008739F
	private void OnReceiverFinishedTeleport(ITeleportable _object)
	{
		base.StartCoroutine(this.PostTeleportCooldown());
		if (!this.m_teleportal.m_allowImmediateReteleport)
		{
			this.m_recentlyTeleported.Add(_object);
		}
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x00088FCC File Offset: 0x000873CC
	private ITeleportable FindTeleportable(Transform _transform)
	{
		ITeleportable teleportable = _transform.gameObject.RequestInterface<ITeleportable>();
		if (teleportable != null && teleportable.CanTeleport(this))
		{
			return teleportable;
		}
		if (_transform.parent != null)
		{
			return this.FindTeleportable(_transform.parent);
		}
		return null;
	}

	// Token: 0x06001C1E RID: 7198 RVA: 0x00089018 File Offset: 0x00087418
	private void OnTriggerStay(Collider collider)
	{
		if (this.m_teleportRoutine == null && !this.IsTeleporting() && !this.IsReceiving())
		{
			ITeleportable teleportable = this.FindTeleportable(collider.transform);
			if (teleportable != null && this.CanTeleport(teleportable) && this.IsInTeleportArc(collider.gameObject))
			{
				this.Teleport(teleportable);
			}
		}
	}

	// Token: 0x040015EF RID: 5615
	private Teleportal m_teleportal;

	// Token: 0x040015F0 RID: 5616
	public static TeleportalMessage m_data = new TeleportalMessage();

	// Token: 0x040015F1 RID: 5617
	private const float c_facingTeleportAngleMax = 45f;

	// Token: 0x040015F2 RID: 5618
	private ITeleportalSender[] m_senders = new ITeleportalSender[0];

	// Token: 0x040015F3 RID: 5619
	private ITeleportalReceiver[] m_selfReceivers = new ITeleportalReceiver[0];

	// Token: 0x040015F4 RID: 5620
	private ITeleportalReceiver[] m_exitReceivers = new ITeleportalReceiver[0];

	// Token: 0x040015F5 RID: 5621
	private Collider m_collider;

	// Token: 0x040015F6 RID: 5622
	private TriggerRecorder m_triggerRecorder;

	// Token: 0x040015F7 RID: 5623
	private CallbackBool m_canTeleportStateChanged = delegate(bool _canTeleport)
	{
	};

	// Token: 0x040015F8 RID: 5624
	private CallbackBool m_teleportStateChanged = delegate(bool _teleporting)
	{
	};

	// Token: 0x040015F9 RID: 5625
	private List<ITeleportable> m_recentlyTeleported = new List<ITeleportable>();

	// Token: 0x040015FA RID: 5626
	private List<IEnumerator> m_receiveRoutines = new List<IEnumerator>();

	// Token: 0x040015FB RID: 5627
	private IEnumerator m_teleportRoutine;

	// Token: 0x040015FC RID: 5628
	private bool m_teleporting;

	// Token: 0x040015FD RID: 5629
	private bool m_cooldown;
}
