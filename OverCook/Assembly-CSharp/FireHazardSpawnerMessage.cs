using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000495 RID: 1173
public class FireHazardSpawnerMessage : Serialisable
{
	// Token: 0x060015F4 RID: 5620 RVA: 0x00075336 File Offset: 0x00073736
	public void Serialise(BitStreamWriter writer)
	{
		this.m_parentEntry.m_Header.Serialise(writer);
		this.m_spawnedEntry.m_Header.Serialise(writer);
	}

	// Token: 0x060015F5 RID: 5621 RVA: 0x0007535A File Offset: 0x0007375A
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_parentEntry.m_Header.Deserialise(reader);
		this.m_spawnedEntry.m_Header.Deserialise(reader);
		return true;
	}

	// Token: 0x04001097 RID: 4247
	public EntitySerialisationEntry m_parentEntry = new EntitySerialisationEntry();

	// Token: 0x04001098 RID: 4248
	public EntitySerialisationEntry m_spawnedEntry = new EntitySerialisationEntry();
}
