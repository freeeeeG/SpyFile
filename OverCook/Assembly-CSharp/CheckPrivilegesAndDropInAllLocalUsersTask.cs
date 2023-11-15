using System;
using System.Collections.Generic;
using Team17.Online;

// Token: 0x02000864 RID: 2148
public class CheckPrivilegesAndDropInAllLocalUsersTask : IMultiplayerTask
{
	// Token: 0x06002961 RID: 10593 RVA: 0x000C21DB File Offset: 0x000C05DB
	public void Initialise(GenericVoid<GamepadUser, EngagementSlot, IConnectionModeSwitchStatus> userChecked)
	{
		this.m_PlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		this.m_Status.m_TaskSubStatus = null;
		this.m_NothingToDoStatus.Progress = eConnectionModeSwitchProgress.Complete;
		this.m_NothingToDoStatus.Result = eConnectionModeSwitchResult.Success;
		this.m_UserChecked = userChecked;
	}

	// Token: 0x06002962 RID: 10594 RVA: 0x000C2213 File Offset: 0x000C0613
	public void Start(object startData)
	{
		this.m_bFinished = false;
		this.m_Status.bFinalTask = false;
		this.m_Status.m_TaskSubStatus = this.m_CurrentUserTask.GetStatus();
		this.m_bStarted = true;
		this.TryStart();
	}

	// Token: 0x06002963 RID: 10595 RVA: 0x000C224B File Offset: 0x000C064B
	public void Stop()
	{
		this.m_bStarted = false;
		this.m_Status.m_TaskSubStatus = null;
	}

	// Token: 0x06002964 RID: 10596 RVA: 0x000C2260 File Offset: 0x000C0660
	private void StartNewDropIn(GamepadUser pad, EngagementSlot slot)
	{
		this.m_Slot = slot;
		this.m_CurrentUser = pad;
		this.m_CurrentUserTask.Initialise(pad);
		this.m_CurrentUserTask.Start(null);
	}

	// Token: 0x06002965 RID: 10597 RVA: 0x000C2288 File Offset: 0x000C0688
	private bool DropInPreExistingUser(EngagementSlot slot)
	{
		bool result = false;
		GamepadUser user = this.m_PlayerManager.GetUser(slot);
		if (null != user)
		{
			OnlineMultiplayerLocalUserId allowedUser = PrivilegeCheckCache.GetAllowedUser(user);
			if (allowedUser != null)
			{
				if (!LocalDroppedInUserCache.HasBeenDroppedIn(allowedUser))
				{
					this.StartNewDropIn(user, slot);
					result = true;
				}
			}
			else
			{
				this.StartNewDropIn(user, slot);
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06002966 RID: 10598 RVA: 0x000C22E8 File Offset: 0x000C06E8
	private bool DropInEngagedPad(EngagementSlot slot)
	{
		bool result = false;
		GamepadUser user = this.m_PlayerManager.GetUser(slot);
		if (null != user)
		{
			OnlineMultiplayerLocalUserId allowedUser = PrivilegeCheckCache.GetAllowedUser(user);
			if (allowedUser == null || !LocalDroppedInUserCache.HasBeenDroppedIn(allowedUser))
			{
				this.StartNewDropIn(user, slot);
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06002967 RID: 10599 RVA: 0x000C2334 File Offset: 0x000C0734
	private void TryStart()
	{
		if (this.m_bStarted && this.m_CurrentUserTask.GetStatus().GetProgress() != eConnectionModeSwitchProgress.InProgress)
		{
			bool flag = false;
			for (int i = 1; i < 4; i++)
			{
				FastList<User> users = ServerUserSystem.m_Users;
				User.MachineID s_LocalMachineId = ServerUserSystem.s_LocalMachineId;
				User user = UserSystemUtils.FindUser(users, null, s_LocalMachineId, (EngagementSlot)i, TeamID.Count, User.SplitStatus.Count);
				if (user != null)
				{
					flag = this.DropInPreExistingUser((EngagementSlot)i);
				}
				else
				{
					flag = this.DropInEngagedPad((EngagementSlot)i);
				}
				if (flag)
				{
					break;
				}
			}
			if (!flag)
			{
				if (this.m_Status.m_TaskSubStatus.GetProgress() != eConnectionModeSwitchProgress.Complete)
				{
					this.m_Status.m_TaskSubStatus = this.m_NothingToDoStatus;
				}
				this.m_Status.bFinalTask = true;
				this.m_bFinished = true;
			}
		}
	}

	// Token: 0x06002968 RID: 10600 RVA: 0x000C23F8 File Offset: 0x000C07F8
	public void Update()
	{
		if (!this.m_bFinished)
		{
			this.m_CurrentUserTask.Update();
			eConnectionModeSwitchProgress progress = this.m_CurrentUserTask.GetStatus().GetProgress();
			if (progress != eConnectionModeSwitchProgress.NotStarted)
			{
				if (progress != eConnectionModeSwitchProgress.InProgress)
				{
					if (progress == eConnectionModeSwitchProgress.Complete)
					{
						if (this.m_CurrentUserTask.GetStatus().GetResult() != eConnectionModeSwitchResult.Success)
						{
							this.m_Status.bFinalTask = true;
							this.m_bFinished = true;
						}
						if (this.m_UserChecked != null)
						{
							this.m_UserChecked(this.m_CurrentUser, this.m_Slot, this.m_CurrentUserTask.GetStatus());
						}
						if (this.m_CurrentUserTask.GetStatus().GetResult() == eConnectionModeSwitchResult.Success)
						{
							this.TryStart();
						}
					}
				}
			}
			else
			{
				this.TryStart();
			}
		}
	}

	// Token: 0x06002969 RID: 10601 RVA: 0x000C24CC File Offset: 0x000C08CC
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Status;
	}

	// Token: 0x0600296A RID: 10602 RVA: 0x000C24D4 File Offset: 0x000C08D4
	public object GetData()
	{
		return null;
	}

	// Token: 0x040020AF RID: 8367
	private CheckPrivilegesAndDropInLocalUserTask m_CurrentUserTask = new CheckPrivilegesAndDropInLocalUserTask();

	// Token: 0x040020B0 RID: 8368
	private DefaultStatus m_NothingToDoStatus = new DefaultStatus();

	// Token: 0x040020B1 RID: 8369
	private bool m_bFinished;

	// Token: 0x040020B2 RID: 8370
	private bool m_bStarted;

	// Token: 0x040020B3 RID: 8371
	private CompositeStatus m_Status = new CompositeStatus();

	// Token: 0x040020B4 RID: 8372
	private IPlayerManager m_PlayerManager;

	// Token: 0x040020B5 RID: 8373
	private EngagementSlot m_Slot = EngagementSlot.Count;

	// Token: 0x040020B6 RID: 8374
	private GamepadUser m_CurrentUser;

	// Token: 0x040020B7 RID: 8375
	private GenericVoid<GamepadUser, EngagementSlot, IConnectionModeSwitchStatus> m_UserChecked;
}
