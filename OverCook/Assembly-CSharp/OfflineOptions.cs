using System;
using Team17.Online;

// Token: 0x02000849 RID: 2121
public struct OfflineOptions
{
	// Token: 0x04002057 RID: 8279
	public GamepadUser hostUser;

	// Token: 0x04002058 RID: 8280
	public GameMode searchGameMode;

	// Token: 0x04002059 RID: 8281
	public OfflineOptions.AdditionalAction eAdditionalAction;

	// Token: 0x0400205A RID: 8282
	public OnlineMultiplayerConnectionMode? connectionMode;

	// Token: 0x0200084A RID: 2122
	public enum AdditionalAction
	{
		// Token: 0x0400205C RID: 8284
		None,
		// Token: 0x0400205D RID: 8285
		PrivilegeCheckAllUsers,
		// Token: 0x0400205E RID: 8286
		PrivilegeCheckAllUsersAndSearchForGames
	}
}
