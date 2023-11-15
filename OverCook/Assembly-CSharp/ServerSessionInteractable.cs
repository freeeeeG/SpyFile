using System;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005A9 RID: 1449
public abstract class ServerSessionInteractable : ServerSynchroniserBase
{
	// Token: 0x06001B85 RID: 7045 RVA: 0x0006F43C File Offset: 0x0006D83C
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_sessionInteractable = (SessionInteractable)synchronisedObject;
		this.m_interactable = base.gameObject.GetComponent<ServerInteractable>();
		this.m_interactable.RegisterTriggerCallbacks(new ServerInteractable.BeginInteractCallback(this.StartSession));
		this.m_interactable.RegisterCanInteractCallbacks(new Generic<bool, GameObject>(this.CanInteract));
	}

	// Token: 0x06001B86 RID: 7046 RVA: 0x0006F49D File Offset: 0x0006D89D
	public override EntityType GetEntityType()
	{
		return EntityType.SessionInteractable;
	}

	// Token: 0x06001B87 RID: 7047 RVA: 0x0006F4A4 File Offset: 0x0006D8A4
	private void SynchroniseInteractionState(GameObject _interacter)
	{
		this.m_data.m_msgType = SessionInteractableMessage.MessageType.InteractionState;
		if (_interacter)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(_interacter);
			uint uEntityID = entry.m_Header.m_uEntityID;
			this.m_data.m_interacterID = uEntityID;
		}
		else
		{
			this.m_data.m_interacterID = 0U;
		}
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06001B88 RID: 7048
	protected abstract ServerSessionInteractable.SessionBase BuildSession(GameObject _interacter);

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06001B89 RID: 7049 RVA: 0x0006F504 File Offset: 0x0006D904
	protected ServerSessionInteractable.SessionBase Session
	{
		get
		{
			return this.m_session;
		}
	}

	// Token: 0x06001B8A RID: 7050 RVA: 0x0006F50C File Offset: 0x0006D90C
	protected virtual void Awake()
	{
		base.enabled = false;
	}

	// Token: 0x06001B8B RID: 7051 RVA: 0x0006F518 File Offset: 0x0006D918
	protected virtual bool CanInteract(GameObject _interacter)
	{
		ICarrier carrier = _interacter.RequireInterface<ICarrier>();
		return carrier != null && !(carrier.InspectCarriedItem() != null);
	}

	// Token: 0x06001B8C RID: 7052 RVA: 0x0006F548 File Offset: 0x0006D948
	protected virtual void StartSession(GameObject _interacter, Vector2 _directionXZ)
	{
		this.m_interactable.SetInteractionSuppressed(true);
		this.SynchroniseInteractionState(_interacter);
		this.m_session = this.BuildSession(_interacter);
		base.enabled = true;
		if (this.m_sessionInteractable.m_onSessionBegun != string.Empty)
		{
			base.SendMessage("OnTrigger", this.m_sessionInteractable.m_onSessionBegun, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06001B8D RID: 7053 RVA: 0x0006F5B0 File Offset: 0x0006D9B0
	protected virtual void OnSessionEnded()
	{
		this.SynchroniseInteractionState(null);
		this.m_interactable.SetInteractionSuppressed(false);
		this.m_session = null;
		base.enabled = false;
		if (this.m_sessionInteractable.m_onSesionEnded != string.Empty)
		{
			base.SendMessage("OnTrigger", this.m_sessionInteractable.m_onSesionEnded, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06001B8E RID: 7054 RVA: 0x0006F60F File Offset: 0x0006DA0F
	public override void UpdateSynchronising()
	{
		this.m_session.Update();
		if (!this.m_session.IsRunning)
		{
			this.OnSessionEnded();
		}
	}

	// Token: 0x06001B8F RID: 7055 RVA: 0x0006F634 File Offset: 0x0006DA34
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_interactable != null)
		{
			this.m_interactable.UnregisterTriggerCallbacks(new ServerInteractable.BeginInteractCallback(this.StartSession));
			this.m_interactable.UnregisterCanInteractCallbacks(new Generic<bool, GameObject>(this.CanInteract));
		}
	}

	// Token: 0x040015A2 RID: 5538
	private SessionInteractable m_sessionInteractable;

	// Token: 0x040015A3 RID: 5539
	private ServerInteractable m_interactable;

	// Token: 0x040015A4 RID: 5540
	private ServerSessionInteractable.SessionBase m_session;

	// Token: 0x040015A5 RID: 5541
	private SessionInteractableMessage m_data = new SessionInteractableMessage();

	// Token: 0x020005AA RID: 1450
	protected abstract class SessionBase
	{
		// Token: 0x06001B90 RID: 7056 RVA: 0x0006F688 File Offset: 0x0006DA88
		public SessionBase(ServerSessionInteractable _self, GameObject _avatar)
		{
			this.m_PlayerLayer = LayerMask.NameToLayer("Players");
			this.gameObject = _self.gameObject;
			this.m_movement = _avatar.RequireComponent<PlayerControls>();
			this.m_movement.enabled = false;
			this.m_CollisionRecorder = _avatar.RequireComponent<CollisionRecorder>();
			this.m_CollisionRecorder.SetFilter(new Generic<bool, Collision>(this.CollisionFilter));
			this.m_RigidBody = _avatar.RequireComponent<Rigidbody>();
			this.m_RigidBody.isKinematic = true;
			this.m_movement.ControlScheme.ClearEvents();
			Mailbox.Client.RegisterForMessageType(MessageType.DestroyChef, new OrderedMessageReceivedCallback(this.OnDestroyChefMessageReceived));
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06001B91 RID: 7057 RVA: 0x0006F73A File Offset: 0x0006DB3A
		protected PlayerControls PlayerControls
		{
			get
			{
				return this.m_movement;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06001B92 RID: 7058 RVA: 0x0006F742 File Offset: 0x0006DB42
		public GameObject Avatar
		{
			get
			{
				return this.m_movement.gameObject;
			}
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x0006F750 File Offset: 0x0006DB50
		public virtual void OnSessionEnded()
		{
			this.m_running = false;
			if (this.m_CollisionRecorder != null)
			{
				this.m_CollisionRecorder.SetFilter(null);
			}
			if (this.m_RigidBody != null)
			{
				this.m_RigidBody.isKinematic = false;
			}
			if (this.m_movement != null)
			{
				this.m_movement.enabled = true;
			}
			Mailbox.Client.UnregisterForMessageType(MessageType.DestroyChef, new OrderedMessageReceivedCallback(this.OnDestroyChefMessageReceived));
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x0006F7D4 File Offset: 0x0006DBD4
		public virtual void OnDestroyChefMessageReceived(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			DestroyChefMessage destroyChefMessage = (DestroyChefMessage)message;
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(destroyChefMessage.m_Chef.m_Header.m_uEntityID);
			if (entry != null && entry.m_GameObject != null)
			{
				PlayerControls y = entry.m_GameObject.RequireComponent<PlayerControls>();
				if (this.m_movement == y)
				{
					this.OnSessionEnded();
				}
			}
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x0006F838 File Offset: 0x0006DC38
		private bool CollisionFilter(Collision _collision)
		{
			return _collision != null && _collision.gameObject != null && _collision.rigidbody != null && _collision.gameObject.layer == this.m_PlayerLayer && _collision.rigidbody.velocity.sqrMagnitude > 0.001f;
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x0006F8A4 File Offset: 0x0006DCA4
		public virtual void Update()
		{
			if ((this.m_movement == null || this.m_movement.ControlScheme.m_pickupButton.JustPressed() || this.m_movement.ControlScheme.m_worksurfaceUseButton.JustPressed() || this.m_movement.ControlScheme.m_dashButton.JustPressed() || !this.m_movement.GetDirectlyUnderPlayerControl()) && this.m_running)
			{
				this.OnSessionEnded();
			}
			if (this.m_running)
			{
				List<Collision> recentCollisions = this.m_CollisionRecorder.GetRecentCollisions();
				for (int i = 0; i < recentCollisions.Count; i++)
				{
					PlayerControls component = recentCollisions[i].gameObject.GetComponent<PlayerControls>();
					if (null != component && component.IsDashing())
					{
						this.OnSessionEnded();
						break;
					}
				}
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06001B97 RID: 7063 RVA: 0x0006F993 File Offset: 0x0006DD93
		public bool IsRunning
		{
			get
			{
				return this.m_running;
			}
		}

		// Token: 0x040015A6 RID: 5542
		private GameObject gameObject;

		// Token: 0x040015A7 RID: 5543
		private PlayerControls m_movement;

		// Token: 0x040015A8 RID: 5544
		protected bool m_running = true;

		// Token: 0x040015A9 RID: 5545
		private CollisionRecorder m_CollisionRecorder;

		// Token: 0x040015AA RID: 5546
		private int m_PlayerLayer;

		// Token: 0x040015AB RID: 5547
		private Rigidbody m_RigidBody;
	}
}
