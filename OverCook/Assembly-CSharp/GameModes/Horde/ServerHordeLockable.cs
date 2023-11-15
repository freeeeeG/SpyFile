using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007D1 RID: 2001
	public class ServerHordeLockable : ServerSynchroniserBase
	{
		// Token: 0x06002678 RID: 9848 RVA: 0x000B73DB File Offset: 0x000B57DB
		public override EntityType GetEntityType()
		{
			return EntityType.HordeLockable;
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x000B73E0 File Offset: 0x000B57E0
		public override void StartSynchronising(Component synchronisedObject)
		{
			base.StartSynchronising(synchronisedObject);
			this.m_lockable = (HordeLockable)synchronisedObject;
			this.m_interactable = base.gameObject.RequestComponent<ServerInteractable>();
			if (this.m_interactable != null)
			{
				this.m_interactable.RegisterCallbacks(new ServerInteractable.BeginInteractCallback(this.OnBeginInteract), null);
				this.m_interactable.SetInteractionSuppressed(false);
			}
			Mailbox.Server.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x000B745E File Offset: 0x000B585E
		public override void OnDestroy()
		{
			base.OnDestroy();
			Mailbox.Server.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x000B7480 File Offset: 0x000B5880
		private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			GameStateMessage gameStateMessage = (GameStateMessage)message;
			if (gameStateMessage.m_State == GameState.StartedEntities)
			{
				FlowControllerBase flowControllerBase = GameUtils.RequireManager<FlowControllerBase>();
				this.m_flowController = flowControllerBase.gameObject.RequireComponent<ServerHordeFlowController>();
			}
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x000B74B8 File Offset: 0x000B58B8
		private void OnBeginInteract(GameObject interacter, Vector2 dir)
		{
			if (this.m_locked && this.m_flowController.SpendMoney(this.m_lockable.m_unlockCost))
			{
				this.Unlock();
			}
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x000B74E8 File Offset: 0x000B58E8
		public void Lock()
		{
			if (!this.m_locked)
			{
				this.m_locked = true;
				if (this.m_interactable != null)
				{
					this.m_interactable.SetInteractionSuppressed(false);
				}
				HordeLockableMessage.Lock(ref this.m_message);
				this.SendServerEvent(this.m_message);
				if (this.m_lockable.m_lockables != null && this.m_lockable.m_lockables.Length > 0)
				{
					for (int i = 0; i < this.m_lockable.m_lockables.Length; i++)
					{
						if (this.m_lockable.m_lockables[i] != null)
						{
							this.m_lockable.m_lockables[i].gameObject.RequireComponent<ServerHordeLockable>().Lock();
						}
					}
				}
			}
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x000B75B0 File Offset: 0x000B59B0
		public void Unlock()
		{
			if (this.m_locked)
			{
				this.m_locked = false;
				if (this.m_interactable != null)
				{
					this.m_interactable.SetInteractionSuppressed(true);
				}
				HordeLockableMessage.Unlock(ref this.m_message);
				this.SendServerEvent(this.m_message);
				if (this.m_lockable.m_lockables != null && this.m_lockable.m_lockables.Length > 0)
				{
					for (int i = 0; i < this.m_lockable.m_lockables.Length; i++)
					{
						if (this.m_lockable.m_lockables[i] != null)
						{
							this.m_lockable.m_lockables[i].gameObject.RequireComponent<ServerHordeLockable>().Unlock();
						}
					}
				}
			}
		}

		// Token: 0x04001E81 RID: 7809
		private HordeLockable m_lockable;

		// Token: 0x04001E82 RID: 7810
		private ServerInteractable m_interactable;

		// Token: 0x04001E83 RID: 7811
		private ServerHordeFlowController m_flowController;

		// Token: 0x04001E84 RID: 7812
		private HordeLockableMessage m_message = new HordeLockableMessage();

		// Token: 0x04001E85 RID: 7813
		private bool m_locked = true;
	}
}
