using System;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008AF RID: 2223
	public class DestroyEntityMessage : Serialisable
	{
		// Token: 0x06002B53 RID: 11091 RVA: 0x000CAD7D File Offset: 0x000C917D
		public void Initialise(EntityMessageHeader header)
		{
			this.m_Header = header;
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x000CAD86 File Offset: 0x000C9186
		public void Serialise(BitStreamWriter writer)
		{
			this.m_Header.Serialise(writer);
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x000CAD94 File Offset: 0x000C9194
		public bool Deserialise(BitStreamReader reader)
		{
			return this.m_Header.Deserialise(reader);
		}

		// Token: 0x0400224D RID: 8781
		public EntityMessageHeader m_Header = new EntityMessageHeader();
	}
}
