using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200095C RID: 2396
[SerializationConfig(MemberSerialization.OptIn)]
public class Schedule : ISaveLoadable, IListableOption
{
	// Token: 0x0600465B RID: 18011 RVA: 0x0018DF20 File Offset: 0x0018C120
	public static int GetBlockIdx()
	{
		return Math.Min((int)(GameClock.Instance.GetCurrentCycleAsPercentage() * 24f), 23);
	}

	// Token: 0x0600465C RID: 18012 RVA: 0x0018DF3A File Offset: 0x0018C13A
	public static int GetLastBlockIdx()
	{
		return (Schedule.GetBlockIdx() + 24 - 1) % 24;
	}

	// Token: 0x0600465D RID: 18013 RVA: 0x0018DF49 File Offset: 0x0018C149
	public void ClearNullReferences()
	{
		this.assigned.RemoveAll((Ref<Schedulable> x) => x.Get() == null);
	}

	// Token: 0x0600465E RID: 18014 RVA: 0x0018DF78 File Offset: 0x0018C178
	public Schedule(string name, List<ScheduleGroup> defaultGroups, bool alarmActivated)
	{
		this.name = name;
		this.alarmActivated = alarmActivated;
		this.blocks = new List<ScheduleBlock>(24);
		this.assigned = new List<Ref<Schedulable>>();
		this.tones = this.GenerateTones();
		this.SetBlocksToGroupDefaults(defaultGroups);
	}

	// Token: 0x0600465F RID: 18015 RVA: 0x0018DFCC File Offset: 0x0018C1CC
	public void SetBlocksToGroupDefaults(List<ScheduleGroup> defaultGroups)
	{
		this.blocks.Clear();
		int num = 0;
		for (int i = 0; i < defaultGroups.Count; i++)
		{
			ScheduleGroup scheduleGroup = defaultGroups[i];
			for (int j = 0; j < scheduleGroup.defaultSegments; j++)
			{
				this.blocks.Add(new ScheduleBlock(scheduleGroup.Name, scheduleGroup.allowedTypes, scheduleGroup.Id));
				num++;
			}
		}
		global::Debug.Assert(num == 24);
		this.Changed();
	}

	// Token: 0x06004660 RID: 18016 RVA: 0x0018E048 File Offset: 0x0018C248
	public void Tick()
	{
		ScheduleBlock block = this.GetBlock(Schedule.GetBlockIdx());
		ScheduleBlock block2 = this.GetBlock(Schedule.GetLastBlockIdx());
		if (!Schedule.AreScheduleTypesIdentical(block.allowed_types, block2.allowed_types))
		{
			ScheduleGroup scheduleGroup = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(block.allowed_types);
			ScheduleGroup scheduleGroup2 = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(block2.allowed_types);
			if (this.alarmActivated && scheduleGroup2.alarm != scheduleGroup.alarm)
			{
				ScheduleManager.Instance.PlayScheduleAlarm(this, block, scheduleGroup.alarm);
			}
			foreach (Ref<Schedulable> @ref in this.GetAssigned())
			{
				@ref.Get().OnScheduleBlocksChanged(this);
			}
		}
		foreach (Ref<Schedulable> ref2 in this.GetAssigned())
		{
			ref2.Get().OnScheduleBlocksTick(this);
		}
	}

	// Token: 0x06004661 RID: 18017 RVA: 0x0018E168 File Offset: 0x0018C368
	string IListableOption.GetProperName()
	{
		return this.name;
	}

	// Token: 0x06004662 RID: 18018 RVA: 0x0018E170 File Offset: 0x0018C370
	public int[] GenerateTones()
	{
		int minToneIndex = TuningData<ScheduleManager.Tuning>.Get().minToneIndex;
		int maxToneIndex = TuningData<ScheduleManager.Tuning>.Get().maxToneIndex;
		int firstLastToneSpacing = TuningData<ScheduleManager.Tuning>.Get().firstLastToneSpacing;
		int[] array = new int[4];
		array[0] = UnityEngine.Random.Range(minToneIndex, maxToneIndex - firstLastToneSpacing + 1);
		array[1] = UnityEngine.Random.Range(minToneIndex, maxToneIndex + 1);
		array[2] = UnityEngine.Random.Range(minToneIndex, maxToneIndex + 1);
		array[3] = UnityEngine.Random.Range(array[0] + firstLastToneSpacing, maxToneIndex + 1);
		return array;
	}

	// Token: 0x06004663 RID: 18019 RVA: 0x0018E1DC File Offset: 0x0018C3DC
	public List<Ref<Schedulable>> GetAssigned()
	{
		if (this.assigned == null)
		{
			this.assigned = new List<Ref<Schedulable>>();
		}
		return this.assigned;
	}

	// Token: 0x06004664 RID: 18020 RVA: 0x0018E1F7 File Offset: 0x0018C3F7
	public int[] GetTones()
	{
		if (this.tones == null)
		{
			this.tones = this.GenerateTones();
		}
		return this.tones;
	}

	// Token: 0x06004665 RID: 18021 RVA: 0x0018E213 File Offset: 0x0018C413
	public void SetGroup(int idx, ScheduleGroup group)
	{
		if (0 <= idx && idx < this.blocks.Count)
		{
			this.blocks[idx] = new ScheduleBlock(group.Name, group.allowedTypes, group.Id);
			this.Changed();
		}
	}

	// Token: 0x06004666 RID: 18022 RVA: 0x0018E250 File Offset: 0x0018C450
	private void Changed()
	{
		foreach (Ref<Schedulable> @ref in this.GetAssigned())
		{
			@ref.Get().OnScheduleChanged(this);
		}
		if (this.onChanged != null)
		{
			this.onChanged(this);
		}
	}

	// Token: 0x06004667 RID: 18023 RVA: 0x0018E2BC File Offset: 0x0018C4BC
	public List<ScheduleBlock> GetBlocks()
	{
		return this.blocks;
	}

	// Token: 0x06004668 RID: 18024 RVA: 0x0018E2C4 File Offset: 0x0018C4C4
	public ScheduleBlock GetBlock(int idx)
	{
		return this.blocks[idx];
	}

	// Token: 0x06004669 RID: 18025 RVA: 0x0018E2D2 File Offset: 0x0018C4D2
	public void Assign(Schedulable schedulable)
	{
		if (!this.IsAssigned(schedulable))
		{
			this.GetAssigned().Add(new Ref<Schedulable>(schedulable));
		}
		this.Changed();
	}

	// Token: 0x0600466A RID: 18026 RVA: 0x0018E2F4 File Offset: 0x0018C4F4
	public void Unassign(Schedulable schedulable)
	{
		for (int i = 0; i < this.GetAssigned().Count; i++)
		{
			if (this.GetAssigned()[i].Get() == schedulable)
			{
				this.GetAssigned().RemoveAt(i);
				break;
			}
		}
		this.Changed();
	}

	// Token: 0x0600466B RID: 18027 RVA: 0x0018E344 File Offset: 0x0018C544
	public bool IsAssigned(Schedulable schedulable)
	{
		using (List<Ref<Schedulable>>.Enumerator enumerator = this.GetAssigned().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get() == schedulable)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600466C RID: 18028 RVA: 0x0018E3A4 File Offset: 0x0018C5A4
	public static bool AreScheduleTypesIdentical(List<ScheduleBlockType> a, List<ScheduleBlockType> b)
	{
		if (a.Count != b.Count)
		{
			return false;
		}
		foreach (ScheduleBlockType scheduleBlockType in a)
		{
			bool flag = false;
			foreach (ScheduleBlockType scheduleBlockType2 in b)
			{
				if (scheduleBlockType.IdHash == scheduleBlockType2.IdHash)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04002EA0 RID: 11936
	[Serialize]
	private List<ScheduleBlock> blocks;

	// Token: 0x04002EA1 RID: 11937
	[Serialize]
	private List<Ref<Schedulable>> assigned;

	// Token: 0x04002EA2 RID: 11938
	[Serialize]
	public string name;

	// Token: 0x04002EA3 RID: 11939
	[Serialize]
	public bool alarmActivated = true;

	// Token: 0x04002EA4 RID: 11940
	[Serialize]
	private int[] tones;

	// Token: 0x04002EA5 RID: 11941
	public Action<Schedule> onChanged;
}
