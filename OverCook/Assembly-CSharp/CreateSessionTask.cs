using System;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer;

// Token: 0x02000866 RID: 2150
public class CreateSessionTask : IMultiplayerTask
{
	// Token: 0x06002973 RID: 10611 RVA: 0x000C25B8 File Offset: 0x000C09B8
	public void Initialise(Server server, Client client, OnlineMultiplayerSessionVisibility visibility, OnlineMultiplayerSessionPlayTogetherHosting playTogether, List<OnlineMultiplayerSessionPropertyValue> values)
	{
		this.m_Server = server;
		this.m_Client = client;
		this.m_Visibility = visibility;
		this.m_PlayTogether = playTogether;
		this.m_PropertyValues = values;
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_iOnlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
	}

	// Token: 0x06002974 RID: 10612 RVA: 0x000C25FC File Offset: 0x000C09FC
	public void Start(object startData)
	{
		OnlineMultiplayerLocalUserId onlineMultiplayerLocalUserId = startData as OnlineMultiplayerLocalUserId;
		if (onlineMultiplayerLocalUserId == null)
		{
			this.m_Status.sessionCreateResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionCreateResult>
			{
				m_returnCode = OnlineMultiplayerSessionCreateResult.eGenericFailure
			};
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Failure;
			return;
		}
		if (this.m_iOnlineMultiplayerSessionCoordinator == null)
		{
			this.m_Status.sessionCreateResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionCreateResult>
			{
				m_returnCode = OnlineMultiplayerSessionCreateResult.eGenericFailure
			};
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Failure;
			return;
		}
		if (this.m_LocalUserId == onlineMultiplayerLocalUserId)
		{
			if (this.m_iOnlineMultiplayerSessionCoordinator.IsHost() && this.m_Status.Progress == eConnectionModeSwitchProgress.Complete && this.m_Status.Result == eConnectionModeSwitchResult.Success)
			{
				return;
			}
			if (this.m_Status.Progress == eConnectionModeSwitchProgress.InProgress && this.m_Status.Result == eConnectionModeSwitchResult.NotAvailableYet)
			{
				return;
			}
		}
		if (!this.m_iOnlineMultiplayerSessionCoordinator.IsIdle())
		{
			this.m_Status.sessionCreateResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionCreateResult>
			{
				m_returnCode = OnlineMultiplayerSessionCreateResult.eGenericFailure
			};
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Failure;
			return;
		}
		this.m_LocalUserId = onlineMultiplayerLocalUserId;
		this.m_Status.currentUser = this.m_LocalUserId;
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
		this.m_bStarted = false;
		this.TryStart();
	}

	// Token: 0x06002975 RID: 10613 RVA: 0x000C2765 File Offset: 0x000C0B65
	public void Stop()
	{
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
	}

	// Token: 0x06002976 RID: 10614 RVA: 0x000C2780 File Offset: 0x000C0B80
	private void TryStart()
	{
		if (!this.m_bStarted)
		{
			string sessionName = Localization.Get("Online.RoomName", new LocToken[]
			{
				new LocToken("[HostName]", this.m_LocalUserId.m_userName)
			});
			this.m_bStarted = this.m_iOnlineMultiplayerSessionCoordinator.Create(this.m_LocalUserId, this.m_PropertyValues, this.m_Visibility, sessionName, this.m_PlayTogether, new OnlineMultiplayerSessionCreateCallback(this.OnlineMultiplayerSessionCreateCallback), new OnlineMultiplayerSessionJoinDecisionCallback(this.m_Server.OnlineMultiplayerSessionJoinDecisionCallback), new OnlineMultiplayerSessionUserJoinedCallback(this.m_Server.OnlineMultiplayerSessionUserJoinedCallback));
			if (this.m_bStarted)
			{
				this.m_Status.Progress = eConnectionModeSwitchProgress.InProgress;
				this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
			}
			else
			{
				this.m_Status.sessionCreateResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionCreateResult>
				{
					m_returnCode = OnlineMultiplayerSessionCreateResult.eGenericFailure
				};
				this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
				this.m_Status.Result = eConnectionModeSwitchResult.Failure;
			}
		}
	}

	// Token: 0x06002977 RID: 10615 RVA: 0x000C287B File Offset: 0x000C0C7B
	public void Update()
	{
		this.TryStart();
	}

	// Token: 0x06002978 RID: 10616 RVA: 0x000C2883 File Offset: 0x000C0C83
	public object GetData()
	{
		return (this.m_Status.Result != eConnectionModeSwitchResult.Success) ? null : this.m_LocalUserId;
	}

	// Token: 0x06002979 RID: 10617 RVA: 0x000C28A2 File Offset: 0x000C0CA2
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Status;
	}

	// Token: 0x0600297A RID: 10618 RVA: 0x000C28AC File Offset: 0x000C0CAC
	private void OnlineMultiplayerSessionCreateCallback(OnlineMultiplayerReturnCode<OnlineMultiplayerSessionCreateResult> result)
	{
		this.m_Status.sessionCreateResult = result;
		if (result != null && result.m_returnCode == OnlineMultiplayerSessionCreateResult.eSuccess)
		{
			NetworkSystemConfigurator.Server(this.m_Server, this.m_Client, this.m_LocalUserId);
			this.m_Status.Result = eConnectionModeSwitchResult.Success;
		}
		else
		{
			this.m_Status.Result = eConnectionModeSwitchResult.Failure;
		}
		this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
	}

	// Token: 0x040020BE RID: 8382
	private Server m_Server;

	// Token: 0x040020BF RID: 8383
	private Client m_Client;

	// Token: 0x040020C0 RID: 8384
	private OnlineMultiplayerSessionVisibility m_Visibility = OnlineMultiplayerSessionVisibility.eClosed;

	// Token: 0x040020C1 RID: 8385
	private OnlineMultiplayerSessionPlayTogetherHosting m_PlayTogether;

	// Token: 0x040020C2 RID: 8386
	private bool m_bStarted;

	// Token: 0x040020C3 RID: 8387
	private OnlineMultiplayerLocalUserId m_LocalUserId;

	// Token: 0x040020C4 RID: 8388
	private List<OnlineMultiplayerSessionPropertyValue> m_PropertyValues;

	// Token: 0x040020C5 RID: 8389
	private CreateSessionStatus m_Status = new CreateSessionStatus();

	// Token: 0x040020C6 RID: 8390
	private IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator;
}
