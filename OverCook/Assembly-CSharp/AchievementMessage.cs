using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008A0 RID: 2208
public class AchievementMessage : Serialisable
{
	// Token: 0x06002B12 RID: 11026 RVA: 0x000CA098 File Offset: 0x000C8498
	public void Initialise(EntityMessageHeader header, int statId, int increment = 1)
	{
		this.m_Header = header;
		this.m_statId = statId;
		this.m_increment = increment;
	}

	// Token: 0x06002B13 RID: 11027 RVA: 0x000CA0AF File Offset: 0x000C84AF
	public void Serialise(BitStreamWriter writer)
	{
		this.m_Header.Serialise(writer);
		writer.Write((uint)this.m_statId, 10);
		writer.Write((uint)this.m_increment, 3);
	}

	// Token: 0x06002B14 RID: 11028 RVA: 0x000CA0D8 File Offset: 0x000C84D8
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_Header.Deserialise(reader);
		this.m_statId = (int)reader.ReadUInt32(10);
		this.m_increment = (int)reader.ReadUInt32(3);
		return true;
	}

	// Token: 0x04002206 RID: 8710
	public EntityMessageHeader m_Header = new EntityMessageHeader();

	// Token: 0x04002207 RID: 8711
	public int m_statId = -1;

	// Token: 0x04002208 RID: 8712
	public int m_increment = 1;

	// Token: 0x04002209 RID: 8713
	private const int kBitsPerAchievementType = 10;

	// Token: 0x0400220A RID: 8714
	private const int kBitsPerAchievementIncrement = 3;
}
