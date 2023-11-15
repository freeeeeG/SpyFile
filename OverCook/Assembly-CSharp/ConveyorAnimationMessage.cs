using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020005B2 RID: 1458
public class ConveyorAnimationMessage : Serialisable
{
	// Token: 0x06001BB8 RID: 7096 RVA: 0x00087EB9 File Offset: 0x000862B9
	public void Initialise(TriggerAnimationOnConveyor.State _state)
	{
		this.m_state = _state;
	}

	// Token: 0x06001BB9 RID: 7097 RVA: 0x00087EC2 File Offset: 0x000862C2
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_state, 2);
	}

	// Token: 0x06001BBA RID: 7098 RVA: 0x00087ED1 File Offset: 0x000862D1
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_state = (TriggerAnimationOnConveyor.State)reader.ReadUInt32(2);
		return true;
	}

	// Token: 0x040015BB RID: 5563
	public const int kBitsPerState = 2;

	// Token: 0x040015BC RID: 5564
	public TriggerAnimationOnConveyor.State m_state;
}
