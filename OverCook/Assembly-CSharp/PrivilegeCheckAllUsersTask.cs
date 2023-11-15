using System;
using Team17.Online;

// Token: 0x02000874 RID: 2164
public class PrivilegeCheckAllUsersTask : IMultiplayerTask
{
	// Token: 0x060029CC RID: 10700 RVA: 0x000C3AA8 File Offset: 0x000C1EA8
	public void Initialise(GamepadUser primaryUser)
	{
		this.m_PrimaryUser = primaryUser;
		for (int i = 0; i < this.m_Tasks.Length; i++)
		{
			if (this.m_Tasks[i] == null)
			{
				this.m_Tasks[i] = new PrivilegeCheckAllUsersTask.Task();
			}
		}
	}

	// Token: 0x060029CD RID: 10701 RVA: 0x000C3AF0 File Offset: 0x000C1EF0
	public void Start(object startData)
	{
		this.m_PrimaryUserTask = null;
		int num = 0;
		int count = ServerUserSystem.m_Users.Count;
		for (int i = 0; i < count; i++)
		{
			User user = ServerUserSystem.m_Users._items[i];
			if (user.IsLocal && null != user.GamepadUser)
			{
				PrivilegeCheckAllUsersTask.Task task = this.m_Tasks[num];
				task.Slot = user.Engagement;
				task.Initialise(user.GamepadUser);
				num++;
				if (user.GamepadUser.UID == this.m_PrimaryUser.UID)
				{
					this.m_PrimaryUserTask = task;
				}
			}
		}
		this.m_Status.m_TaskSubStatus = null;
		this.m_Status.bFinalTask = false;
		this.m_iCurrentTask = -1;
		this.m_iTaskCount = num;
		if (this.m_iTaskCount != 0)
		{
			this.StartNextCheck();
		}
	}

	// Token: 0x060029CE RID: 10702 RVA: 0x000C3BD4 File Offset: 0x000C1FD4
	public void Stop()
	{
		this.m_Status.m_TaskSubStatus = null;
		this.m_Status.bFinalTask = false;
		for (int i = 0; i < this.m_Tasks.Length; i++)
		{
			this.m_Tasks[i].PrivilegeCheck.Stop();
		}
	}

	// Token: 0x060029CF RID: 10703 RVA: 0x000C3C24 File Offset: 0x000C2024
	public void Update()
	{
		if (this.m_iCurrentTask >= 0 && this.m_iCurrentTask < this.m_iTaskCount)
		{
			PrivilegeCheckAllUsersTask.Task task = this.m_Tasks[this.m_iCurrentTask];
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

	// Token: 0x060029D0 RID: 10704 RVA: 0x000C3C9C File Offset: 0x000C209C
	private void StartNextCheck()
	{
		if (this.m_iCurrentTask == this.m_iTaskCount)
		{
			return;
		}
		this.m_iCurrentTask++;
		PrivilegeCheckAllUsersTask.Task task = this.m_Tasks[this.m_iCurrentTask];
		PrivilegeCheckTask privilegeCheck = task.PrivilegeCheck;
		privilegeCheck.Start(null);
		this.m_Status.m_TaskSubStatus = privilegeCheck.GetStatus();
		if (this.m_iCurrentTask == this.m_iTaskCount - 1)
		{
			this.m_Status.bFinalTask = true;
		}
	}

	// Token: 0x060029D1 RID: 10705 RVA: 0x000C3D15 File Offset: 0x000C2115
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Status;
	}

	// Token: 0x060029D2 RID: 10706 RVA: 0x000C3D1D File Offset: 0x000C211D
	public object GetData()
	{
		if (this.GetStatus().GetProgress() == eConnectionModeSwitchProgress.Complete && this.GetStatus().GetResult() == eConnectionModeSwitchResult.Success)
		{
			return this.m_PrimaryUserTask.PrivilegeCheck.GetData();
		}
		return null;
	}

	// Token: 0x040020F4 RID: 8436
	private int m_iCurrentTask;

	// Token: 0x040020F5 RID: 8437
	private int m_iTaskCount;

	// Token: 0x040020F6 RID: 8438
	private PrivilegeCheckAllUsersTask.Task[] m_Tasks = new PrivilegeCheckAllUsersTask.Task[4];

	// Token: 0x040020F7 RID: 8439
	private CompositeStatus m_Status = new CompositeStatus();

	// Token: 0x040020F8 RID: 8440
	private GamepadUser m_PrimaryUser;

	// Token: 0x040020F9 RID: 8441
	private PrivilegeCheckAllUsersTask.Task m_PrimaryUserTask;

	// Token: 0x02000875 RID: 2165
	private class Task
	{
		// Token: 0x060029D4 RID: 10708 RVA: 0x000C3D66 File Offset: 0x000C2166
		public void Initialise(GamepadUser user)
		{
			this.PrivilegeCheck.Initialise(user);
			this.Slot = EngagementSlot.Count;
		}

		// Token: 0x040020FA RID: 8442
		public PrivilegeCheckTask PrivilegeCheck = new PrivilegeCheckTask();

		// Token: 0x040020FB RID: 8443
		public EngagementSlot Slot;
	}
}
