using System;
using System.Collections.Generic;

// Token: 0x02000418 RID: 1048
public class SchedulerGroup
{
	// Token: 0x17000096 RID: 150
	// (get) Token: 0x0600162A RID: 5674 RVA: 0x00074350 File Offset: 0x00072550
	// (set) Token: 0x0600162B RID: 5675 RVA: 0x00074358 File Offset: 0x00072558
	public Scheduler scheduler { get; private set; }

	// Token: 0x0600162C RID: 5676 RVA: 0x00074361 File Offset: 0x00072561
	public SchedulerGroup(Scheduler scheduler)
	{
		this.scheduler = scheduler;
		this.Reset();
	}

	// Token: 0x0600162D RID: 5677 RVA: 0x00074381 File Offset: 0x00072581
	public void FreeResources()
	{
		if (this.scheduler != null)
		{
			this.scheduler.FreeResources();
		}
		this.scheduler = null;
		if (this.handles != null)
		{
			this.handles.Clear();
		}
		this.handles = null;
	}

	// Token: 0x0600162E RID: 5678 RVA: 0x000743B8 File Offset: 0x000725B8
	public void Reset()
	{
		foreach (SchedulerHandle schedulerHandle in this.handles)
		{
			schedulerHandle.ClearScheduler();
		}
		this.handles.Clear();
	}

	// Token: 0x0600162F RID: 5679 RVA: 0x00074418 File Offset: 0x00072618
	public void Add(SchedulerHandle handle)
	{
		this.handles.Add(handle);
	}

	// Token: 0x04000C5C RID: 3164
	private List<SchedulerHandle> handles = new List<SchedulerHandle>();
}
