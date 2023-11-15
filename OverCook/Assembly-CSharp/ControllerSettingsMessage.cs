using System;
using BitStream;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008AB RID: 2219
internal class ControllerSettingsMessage : Serialisable
{
	// Token: 0x06002B40 RID: 11072 RVA: 0x000CA8F7 File Offset: 0x000C8CF7
	public void Initialise(PadSide side, User.MachineID machine, EngagementSlot engagement, User.SplitStatus split)
	{
		this.m_side = side;
		this.m_Machine = machine;
		this.m_EngagementSlot = engagement;
		this.m_Split = split;
	}

	// Token: 0x06002B41 RID: 11073 RVA: 0x000CA916 File Offset: 0x000C8D16
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_side, 2);
		writer.Write((uint)this.m_Machine, 3);
		writer.Write((uint)this.m_EngagementSlot, 3);
		writer.Write((uint)this.m_Split, 2);
	}

	// Token: 0x06002B42 RID: 11074 RVA: 0x000CA94C File Offset: 0x000C8D4C
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_side = (PadSide)reader.ReadUInt32(2);
		this.m_Machine = (User.MachineID)reader.ReadUInt32(3);
		this.m_EngagementSlot = (EngagementSlot)reader.ReadUInt32(3);
		this.m_Split = (User.SplitStatus)reader.ReadUInt32(2);
		return true;
	}

	// Token: 0x0400223E RID: 8766
	public PadSide m_side = PadSide.Both;

	// Token: 0x0400223F RID: 8767
	public User.MachineID m_Machine = User.MachineID.Count;

	// Token: 0x04002240 RID: 8768
	public EngagementSlot m_EngagementSlot = EngagementSlot.Count;

	// Token: 0x04002241 RID: 8769
	public User.SplitStatus m_Split = User.SplitStatus.Count;
}
