using System;
using System.Diagnostics;

// Token: 0x02000AA5 RID: 2725
public abstract class KitchenTask : IKitchenTask
{
	// Token: 0x060035E0 RID: 13792 RVA: 0x000FB2EB File Offset: 0x000F96EB
	public KitchenTask()
	{
		this.m_status = KitchenTaskStatus.NotStarted;
		this.m_IPlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
	}

	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x060035E1 RID: 13793 RVA: 0x000FB305 File Offset: 0x000F9705
	public bool isRunning
	{
		get
		{
			return this.m_status == KitchenTaskStatus.Running;
		}
	}

	// Token: 0x14000034 RID: 52
	// (add) Token: 0x060035E2 RID: 13794 RVA: 0x000FB310 File Offset: 0x000F9710
	// (remove) Token: 0x060035E3 RID: 13795 RVA: 0x000FB348 File Offset: 0x000F9748
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event GenericVoid<KitchenTaskResult> onComplete;

	// Token: 0x060035E4 RID: 13796 RVA: 0x000FB37E File Offset: 0x000F977E
	public virtual void Start()
	{
	}

	// Token: 0x060035E5 RID: 13797 RVA: 0x000FB380 File Offset: 0x000F9780
	public virtual void CleanUp()
	{
		this.onComplete = null;
	}

	// Token: 0x060035E6 RID: 13798 RVA: 0x000FB389 File Offset: 0x000F9789
	public KitchenTaskStatus GetStatus()
	{
		return this.m_status;
	}

	// Token: 0x060035E7 RID: 13799 RVA: 0x000FB391 File Offset: 0x000F9791
	public virtual void Update()
	{
	}

	// Token: 0x060035E8 RID: 13800 RVA: 0x000FB393 File Offset: 0x000F9793
	protected virtual void TaskComplete(KitchenTaskResult result)
	{
		this.m_status = KitchenTaskStatus.Complete;
		if (this.onComplete != null)
		{
			this.onComplete(result);
		}
	}

	// Token: 0x04002B6B RID: 11115
	protected KitchenTaskStatus m_status;

	// Token: 0x04002B6C RID: 11116
	protected IPlayerManager m_IPlayerManager;
}
