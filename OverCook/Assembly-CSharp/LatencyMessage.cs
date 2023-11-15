using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008C4 RID: 2244
public class LatencyMessage : Serialisable
{
	// Token: 0x06002BA0 RID: 11168 RVA: 0x000CBF27 File Offset: 0x000CA327
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_Stage, 1);
		writer.Write(this.m_bReliable);
		writer.Write(this.m_fTime);
	}

	// Token: 0x06002BA1 RID: 11169 RVA: 0x000CBF4E File Offset: 0x000CA34E
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_Stage = (LatencyMessage.Stage)reader.ReadUInt32(1);
		this.m_bReliable = reader.ReadBit();
		this.m_fTime = reader.ReadFloat32();
		return true;
	}

	// Token: 0x040022DD RID: 8925
	public LatencyMessage.Stage m_Stage;

	// Token: 0x040022DE RID: 8926
	public bool m_bReliable;

	// Token: 0x040022DF RID: 8927
	public float m_fTime;

	// Token: 0x020008C5 RID: 2245
	public enum Stage
	{
		// Token: 0x040022E1 RID: 8929
		Ping,
		// Token: 0x040022E2 RID: 8930
		Pong
	}
}
