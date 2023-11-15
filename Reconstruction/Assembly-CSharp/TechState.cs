using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200018C RID: 396
public class TechState : BattleOperationState
{
	// Token: 0x06000A11 RID: 2577 RVA: 0x0001B659 File Offset: 0x00019859
	public TechState(GameManager gameManager) : base(gameManager)
	{
	}

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x06000A12 RID: 2578 RVA: 0x0001B662 File Offset: 0x00019862
	public override StateName StateName
	{
		get
		{
			return StateName.TechState;
		}
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x0001B665 File Offset: 0x00019865
	public override IEnumerator EnterState()
	{
		yield return new WaitForSeconds(0.5f);
		this.gameManager.ShowTechSelect(true, false);
		Singleton<Sound>.Instance.PlayUISound("Sound_Tips");
		Singleton<Sound>.Instance.PlayBg("Music_Preparing");
		yield return null;
		yield break;
	}
}
