using System;

namespace Team17.Online
{
	// Token: 0x0200093E RID: 2366
	public enum OnlineMultiplayerSessionJoinResult : byte
	{
		// Token: 0x04002549 RID: 9545
		eSuccess,
		// Token: 0x0400254A RID: 9546
		eClosed,
		// Token: 0x0400254B RID: 9547
		eFull,
		// Token: 0x0400254C RID: 9548
		eNoLongerExists,
		// Token: 0x0400254D RID: 9549
		eNoHostConnection,
		// Token: 0x0400254E RID: 9550
		eLostNetwork,
		// Token: 0x0400254F RID: 9551
		eApplicationSuspended,
		// Token: 0x04002550 RID: 9552
		eGoneOffline,
		// Token: 0x04002551 RID: 9553
		eLoggedOut,
		// Token: 0x04002552 RID: 9554
		eCodeVersionMismatch,
		// Token: 0x04002553 RID: 9555
		eGenericFailure,
		// Token: 0x04002554 RID: 9556
		eNotEnoughRoomForAllLocalUsers
	}
}
