using System;
using System.Collections.Generic;
using Team17.Online;

// Token: 0x02000868 RID: 2152
public class JoinEnumeratedRoomTask : JoinSessionBaseTask
{
	// Token: 0x06002987 RID: 10631 RVA: 0x000C2D0F File Offset: 0x000C110F
	public void Initialise(OnlineMultiplayerSessionEnumeratedRoom room, NetConnectionState requestingAgent)
	{
		this.m_localUsersJoinData.Clear();
		this.m_Status.currentUser = null;
		this.m_room = room;
		this.m_requestingAgent = requestingAgent;
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
	}

	// Token: 0x06002988 RID: 10632 RVA: 0x000C2D50 File Offset: 0x000C1150
	public override void Start(object startData)
	{
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_iOnlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		List<JoinSessionBaseTask.UserData> list = startData as List<JoinSessionBaseTask.UserData>;
		if (list != null && list.Count > 0)
		{
			this.m_Status.currentUser = list[0].UserId;
			bool flag = false;
			int num = 0;
			while (num < list.Count && !flag)
			{
				OnlineMultiplayerSessionJoinLocalUserData onlineMultiplayerSessionJoinLocalUserData = JoinDataProvider.BuildJoinRequestData(list[num].Slot, this.m_requestingAgent, list[num].UserId);
				if (onlineMultiplayerSessionJoinLocalUserData == null)
				{
					this.m_localUsersJoinData.Clear();
					break;
				}
				this.m_localUsersJoinData.Add(onlineMultiplayerSessionJoinLocalUserData);
				num++;
			}
			list.Clear();
		}
		base.Start(startData);
	}

	// Token: 0x06002989 RID: 10633 RVA: 0x000C2E1C File Offset: 0x000C121C
	public override void TryStart()
	{
		if (this.m_iOnlineMultiplayerSessionCoordinator.IsIdle())
		{
			int num = (int)(OnlineMultiplayerConfig.MaxPlayers - 1U);
			if (this.m_localUsersJoinData != null && this.m_localUsersJoinData.Count > num)
			{
				this.m_Status.sessionJoinResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult>
				{
					m_returnCode = OnlineMultiplayerSessionJoinResult.eNotEnoughRoomForAllLocalUsers
				};
				this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
				this.m_Status.Result = eConnectionModeSwitchResult.Failure;
			}
			else if (this.m_iOnlineMultiplayerSessionCoordinator.Join(this.m_localUsersJoinData, this.m_room, new OnlineMultiplayerSessionJoinCallback(base.OnlineMultiplayerSessionJoinCallback)))
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
			this.m_localUsersJoinData.Clear();
			this.m_room = null;
		}
	}

	// Token: 0x040020CC RID: 8396
	private IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator;

	// Token: 0x040020CD RID: 8397
	private OnlineMultiplayerSessionEnumeratedRoom m_room;

	// Token: 0x040020CE RID: 8398
	private List<OnlineMultiplayerSessionJoinLocalUserData> m_localUsersJoinData = new List<OnlineMultiplayerSessionJoinLocalUserData>((int)OnlineMultiplayerConfig.MaxPlayers);

	// Token: 0x040020CF RID: 8399
	private NetConnectionState m_requestingAgent = NetConnectionState.COUNT;
}
