using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200042F RID: 1071
public class AttachStationMessage : Serialisable
{
	// Token: 0x0600136A RID: 4970 RVA: 0x0006C44C File Offset: 0x0006A84C
	public void Serialise(BitStreamWriter writer)
	{
		EntitySerialisationEntry entitySerialisationEntry = null;
		if (this.m_item != null)
		{
			entitySerialisationEntry = EntitySerialisationRegistry.GetEntry(this.m_item);
		}
		bool flag = null != entitySerialisationEntry;
		writer.Write(flag);
		if (flag)
		{
			entitySerialisationEntry.m_Header.Serialise(writer);
		}
	}

	// Token: 0x0600136B RID: 4971 RVA: 0x0006C49C File Offset: 0x0006A89C
	public bool Deserialise(BitStreamReader reader)
	{
		bool flag = reader.ReadBit();
		if (flag)
		{
			this.m_itemHeader.Deserialise(reader);
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_itemHeader.m_uEntityID);
			if (entry != null)
			{
				this.m_item = entry.m_GameObject;
			}
			else
			{
				this.m_item = null;
			}
		}
		else
		{
			this.m_item = null;
		}
		return true;
	}

	// Token: 0x04000F49 RID: 3913
	public GameObject m_item;

	// Token: 0x04000F4A RID: 3914
	private EntityMessageHeader m_itemHeader = new EntityMessageHeader();
}
