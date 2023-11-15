using System;

// Token: 0x02000419 RID: 1049
public struct SchedulerHandle
{
	// Token: 0x06001630 RID: 5680 RVA: 0x00074426 File Offset: 0x00072626
	public SchedulerHandle(Scheduler scheduler, SchedulerEntry entry)
	{
		this.entry = entry;
		this.scheduler = scheduler;
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x06001631 RID: 5681 RVA: 0x00074436 File Offset: 0x00072636
	public float TimeRemaining
	{
		get
		{
			if (!this.IsValid)
			{
				return -1f;
			}
			return this.entry.time - this.scheduler.GetTime();
		}
	}

	// Token: 0x06001632 RID: 5682 RVA: 0x0007445D File Offset: 0x0007265D
	public void FreeResources()
	{
		this.entry.FreeResources();
		this.scheduler = null;
	}

	// Token: 0x06001633 RID: 5683 RVA: 0x00074471 File Offset: 0x00072671
	public void ClearScheduler()
	{
		if (this.scheduler == null)
		{
			return;
		}
		this.scheduler.Clear(this);
		this.scheduler = null;
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x06001634 RID: 5684 RVA: 0x00074494 File Offset: 0x00072694
	public bool IsValid
	{
		get
		{
			return this.scheduler != null;
		}
	}

	// Token: 0x04000C5D RID: 3165
	public SchedulerEntry entry;

	// Token: 0x04000C5E RID: 3166
	private Scheduler scheduler;
}
