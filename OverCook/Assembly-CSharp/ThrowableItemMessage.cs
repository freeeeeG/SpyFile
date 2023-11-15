using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000817 RID: 2071
public class ThrowableItemMessage : Serialisable
{
	// Token: 0x060027A6 RID: 10150 RVA: 0x000BA4F6 File Offset: 0x000B88F6
	public void Initialise(bool _inFlight, GameObject _thrower)
	{
		this.m_inFlight = _inFlight;
		this.m_thrower = _thrower;
	}

	// Token: 0x060027A7 RID: 10151 RVA: 0x000BA508 File Offset: 0x000B8908
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_inFlight);
		if (this.m_inFlight)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_thrower);
			entry.m_Header.Serialise(writer);
		}
	}

	// Token: 0x060027A8 RID: 10152 RVA: 0x000BA544 File Offset: 0x000B8944
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_inFlight = reader.ReadBit();
		if (this.m_inFlight)
		{
			this.m_entityHeader.Deserialise(reader);
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_entityHeader.m_uEntityID);
			if (entry == null)
			{
				return false;
			}
			this.m_thrower = entry.m_GameObject;
		}
		return true;
	}

	// Token: 0x04001F2A RID: 7978
	public GameObject m_thrower;

	// Token: 0x04001F2B RID: 7979
	public bool m_inFlight;

	// Token: 0x04001F2C RID: 7980
	private EntityMessageHeader m_entityHeader = new EntityMessageHeader();
}
