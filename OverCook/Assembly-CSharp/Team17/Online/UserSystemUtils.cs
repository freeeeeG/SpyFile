using System;
using System.Collections.Generic;

namespace Team17.Online
{
	// Token: 0x0200099C RID: 2460
	public class UserSystemUtils
	{
		// Token: 0x06003026 RID: 12326 RVA: 0x000E2284 File Offset: 0x000E0684
		public static void Initialise()
		{
			IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
			UserSystemUtils.m_iOnlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		}

		// Token: 0x06003027 RID: 12327 RVA: 0x000E22A4 File Offset: 0x000E06A4
		public static User FindUser(FastList<User> users, IOnlineMultiplayerSessionUserId sessionId = null, User.MachineID machineId = User.MachineID.Count, EngagementSlot slot = EngagementSlot.Count, TeamID team = TeamID.Count, User.SplitStatus splitStatus = User.SplitStatus.Count)
		{
			User[] array = UserSystemUtils.FindUsers(users, sessionId, machineId, slot, team, splitStatus);
			if (array.Length > 0)
			{
				return array[0];
			}
			return null;
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x000E22D0 File Offset: 0x000E06D0
		public static User[] FindUsers(FastList<User> users, IOnlineMultiplayerSessionUserId sessionId = null, User.MachineID machineId = User.MachineID.Count, EngagementSlot slot = EngagementSlot.Count, TeamID team = TeamID.Count, User.SplitStatus splitStatus = User.SplitStatus.Count)
		{
			if (users == null)
			{
				return null;
			}
			List<User> list = new List<User>();
			int count = users.Count;
			for (int i = 0; i < count; i++)
			{
				User user = users._items[i];
				if (slot == EngagementSlot.Count || slot == user.Engagement)
				{
					if (sessionId == null || sessionId == user.SessionId)
					{
						if (machineId == User.MachineID.Count || machineId == user.Machine)
						{
							if (slot == EngagementSlot.Count || slot == user.Engagement)
							{
								if (team == TeamID.Count || team == user.Team)
								{
									if (splitStatus == User.SplitStatus.Count || splitStatus == user.Split)
									{
										list.Add(user);
									}
								}
							}
						}
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x000E23B0 File Offset: 0x000E07B0
		public static void ChangeGameState(GameState state, GameStateMessage.GameStatePayload payload = null)
		{
			FastList<User> users = ServerUserSystem.m_Users;
			int count = users.Count;
			for (int i = 0; i < count; i++)
			{
				User user = users._items[i];
				user.GameState = state;
			}
			ServerMessenger.GameState(state, payload);
			if (UserSystemUtils.OnServerChangedGameState != null)
			{
				UserSystemUtils.OnServerChangedGameState(state, payload);
			}
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x000E240C File Offset: 0x000E080C
		public static bool AreAllUsersInGameState(FastList<User> users, GameState state)
		{
			int count = users.Count;
			for (int i = 0; i < count; i++)
			{
				User user = users._items[i];
				if (user.GameState != state)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x000E244C File Offset: 0x000E084C
		public static bool AreAnyUsersInGameState(FastList<User> users, GameState state)
		{
			int count = users.Count;
			for (int i = 0; i < count; i++)
			{
				User user = users._items[i];
				if (user.GameState == state)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x000E248C File Offset: 0x000E088C
		public static void ResetUsersToOffline(ref FastList<User> users)
		{
			FastList<User> fastList = new FastList<User>(4);
			int count = users.Count;
			for (int i = 0; i < count; i++)
			{
				User user = users._items[i];
				if (user.IsLocal)
				{
					user.Machine = User.MachineID.One;
					user.SessionId = null;
					user.EntityID = 0U;
					user.Entity2ID = 0U;
					user.Team = TeamID.None;
				}
				else
				{
					fastList.Add(user);
				}
			}
			for (int j = 0; j < fastList.Count; j++)
			{
				users.Remove(fastList._items[j]);
			}
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x000E252C File Offset: 0x000E092C
		public static IOnlineMultiplayerSessionUserId GetSessionLocalUser()
		{
			if (UserSystemUtils.m_iOnlineMultiplayerSessionCoordinator != null)
			{
				IOnlineMultiplayerSessionUserId[] array = UserSystemUtils.m_iOnlineMultiplayerSessionCoordinator.Members();
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].IsLocal)
						{
							return array[i];
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x000E257C File Offset: 0x000E097C
		public static IOnlineMultiplayerSessionUserId GetSessionHostUser()
		{
			if (UserSystemUtils.m_iOnlineMultiplayerSessionCoordinator != null)
			{
				IOnlineMultiplayerSessionUserId[] array = UserSystemUtils.m_iOnlineMultiplayerSessionCoordinator.Members();
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].IsHost)
						{
							return array[i];
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x000E25CC File Offset: 0x000E09CC
		public static IOnlineMultiplayerSessionUserId GetSessionUserFromUniqueId(FastList<User> users, byte uniqueId)
		{
			IOnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId = null;
			if (UserSystemUtils.m_iOnlineMultiplayerSessionCoordinator != null)
			{
				IOnlineMultiplayerSessionUserId[] array = UserSystemUtils.m_iOnlineMultiplayerSessionCoordinator.Members();
				if (array != null)
				{
					onlineMultiplayerSessionUserId = Array.Find<IOnlineMultiplayerSessionUserId>(array, (IOnlineMultiplayerSessionUserId x) => x != null && x.UniqueId == uniqueId);
					if (onlineMultiplayerSessionUserId != null)
					{
					}
				}
			}
			return onlineMultiplayerSessionUserId;
		}

		// Token: 0x06003030 RID: 12336 RVA: 0x000E2620 File Offset: 0x000E0A20
		public static bool AnyRemoteUsers()
		{
			if (UserSystemUtils.m_iOnlineMultiplayerSessionCoordinator != null)
			{
				IOnlineMultiplayerSessionUserId[] array = UserSystemUtils.m_iOnlineMultiplayerSessionCoordinator.Members();
				if (array != null)
				{
					IOnlineMultiplayerSessionUserId onlineMultiplayerSessionUserId = Array.Find<IOnlineMultiplayerSessionUserId>(array, (IOnlineMultiplayerSessionUserId x) => x != null && !x.IsLocal);
					if (onlineMultiplayerSessionUserId != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003031 RID: 12337 RVA: 0x000E2678 File Offset: 0x000E0A78
		public static bool AnySplitPadUsers()
		{
			FastList<User> users = ClientUserSystem.m_Users;
			for (int i = 0; i < users.Count; i++)
			{
				if (users._items[i].Split == User.SplitStatus.SplitPadHost || users._items[i].Split == User.SplitStatus.SplitPadGuest)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x000E26CC File Offset: 0x000E0ACC
		public static uint LocalUserCount(FastList<User> users, bool includeSplitPadGuests)
		{
			uint num = 0U;
			int count = users.Count;
			for (int i = 0; i < count; i++)
			{
				User user = users._items[i];
				if (user.IsLocal)
				{
					if (user.Split == User.SplitStatus.SplitPadGuest)
					{
						if (includeSplitPadGuests)
						{
							num += 1U;
						}
					}
					else
					{
						num += 1U;
					}
				}
			}
			return num;
		}

		// Token: 0x06003033 RID: 12339 RVA: 0x000E2728 File Offset: 0x000E0B28
		public static bool AtMaxUserCount()
		{
			return ServerUserSystem.m_Users.Count == 4;
		}

		// Token: 0x06003034 RID: 12340 RVA: 0x000E2738 File Offset: 0x000E0B38
		public static void BuildGameInputConfig()
		{
			PlayerManager playerManager = GameUtils.RequireManager<PlayerManager>();
			if (playerManager == null)
			{
				return;
			}
			List<GameInputConfig.ConfigEntry> list = new List<GameInputConfig.ConfigEntry>();
			FastList<User> users = ClientUserSystem.m_Users;
			FastList<User> fastList = new FastList<User>();
			FastList<User> fastList2 = new FastList<User>();
			for (int i = 0; i < users.Count; i++)
			{
				User user = users._items[i];
				if (user.IsLocal)
				{
					fastList.Add(user);
				}
				else
				{
					fastList2.Add(user);
				}
			}
			for (int j = 0; j < fastList.Count; j++)
			{
				UserSystemUtils.SetupUserConfig(playerManager, ref list, fastList._items[j], j);
			}
			for (int k = 0; k < fastList2.Count; k++)
			{
				UserSystemUtils.SetupUserConfig(playerManager, ref list, fastList2._items[k], fastList.Count + k);
			}
			PadSplitManager.FixupConfigList(list, playerManager.UnsidedAmbiMapping);
			GameInputConfig baseInputConfig = new GameInputConfig(list.ToArray());
			PlayerInputLookup.SetBaseInputConfig(baseInputConfig);
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x000E2840 File Offset: 0x000E0C40
		private static void SetupUserConfig(PlayerManager _playerManager, ref List<GameInputConfig.ConfigEntry> _configEntries, User _user, int _index)
		{
			ControlPadInput.PadNum engagement = (ControlPadInput.PadNum)_user.Engagement;
			PadSide padSide = _user.PadSide;
			User.MachineID machine = _user.Machine;
			AmbiControlsMappingData mappingData = (_user.PadSide != PadSide.Both) ? _playerManager.SidedAmbiMapping : _playerManager.UnsidedAmbiMapping;
			GameInputConfig.ConfigEntry item = new GameInputConfig.ConfigEntry((PlayerInputLookup.Player)_index, engagement, padSide, machine, mappingData);
			_configEntries.Add(item);
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x000E289C File Offset: 0x000E0C9C
		public static void RemoveAllSplitPadGuestUsers()
		{
			FastList<User> users = ServerUserSystem.m_Users;
			FastList<User> fastList = new FastList<User>();
			for (int i = 0; i < users.Count; i++)
			{
				if (users._items[i].Split == User.SplitStatus.SplitPadGuest)
				{
					fastList.Add(users._items[i]);
				}
			}
			for (int j = 0; j < fastList.Count; j++)
			{
				ServerUserSystem.RemoveUser(fastList._items[j], true);
			}
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x000E2914 File Offset: 0x000E0D14
		public static void DisengageNonRequiredUsersForOnline(bool allowAllActiveLocalUsers)
		{
			UserSystemUtils.RemoveAllSplitPadGuestUsers();
			if (!allowAllActiveLocalUsers)
			{
				bool engagementsLocked = ServerUserSystem.EngagementsLocked;
				ServerUserSystem.UnlockEngagement();
				IPlayerManager playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
				FastList<User> users = ServerUserSystem.m_Users;
				while (users.Count > 1)
				{
					playerManager.DisengagePad(users._items[users.Count - 1].Engagement);
				}
				if (engagementsLocked)
				{
					ServerUserSystem.LockEngagement();
				}
			}
		}

		// Token: 0x040026A4 RID: 9892
		public const int kMaxUsers = 4;

		// Token: 0x040026A5 RID: 9893
		private static IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator;

		// Token: 0x040026A6 RID: 9894
		public static GenericVoid<GameState, GameStateMessage.GameStatePayload> OnServerChangedGameState;
	}
}
