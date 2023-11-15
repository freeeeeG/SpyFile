using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200052D RID: 1325
public class PickupItemSwitcherMessage : Serialisable
{
	// Token: 0x060018D4 RID: 6356 RVA: 0x0007E0D2 File Offset: 0x0007C4D2
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_itemIndex, 4);
	}

	// Token: 0x060018D5 RID: 6357 RVA: 0x0007E0E1 File Offset: 0x0007C4E1
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_itemIndex = (int)reader.ReadUInt32(4);
		return true;
	}

	// Token: 0x040013F3 RID: 5107
	public int m_itemIndex;

	// Token: 0x040013F4 RID: 5108
	private const int indexBitSize = 4;
}
