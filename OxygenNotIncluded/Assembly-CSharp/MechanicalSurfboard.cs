using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000859 RID: 2137
public class MechanicalSurfboard : StateMachineComponent<MechanicalSurfboard.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06003E84 RID: 16004 RVA: 0x0015AFB4 File Offset: 0x001591B4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06003E85 RID: 16005 RVA: 0x0015AFC7 File Offset: 0x001591C7
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003E86 RID: 16006 RVA: 0x0015AFD0 File Offset: 0x001591D0
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Element element = ElementLoader.FindElementByHash(SimHashes.Water);
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect, false));
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		list.Add(new Descriptor(BUILDINGS.PREFABS.MECHANICALSURFBOARD.WATER_REQUIREMENT.Replace("{element}", element.name).Replace("{amount}", GameUtil.GetFormattedMass(this.minOperationalWaterKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), BUILDINGS.PREFABS.MECHANICALSURFBOARD.WATER_REQUIREMENT_TOOLTIP.Replace("{element}", element.name).Replace("{amount}", GameUtil.GetFormattedMass(this.minOperationalWaterKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		list.Add(new Descriptor(BUILDINGS.PREFABS.MECHANICALSURFBOARD.LEAK_REQUIREMENT.Replace("{amount}", GameUtil.GetFormattedMass(this.waterSpillRateKG, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), BUILDINGS.PREFABS.MECHANICALSURFBOARD.LEAK_REQUIREMENT_TOOLTIP.Replace("{amount}", GameUtil.GetFormattedMass(this.waterSpillRateKG, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false).IncreaseIndent());
		return list;
	}

	// Token: 0x04002885 RID: 10373
	public string specificEffect;

	// Token: 0x04002886 RID: 10374
	public string trackingEffect;

	// Token: 0x04002887 RID: 10375
	public float waterSpillRateKG;

	// Token: 0x04002888 RID: 10376
	public float minOperationalWaterKG;

	// Token: 0x04002889 RID: 10377
	public string[] interactAnims = new string[]
	{
		"anim_interacts_mechanical_surfboard_kanim",
		"anim_interacts_mechanical_surfboard2_kanim",
		"anim_interacts_mechanical_surfboard3_kanim"
	};

	// Token: 0x0200162C RID: 5676
	public class States : GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard>
	{
		// Token: 0x060089E6 RID: 35302 RVA: 0x00312404 File Offset: 0x00310604
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false).ToggleMainStatusItem(Db.Get().BuildingStatusItems.MissingRequirements, null);
			this.operational.PlayAnim("off").TagTransition(GameTags.Operational, this.inoperational, true).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.Transition.ConditionCallback(this.IsReady)).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GettingReady, null);
			this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<MechanicalSurfboard.StatesInstance, Chore>(this.CreateChore), this.operational).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null);
			this.ready.idle.PlayAnim("on", KAnim.PlayMode.Loop).WorkableStartTransition((MechanicalSurfboard.StatesInstance smi) => smi.master.GetComponent<MechanicalSurfboardWorkable>(), this.ready.working).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.Not(new StateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.Transition.ConditionCallback(this.IsReady)));
			this.ready.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).WorkableStopTransition((MechanicalSurfboard.StatesInstance smi) => smi.master.GetComponent<MechanicalSurfboardWorkable>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		// Token: 0x060089E7 RID: 35303 RVA: 0x003125D0 File Offset: 0x003107D0
		private Chore CreateChore(MechanicalSurfboard.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<MechanicalSurfboardWorkable>();
			WorkChore<MechanicalSurfboardWorkable> workChore = new WorkChore<MechanicalSurfboardWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x060089E8 RID: 35304 RVA: 0x00312630 File Offset: 0x00310830
		private bool IsReady(MechanicalSurfboard.StatesInstance smi)
		{
			PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
			return !(primaryElement == null) && primaryElement.Mass >= smi.master.minOperationalWaterKG;
		}

		// Token: 0x04006AD8 RID: 27352
		private GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State inoperational;

		// Token: 0x04006AD9 RID: 27353
		private GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State operational;

		// Token: 0x04006ADA RID: 27354
		private MechanicalSurfboard.States.ReadyStates ready;

		// Token: 0x020021A0 RID: 8608
		public class ReadyStates : GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State
		{
			// Token: 0x040096B0 RID: 38576
			public GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State idle;

			// Token: 0x040096B1 RID: 38577
			public GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State working;

			// Token: 0x040096B2 RID: 38578
			public GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State post;
		}
	}

	// Token: 0x0200162D RID: 5677
	public class StatesInstance : GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.GameInstance
	{
		// Token: 0x060089EA RID: 35306 RVA: 0x00312677 File Offset: 0x00310877
		public StatesInstance(MechanicalSurfboard smi) : base(smi)
		{
		}
	}
}
