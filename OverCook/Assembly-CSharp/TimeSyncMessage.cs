using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008E4 RID: 2276
public class TimeSyncMessage : Serialisable
{
	// Token: 0x06002C30 RID: 11312 RVA: 0x000CDD40 File Offset: 0x000CC140
	public void Initialise(float _fTime)
	{
		this.fTime = _fTime;
	}

	// Token: 0x06002C31 RID: 11313 RVA: 0x000CDD49 File Offset: 0x000CC149
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.fTime);
	}

	// Token: 0x06002C32 RID: 11314 RVA: 0x000CDD57 File Offset: 0x000CC157
	public bool Deserialise(BitStreamReader reader)
	{
		this.fTime = reader.ReadFloat32();
		return true;
	}

	// Token: 0x04002385 RID: 9093
	public float fTime;
}
