using System;
using UnityEngine;

// Token: 0x02000791 RID: 1937
public class ClientUnlimitedRoundTimer : IClientRoundTimer
{
	// Token: 0x06002575 RID: 9589 RVA: 0x000B13B2 File Offset: 0x000AF7B2
	public virtual void Initialise()
	{
		this.m_dataStore = GameUtils.RequireManager<DataStore>();
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06002576 RID: 9590 RVA: 0x000B13BF File Offset: 0x000AF7BF
	public SuppressionController Suppressor
	{
		get
		{
			return this.m_suppressionController;
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x06002577 RID: 9591 RVA: 0x000B13C7 File Offset: 0x000AF7C7
	public bool IsSuppressed
	{
		get
		{
			return this.m_suppressionController.IsSuppressed();
		}
	}

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x06002578 RID: 9592 RVA: 0x000B13D4 File Offset: 0x000AF7D4
	public float TimeElapsed
	{
		get
		{
			return this.m_roundTimer;
		}
	}

	// Token: 0x06002579 RID: 9593 RVA: 0x000B13DC File Offset: 0x000AF7DC
	public void Zero()
	{
		this.m_roundTimer = 0f;
		this.Update();
	}

	// Token: 0x0600257A RID: 9594 RVA: 0x000B13F0 File Offset: 0x000AF7F0
	public virtual void Update()
	{
		this.m_suppressionController.UpdateSuppressors();
		if (!this.m_suppressionController.IsSuppressed() && !DebugManager.Instance.GetOption("Freeze time"))
		{
			this.m_roundTimer += TimeManager.GetDeltaTime(LayerMask.NameToLayer("Default"));
			this.m_roundTimer = Mathf.Clamp(this.m_roundTimer, 0f, 5999f);
			this.m_dataStore.Write(ClientUnlimitedRoundTimer.k_timeUpdatedId, this.m_roundTimer);
		}
	}

	// Token: 0x04001CFD RID: 7421
	protected const int k_timeMax = 5999;

	// Token: 0x04001CFE RID: 7422
	private DataStore m_dataStore;

	// Token: 0x04001CFF RID: 7423
	private static readonly DataStore.Id k_timeUpdatedId = new DataStore.Id("time.updated");

	// Token: 0x04001D00 RID: 7424
	protected float m_roundTimer;

	// Token: 0x04001D01 RID: 7425
	protected SuppressionController m_suppressionController = new SuppressionController();
}
