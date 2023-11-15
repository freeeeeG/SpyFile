using System;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008CC RID: 2252
	public class MapAvatarHornMessage : Serialisable
	{
		// Token: 0x06002BC5 RID: 11205 RVA: 0x000CC6DF File Offset: 0x000CAADF
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write((uint)this.m_playerIdx, 3);
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x000CC6EE File Offset: 0x000CAAEE
		public bool Deserialise(BitStreamReader reader)
		{
			this.m_playerIdx = (int)reader.ReadUInt32(3);
			return true;
		}

		// Token: 0x040022F9 RID: 8953
		public int m_playerIdx;
	}
}
