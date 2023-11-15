using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FMOD.Studio;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200095D RID: 2397
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleManager")]
public class ScheduleManager : KMonoBehaviour, ISim33ms
{
	// Token: 0x1400001E RID: 30
	// (add) Token: 0x0600466D RID: 18029 RVA: 0x0018E458 File Offset: 0x0018C658
	// (remove) Token: 0x0600466E RID: 18030 RVA: 0x0018E490 File Offset: 0x0018C690
	public event Action<List<Schedule>> onSchedulesChanged;

	// Token: 0x0600466F RID: 18031 RVA: 0x0018E4C5 File Offset: 0x0018C6C5
	public static void DestroyInstance()
	{
		ScheduleManager.Instance = null;
	}

	// Token: 0x06004670 RID: 18032 RVA: 0x0018E4CD File Offset: 0x0018C6CD
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.schedules.Count == 0)
		{
			this.AddDefaultSchedule(true);
		}
	}

	// Token: 0x06004671 RID: 18033 RVA: 0x0018E4E3 File Offset: 0x0018C6E3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.schedules = new List<Schedule>();
		ScheduleManager.Instance = this;
	}

	// Token: 0x06004672 RID: 18034 RVA: 0x0018E4FC File Offset: 0x0018C6FC
	protected override void OnSpawn()
	{
		if (this.schedules.Count == 0)
		{
			this.AddDefaultSchedule(true);
		}
		foreach (Schedule schedule in this.schedules)
		{
			schedule.ClearNullReferences();
		}
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			Schedulable component = minionIdentity.GetComponent<Schedulable>();
			if (this.GetSchedule(component) == null)
			{
				this.schedules[0].Assign(component);
			}
		}
		Components.LiveMinionIdentities.OnAdd += this.OnAddDupe;
		Components.LiveMinionIdentities.OnRemove += this.OnRemoveDupe;
	}

	// Token: 0x06004673 RID: 18035 RVA: 0x0018E5EC File Offset: 0x0018C7EC
	private void OnAddDupe(MinionIdentity minion)
	{
		Schedulable component = minion.GetComponent<Schedulable>();
		if (this.GetSchedule(component) == null)
		{
			this.schedules[0].Assign(component);
		}
	}

	// Token: 0x06004674 RID: 18036 RVA: 0x0018E61C File Offset: 0x0018C81C
	private void OnRemoveDupe(MinionIdentity minion)
	{
		Schedulable component = minion.GetComponent<Schedulable>();
		Schedule schedule = this.GetSchedule(component);
		if (schedule != null)
		{
			schedule.Unassign(component);
		}
	}

	// Token: 0x06004675 RID: 18037 RVA: 0x0018E644 File Offset: 0x0018C844
	public void OnStoredDupeDestroyed(StoredMinionIdentity dupe)
	{
		foreach (Schedule schedule in this.schedules)
		{
			schedule.Unassign(dupe.gameObject.GetComponent<Schedulable>());
		}
	}

	// Token: 0x06004676 RID: 18038 RVA: 0x0018E6A0 File Offset: 0x0018C8A0
	public void AddDefaultSchedule(bool alarmOn)
	{
		Schedule schedule = this.AddSchedule(Db.Get().ScheduleGroups.allGroups, UI.SCHEDULESCREEN.SCHEDULE_NAME_DEFAULT, alarmOn);
		if (Game.Instance.FastWorkersModeActive)
		{
			for (int i = 0; i < 21; i++)
			{
				schedule.SetGroup(i, Db.Get().ScheduleGroups.Worktime);
			}
			schedule.SetGroup(21, Db.Get().ScheduleGroups.Recreation);
			schedule.SetGroup(22, Db.Get().ScheduleGroups.Recreation);
			schedule.SetGroup(23, Db.Get().ScheduleGroups.Sleep);
		}
	}

	// Token: 0x06004677 RID: 18039 RVA: 0x0018E744 File Offset: 0x0018C944
	public Schedule AddSchedule(List<ScheduleGroup> groups, string name = null, bool alarmOn = false)
	{
		this.scheduleNameIncrementor++;
		if (name == null)
		{
			name = string.Format(UI.SCHEDULESCREEN.SCHEDULE_NAME_FORMAT, this.scheduleNameIncrementor.ToString());
		}
		Schedule schedule = new Schedule(name, groups, alarmOn);
		this.schedules.Add(schedule);
		if (this.onSchedulesChanged != null)
		{
			this.onSchedulesChanged(this.schedules);
		}
		return schedule;
	}

	// Token: 0x06004678 RID: 18040 RVA: 0x0018E7B0 File Offset: 0x0018C9B0
	public void DeleteSchedule(Schedule schedule)
	{
		if (this.schedules.Count == 1)
		{
			return;
		}
		List<Ref<Schedulable>> assigned = schedule.GetAssigned();
		this.schedules.Remove(schedule);
		foreach (Ref<Schedulable> @ref in assigned)
		{
			this.schedules[0].Assign(@ref.Get());
		}
		if (this.onSchedulesChanged != null)
		{
			this.onSchedulesChanged(this.schedules);
		}
	}

	// Token: 0x06004679 RID: 18041 RVA: 0x0018E848 File Offset: 0x0018CA48
	public Schedule GetSchedule(Schedulable schedulable)
	{
		foreach (Schedule schedule in this.schedules)
		{
			if (schedule.IsAssigned(schedulable))
			{
				return schedule;
			}
		}
		return null;
	}

	// Token: 0x0600467A RID: 18042 RVA: 0x0018E8A4 File Offset: 0x0018CAA4
	public List<Schedule> GetSchedules()
	{
		return this.schedules;
	}

	// Token: 0x0600467B RID: 18043 RVA: 0x0018E8AC File Offset: 0x0018CAAC
	public bool IsAllowed(Schedulable schedulable, ScheduleBlockType schedule_block_type)
	{
		int blockIdx = Schedule.GetBlockIdx();
		Schedule schedule = this.GetSchedule(schedulable);
		return schedule != null && schedule.GetBlock(blockIdx).IsAllowed(schedule_block_type);
	}

	// Token: 0x0600467C RID: 18044 RVA: 0x0018E8DC File Offset: 0x0018CADC
	public void Sim33ms(float dt)
	{
		int blockIdx = Schedule.GetBlockIdx();
		if (blockIdx != this.lastIdx)
		{
			foreach (Schedule schedule in this.schedules)
			{
				schedule.Tick();
			}
			this.lastIdx = blockIdx;
		}
	}

	// Token: 0x0600467D RID: 18045 RVA: 0x0018E944 File Offset: 0x0018CB44
	public void PlayScheduleAlarm(Schedule schedule, ScheduleBlock block, bool forwards)
	{
		Notification notification = new Notification(string.Format(MISC.NOTIFICATIONS.SCHEDULE_CHANGED.NAME, schedule.name, block.name), NotificationType.Good, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.SCHEDULE_CHANGED.TOOLTIP.Replace("{0}", schedule.name).Replace("{1}", block.name).Replace("{2}", Db.Get().ScheduleGroups.Get(block.GroupId).notificationTooltip), null, true, 0f, null, null, null, true, false, false);
		base.GetComponent<Notifier>().Add(notification, "");
		base.StartCoroutine(this.PlayScheduleTone(schedule, forwards));
	}

	// Token: 0x0600467E RID: 18046 RVA: 0x0018E9CF File Offset: 0x0018CBCF
	private IEnumerator PlayScheduleTone(Schedule schedule, bool forwards)
	{
		int[] tones = schedule.GetTones();
		int num2;
		for (int i = 0; i < tones.Length; i = num2 + 1)
		{
			int num = forwards ? i : (tones.Length - 1 - i);
			this.PlayTone(tones[num], forwards);
			yield return SequenceUtil.WaitForSeconds(TuningData<ScheduleManager.Tuning>.Get().toneSpacingSeconds);
			num2 = i;
		}
		yield break;
	}

	// Token: 0x0600467F RID: 18047 RVA: 0x0018E9EC File Offset: 0x0018CBEC
	private void PlayTone(int pitch, bool forwards)
	{
		EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("WorkChime_tone", false), Vector3.zero, 1f);
		instance.setParameterByName("WorkChime_pitch", (float)pitch, false);
		instance.setParameterByName("WorkChime_start", (float)(forwards ? 1 : 0), false);
		KFMOD.EndOneShot(instance);
	}

	// Token: 0x04002EA6 RID: 11942
	[Serialize]
	private List<Schedule> schedules;

	// Token: 0x04002EA7 RID: 11943
	[Serialize]
	private int lastIdx;

	// Token: 0x04002EA8 RID: 11944
	[Serialize]
	private int scheduleNameIncrementor;

	// Token: 0x04002EAA RID: 11946
	public static ScheduleManager Instance;

	// Token: 0x020017C4 RID: 6084
	public class Tuning : TuningData<ScheduleManager.Tuning>
	{
		// Token: 0x04006FD5 RID: 28629
		public float toneSpacingSeconds;

		// Token: 0x04006FD6 RID: 28630
		public int minToneIndex;

		// Token: 0x04006FD7 RID: 28631
		public int maxToneIndex;

		// Token: 0x04006FD8 RID: 28632
		public int firstLastToneSpacing;
	}
}
