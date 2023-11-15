using System;
using Team17.Online.Multiplayer;

// Token: 0x0200086F RID: 2159
public class OfflineTask : IMultiplayerTask
{
	// Token: 0x060029B1 RID: 10673 RVA: 0x000C3404 File Offset: 0x000C1804
	public void Initialise(Server server, Client client)
	{
		this.m_Server = server;
		this.m_Client = client;
	}

	// Token: 0x060029B2 RID: 10674 RVA: 0x000C3414 File Offset: 0x000C1814
	public void Start(object startData)
	{
		NetworkSystemConfigurator.Offline(this.m_Server, this.m_Client);
		this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
		this.m_Status.Result = eConnectionModeSwitchResult.Success;
	}

	// Token: 0x060029B3 RID: 10675 RVA: 0x000C343F File Offset: 0x000C183F
	public void Stop()
	{
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
	}

	// Token: 0x060029B4 RID: 10676 RVA: 0x000C3459 File Offset: 0x000C1859
	public void Update()
	{
	}

	// Token: 0x060029B5 RID: 10677 RVA: 0x000C345B File Offset: 0x000C185B
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Status;
	}

	// Token: 0x060029B6 RID: 10678 RVA: 0x000C3463 File Offset: 0x000C1863
	public object GetData()
	{
		return null;
	}

	// Token: 0x040020E5 RID: 8421
	private DefaultStatus m_Status = new DefaultStatus();

	// Token: 0x040020E6 RID: 8422
	private Server m_Server;

	// Token: 0x040020E7 RID: 8423
	private Client m_Client;
}
