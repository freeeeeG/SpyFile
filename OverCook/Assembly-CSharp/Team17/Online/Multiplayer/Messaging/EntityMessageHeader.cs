using System;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008B4 RID: 2228
	public class EntityMessageHeader : Serialisable
	{
		// Token: 0x06002B62 RID: 11106 RVA: 0x000CAF6C File Offset: 0x000C936C
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write(this.m_uEntityID, 10);
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x000CAF7C File Offset: 0x000C937C
		public bool Deserialise(BitStreamReader reader)
		{
			this.m_uEntityID = reader.ReadUInt32(10);
			return true;
		}

		// Token: 0x04002298 RID: 8856
		public uint m_uEntityID;
	}
}
