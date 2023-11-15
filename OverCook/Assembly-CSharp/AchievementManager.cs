using System;

// Token: 0x020006EE RID: 1774
public abstract class AchievementManager : Manager
{
	// Token: 0x060021A4 RID: 8612 RVA: 0x000A2ED3 File Offset: 0x000A12D3
	protected virtual void Awake()
	{
		this.m_playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		this.m_StatSystem = base.gameObject.RequireComponent<StatSystem>();
	}

	// Token: 0x060021A5 RID: 8613 RVA: 0x000A2EF1 File Offset: 0x000A12F1
	public virtual void Init()
	{
	}

	// Token: 0x060021A6 RID: 8614 RVA: 0x000A2EF3 File Offset: 0x000A12F3
	public virtual void Unload()
	{
	}

	// Token: 0x060021A7 RID: 8615 RVA: 0x000A2EF5 File Offset: 0x000A12F5
	protected virtual void OnDestroy()
	{
	}

	// Token: 0x060021A8 RID: 8616 RVA: 0x000A2EF7 File Offset: 0x000A12F7
	protected virtual void SetProgress(int trophyId, float progress)
	{
	}

	// Token: 0x060021A9 RID: 8617 RVA: 0x000A2EF9 File Offset: 0x000A12F9
	protected virtual void Unlock(int trophyId)
	{
	}

	// Token: 0x040019D3 RID: 6611
	protected IPlayerManager m_playerManager;

	// Token: 0x040019D4 RID: 6612
	protected StatSystem m_StatSystem;
}
