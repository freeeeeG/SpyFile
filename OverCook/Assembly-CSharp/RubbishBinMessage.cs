using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000563 RID: 1379
public class RubbishBinMessage : Serialisable
{
	// Token: 0x060019F3 RID: 6643 RVA: 0x0008221E File Offset: 0x0008061E
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.BinnedItemEntityID, 10);
		writer.Write(this.m_alive);
	}

	// Token: 0x060019F4 RID: 6644 RVA: 0x0008223A File Offset: 0x0008063A
	public bool Deserialise(BitStreamReader reader)
	{
		this.BinnedItemEntityID = reader.ReadUInt32(10);
		this.m_alive = reader.ReadBit();
		return true;
	}

	// Token: 0x040014A5 RID: 5285
	public uint BinnedItemEntityID;

	// Token: 0x040014A6 RID: 5286
	public bool m_alive = true;
}
