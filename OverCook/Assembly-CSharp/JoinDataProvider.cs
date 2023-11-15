using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using BitStream;
using Team17.Online;

// Token: 0x0200084E RID: 2126
public static class JoinDataProvider
{
	// Token: 0x060028F0 RID: 10480 RVA: 0x000C01AC File Offset: 0x000BE5AC
	public static OnlineMultiplayerSessionJoinLocalUserData BuildJoinRequestData(EngagementSlot slot, NetConnectionState requestingJoinState, OnlineMultiplayerLocalUserId requestingLocalUser)
	{
		try
		{
			JoinDataProvider.m_gameUserData.Clear();
			JoinDataProvider.m_buffer.Clear();
			JoinDataProvider.m_streamWriter.Reset(JoinDataProvider.m_buffer);
			int i = 0;
			while (i < ClientUserSystem.m_Users.Count)
			{
				User user = ClientUserSystem.m_Users._items[i];
				if (user.IsLocal && slot == user.Engagement)
				{
					JoinDataProvider.m_gameUserData.DisplayName = requestingLocalUser.m_userName;
					JoinDataProvider.m_gameUserData.Slot = slot;
					JoinDataProvider.m_gameUserData.JoinMethod = requestingJoinState;
					JoinDataProvider.m_gameUserData.PlatformId = requestingLocalUser.m_platformId;
					JoinDataProvider.m_gameUserData.AvatarId = user.SelectedChefAvatar;
					JoinDataProvider.m_gameUserData.ControllerSide = user.PadSide;
					JoinDataProvider.m_gameUserData.ControllerSplitStatus = user.Split;
					if (JoinDataProvider.m_gameUserData.Serialize(JoinDataProvider.m_streamWriter))
					{
						JoinDataProvider.m_gameUserData.Clear();
						byte[] array = new byte[JoinDataProvider.m_buffer.Count];
						Buffer.BlockCopy(JoinDataProvider.m_buffer._items, 0, array, 0, JoinDataProvider.m_buffer.Count);
						return new OnlineMultiplayerSessionJoinLocalUserData
						{
							Id = requestingLocalUser,
							GameData = array,
							GameDataSize = (uint)array.Length
						};
					}
					throw new Exception("GameUserData.Serialize() : Failed!");
				}
				else
				{
					i++;
				}
			}
			throw new Exception("JoinDataProvider.BuildJoinRequestData() : Failed to find local user!");
		}
		catch (Exception ex)
		{
		}
		JoinDataProvider.m_gameUserData.Clear();
		return null;
	}

	// Token: 0x0400206C RID: 8300
	private static FastList<byte> m_buffer = new FastList<byte>(256);

	// Token: 0x0400206D RID: 8301
	private static BitStreamWriter m_streamWriter = new BitStreamWriter(JoinDataProvider.m_buffer);

	// Token: 0x0400206E RID: 8302
	private static JoinDataProvider.GameUserData m_gameUserData = new JoinDataProvider.GameUserData();

	// Token: 0x0200084F RID: 2127
	public class GameUserData
	{
		// Token: 0x17000329 RID: 809
		// (get) Token: 0x060028F3 RID: 10483 RVA: 0x000C036A File Offset: 0x000BE76A
		// (set) Token: 0x060028F4 RID: 10484 RVA: 0x000C0372 File Offset: 0x000BE772
		[DefaultValue(null)]
		public string DisplayName { get; set; }

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x060028F5 RID: 10485 RVA: 0x000C037B File Offset: 0x000BE77B
		// (set) Token: 0x060028F6 RID: 10486 RVA: 0x000C0383 File Offset: 0x000BE783
		[DefaultValue(EngagementSlot.Count)]
		public EngagementSlot Slot { get; set; }

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x060028F7 RID: 10487 RVA: 0x000C038C File Offset: 0x000BE78C
		// (set) Token: 0x060028F8 RID: 10488 RVA: 0x000C0394 File Offset: 0x000BE794
		[DefaultValue(NetConnectionState.COUNT)]
		public NetConnectionState JoinMethod { get; set; }

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x060028F9 RID: 10489 RVA: 0x000C039D File Offset: 0x000BE79D
		// (set) Token: 0x060028FA RID: 10490 RVA: 0x000C03A5 File Offset: 0x000BE7A5
		[DefaultValue(null)]
		public OnlineUserPlatformId PlatformId { get; set; }

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x060028FB RID: 10491 RVA: 0x000C03AE File Offset: 0x000BE7AE
		// (set) Token: 0x060028FC RID: 10492 RVA: 0x000C03B6 File Offset: 0x000BE7B6
		[DefaultValue(127L)]
		public uint AvatarId { get; set; }

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x060028FD RID: 10493 RVA: 0x000C03BF File Offset: 0x000BE7BF
		// (set) Token: 0x060028FE RID: 10494 RVA: 0x000C03C7 File Offset: 0x000BE7C7
		[DefaultValue(PadSide.Both)]
		public PadSide ControllerSide { get; set; }

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x060028FF RID: 10495 RVA: 0x000C03D0 File Offset: 0x000BE7D0
		// (set) Token: 0x06002900 RID: 10496 RVA: 0x000C03D8 File Offset: 0x000BE7D8
		[DefaultValue(User.SplitStatus.Count)]
		public User.SplitStatus ControllerSplitStatus { get; set; }

		// Token: 0x06002901 RID: 10497 RVA: 0x000C03E1 File Offset: 0x000BE7E1
		public void Clear()
		{
			this.Slot = EngagementSlot.Count;
			this.JoinMethod = NetConnectionState.COUNT;
			this.PlatformId = null;
			this.AvatarId = 127U;
			this.DisplayName = null;
			this.ControllerSide = PadSide.Both;
			this.ControllerSplitStatus = User.SplitStatus.Count;
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x000C0418 File Offset: 0x000BE818
		public bool Serialize(BitStreamWriter writer)
		{
			try
			{
				writer.Write(this.DisplayName, Encoding.Unicode);
				writer.Write((uint)this.Slot, 3);
				writer.Write((uint)this.JoinMethod, 4);
				this.PlatformId.Serialise(writer);
				writer.Write(this.AvatarId, 8);
				writer.Write((uint)this.ControllerSide, 3);
				writer.Write((uint)this.ControllerSplitStatus, 3);
				return true;
			}
			catch (Exception ex)
			{
			}
			return false;
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x000C04A4 File Offset: 0x000BE8A4
		public bool Deserialize(BitStreamReader reader)
		{
			try
			{
				this.PlatformId = new OnlineUserPlatformId();
				this.DisplayName = reader.ReadString(Encoding.Unicode);
				this.Slot = (EngagementSlot)reader.ReadUInt32(3);
				this.JoinMethod = (NetConnectionState)reader.ReadUInt32(4);
				this.PlatformId.Deserialise(reader);
				this.AvatarId = reader.ReadUInt32(8);
				this.ControllerSide = (PadSide)reader.ReadUInt32(3);
				this.ControllerSplitStatus = (User.SplitStatus)reader.ReadUInt32(3);
				return true;
			}
			catch (Exception ex)
			{
			}
			this.Clear();
			return false;
		}
	}
}
