using System;
using System.Collections.Generic;
using Team17.Online;

// Token: 0x02000862 RID: 2146
public class AcceptInviteTask : JoinSessionBaseTask
{
	// Token: 0x06002959 RID: 10585 RVA: 0x000C1E17 File Offset: 0x000C0217
	public void Initialise(OnlineMultiplayerSessionInvite invite)
	{
		this.m_Invite = invite;
	}

	// Token: 0x0600295A RID: 10586 RVA: 0x000C1E20 File Offset: 0x000C0220
	public override void Start(object startData)
	{
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_iOnlineMultiplayerSessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		this.m_localUsersJoinData.Clear();
		List<JoinSessionBaseTask.UserData> list = startData as List<JoinSessionBaseTask.UserData>;
		if (list != null && list.Count > 0)
		{
			this.m_Status.currentUser = list[0].UserId;
			bool flag = false;
			int num = 0;
			while (num < list.Count && !flag)
			{
				OnlineMultiplayerSessionJoinLocalUserData onlineMultiplayerSessionJoinLocalUserData = JoinDataProvider.BuildJoinRequestData(list[num].Slot, NetConnectionState.AcceptInvite, list[num].UserId);
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

	// Token: 0x0600295B RID: 10587 RVA: 0x000C1EF4 File Offset: 0x000C02F4
	public override void TryStart()
	{
		if (this.m_iOnlineMultiplayerSessionCoordinator.IsIdle())
		{
			if (this.m_localUsersJoinData.Count > 0)
			{
				if (this.m_iOnlineMultiplayerSessionCoordinator.Join(this.m_localUsersJoinData, this.m_Invite, new OnlineMultiplayerSessionJoinCallback(base.OnlineMultiplayerSessionJoinCallback)))
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
			else
			{
				this.m_Status.sessionJoinResult = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult>
				{
					m_returnCode = OnlineMultiplayerSessionJoinResult.eGenericFailure
				};
				this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
				this.m_Status.Result = eConnectionModeSwitchResult.Failure;
			}
			this.m_Invite = null;
			this.m_localUsersJoinData.Clear();
		}
	}

	// Token: 0x040020A8 RID: 8360
	private OnlineMultiplayerSessionInvite m_Invite;

	// Token: 0x040020A9 RID: 8361
	private IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator;

	// Token: 0x040020AA RID: 8362
	private List<OnlineMultiplayerSessionJoinLocalUserData> m_localUsersJoinData = new List<OnlineMultiplayerSessionJoinLocalUserData>((int)(OnlineMultiplayerConfig.MaxPlayers - 1U));
}
