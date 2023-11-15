using System;
using System.Collections.Generic;

// Token: 0x020003D8 RID: 984
public class ChoreTable
{
	// Token: 0x060014AC RID: 5292 RVA: 0x0006D74E File Offset: 0x0006B94E
	public ChoreTable(ChoreTable.Entry[] entries)
	{
		this.entries = entries;
	}

	// Token: 0x060014AD RID: 5293 RVA: 0x0006D760 File Offset: 0x0006B960
	public ref ChoreTable.Entry GetEntry<T>()
	{
		ref ChoreTable.Entry result = ref ChoreTable.InvalidEntry;
		for (int i = 0; i < this.entries.Length; i++)
		{
			if (this.entries[i].stateMachineDef is T)
			{
				result = ref this.entries[i];
				break;
			}
		}
		return ref result;
	}

	// Token: 0x060014AE RID: 5294 RVA: 0x0006D7B0 File Offset: 0x0006B9B0
	public int GetChorePriority<StateMachineType>(ChoreConsumer chore_consumer)
	{
		for (int i = 0; i < this.entries.Length; i++)
		{
			ChoreTable.Entry entry = this.entries[i];
			if (entry.stateMachineDef.GetStateMachineType() == typeof(StateMachineType))
			{
				return entry.choreType.priority;
			}
		}
		Debug.LogError(chore_consumer.name + "'s chore table does not have an entry for: " + typeof(StateMachineType).Name);
		return -1;
	}

	// Token: 0x04000B2D RID: 2861
	private ChoreTable.Entry[] entries;

	// Token: 0x04000B2E RID: 2862
	public static ChoreTable.Entry InvalidEntry;

	// Token: 0x02001047 RID: 4167
	public class Builder
	{
		// Token: 0x0600753B RID: 30011 RVA: 0x002CC7B7 File Offset: 0x002CA9B7
		public ChoreTable.Builder PushInterruptGroup()
		{
			this.interruptGroupId++;
			return this;
		}

		// Token: 0x0600753C RID: 30012 RVA: 0x002CC7C8 File Offset: 0x002CA9C8
		public ChoreTable.Builder PopInterruptGroup()
		{
			DebugUtil.Assert(this.interruptGroupId > 0);
			this.interruptGroupId--;
			return this;
		}

		// Token: 0x0600753D RID: 30013 RVA: 0x002CC7E8 File Offset: 0x002CA9E8
		public ChoreTable.Builder Add(StateMachine.BaseDef def, bool condition = true, int forcePriority = -1)
		{
			if (condition)
			{
				ChoreTable.Builder.Info item = new ChoreTable.Builder.Info
				{
					interruptGroupId = this.interruptGroupId,
					forcePriority = forcePriority,
					def = def
				};
				this.infos.Add(item);
			}
			return this;
		}

		// Token: 0x0600753E RID: 30014 RVA: 0x002CC82C File Offset: 0x002CAA2C
		public ChoreTable CreateTable()
		{
			DebugUtil.Assert(this.interruptGroupId == 0);
			ChoreTable.Entry[] array = new ChoreTable.Entry[this.infos.Count];
			Stack<int> stack = new Stack<int>();
			int num = 10000;
			for (int i = 0; i < this.infos.Count; i++)
			{
				int num2 = (this.infos[i].forcePriority != -1) ? this.infos[i].forcePriority : (num - 100);
				num = num2;
				int num3 = 10000 - i * 100;
				int num4 = this.infos[i].interruptGroupId;
				if (num4 != 0)
				{
					if (stack.Count != num4)
					{
						stack.Push(num3);
					}
					else
					{
						num3 = stack.Peek();
					}
				}
				else if (stack.Count > 0)
				{
					stack.Pop();
				}
				array[i] = new ChoreTable.Entry(this.infos[i].def, num2, num3);
			}
			return new ChoreTable(array);
		}

		// Token: 0x040058AD RID: 22701
		private int interruptGroupId;

		// Token: 0x040058AE RID: 22702
		private List<ChoreTable.Builder.Info> infos = new List<ChoreTable.Builder.Info>();

		// Token: 0x040058AF RID: 22703
		private const int INVALID_PRIORITY = -1;

		// Token: 0x02001FFE RID: 8190
		private struct Info
		{
			// Token: 0x0400900D RID: 36877
			public int interruptGroupId;

			// Token: 0x0400900E RID: 36878
			public int forcePriority;

			// Token: 0x0400900F RID: 36879
			public StateMachine.BaseDef def;
		}
	}

	// Token: 0x02001048 RID: 4168
	public class ChoreTableChore<StateMachineType, StateMachineInstanceType> : Chore<StateMachineInstanceType> where StateMachineInstanceType : StateMachine.Instance
	{
		// Token: 0x06007540 RID: 30016 RVA: 0x002CC93C File Offset: 0x002CAB3C
		public ChoreTableChore(StateMachine.BaseDef state_machine_def, ChoreType chore_type, KPrefabID prefab_id) : base(chore_type, prefab_id, prefab_id.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
		{
			this.showAvailabilityInHoverText = false;
			base.smi = (state_machine_def.CreateSMI(this) as StateMachineInstanceType);
		}
	}

	// Token: 0x02001049 RID: 4169
	public struct Entry
	{
		// Token: 0x06007541 RID: 30017 RVA: 0x002CC984 File Offset: 0x002CAB84
		public Entry(StateMachine.BaseDef state_machine_def, int priority, int interrupt_priority)
		{
			Type stateMachineInstanceType = Singleton<StateMachineManager>.Instance.CreateStateMachine(state_machine_def.GetStateMachineType()).GetStateMachineInstanceType();
			Type[] typeArguments = new Type[]
			{
				state_machine_def.GetStateMachineType(),
				stateMachineInstanceType
			};
			this.choreClassType = typeof(ChoreTable.ChoreTableChore<, >).MakeGenericType(typeArguments);
			this.choreType = new ChoreType(state_machine_def.ToString(), null, new string[0], "", "", "", "", new Tag[0], priority, priority);
			this.choreType.interruptPriority = interrupt_priority;
			this.stateMachineDef = state_machine_def;
		}

		// Token: 0x040058B0 RID: 22704
		public Type choreClassType;

		// Token: 0x040058B1 RID: 22705
		public ChoreType choreType;

		// Token: 0x040058B2 RID: 22706
		public StateMachine.BaseDef stateMachineDef;
	}

	// Token: 0x0200104A RID: 4170
	public class Instance
	{
		// Token: 0x06007542 RID: 30018 RVA: 0x002CCA18 File Offset: 0x002CAC18
		public static void ResetParameters()
		{
			for (int i = 0; i < ChoreTable.Instance.parameters.Length; i++)
			{
				ChoreTable.Instance.parameters[i] = null;
			}
		}

		// Token: 0x06007543 RID: 30019 RVA: 0x002CCA40 File Offset: 0x002CAC40
		public Instance(ChoreTable chore_table, KPrefabID prefab_id)
		{
			this.prefabId = prefab_id;
			this.entries = ListPool<ChoreTable.Instance.Entry, ChoreTable.Instance>.Allocate();
			for (int i = 0; i < chore_table.entries.Length; i++)
			{
				this.entries.Add(new ChoreTable.Instance.Entry(chore_table.entries[i], prefab_id));
			}
		}

		// Token: 0x06007544 RID: 30020 RVA: 0x002CCA98 File Offset: 0x002CAC98
		~Instance()
		{
			this.OnCleanUp(this.prefabId);
		}

		// Token: 0x06007545 RID: 30021 RVA: 0x002CCACC File Offset: 0x002CACCC
		public void OnCleanUp(KPrefabID prefab_id)
		{
			if (this.entries == null)
			{
				return;
			}
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.entries[i].OnCleanUp(prefab_id);
			}
			this.entries.Recycle();
			this.entries = null;
		}

		// Token: 0x040058B3 RID: 22707
		private static object[] parameters = new object[3];

		// Token: 0x040058B4 RID: 22708
		private KPrefabID prefabId;

		// Token: 0x040058B5 RID: 22709
		private ListPool<ChoreTable.Instance.Entry, ChoreTable.Instance>.PooledList entries;

		// Token: 0x02001FFF RID: 8191
		private struct Entry
		{
			// Token: 0x0600A45D RID: 42077 RVA: 0x0036977C File Offset: 0x0036797C
			public Entry(ChoreTable.Entry chore_table_entry, KPrefabID prefab_id)
			{
				ChoreTable.Instance.parameters[0] = chore_table_entry.stateMachineDef;
				ChoreTable.Instance.parameters[1] = chore_table_entry.choreType;
				ChoreTable.Instance.parameters[2] = prefab_id;
				this.chore = (Chore)Activator.CreateInstance(chore_table_entry.choreClassType, ChoreTable.Instance.parameters);
				ChoreTable.Instance.parameters[0] = null;
				ChoreTable.Instance.parameters[1] = null;
				ChoreTable.Instance.parameters[2] = null;
			}

			// Token: 0x0600A45E RID: 42078 RVA: 0x003697DE File Offset: 0x003679DE
			public void OnCleanUp(KPrefabID prefab_id)
			{
				if (this.chore != null)
				{
					this.chore.Cancel("ChoreTable.Instance.OnCleanUp");
					this.chore = null;
				}
			}

			// Token: 0x04009010 RID: 36880
			public Chore chore;
		}
	}
}
