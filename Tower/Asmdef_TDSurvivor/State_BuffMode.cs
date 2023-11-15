using System;
using System.Collections;
using Lean.Touch;
using UnityEngine;

// Token: 0x02000095 RID: 149
public class State_BuffMode : AGameState
{
	// Token: 0x06000312 RID: 786 RVA: 0x0000C1A3 File Offset: 0x0000A3A3
	protected override void StateInitProc()
	{
		this.isFirstFrame = true;
		if (Input.GetMouseButtonDown(0))
		{
			this.isMouseDownOnEnter = true;
		}
	}

	// Token: 0x06000313 RID: 787 RVA: 0x0000C1BB File Offset: 0x0000A3BB
	protected override void StateEndProc()
	{
		EventMgr.SendEvent(eGameEvents.CancelBuffSelection);
	}

	// Token: 0x06000314 RID: 788 RVA: 0x0000C1CC File Offset: 0x0000A3CC
	protected override void StateUpdateProc(float deltaTime)
	{
		if (this.isFirstFrame)
		{
			this.isFirstFrame = false;
			return;
		}
		if (Input.GetMouseButtonUp(0) && !LeanTouch.PointOverGui(Input.mousePosition))
		{
			Debug.Log("放開左鍵");
			EventMgr.SendEvent(eGameEvents.ConfirmBuffSelection);
			EventMgr.SendEvent<eGameState>(eGameEvents.RequestChangeGameState, eGameState.NORMAL_MODE);
		}
		if (Input.GetMouseButtonDown(2) && !LeanTouch.PointOverGui(Input.mousePosition))
		{
			Debug.Log("按下右鍵");
			EventMgr.SendEvent(eGameEvents.CancelBuffSelection);
			EventMgr.SendEvent<eGameState>(eGameEvents.RequestChangeGameState, eGameState.NORMAL_MODE);
		}
	}

	// Token: 0x06000315 RID: 789 RVA: 0x0000C263 File Offset: 0x0000A463
	protected override IEnumerator StateProc()
	{
		yield return null;
		yield break;
	}

	// Token: 0x04000360 RID: 864
	private bool isFirstFrame = true;

	// Token: 0x04000361 RID: 865
	private bool isMouseDownOnEnter;
}
