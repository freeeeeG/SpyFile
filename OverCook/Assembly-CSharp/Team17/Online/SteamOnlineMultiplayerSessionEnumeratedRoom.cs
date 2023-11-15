using System;
using Steamworks;

namespace Team17.Online
{
	// Token: 0x02000988 RID: 2440
	public abstract class SteamOnlineMultiplayerSessionEnumeratedRoom
	{
		// Token: 0x06002FA6 RID: 12198 RVA: 0x000DBB65 File Offset: 0x000D9F65
		public virtual string GetHostName()
		{
			return string.Empty;
		}

		// Token: 0x0400263E RID: 9790
		public CSteamID m_steamLobbyId;
	}
}
