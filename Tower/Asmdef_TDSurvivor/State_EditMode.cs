using System;
using System.Collections;
using Lean.Touch;
using UnityEngine;

// Token: 0x02000096 RID: 150
public class State_EditMode : AGameState
{
	// Token: 0x06000317 RID: 791 RVA: 0x0000C27A File Offset: 0x0000A47A
	protected override void StateInitProc()
	{
		this.isFirstFrame = true;
		if (Input.GetMouseButtonDown(0))
		{
			this.isMouseDownOnEnter = true;
		}
	}

	// Token: 0x06000318 RID: 792 RVA: 0x0000C292 File Offset: 0x0000A492
	protected override void StateEndProc()
	{
	}

	// Token: 0x06000319 RID: 793 RVA: 0x0000C294 File Offset: 0x0000A494
	protected override void StateUpdateProc(float deltaTime)
	{
		if (this.isFirstFrame)
		{
			this.isFirstFrame = false;
			return;
		}
		if (Input.GetMouseButtonUp(0) && !LeanTouch.PointOverGui(Input.mousePosition))
		{
			EventMgr.SendEvent<eGameState>(eGameEvents.RequestChangeGameState, eGameState.NORMAL_MODE);
			EventMgr.SendEvent(eGameEvents.ConfirmPlacement);
		}
		if ((Input.GetMouseButtonDown(2) && !LeanTouch.PointOverGui(Input.mousePosition)) || Input.GetKeyDown(KeyCode.Escape))
		{
			EventMgr.SendEvent<eGameState>(eGameEvents.RequestChangeGameState, eGameState.NORMAL_MODE);
			EventMgr.SendEvent(eGameEvents.CancelPlacement);
		}
	}

	// Token: 0x0600031A RID: 794 RVA: 0x0000C320 File Offset: 0x0000A520
	protected override IEnumerator StateProc()
	{
		yield return null;
		yield break;
	}

	// Token: 0x04000362 RID: 866
	private bool isFirstFrame = true;

	// Token: 0x04000363 RID: 867
	private bool isMouseDownOnEnter;
}
