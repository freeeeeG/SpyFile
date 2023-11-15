using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200081B RID: 2075
public class WorkableMessage : Serialisable
{
	// Token: 0x060027D1 RID: 10193 RVA: 0x000BAC8C File Offset: 0x000B908C
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_onWorkstation);
		writer.Write((uint)this.m_progress, 4);
		writer.Write((uint)this.m_subProgress, 4);
	}

	// Token: 0x060027D2 RID: 10194 RVA: 0x000BACB4 File Offset: 0x000B90B4
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_onWorkstation = reader.ReadBit();
		this.m_progress = (int)reader.ReadUInt32(4);
		this.m_subProgress = (int)reader.ReadUInt32(4);
		return true;
	}

	// Token: 0x04001F4E RID: 8014
	public bool m_onWorkstation;

	// Token: 0x04001F4F RID: 8015
	public int m_progress;

	// Token: 0x04001F50 RID: 8016
	public int m_subProgress;
}
