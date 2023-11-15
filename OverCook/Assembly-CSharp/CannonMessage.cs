using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000448 RID: 1096
public class CannonMessage : Serialisable
{
	// Token: 0x06001430 RID: 5168 RVA: 0x0006E42C File Offset: 0x0006C82C
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_state, 2);
		writer.Write(this.m_angle);
		bool flag = this.m_loadedObject != null;
		writer.Write(flag);
		if (flag)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_loadedObject);
			entry.m_Header.Serialise(writer);
		}
	}

	// Token: 0x06001431 RID: 5169 RVA: 0x0006E484 File Offset: 0x0006C884
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_state = (CannonMessage.CannonState)reader.ReadUInt32(2);
		this.m_angle = reader.ReadFloat32();
		if (reader.ReadBit())
		{
			this.m_entityHeader.Deserialise(reader);
			this.m_loadedObject = EntitySerialisationRegistry.GetEntry(this.m_entityHeader.m_uEntityID).m_GameObject;
		}
		else
		{
			this.m_loadedObject = null;
		}
		return true;
	}

	// Token: 0x04000FA6 RID: 4006
	public CannonMessage.CannonState m_state;

	// Token: 0x04000FA7 RID: 4007
	public float m_angle;

	// Token: 0x04000FA8 RID: 4008
	public GameObject m_loadedObject;

	// Token: 0x04000FA9 RID: 4009
	public const int m_stateBits = 2;

	// Token: 0x04000FAA RID: 4010
	private EntityMessageHeader m_entityHeader = new EntityMessageHeader();

	// Token: 0x02000449 RID: 1097
	public enum CannonState
	{
		// Token: 0x04000FAC RID: 4012
		Launched,
		// Token: 0x04000FAD RID: 4013
		Load,
		// Token: 0x04000FAE RID: 4014
		Unload
	}
}
