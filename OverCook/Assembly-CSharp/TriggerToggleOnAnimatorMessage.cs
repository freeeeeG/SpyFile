using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000199 RID: 409
public class TriggerToggleOnAnimatorMessage : Serialisable
{
	// Token: 0x060006F5 RID: 1781 RVA: 0x0002E0F7 File Offset: 0x0002C4F7
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_value);
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x0002E105 File Offset: 0x0002C505
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_value = reader.ReadBit();
		return true;
	}

	// Token: 0x040005CD RID: 1485
	public bool m_value;
}
