using System;
using System.Collections.Generic;

// Token: 0x02000869 RID: 2153
public class JoinRandomRoomTask : IMultiplayerTask
{
	// Token: 0x0600298B RID: 10635 RVA: 0x000C2F54 File Offset: 0x000C1354
	public void Start(object startData)
	{
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
		this.m_joinUserData.Clear();
		this.m_iJoinAttemptIndex = 0;
		this.m_SearchData = (startData as SearchTask.SearchResultData);
		GameUtils.s_RoomSearch_NoneAvailable = false;
		if (this.m_SearchData == null || this.m_SearchData.m_AvailableSessions == null || this.m_SearchData.m_LocalUser == null)
		{
			if (this.m_SearchData != null && this.m_SearchData.m_AvailableSessions == null)
			{
				GameUtils.s_RoomSearch_NoneAvailable = true;
			}
			this.Fail();
			return;
		}
		this.JoinGame();
	}

	// Token: 0x0600298C RID: 10636 RVA: 0x000C2FF6 File Offset: 0x000C13F6
	public void Stop()
	{
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
	}

	// Token: 0x0600298D RID: 10637 RVA: 0x000C3010 File Offset: 0x000C1410
	private void JoinGame()
	{
		if (this.m_iJoinAttemptIndex == this.m_SearchData.m_AvailableSessions.Count)
		{
			this.Fail();
		}
		else
		{
			this.m_Status.Progress = eConnectionModeSwitchProgress.InProgress;
			this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
			this.m_joinUserData.Add(new JoinSessionBaseTask.UserData
			{
				UserId = this.m_SearchData.m_LocalUser,
				Slot = EngagementSlot.One
			});
			this.m_JoinRoomTask.Initialise(this.m_SearchData.m_AvailableSessions[this.m_iJoinAttemptIndex], NetConnectionState.Matchmake);
			this.m_JoinRoomTask.Start(this.m_joinUserData);
			this.m_joinUserData.Clear();
		}
	}

	// Token: 0x0600298E RID: 10638 RVA: 0x000C30C4 File Offset: 0x000C14C4
	public void Update()
	{
		eConnectionModeSwitchProgress progress = this.m_JoinRoomTask.GetStatus().GetProgress();
		if (progress != eConnectionModeSwitchProgress.NotStarted)
		{
			if (progress != eConnectionModeSwitchProgress.InProgress)
			{
				if (progress == eConnectionModeSwitchProgress.Complete)
				{
					this.UpdateComplete();
				}
			}
		}
		else
		{
			this.JoinGame();
		}
	}

	// Token: 0x0600298F RID: 10639 RVA: 0x000C3118 File Offset: 0x000C1518
	private void UpdateComplete()
	{
		eConnectionModeSwitchResult result = this.m_JoinRoomTask.GetStatus().GetResult();
		if (result != eConnectionModeSwitchResult.NotAvailableYet)
		{
			if (result != eConnectionModeSwitchResult.Failure)
			{
				if (result == eConnectionModeSwitchResult.Success)
				{
					this.Success();
				}
			}
			else
			{
				this.m_iJoinAttemptIndex++;
				this.JoinGame();
			}
		}
		else
		{
			this.Fail();
		}
	}

	// Token: 0x06002990 RID: 10640 RVA: 0x000C317E File Offset: 0x000C157E
	private void Fail()
	{
		this.m_SearchData.m_AvailableSessions = null;
		this.m_SearchData.m_LocalUser = null;
		this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
		this.m_Status.Result = eConnectionModeSwitchResult.Failure;
	}

	// Token: 0x06002991 RID: 10641 RVA: 0x000C31B0 File Offset: 0x000C15B0
	private void Success()
	{
		this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
		this.m_Status.Result = eConnectionModeSwitchResult.Success;
	}

	// Token: 0x06002992 RID: 10642 RVA: 0x000C31CA File Offset: 0x000C15CA
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Status;
	}

	// Token: 0x06002993 RID: 10643 RVA: 0x000C31D2 File Offset: 0x000C15D2
	public object GetData()
	{
		return this.m_JoinRoomTask.GetData();
	}

	// Token: 0x040020D0 RID: 8400
	private SearchStatus m_Status = new SearchStatus();

	// Token: 0x040020D1 RID: 8401
	private int m_iJoinAttemptIndex;

	// Token: 0x040020D2 RID: 8402
	private SearchTask.SearchResultData m_SearchData = new SearchTask.SearchResultData();

	// Token: 0x040020D3 RID: 8403
	private JoinEnumeratedRoomTask m_JoinRoomTask = new JoinEnumeratedRoomTask();

	// Token: 0x040020D4 RID: 8404
	private List<JoinSessionBaseTask.UserData> m_joinUserData = new List<JoinSessionBaseTask.UserData>(1);
}
