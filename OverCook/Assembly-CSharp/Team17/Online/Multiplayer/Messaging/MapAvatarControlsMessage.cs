using System;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008CD RID: 2253
	public class MapAvatarControlsMessage : Serialisable
	{
		// Token: 0x06002BC8 RID: 11208 RVA: 0x000CC714 File Offset: 0x000CAB14
		public void Serialise(BitStreamWriter writer)
		{
			for (int i = 0; i < 4; i++)
			{
				writer.Write(this.m_bHorns[i]);
			}
			writer.Write(this.m_bDash);
			writer.Write(this.CurrentSelectableEntityId, 10);
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x000CC75C File Offset: 0x000CAB5C
		public bool Deserialise(BitStreamReader reader)
		{
			for (int i = 0; i < 4; i++)
			{
				this.m_bHorns[i] = reader.ReadBit();
			}
			this.m_bDash = reader.ReadBit();
			this.CurrentSelectableEntityId = reader.ReadUInt32(10);
			return true;
		}

		// Token: 0x040022FA RID: 8954
		public bool[] m_bHorns = new bool[4];

		// Token: 0x040022FB RID: 8955
		public bool m_bDash;

		// Token: 0x040022FC RID: 8956
		public uint CurrentSelectableEntityId;
	}
}
