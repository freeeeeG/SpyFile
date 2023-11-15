using System;
using System.Collections.Generic;
using Team17.Online;

// Token: 0x02000878 RID: 2168
public class SearchTask : IMultiplayerTask
{
	// Token: 0x060029E8 RID: 10728 RVA: 0x000C41D0 File Offset: 0x000C25D0
	public void Initialise(List<OnlineMultiplayerSessionPropertySearchValue> filterParameter)
	{
		this.m_FilterParameter = filterParameter;
	}

	// Token: 0x060029E9 RID: 10729 RVA: 0x000C41DC File Offset: 0x000C25DC
	public void Start(object startData)
	{
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_iOnlineMultiplayerSessionEnumerater = onlinePlatformManager.OnlineMultiplayerSessionEnumerateCoordinator();
		if (!this.m_iOnlineMultiplayerSessionEnumerater.IsIdle())
		{
			this.m_iOnlineMultiplayerSessionEnumerater.Cancel();
		}
		this.m_LocalUser = (startData as OnlineMultiplayerLocalUserId);
		this.TryStart();
	}

	// Token: 0x060029EA RID: 10730 RVA: 0x000C4240 File Offset: 0x000C2640
	public void Stop()
	{
		if (this.m_registeredForErrors)
		{
			IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
			IOnlineMultiplayerConnectionModeCoordinator onlineMultiplayerConnectionModeCoordinator = onlinePlatformManager.OnlineMultiplayerConnectionModeCoordinator();
			if (onlineMultiplayerConnectionModeCoordinator != null)
			{
				onlineMultiplayerConnectionModeCoordinator.UnRegisterErrorCallback(new OnlineMultiplayerConnectionModeErrorCallback(this.OnlineMultiplayerConnectionModeErrorCallback));
			}
			this.m_registeredForErrors = false;
		}
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
	}

	// Token: 0x060029EB RID: 10731 RVA: 0x000C429C File Offset: 0x000C269C
	public void TryStart()
	{
		if (this.m_iOnlineMultiplayerSessionEnumerater.IsIdle())
		{
			if (this.m_iOnlineMultiplayerSessionEnumerater.Start(this.m_LocalUser, this.m_FilterParameter, OnlineMultiplayerConfig.MaxBrowsedSessionsToEnumerate, new OnlineMultiplayerSessionEnumerateCallback(this.OnlineMultiplayerSessionEnumerateCallback)))
			{
				this.m_Status.Progress = eConnectionModeSwitchProgress.InProgress;
				this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
				if (!this.m_registeredForErrors)
				{
					IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
					IOnlineMultiplayerConnectionModeCoordinator onlineMultiplayerConnectionModeCoordinator = onlinePlatformManager.OnlineMultiplayerConnectionModeCoordinator();
					if (onlineMultiplayerConnectionModeCoordinator != null)
					{
						onlineMultiplayerConnectionModeCoordinator.RegisterErrorCallback(new OnlineMultiplayerConnectionModeErrorCallback(this.OnlineMultiplayerConnectionModeErrorCallback));
					}
					this.m_registeredForErrors = true;
				}
			}
			else
			{
				this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
				this.m_Status.Result = eConnectionModeSwitchResult.Failure;
			}
		}
	}

	// Token: 0x060029EC RID: 10732 RVA: 0x000C4352 File Offset: 0x000C2752
	public void Update()
	{
		if (this.m_Status.Progress == eConnectionModeSwitchProgress.NotStarted && this.m_Status.Result == eConnectionModeSwitchResult.NotAvailableYet)
		{
			this.TryStart();
		}
	}

	// Token: 0x060029ED RID: 10733 RVA: 0x000C437C File Offset: 0x000C277C
	public void OnlineMultiplayerConnectionModeErrorCallback(OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult> result)
	{
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		IOnlineMultiplayerConnectionModeCoordinator onlineMultiplayerConnectionModeCoordinator = onlinePlatformManager.OnlineMultiplayerConnectionModeCoordinator();
		if (onlineMultiplayerConnectionModeCoordinator != null)
		{
			onlineMultiplayerConnectionModeCoordinator.UnRegisterErrorCallback(new OnlineMultiplayerConnectionModeErrorCallback(this.OnlineMultiplayerConnectionModeErrorCallback));
		}
		this.m_registeredForErrors = false;
		this.m_SearchResultData.m_AvailableSessions = null;
		this.m_SearchResultData.m_LocalUser = null;
		result.DisplayPlatformSpecificError(false);
		this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
		this.m_Status.Result = eConnectionModeSwitchResult.Failure;
	}

	// Token: 0x060029EE RID: 10734 RVA: 0x000C43ED File Offset: 0x000C27ED
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Status;
	}

	// Token: 0x060029EF RID: 10735 RVA: 0x000C43F5 File Offset: 0x000C27F5
	public object GetData()
	{
		return this.m_SearchResultData;
	}

	// Token: 0x060029F0 RID: 10736 RVA: 0x000C4400 File Offset: 0x000C2800
	private void OnlineMultiplayerSessionEnumerateCallback(List<OnlineMultiplayerSessionEnumeratedRoom> availableSessions, bool requestSuccessful)
	{
		if (requestSuccessful)
		{
			this.m_SearchResultData.m_AvailableSessions = availableSessions;
			this.m_SearchResultData.m_LocalUser = this.m_LocalUser;
			this.m_Status.Result = eConnectionModeSwitchResult.Success;
		}
		else
		{
			this.m_SearchResultData.m_AvailableSessions = null;
			this.m_SearchResultData.m_LocalUser = null;
			this.m_Status.Result = eConnectionModeSwitchResult.Failure;
		}
		if (this.m_registeredForErrors)
		{
			IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
			IOnlineMultiplayerConnectionModeCoordinator onlineMultiplayerConnectionModeCoordinator = onlinePlatformManager.OnlineMultiplayerConnectionModeCoordinator();
			if (onlineMultiplayerConnectionModeCoordinator != null)
			{
				onlineMultiplayerConnectionModeCoordinator.UnRegisterErrorCallback(new OnlineMultiplayerConnectionModeErrorCallback(this.OnlineMultiplayerConnectionModeErrorCallback));
			}
			this.m_registeredForErrors = false;
		}
		this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
	}

	// Token: 0x04002103 RID: 8451
	private IOnlineMultiplayerSessionEnumerateCoordinator m_iOnlineMultiplayerSessionEnumerater;

	// Token: 0x04002104 RID: 8452
	private SearchStatus m_Status = new SearchStatus();

	// Token: 0x04002105 RID: 8453
	private OnlineMultiplayerLocalUserId m_LocalUser;

	// Token: 0x04002106 RID: 8454
	private List<OnlineMultiplayerSessionPropertySearchValue> m_FilterParameter;

	// Token: 0x04002107 RID: 8455
	private SearchTask.SearchResultData m_SearchResultData = new SearchTask.SearchResultData();

	// Token: 0x04002108 RID: 8456
	private bool m_registeredForErrors;

	// Token: 0x02000879 RID: 2169
	public class SearchResultData
	{
		// Token: 0x04002109 RID: 8457
		public List<OnlineMultiplayerSessionEnumeratedRoom> m_AvailableSessions;

		// Token: 0x0400210A RID: 8458
		public OnlineMultiplayerLocalUserId m_LocalUser;
	}
}
