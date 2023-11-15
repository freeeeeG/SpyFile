using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000578 RID: 1400
public class ConveyorStationMessage : Serialisable
{
	// Token: 0x06001A74 RID: 6772 RVA: 0x00084A08 File Offset: 0x00082E08
	public void Serialise(BitStreamWriter writer)
	{
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_receiverEntityID);
		entry.m_Header.Serialise(writer);
		entry = EntitySerialisationRegistry.GetEntry(this.m_itemEntityID);
		entry.m_Header.Serialise(writer);
		writer.Write(this.m_arriveTime);
	}

	// Token: 0x06001A75 RID: 6773 RVA: 0x00084A54 File Offset: 0x00082E54
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_receiverHeader.Deserialise(reader);
		this.m_receiverEntityID = this.m_receiverHeader.m_uEntityID;
		this.m_receiverItem.Deserialise(reader);
		this.m_itemEntityID = this.m_receiverItem.m_uEntityID;
		this.m_arriveTime = reader.ReadFloat32();
		return true;
	}

	// Token: 0x040014ED RID: 5357
	public uint m_receiverEntityID;

	// Token: 0x040014EE RID: 5358
	public uint m_itemEntityID;

	// Token: 0x040014EF RID: 5359
	public float m_arriveTime;

	// Token: 0x040014F0 RID: 5360
	private EntityMessageHeader m_receiverHeader = new EntityMessageHeader();

	// Token: 0x040014F1 RID: 5361
	private EntityMessageHeader m_receiverItem = new EntityMessageHeader();
}
