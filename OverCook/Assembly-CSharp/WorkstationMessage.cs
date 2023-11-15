using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000633 RID: 1587
public class WorkstationMessage : Serialisable
{
	// Token: 0x06001E30 RID: 7728 RVA: 0x0009214A File Offset: 0x0009054A
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_interacting);
		this.m_interactor.m_Header.Serialise(writer);
		if (this.m_interacting)
		{
			this.m_item.m_Header.Serialise(writer);
		}
	}

	// Token: 0x06001E31 RID: 7729 RVA: 0x00092185 File Offset: 0x00090585
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_interacting = reader.ReadBit();
		this.m_interactorHeader.Deserialise(reader);
		if (this.m_interacting)
		{
			this.m_itemHeader.Deserialise(reader);
		}
		return true;
	}

	// Token: 0x04001741 RID: 5953
	public EntitySerialisationEntry m_interactor;

	// Token: 0x04001742 RID: 5954
	public EntitySerialisationEntry m_item;

	// Token: 0x04001743 RID: 5955
	public bool m_interacting;

	// Token: 0x04001744 RID: 5956
	public EntityMessageHeader m_interactorHeader = new EntityMessageHeader();

	// Token: 0x04001745 RID: 5957
	public EntityMessageHeader m_itemHeader = new EntityMessageHeader();
}
