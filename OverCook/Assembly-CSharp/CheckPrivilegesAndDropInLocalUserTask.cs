using System;

// Token: 0x02000865 RID: 2149
public class CheckPrivilegesAndDropInLocalUserTask : IMultiplayerTask
{
	// Token: 0x0600296C RID: 10604 RVA: 0x000C2500 File Offset: 0x000C0900
	public void Initialise(GamepadUser user)
	{
		this.m_AddUserToSessionTasks = new IMultiplayerTask[]
		{
			this.m_PrivilegeCheck,
			this.m_DropInLocalUser
		};
		this.m_GamepadUser = user;
		this.m_PrivilegeCheck.Initialise(user);
		this.m_DropInLocalUser.Initialise(user);
	}

	// Token: 0x0600296D RID: 10605 RVA: 0x000C253F File Offset: 0x000C093F
	public void Start(object startData)
	{
		this.m_CurrentAction.Start(this.m_AddUserToSessionTasks);
		this.m_bBusy = true;
	}

	// Token: 0x0600296E RID: 10606 RVA: 0x000C2559 File Offset: 0x000C0959
	public void Stop()
	{
		this.m_GamepadUser = null;
		this.m_CurrentAction.Stop();
		this.m_bBusy = false;
	}

	// Token: 0x0600296F RID: 10607 RVA: 0x000C2574 File Offset: 0x000C0974
	public void Update()
	{
		if (this.m_bBusy)
		{
			this.m_CurrentAction.Update();
		}
	}

	// Token: 0x06002970 RID: 10608 RVA: 0x000C258C File Offset: 0x000C098C
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_CurrentAction.GetStatus();
	}

	// Token: 0x06002971 RID: 10609 RVA: 0x000C2599 File Offset: 0x000C0999
	public object GetData()
	{
		return null;
	}

	// Token: 0x040020B8 RID: 8376
	private GamepadUser m_GamepadUser;

	// Token: 0x040020B9 RID: 8377
	private PrivilegeCheckTask m_PrivilegeCheck = new PrivilegeCheckTask();

	// Token: 0x040020BA RID: 8378
	private DropInLocalUserTask m_DropInLocalUser = new DropInLocalUserTask();

	// Token: 0x040020BB RID: 8379
	private IMultiplayerTask[] m_AddUserToSessionTasks;

	// Token: 0x040020BC RID: 8380
	private MultiplayerOperation m_CurrentAction = new MultiplayerOperation();

	// Token: 0x040020BD RID: 8381
	private bool m_bBusy;
}
