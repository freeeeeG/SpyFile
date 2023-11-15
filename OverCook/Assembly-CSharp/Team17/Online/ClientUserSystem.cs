using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace Team17.Online
{
	// Token: 0x02000993 RID: 2451
	public class ClientUserSystem
	{
		// Token: 0x06002FCC RID: 12236 RVA: 0x000E0620 File Offset: 0x000DEA20
		public void Initialise()
		{
			Mailbox.Client.UnregisterForMessageType(MessageType.UsersChanged, new OrderedMessageReceivedCallback(this.OnUsersChanged));
			Mailbox.Client.RegisterForMessageType(MessageType.UsersChanged, new OrderedMessageReceivedCallback(this.OnUsersChanged));
			Mailbox.Client.UnregisterForMessageType(MessageType.UsersAdded, new OrderedMessageReceivedCallback(this.OnUserAdded));
			Mailbox.Client.RegisterForMessageType(MessageType.UsersAdded, new OrderedMessageReceivedCallback(this.OnUserAdded));
			GameUtils.RequireManager<PlayerManager>().EngagementChangeCallback += this.OnEngagementChanged;
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x000E06A4 File Offset: 0x000DEAA4
		private void OnEngagementChanged(EngagementSlot slot, GamepadUser user, GamepadUser gamepad)
		{
			FastList<User> users = ClientUserSystem.m_Users;
			User.MachineID machineId = ClientUserSystem.s_LocalMachineId;
			User user2 = UserSystemUtils.FindUser(users, null, machineId, slot, TeamID.Count, User.SplitStatus.Count);
			if (user2 != null)
			{
				user2.Engagement = slot;
			}
			if (user != null && gamepad != null && user.UID != gamepad.UID)
			{
				for (int i = ClientUserSystem.m_avatarImageCache.Count - 1; i >= 0; i--)
				{
					ClientUserSystem.AvatarImageCacheEntry avatarImageCacheEntry = ClientUserSystem.m_avatarImageCache._items[i];
					if (avatarImageCacheEntry != null && avatarImageCacheEntry.m_engagementSlot == slot)
					{
						avatarImageCacheEntry.Clear();
						ClientUserSystem.m_avatarImageCache.RemoveAt(i);
					}
				}
			}
			ClientUserSystem.UpdateAvatarImageCache();
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x000E075C File Offset: 0x000DEB5C
		public void Clear()
		{
			Mailbox.Client.UnregisterForMessageType(MessageType.UsersChanged, new OrderedMessageReceivedCallback(this.OnUsersChanged));
			Mailbox.Client.UnregisterForMessageType(MessageType.UsersAdded, new OrderedMessageReceivedCallback(this.OnUserAdded));
			ClientUserSystem.m_Users.Clear();
			GameUtils.RequireManager<PlayerManager>().EngagementChangeCallback -= this.OnEngagementChanged;
			ClientUserSystem.ClearAvatarImageCache();
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x000E07BD File Offset: 0x000DEBBD
		public static void Update()
		{
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x000E07C0 File Offset: 0x000DEBC0
		public static void SetMachineId(User.MachineID eMachine)
		{
			for (int i = 0; i < ClientUserSystem.m_Users.Count; i++)
			{
				User user = ClientUserSystem.m_Users._items[i];
				if (user.Machine == ClientUserSystem.s_LocalMachineId)
				{
					user.Machine = eMachine;
				}
			}
			ClientUserSystem.s_LocalMachineId = eMachine;
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x000E0814 File Offset: 0x000DEC14
		public static void ClearAvatarImageCache()
		{
			for (int i = 0; i < ClientUserSystem.m_avatarImageCache.Count; i++)
			{
				ClientUserSystem.m_avatarImageCache._items[i].Clear();
			}
			ClientUserSystem.m_avatarImageCache.Clear();
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x000E0858 File Offset: 0x000DEC58
		public void OnUserAdded(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			UserAddedMessage userAddedMessage = (UserAddedMessage)message;
			if (ClientUserSystem.userAdded != null)
			{
				ClientUserSystem.userAdded(userAddedMessage.UserIndex, userAddedMessage.User);
			}
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x000E088C File Offset: 0x000DEC8C
		public void OnUsersChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			this.m_UsersChanged = (UsersChangedMessage)message;
			for (int i = 0; i < this.m_UsersChanged.m_Users.Count; i++)
			{
				UserData userData = this.m_UsersChanged.m_Users._items[i];
				IOnlineMultiplayerSessionUserId sessionUserFromUniqueId = UserSystemUtils.GetSessionUserFromUniqueId(ClientUserSystem.m_Users, userData.sessionUserUniqueId);
				User user = null;
				if (i < ClientUserSystem.m_Users.Count)
				{
					user = ClientUserSystem.m_Users._items[i];
				}
				if (user != null)
				{
					user.IsLocal = (userData.machine == ClientUserSystem.s_LocalMachineId);
					user.Machine = userData.machine;
					user.SessionId = sessionUserFromUniqueId;
					user.EntityID = userData.entity;
					user.Entity2ID = userData.entity2;
					user.Engagement = userData.slot;
					user.GameState = userData.gameState;
					user.Team = userData.team;
					user.Colour = userData.colour;
					user.SelectedChefAvatar = userData.selectedChefAvatar;
					user.PadSide = userData.padSide;
					user.Split = userData.splitStatus;
					user.PartyPersist = userData.partyPersist;
					user.DisplayName = userData.displayName;
					user.PlatformID = userData.platformId;
					user.SelectedChefAvatar = userData.selectedChefAvatar;
				}
				else
				{
					this.AddUser(userData.machine == ClientUserSystem.s_LocalMachineId, userData.machine, sessionUserFromUniqueId, userData.entity, userData.entity2, userData.slot, userData.team, userData.colour, userData.selectedChefAvatar, userData.padSide, userData.splitStatus, userData.partyPersist, userData.displayName, userData.platformId, userData.selectedChefAvatar);
				}
			}
			if (this.m_UsersChanged.m_Users.Count < ClientUserSystem.m_Users.Count)
			{
				int count = ClientUserSystem.m_Users.Count - this.m_UsersChanged.m_Users.Count;
				ClientUserSystem.m_Users.RemoveRange(this.m_UsersChanged.m_Users.Count, count);
			}
			ClientUserSystem.UpdateAvatarImageCache();
			if (ClientUserSystem.usersChanged != null)
			{
				ClientUserSystem.usersChanged();
			}
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x000E0AAC File Offset: 0x000DEEAC
		public static Texture2D GetAvatarImage(User user)
		{
			if (user != null)
			{
				for (int i = 0; i < ClientUserSystem.m_avatarImageCache.Count; i++)
				{
					ClientUserSystem.AvatarImageCacheEntry avatarImageCacheEntry = ClientUserSystem.m_avatarImageCache._items[i];
					if (avatarImageCacheEntry.m_machineId == user.Machine && avatarImageCacheEntry.m_engagementSlot == user.Engagement)
					{
						return avatarImageCacheEntry.m_image;
					}
				}
			}
			return null;
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x000E0B14 File Offset: 0x000DEF14
		private User AddUser(bool bLocal = false, User.MachineID machine = User.MachineID.Count, IOnlineMultiplayerSessionUserId sessionId = null, uint entityId = 0U, uint entity2Id = 0U, EngagementSlot engagement = EngagementSlot.Count, TeamID team = TeamID.None, uint colour = 7U, uint avatar = 127U, PadSide padSide = PadSide.Both, User.SplitStatus splitStatus = User.SplitStatus.Count, User.PartyPersistance partyPersist = User.PartyPersistance.NotSet, string displayName = "Unknown", OnlineUserPlatformId platformUserId = null, uint chefAvatarID = 127U)
		{
			User user = new User(bLocal, machine, sessionId, entityId, entity2Id, engagement, team, colour, avatar, padSide, splitStatus, partyPersist, displayName, platformUserId, chefAvatarID);
			ClientUserSystem.m_Users.Add(user);
			return user;
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x000E0B50 File Offset: 0x000DEF50
		private static void UpdateAvatarImageCache()
		{
			if (ClientUserSystem.m_avatarCoordinator == null)
			{
				ClientUserSystem.m_avatarCoordinator = GameUtils.RequireManagerInterface<IOnlinePlatformManager>().OnlineAvatarImageCoordinator();
			}
			if (ClientUserSystem.m_avatarCoordinator != null)
			{
				int count = ClientUserSystem.m_avatarImageCache.Count;
				if (count > 0)
				{
					for (int i = count - 1; i >= 0; i--)
					{
						ClientUserSystem.AvatarImageCacheEntry avatarImageCacheEntry = ClientUserSystem.m_avatarImageCache._items[i];
						bool flag = false;
						int num = 0;
						while (num < ClientUserSystem.m_Users.Count && !flag)
						{
							User user = ClientUserSystem.m_Users._items[num];
							if (avatarImageCacheEntry.m_machineId == user.Machine && avatarImageCacheEntry.m_engagementSlot == user.Engagement)
							{
								flag = true;
							}
							num++;
						}
						if (!flag)
						{
							avatarImageCacheEntry.Clear();
							ClientUserSystem.m_avatarImageCache.RemoveAt(i);
						}
					}
				}
				User user2 = null;
				for (int j = 0; j < ClientUserSystem.m_Users.Count; j++)
				{
					User user3 = ClientUserSystem.m_Users._items[j];
					bool flag2 = false;
					int num2 = 0;
					while (num2 < ClientUserSystem.m_avatarImageCache.Count && !flag2)
					{
						ClientUserSystem.AvatarImageCacheEntry avatarImageCacheEntry2 = ClientUserSystem.m_avatarImageCache._items[num2];
						if (avatarImageCacheEntry2.m_machineId == user3.Machine && avatarImageCacheEntry2.m_engagementSlot == user3.Engagement)
						{
							flag2 = true;
						}
						num2++;
					}
					if (!flag2)
					{
						if (user3.IsLocal)
						{
							ulong uniqueRequestId = 0UL;
							IOnlineAvatarImageCoordinator avatarCoordinator = ClientUserSystem.m_avatarCoordinator;
							GamepadUser gamepadUser = user3.GamepadUser;
							if (ClientUserSystem.<>f__mg$cache0 == null)
							{
								ClientUserSystem.<>f__mg$cache0 = new AvatarImageRequestCompletionCallback(ClientUserSystem.OnAvatarImageRequestCompletionCallback);
							}
							if (avatarCoordinator.RequestAvatarImage(gamepadUser, ClientUserSystem.<>f__mg$cache0, out uniqueRequestId))
							{
								ClientUserSystem.m_avatarImageCache.Add(new ClientUserSystem.AvatarImageCacheEntry
								{
									m_uniqueRequestId = uniqueRequestId,
									m_machineId = user3.Machine,
									m_engagementSlot = user3.Engagement
								});
							}
						}
						else
						{
							if (user2 == null)
							{
								int num3 = 0;
								while (num3 < ClientUserSystem.m_Users.Count && user2 == null)
								{
									User user4 = ClientUserSystem.m_Users._items[num3];
									if (user4.IsLocal && user4.Engagement == EngagementSlot.One)
									{
										user2 = user4;
									}
									num3++;
								}
							}
							if (user2 != null && null != user2.GamepadUser)
							{
								ulong uniqueRequestId2 = 0UL;
								IOnlineAvatarImageCoordinator avatarCoordinator2 = ClientUserSystem.m_avatarCoordinator;
								GamepadUser gamepadUser2 = user2.GamepadUser;
								OnlineUserPlatformId platformID = user3.PlatformID;
								if (ClientUserSystem.<>f__mg$cache1 == null)
								{
									ClientUserSystem.<>f__mg$cache1 = new AvatarImageRequestCompletionCallback(ClientUserSystem.OnAvatarImageRequestCompletionCallback);
								}
								if (avatarCoordinator2.RequestAvatarImage(gamepadUser2, platformID, ClientUserSystem.<>f__mg$cache1, out uniqueRequestId2))
								{
									ClientUserSystem.m_avatarImageCache.Add(new ClientUserSystem.AvatarImageCacheEntry
									{
										m_uniqueRequestId = uniqueRequestId2,
										m_machineId = user3.Machine,
										m_engagementSlot = user3.Engagement
									});
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x000E0E2C File Offset: 0x000DF22C
		private static void OnAvatarImageRequestCompletionCallback(Texture2D image, ulong uniqueRequestId)
		{
			bool flag = false;
			int num = 0;
			while (num < ClientUserSystem.m_avatarImageCache.Count && !flag)
			{
				ClientUserSystem.AvatarImageCacheEntry avatarImageCacheEntry = ClientUserSystem.m_avatarImageCache._items[num];
				if (uniqueRequestId == avatarImageCacheEntry.m_uniqueRequestId)
				{
					flag = true;
					if (null != avatarImageCacheEntry.m_image)
					{
						UnityEngine.Object.Destroy(avatarImageCacheEntry.m_image);
					}
					avatarImageCacheEntry.m_image = image;
					avatarImageCacheEntry.m_uniqueRequestId = 0UL;
				}
				num++;
			}
			if (flag)
			{
				if (ClientUserSystem.usersChanged != null)
				{
					ClientUserSystem.usersChanged();
				}
			}
			else
			{
				UnityEngine.Object.Destroy(image);
			}
		}

		// Token: 0x04002660 RID: 9824
		private static IOnlineAvatarImageCoordinator m_avatarCoordinator = null;

		// Token: 0x04002661 RID: 9825
		public static User.MachineID s_LocalMachineId = User.MachineID.One;

		// Token: 0x04002662 RID: 9826
		public static FastList<User> m_Users = new FastList<User>(4);

		// Token: 0x04002663 RID: 9827
		private static FastList<ClientUserSystem.AvatarImageCacheEntry> m_avatarImageCache = new FastList<ClientUserSystem.AvatarImageCacheEntry>(4);

		// Token: 0x04002664 RID: 9828
		public static GenericVoid usersChanged;

		// Token: 0x04002665 RID: 9829
		public static GenericVoid<uint, UserData> userAdded;

		// Token: 0x04002666 RID: 9830
		private UsersChangedMessage m_UsersChanged = new UsersChangedMessage();

		// Token: 0x04002667 RID: 9831
		[CompilerGenerated]
		private static AvatarImageRequestCompletionCallback <>f__mg$cache0;

		// Token: 0x04002668 RID: 9832
		[CompilerGenerated]
		private static AvatarImageRequestCompletionCallback <>f__mg$cache1;

		// Token: 0x02000994 RID: 2452
		private class AvatarImageCacheEntry
		{
			// Token: 0x06002FDA RID: 12250 RVA: 0x000E0F01 File Offset: 0x000DF301
			public void Clear()
			{
				if (null != this.m_image)
				{
					UnityEngine.Object.Destroy(this.m_image);
				}
				this.m_image = null;
				this.m_uniqueRequestId = 0UL;
				this.m_machineId = User.MachineID.Count;
				this.m_engagementSlot = EngagementSlot.Count;
			}

			// Token: 0x04002669 RID: 9833
			public Texture2D m_image;

			// Token: 0x0400266A RID: 9834
			public ulong m_uniqueRequestId;

			// Token: 0x0400266B RID: 9835
			public User.MachineID m_machineId = User.MachineID.Count;

			// Token: 0x0400266C RID: 9836
			public EngagementSlot m_engagementSlot = EngagementSlot.Count;
		}
	}
}
