using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008E1 RID: 2273
public class SprayingUtensilMessage : Serialisable
{
	// Token: 0x06002C25 RID: 11301 RVA: 0x000CDA6A File Offset: 0x000CBE6A
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_bSpraying);
		if (this.m_bSpraying)
		{
			writer.Write(this.m_Carrier, 10);
		}
	}

	// Token: 0x06002C26 RID: 11302 RVA: 0x000CDA91 File Offset: 0x000CBE91
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_bSpraying = reader.ReadBit();
		if (this.m_bSpraying)
		{
			this.m_Carrier = reader.ReadUInt32(10);
		}
		return true;
	}

	// Token: 0x04002374 RID: 9076
	public bool m_bSpraying;

	// Token: 0x04002375 RID: 9077
	public uint m_Carrier;
}
