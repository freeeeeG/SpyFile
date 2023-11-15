using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200062C RID: 1580
public class WashingStationMessage : Serialisable
{
	// Token: 0x06001DF4 RID: 7668 RVA: 0x000912B4 File Offset: 0x0008F6B4
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_msgType, 2);
		if (this.m_msgType == WashingStationMessage.MessageType.InteractionState)
		{
			writer.Write(this.m_interacting);
			writer.Write(this.m_progress);
		}
		else
		{
			writer.Write((uint)this.m_plateCount, 4);
		}
	}

	// Token: 0x06001DF5 RID: 7669 RVA: 0x00091304 File Offset: 0x0008F704
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_msgType = (WashingStationMessage.MessageType)reader.ReadUInt32(2);
		if (this.m_msgType == WashingStationMessage.MessageType.InteractionState)
		{
			this.m_interacting = reader.ReadBit();
			this.m_progress = reader.ReadFloat32();
		}
		else
		{
			this.m_plateCount = (int)reader.ReadUInt32(4);
		}
		return true;
	}

	// Token: 0x04001718 RID: 5912
	private const int c_msgTypeBits = 2;

	// Token: 0x04001719 RID: 5913
	private const int c_plateCountBits = 4;

	// Token: 0x0400171A RID: 5914
	public WashingStationMessage.MessageType m_msgType;

	// Token: 0x0400171B RID: 5915
	public bool m_interacting;

	// Token: 0x0400171C RID: 5916
	public float m_progress;

	// Token: 0x0400171D RID: 5917
	public int m_plateCount;

	// Token: 0x0200062D RID: 1581
	public enum MessageType
	{
		// Token: 0x0400171F RID: 5919
		InteractionState,
		// Token: 0x04001720 RID: 5920
		AddPlates,
		// Token: 0x04001721 RID: 5921
		CleanedPlate
	}
}
