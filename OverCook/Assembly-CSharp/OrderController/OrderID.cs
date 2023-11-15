using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

namespace OrderController
{
	// Token: 0x02000756 RID: 1878
	public struct OrderID : Serialisable
	{
		// Token: 0x0600241A RID: 9242 RVA: 0x000AC380 File Offset: 0x000AA780
		public OrderID(uint _id)
		{
			this.m_id = _id;
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x000AC389 File Offset: 0x000AA789
		public static bool operator ==(OrderID _id, OrderID _other)
		{
			return _id.m_id == _other.m_id;
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x000AC39B File Offset: 0x000AA79B
		public static bool operator !=(OrderID _id, OrderID _other)
		{
			return _id.m_id != _other.m_id;
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x000AC3B0 File Offset: 0x000AA7B0
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write(this.m_id, 8);
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x000AC3BF File Offset: 0x000AA7BF
		public bool Deserialise(BitStreamReader reader)
		{
			this.m_id = reader.ReadUInt32(8);
			return true;
		}

		// Token: 0x04001B94 RID: 7060
		private const int kBitsPerID = 8;

		// Token: 0x04001B95 RID: 7061
		public uint m_id;
	}
}
