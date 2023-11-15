using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200088A RID: 2186
internal class DisconnectionHandler
{
	// Token: 0x06002A6B RID: 10859 RVA: 0x000C6BFC File Offset: 0x000C4FFC
	public void Initialise()
	{
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		DisconnectionHandler.m_iOnlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		this.m_connectionModeCoordinator = onlinePlatformManager.OnlineMultiplayerConnectionModeCoordinator();
		if (DisconnectionHandler.m_iOnlineMultiplayerSessionCoordinator != null)
		{
			DisconnectionHandler.m_iOnlineMultiplayerSessionCoordinator.RegisterDisconnectionCallback(new OnlineMultiplayerSessionDisconnectionCallback(this.HandleLocalDisconnection));
			DisconnectionHandler.m_iOnlineMultiplayerSessionCoordinator.RegisterRemoteUserDisconnectionCallback(new OnlineMultiplayerSessionRemoteUserDisconnectionCallback(this.HandleRemoteDisconnection));
		}
		if (this.m_connectionModeCoordinator != null)
		{
			this.m_connectionModeCoordinator.RegisterErrorCallback(new OnlineMultiplayerConnectionModeErrorCallback(this.OnlineMultiplayerConnectionModeErrorCallback));
		}
		this.m_OnUsersChanged = new GenericVoid(this.OnClientUsersChanged);
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Combine(ClientUserSystem.usersChanged, this.m_OnUsersChanged);
		ServerUserSystem.OnUserRemoved = (GenericVoid<User>)Delegate.Combine(ServerUserSystem.OnUserRemoved, new GenericVoid<User>(this.OnServerUserRemoved));
	}

	// Token: 0x06002A6C RID: 10860 RVA: 0x000C6CCC File Offset: 0x000C50CC
	public void Shutdown()
	{
		if (DisconnectionHandler.m_iOnlineMultiplayerSessionCoordinator != null)
		{
			DisconnectionHandler.m_iOnlineMultiplayerSessionCoordinator.UnRegisterDisconnectionCallback(new OnlineMultiplayerSessionDisconnectionCallback(this.HandleLocalDisconnection));
			DisconnectionHandler.m_iOnlineMultiplayerSessionCoordinator.UnRegisterRemoteUserDisconnectionCallback(new OnlineMultiplayerSessionRemoteUserDisconnectionCallback(this.HandleRemoteDisconnection));
		}
		if (this.m_connectionModeCoordinator != null)
		{
			this.m_connectionModeCoordinator.UnRegisterErrorCallback(new OnlineMultiplayerConnectionModeErrorCallback(this.OnlineMultiplayerConnectionModeErrorCallback));
		}
		DisconnectionHandler.m_iOnlineMultiplayerSessionCoordinator = null;
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, this.m_OnUsersChanged);
		ServerUserSystem.OnUserRemoved = (GenericVoid<User>)Delegate.Remove(ServerUserSystem.OnUserRemoved, new GenericVoid<User>(this.OnServerUserRemoved));
	}

	// Token: 0x06002A6D RID: 10861 RVA: 0x000C6D71 File Offset: 0x000C5171
	public void HandleSessionConnectionLost()
	{
		if (ConnectionModeSwitcher.GetRequestedConnectionState() == NetConnectionState.Offline || ConnectionModeSwitcher.GetStatus().GetProgress() != eConnectionModeSwitchProgress.Complete)
		{
			return;
		}
		DisconnectionHandler.m_requestedOfflineMode = ConnectionStatus.CurrentConnectionMode();
		DisconnectionHandler.GoOffline(new GenericVoid<IConnectionModeSwitchStatus>(this.FireSessionConnectionLostEvent), DisconnectionHandler.m_requestedOfflineMode);
	}

	// Token: 0x06002A6E RID: 10862 RVA: 0x000C6DB0 File Offset: 0x000C51B0
	private void FireSessionConnectionLostEvent(IConnectionModeSwitchStatus result)
	{
		if (result.GetResult() != eConnectionModeSwitchResult.Success && (!ConnectionStatus.HasConnectionModes() || (ConnectionStatus.HasConnectionModes() && DisconnectionHandler.m_requestedOfflineMode != OnlineMultiplayerConnectionMode.eNone)))
		{
			DisconnectionHandler.m_requestedOfflineMode = OnlineMultiplayerConnectionMode.eNone;
			DisconnectionHandler.GoOffline(new GenericVoid<IConnectionModeSwitchStatus>(this.FireSessionConnectionLostEvent), DisconnectionHandler.m_requestedOfflineMode);
			return;
		}
		if (DisconnectionHandler.SessionConnectionLostEvent != null)
		{
			DisconnectionHandler.SessionConnectionLostEvent();
		}
	}

	// Token: 0x06002A6F RID: 10863 RVA: 0x000C6E18 File Offset: 0x000C5218
	public void OnlineMultiplayerConnectionModeErrorCallback(OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult> result)
	{
		if (ConnectionModeSwitcher.GetRequestedConnectionState() == NetConnectionState.Offline)
		{
			return;
		}
		DisconnectionHandler.m_LastConnectionModeError = result;
		DisconnectionHandler.GoOffline(new GenericVoid<IConnectionModeSwitchStatus>(this.FireConnectionModeErrorEvent), OnlineMultiplayerConnectionMode.eNone);
	}

	// Token: 0x06002A70 RID: 10864 RVA: 0x000C6E3D File Offset: 0x000C523D
	private void FireConnectionModeErrorEvent(IConnectionModeSwitchStatus result)
	{
		if (DisconnectionHandler.ConnectionModeErrorEvent != null)
		{
			DisconnectionHandler.ConnectionModeErrorEvent(DisconnectionHandler.m_LastConnectionModeError);
		}
	}

	// Token: 0x06002A71 RID: 10865 RVA: 0x000C6E58 File Offset: 0x000C5258
	public void HandleLocalDisconnection(OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult> result)
	{
		if (ConnectionModeSwitcher.GetRequestedConnectionState() == NetConnectionState.Offline)
		{
			return;
		}
		DisconnectionHandler.m_LastSessionConnectionLostError = result;
		DisconnectionHandler.m_requestedOfflineMode = ConnectionStatus.CurrentConnectionMode();
		DisconnectionHandler.GoOffline(new GenericVoid<IConnectionModeSwitchStatus>(this.FireLocalDisconnectionEvent), DisconnectionHandler.m_requestedOfflineMode);
	}

	// Token: 0x06002A72 RID: 10866 RVA: 0x000C6E8C File Offset: 0x000C528C
	private void FireLocalDisconnectionEvent(IConnectionModeSwitchStatus result)
	{
		if (result.GetResult() != eConnectionModeSwitchResult.Success && (!ConnectionStatus.HasConnectionModes() || (ConnectionStatus.HasConnectionModes() && DisconnectionHandler.m_requestedOfflineMode != OnlineMultiplayerConnectionMode.eNone)))
		{
			DisconnectionHandler.m_requestedOfflineMode = OnlineMultiplayerConnectionMode.eNone;
			DisconnectionHandler.GoOffline(new GenericVoid<IConnectionModeSwitchStatus>(this.FireLocalDisconnectionEvent), DisconnectionHandler.m_requestedOfflineMode);
			return;
		}
		if (DisconnectionHandler.LocalDisconnectionEvent != null)
		{
			DisconnectionHandler.LocalDisconnectionEvent(DisconnectionHandler.m_LastSessionConnectionLostError);
		}
	}

	// Token: 0x06002A73 RID: 10867 RVA: 0x000C6EFC File Offset: 0x000C52FC
	private static void GoOffline(GenericVoid<IConnectionModeSwitchStatus> callback, OnlineMultiplayerConnectionMode offlineMode)
	{
		UserSystemUtils.ResetUsersToOffline(ref ServerUserSystem.m_Users);
		UserSystemUtils.ResetUsersToOffline(ref ClientUserSystem.m_Users);
		ServerUserSystem.s_LocalMachineId = User.MachineID.One;
		ClientUserSystem.s_LocalMachineId = User.MachineID.One;
		IPlayerManager playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		OfflineOptions offlineOptions = new OfflineOptions
		{
			hostUser = playerManager.GetUser(EngagementSlot.One),
			eAdditionalAction = OfflineOptions.AdditionalAction.None,
			connectionMode = new OnlineMultiplayerConnectionMode?(offlineMode)
		};
		ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, offlineOptions, callback);
	}

	// Token: 0x06002A74 RID: 10868 RVA: 0x000C6F6C File Offset: 0x000C536C
	public void HandleRemoteDisconnection(IOnlineMultiplayerSessionUserId leavingUserId)
	{
		if (ConnectionModeSwitcher.GetRequestedConnectionState() == NetConnectionState.Offline)
		{
			return;
		}
		User user = UserSystemUtils.FindUser(ServerUserSystem.m_Users, leavingUserId, User.MachineID.Count, EngagementSlot.Count, TeamID.Count, User.SplitStatus.Count);
		if (user != null)
		{
			FastList<User> users = ServerUserSystem.m_Users;
			User.MachineID machine = user.Machine;
			User[] array = UserSystemUtils.FindUsers(users, null, machine, EngagementSlot.Count, TeamID.Count, User.SplitStatus.Count);
			if (array != null && array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					ServerUserSystem.RemoveUser(array[i], i == array.Length - 1);
				}
			}
		}
	}

	// Token: 0x06002A75 RID: 10869 RVA: 0x000C6FEC File Offset: 0x000C53EC
	public static void HandleKickMessage()
	{
		DisconnectionHandler.m_requestedOfflineMode = ConnectionStatus.CurrentConnectionMode();
		if (DisconnectionHandler.<>f__mg$cache0 == null)
		{
			DisconnectionHandler.<>f__mg$cache0 = new GenericVoid<IConnectionModeSwitchStatus>(DisconnectionHandler.FireKickedFromSessionEvent);
		}
		DisconnectionHandler.GoOffline(DisconnectionHandler.<>f__mg$cache0, DisconnectionHandler.m_requestedOfflineMode);
	}

	// Token: 0x06002A76 RID: 10870 RVA: 0x000C7020 File Offset: 0x000C5420
	private static void FireKickedFromSessionEvent(IConnectionModeSwitchStatus result)
	{
		if (result.GetResult() != eConnectionModeSwitchResult.Success && (!ConnectionStatus.HasConnectionModes() || (ConnectionStatus.HasConnectionModes() && DisconnectionHandler.m_requestedOfflineMode != OnlineMultiplayerConnectionMode.eNone)))
		{
			DisconnectionHandler.m_requestedOfflineMode = OnlineMultiplayerConnectionMode.eNone;
			if (DisconnectionHandler.<>f__mg$cache1 == null)
			{
				DisconnectionHandler.<>f__mg$cache1 = new GenericVoid<IConnectionModeSwitchStatus>(DisconnectionHandler.FireKickedFromSessionEvent);
			}
			DisconnectionHandler.GoOffline(DisconnectionHandler.<>f__mg$cache1, DisconnectionHandler.m_requestedOfflineMode);
			return;
		}
		if (GameUtils.RequireManager<PlayerManager>().HasPlayer() && DisconnectionHandler.KickedFromSessionEvent != null)
		{
			DisconnectionHandler.KickedFromSessionEvent();
		}
	}

	// Token: 0x06002A77 RID: 10871 RVA: 0x000C70A8 File Offset: 0x000C54A8
	public void OnServerUserRemoved(User removedUser)
	{
		this.DeleteChef(removedUser.EntityID);
		this.DeleteChef(removedUser.Entity2ID);
	}

	// Token: 0x06002A78 RID: 10872 RVA: 0x000C70C4 File Offset: 0x000C54C4
	private void DeleteChef(uint uEntityID)
	{
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(uEntityID);
		if (entry != null && null != entry.m_GameObject)
		{
			DisconnectionHandler.OnChefBeingDeleted(entry);
			IPlayerCarrier playerCarrier = entry.m_GameObject.RequestInterface<IPlayerCarrier>();
			if (playerCarrier != null)
			{
				for (int i = 0; i < 2; i++)
				{
					if (playerCarrier.InspectCarriedItem((PlayerAttachTarget)i) != null)
					{
						playerCarrier.TakeItem((PlayerAttachTarget)i);
					}
				}
			}
			ServerMessenger.DestroyChef(entry.m_GameObject);
		}
	}

	// Token: 0x06002A79 RID: 10873 RVA: 0x000C7144 File Offset: 0x000C5544
	public void OnClientUsersChanged()
	{
		bool flag = false;
		for (int i = 0; i < ClientUserSystem.m_Users.Count; i++)
		{
			if (ClientUserSystem.m_Users._items[i].IsLocal)
			{
				flag = true;
				break;
			}
		}
		if (this.m_bAnyLocalUsers && !flag && ConnectionModeSwitcher.GetRequestedConnectionState() != NetConnectionState.Offline)
		{
			DisconnectionHandler.HandleKickMessage();
		}
		this.m_bAnyLocalUsers = flag;
	}

	// Token: 0x04002173 RID: 8563
	private static IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator;

	// Token: 0x04002174 RID: 8564
	private IOnlineMultiplayerConnectionModeCoordinator m_connectionModeCoordinator;

	// Token: 0x04002175 RID: 8565
	private GenericVoid m_OnUsersChanged;

	// Token: 0x04002176 RID: 8566
	private bool m_bAnyLocalUsers;

	// Token: 0x04002177 RID: 8567
	public static GenericVoid SessionConnectionLostEvent;

	// Token: 0x04002178 RID: 8568
	public static GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>> ConnectionModeErrorEvent;

	// Token: 0x04002179 RID: 8569
	public static GenericVoid KickedFromSessionEvent;

	// Token: 0x0400217A RID: 8570
	public static GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>> LocalDisconnectionEvent;

	// Token: 0x0400217B RID: 8571
	private static OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult> m_LastConnectionModeError;

	// Token: 0x0400217C RID: 8572
	private static OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult> m_LastSessionConnectionLostError;

	// Token: 0x0400217D RID: 8573
	private static OnlineMultiplayerConnectionMode m_requestedOfflineMode;

	// Token: 0x0400217E RID: 8574
	public static GenericVoid<EntitySerialisationEntry> OnChefBeingDeleted;

	// Token: 0x0400217F RID: 8575
	[CompilerGenerated]
	private static GenericVoid<IConnectionModeSwitchStatus> <>f__mg$cache0;

	// Token: 0x04002180 RID: 8576
	[CompilerGenerated]
	private static GenericVoid<IConnectionModeSwitchStatus> <>f__mg$cache1;
}
