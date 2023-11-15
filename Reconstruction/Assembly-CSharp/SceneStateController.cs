using System;

// Token: 0x0200013E RID: 318
public class SceneStateController
{
	// Token: 0x0600083A RID: 2106 RVA: 0x0001579D File Offset: 0x0001399D
	public void SetState(ISceneState state)
	{
		if (this.m_RunBegin)
		{
			return;
		}
		DebugLog.Logger("EnterSceneState" + state.StateName);
		this.m_State = state;
		this.m_State.StateBegin();
		this.m_RunBegin = true;
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x000157D6 File Offset: 0x000139D6
	public void EndState()
	{
		this.m_RunBegin = false;
		if (this.m_State != null)
		{
			this.m_State.StateEnd();
			this.m_State = null;
		}
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x000157F9 File Offset: 0x000139F9
	public void StateUpdate()
	{
		if (this.m_State != null)
		{
			this.m_State.StateUpdate();
		}
	}

	// Token: 0x0400040F RID: 1039
	public ISceneState m_State;

	// Token: 0x04000410 RID: 1040
	private bool m_RunBegin;
}
