using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007FA RID: 2042
[SerializationConfig(MemberSerialization.OptIn)]
public class HotTub : StateMachineComponent<HotTub.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x06003A50 RID: 14928 RVA: 0x0014472C File Offset: 0x0014292C
	public float PercentFull
	{
		get
		{
			return 100f * this.waterStorage.GetMassAvailable(SimHashes.Water) / this.hotTubCapacity;
		}
	}

	// Token: 0x06003A51 RID: 14929 RVA: 0x0014474C File Offset: 0x0014294C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new HotTubWorkable[this.choreOffsets.Length];
		this.chores = new Chore[this.choreOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.choreOffsets[i]), Grid.SceneLayer.Move);
			GameObject go = ChoreHelpers.CreateLocator("HotTubWorkable", pos);
			KSelectable kselectable = go.AddOrGet<KSelectable>();
			kselectable.SetName(this.GetProperName());
			kselectable.IsSelectable = false;
			HotTubWorkable hotTubWorkable = go.AddOrGet<HotTubWorkable>();
			int player_index = i;
			HotTubWorkable hotTubWorkable2 = hotTubWorkable;
			hotTubWorkable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(hotTubWorkable2.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(delegate(Workable workable, Workable.WorkableEvent ev)
			{
				this.OnWorkableEvent(player_index, ev);
			}));
			this.workables[i] = hotTubWorkable;
			this.workables[i].hotTub = this;
		}
		this.waterMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_water_target", "meter_water", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_water_target"
		});
		base.smi.UpdateWaterMeter();
		this.tempMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_temperature_target", "meter_temp", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_temperature_target"
		});
		base.smi.TestWaterTemperature();
		base.smi.StartSM();
	}

	// Token: 0x06003A52 RID: 14930 RVA: 0x001448E4 File Offset: 0x00142AE4
	protected override void OnCleanUp()
	{
		this.UpdateChores(false);
		for (int i = 0; i < this.workables.Length; i++)
		{
			if (this.workables[i])
			{
				Util.KDestroyGameObject(this.workables[i]);
				this.workables[i] = null;
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x06003A53 RID: 14931 RVA: 0x00144938 File Offset: 0x00142B38
	private Chore CreateChore(int i)
	{
		Workable workable = this.workables[i];
		ChoreType relax = Db.Get().ChoreTypes.Relax;
		IStateMachineTarget target = workable;
		ChoreProvider chore_provider = null;
		bool run_until_complete = true;
		Action<Chore> on_complete = null;
		Action<Chore> on_begin = null;
		ScheduleBlockType recreation = Db.Get().ScheduleBlockTypes.Recreation;
		WorkChore<HotTubWorkable> workChore = new WorkChore<HotTubWorkable>(relax, target, chore_provider, run_until_complete, on_complete, on_begin, new Action<Chore>(this.OnSocialChoreEnd), false, recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
		return workChore;
	}

	// Token: 0x06003A54 RID: 14932 RVA: 0x001449A0 File Offset: 0x00142BA0
	private void OnSocialChoreEnd(Chore chore)
	{
		if (base.gameObject.HasTag(GameTags.Operational))
		{
			this.UpdateChores(true);
		}
	}

	// Token: 0x06003A55 RID: 14933 RVA: 0x001449BC File Offset: 0x00142BBC
	public void UpdateChores(bool update = true)
	{
		for (int i = 0; i < this.choreOffsets.Length; i++)
		{
			Chore chore = this.chores[i];
			if (update)
			{
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = this.CreateChore(i);
				}
			}
			else if (chore != null)
			{
				chore.Cancel("locator invalidated");
				this.chores[i] = null;
			}
		}
	}

	// Token: 0x06003A56 RID: 14934 RVA: 0x00144A1C File Offset: 0x00142C1C
	public void OnWorkableEvent(int player, Workable.WorkableEvent ev)
	{
		if (ev == Workable.WorkableEvent.WorkStarted)
		{
			this.occupants.Add(player);
		}
		else
		{
			this.occupants.Remove(player);
		}
		base.smi.sm.userCount.Set(this.occupants.Count, base.smi, false);
	}

	// Token: 0x06003A57 RID: 14935 RVA: 0x00144A74 File Offset: 0x00142C74
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Element element = ElementLoader.FindElementByHash(SimHashes.Water);
		list.Add(new Descriptor(BUILDINGS.PREFABS.HOTTUB.WATER_REQUIREMENT.Replace("{element}", element.name).Replace("{amount}", GameUtil.GetFormattedMass(this.hotTubCapacity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), BUILDINGS.PREFABS.HOTTUB.WATER_REQUIREMENT_TOOLTIP.Replace("{element}", element.name).Replace("{amount}", GameUtil.GetFormattedMass(this.hotTubCapacity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		list.Add(new Descriptor(BUILDINGS.PREFABS.HOTTUB.TEMPERATURE_REQUIREMENT.Replace("{element}", element.name).Replace("{temperature}", GameUtil.GetFormattedTemperature(this.minimumWaterTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), BUILDINGS.PREFABS.HOTTUB.TEMPERATURE_REQUIREMENT_TOOLTIP.Replace("{element}", element.name).Replace("{temperature}", GameUtil.GetFormattedTemperature(this.minimumWaterTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Requirement, false));
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect, false));
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		return list;
	}

	// Token: 0x040026CC RID: 9932
	public string specificEffect;

	// Token: 0x040026CD RID: 9933
	public string trackingEffect;

	// Token: 0x040026CE RID: 9934
	public int basePriority;

	// Token: 0x040026CF RID: 9935
	public CellOffset[] choreOffsets = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(1, 0),
		new CellOffset(0, 0),
		new CellOffset(2, 0)
	};

	// Token: 0x040026D0 RID: 9936
	private HotTubWorkable[] workables;

	// Token: 0x040026D1 RID: 9937
	private Chore[] chores;

	// Token: 0x040026D2 RID: 9938
	public HashSet<int> occupants = new HashSet<int>();

	// Token: 0x040026D3 RID: 9939
	public float waterCoolingRate;

	// Token: 0x040026D4 RID: 9940
	public float hotTubCapacity = 100f;

	// Token: 0x040026D5 RID: 9941
	public float minimumWaterTemperature;

	// Token: 0x040026D6 RID: 9942
	public float bleachStoneConsumption;

	// Token: 0x040026D7 RID: 9943
	public float maxOperatingTemperature;

	// Token: 0x040026D8 RID: 9944
	[MyCmpGet]
	public Storage waterStorage;

	// Token: 0x040026D9 RID: 9945
	private MeterController waterMeter;

	// Token: 0x040026DA RID: 9946
	private MeterController tempMeter;

	// Token: 0x020015DB RID: 5595
	public class States : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub>
	{
		// Token: 0x060088A8 RID: 34984 RVA: 0x0030EC80 File Offset: 0x0030CE80
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.ready;
			this.root.Update(delegate(HotTub.StatesInstance smi, float dt)
			{
				smi.SapHeatFromWater(dt);
				smi.TestWaterTemperature();
			}, UpdateRate.SIM_4000ms, false).EventHandler(GameHashes.OnStorageChange, delegate(HotTub.StatesInstance smi)
			{
				smi.UpdateWaterMeter();
				smi.TestWaterTemperature();
			});
			this.unoperational.TagTransition(GameTags.Operational, this.off, false).PlayAnim("off");
			this.off.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.off.filling);
			this.off.filling.DefaultState(this.off.filling.normal).Transition(this.ready, (HotTub.StatesInstance smi) => smi.master.waterStorage.GetMassAvailable(SimHashes.Water) >= smi.master.hotTubCapacity, UpdateRate.SIM_200ms).PlayAnim("off").Enter(delegate(HotTub.StatesInstance smi)
			{
				smi.GetComponent<ConduitConsumer>().SetOnState(true);
			}).Exit(delegate(HotTub.StatesInstance smi)
			{
				smi.GetComponent<ConduitConsumer>().SetOnState(false);
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.HotTubFilling, (HotTub.StatesInstance smi) => smi.master);
			this.off.filling.normal.ParamTransition<bool>(this.waterTooCold, this.off.filling.too_cold, GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.IsTrue);
			this.off.filling.too_cold.ParamTransition<bool>(this.waterTooCold, this.off.filling.normal, GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.HotTubWaterTooCold, (HotTub.StatesInstance smi) => smi.master);
			this.off.draining.Transition(this.off.filling, (HotTub.StatesInstance smi) => smi.master.waterStorage.GetMassAvailable(SimHashes.Water) <= 0f, UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.HotTubWaterTooCold, (HotTub.StatesInstance smi) => smi.master).PlayAnim("off").Enter(delegate(HotTub.StatesInstance smi)
			{
				smi.GetComponent<ConduitDispenser>().SetOnState(true);
			}).Exit(delegate(HotTub.StatesInstance smi)
			{
				smi.GetComponent<ConduitDispenser>().SetOnState(false);
			});
			this.off.too_hot.Transition(this.ready, (HotTub.StatesInstance smi) => !smi.IsTubTooHot(), UpdateRate.SIM_200ms).PlayAnim("overheated").ToggleMainStatusItem(Db.Get().BuildingStatusItems.HotTubTooHot, (HotTub.StatesInstance smi) => smi.master);
			this.off.awaiting_delivery.EventTransition(GameHashes.OnStorageChange, this.ready, (HotTub.StatesInstance smi) => smi.HasBleachStone());
			this.ready.DefaultState(this.ready.idle).Enter("CreateChore", delegate(HotTub.StatesInstance smi)
			{
				smi.master.UpdateChores(true);
			}).Exit("CancelChore", delegate(HotTub.StatesInstance smi)
			{
				smi.master.UpdateChores(false);
			}).TagTransition(GameTags.Operational, this.unoperational, true).ParamTransition<bool>(this.waterTooCold, this.off.draining, GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.IsTrue).EventTransition(GameHashes.OnStorageChange, this.off.awaiting_delivery, (HotTub.StatesInstance smi) => !smi.HasBleachStone()).Transition(this.off.filling, (HotTub.StatesInstance smi) => smi.master.waterStorage.IsEmpty(), UpdateRate.SIM_200ms).Transition(this.off.too_hot, (HotTub.StatesInstance smi) => smi.IsTubTooHot(), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null);
			this.ready.idle.PlayAnim("on").ParamTransition<int>(this.userCount, this.ready.on.pre, (HotTub.StatesInstance smi, int p) => p > 0);
			this.ready.on.Enter(delegate(HotTub.StatesInstance smi)
			{
				smi.SetActive(true);
			}).Update(delegate(HotTub.StatesInstance smi, float dt)
			{
				smi.ConsumeBleachstone(dt);
			}, UpdateRate.SIM_4000ms, false).Exit(delegate(HotTub.StatesInstance smi)
			{
				smi.SetActive(false);
			});
			this.ready.on.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.ready.on.relaxing);
			this.ready.on.relaxing.PlayAnim("working_loop", KAnim.PlayMode.Loop).ParamTransition<int>(this.userCount, this.ready.on.post, (HotTub.StatesInstance smi, int p) => p == 0).ParamTransition<int>(this.userCount, this.ready.on.relaxing_together, (HotTub.StatesInstance smi, int p) => p > 1);
			this.ready.on.relaxing_together.PlayAnim("working_loop", KAnim.PlayMode.Loop).ParamTransition<int>(this.userCount, this.ready.on.post, (HotTub.StatesInstance smi, int p) => p == 0).ParamTransition<int>(this.userCount, this.ready.on.relaxing, (HotTub.StatesInstance smi, int p) => p == 1);
			this.ready.on.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready.idle);
		}

		// Token: 0x060088A9 RID: 34985 RVA: 0x0030F390 File Offset: 0x0030D590
		private string GetRelaxingAnim(HotTub.StatesInstance smi)
		{
			bool flag = smi.master.occupants.Contains(0);
			bool flag2 = smi.master.occupants.Contains(1);
			if (flag && !flag2)
			{
				return "working_loop_one_p";
			}
			if (flag2 && !flag)
			{
				return "working_loop_two_p";
			}
			return "working_loop_coop_p";
		}

		// Token: 0x040069BC RID: 27068
		public StateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.IntParameter userCount;

		// Token: 0x040069BD RID: 27069
		public StateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.BoolParameter waterTooCold;

		// Token: 0x040069BE RID: 27070
		public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State unoperational;

		// Token: 0x040069BF RID: 27071
		public HotTub.States.OffStates off;

		// Token: 0x040069C0 RID: 27072
		public HotTub.States.ReadyStates ready;

		// Token: 0x02002193 RID: 8595
		public class OffStates : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State
		{
			// Token: 0x04009661 RID: 38497
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State draining;

			// Token: 0x04009662 RID: 38498
			public HotTub.States.FillingStates filling;

			// Token: 0x04009663 RID: 38499
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State too_hot;

			// Token: 0x04009664 RID: 38500
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State awaiting_delivery;
		}

		// Token: 0x02002194 RID: 8596
		public class OnStates : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State
		{
			// Token: 0x04009665 RID: 38501
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State pre;

			// Token: 0x04009666 RID: 38502
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State relaxing;

			// Token: 0x04009667 RID: 38503
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State relaxing_together;

			// Token: 0x04009668 RID: 38504
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State post;
		}

		// Token: 0x02002195 RID: 8597
		public class ReadyStates : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State
		{
			// Token: 0x04009669 RID: 38505
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State idle;

			// Token: 0x0400966A RID: 38506
			public HotTub.States.OnStates on;
		}

		// Token: 0x02002196 RID: 8598
		public class FillingStates : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State
		{
			// Token: 0x0400966B RID: 38507
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State normal;

			// Token: 0x0400966C RID: 38508
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State too_cold;
		}
	}

	// Token: 0x020015DC RID: 5596
	public class StatesInstance : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.GameInstance
	{
		// Token: 0x060088AB RID: 34987 RVA: 0x0030F3E6 File Offset: 0x0030D5E6
		public StatesInstance(HotTub smi) : base(smi)
		{
			this.operational = base.master.GetComponent<Operational>();
		}

		// Token: 0x060088AC RID: 34988 RVA: 0x0030F400 File Offset: 0x0030D600
		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		// Token: 0x060088AD RID: 34989 RVA: 0x0030F41C File Offset: 0x0030D61C
		public void UpdateWaterMeter()
		{
			base.smi.master.waterMeter.SetPositionPercent(Mathf.Clamp(base.smi.master.waterStorage.GetMassAvailable(SimHashes.Water) / base.smi.master.hotTubCapacity, 0f, 1f));
		}

		// Token: 0x060088AE RID: 34990 RVA: 0x0030F478 File Offset: 0x0030D678
		public void UpdateTemperatureMeter(float waterTemp)
		{
			Element element = ElementLoader.GetElement(SimHashes.Water.CreateTag());
			base.smi.master.tempMeter.SetPositionPercent(Mathf.Clamp((waterTemp - base.smi.master.minimumWaterTemperature) / (element.highTemp - base.smi.master.minimumWaterTemperature), 0f, 1f));
		}

		// Token: 0x060088AF RID: 34991 RVA: 0x0030F4E4 File Offset: 0x0030D6E4
		public void TestWaterTemperature()
		{
			GameObject gameObject = base.smi.master.waterStorage.FindFirst(new Tag(1836671383));
			float num = 0f;
			if (!gameObject)
			{
				this.UpdateTemperatureMeter(num);
				base.smi.sm.waterTooCold.Set(false, base.smi, false);
				return;
			}
			num = gameObject.GetComponent<PrimaryElement>().Temperature;
			this.UpdateTemperatureMeter(num);
			if (num < base.smi.master.minimumWaterTemperature)
			{
				base.smi.sm.waterTooCold.Set(true, base.smi, false);
				return;
			}
			base.smi.sm.waterTooCold.Set(false, base.smi, false);
		}

		// Token: 0x060088B0 RID: 34992 RVA: 0x0030F5A8 File Offset: 0x0030D7A8
		public bool IsTubTooHot()
		{
			return base.smi.master.GetComponent<PrimaryElement>().Temperature > base.smi.master.maxOperatingTemperature;
		}

		// Token: 0x060088B1 RID: 34993 RVA: 0x0030F5D4 File Offset: 0x0030D7D4
		public bool HasBleachStone()
		{
			GameObject gameObject = base.smi.master.waterStorage.FindFirst(new Tag(-839728230));
			return gameObject != null && gameObject.GetComponent<PrimaryElement>().Mass > 0f;
		}

		// Token: 0x060088B2 RID: 34994 RVA: 0x0030F620 File Offset: 0x0030D820
		public void SapHeatFromWater(float dt)
		{
			float num = base.smi.master.waterCoolingRate * dt / (float)base.smi.master.waterStorage.items.Count;
			foreach (GameObject gameObject in base.smi.master.waterStorage.items)
			{
				GameUtil.DeltaThermalEnergy(gameObject.GetComponent<PrimaryElement>(), -num, base.smi.master.minimumWaterTemperature);
				GameUtil.DeltaThermalEnergy(base.GetComponent<PrimaryElement>(), num, base.GetComponent<PrimaryElement>().Element.highTemp);
			}
		}

		// Token: 0x060088B3 RID: 34995 RVA: 0x0030F6E4 File Offset: 0x0030D8E4
		public void ConsumeBleachstone(float dt)
		{
			base.smi.master.waterStorage.ConsumeIgnoringDisease(new Tag(-839728230), base.smi.master.bleachStoneConsumption * dt);
		}

		// Token: 0x040069C1 RID: 27073
		private Operational operational;
	}
}
