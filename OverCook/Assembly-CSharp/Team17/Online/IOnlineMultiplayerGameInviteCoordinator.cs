using System;

namespace Team17.Online
{
	// Token: 0x02000932 RID: 2354
	public interface IOnlineMultiplayerGameInviteCoordinator
	{
		// Token: 0x06002E6E RID: 11886
		OnlineMultiplayerSessionInvite InviteAccepted();

		// Token: 0x06002E6F RID: 11887
		bool HasPendingAcceptedInvite();

		// Token: 0x06002E70 RID: 11888
		OnlineMultiplayerSessionPlayTogetherHosting PlayTogetherHosting();
	}
}
