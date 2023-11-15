using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000532 RID: 1330
public class PilotRotationMessage : Serialisable
{
	// Token: 0x060018E6 RID: 6374 RVA: 0x0007E5ED File Offset: 0x0007C9ED
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_angle);
	}

	// Token: 0x060018E7 RID: 6375 RVA: 0x0007E5FB File Offset: 0x0007C9FB
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_angle = reader.ReadFloat32();
		return true;
	}

	// Token: 0x04001403 RID: 5123
	public float m_angle;
}
