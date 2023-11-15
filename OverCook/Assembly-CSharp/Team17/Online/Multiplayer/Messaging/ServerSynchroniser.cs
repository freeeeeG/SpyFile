using System;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000926 RID: 2342
	public interface ServerSynchroniser : Synchroniser
	{
		// Token: 0x06002E12 RID: 11794
		void Initialise(uint uEntityId, uint uComponentId);

		// Token: 0x06002E13 RID: 11795
		Serialisable GetServerUpdate();

		// Token: 0x06002E14 RID: 11796
		bool HasTargetedServerUpdates();

		// Token: 0x06002E15 RID: 11797
		Serialisable GetServerUpdateForRecipient(IOnlineMultiplayerSessionUserId recipient);
	}
}
