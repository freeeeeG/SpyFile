using System;

// Token: 0x0200013C RID: 316
public class ISceneState
{
	// Token: 0x1700031E RID: 798
	// (get) Token: 0x0600082E RID: 2094 RVA: 0x000156AE File Offset: 0x000138AE
	// (set) Token: 0x0600082F RID: 2095 RVA: 0x000156B6 File Offset: 0x000138B6
	public string StateName
	{
		get
		{
			return this.m_StateName;
		}
		set
		{
			this.m_StateName = value;
		}
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x000156BF File Offset: 0x000138BF
	public ISceneState(SceneStateController Controller)
	{
		this.m_Controller = Controller;
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x000156D9 File Offset: 0x000138D9
	public virtual void StateBegin()
	{
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x000156DB File Offset: 0x000138DB
	public virtual void StateEnd()
	{
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x000156DD File Offset: 0x000138DD
	public virtual void StateUpdate()
	{
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x000156DF File Offset: 0x000138DF
	public override string ToString()
	{
		return string.Format("[I_SceneState: StateName={0}]", this.StateName);
	}

	// Token: 0x0400040D RID: 1037
	private string m_StateName = "ISceneState";

	// Token: 0x0400040E RID: 1038
	protected SceneStateController m_Controller;
}
