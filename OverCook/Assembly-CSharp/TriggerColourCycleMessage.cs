using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000176 RID: 374
public class TriggerColourCycleMessage : Serialisable
{
	// Token: 0x0600068F RID: 1679 RVA: 0x0002D2E8 File Offset: 0x0002B6E8
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_colourIndex, 4);
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x0002D2F7 File Offset: 0x0002B6F7
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_colourIndex = (int)reader.ReadUInt32(4);
		return true;
	}

	// Token: 0x0400057B RID: 1403
	public int m_colourIndex;

	// Token: 0x0400057C RID: 1404
	private const int indexBitSize = 4;
}
