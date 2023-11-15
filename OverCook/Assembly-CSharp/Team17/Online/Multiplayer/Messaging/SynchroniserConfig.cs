using System;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000916 RID: 2326
	public class SynchroniserConfig
	{
		// Token: 0x06002D83 RID: 11651 RVA: 0x000D7E09 File Offset: 0x000D6209
		public SynchroniserConfig(InstancesPerGameObject instancesAllowed, Type serverSynchroniserType, Type clientSynchroniserType)
		{
			this.m_InstancesAllowed = instancesAllowed;
			this.m_ServerSynchroniserType = serverSynchroniserType;
			this.m_ClientSynchroniserType = clientSynchroniserType;
		}

		// Token: 0x0400248F RID: 9359
		public InstancesPerGameObject m_InstancesAllowed;

		// Token: 0x04002490 RID: 9360
		public Type m_ServerSynchroniserType;

		// Token: 0x04002491 RID: 9361
		public Type m_ClientSynchroniserType;
	}
}
