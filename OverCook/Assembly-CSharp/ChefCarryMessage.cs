using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008A3 RID: 2211
public class ChefCarryMessage : Serialisable
{
	// Token: 0x06002B1D RID: 11037 RVA: 0x000CA24E File Offset: 0x000C864E
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_carriableItem, 10);
		writer.Write((uint)this.m_playerAttachTarget, ChefCarryMessage.playerAttachTargetBits);
	}

	// Token: 0x06002B1E RID: 11038 RVA: 0x000CA26F File Offset: 0x000C866F
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_carriableItem = reader.ReadUInt32(10);
		this.m_playerAttachTarget = (PlayerAttachTarget)reader.ReadUInt32(ChefCarryMessage.playerAttachTargetBits);
		return true;
	}

	// Token: 0x04002211 RID: 8721
	private static readonly int playerAttachTargetBits = GameUtils.GetRequiredBitCount(2);

	// Token: 0x04002212 RID: 8722
	public uint m_carriableItem;

	// Token: 0x04002213 RID: 8723
	public PlayerAttachTarget m_playerAttachTarget;
}
