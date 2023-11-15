using System;
using System.Collections;

// Token: 0x02000099 RID: 153
public class State_PauseGame : AGameState
{
	// Token: 0x06000326 RID: 806 RVA: 0x0000C379 File Offset: 0x0000A579
	protected override void StateInitProc()
	{
		EventMgr.SendEvent(eGameEvents.CancelPlacement);
		EventMgr.SendEvent(eGameEvents.OnPauseGame);
		EventMgr.SendEvent<bool>(eGameEvents.UI_TogglePauseMenu, true);
		EventMgr.SendEvent<float>(eGameEvents.RequestModifySystemGameSpeed, 0f);
	}

	// Token: 0x06000327 RID: 807 RVA: 0x0000C3B5 File Offset: 0x0000A5B5
	protected override void StateEndProc()
	{
		EventMgr.SendEvent<float>(eGameEvents.RequestModifySystemGameSpeed, 1f);
		EventMgr.SendEvent<bool>(eGameEvents.UI_TogglePauseMenu, false);
	}

	// Token: 0x06000328 RID: 808 RVA: 0x0000C3D7 File Offset: 0x0000A5D7
	protected override void StateUpdateProc(float deltaTime)
	{
	}

	// Token: 0x06000329 RID: 809 RVA: 0x0000C3D9 File Offset: 0x0000A5D9
	protected override IEnumerator StateProc()
	{
		SingleEventCapturer sc = new SingleEventCapturer(eGameEvents.RequestEndPause, null);
		while (!sc.IsEventReceived)
		{
			yield return null;
		}
		EventMgr.SendEvent<eGameState>(eGameEvents.RequestChangeGameState, eGameState.NORMAL_MODE);
		yield break;
	}
}
