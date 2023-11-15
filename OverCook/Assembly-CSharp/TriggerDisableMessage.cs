using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000180 RID: 384
public class TriggerDisableMessage : Serialisable
{
	// Token: 0x060006AA RID: 1706 RVA: 0x0002D683 File Offset: 0x0002BA83
	public void Initialise(bool _enabled)
	{
		this.m_enabled = _enabled;
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x0002D68C File Offset: 0x0002BA8C
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_enabled);
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x0002D69A File Offset: 0x0002BA9A
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_enabled = reader.ReadBit();
		return true;
	}

	// Token: 0x0400058E RID: 1422
	public bool m_enabled;
}
