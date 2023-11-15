using System;

namespace Team17.Online
{
	// Token: 0x02000959 RID: 2393
	public class OnlineMultiplayerSessionPlayTogetherHosting : SteamOnlineMultiplayerSessionInvite
	{
		// Token: 0x06002EDC RID: 11996 RVA: 0x000DBBB2 File Offset: 0x000D9FB2
		public override bool WasAcceptedBy(GamepadUser localUser)
		{
			return base.WasAcceptedBy(localUser);
		}
	}
}
