using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008B6 RID: 2230
internal class ExampleNetworkMessage : Serialisable
{
	// Token: 0x06002B69 RID: 11113 RVA: 0x000CB0E0 File Offset: 0x000C94E0
	public void Initialise(float fFloat, bool bBool)
	{
		this.m_ExampleFloat = fFloat;
		this.m_ExampleBool = bBool;
	}

	// Token: 0x06002B6A RID: 11114 RVA: 0x000CB0F0 File Offset: 0x000C94F0
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_ExampleFloat);
		writer.Write(this.m_ExampleBool);
	}

	// Token: 0x06002B6B RID: 11115 RVA: 0x000CB10A File Offset: 0x000C950A
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_ExampleFloat = reader.ReadFloat32();
		this.m_ExampleBool = reader.ReadBit();
		return true;
	}

	// Token: 0x0400229B RID: 8859
	public float m_ExampleFloat;

	// Token: 0x0400229C RID: 8860
	public bool m_ExampleBool;
}
