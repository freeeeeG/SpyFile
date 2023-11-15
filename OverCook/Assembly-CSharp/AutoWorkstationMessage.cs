using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000572 RID: 1394
public class AutoWorkstationMessage : Serialisable
{
	// Token: 0x06001A45 RID: 6725 RVA: 0x00083884 File Offset: 0x00081C84
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_working);
		if (this.m_working)
		{
			writer.Write((uint)this.m_items.Length, 4);
			for (int i = 0; i < this.m_items.Length; i++)
			{
				if (this.m_items[i] != null)
				{
					this.m_items[i].m_Header.Serialise(writer);
				}
				else
				{
					this.m_itemHeader.Serialise(writer);
				}
			}
		}
	}

	// Token: 0x06001A46 RID: 6726 RVA: 0x00083904 File Offset: 0x00081D04
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_working = reader.ReadBit();
		if (this.m_working)
		{
			int num = (int)reader.ReadUInt32(4);
			Array.Resize<EntitySerialisationEntry>(ref this.m_items, num);
			for (int i = 0; i < num; i++)
			{
				bool flag = this.m_itemHeader.Deserialise(reader);
				if (flag)
				{
					if (this.m_itemHeader.m_uEntityID != 0U)
					{
						this.m_items[i] = EntitySerialisationRegistry.GetEntry(this.m_itemHeader.m_uEntityID);
					}
					else
					{
						this.m_items[i] = null;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x040014D0 RID: 5328
	private const int kBitsPerItemCount = 4;

	// Token: 0x040014D1 RID: 5329
	public EntitySerialisationEntry[] m_items = new EntitySerialisationEntry[0];

	// Token: 0x040014D2 RID: 5330
	public bool m_working;

	// Token: 0x040014D3 RID: 5331
	public EntityMessageHeader m_itemHeader = new EntityMessageHeader();
}
