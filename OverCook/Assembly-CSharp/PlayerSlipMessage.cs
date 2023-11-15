using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000A1E RID: 2590
public class PlayerSlipMessage : Serialisable
{
	// Token: 0x0600335E RID: 13150 RVA: 0x000F1FEA File Offset: 0x000F03EA
	public void Initialise(PlayerSlipMessage.MsgType _msgType)
	{
		this.m_msgType = _msgType;
	}

	// Token: 0x0600335F RID: 13151 RVA: 0x000F1FF3 File Offset: 0x000F03F3
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_msgType, 2);
	}

	// Token: 0x06003360 RID: 13152 RVA: 0x000F2002 File Offset: 0x000F0402
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_msgType = (PlayerSlipMessage.MsgType)reader.ReadUInt32(2);
		return true;
	}

	// Token: 0x04002954 RID: 10580
	private const int kBitsPerMsgType = 2;

	// Token: 0x04002955 RID: 10581
	public PlayerSlipMessage.MsgType m_msgType;

	// Token: 0x02000A1F RID: 2591
	public enum MsgType
	{
		// Token: 0x04002957 RID: 10583
		Slip,
		// Token: 0x04002958 RID: 10584
		Stand,
		// Token: 0x04002959 RID: 10585
		Finished
	}
}
