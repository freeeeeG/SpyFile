using System;
using System.Collections;

// Token: 0x0200018B RID: 395
public class SelectingState : BattleOperationState
{
	// Token: 0x17000370 RID: 880
	// (get) Token: 0x06000A0D RID: 2573 RVA: 0x0001B628 File Offset: 0x00019828
	public override StateName StateName
	{
		get
		{
			return StateName.SelectingState;
		}
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x0001B62B File Offset: 0x0001982B
	public SelectingState(GameManager gameManager) : base(gameManager)
	{
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x0001B634 File Offset: 0x00019834
	public override IEnumerator EnterState()
	{
		this.gameManager.OperationState = this;
		yield break;
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x0001B643 File Offset: 0x00019843
	public override IEnumerator ExitState(BattleOperationState newState)
	{
		this.gameManager.StartCoroutine(newState.EnterState());
		yield break;
	}
}
