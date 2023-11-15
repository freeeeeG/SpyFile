using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020005A7 RID: 1447
public class SessionInteractableMessage : Serialisable
{
	// Token: 0x06001B82 RID: 7042 RVA: 0x00087C64 File Offset: 0x00086064
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_msgType, 2);
		if (this.m_msgType == SessionInteractableMessage.MessageType.InteractionState)
		{
			writer.Write(this.m_interacterID, 10);
		}
	}

	// Token: 0x06001B83 RID: 7043 RVA: 0x00087C8C File Offset: 0x0008608C
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_msgType = (SessionInteractableMessage.MessageType)reader.ReadUInt32(2);
		if (this.m_msgType == SessionInteractableMessage.MessageType.InteractionState)
		{
			this.m_interacterID = reader.ReadUInt32(10);
		}
		return true;
	}

	// Token: 0x0400159D RID: 5533
	private const int c_msgTypeBits = 2;

	// Token: 0x0400159E RID: 5534
	public SessionInteractableMessage.MessageType m_msgType;

	// Token: 0x0400159F RID: 5535
	public uint m_interacterID;

	// Token: 0x020005A8 RID: 1448
	public enum MessageType
	{
		// Token: 0x040015A1 RID: 5537
		InteractionState
	}
}
