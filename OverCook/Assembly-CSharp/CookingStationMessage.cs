using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000460 RID: 1120
public class CookingStationMessage : Serialisable
{
	// Token: 0x060014C6 RID: 5318 RVA: 0x000718F5 File Offset: 0x0006FCF5
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_isTurnedOn);
		writer.Write(this.m_isCooking);
	}

	// Token: 0x060014C7 RID: 5319 RVA: 0x0007190F File Offset: 0x0006FD0F
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_isTurnedOn = reader.ReadBit();
		this.m_isCooking = reader.ReadBit();
		return true;
	}

	// Token: 0x04001002 RID: 4098
	public bool m_isTurnedOn;

	// Token: 0x04001003 RID: 4099
	public bool m_isCooking;
}
