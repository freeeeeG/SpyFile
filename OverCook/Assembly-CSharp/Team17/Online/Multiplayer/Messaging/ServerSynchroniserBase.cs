using System;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000927 RID: 2343
	public class ServerSynchroniserBase : SynchroniserBase, ServerSynchroniser, Synchroniser
	{
		// Token: 0x06002E17 RID: 11799 RVA: 0x00027B0C File Offset: 0x00025F0C
		public override void StartSynchronising(Component synchronisedObject)
		{
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x00027B0E File Offset: 0x00025F0E
		public override void UpdateSynchronising()
		{
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x00027B10 File Offset: 0x00025F10
		public override EntityType GetEntityType()
		{
			return EntityType.Unknown;
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x00027B13 File Offset: 0x00025F13
		public void Initialise(uint uEntityId, uint uComponentId)
		{
			this.m_uEntityId = uEntityId;
			this.m_uComponentId = uComponentId;
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x00027B23 File Offset: 0x00025F23
		public virtual Serialisable GetServerUpdate()
		{
			return null;
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x00027B26 File Offset: 0x00025F26
		public virtual bool HasTargetedServerUpdates()
		{
			return false;
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x00027B29 File Offset: 0x00025F29
		public virtual Serialisable GetServerUpdateForRecipient(IOnlineMultiplayerSessionUserId recipient)
		{
			return null;
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x00027B2C File Offset: 0x00025F2C
		public uint GetEntityId()
		{
			return this.m_uEntityId;
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x00027B34 File Offset: 0x00025F34
		public uint GetComponentId()
		{
			return this.m_uComponentId;
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x00027B3C File Offset: 0x00025F3C
		public virtual void SendServerEvent(Serialisable message)
		{
			if (MultiplayerController.IsSynchronisationActive())
			{
				ServerMessenger.EntityEvent(this, message);
			}
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x00027B55 File Offset: 0x00025F55
		public virtual void SendServerEventToRecipient(IOnlineMultiplayerSessionUserId recipient, Serialisable message)
		{
			if (MultiplayerController.IsSynchronisationActive())
			{
				ServerMessenger.EntityEvent(recipient, this, message);
			}
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x00027B6F File Offset: 0x00025F6F
		public virtual void OnDestroy()
		{
			this.StopSynchronising();
			EntitySerialisationRegistry.UnregisterObject(base.gameObject);
		}

		// Token: 0x04002510 RID: 9488
		private uint m_uEntityId;

		// Token: 0x04002511 RID: 9489
		private uint m_uComponentId;
	}
}
