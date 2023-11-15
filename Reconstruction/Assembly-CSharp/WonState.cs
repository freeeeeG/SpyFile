using System;
using System.Collections;

// Token: 0x0200018E RID: 398
public class WonState : BattleOperationState
{
	// Token: 0x06000A18 RID: 2584 RVA: 0x0001B6B3 File Offset: 0x000198B3
	public WonState(GameManager gameManager) : base(gameManager)
	{
	}

	// Token: 0x17000373 RID: 883
	// (get) Token: 0x06000A19 RID: 2585 RVA: 0x0001B6BC File Offset: 0x000198BC
	public override StateName StateName
	{
		get
		{
			return StateName.WonState;
		}
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x0001B6BF File Offset: 0x000198BF
	public override IEnumerator EnterState()
	{
		this.gameManager.OperationState = this;
		Singleton<Sound>.Instance.PlayBg("Borner");
		yield return null;
		yield break;
	}
}
