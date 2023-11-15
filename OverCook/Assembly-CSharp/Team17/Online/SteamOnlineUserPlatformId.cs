using System;
using BitStream;
using Steamworks;
using Team17.Online.Multiplayer.Messaging;

namespace Team17.Online
{
	// Token: 0x02000992 RID: 2450
	public class SteamOnlineUserPlatformId : Serialisable
	{
		// Token: 0x06002FC9 RID: 12233 RVA: 0x000DC9F6 File Offset: 0x000DADF6
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write(this.m_steamId.m_SteamID, 64);
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x000DCA0B File Offset: 0x000DAE0B
		public bool Deserialise(BitStreamReader reader)
		{
			this.m_steamId = new CSteamID((ulong)reader.ReadUInt64(64));
			return true;
		}

		// Token: 0x0400265F RID: 9823
		public CSteamID m_steamId = default(CSteamID);
	}
}
