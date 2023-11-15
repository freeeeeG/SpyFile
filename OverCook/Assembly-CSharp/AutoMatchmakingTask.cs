using System;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer;

// Token: 0x02000863 RID: 2147
public class AutoMatchmakingTask : JoinSessionBaseTask
{
	// Token: 0x0600295D RID: 10589 RVA: 0x000C1FEA File Offset: 0x000C03EA
	public void Initialise(List<OnlineMultiplayerSessionPropertySearchValue> filterParameter, Server server, Client client)
	{
		this.m_FilterParameter = filterParameter;
		this.m_Server = server;
		if (!(this.m_Status is AutoMatchmakingStatus))
		{
			this.m_Status = new AutoMatchmakingStatus();
		}
	}

	// Token: 0x0600295E RID: 10590 RVA: 0x000C2018 File Offset: 0x000C0418
	public override void Start(object startData)
	{
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_iOnlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		if (!this.m_iOnlineMultiplayerSessionCoordinator.IsIdle())
		{
			this.m_Status.sessionJoinResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult>
			{
				m_returnCode = OnlineMultiplayerSessionJoinResult.eGenericFailure
			};
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Failure;
			return;
		}
		this.m_LocalUser = (startData as OnlineMultiplayerLocalUserId);
		this.m_Status.currentUser = this.m_LocalUser;
		base.Start(startData);
	}

	// Token: 0x0600295F RID: 10591 RVA: 0x000C20A0 File Offset: 0x000C04A0
	public override void TryStart()
	{
		if (this.m_iOnlineMultiplayerSessionCoordinator.IsIdle())
		{
			List<OnlineMultiplayerSessionPropertyValue> values = ServerSessionPropertyValuesProvider.GetValues();
			for (int i = 0; i < values.Count; i++)
			{
			}
			for (int j = 0; j < this.m_FilterParameter.Count; j++)
			{
			}
			OnlineMultiplayerSessionJoinLocalUserData onlineMultiplayerSessionJoinLocalUserData = JoinDataProvider.BuildJoinRequestData(EngagementSlot.One, NetConnectionState.Matchmake, this.m_LocalUser);
			if (onlineMultiplayerSessionJoinLocalUserData != null && this.m_iOnlineMultiplayerSessionCoordinator.AutoMatchmake(onlineMultiplayerSessionJoinLocalUserData, ServerSessionPropertyValuesProvider.GetValues(), "placeholder room name", new OnlineMultiplayerSessionJoinDecisionCallback(this.m_Server.OnlineMultiplayerSessionJoinDecisionCallback), new OnlineMultiplayerSessionUserJoinedCallback(this.m_Server.OnlineMultiplayerSessionUserJoinedCallback), this.m_FilterParameter, new OnlineMultiplayerSessionJoinCallback(base.OnlineMultiplayerSessionJoinCallback)))
			{
				this.m_Status.Progress = eConnectionModeSwitchProgress.InProgress;
				this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
			}
			else
			{
				this.m_Status.sessionJoinResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult>
				{
					m_returnCode = OnlineMultiplayerSessionJoinResult.eGenericFailure
				};
				this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
				this.m_Status.Result = eConnectionModeSwitchResult.Failure;
			}
		}
	}

	// Token: 0x040020AB RID: 8363
	private IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator;

	// Token: 0x040020AC RID: 8364
	private OnlineMultiplayerLocalUserId m_LocalUser;

	// Token: 0x040020AD RID: 8365
	private List<OnlineMultiplayerSessionPropertySearchValue> m_FilterParameter;

	// Token: 0x040020AE RID: 8366
	private Server m_Server;
}
