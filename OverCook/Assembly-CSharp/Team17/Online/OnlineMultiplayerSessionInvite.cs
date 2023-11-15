using System;

namespace Team17.Online
{
	// Token: 0x02000958 RID: 2392
	public class OnlineMultiplayerSessionInvite : SteamOnlineMultiplayerSessionInvite
	{
		// Token: 0x06002EDA RID: 11994 RVA: 0x000DBBA1 File Offset: 0x000D9FA1
		public override bool WasAcceptedBy(GamepadUser localUser)
		{
			return base.WasAcceptedBy(localUser);
		}
	}
}
