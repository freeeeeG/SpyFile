using System;
using BitStream;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008A2 RID: 2210
public class ChefAvatarMessage : Serialisable
{
	// Token: 0x06002B19 RID: 11033 RVA: 0x000CA1BA File Offset: 0x000C85BA
	public void Initialise(uint chefAvatar, User.MachineID machine, EngagementSlot engagement, User.SplitStatus split)
	{
		this.m_ChefAvatar = chefAvatar;
		this.m_Machine = machine;
		this.m_EngagementSlot = engagement;
		this.m_Split = split;
	}

	// Token: 0x06002B1A RID: 11034 RVA: 0x000CA1D9 File Offset: 0x000C85D9
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write(this.m_ChefAvatar, 7);
		writer.Write((uint)this.m_Machine, 3);
		writer.Write((uint)this.m_EngagementSlot, 3);
		writer.Write((uint)this.m_Split, 3);
	}

	// Token: 0x06002B1B RID: 11035 RVA: 0x000CA20F File Offset: 0x000C860F
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_ChefAvatar = reader.ReadUInt32(7);
		this.m_Machine = (User.MachineID)reader.ReadUInt32(3);
		this.m_EngagementSlot = (EngagementSlot)reader.ReadUInt32(3);
		this.m_Split = (User.SplitStatus)reader.ReadUInt32(3);
		return true;
	}

	// Token: 0x0400220D RID: 8717
	public uint m_ChefAvatar;

	// Token: 0x0400220E RID: 8718
	public User.MachineID m_Machine = User.MachineID.Count;

	// Token: 0x0400220F RID: 8719
	public EngagementSlot m_EngagementSlot = EngagementSlot.Count;

	// Token: 0x04002210 RID: 8720
	public User.SplitStatus m_Split = User.SplitStatus.Count;
}
