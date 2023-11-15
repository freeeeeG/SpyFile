using System;

namespace Team17.Online
{
	// Token: 0x0200093D RID: 2365
	public enum OnlineMultiplayerSessionCreateResult : byte
	{
		// Token: 0x04002542 RID: 9538
		eSuccess,
		// Token: 0x04002543 RID: 9539
		eGenericFailure,
		// Token: 0x04002544 RID: 9540
		eLostNetwork,
		// Token: 0x04002545 RID: 9541
		eApplicationSuspended,
		// Token: 0x04002546 RID: 9542
		eGoneOffline,
		// Token: 0x04002547 RID: 9543
		eLoggedOut
	}
}
