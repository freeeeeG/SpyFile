using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200066A RID: 1642
public class DynamicLevelMessage : Serialisable
{
	// Token: 0x06001F50 RID: 8016 RVA: 0x00098DC7 File Offset: 0x000971C7
	public void Initialise(int _phase)
	{
		this.m_phase = _phase;
	}

	// Token: 0x06001F51 RID: 8017 RVA: 0x00098DD0 File Offset: 0x000971D0
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_phase, 8);
	}

	// Token: 0x06001F52 RID: 8018 RVA: 0x00098DDF File Offset: 0x000971DF
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_phase = (int)reader.ReadUInt32(8);
		return true;
	}

	// Token: 0x040017E6 RID: 6118
	public const int kBitsPerPhaseNumber = 8;

	// Token: 0x040017E7 RID: 6119
	public int m_phase;
}
