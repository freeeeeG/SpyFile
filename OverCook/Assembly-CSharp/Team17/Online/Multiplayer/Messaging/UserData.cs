using System;
using System.Text;
using BitStream;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008E7 RID: 2279
	public class UserData : Serialisable
	{
		// Token: 0x06002C40 RID: 11328 RVA: 0x000CDECC File Offset: 0x000CC2CC
		public void Initialise(User user)
		{
			if (user.SessionId != null)
			{
				this.online = true;
				this.sessionUserUniqueId = user.SessionId.UniqueId;
			}
			else
			{
				this.online = false;
				this.sessionUserUniqueId = 0;
			}
			this.machine = user.Machine;
			this.entity = user.EntityID;
			this.entity2 = user.Entity2ID;
			this.slot = user.Engagement;
			this.team = user.Team;
			this.colour = user.Colour;
			this.selectedChefAvatar = user.SelectedChefAvatar;
			this.gameState = user.GameState;
			this.padSide = user.PadSide;
			this.splitStatus = user.Split;
			this.partyPersist = user.PartyPersist;
			this.displayName = user.DisplayName;
			this.platformId = user.PlatformID;
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x000CDFAC File Offset: 0x000CC3AC
		public void Serialise(BitStreamWriter writer)
		{
			writer.Write(this.online);
			if (this.online)
			{
				writer.Write(this.sessionUserUniqueId, 8);
			}
			writer.Write((uint)this.machine, 2);
			writer.Write(this.entity, 10);
			writer.Write(this.entity2, 10);
			writer.Write((uint)this.slot, 3);
			writer.Write(this.selectedChefAvatar, 7);
			writer.Write((uint)this.gameState, 5);
			writer.Write((uint)this.team, 2);
			writer.Write(this.colour, 3);
			writer.Write((uint)this.padSide, 2);
			writer.Write((uint)this.splitStatus, 2);
			writer.Write((uint)this.partyPersist, 2);
			bool flag = null != this.platformId;
			writer.Write(flag);
			if (flag)
			{
				this.platformId.Serialise(writer);
			}
			writer.Write(this.displayName, Encoding.Unicode);
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x000CE0A8 File Offset: 0x000CC4A8
		public bool Deserialise(BitStreamReader reader)
		{
			this.online = reader.ReadBit();
			if (this.online)
			{
				this.sessionUserUniqueId = reader.ReadByte(8);
			}
			this.machine = (User.MachineID)reader.ReadUInt32(2);
			this.entity = reader.ReadUInt32(10);
			this.entity2 = reader.ReadUInt32(10);
			this.slot = (EngagementSlot)reader.ReadUInt32(3);
			this.selectedChefAvatar = reader.ReadUInt32(7);
			this.gameState = (GameState)reader.ReadUInt32(5);
			this.team = (TeamID)reader.ReadUInt32(2);
			this.colour = reader.ReadUInt32(3);
			this.padSide = (PadSide)reader.ReadUInt32(2);
			this.splitStatus = (User.SplitStatus)reader.ReadUInt32(2);
			this.partyPersist = (User.PartyPersistance)reader.ReadUInt32(2);
			bool flag = reader.ReadBit();
			if (flag)
			{
				if (this.platformId == null)
				{
					this.platformId = new OnlineUserPlatformId();
				}
				this.platformId.Deserialise(reader);
			}
			this.displayName = reader.ReadString(Encoding.Unicode);
			return true;
		}

		// Token: 0x0400238D RID: 9101
		public bool online;

		// Token: 0x0400238E RID: 9102
		public byte sessionUserUniqueId;

		// Token: 0x0400238F RID: 9103
		public User.MachineID machine;

		// Token: 0x04002390 RID: 9104
		public uint entity;

		// Token: 0x04002391 RID: 9105
		public uint entity2;

		// Token: 0x04002392 RID: 9106
		public EngagementSlot slot;

		// Token: 0x04002393 RID: 9107
		public TeamID team;

		// Token: 0x04002394 RID: 9108
		public uint colour;

		// Token: 0x04002395 RID: 9109
		public uint selectedChefAvatar;

		// Token: 0x04002396 RID: 9110
		public GameState gameState;

		// Token: 0x04002397 RID: 9111
		public PadSide padSide;

		// Token: 0x04002398 RID: 9112
		public User.SplitStatus splitStatus;

		// Token: 0x04002399 RID: 9113
		public User.PartyPersistance partyPersist;

		// Token: 0x0400239A RID: 9114
		public string displayName;

		// Token: 0x0400239B RID: 9115
		public OnlineUserPlatformId platformId;
	}
}
