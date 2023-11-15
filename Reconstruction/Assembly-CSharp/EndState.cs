using System;
using System.Collections;

// Token: 0x02000189 RID: 393
public class EndState : BattleOperationState
{
	// Token: 0x06000A06 RID: 2566 RVA: 0x0001B5D5 File Offset: 0x000197D5
	public EndState(GameManager gameManager) : base(gameManager)
	{
	}

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x06000A07 RID: 2567 RVA: 0x0001B5DE File Offset: 0x000197DE
	public override StateName StateName
	{
		get
		{
			return StateName.LoseState;
		}
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x0001B5E1 File Offset: 0x000197E1
	public override IEnumerator EnterState()
	{
		this.gameManager.OperationState = this;
		Singleton<Sound>.Instance.PlayBg("lastwave");
		yield return null;
		yield break;
	}
}
