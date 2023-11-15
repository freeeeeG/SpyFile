using System;
using System.Collections.Generic;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008B0 RID: 2224
	public class DestroyEntitiesMessage : Serialisable
	{
		// Token: 0x06002B57 RID: 11095 RVA: 0x000CADB7 File Offset: 0x000C91B7
		public void Initialise(uint root, FastList<uint> ids)
		{
			this.m_rootId = root;
			this.m_ids.Clear();
			ids.CopyTo(this.m_ids._items);
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x000CADDC File Offset: 0x000C91DC
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write(this.m_rootId, 10);
			writer.Write((uint)this.m_ids.Count, 5);
			for (int i = 0; i < this.m_ids.Count; i++)
			{
				writer.Write(this.m_ids._items[i], 10);
			}
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x000CAE3C File Offset: 0x000C923C
		public bool Deserialise(BitStreamReader reader)
		{
			this.m_rootId = reader.ReadUInt32(10);
			uint num = reader.ReadUInt32(5);
			this.m_ids.Clear();
			int num2 = 0;
			while ((long)num2 < (long)((ulong)num))
			{
				uint item = reader.ReadUInt32(10);
				this.m_ids.Add(item);
				num2++;
			}
			return true;
		}

		// Token: 0x0400224E RID: 8782
		public const int k_idCountBitCount = 5;

		// Token: 0x0400224F RID: 8783
		public const int k_idCapacity = 16;

		// Token: 0x04002250 RID: 8784
		public uint m_rootId;

		// Token: 0x04002251 RID: 8785
		public FastList<uint> m_ids = new FastList<uint>(16);
	}
}
