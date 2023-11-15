using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000522 RID: 1314
public class MixingStationMessage : Serialisable
{
	// Token: 0x0600188C RID: 6284 RVA: 0x0007CAF9 File Offset: 0x0007AEF9
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_isTurnedOn);
	}

	// Token: 0x0600188D RID: 6285 RVA: 0x0007CB07 File Offset: 0x0007AF07
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_isTurnedOn = reader.ReadBit();
		return true;
	}

	// Token: 0x040013B9 RID: 5049
	public bool m_isTurnedOn;
}
