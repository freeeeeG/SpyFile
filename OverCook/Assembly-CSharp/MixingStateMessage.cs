using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008D3 RID: 2259
public class MixingStateMessage : Serialisable
{
	// Token: 0x06002BD6 RID: 11222 RVA: 0x000CC86E File Offset: 0x000CAC6E
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_mixingState, 4);
		writer.Write(this.m_mixingProgress);
	}

	// Token: 0x06002BD7 RID: 11223 RVA: 0x000CC889 File Offset: 0x000CAC89
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_mixingState = (CookingUIController.State)reader.ReadUInt32(4);
		this.m_mixingProgress = reader.ReadFloat32();
		return true;
	}

	// Token: 0x04002333 RID: 9011
	private const int kNumStateBits = 4;

	// Token: 0x04002334 RID: 9012
	public CookingUIController.State m_mixingState;

	// Token: 0x04002335 RID: 9013
	public float m_mixingProgress;
}
