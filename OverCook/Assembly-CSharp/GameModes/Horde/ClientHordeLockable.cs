using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007D2 RID: 2002
	public class ClientHordeLockable : ClientSynchroniserBase
	{
		// Token: 0x06002680 RID: 9856 RVA: 0x000B768B File Offset: 0x000B5A8B
		public void RegisterOnLock(object handle, GenericVoid<ClientHordeLockable> onLock)
		{
			this.m_onLock = (GenericVoid<ClientHordeLockable>)Delegate.Combine(this.m_onLock, onLock);
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x000B76A4 File Offset: 0x000B5AA4
		public void UnregisterOnLock(object handle, GenericVoid<ClientHordeLockable> onLock)
		{
			this.m_onLock = (GenericVoid<ClientHordeLockable>)Delegate.Remove(this.m_onLock, onLock);
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x000B76BD File Offset: 0x000B5ABD
		public void RegisterOnUnlock(object handle, GenericVoid<ClientHordeLockable> onUnlock)
		{
			this.m_onUnlock = (GenericVoid<ClientHordeLockable>)Delegate.Combine(this.m_onUnlock, onUnlock);
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x000B76D6 File Offset: 0x000B5AD6
		public void UnregisterOnUnlock(object handle, GenericVoid<ClientHordeLockable> onUnlock)
		{
			this.m_onUnlock = (GenericVoid<ClientHordeLockable>)Delegate.Remove(this.m_onUnlock, onUnlock);
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x000B76EF File Offset: 0x000B5AEF
		public override EntityType GetEntityType()
		{
			return EntityType.HordeLockable;
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x000B76F4 File Offset: 0x000B5AF4
		public override void StartSynchronising(Component synchronisedObject)
		{
			base.StartSynchronising(synchronisedObject);
			this.m_lockable = (HordeLockable)synchronisedObject;
			this.m_interactable = base.gameObject.RequestComponent<ClientInteractable>();
			if (this.m_interactable != null)
			{
				this.m_interactable.SetInteractionSuppressed(false);
			}
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x000B7744 File Offset: 0x000B5B44
		public override void ApplyServerEvent(Serialisable serialisable)
		{
			HordeLockableMessage hordeLockableMessage = (HordeLockableMessage)serialisable;
			if (!hordeLockableMessage.m_locked)
			{
				this.m_onUnlock(this);
				this.m_lockable.m_collider.enabled = false;
				if (this.m_interactable != null)
				{
					this.m_interactable.SetInteractionSuppressed(true);
				}
			}
			else
			{
				this.m_onLock(this);
				this.m_lockable.m_collider.enabled = true;
				if (this.m_interactable != null)
				{
					this.m_interactable.SetInteractionSuppressed(false);
				}
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06002687 RID: 9863 RVA: 0x000B77DC File Offset: 0x000B5BDC
		public int UnlockCost
		{
			get
			{
				return this.m_lockable.m_unlockCost;
			}
		}

		// Token: 0x04001E86 RID: 7814
		private HordeLockable m_lockable;

		// Token: 0x04001E87 RID: 7815
		private ClientInteractable m_interactable;

		// Token: 0x04001E88 RID: 7816
		private HordeLockableMessage m_message = new HordeLockableMessage();

		// Token: 0x04001E89 RID: 7817
		public GenericVoid<ClientHordeLockable> m_onLock;

		// Token: 0x04001E8A RID: 7818
		public GenericVoid<ClientHordeLockable> m_onUnlock;
	}
}
