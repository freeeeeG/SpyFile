using System;
using UnityEngine;

// Token: 0x02000754 RID: 1876
public class ServerModifiableRoundTimer : ServerRoundTimer
{
	// Token: 0x0600240F RID: 9231 RVA: 0x000AC068 File Offset: 0x000AA468
	public override void Initialise()
	{
		base.Initialise();
		this.m_roundTimer = this.m_timeLimit;
	}

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06002410 RID: 9232 RVA: 0x000AC07C File Offset: 0x000AA47C
	public override float TimeElapsed
	{
		get
		{
			return this.m_roundTimer;
		}
	}

	// Token: 0x06002411 RID: 9233 RVA: 0x000AC084 File Offset: 0x000AA484
	public override bool TimeExpired()
	{
		return this.m_roundTimer <= 0f;
	}

	// Token: 0x06002412 RID: 9234 RVA: 0x000AC096 File Offset: 0x000AA496
	public void AddTime(int time)
	{
		this.m_roundTimer += (float)time;
		this.m_roundTimer = Mathf.Clamp(this.m_roundTimer, 0f, 5999f);
	}

	// Token: 0x06002413 RID: 9235 RVA: 0x000AC0C4 File Offset: 0x000AA4C4
	public override void Update()
	{
		this.m_suppressionController.UpdateSuppressors();
		if (!this.m_suppressionController.IsSuppressed() && !DebugManager.Instance.GetOption("Freeze time"))
		{
			this.m_roundTimer -= TimeManager.GetDeltaTime(LayerMask.NameToLayer("Default"));
		}
	}

	// Token: 0x04001B91 RID: 7057
	protected const int k_timeMax = 5999;
}
