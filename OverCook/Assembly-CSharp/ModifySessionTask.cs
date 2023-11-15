using System;
using System.Collections.Generic;
using Team17.Online;

// Token: 0x0200086E RID: 2158
public class ModifySessionTask : IMultiplayerTask
{
	// Token: 0x060029AA RID: 10666 RVA: 0x000C3348 File Offset: 0x000C1748
	public void Initialise(IOnlineMultiplayerSessionCoordinator sessionCoordinator, OnlineMultiplayerSessionVisibility visibility, List<OnlineMultiplayerSessionPropertyValue> values)
	{
		this.m_Visibility = visibility;
		this.m_PropertyValues = values;
		this.m_iOnlineMultiplayerSessionCoordinator = sessionCoordinator;
	}

	// Token: 0x060029AB RID: 10667 RVA: 0x000C3360 File Offset: 0x000C1760
	public void Start(object startData)
	{
		bool flag = this.m_iOnlineMultiplayerSessionCoordinator.Modify(this.m_PropertyValues, this.m_Visibility);
		if (flag)
		{
			ServerGameSetup.Mode = ServerSessionPropertyValuesProvider.GetGameMode();
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Success;
		}
		else
		{
			this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
			this.m_Status.Result = eConnectionModeSwitchResult.Failure;
		}
	}

	// Token: 0x060029AC RID: 10668 RVA: 0x000C33CA File Offset: 0x000C17CA
	public void Stop()
	{
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
	}

	// Token: 0x060029AD RID: 10669 RVA: 0x000C33E4 File Offset: 0x000C17E4
	public void Update()
	{
	}

	// Token: 0x060029AE RID: 10670 RVA: 0x000C33E6 File Offset: 0x000C17E6
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Status;
	}

	// Token: 0x060029AF RID: 10671 RVA: 0x000C33EE File Offset: 0x000C17EE
	public object GetData()
	{
		return null;
	}

	// Token: 0x040020E1 RID: 8417
	private IOnlineMultiplayerSessionCoordinator m_iOnlineMultiplayerSessionCoordinator;

	// Token: 0x040020E2 RID: 8418
	private ModifySessionStatus m_Status = new ModifySessionStatus();

	// Token: 0x040020E3 RID: 8419
	private List<OnlineMultiplayerSessionPropertyValue> m_PropertyValues;

	// Token: 0x040020E4 RID: 8420
	private OnlineMultiplayerSessionVisibility m_Visibility = OnlineMultiplayerSessionVisibility.eClosed;
}
