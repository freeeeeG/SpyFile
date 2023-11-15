using System;
using Steamworks;

namespace Team17.Online
{
	// Token: 0x02000989 RID: 2441
	public abstract class SteamOnlineMultiplayerSessionInvite
	{
		// Token: 0x06002FA8 RID: 12200 RVA: 0x000DBB84 File Offset: 0x000D9F84
		public virtual bool WasAcceptedBy(GamepadUser localUser)
		{
			return null != localUser;
		}

		// Token: 0x0400263F RID: 9791
		public CSteamID m_steamLobbyId;
	}
}
