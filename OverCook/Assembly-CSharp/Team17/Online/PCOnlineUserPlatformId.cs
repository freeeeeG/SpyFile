using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

namespace Team17.Online
{
	// Token: 0x02000973 RID: 2419
	public abstract class PCOnlineUserPlatformId : Serialisable
	{
		// Token: 0x06002F3C RID: 12092 RVA: 0x000DCAC4 File Offset: 0x000DAEC4
		public void Serialise(BitStreamWriter writer)
		{
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x000DCAC6 File Offset: 0x000DAEC6
		public bool Deserialise(BitStreamReader reader)
		{
			return true;
		}
	}
}
