using System;
using Team17.Online.Multiplayer;

// Token: 0x02000877 RID: 2167
public class ResetServerUsersTask : IMultiplayerTask
{
	// Token: 0x060029E1 RID: 10721 RVA: 0x000C4158 File Offset: 0x000C2558
	public void Initialise(Server server)
	{
		this.m_Server = server;
	}

	// Token: 0x060029E2 RID: 10722 RVA: 0x000C4161 File Offset: 0x000C2561
	public void Start(object startData)
	{
		this.m_Server.GetUserSystem().ResetUsersToOfflineState();
		this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
		this.m_Status.Result = eConnectionModeSwitchResult.Success;
	}

	// Token: 0x060029E3 RID: 10723 RVA: 0x000C418B File Offset: 0x000C258B
	public void Stop()
	{
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
	}

	// Token: 0x060029E4 RID: 10724 RVA: 0x000C41A5 File Offset: 0x000C25A5
	public void Update()
	{
	}

	// Token: 0x060029E5 RID: 10725 RVA: 0x000C41A7 File Offset: 0x000C25A7
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Status;
	}

	// Token: 0x060029E6 RID: 10726 RVA: 0x000C41AF File Offset: 0x000C25AF
	public object GetData()
	{
		return null;
	}

	// Token: 0x04002101 RID: 8449
	private DefaultStatus m_Status = new DefaultStatus();

	// Token: 0x04002102 RID: 8450
	private Server m_Server;
}
