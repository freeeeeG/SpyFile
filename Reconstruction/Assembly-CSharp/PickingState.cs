using System;
using System.Collections;

// Token: 0x0200018A RID: 394
public class PickingState : BattleOperationState
{
	// Token: 0x1700036F RID: 879
	// (get) Token: 0x06000A09 RID: 2569 RVA: 0x0001B5F0 File Offset: 0x000197F0
	public override StateName StateName
	{
		get
		{
			return StateName.PickingState;
		}
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x0001B5F3 File Offset: 0x000197F3
	public PickingState(GameManager gameManager, FuncUI funcUI) : base(gameManager)
	{
		this.m_FuncUI = funcUI;
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x0001B603 File Offset: 0x00019803
	public override IEnumerator EnterState()
	{
		this.gameManager.OperationState = this;
		this.m_FuncUI.Hide();
		yield break;
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x0001B612 File Offset: 0x00019812
	public override IEnumerator ExitState(BattleOperationState newState)
	{
		this.gameManager.StartCoroutine(newState.EnterState());
		yield break;
	}

	// Token: 0x04000556 RID: 1366
	private FuncUI m_FuncUI;
}
