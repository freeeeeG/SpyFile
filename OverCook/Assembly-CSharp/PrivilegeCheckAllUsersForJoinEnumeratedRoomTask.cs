using System;
using System.Collections.Generic;
using Team17.Online;

// Token: 0x02000872 RID: 2162
public class PrivilegeCheckAllUsersForJoinEnumeratedRoomTask : IMultiplayerTask
{
	// Token: 0x060029C2 RID: 10690 RVA: 0x000C3798 File Offset: 0x000C1B98
	public void Initialise()
	{
		for (int i = 0; i < this.m_Tasks.Length; i++)
		{
			if (this.m_Tasks[i] == null)
			{
				this.m_Tasks[i] = new PrivilegeCheckAllUsersForJoinEnumeratedRoomTask.Task();
			}
		}
	}

	// Token: 0x060029C3 RID: 10691 RVA: 0x000C37D8 File Offset: 0x000C1BD8
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

	// Token: 0x060029C4 RID: 10692 RVA: 0x000C3884 File Offset: 0x000C1C84
	public void Stop()
	{
		this.m_Status.m_TaskSubStatus = null;
		this.m_Status.bFinalTask = false;
		for (int i = 0; i < this.m_Tasks.Length; i++)
		{
			this.m_Tasks[i].PrivilegeCheck.Stop();
		}
	}

	// Token: 0x060029C5 RID: 10693 RVA: 0x000C38D4 File Offset: 0x000C1CD4
	public void Update()
	{
		if (this.m_iCurrentTask >= 0 && this.m_iCurrentTask < this.m_iTaskCount)
		{
			PrivilegeCheckAllUsersForJoinEnumeratedRoomTask.Task task = this.m_Tasks[this.m_iCurrentTask];
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

	// Token: 0x060029C6 RID: 10694 RVA: 0x000C394C File Offset: 0x000C1D4C
	private void StartNextCheck()
	{
		if (this.m_iCurrentTask == this.m_iTaskCount)
		{
			return;
		}
		this.m_iCurrentTask++;
		PrivilegeCheckAllUsersForJoinEnumeratedRoomTask.Task task = this.m_Tasks[this.m_iCurrentTask];
		PrivilegeCheckTask privilegeCheck = task.PrivilegeCheck;
		privilegeCheck.Start(null);
		this.m_Status.m_TaskSubStatus = privilegeCheck.GetStatus();
		if (this.m_iCurrentTask == this.m_iTaskCount - 1)
		{
			this.m_Status.bFinalTask = true;
		}
	}

	// Token: 0x060029C7 RID: 10695 RVA: 0x000C39C5 File Offset: 0x000C1DC5
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Status;
	}

	// Token: 0x060029C8 RID: 10696 RVA: 0x000C39D0 File Offset: 0x000C1DD0
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

	// Token: 0x040020EE RID: 8430
	private int m_iCurrentTask;

	// Token: 0x040020EF RID: 8431
	private int m_iTaskCount;

	// Token: 0x040020F0 RID: 8432
	private PrivilegeCheckAllUsersForJoinEnumeratedRoomTask.Task[] m_Tasks = new PrivilegeCheckAllUsersForJoinEnumeratedRoomTask.Task[4];

	// Token: 0x040020F1 RID: 8433
	private CompositeStatus m_Status = new CompositeStatus();

	// Token: 0x02000873 RID: 2163
	private class Task
	{
		// Token: 0x060029CA RID: 10698 RVA: 0x000C3A74 File Offset: 0x000C1E74
		public void Initialise(GamepadUser user, EngagementSlot slot)
		{
			this.PrivilegeCheck.Initialise(user);
			this.Slot = slot;
		}

		// Token: 0x040020F2 RID: 8434
		public PrivilegeCheckTask PrivilegeCheck = new PrivilegeCheckTask();

		// Token: 0x040020F3 RID: 8435
		public EngagementSlot Slot;
	}
}
