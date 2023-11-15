using System;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008DA RID: 2266
	public interface Serialisable
	{
		// Token: 0x06002BEA RID: 11242
		void Serialise(BitStreamWriter writer);

		// Token: 0x06002BEB RID: 11243
		bool Deserialise(BitStreamReader reader);
	}
}
