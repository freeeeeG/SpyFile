using System;
using UnityEngine;

// Token: 0x02000417 RID: 1047
public struct SchedulerEntry
{
	// Token: 0x17000092 RID: 146
	// (get) Token: 0x06001621 RID: 5665 RVA: 0x000742D8 File Offset: 0x000724D8
	// (set) Token: 0x06001622 RID: 5666 RVA: 0x000742E0 File Offset: 0x000724E0
	public SchedulerEntry.Details details { readonly get; private set; }

	// Token: 0x06001623 RID: 5667 RVA: 0x000742E9 File Offset: 0x000724E9
	public SchedulerEntry(string name, float time, float time_interval, Action<object> callback, object callback_data, GameObject profiler_obj)
	{
		this.time = time;
		this.details = new SchedulerEntry.Details(name, callback, callback_data, time_interval, profiler_obj);
	}

	// Token: 0x06001624 RID: 5668 RVA: 0x00074305 File Offset: 0x00072505
	public void FreeResources()
	{
		this.details = null;
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x06001625 RID: 5669 RVA: 0x0007430E File Offset: 0x0007250E
	public Action<object> callback
	{
		get
		{
			return this.details.callback;
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x06001626 RID: 5670 RVA: 0x0007431B File Offset: 0x0007251B
	public object callbackData
	{
		get
		{
			return this.details.callbackData;
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x06001627 RID: 5671 RVA: 0x00074328 File Offset: 0x00072528
	public float timeInterval
	{
		get
		{
			return this.details.timeInterval;
		}
	}

	// Token: 0x06001628 RID: 5672 RVA: 0x00074335 File Offset: 0x00072535
	public override string ToString()
	{
		return this.time.ToString();
	}

	// Token: 0x06001629 RID: 5673 RVA: 0x00074342 File Offset: 0x00072542
	public void Clear()
	{
		this.details.callback = null;
	}

	// Token: 0x04000C59 RID: 3161
	public float time;

	// Token: 0x02001095 RID: 4245
	public class Details
	{
		// Token: 0x06007622 RID: 30242 RVA: 0x002CE9CF File Offset: 0x002CCBCF
		public Details(string name, Action<object> callback, object callback_data, float time_interval, GameObject profiler_obj)
		{
			this.timeInterval = time_interval;
			this.callback = callback;
			this.callbackData = callback_data;
		}

		// Token: 0x0400599C RID: 22940
		public Action<object> callback;

		// Token: 0x0400599D RID: 22941
		public object callbackData;

		// Token: 0x0400599E RID: 22942
		public float timeInterval;
	}
}
