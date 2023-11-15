using System;
using System.Collections.Generic;
using Team17.Online;

// Token: 0x02000870 RID: 2160
public class PrivilegeCheckAllUsersForInviteTask : IMultiplayerTask
{
	// Token: 0x060029B8 RID: 10680 RVA: 0x000C3488 File Offset: 0x000C1888
	public void Initialise()
	{
		for (int i = 0; i < this.m_Tasks.Length; i++)
		{
			if (this.m_Tasks[i] == null)
			{
				this.m_Tasks[i] = new PrivilegeCheckAllUsersForInviteTask.Task();
			}
		}
	}

	// Token: 0x060029B9 RID: 10681 RVA: 0x000C34C8 File Offset: 0x000C18C8
	public void Start(object startData)
	{
		int iTaskCount = 0;
		int count = ServerUserSystem.m_Users.Count;
		for (int i = 0; i < count; i++)
		{
			User user = ServerUserSystem.m_Users._items[i];
			if (user.IsLocal && null != user.GamepadUser)
			{
				this.m_Tasks[iTaskCount++].Initialise(user.GamepadUser, user.Engagement);
			}
		}
		this.m_Status.m_TaskSubStatus = null;
		this.m_Status.bFinalTask = false;
		this.m_iCurrentTask = -1;
		this.m_iTaskCount = iTaskCount;
		if (this.m_iTaskCount != 0)
		{
			this.StartNextCheck();
		}
	}

	// Token: 0x060029BA RID: 10682 RVA: 0x000C3574 File Offset: 0x000C1974
	public void Stop()
	{
		this.m_Status.m_TaskSubStatus = null;
		this.m_Status.bFinalTask = false;
		for (int i = 0; i < this.m_Tasks.Length; i++)
		{
			this.m_Tasks[i].PrivilegeCheck.Stop();
		}
	}

	// Token: 0x060029BB RID: 10683 RVA: 0x000C35C4 File Offset: 0x000C19C4
	public void Update()
	{
		if (this.m_iCurrentTask >= 0 && this.m_iCurrentTask < this.m_iTaskCount)
		{
			PrivilegeCheckAllUsersForInviteTask.Task task = this.m_Tasks[this.m_iCurrentTask];
			PrivilegeCheckTask privilegeCheck = task.PrivilegeCheck;
			if (privilegeCheck.GetStatus().GetProgress() == eConnectionModeSwitchProgress.Complete)
			{
				if (privilegeCheck.GetStatus().GetResult() == eConnectionModeSwitchResult.Success)
				{
					this.StartNextCheck();
				}
				else
				{
					this.m_Status.bFinalTask = true;
				}
			}
		}
	}

	// Token: 0x060029BC RID: 10684 RVA: 0x000C363C File Offset: 0x000C1A3C
	private void StartNextCheck()
	{
		if (this.m_iCurrentTask == this.m_iTaskCount)
		{
			return;
		}
		this.m_iCurrentTask++;
		PrivilegeCheckAllUsersForInviteTask.Task task = this.m_Tasks[this.m_iCurrentTask];
		PrivilegeCheckTask privilegeCheck = task.PrivilegeCheck;
		privilegeCheck.Start(null);
		this.m_Status.m_TaskSubStatus = privilegeCheck.GetStatus();
		if (this.m_iCurrentTask == this.m_iTaskCount - 1)
		{
			this.m_Status.bFinalTask = true;
		}
	}

	// Token: 0x060029BD RID: 10685 RVA: 0x000C36B5 File Offset: 0x000C1AB5
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Status;
	}

	// Token: 0x060029BE RID: 10686 RVA: 0x000C36C0 File Offset: 0x000C1AC0
	public object GetData()
	{
		if (this.GetStatus().GetProgress() == eConnectionModeSwitchProgress.Complete && this.GetStatus().GetResult() == eConnectionModeSwitchResult.Success)
		{
			List<JoinSessionBaseTask.UserData> list = new List<JoinSessionBaseTask.UserData>((int)OnlineMultiplayerConfig.MaxPlayers);
			for (int i = 0; i < this.m_iTaskCount; i++)
			{
				list.Add(new JoinSessionBaseTask.UserData
				{
					UserId = (this.m_Tasks[i].PrivilegeCheck.GetData() as OnlineMultiplayerLocalUserId),
					Slot = this.m_Tasks[i].Slot
				});
			}
			return list;
		}
		return null;
	}

	// Token: 0x040020E8 RID: 8424
	private int m_iCurrentTask;

	// Token: 0x040020E9 RID: 8425
	private int m_iTaskCount;

	// Token: 0x040020EA RID: 8426
	private PrivilegeCheckAllUsersForInviteTask.Task[] m_Tasks = new PrivilegeCheckAllUsersForInviteTask.Task[4];

	// Token: 0x040020EB RID: 8427
	private CompositeStatus m_Status = new CompositeStatus();

	// Token: 0x02000871 RID: 2161
	private class Task
	{
		// Token: 0x060029C0 RID: 10688 RVA: 0x000C3764 File Offset: 0x000C1B64
		public void Initialise(GamepadUser user, EngagementSlot slot)
		{
			this.PrivilegeCheck.Initialise(user);
			this.Slot = slot;
		}

		// Token: 0x040020EC RID: 8428
		public PrivilegeCheckTask PrivilegeCheck = new PrivilegeCheckTask();

		// Token: 0x040020ED RID: 8429
		public EngagementSlot Slot;
	}
}
