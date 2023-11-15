using System;

// Token: 0x02000414 RID: 1044
public interface IScheduler
{
	// Token: 0x06001615 RID: 5653
	SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null);
}
