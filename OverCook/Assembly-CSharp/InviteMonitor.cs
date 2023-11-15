using System;
using System.Collections.Generic;
using Team17.Online;

// Token: 0x0200089A RID: 2202
public class InviteMonitor
{
	// Token: 0x06002AF5 RID: 10997 RVA: 0x000C9704 File Offset: 0x000C7B04
	public static bool CheckStatus(InviteMonitor.StatusFlags flags)
	{
		bool flag = true;
		if ((flags & InviteMonitor.StatusFlags.HandlerIsValid) == InviteMonitor.StatusFlags.HandlerIsValid && InviteMonitor.m_Handler == null)
		{
			flag = false;
		}
		if ((flags & InviteMonitor.StatusFlags.HandlerIsIdle) == InviteMonitor.StatusFlags.HandlerIsIdle && InviteMonitor.m_Handler != null)
		{
			flag &= !InviteMonitor.m_Handler.IsBusy();
		}
		if ((flags & InviteMonitor.StatusFlags.HandlerIsWaitingOnUserInput) == InviteMonitor.StatusFlags.HandlerIsWaitingOnUserInput)
		{
			flag = (InviteMonitor.m_Handler != null && (flag & InviteMonitor.m_Handler.IsAwaitingUserInput()));
		}
		if ((flags & InviteMonitor.StatusFlags.PendingInvite) == InviteMonitor.StatusFlags.PendingInvite)
		{
			IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
			if (onlinePlatformManager != null)
			{
				IOnlineMultiplayerGameInviteCoordinator onlineMultiplayerGameInviteCoordinator = onlinePlatformManager.OnlineMultiplayerGameInviteCoordinator();
				flag = (onlineMultiplayerGameInviteCoordinator != null && (flag & onlineMultiplayerGameInviteCoordinator.HasPendingAcceptedInvite()));
			}
			else
			{
				flag = false;
			}
		}
		return flag;
	}

	// Token: 0x06002AF6 RID: 10998 RVA: 0x000C97B4 File Offset: 0x000C7BB4
	public void Initialise()
	{
		this.m_saveManager = GameUtils.RequireManager<SaveManager>();
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_iOnlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		this.m_iOnlineMultiplayerGameInviteCoordinator = onlinePlatformManager.OnlineMultiplayerGameInviteCoordinator();
		this.m_LocalPlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		InviteMonitor.m_Handlers.Add(InviteMonitor.HandlerType.None, null);
		InviteMonitor.m_Handlers.Add(InviteMonitor.HandlerType.Frontend, InviteMonitor.m_FrontendHandler);
		InviteMonitor.m_Handlers.Add(InviteMonitor.HandlerType.Gameplay, InviteMonitor.m_GameplayHandler);
	}

	// Token: 0x06002AF7 RID: 10999 RVA: 0x000C9824 File Offset: 0x000C7C24
	public void Update()
	{
		if (this.m_LocalPlayerManager != null)
		{
			if (this.m_transitionManager == null)
			{
				this.m_transitionManager = GameUtils.RequestManager<ScreenTransitionManager>();
			}
			bool flag = this.m_transitionManager != null && !this.m_transitionManager.IsIdle;
			bool isLoading = LoadingScreenFlow.IsLoading;
			bool flag2 = T17DialogBoxManager.HasAnyOpenDialogs() || this.m_LocalPlayerManager.IsWarningActive(PlayerWarning.Disengaged);
			bool isSaving = this.m_saveManager.IsSaving;
			if (this.m_iOnlineMultiplayerGameInviteCoordinator != null && this.m_iOnlineMultiplayerSessionCoordinator != null && !flag && !isLoading && !isSaving && !flag2)
			{
				OnlineMultiplayerSessionInvite onlineMultiplayerSessionInvite = this.m_iOnlineMultiplayerGameInviteCoordinator.InviteAccepted();
				OnlineMultiplayerSessionInvite onlineMultiplayerSessionInvite2 = null;
				while (onlineMultiplayerSessionInvite != null)
				{
					onlineMultiplayerSessionInvite2 = onlineMultiplayerSessionInvite;
					onlineMultiplayerSessionInvite = this.m_iOnlineMultiplayerGameInviteCoordinator.InviteAccepted();
				}
				if (onlineMultiplayerSessionInvite2 != null && (!ConnectionStatus.IsInSession() || !this.m_iOnlineMultiplayerSessionCoordinator.IsMemberAlready(onlineMultiplayerSessionInvite2)))
				{
					InviteMonitor.m_AcceptedInvite = new AcceptInviteData();
					InviteMonitor.m_AcceptedInvite.Invite = onlineMultiplayerSessionInvite2;
				}
				if (InviteMonitor.m_Handler != null)
				{
					OnlineMultiplayerSessionPlayTogetherHosting onlineMultiplayerSessionPlayTogetherHosting = this.m_iOnlineMultiplayerGameInviteCoordinator.PlayTogetherHosting();
					if (onlineMultiplayerSessionPlayTogetherHosting != null)
					{
						IPlayerManager playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
						GamepadUser user = playerManager.GetUser(EngagementSlot.One);
						if (null != user && onlineMultiplayerSessionPlayTogetherHosting.WasAcceptedBy(user))
						{
							InviteMonitor.m_PlayTogetherHost = onlineMultiplayerSessionPlayTogetherHosting;
							InviteMonitor.m_Handler.HandlePlayTogetherHost(InviteMonitor.m_PlayTogetherHost);
						}
					}
				}
			}
			if (InviteMonitor.m_Handler != null)
			{
				if (!InviteMonitor.m_AcceptedInviteHandled && InviteMonitor.m_AcceptedInvite != null)
				{
					GamepadUser gamepadUser = null;
					EngagementSlot engagementSlot = EngagementSlot.Count;
					IPlayerManager playerManager2 = GameUtils.RequireManagerInterface<IPlayerManager>();
					for (int i = 0; i < 4; i++)
					{
						GamepadUser user2 = playerManager2.GetUser((EngagementSlot)i);
						if (null != user2 && InviteMonitor.m_AcceptedInvite.Invite.WasAcceptedBy(user2))
						{
							gamepadUser = user2;
							engagementSlot = (EngagementSlot)i;
							break;
						}
					}
					if (this.m_LocalPlayerManager.SupportsInvitesForAnyUser() || (null != gamepadUser && engagementSlot == EngagementSlot.One))
					{
						InviteMonitor.m_AcceptedInvite.User = gamepadUser;
						InviteMonitor.m_AcceptedInviteHandled = true;
						InviteMonitor.m_Handler.HandleAcceptedInvite(InviteMonitor.m_AcceptedInvite);
					}
					else
					{
						InviteMonitor.m_AcceptedInvite = null;
					}
				}
				InviteMonitor.m_Handler.Update();
			}
		}
	}

	// Token: 0x06002AF8 RID: 11000 RVA: 0x000C9A74 File Offset: 0x000C7E74
	public static AcceptInviteData GetAcceptedInvite()
	{
		return InviteMonitor.m_AcceptedInvite;
	}

	// Token: 0x06002AF9 RID: 11001 RVA: 0x000C9A7B File Offset: 0x000C7E7B
	public static OnlineMultiplayerSessionPlayTogetherHosting GetPlayTogetherHost()
	{
		return InviteMonitor.m_PlayTogetherHost;
	}

	// Token: 0x06002AFA RID: 11002 RVA: 0x000C9A82 File Offset: 0x000C7E82
	public static void ClearInvite()
	{
		InviteMonitor.m_AcceptedInvite = null;
		InviteMonitor.m_AcceptedInviteHandled = false;
	}

	// Token: 0x06002AFB RID: 11003 RVA: 0x000C9A90 File Offset: 0x000C7E90
	public static void ClearPlayTogetherHost()
	{
		InviteMonitor.m_PlayTogetherHost = null;
	}

	// Token: 0x06002AFC RID: 11004 RVA: 0x000C9A98 File Offset: 0x000C7E98
	public static void SwitchHandlerType(InviteMonitor.HandlerType type)
	{
		InviteHandler inviteHandler = InviteMonitor.m_Handlers[type];
		if (inviteHandler != InviteMonitor.m_Handler)
		{
			if (InviteMonitor.m_Handler != null)
			{
				InviteMonitor.m_Handler.Stop();
			}
			InviteMonitor.m_Handler = inviteHandler;
			InviteMonitor.m_HandlerType = type;
			if (inviteHandler != null)
			{
				inviteHandler.Start();
			}
		}
	}

	// Token: 0x040021DC RID: 8668
	public static GenericVoid InviteAccepted;

	// Token: 0x040021DD RID: 8669
	public static GenericVoid InviteJoinComplete;

	// Token: 0x040021DE RID: 8670
	private static bool m_AcceptedInviteHandled = false;

	// Token: 0x040021DF RID: 8671
	private static AcceptInviteData m_AcceptedInvite = null;

	// Token: 0x040021E0 RID: 8672
	private static OnlineMultiplayerSessionPlayTogetherHosting m_PlayTogetherHost = null;

	// Token: 0x040021E1 RID: 8673
	private IOnlineMultiplayerGameInviteCoordinator m_iOnlineMultiplayerGameInviteCoordinator;

	// Token: 0x040021E2 RID: 8674
	private IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator;

	// Token: 0x040021E3 RID: 8675
	private SaveManager m_saveManager;

	// Token: 0x040021E4 RID: 8676
	private ScreenTransitionManager m_transitionManager;

	// Token: 0x040021E5 RID: 8677
	private IPlayerManager m_LocalPlayerManager;

	// Token: 0x040021E6 RID: 8678
	private static InviteHandler m_Handler = null;

	// Token: 0x040021E7 RID: 8679
	private static InviteMonitor.HandlerType m_HandlerType = InviteMonitor.HandlerType.None;

	// Token: 0x040021E8 RID: 8680
	private static FrontendInviteHandler m_FrontendHandler = new FrontendInviteHandler();

	// Token: 0x040021E9 RID: 8681
	private static GameplayInviteHandler m_GameplayHandler = new GameplayInviteHandler();

	// Token: 0x040021EA RID: 8682
	private static Dictionary<InviteMonitor.HandlerType, InviteHandler> m_Handlers = new Dictionary<InviteMonitor.HandlerType, InviteHandler>();

	// Token: 0x0200089B RID: 2203
	public enum HandlerType
	{
		// Token: 0x040021EC RID: 8684
		None,
		// Token: 0x040021ED RID: 8685
		Frontend,
		// Token: 0x040021EE RID: 8686
		Gameplay
	}

	// Token: 0x0200089C RID: 2204
	[Flags]
	public enum StatusFlags
	{
		// Token: 0x040021F0 RID: 8688
		HandlerIsValid = 1,
		// Token: 0x040021F1 RID: 8689
		HandlerIsIdle = 2,
		// Token: 0x040021F2 RID: 8690
		HandlerIsWaitingOnUserInput = 4,
		// Token: 0x040021F3 RID: 8691
		PendingInvite = 8
	}
}
