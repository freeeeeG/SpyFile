using System;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000909 RID: 2313
	public class ServerChefSynchroniser : ServerWorldObjectSynchroniser
	{
		// Token: 0x06002D34 RID: 11572 RVA: 0x000D5DBD File Offset: 0x000D41BD
		public override void StartSynchronising(Component synchronisedObject)
		{
			this.m_Rigidbody = base.GetComponent<Rigidbody>();
			this.m_ChefData.WorldObject = base.GetMessageData();
			this.m_InputReceiver = base.GetComponent<ServerInputReceiver>();
			base.StartSynchronising(synchronisedObject);
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x000D5DF0 File Offset: 0x000D41F0
		public override void Awake()
		{
			IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
			this.m_SessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
			base.Awake();
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x000D5E15 File Offset: 0x000D4215
		public override EntityType GetEntityType()
		{
			return EntityType.Chef;
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x000D5E18 File Offset: 0x000D4218
		public override Serialisable GetServerUpdate()
		{
			this.m_bSleepAllowed = false;
			base.GetServerUpdate();
			return null;
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x000D5E2C File Offset: 0x000D422C
		public override void SendServerEvent(Serialisable message)
		{
			if (this.m_SessionCoordinator != null)
			{
				IOnlineMultiplayerSessionUserId[] array = this.m_SessionCoordinator.Members();
				if (array != null)
				{
					foreach (IOnlineMultiplayerSessionUserId recipient in array)
					{
						Serialisable serialisable = this.BuildMessageForRecipient(recipient, true);
						if (serialisable != null)
						{
							this.SendServerEventToRecipient(recipient, serialisable);
						}
					}
				}
			}
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x000D5E85 File Offset: 0x000D4285
		public override bool HasTargetedServerUpdates()
		{
			return true;
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x000D5E88 File Offset: 0x000D4288
		public Serialisable BuildMessageForRecipient(IOnlineMultiplayerSessionUserId recipient, bool bForce = false)
		{
			if (recipient == null)
			{
				return null;
			}
			if (!this.m_bStartedSynchronising)
			{
				return null;
			}
			if (null == this.m_Transform)
			{
				return null;
			}
			if (this.m_bSleepAllowed && !this.m_bOwnerCalculated)
			{
				return null;
			}
			if (bForce || base.IsWorldObjectActive() || this.m_Rigidbody.velocity != this.m_ChefData.Velocity)
			{
				this.m_ChefData.Velocity = this.m_Rigidbody.velocity;
				this.m_ChefData.NetworkTime = ClientTime.Time();
				this.m_ChefData.ClientTimeStamp = this.m_InputReceiver.GetLastClientInputTimeStamp();
				return this.m_ChefData;
			}
			return null;
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x000D5F4A File Offset: 0x000D434A
		public override Serialisable GetServerUpdateForRecipient(IOnlineMultiplayerSessionUserId recipient)
		{
			return this.BuildMessageForRecipient(recipient, false);
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x000D5F54 File Offset: 0x000D4354
		protected override void OnGameStateChanged(GameState state, GameStateMessage.GameStatePayload payload)
		{
			base.OnGameStateChanged(state, payload);
			if (state == GameState.StartEntities)
			{
				for (int i = 0; i < ServerUserSystem.m_Users.Count; i++)
				{
					User user = ServerUserSystem.m_Users._items[i];
					if (user.EntityID == base.GetEntityId() || user.Entity2ID == base.GetEntityId())
					{
						this.m_Owner = user.SessionId;
						break;
					}
				}
				this.m_bOwnerCalculated = true;
			}
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000D5FD4 File Offset: 0x000D43D4
		protected override bool SendResumeData(IOnlineMultiplayerSessionUserId sessionUserId)
		{
			base.RefreshParent();
			this.m_ChefData.WorldObject.LocalPosition = this.m_Transform.localPosition;
			this.m_ChefData.WorldObject.LocalRotation = this.m_Transform.localRotation;
			this.m_ChefData.Velocity = this.m_Rigidbody.velocity;
			this.m_ChefData.NetworkTime = ClientTime.Time();
			this.m_ChefData.ClientTimeStamp = this.m_InputReceiver.GetLastClientInputTimeStamp();
			ServerMessenger.ResumeChefPositionSync(base.gameObject, this.m_ChefData, sessionUserId);
			return true;
		}

		// Token: 0x0400243C RID: 9276
		private Rigidbody m_Rigidbody;

		// Token: 0x0400243D RID: 9277
		private ChefPositionMessage m_ChefData = new ChefPositionMessage();

		// Token: 0x0400243E RID: 9278
		private ServerInputReceiver m_InputReceiver;

		// Token: 0x0400243F RID: 9279
		private IOnlineMultiplayerSessionUserId m_Owner;

		// Token: 0x04002440 RID: 9280
		private bool m_bOwnerCalculated;

		// Token: 0x04002441 RID: 9281
		private IOnlineMultiplayerSessionCoordinator m_SessionCoordinator;
	}
}
