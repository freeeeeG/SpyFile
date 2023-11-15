using System;
using UnityEngine;

// Token: 0x02000413 RID: 1043
[AddComponentMenu("KMonoBehaviour/scripts/GameScheduler")]
public class GameScheduler : KMonoBehaviour, IScheduler
{
	// Token: 0x0600160C RID: 5644 RVA: 0x00074048 File Offset: 0x00072248
	public static void DestroyInstance()
	{
		GameScheduler.Instance = null;
	}

	// Token: 0x0600160D RID: 5645 RVA: 0x00074050 File Offset: 0x00072250
	protected override void OnPrefabInit()
	{
		GameScheduler.Instance = this;
		Singleton<StateMachineManager>.Instance.RegisterScheduler(this.scheduler);
	}

	// Token: 0x0600160E RID: 5646 RVA: 0x00074068 File Offset: 0x00072268
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, time, callback, callback_data, group);
	}

	// Token: 0x0600160F RID: 5647 RVA: 0x0007407C File Offset: 0x0007227C
	public SchedulerHandle ScheduleNextFrame(string name, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, 0f, callback, callback_data, group);
	}

	// Token: 0x06001610 RID: 5648 RVA: 0x00074093 File Offset: 0x00072293
	private void Update()
	{
		this.scheduler.Update();
	}

	// Token: 0x06001611 RID: 5649 RVA: 0x000740A0 File Offset: 0x000722A0
	protected override void OnLoadLevel()
	{
		this.scheduler.FreeResources();
		this.scheduler = null;
	}

	// Token: 0x06001612 RID: 5650 RVA: 0x000740B4 File Offset: 0x000722B4
	public SchedulerGroup CreateGroup()
	{
		return new SchedulerGroup(this.scheduler);
	}

	// Token: 0x06001613 RID: 5651 RVA: 0x000740C1 File Offset: 0x000722C1
	public Scheduler GetScheduler()
	{
		return this.scheduler;
	}

	// Token: 0x04000C54 RID: 3156
	private Scheduler scheduler = new Scheduler(new GameScheduler.GameSchedulerClock());

	// Token: 0x04000C55 RID: 3157
	public static GameScheduler Instance;

	// Token: 0x02001094 RID: 4244
	public class GameSchedulerClock : SchedulerClock
	{
		// Token: 0x06007620 RID: 30240 RVA: 0x002CE9BB File Offset: 0x002CCBBB
		public override float GetTime()
		{
			return GameClock.Instance.GetTime();
		}
	}
}
