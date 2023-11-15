using System;
using Team17.Online;

// Token: 0x0200083D RID: 2109
public class AcceptInviteData
{
	// Token: 0x0400201E RID: 8222
	public OnlineMultiplayerSessionInvite Invite;

	// Token: 0x0400201F RID: 8223
	public GamepadUser User;

	// Token: 0x04002020 RID: 8224
	public bool FromIIS;

	// Token: 0x04002021 RID: 8225
	public AcceptInviteData.LocalUsersChoice JoinLocalUsersChoice;

	// Token: 0x0200083E RID: 2110
	public enum LocalUsersChoice
	{
		// Token: 0x04002023 RID: 8227
		eNotChosenYet,
		// Token: 0x04002024 RID: 8228
		ePrimary,
		// Token: 0x04002025 RID: 8229
		eAll
	}
}
