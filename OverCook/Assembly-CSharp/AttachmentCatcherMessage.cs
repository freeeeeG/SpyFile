using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020009CA RID: 2506
public class AttachmentCatcherMessage : Serialisable
{
	// Token: 0x0600311B RID: 12571 RVA: 0x000E6699 File Offset: 0x000E4A99
	public void Initialise(GameObject _object)
	{
		this.m_object = _object;
		this.m_hasObject = (_object != null);
	}

	// Token: 0x0600311C RID: 12572 RVA: 0x000E66B0 File Offset: 0x000E4AB0
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_hasObject);
		if (this.m_hasObject)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_object);
			entry.m_Header.Serialise(writer);
		}
	}

	// Token: 0x0600311D RID: 12573 RVA: 0x000E66EC File Offset: 0x000E4AEC
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_hasObject = reader.ReadBit();
		if (this.m_hasObject)
		{
			this.m_entityHeader.Deserialise(reader);
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_entityHeader.m_uEntityID);
			if (entry == null)
			{
				return false;
			}
			this.m_object = entry.m_GameObject;
		}
		else
		{
			this.m_object = null;
		}
		return true;
	}

	// Token: 0x0400275C RID: 10076
	public GameObject m_object;

	// Token: 0x0400275D RID: 10077
	public bool m_hasObject;

	// Token: 0x0400275E RID: 10078
	private EntityMessageHeader m_entityHeader = new EntityMessageHeader();
}
