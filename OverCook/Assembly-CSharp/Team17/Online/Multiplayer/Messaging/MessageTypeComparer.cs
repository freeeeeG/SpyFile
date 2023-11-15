using System;
using System.Collections.Generic;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008D0 RID: 2256
	public class MessageTypeComparer : IEqualityComparer<MessageType>
	{
		// Token: 0x06002BCB RID: 11211 RVA: 0x000CC7AC File Offset: 0x000CABAC
		public bool Equals(MessageType x, MessageType y)
		{
			return x == y;
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x000CC7B2 File Offset: 0x000CABB2
		public int GetHashCode(MessageType obj)
		{
			return (int)obj;
		}
	}
}
