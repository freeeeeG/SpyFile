using System;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000914 RID: 2324
	public class EntitySerialisationEntry
	{
		// Token: 0x06002D81 RID: 11649 RVA: 0x000D7DDD File Offset: 0x000D61DD
		public bool HasUrgentUpdate()
		{
			return this.m_bUrgentUpdate;
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x000D7DE5 File Offset: 0x000D61E5
		public void SetRequiresUrgentUpdate(bool bUpdate)
		{
			this.m_bUrgentUpdate = bUpdate;
			if (!EntitySerialisationRegistry.HasUrgentOutgoingUpdates && this.m_bUrgentUpdate)
			{
				EntitySerialisationRegistry.HasUrgentOutgoingUpdates = true;
			}
		}

		// Token: 0x04002486 RID: 9350
		public int m_iLevelDataID;

		// Token: 0x04002487 RID: 9351
		public GameObject m_GameObject;

		// Token: 0x04002488 RID: 9352
		public EntityMessageHeader m_Header = new EntityMessageHeader();

		// Token: 0x04002489 RID: 9353
		public FastList<ServerSynchroniser> m_ServerSynchronisedComponents = new FastList<ServerSynchroniser>();

		// Token: 0x0400248A RID: 9354
		public FastList<ClientSynchroniser> m_ClientSynchronisedComponents = new FastList<ClientSynchroniser>();

		// Token: 0x0400248B RID: 9355
		private bool m_bUrgentUpdate;
	}
}
