using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;

namespace Team17.Online
{
	// Token: 0x02000997 RID: 2455
	public class ServerUserSystem
	{
		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06002FE7 RID: 12263 RVA: 0x000E1239 File Offset: 0x000DF639
		public static bool EngagementsLocked
		{
			get
			{
				return ServerUserSystem.m_ignoreEngagement;
			}
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x000E1240 File Offset: 0x000DF640
		public void Initialise()
		{
			this.m_IPlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
			this.m_IPlayerManager.EngagementChangeCallback += this.OnEngagementChanged;
			ServerUserSystem.m_PrivilegeChecker.Initialise(new GenericVoid<IConnectionModeSwitchStatus>(this.EngagementPrivilegeCheckStarted), new GenericVoid<IConnectionModeSwitchStatus>(this.EngagementPrivilegeCheckComplete));
			Mailbox.Server.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnClientGameStateChanged));
			Mailbox.Server.RegisterForMessageType(MessageType.ChefAvatar, new OrderedMessageReceivedCallback(this.OnClientChefAvatarChanged));
			Mailbox.Server.RegisterForMessageType(MessageType.ControllerSettings, new OrderedMessageReceivedCallback(this.OnClientControllerSettingsChanged));
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x000E12DC File Offset: 0x000DF6DC
		public void Shutdown()
		{
			this.m_IPlayerManager.EngagementChangeCallback -= this.OnEngagementChanged;
			Mailbox.Server.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnClientGameStateChanged));
			Mailbox.Server.UnregisterForMessageType(MessageType.ChefAvatar, new OrderedMessageReceivedCallback(this.OnClientChefAvatarChanged));
			Mailbox.Server.UnregisterForMessageType(MessageType.ControllerSettings, new OrderedMessageReceivedCallback(this.OnClientControllerSettingsChanged));
			ServerUserSystem.m_PrivilegeChecker.Shutdown();
			ServerUserSystem.m_Users.Clear();
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x000E135C File Offset: 0x000DF75C
		public void InvalidateEntities()
		{
			int count = ServerUserSystem.m_Users.Count;
			for (int i = 0; i < count; i++)
			{
				ServerUserSystem.m_Users._items[i].EntityID = 0U;
				ServerUserSystem.m_Users._items[i].Entity2ID = 0U;
			}
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x000E13AC File Offset: 0x000DF7AC
		public static void RemoveMatchmadeUsers()
		{
			FastList<User> fastList = new FastList<User>(4);
			int count = ServerUserSystem.m_Users.Count;
			for (int i = 0; i < count; i++)
			{
				User user = ServerUserSystem.m_Users._items[i];
				if (user.PartyPersist == User.PartyPersistance.Kick)
				{
					fastList.Add(user);
				}
			}
			for (int j = 0; j < fastList.Count; j++)
			{
				ServerUserSystem.RemoveUser(fastList._items[j], true);
			}
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x000E1428 File Offset: 0x000DF828
		public static void ResetTeams()
		{
			int count = ServerUserSystem.m_Users.Count;
			for (int i = 0; i < count; i++)
			{
				User user = ServerUserSystem.m_Users._items[i];
				user.Team = TeamID.None;
			}
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x000E1468 File Offset: 0x000DF868
		private void OnClientGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			GameStateMessage gameStateMessage = (GameStateMessage)message;
			int count = ServerUserSystem.m_Users.Count;
			for (int i = 0; i < count; i++)
			{
				User user = ServerUserSystem.m_Users._items[i];
				if (user.Machine == gameStateMessage.m_Machine)
				{
					user.GameState = gameStateMessage.m_State;
				}
			}
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x000E14C4 File Offset: 0x000DF8C4
		private void OnClientChefAvatarChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			ChefAvatarMessage chefAvatarMessage = (ChefAvatarMessage)message;
			FastList<User> users = ServerUserSystem.m_Users;
			IOnlineMultiplayerSessionUserId sessionId = null;
			User.MachineID machine = chefAvatarMessage.m_Machine;
			EngagementSlot engagementSlot = chefAvatarMessage.m_EngagementSlot;
			User.SplitStatus split = chefAvatarMessage.m_Split;
			User user = UserSystemUtils.FindUser(users, sessionId, machine, engagementSlot, TeamID.Count, split);
			if (user != null)
			{
				user.SelectedChefAvatar = chefAvatarMessage.m_ChefAvatar;
			}
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x000E151C File Offset: 0x000DF91C
		private void OnClientControllerSettingsChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			ControllerSettingsMessage controllerSettingsMessage = (ControllerSettingsMessage)message;
			FastList<User> users = ServerUserSystem.m_Users;
			IOnlineMultiplayerSessionUserId sessionId = null;
			User.MachineID machine = controllerSettingsMessage.m_Machine;
			EngagementSlot engagementSlot = controllerSettingsMessage.m_EngagementSlot;
			User.SplitStatus split = controllerSettingsMessage.m_Split;
			User user = UserSystemUtils.FindUser(users, sessionId, machine, engagementSlot, TeamID.Count, split);
			if (user != null)
			{
				user.PadSide = controllerSettingsMessage.m_side;
			}
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x000E1574 File Offset: 0x000DF974
		public void Update()
		{
			ServerUserSystem.m_PrivilegeChecker.Update();
			int count = ServerUserSystem.m_Users.Count;
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				if (ServerUserSystem.m_Users._items[i].ChangedThisFrame)
				{
					flag = true;
				}
			}
			if (flag && ConnectionModeSwitcher.GetStatus().GetProgress() == eConnectionModeSwitchProgress.Complete && ConnectionModeSwitcher.GetStatus().GetResult() == eConnectionModeSwitchResult.Success)
			{
				for (int j = 0; j < count; j++)
				{
					if (ServerUserSystem.m_Users._items[j].ChangedThisFrame)
					{
						ServerUserSystem.m_Users._items[j].ChangedThisFrame = false;
					}
				}
				ServerMessenger.UsersChanged();
			}
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x000E1628 File Offset: 0x000DFA28
		public static User AddUser(bool bLocal = false, User.MachineID machine = User.MachineID.Count, IOnlineMultiplayerSessionUserId sessionId = null, uint entityId = 0U, uint entity2Id = 0U, EngagementSlot engagement = EngagementSlot.Count, PadSide side = PadSide.Both, TeamID team = TeamID.None, User.PartyPersistance partyPersist = User.PartyPersistance.NotSet, OnlineUserPlatformId platformID = null, uint chefAvatarID = 127U, string remoteDisplayName = null, User.SplitStatus splitStatus = User.SplitStatus.NotSplit)
		{
			if (ServerUserSystem.m_Users.Count >= 4)
			{
				return null;
			}
			uint num = (uint)ServerUserSystem.m_Users.Count;
			if (bLocal)
			{
				if ((ulong)engagement <= (ulong)((long)ServerUserSystem.m_Users.Count))
				{
					num = (uint)engagement;
				}
				FastList<User> users = ServerUserSystem.m_Users;
				User user = UserSystemUtils.FindUser(users, null, machine, engagement, TeamID.Count, User.SplitStatus.Count);
				if (user != null)
				{
					splitStatus = User.SplitStatus.SplitPadGuest;
					side = ((user.PadSide == PadSide.Right) ? PadSide.Left : PadSide.Right);
					user.Split = User.SplitStatus.SplitPadHost;
					user.PadSide = ((side != PadSide.Right) ? PadSide.Right : PadSide.Left);
					num = (uint)ServerUserSystem.m_Users.Count;
				}
			}
			uint num2 = num;
			string displayName = "Unknown";
			if (bLocal)
			{
				IPlayerManager playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
				if (playerManager != null)
				{
					GamepadUser user2 = playerManager.GetUser(engagement);
					if (user2 != null)
					{
						OnlineMultiplayerLocalUserId allowedUser = PrivilegeCheckCache.GetAllowedUser(user2);
						if (allowedUser != null)
						{
							if (platformID == null)
							{
								platformID = allowedUser.m_platformId;
							}
							displayName = allowedUser.m_userName;
						}
						else if (user2 != null)
						{
							displayName = user2.DisplayName;
						}
					}
				}
			}
			else if (remoteDisplayName != null)
			{
				displayName = remoteDisplayName;
			}
			uint colour = num2;
			PadSide padSide = side;
			User user3 = new User(bLocal, machine, sessionId, entityId, entity2Id, engagement, team, colour, 127U, padSide, splitStatus, partyPersist, displayName, platformID, chefAvatarID);
			if ((ulong)num >= (ulong)((long)ServerUserSystem.m_Users.Count))
			{
				ServerUserSystem.m_Users.Add(user3);
			}
			else
			{
				ServerUserSystem.m_Users.Insert((int)num, user3);
			}
			ServerUserSystem.UpdatePlayersColourAssignments();
			if (ServerUserSystem.OnUserAdded != null)
			{
				ServerUserSystem.OnUserAdded(user3);
			}
			ServerMessenger.UserAdded(num, user3);
			return user3;
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x000E17EC File Offset: 0x000DFBEC
		public static void RemoveUser(User user, bool fireUsersChanged = true)
		{
			User user2 = null;
			if (user.Split == User.SplitStatus.SplitPadHost)
			{
				FastList<User> users = ServerUserSystem.m_Users;
				User.MachineID machineId = ServerUserSystem.s_LocalMachineId;
				user2 = UserSystemUtils.FindUser(users, null, machineId, user.Engagement, TeamID.Count, User.SplitStatus.SplitPadGuest);
			}
			else if (user.Split == User.SplitStatus.SplitPadGuest)
			{
				FastList<User> users = ServerUserSystem.m_Users;
				User.MachineID machineId = ServerUserSystem.s_LocalMachineId;
				User user3 = UserSystemUtils.FindUser(users, null, machineId, user.Engagement, TeamID.Count, User.SplitStatus.SplitPadHost);
				if (user3 != null)
				{
					user3.Split = User.SplitStatus.NotSplit;
					user3.PadSide = PadSide.Both;
				}
			}
			int param = ServerUserSystem.m_Users.FindIndex((User x) => x == user);
			if (ServerUserSystem.m_Users.Remove(user))
			{
				if (user2 != null)
				{
					ServerUserSystem.m_Users.Remove(user2);
				}
				ServerUserSystem.UpdatePlayersColourAssignments();
				if (ServerUserSystem.OnUserRemoved != null)
				{
					ServerUserSystem.OnUserRemoved(user);
				}
				if (ServerUserSystem.OnUserRemovedWithIndex != null)
				{
					ServerUserSystem.OnUserRemovedWithIndex(user, param);
				}
				if (fireUsersChanged)
				{
					ServerMessenger.UsersChanged();
				}
			}
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x000E1910 File Offset: 0x000DFD10
		public static void LockEngagement()
		{
			ServerUserSystem.m_ignoreEngagement = true;
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x000E1918 File Offset: 0x000DFD18
		public static void UnlockEngagement()
		{
			ServerUserSystem.m_ignoreEngagement = false;
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x000E1920 File Offset: 0x000DFD20
		public void OnEngagementChanged(EngagementSlot _e, GamepadUser _prev, GamepadUser _new)
		{
			FastList<User> users = ServerUserSystem.m_Users;
			User.MachineID machineID = ServerUserSystem.s_LocalMachineId;
			User user = UserSystemUtils.FindUser(users, null, machineID, _e, TeamID.Count, User.SplitStatus.Count);
			if (user != null)
			{
				if (_new == null && _prev != null)
				{
					if (_e != EngagementSlot.One || !user.IsLocal)
					{
						if (!ServerUserSystem.m_ignoreEngagement)
						{
							ServerUserSystem.RemoveUser(user, true);
						}
					}
				}
				else
				{
					user.Engagement = _e;
				}
			}
			else if (null != _new && !ServerUserSystem.m_ignoreEngagement)
			{
				if (ConnectionModeSwitcher.GetRequestedConnectionState() == NetConnectionState.Offline)
				{
					bool bLocal = true;
					machineID = ServerUserSystem.s_LocalMachineId;
					IOnlineMultiplayerSessionUserId sessionId = null;
					ServerUserSystem.AddUser(bLocal, machineID, sessionId, 0U, 0U, _e, PadSide.Both, TeamID.None, User.PartyPersistance.Remain, null, 127U, null, User.SplitStatus.NotSplit);
				}
				else if (ServerUserSystem.m_PrivilegeChecker.GetStatus().GetProgress() != eConnectionModeSwitchProgress.InProgress && ConnectionModeSwitcher.GetStatus().GetProgress() == eConnectionModeSwitchProgress.Complete && ConnectionModeSwitcher.GetStatus().GetResult() == eConnectionModeSwitchResult.Success)
				{
					ServerUserSystem.m_PrivilegeChecker.TriggerDropIn();
				}
			}
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x000E1A24 File Offset: 0x000DFE24
		public static bool SplitUser(EngagementSlot _e)
		{
			if (!ConnectionStatus.IsInSession() && ConnectionModeSwitcher.GetRequestedConnectionState() == NetConnectionState.Offline)
			{
				FastList<User> users = ServerUserSystem.m_Users;
				User.MachineID machineID = ServerUserSystem.s_LocalMachineId;
				User user = UserSystemUtils.FindUser(users, null, machineID, _e, TeamID.Count, User.SplitStatus.NotSplit);
				if (user != null)
				{
					bool bLocal = true;
					machineID = ServerUserSystem.s_LocalMachineId;
					IOnlineMultiplayerSessionUserId sessionId = null;
					ServerUserSystem.AddUser(bLocal, machineID, sessionId, 0U, 0U, _e, PadSide.Both, TeamID.None, User.PartyPersistance.Remain, null, 127U, null, User.SplitStatus.NotSplit);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x000E1A8C File Offset: 0x000DFE8C
		public void ResetUsersToOfflineState()
		{
			UserSystemUtils.ResetUsersToOffline(ref ServerUserSystem.m_Users);
			ServerUserSystem.s_LocalMachineId = User.MachineID.One;
			for (int i = 0; i < 4; i++)
			{
				EngagementSlot engagementSlot = (EngagementSlot)i;
				GamepadUser user = this.m_IPlayerManager.GetUser(engagementSlot);
				if (user != null)
				{
					FastList<User> users = ServerUserSystem.m_Users;
					EngagementSlot engagementSlot2 = engagementSlot;
					User user2 = UserSystemUtils.FindUser(users, null, User.MachineID.Count, engagementSlot2, TeamID.Count, User.SplitStatus.Count);
					if (user2 != null)
					{
						user2.Machine = User.MachineID.One;
					}
					else
					{
						UserSystemUtils.ResetUsersToOffline(ref ClientUserSystem.m_Users);
						users = ClientUserSystem.m_Users;
						engagementSlot2 = engagementSlot;
						user2 = UserSystemUtils.FindUser(users, null, User.MachineID.Count, engagementSlot2, TeamID.Count, User.SplitStatus.Count);
						if (user2 != null)
						{
							bool bLocal = true;
							User.MachineID machine = ServerUserSystem.s_LocalMachineId;
							engagementSlot2 = engagementSlot;
							PadSide padSide = user2.PadSide;
							ServerUserSystem.AddUser(bLocal, machine, null, 0U, 0U, engagementSlot2, padSide, TeamID.None, User.PartyPersistance.Remain, null, user2.SelectedChefAvatar, null, User.SplitStatus.NotSplit);
						}
						else
						{
							bool bLocal = true;
							User.MachineID machine = ServerUserSystem.s_LocalMachineId;
							engagementSlot2 = engagementSlot;
							ServerUserSystem.AddUser(bLocal, machine, null, 0U, 0U, engagementSlot2, PadSide.Both, TeamID.None, User.PartyPersistance.Remain, null, 127U, null, User.SplitStatus.NotSplit);
						}
					}
				}
			}
			ServerMessenger.UsersChanged();
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x000E1B88 File Offset: 0x000DFF88
		public void ResetUsersToOnlineState(OnlineMultiplayerLocalUserId localUserId)
		{
			IOnlineMultiplayerSessionUserId sessionHostUser = UserSystemUtils.GetSessionHostUser();
			for (int i = 0; i < 4; i++)
			{
				EngagementSlot engagementSlot = (EngagementSlot)i;
				GamepadUser user = this.m_IPlayerManager.GetUser(engagementSlot);
				if (user != null)
				{
					OnlineMultiplayerLocalUserId allowedUser = PrivilegeCheckCache.GetAllowedUser(user);
					OnlineUserPlatformId platformID = (allowedUser == null) ? null : allowedUser.m_platformId;
					User user2 = UserSystemUtils.FindUser(ServerUserSystem.m_Users, null, ServerUserSystem.s_LocalMachineId, engagementSlot, TeamID.Count, User.SplitStatus.Count);
					if (user2 != null)
					{
						if (engagementSlot == EngagementSlot.One)
						{
							user2.SessionId = sessionHostUser;
						}
						user2.PlatformID = platformID;
					}
					else if (engagementSlot == EngagementSlot.One)
					{
						bool bLocal = true;
						User.MachineID machine = ServerUserSystem.s_LocalMachineId;
						IOnlineMultiplayerSessionUserId sessionId = sessionHostUser;
						EngagementSlot engagement = engagementSlot;
						ServerUserSystem.AddUser(bLocal, machine, sessionId, 0U, 0U, engagement, PadSide.Both, TeamID.None, User.PartyPersistance.Remain, platformID, 127U, null, User.SplitStatus.NotSplit);
					}
				}
			}
			ServerMessenger.UsersChanged();
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x000E1C54 File Offset: 0x000E0054
		public User AddNewRemoteUser(User.MachineID machineId, IOnlineMultiplayerSessionUserId sessionUser, JoinDataProvider.GameUserData gameData)
		{
			if (ServerUserSystem.m_Users.Count == 4)
			{
				return null;
			}
			bool bLocal = false;
			EngagementSlot slot = gameData.Slot;
			PadSide controllerSide = gameData.ControllerSide;
			uint avatarId = gameData.AvatarId;
			return ServerUserSystem.AddUser(bLocal, machineId, sessionUser, 0U, 0U, slot, controllerSide, TeamID.None, NetworkUtils.GetRemoteUserPartyPersistanceForJoinState(gameData.JoinMethod), gameData.PlatformId, avatarId, gameData.DisplayName, gameData.ControllerSplitStatus);
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x000E1CC4 File Offset: 0x000E00C4
		public static User.MachineID GetAvailableMachineId()
		{
			Array values = Enum.GetValues(typeof(User.MachineID));
			int length = values.Length;
			for (int i = 0; i < length; i++)
			{
				User.MachineID machineID = (User.MachineID)values.GetValue(i);
				FastList<User> users = ServerUserSystem.m_Users;
				User.MachineID machineId = machineID;
				if (UserSystemUtils.FindUser(users, null, machineId, EngagementSlot.Count, TeamID.Count, User.SplitStatus.Count) == null)
				{
					return machineID;
				}
			}
			return User.MachineID.Count;
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x000E1D30 File Offset: 0x000E0130
		private static void UpdatePlayersColourAssignments()
		{
			for (int i = 0; i < ServerUserSystem.m_Users.Count; i++)
			{
				ServerUserSystem.m_Users._items[i].Colour = (uint)i;
			}
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x000E1D6A File Offset: 0x000E016A
		public static IConnectionModeSwitchStatus GetEngagementPrivilegeCheckStatus()
		{
			return ServerUserSystem.m_PrivilegeChecker.GetStatus();
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x000E1D76 File Offset: 0x000E0176
		private void EngagementPrivilegeCheckStarted(IConnectionModeSwitchStatus status)
		{
			if (ServerUserSystem.OnEngagementPrivilegeCheckStarted != null)
			{
				ServerUserSystem.OnEngagementPrivilegeCheckStarted(status);
			}
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x000E1D8D File Offset: 0x000E018D
		private void EngagementPrivilegeCheckComplete(IConnectionModeSwitchStatus status)
		{
			if (ServerUserSystem.OnEngagementPrivilegeCheckCompleted != null)
			{
				ServerUserSystem.OnEngagementPrivilegeCheckCompleted(status);
			}
		}

		// Token: 0x04002677 RID: 9847
		public static User.MachineID s_LocalMachineId = User.MachineID.One;

		// Token: 0x04002678 RID: 9848
		public static FastList<User> m_Users = new FastList<User>(4);

		// Token: 0x04002679 RID: 9849
		private IPlayerManager m_IPlayerManager;

		// Token: 0x0400267A RID: 9850
		private static ServerDropInUserMonitor m_PrivilegeChecker = new ServerDropInUserMonitor();

		// Token: 0x0400267B RID: 9851
		private static bool m_ignoreEngagement = false;

		// Token: 0x0400267C RID: 9852
		public static GenericVoid<User> OnUserRemoved = null;

		// Token: 0x0400267D RID: 9853
		public static GenericVoid<User, int> OnUserRemovedWithIndex = null;

		// Token: 0x0400267E RID: 9854
		public static GenericVoid<User> OnUserAdded = null;

		// Token: 0x0400267F RID: 9855
		public static GenericVoid<IConnectionModeSwitchStatus> OnEngagementPrivilegeCheckStarted;

		// Token: 0x04002680 RID: 9856
		public static GenericVoid<IConnectionModeSwitchStatus> OnEngagementPrivilegeCheckCompleted;
	}
}
