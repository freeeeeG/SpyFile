using System;
using UnityEngine;

// Token: 0x02000769 RID: 1897
public class ServerRoundTimer : IServerRoundTimer
{
	// Token: 0x06002479 RID: 9337 RVA: 0x000ABF88 File Offset: 0x000AA388
	public virtual void Initialise()
	{
		KitchenLevelConfigBase kitchenLevelConfigBase = GameUtils.GetLevelConfig() as KitchenLevelConfigBase;
		this.m_timeLimit = kitchenLevelConfigBase.GetTimeLimit();
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x0600247A RID: 9338 RVA: 0x000ABFAC File Offset: 0x000AA3AC
	public SuppressionController Suppressor
	{
		get
		{
			return this.m_suppressionController;
		}
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x0600247B RID: 9339 RVA: 0x000ABFB4 File Offset: 0x000AA3B4
	public bool IsSuppressed
	{
		get
		{
			return this.m_suppressionController.IsSuppressed();
		}
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x0600247C RID: 9340 RVA: 0x000ABFC1 File Offset: 0x000AA3C1
	public virtual float TimeElapsed
	{
		get
		{
			return this.m_roundTimer;
		}
	}

	// Token: 0x0600247D RID: 9341 RVA: 0x000ABFC9 File Offset: 0x000AA3C9
	public virtual bool TimeExpired()
	{
		return this.m_timeLeft <= 0;
	}

	// Token: 0x0600247E RID: 9342 RVA: 0x000ABFD8 File Offset: 0x000AA3D8
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
		}
	}

	// Token: 0x04001BE8 RID: 7144
	protected int m_timeLeft;

	// Token: 0x04001BE9 RID: 7145
	protected float m_roundTimer;

	// Token: 0x04001BEA RID: 7146
	protected float m_timeLimit;

	// Token: 0x04001BEB RID: 7147
	protected SuppressionController m_suppressionController = new SuppressionController();
}
