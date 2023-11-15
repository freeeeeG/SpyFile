using System;
using UnityEngine;

// Token: 0x02000790 RID: 1936
public class ServerUnlimitedRoundTimer : IServerRoundTimer
{
	// Token: 0x0600256E RID: 9582 RVA: 0x000B1309 File Offset: 0x000AF709
	public virtual void Initialise()
	{
	}

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x0600256F RID: 9583 RVA: 0x000B130B File Offset: 0x000AF70B
	public SuppressionController Suppressor
	{
		get
		{
			return this.m_suppressionController;
		}
	}

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x06002570 RID: 9584 RVA: 0x000B1313 File Offset: 0x000AF713
	public bool IsSuppressed
	{
		get
		{
			return this.m_suppressionController.IsSuppressed();
		}
	}

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x06002571 RID: 9585 RVA: 0x000B1320 File Offset: 0x000AF720
	public float TimeElapsed
	{
		get
		{
			return this.m_roundTimer;
		}
	}

	// Token: 0x06002572 RID: 9586 RVA: 0x000B1328 File Offset: 0x000AF728
	public bool TimeExpired()
	{
		return false;
	}

	// Token: 0x06002573 RID: 9587 RVA: 0x000B132C File Offset: 0x000AF72C
	public virtual void Update()
	{
		this.m_suppressionController.UpdateSuppressors();
		if (!this.m_suppressionController.IsSuppressed() && !DebugManager.Instance.GetOption("Freeze time"))
		{
			this.m_roundTimer += TimeManager.GetDeltaTime(LayerMask.NameToLayer("Default"));
			this.m_roundTimer = Mathf.Clamp(this.m_roundTimer, 0f, 5999f);
		}
	}

	// Token: 0x04001CFA RID: 7418
	protected const int k_timeMax = 5999;

	// Token: 0x04001CFB RID: 7419
	protected float m_roundTimer;

	// Token: 0x04001CFC RID: 7420
	protected SuppressionController m_suppressionController = new SuppressionController();
}
