using System;
using UnityEngine;

// Token: 0x02000C88 RID: 3208
[AddComponentMenu("KMonoBehaviour/scripts/UIScheduler")]
public class UIScheduler : KMonoBehaviour, IScheduler
{
	// Token: 0x06006641 RID: 26177 RVA: 0x002627DD File Offset: 0x002609DD
	public static void DestroyInstance()
	{
		UIScheduler.Instance = null;
	}

	// Token: 0x06006642 RID: 26178 RVA: 0x002627E5 File Offset: 0x002609E5
	protected override void OnPrefabInit()
	{
		UIScheduler.Instance = this;
	}

	// Token: 0x06006643 RID: 26179 RVA: 0x002627ED File Offset: 0x002609ED
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, time, callback, callback_data, group);
	}

	// Token: 0x06006644 RID: 26180 RVA: 0x00262801 File Offset: 0x00260A01
	public SchedulerHandle ScheduleNextFrame(string name, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, 0f, callback, callback_data, group);
	}

	// Token: 0x06006645 RID: 26181 RVA: 0x00262818 File Offset: 0x00260A18
	private void Update()
	{
		this.scheduler.Update();
	}

	// Token: 0x06006646 RID: 26182 RVA: 0x00262825 File Offset: 0x00260A25
	protected override void OnLoadLevel()
	{
		this.scheduler.FreeResources();
		this.scheduler = null;
	}

	// Token: 0x06006647 RID: 26183 RVA: 0x00262839 File Offset: 0x00260A39
	public SchedulerGroup CreateGroup()
	{
		return new SchedulerGroup(this.scheduler);
	}

	// Token: 0x06006648 RID: 26184 RVA: 0x00262846 File Offset: 0x00260A46
	public Scheduler GetScheduler()
	{
		return this.scheduler;
	}

	// Token: 0x0400466E RID: 18030
	private Scheduler scheduler = new Scheduler(new UIScheduler.UISchedulerClock());

	// Token: 0x0400466F RID: 18031
	public static UIScheduler Instance;

	// Token: 0x02001BB5 RID: 7093
	public class UISchedulerClock : SchedulerClock
	{
		// Token: 0x06009AC2 RID: 39618 RVA: 0x00347472 File Offset: 0x00345672
		public override float GetTime()
		{
			return Time.unscaledTime;
		}
	}
}
