using System;
using System.Collections.Generic;

namespace Team17.Online
{
	// Token: 0x02000944 RID: 2372
	// (Invoke) Token: 0x06002E8D RID: 11917
	public delegate OnlineMultiplayerSessionJoinResult OnlineMultiplayerSessionJoinDecisionCallback(IOnlineMultiplayerSessionUserId remotePrimaryUserId, List<OnlineMultiplayerSessionJoinRemoteUserData> remoteUserData, out byte[] replyData, out int replyDataSize);
}
