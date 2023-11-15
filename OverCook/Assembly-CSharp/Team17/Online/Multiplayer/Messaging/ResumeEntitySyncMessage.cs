using System;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008D8 RID: 2264
	public class ResumeEntitySyncMessage : Serialisable
	{
		// Token: 0x06002BE3 RID: 11235 RVA: 0x000CCC26 File Offset: 0x000CB026
		public void Initialise(EntityMessageHeader header)
		{
			this.m_header = header;
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x000CCC2F File Offset: 0x000CB02F
		public void Serialise(BitStreamWriter writer)
		{
			this.m_header.Serialise(writer);
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x000CCC3D File Offset: 0x000CB03D
		public bool Deserialise(BitStreamReader reader)
		{
			return this.m_header.Deserialise(reader);
		}

		// Token: 0x04002345 RID: 9029
		public EntityMessageHeader m_header = new EntityMessageHeader();
	}
}
