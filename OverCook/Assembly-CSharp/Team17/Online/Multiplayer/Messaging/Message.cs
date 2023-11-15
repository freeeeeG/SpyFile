using System;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008D2 RID: 2258
	public class Message : Serialisable
	{
		// Token: 0x06002BD3 RID: 11219 RVA: 0x000CC80E File Offset: 0x000CAC0E
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write((byte)this.Type, 8);
			this.Payload.Serialise(writer);
		}

		// Token: 0x06002BD4 RID: 11220 RVA: 0x000CC82A File Offset: 0x000CAC2A
		public bool Deserialise(BitStreamReader reader)
		{
			this.Type = (MessageType)reader.ReadByte(8);
			return this.Type >= MessageType.Example && this.Type < MessageType.COUNT && SerialisationRegistry<MessageType>.Deserialise(out this.Payload, this.Type, reader);
		}

		// Token: 0x04002331 RID: 9009
		public MessageType Type;

		// Token: 0x04002332 RID: 9010
		public Serialisable Payload;
	}
}
