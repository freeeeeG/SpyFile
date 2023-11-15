using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020001A0 RID: 416
public class TriggerZoneMessage : Serialisable
{
	// Token: 0x0600070A RID: 1802 RVA: 0x0002E3F2 File Offset: 0x0002C7F2
	public void Initialise(bool _occupied)
	{
		this.m_occupied = _occupied;
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x0002E3FB File Offset: 0x0002C7FB
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_occupied);
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x0002E409 File Offset: 0x0002C809
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_occupied = reader.ReadBit();
		return true;
	}

	// Token: 0x040005DC RID: 1500
	public bool m_occupied;
}
