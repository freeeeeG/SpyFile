using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000582 RID: 1410
public class FlammableMessage : Serialisable
{
	// Token: 0x06001AAE RID: 6830 RVA: 0x00085A5E File Offset: 0x00083E5E
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_playerExtinguished);
		writer.Write(this.m_onFire);
		writer.Write(this.m_fireStrength);
		writer.Write(this.m_fireStrengthVelocity);
	}

	// Token: 0x06001AAF RID: 6831 RVA: 0x00085A90 File Offset: 0x00083E90
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_playerExtinguished = reader.ReadBit();
		this.m_onFire = reader.ReadBit();
		this.m_fireStrength = reader.ReadFloat32();
		this.m_fireStrengthVelocity = reader.ReadFloat32();
		return true;
	}

	// Token: 0x04001523 RID: 5411
	public bool m_playerExtinguished;

	// Token: 0x04001524 RID: 5412
	public bool m_onFire;

	// Token: 0x04001525 RID: 5413
	public float m_fireStrength;

	// Token: 0x04001526 RID: 5414
	public float m_fireStrengthVelocity;
}
