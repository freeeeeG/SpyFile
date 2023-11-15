using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008AD RID: 2221
public class CookingStateMessage : Serialisable
{
	// Token: 0x06002B4C RID: 11084 RVA: 0x000CACF6 File Offset: 0x000C90F6
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_cookingState, 4);
		writer.Write(this.m_cookingProgress);
	}

	// Token: 0x06002B4D RID: 11085 RVA: 0x000CAD11 File Offset: 0x000C9111
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_cookingState = (CookingUIController.State)reader.ReadUInt32(4);
		this.m_cookingProgress = reader.ReadFloat32();
		return true;
	}

	// Token: 0x04002249 RID: 8777
	private const int kNumStateBits = 4;

	// Token: 0x0400224A RID: 8778
	public CookingUIController.State m_cookingState;

	// Token: 0x0400224B RID: 8779
	public float m_cookingProgress;
}
