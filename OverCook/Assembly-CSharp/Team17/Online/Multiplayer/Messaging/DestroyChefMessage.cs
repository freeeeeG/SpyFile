using System;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008AE RID: 2222
	public class DestroyChefMessage : Serialisable
	{
		// Token: 0x06002B4F RID: 11087 RVA: 0x000CAD40 File Offset: 0x000C9140
		public void Initialise(EntityMessageHeader chef)
		{
			this.m_Chef.Initialise(chef);
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x000CAD4E File Offset: 0x000C914E
		public void Serialise(BitStreamWriter writer)
		{
			this.m_Chef.Serialise(writer);
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x000CAD5C File Offset: 0x000C915C
		public bool Deserialise(BitStreamReader reader)
		{
			return this.m_Chef.Deserialise(reader);
		}

		// Token: 0x0400224C RID: 8780
		public DestroyEntityMessage m_Chef = new DestroyEntityMessage();
	}
}
