using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class State_NormalMode : AGameState
{
	// Token: 0x06000321 RID: 801 RVA: 0x0000C34D File Offset: 0x0000A54D
	protected override void StateInitProc()
	{
	}

	// Token: 0x06000322 RID: 802 RVA: 0x0000C34F File Offset: 0x0000A54F
	protected override void StateEndProc()
	{
	}

	// Token: 0x06000323 RID: 803 RVA: 0x0000C351 File Offset: 0x0000A551
	protected override void StateUpdateProc(float deltaTime)
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			EventMgr.SendEvent<eGameState>(eGameEvents.RequestChangeGameState, eGameState.PAUSE_GAME);
		}
	}

	// Token: 0x06000324 RID: 804 RVA: 0x0000C369 File Offset: 0x0000A569
	protected override IEnumerator StateProc()
	{
		yield return null;
		yield break;
	}
}
