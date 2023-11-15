using System;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200059D RID: 1437
public class ClientTeleportalConveyenceReceiver : ClientSynchroniserBase, IClientConveyenceReceiver
{
	// Token: 0x06001B59 RID: 7001 RVA: 0x00087A20 File Offset: 0x00085E20
	private void Awake()
	{
		this.m_teleportalConveyenceReceiver = base.gameObject.RequireComponent<TeleportalConveyenceReceiver>();
	}

	// Token: 0x06001B5A RID: 7002 RVA: 0x00087A33 File Offset: 0x00085E33
	public void InformStartingConveyToMe()
	{
		this.m_receiving = true;
	}

	// Token: 0x06001B5B RID: 7003 RVA: 0x00087A3C File Offset: 0x00085E3C
	public void InformEndingConveyToMe()
	{
		this.m_receiving = false;
	}

	// Token: 0x06001B5C RID: 7004 RVA: 0x00087A45 File Offset: 0x00085E45
	public bool IsReceiving()
	{
		return this.m_receiving;
	}

	// Token: 0x06001B5D RID: 7005 RVA: 0x00087A4D File Offset: 0x00085E4D
	public void RefreshConveyTo()
	{
	}

	// Token: 0x06001B5E RID: 7006 RVA: 0x00087A4F File Offset: 0x00085E4F
	public void RegisterRefreshedConveyToCallback(CallbackVoid _callback)
	{
	}

	// Token: 0x06001B5F RID: 7007 RVA: 0x00087A51 File Offset: 0x00085E51
	public void UnregisterRefreshedConveyToCallback(CallbackVoid _callback)
	{
	}

	// Token: 0x04001574 RID: 5492
	private TeleportalConveyenceReceiver m_teleportalConveyenceReceiver;

	// Token: 0x04001575 RID: 5493
	private bool m_receiving;
}
