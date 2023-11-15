using System;

namespace Team17.Online
{
	// Token: 0x02000934 RID: 2356
	public enum OnlineMultiplayerPrivilegeCheckResult : byte
	{
		// Token: 0x0400252D RID: 9517
		eSuccess,
		// Token: 0x0400252E RID: 9518
		eNoNetwork,
		// Token: 0x0400252F RID: 9519
		ePatchRequired,
		// Token: 0x04002530 RID: 9520
		eSystemSoftwareUpdateRequired,
		// Token: 0x04002531 RID: 9521
		eNotSignedInToPlatform,
		// Token: 0x04002532 RID: 9522
		eNoOnlineAccount,
		// Token: 0x04002533 RID: 9523
		eUnderAge,
		// Token: 0x04002534 RID: 9524
		eNoMultiplayerPrivilege,
		// Token: 0x04002535 RID: 9525
		eApplicationSuspended,
		// Token: 0x04002536 RID: 9526
		eGenericFailure
	}
}
