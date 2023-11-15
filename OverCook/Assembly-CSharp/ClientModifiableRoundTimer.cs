using System;
using UnityEngine;

// Token: 0x02000755 RID: 1877
public class ClientModifiableRoundTimer : ClientRoundTimer
{
	// Token: 0x06002415 RID: 9237 RVA: 0x000AC269 File Offset: 0x000AA669
	public override void Initialise()
	{
		base.Initialise();
		this.m_roundTimer = this.m_timeLimit;
		this.m_dataStore.Write(ClientRoundTimer.k_timeUpdatedId, this.m_roundTimer);
	}

	// Token: 0x06002416 RID: 9238 RVA: 0x000AC298 File Offset: 0x000AA698
	public override void Zero()
	{
		this.m_roundTimer = 0f;
		this.Update();
	}

	// Token: 0x06002417 RID: 9239 RVA: 0x000AC2AC File Offset: 0x000AA6AC
	public void AddTime(int time)
	{
		this.m_roundTimer += (float)time;
		this.m_roundTimer = Mathf.Clamp(this.m_roundTimer, 0f, 5999f);
		this.m_dataStore.Write(ClientModifiableRoundTimer.k_timeAddedId, time);
	}

	// Token: 0x06002418 RID: 9240 RVA: 0x000AC2FC File Offset: 0x000AA6FC
	public override void Update()
	{
		this.m_suppressionController.UpdateSuppressors();
		if (!this.m_suppressionController.IsSuppressed() && !DebugManager.Instance.GetOption("Freeze time"))
		{
			this.m_roundTimer -= TimeManager.GetDeltaTime(LayerMask.NameToLayer("Default"));
			this.m_dataStore.Write(ClientRoundTimer.k_timeUpdatedId, this.m_roundTimer);
		}
	}

	// Token: 0x04001B92 RID: 7058
	protected const int k_timeMax = 5999;

	// Token: 0x04001B93 RID: 7059
	private static readonly DataStore.Id k_timeAddedId = new DataStore.Id("time.added");
}
