using System;
using System.ComponentModel;
using BitStream;
using Team17.Online;

// Token: 0x0200086B RID: 2155
public abstract class JoinSessionBaseTask : IMultiplayerTask
{
	// Token: 0x06002996 RID: 10646
	public abstract void TryStart();

	// Token: 0x06002997 RID: 10647 RVA: 0x000C1C92 File Offset: 0x000C0092
	public virtual void Start(object startData)
	{
		this.m_JoinDataReceived = false;
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
		this.TryStart();
	}

	// Token: 0x06002998 RID: 10648 RVA: 0x000C1CB9 File Offset: 0x000C00B9
	public void Stop()
	{
		this.m_Status.Progress = eConnectionModeSwitchProgress.NotStarted;
		this.m_Status.Result = eConnectionModeSwitchResult.NotAvailableYet;
	}

	// Token: 0x06002999 RID: 10649 RVA: 0x000C1CD3 File Offset: 0x000C00D3
	public virtual void Update()
	{
		if (this.m_Status.Progress == eConnectionModeSwitchProgress.NotStarted && this.m_Status.Result == eConnectionModeSwitchResult.NotAvailableYet)
		{
			this.TryStart();
		}
	}

	// Token: 0x0600299A RID: 10650 RVA: 0x000C1CFB File Offset: 0x000C00FB
	public object GetData()
	{
		if (this.m_JoinDataReceived)
		{
			return this.m_JoinData;
		}
		return null;
	}

	// Token: 0x0600299B RID: 10651 RVA: 0x000C1D10 File Offset: 0x000C0110
	public IConnectionModeSwitchStatus GetStatus()
	{
		return this.m_Status;
	}

	// Token: 0x0600299C RID: 10652 RVA: 0x000C1D18 File Offset: 0x000C0118
	public void OnlineMultiplayerSessionJoinCallback(OnlineMultiplayerReturnCode<OnlineMultiplayerSessionJoinResult> result, byte[] replyData, int replyDataSize)
	{
		this.m_Status.sessionJoinResult = result;
		if (result != null && result.m_returnCode == OnlineMultiplayerSessionJoinResult.eSuccess)
		{
			if (replyData != null)
			{
				BitStreamReader bitStreamReader = new BitStreamReader(replyData);
				this.m_JoinData.machine = (User.MachineID)bitStreamReader.ReadUInt32(3);
				this.m_JoinData.timeSync.Deserialise(bitStreamReader);
				this.m_JoinData.usersChanged.Deserialise(bitStreamReader);
				this.m_JoinData.gameSetup.Deserialise(bitStreamReader);
				this.m_JoinDataReceived = true;
			}
			else
			{
				this.m_JoinDataReceived = false;
			}
			this.m_Status.Result = eConnectionModeSwitchResult.Success;
		}
		else
		{
			this.m_Status.Result = eConnectionModeSwitchResult.Failure;
		}
		this.m_Status.Progress = eConnectionModeSwitchProgress.Complete;
	}

	// Token: 0x040020D9 RID: 8409
	protected JoinSessionStatus m_Status = new JoinSessionStatus();

	// Token: 0x040020DA RID: 8410
	protected JoinData m_JoinData = new JoinData();

	// Token: 0x040020DB RID: 8411
	protected bool m_JoinDataReceived;

	// Token: 0x0200086C RID: 2156
	public class UserData
	{
		// Token: 0x17000330 RID: 816
		// (get) Token: 0x0600299E RID: 10654 RVA: 0x000C1DDB File Offset: 0x000C01DB
		// (set) Token: 0x0600299F RID: 10655 RVA: 0x000C1DE3 File Offset: 0x000C01E3
		[DefaultValue(null)]
		public OnlineMultiplayerLocalUserId UserId { get; set; }

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x060029A0 RID: 10656 RVA: 0x000C1DEC File Offset: 0x000C01EC
		// (set) Token: 0x060029A1 RID: 10657 RVA: 0x000C1DF4 File Offset: 0x000C01F4
		[DefaultValue(EngagementSlot.Count)]
		public EngagementSlot Slot { get; set; }
	}
}
