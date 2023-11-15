using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020008D6 RID: 2262
public class RespawnMessage : Serialisable
{
	// Token: 0x06002BE0 RID: 11232 RVA: 0x000CCBAA File Offset: 0x000CAFAA
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_RespawnType, 4);
		writer.Write((uint)this.m_Phase, 4);
		if (this.m_Phase == RespawnMessage.Phase.End)
		{
			writer.Write(ref this.m_RespawnPosition);
		}
	}

	// Token: 0x06002BE1 RID: 11233 RVA: 0x000CCBDE File Offset: 0x000CAFDE
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_RespawnType = (RespawnCollider.RespawnType)reader.ReadUInt32(4);
		this.m_Phase = (RespawnMessage.Phase)reader.ReadUInt32(4);
		if (this.m_Phase == RespawnMessage.Phase.End)
		{
			reader.ReadVector3(ref this.m_RespawnPosition);
		}
		return true;
	}

	// Token: 0x0400233F RID: 9023
	public RespawnCollider.RespawnType m_RespawnType;

	// Token: 0x04002340 RID: 9024
	public RespawnMessage.Phase m_Phase;

	// Token: 0x04002341 RID: 9025
	public Vector3 m_RespawnPosition = default(Vector3);

	// Token: 0x020008D7 RID: 2263
	public enum Phase
	{
		// Token: 0x04002343 RID: 9027
		Begin,
		// Token: 0x04002344 RID: 9028
		End
	}
}
