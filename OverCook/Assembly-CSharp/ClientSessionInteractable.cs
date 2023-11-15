using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005AB RID: 1451
public abstract class ClientSessionInteractable : ClientSynchroniserBase
{
	// Token: 0x06001B99 RID: 7065 RVA: 0x0006FBC2 File Offset: 0x0006DFC2
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_interactable = base.gameObject.GetComponent<ClientInteractable>();
		this.m_interactable.SetStickyInteractionCallback(new Generic<bool>(this.InteractionIsSticky));
	}

	// Token: 0x06001B9A RID: 7066 RVA: 0x0006FBF3 File Offset: 0x0006DFF3
	public override EntityType GetEntityType()
	{
		return EntityType.SessionInteractable;
	}

	// Token: 0x06001B9B RID: 7067 RVA: 0x0006FBF8 File Offset: 0x0006DFF8
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		SessionInteractableMessage sessionInteractableMessage = (SessionInteractableMessage)serialisable;
		SessionInteractableMessage.MessageType msgType = sessionInteractableMessage.m_msgType;
		if (msgType == SessionInteractableMessage.MessageType.InteractionState)
		{
			uint interacterID = sessionInteractableMessage.m_interacterID;
			if (interacterID != 0U)
			{
				EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(interacterID);
				this.OnSessionStarted(entry.m_GameObject);
			}
			else
			{
				this.OnSessionEnded();
			}
		}
	}

	// Token: 0x06001B9C RID: 7068 RVA: 0x0006FC4E File Offset: 0x0006E04E
	public override void UpdateSynchronising()
	{
		if (this.m_session != null)
		{
			this.m_session.Update();
		}
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x06001B9D RID: 7069 RVA: 0x0006FC66 File Offset: 0x0006E066
	public bool HasSession
	{
		get
		{
			return this.m_session != null;
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06001B9E RID: 7070 RVA: 0x0006FC74 File Offset: 0x0006E074
	protected ClientSessionInteractable.SessionBase Session
	{
		get
		{
			return this.m_session;
		}
	}

	// Token: 0x06001B9F RID: 7071
	protected abstract ClientSessionInteractable.SessionBase BuildSession(GameObject _interacter);

	// Token: 0x06001BA0 RID: 7072 RVA: 0x0006FC7C File Offset: 0x0006E07C
	private void OnSessionStarted(GameObject _avatar)
	{
		this.m_session = this.BuildSession(_avatar);
		this.m_interactable.SetInteractionSuppressed(true);
	}

	// Token: 0x06001BA1 RID: 7073 RVA: 0x0006FC97 File Offset: 0x0006E097
	private void OnSessionEnded()
	{
		this.m_session.OnSessionEnded();
		this.m_session = null;
		this.m_interactable.SetInteractionSuppressed(false);
	}

	// Token: 0x06001BA2 RID: 7074 RVA: 0x0006FCB7 File Offset: 0x0006E0B7
	private bool InteractionIsSticky()
	{
		return false;
	}

	// Token: 0x040015AC RID: 5548
	private ClientInteractable m_interactable;

	// Token: 0x040015AD RID: 5549
	private ClientSessionInteractable.SessionBase m_session;

	// Token: 0x040015AE RID: 5550
	private ClientWorldObjectSynchroniser m_ChefWorldObjectSynchroniser;

	// Token: 0x020005AC RID: 1452
	protected abstract class SessionBase
	{
		// Token: 0x06001BA3 RID: 7075 RVA: 0x0006FCBC File Offset: 0x0006E0BC
		public SessionBase(ClientSessionInteractable _self, GameObject _avatar)
		{
			this.m_interactable = _self;
			this.m_movement = _avatar.RequireComponent<PlayerControls>();
			this.m_movement.enabled = false;
			this.m_rigidbody = _avatar.RequireComponent<Rigidbody>();
			this.m_rigidbody.velocity = Vector3.zero;
			this.m_rigidbody.isKinematic = true;
			this.m_movement.NotifySessionInteractionStarted(this.m_interactable);
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06001BA4 RID: 7076 RVA: 0x0006FD2E File Offset: 0x0006E12E
		protected PlayerControls PlayerControls
		{
			get
			{
				return this.m_movement;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06001BA5 RID: 7077 RVA: 0x0006FD36 File Offset: 0x0006E136
		protected GameObject Avatar
		{
			get
			{
				return this.m_movement.gameObject;
			}
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x0006FD44 File Offset: 0x0006E144
		public virtual void OnSessionEnded()
		{
			if (this.m_rigidbody != null && this.Avatar.GetComponent<PlayerIDProvider>().IsLocallyControlled())
			{
				this.m_rigidbody.isKinematic = false;
			}
			if (this.m_movement != null)
			{
				this.m_movement.enabled = true;
				this.m_movement.NotifySessionInteractionEnded(this.m_interactable);
			}
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x0006FDB1 File Offset: 0x0006E1B1
		public virtual void Update()
		{
		}

		// Token: 0x040015AF RID: 5551
		private ClientSessionInteractable m_interactable;

		// Token: 0x040015B0 RID: 5552
		private PlayerControls m_movement;

		// Token: 0x040015B1 RID: 5553
		private Rigidbody m_rigidbody;

		// Token: 0x040015B2 RID: 5554
		private bool m_running = true;
	}
}
