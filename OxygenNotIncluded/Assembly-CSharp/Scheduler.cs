using System;
using UnityEngine;

// Token: 0x02000415 RID: 1045
public class Scheduler : IScheduler
{
	// Token: 0x17000091 RID: 145
	// (get) Token: 0x06001616 RID: 5654 RVA: 0x000740E1 File Offset: 0x000722E1
	public int Count
	{
		get
		{
			return this.entries.Count;
		}
	}

	// Token: 0x06001617 RID: 5655 RVA: 0x000740EE File Offset: 0x000722EE
	public Scheduler(SchedulerClock clock)
	{
		this.clock = clock;
	}

	// Token: 0x06001618 RID: 5656 RVA: 0x00074113 File Offset: 0x00072313
	public float GetTime()
	{
		return this.clock.GetTime();
	}

	// Token: 0x06001619 RID: 5657 RVA: 0x00074120 File Offset: 0x00072320
	private SchedulerHandle Schedule(SchedulerEntry entry)
	{
		this.entries.Enqueue(entry.time, entry);
		return new SchedulerHandle(this, entry);
	}

	// Token: 0x0600161A RID: 5658 RVA: 0x0007413C File Offset: 0x0007233C
	private SchedulerHandle Schedule(string name, float time, float time_interval, Action<object> callback, object callback_data, GameObject profiler_obj)
	{
		SchedulerEntry entry = new SchedulerEntry(name, time + this.clock.GetTime(), time_interval, callback, callback_data, profiler_obj);
		return this.Schedule(entry);
	}

	// Token: 0x0600161B RID: 5659 RVA: 0x0007416C File Offset: 0x0007236C
	public void FreeResources()
	{
		this.clock = null;
		if (this.entries != null)
		{
			while (this.entries.Count > 0)
			{
				this.entries.Dequeue().Value.FreeResources();
			}
		}
		this.entries = null;
	}

	// Token: 0x0600161C RID: 5660 RVA: 0x000741BC File Offset: 0x000723BC
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		if (group != null && group.scheduler != this)
		{
			global::Debug.LogError("Scheduler group mismatch!");
		}
		SchedulerHandle schedulerHandle = this.Schedule(name, time, -1f, callback, callback_data, null);
		if (group != null)
		{
			group.Add(schedulerHandle);
		}
		return schedulerHandle;
	}

	// Token: 0x0600161D RID: 5661 RVA: 0x00074200 File Offset: 0x00072400
	public void Clear(SchedulerHandle handle)
	{
		handle.entry.Clear();
	}

	// Token: 0x0600161E RID: 5662 RVA: 0x00074210 File Offset: 0x00072410
	public void Update()
	{
		if (this.Count == 0)
		{
			return;
		}
		int count = this.Count;
		int num = 0;
		using (new KProfiler.Region("Scheduler.Update", null))
		{
			float time = this.clock.GetTime();
			if (this.previousTime != time)
			{
				this.previousTime = time;
				while (num < count && time >= this.entries.Peek().Key)
				{
					SchedulerEntry value = this.entries.Dequeue().Value;
					if (value.callback != null)
					{
						value.callback(value.callbackData);
					}
					num++;
				}
			}
		}
	}

	// Token: 0x04000C56 RID: 3158
	public FloatHOTQueue<SchedulerEntry> entries = new FloatHOTQueue<SchedulerEntry>();

	// Token: 0x04000C57 RID: 3159
	private SchedulerClock clock;

	// Token: 0x04000C58 RID: 3160
	private float previousTime = float.NegativeInfinity;
}
