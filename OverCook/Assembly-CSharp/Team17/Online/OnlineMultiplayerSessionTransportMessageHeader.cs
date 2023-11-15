using System;
using BitStream;

namespace Team17.Online
{
	// Token: 0x02000962 RID: 2402
	public class OnlineMultiplayerSessionTransportMessageHeader
	{
		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06002EF8 RID: 12024 RVA: 0x000DBE41 File Offset: 0x000DA241
		// (set) Token: 0x06002EF9 RID: 12025 RVA: 0x000DBE49 File Offset: 0x000DA249
		public bool IsGameMessage { get; set; }

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06002EFA RID: 12026 RVA: 0x000DBE52 File Offset: 0x000DA252
		// (set) Token: 0x06002EFB RID: 12027 RVA: 0x000DBE5A File Offset: 0x000DA25A
		public byte MessageTypeId { get; set; }

		// Token: 0x06002EFC RID: 12028 RVA: 0x000DBE63 File Offset: 0x000DA263
		public void Serialize(BitStreamWriter stream)
		{
			stream.Write(this.IsGameMessage);
			stream.Write(this.MessageTypeId, 7);
		}

		// Token: 0x06002EFD RID: 12029 RVA: 0x000DBE7E File Offset: 0x000DA27E
		public void Deserialize(BitStreamReader stream)
		{
			this.IsGameMessage = stream.ReadBit();
			this.MessageTypeId = stream.ReadByte(7);
		}
	}
}
