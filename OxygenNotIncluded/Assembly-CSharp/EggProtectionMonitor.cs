using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200071B RID: 1819
public class EggProtectionMonitor : GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>
{
	// Token: 0x060031F7 RID: 12791 RVA: 0x001094D4 File Offset: 0x001076D4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.find_egg;
		this.root.EventHandler(GameHashes.ObjectDestroyed, delegate(EggProtectionMonitor.Instance smi, object d)
		{
			smi.Cleanup(d);
		});
		this.find_egg.BatchUpdate(new UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.BatchUpdateDelegate(EggProtectionMonitor.Instance.FindEggToGuard), UpdateRate.SIM_200ms).ParamTransition<bool>(this.hasEggToGuard, this.guard.safe, GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.IsTrue);
		this.guard.Enter(delegate(EggProtectionMonitor.Instance smi)
		{
			smi.gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim("pincher_kanim"), smi.def.animPrefix, "_heat", 0);
			smi.gameObject.AddOrGet<FactionAlignment>().SwitchAlignment(FactionManager.FactionID.Hostile);
		}).Exit(delegate(EggProtectionMonitor.Instance smi)
		{
			if (!smi.def.animPrefix.IsNullOrWhiteSpace())
			{
				smi.gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim("pincher_kanim"), smi.def.animPrefix, null, 0);
			}
			else
			{
				smi.gameObject.AddOrGet<SymbolOverrideController>().RemoveBuildOverride(Assets.GetAnim("pincher_kanim").GetData(), 0);
			}
			smi.gameObject.AddOrGet<FactionAlignment>().SwitchAlignment(FactionManager.FactionID.Pest);
		}).Update("evaulate_egg", delegate(EggProtectionMonitor.Instance smi, float dt)
		{
			smi.CanProtectEgg();
		}, UpdateRate.SIM_1000ms, true).ParamTransition<bool>(this.hasEggToGuard, this.find_egg, GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.IsFalse);
		this.guard.safe.Enter(delegate(EggProtectionMonitor.Instance smi)
		{
			smi.RefreshThreat(null);
		}).Update("safe", delegate(EggProtectionMonitor.Instance smi, float dt)
		{
			smi.RefreshThreat(null);
		}, UpdateRate.SIM_200ms, true).ToggleStatusItem(CREATURES.STATUSITEMS.PROTECTINGENTITY.NAME, CREATURES.STATUSITEMS.PROTECTINGENTITY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, null);
		this.guard.threatened.ToggleBehaviour(GameTags.Creatures.Defend, (EggProtectionMonitor.Instance smi) => smi.MainThreat != null, delegate(EggProtectionMonitor.Instance smi)
		{
			smi.GoTo(this.guard.safe);
		}).Update("Threatened", new Action<EggProtectionMonitor.Instance, float>(EggProtectionMonitor.CritterUpdateThreats), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x060031F8 RID: 12792 RVA: 0x001096C3 File Offset: 0x001078C3
	private static void CritterUpdateThreats(EggProtectionMonitor.Instance smi, float dt)
	{
		if (smi.isMasterNull)
		{
			return;
		}
		if (!smi.CheckForThreats())
		{
			smi.GoTo(smi.sm.guard.safe);
		}
	}

	// Token: 0x04001DF1 RID: 7665
	public StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.BoolParameter hasEggToGuard;

	// Token: 0x04001DF2 RID: 7666
	public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State find_egg;

	// Token: 0x04001DF3 RID: 7667
	public EggProtectionMonitor.GuardEggStates guard;

	// Token: 0x02001486 RID: 5254
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400659A RID: 26010
		public Tag[] allyTags;

		// Token: 0x0400659B RID: 26011
		public string animPrefix;
	}

	// Token: 0x02001487 RID: 5255
	public class GuardEggStates : GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State
	{
		// Token: 0x0400659C RID: 26012
		public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State safe;

		// Token: 0x0400659D RID: 26013
		public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State threatened;
	}

	// Token: 0x02001488 RID: 5256
	public new class Instance : GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.GameInstance
	{
		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x060084E8 RID: 34024 RVA: 0x003038CD File Offset: 0x00301ACD
		public GameObject MainThreat
		{
			get
			{
				return this.mainThreat;
			}
		}

		// Token: 0x060084E9 RID: 34025 RVA: 0x003038D8 File Offset: 0x00301AD8
		public Instance(IStateMachineTarget master, EggProtectionMonitor.Def def) : base(master, def)
		{
			this.alignment = master.GetComponent<FactionAlignment>();
			this.navigator = master.GetComponent<Navigator>();
			this.refreshThreatDelegate = new Action<object>(this.RefreshThreat);
		}

		// Token: 0x060084EA RID: 34026 RVA: 0x0030392C File Offset: 0x00301B2C
		public void CanProtectEgg()
		{
			bool flag = true;
			if (this.eggToProtect == null)
			{
				flag = false;
			}
			if (flag)
			{
				int num = 150;
				int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(this.eggToProtect));
				if (navigationCost == -1 || navigationCost >= num)
				{
					flag = false;
				}
			}
			if (!flag)
			{
				this.SetEggToGuard(null);
			}
		}

		// Token: 0x060084EB RID: 34027 RVA: 0x00303980 File Offset: 0x00301B80
		public static void FindEggToGuard(List<UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry> instances, float time_delta)
		{
			ListPool<KPrefabID, EggProtectionMonitor>.PooledList pooledList = ListPool<KPrefabID, EggProtectionMonitor>.Allocate();
			pooledList.Capacity = Mathf.Max(pooledList.Capacity, Components.IncubationMonitors.Count);
			foreach (object obj in Components.IncubationMonitors)
			{
				IncubationMonitor.Instance instance = (IncubationMonitor.Instance)obj;
				pooledList.Add(instance.gameObject.GetComponent<KPrefabID>());
			}
			ListPool<EggProtectionMonitor.Instance.Egg, EggProtectionMonitor>.PooledList pooledList2 = ListPool<EggProtectionMonitor.Instance.Egg, EggProtectionMonitor>.Allocate();
			EggProtectionMonitor.Instance.find_eggs_job.Reset(pooledList);
			for (int i = 0; i < pooledList.Count; i += 256)
			{
				EggProtectionMonitor.Instance.find_eggs_job.Add(new EggProtectionMonitor.Instance.FindEggsTask(i, Mathf.Min(i + 256, pooledList.Count)));
			}
			GlobalJobManager.Run(EggProtectionMonitor.Instance.find_eggs_job);
			for (int num = 0; num != EggProtectionMonitor.Instance.find_eggs_job.Count; num++)
			{
				EggProtectionMonitor.Instance.find_eggs_job.GetWorkItem(num).Finish(pooledList, pooledList2);
			}
			pooledList.Recycle();
			EggProtectionMonitor.Instance.find_eggs_job.Reset(null);
			foreach (UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry entry in new List<UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry>(instances))
			{
				GameObject eggToGuard = null;
				int num2 = 100;
				foreach (EggProtectionMonitor.Instance.Egg egg in pooledList2)
				{
					int navigationCost = entry.data.navigator.GetNavigationCost(egg.cell);
					if (navigationCost != -1 && navigationCost < num2)
					{
						eggToGuard = egg.game_object;
						num2 = navigationCost;
					}
				}
				entry.data.SetEggToGuard(eggToGuard);
			}
			pooledList2.Recycle();
		}

		// Token: 0x060084EC RID: 34028 RVA: 0x00303B64 File Offset: 0x00301D64
		public void SetEggToGuard(GameObject egg)
		{
			this.eggToProtect = egg;
			base.sm.hasEggToGuard.Set(egg != null, base.smi, false);
		}

		// Token: 0x060084ED RID: 34029 RVA: 0x00303B8C File Offset: 0x00301D8C
		public void SetMainThreat(GameObject threat)
		{
			if (threat == this.mainThreat)
			{
				return;
			}
			if (this.mainThreat != null)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
				if (threat == null)
				{
					base.Trigger(2144432245, null);
				}
			}
			if (this.mainThreat != null)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
			}
			this.mainThreat = threat;
			if (this.mainThreat != null)
			{
				this.mainThreat.Subscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Subscribe(1969584890, this.refreshThreatDelegate);
			}
		}

		// Token: 0x060084EE RID: 34030 RVA: 0x00303C74 File Offset: 0x00301E74
		public void Cleanup(object data)
		{
			if (this.mainThreat)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
			}
		}

		// Token: 0x060084EF RID: 34031 RVA: 0x00303CAF File Offset: 0x00301EAF
		public void GoToThreatened()
		{
			base.smi.GoTo(base.sm.guard.threatened);
		}

		// Token: 0x060084F0 RID: 34032 RVA: 0x00303CCC File Offset: 0x00301ECC
		public void RefreshThreat(object data)
		{
			if (!base.IsRunning() || this.eggToProtect == null)
			{
				return;
			}
			if (base.smi.CheckForThreats())
			{
				this.GoToThreatened();
				return;
			}
			if (base.smi.GetCurrentState() != base.sm.guard.safe)
			{
				base.Trigger(-21431934, null);
				base.smi.GoTo(base.sm.guard.safe);
			}
		}

		// Token: 0x060084F1 RID: 34033 RVA: 0x00303D48 File Offset: 0x00301F48
		public bool CheckForThreats()
		{
			if (this.eggToProtect == null)
			{
				return false;
			}
			GameObject x = this.FindThreat();
			this.SetMainThreat(x);
			return x != null;
		}

		// Token: 0x060084F2 RID: 34034 RVA: 0x00303D7C File Offset: 0x00301F7C
		public GameObject FindThreat()
		{
			this.threats.Clear();
			ListPool<ScenePartitionerEntry, ThreatMonitor>.PooledList pooledList = ListPool<ScenePartitionerEntry, ThreatMonitor>.Allocate();
			Extents extents = new Extents(Grid.PosToCell(this.eggToProtect), this.maxThreatDistance);
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.attackableEntitiesLayer, pooledList);
			for (int i = 0; i < pooledList.Count; i++)
			{
				FactionAlignment factionAlignment = pooledList[i].obj as FactionAlignment;
				if (!(factionAlignment.transform == null) && !(factionAlignment == this.alignment) && factionAlignment.IsAlignmentActive() && this.navigator.CanReach(factionAlignment.attackable))
				{
					bool flag = false;
					foreach (Tag tag in base.def.allyTags)
					{
						if (factionAlignment.HasTag(tag))
						{
							flag = true;
						}
					}
					if (!flag)
					{
						this.threats.Add(factionAlignment);
					}
				}
			}
			pooledList.Recycle();
			return this.PickBestTarget(this.threats);
		}

		// Token: 0x060084F3 RID: 34035 RVA: 0x00303E88 File Offset: 0x00302088
		public GameObject PickBestTarget(List<FactionAlignment> threats)
		{
			float num = 1f;
			Vector2 a = base.gameObject.transform.GetPosition();
			GameObject result = null;
			float num2 = float.PositiveInfinity;
			for (int i = threats.Count - 1; i >= 0; i--)
			{
				FactionAlignment factionAlignment = threats[i];
				float num3 = Vector2.Distance(a, factionAlignment.transform.GetPosition()) / num;
				if (num3 < num2)
				{
					num2 = num3;
					result = factionAlignment.gameObject;
				}
			}
			return result;
		}

		// Token: 0x0400659E RID: 26014
		public GameObject eggToProtect;

		// Token: 0x0400659F RID: 26015
		public FactionAlignment alignment;

		// Token: 0x040065A0 RID: 26016
		private Navigator navigator;

		// Token: 0x040065A1 RID: 26017
		private GameObject mainThreat;

		// Token: 0x040065A2 RID: 26018
		private List<FactionAlignment> threats = new List<FactionAlignment>();

		// Token: 0x040065A3 RID: 26019
		private int maxThreatDistance = 12;

		// Token: 0x040065A4 RID: 26020
		private Action<object> refreshThreatDelegate;

		// Token: 0x040065A5 RID: 26021
		private static WorkItemCollection<EggProtectionMonitor.Instance.FindEggsTask, List<KPrefabID>> find_eggs_job = new WorkItemCollection<EggProtectionMonitor.Instance.FindEggsTask, List<KPrefabID>>();

		// Token: 0x02002165 RID: 8549
		private struct Egg
		{
			// Token: 0x04009588 RID: 38280
			public GameObject game_object;

			// Token: 0x04009589 RID: 38281
			public int cell;
		}

		// Token: 0x02002166 RID: 8550
		private struct FindEggsTask : IWorkItem<List<KPrefabID>>
		{
			// Token: 0x0600AA1E RID: 43550 RVA: 0x00373F3B File Offset: 0x0037213B
			public FindEggsTask(int start, int end)
			{
				this.start = start;
				this.end = end;
				this.eggs = ListPool<int, EggProtectionMonitor>.Allocate();
			}

			// Token: 0x0600AA1F RID: 43551 RVA: 0x00373F58 File Offset: 0x00372158
			public void Run(List<KPrefabID> prefab_ids)
			{
				for (int num = this.start; num != this.end; num++)
				{
					if (EggProtectionMonitor.Instance.FindEggsTask.EGG_TAG.Contains(prefab_ids[num].PrefabTag))
					{
						this.eggs.Add(num);
					}
				}
			}

			// Token: 0x0600AA20 RID: 43552 RVA: 0x00373FA0 File Offset: 0x003721A0
			public void Finish(List<KPrefabID> prefab_ids, List<EggProtectionMonitor.Instance.Egg> eggs)
			{
				foreach (int index in this.eggs)
				{
					GameObject gameObject = prefab_ids[index].gameObject;
					eggs.Add(new EggProtectionMonitor.Instance.Egg
					{
						game_object = gameObject,
						cell = Grid.PosToCell(gameObject)
					});
				}
				this.eggs.Recycle();
			}

			// Token: 0x0400958A RID: 38282
			private static readonly List<Tag> EGG_TAG = new List<Tag>
			{
				"CrabEgg".ToTag(),
				"CrabWoodEgg".ToTag(),
				"CrabFreshWaterEgg".ToTag()
			};

			// Token: 0x0400958B RID: 38283
			private ListPool<int, EggProtectionMonitor>.PooledList eggs;

			// Token: 0x0400958C RID: 38284
			private int start;

			// Token: 0x0400958D RID: 38285
			private int end;
		}
	}
}
