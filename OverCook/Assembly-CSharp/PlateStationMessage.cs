using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000550 RID: 1360
public class PlateStationMessage : Serialisable
{
	// Token: 0x060019A6 RID: 6566 RVA: 0x00080804 File Offset: 0x0007EC04
	public void Serialise(BitStreamWriter writer)
	{
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_delivered);
		entry.m_Header.Serialise(writer);
		writer.Write(this.m_success);
	}

	// Token: 0x060019A7 RID: 6567 RVA: 0x00080838 File Offset: 0x0007EC38
	public bool Deserialise(BitStreamReader reader)
	{
		bool flag = this.m_deliveredHeader.Deserialise(reader);
		if (flag)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_deliveredHeader.m_uEntityID);
			this.m_delivered = entry.m_GameObject;
			this.m_success = reader.ReadBit();
		}
		return flag;
	}

	// Token: 0x04001455 RID: 5205
	public GameObject m_delivered;

	// Token: 0x04001456 RID: 5206
	public bool m_success;

	// Token: 0x04001457 RID: 5207
	private EntityMessageHeader m_deliveredHeader = new EntityMessageHeader();
}
