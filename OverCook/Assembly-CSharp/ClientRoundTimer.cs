using System;
using UnityEngine;

// Token: 0x0200076A RID: 1898
public class ClientRoundTimer : IClientRoundTimer
{
	// Token: 0x06002480 RID: 9344 RVA: 0x000AC130 File Offset: 0x000AA530
	public virtual void Initialise()
	{
		KitchenLevelConfigBase kitchenLevelConfigBase = GameUtils.GetLevelConfig() as KitchenLevelConfigBase;
		this.m_timeLimit = kitchenLevelConfigBase.GetTimeLimit();
		this.m_dataStore = GameUtils.RequireManager<DataStore>();
		this.m_dataStore.Write(ClientRoundTimer.k_timeUpdatedId, this.m_timeLimit);
	}

	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x06002481 RID: 9345 RVA: 0x000AC17A File Offset: 0x000AA57A
	public SuppressionController Suppressor
	{
		get
		{
			return this.m_suppressionController;
		}
	}

	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x06002482 RID: 9346 RVA: 0x000AC182 File Offset: 0x000AA582
	public bool IsSuppressed
	{
		get
		{
			return this.m_suppressionController.IsSuppressed();
		}
	}

	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x06002483 RID: 9347 RVA: 0x000AC18F File Offset: 0x000AA58F
	public virtual float TimeElapsed
	{
		get
		{
			return this.m_roundTimer;
		}
	}

	// Token: 0x06002484 RID: 9348 RVA: 0x000AC197 File Offset: 0x000AA597
	public virtual void Zero()
	{
		this.m_roundTimer = this.m_timeLimit;
		this.Update();
	}

	// Token: 0x06002485 RID: 9349 RVA: 0x000AC1AC File Offset: 0x000AA5AC
	public virtual void Update()
	{
		this.m_suppressionController.UpdateSuppressors();
		if (!this.m_suppressionController.IsSuppressed() && !DebugManager.Instance.GetOption("Freeze time"))
		{
			this.m_roundTimer += TimeManager.GetDeltaTime(LayerMask.NameToLayer("Default"));
		}
		int num = Mathf.CeilToInt(Mathf.Max(this.m_timeLimit - this.m_roundTimer, 0f));
		if (num != this.m_timeLeft)
		{
			this.m_timeLeft = num;
			this.m_dataStore.Write(ClientRoundTimer.k_timeUpdatedId, (float)this.m_timeLeft);
		}
	}

	// Token: 0x04001BEC RID: 7148
	protected DataStore m_dataStore;

	// Token: 0x04001BED RID: 7149
	protected static readonly DataStore.Id k_timeUpdatedId = new DataStore.Id("time.updated");

	// Token: 0x04001BEE RID: 7150
	protected int m_timeLeft;

	// Token: 0x04001BEF RID: 7151
	protected float m_roundTimer;

	// Token: 0x04001BF0 RID: 7152
	protected float m_timeLimit;

	// Token: 0x04001BF1 RID: 7153
	protected SuppressionController m_suppressionController = new SuppressionController();
}
