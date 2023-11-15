using System;
using System.Collections;

// Token: 0x02000187 RID: 391
public abstract class BattleOperationState
{
	// Token: 0x1700036C RID: 876
	// (get) Token: 0x060009FD RID: 2557
	public abstract StateName StateName { get; }

	// Token: 0x060009FE RID: 2558 RVA: 0x0001B565 File Offset: 0x00019765
	public BattleOperationState(GameManager gameManager)
	{
		this.gameManager = gameManager;
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x0001B574 File Offset: 0x00019774
	public virtual IEnumerator EnterState()
	{
		yield break;
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x0001B57C File Offset: 0x0001977C
	public virtual void StateUpdate()
	{
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x0001B57E File Offset: 0x0001977E
	public virtual IEnumerator ExitState(BattleOperationState newState)
	{
		yield break;
	}

	// Token: 0x04000551 RID: 1361
	protected GameManager gameManager;
}
