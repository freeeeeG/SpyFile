using System;

namespace Team17.Online
{
	// Token: 0x02000952 RID: 2386
	public class OnlineFriend : SteamOnlineFriend
	{
		// Token: 0x0400256D RID: 9581
		public string m_displayName;

		// Token: 0x0400256E RID: 9582
		public OnlineFriend.FriendStatus m_status;

		// Token: 0x02000953 RID: 2387
		public enum FriendStatus
		{
			// Token: 0x04002570 RID: 9584
			eOffline,
			// Token: 0x04002571 RID: 9585
			eOnline,
			// Token: 0x04002572 RID: 9586
			eOnlineInSameApplication,
			// Token: 0x04002573 RID: 9587
			eOnlineInSameApplicationAndJoinable
		}
	}
}
