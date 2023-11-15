using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200059E RID: 1438
public class BeachChair : StateMachineComponent<BeachChair.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06002310 RID: 8976 RVA: 0x000C0151 File Offset: 0x000BE351
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06002311 RID: 8977 RVA: 0x000C0164 File Offset: 0x000BE364
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06002312 RID: 8978 RVA: 0x000C016C File Offset: 0x000BE36C
	public static void AddModifierDescriptions(List<Descriptor> descs, string effect_id, bool high_lux)
	{
		Klei.AI.Modifier modifier = Db.Get().effects.Get(effect_id);
		LocString locString = high_lux ? BUILDINGS.PREFABS.BEACHCHAIR.LIGHTEFFECT_HIGH : BUILDINGS.PREFABS.BEACHCHAIR.LIGHTEFFECT_LOW;
		LocString locString2 = high_lux ? BUILDINGS.PREFABS.BEACHCHAIR.LIGHTEFFECT_HIGH_TOOLTIP : BUILDINGS.PREFABS.BEACHCHAIR.LIGHTEFFECT_LOW_TOOLTIP;
		foreach (AttributeModifier attributeModifier in modifier.SelfModifiers)
		{
			Descriptor item = new Descriptor(locString.Replace("{attrib}", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME")).Replace("{amount}", attributeModifier.GetFormattedString()).Replace("{lux}", GameUtil.GetFormattedLux(10000)), locString2.Replace("{attrib}", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME")).Replace("{amount}", attributeModifier.GetFormattedString()).Replace("{lux}", GameUtil.GetFormattedLux(10000)), Descriptor.DescriptorType.Effect, false);
			item.IncreaseIndent();
			descs.Add(item);
		}
	}

	// Token: 0x06002313 RID: 8979 RVA: 0x000C02AC File Offset: 0x000BE4AC
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect, false));
		BeachChair.AddModifierDescriptions(list, this.specificEffectLit, true);
		BeachChair.AddModifierDescriptions(list, this.specificEffectUnlit, false);
		return list;
	}

	// Token: 0x06002314 RID: 8980 RVA: 0x000C02F9 File Offset: 0x000BE4F9
	public void SetLit(bool v)
	{
		base.smi.sm.lit.Set(v, base.smi, false);
	}

	// Token: 0x06002315 RID: 8981 RVA: 0x000C0319 File Offset: 0x000BE519
	public void SetWorker(Worker worker)
	{
		base.smi.sm.worker.Set(worker, base.smi);
	}

	// Token: 0x04001406 RID: 5126
	public string specificEffectUnlit;

	// Token: 0x04001407 RID: 5127
	public string specificEffectLit;

	// Token: 0x04001408 RID: 5128
	public string trackingEffect;

	// Token: 0x04001409 RID: 5129
	public const float LIT_RATIO_FOR_POSITIVE_EFFECT = 0.75f;

	// Token: 0x0200122B RID: 4651
	public class States : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair>
	{
		// Token: 0x06007C0B RID: 31755 RVA: 0x002DF6D4 File Offset: 0x002DD8D4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false).ToggleMainStatusItem(Db.Get().BuildingStatusItems.MissingRequirements, null);
			this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<BeachChair.StatesInstance, Chore>(this.CreateChore), this.inoperational).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null);
			this.ready.idle.PlayAnim("on", KAnim.PlayMode.Loop).WorkableStartTransition((BeachChair.StatesInstance smi) => smi.master.GetComponent<BeachChairWorkable>(), this.ready.working_pre);
			this.ready.working_pre.PlayAnim("working_pre").QueueAnim("working_loop", true, null).Target(this.worker).PlayAnim("working_pre").EventHandler(GameHashes.AnimQueueComplete, delegate(BeachChair.StatesInstance smi)
			{
				if (this.lit.Get(smi))
				{
					smi.GoTo(this.ready.working_lit);
					return;
				}
				smi.GoTo(this.ready.working_unlit);
			});
			this.ready.working_unlit.DefaultState(this.ready.working_unlit.working).Enter(delegate(BeachChair.StatesInstance smi)
			{
				BeachChairWorkable component = smi.master.GetComponent<BeachChairWorkable>();
				component.workingPstComplete = (component.workingPstFailed = this.UNLIT_PST_ANIMS);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.TanningLightInsufficient, null).WorkableStopTransition((BeachChair.StatesInstance smi) => smi.master.GetComponent<BeachChairWorkable>(), this.ready.post).Target(this.worker).PlayAnim("working_unlit_pre");
			this.ready.working_unlit.working.ParamTransition<bool>(this.lit, this.ready.working_unlit.post, GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.IsTrue).Target(this.worker).QueueAnim("working_unlit_loop", true, null);
			this.ready.working_unlit.post.Target(this.worker).PlayAnim("working_unlit_pst").EventHandler(GameHashes.AnimQueueComplete, delegate(BeachChair.StatesInstance smi)
			{
				if (this.lit.Get(smi))
				{
					smi.GoTo(this.ready.working_lit);
					return;
				}
				smi.GoTo(this.ready.working_unlit.working);
			});
			this.ready.working_lit.DefaultState(this.ready.working_lit.working).Enter(delegate(BeachChair.StatesInstance smi)
			{
				BeachChairWorkable component = smi.master.GetComponent<BeachChairWorkable>();
				component.workingPstComplete = (component.workingPstFailed = this.LIT_PST_ANIMS);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.TanningLightSufficient, null).WorkableStopTransition((BeachChair.StatesInstance smi) => smi.master.GetComponent<BeachChairWorkable>(), this.ready.post).Target(this.worker).PlayAnim("working_lit_pre");
			this.ready.working_lit.working.ParamTransition<bool>(this.lit, this.ready.working_lit.post, GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.IsFalse).Target(this.worker).QueueAnim("working_lit_loop", true, null).ScheduleGoTo((BeachChair.StatesInstance smi) => UnityEngine.Random.Range(5f, 15f), this.ready.working_lit.silly);
			this.ready.working_lit.silly.ParamTransition<bool>(this.lit, this.ready.working_lit.post, GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.IsFalse).Target(this.worker).PlayAnim((BeachChair.StatesInstance smi) => this.SILLY_ANIMS[UnityEngine.Random.Range(0, this.SILLY_ANIMS.Length)], KAnim.PlayMode.Once).OnAnimQueueComplete(this.ready.working_lit.working);
			this.ready.working_lit.post.Target(this.worker).PlayAnim("working_lit_pst").EventHandler(GameHashes.AnimQueueComplete, delegate(BeachChair.StatesInstance smi)
			{
				if (!this.lit.Get(smi))
				{
					smi.GoTo(this.ready.working_unlit);
					return;
				}
				smi.GoTo(this.ready.working_lit.working);
			});
			this.ready.post.PlayAnim("working_pst").Exit(delegate(BeachChair.StatesInstance smi)
			{
				this.worker.Set(null, smi);
			}).OnAnimQueueComplete(this.ready);
		}

		// Token: 0x06007C0C RID: 31756 RVA: 0x002DFAF0 File Offset: 0x002DDCF0
		private Chore CreateChore(BeachChair.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<BeachChairWorkable>();
			WorkChore<BeachChairWorkable> workChore = new WorkChore<BeachChairWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x04005EAA RID: 24234
		public StateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.BoolParameter lit;

		// Token: 0x04005EAB RID: 24235
		public StateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.TargetParameter worker;

		// Token: 0x04005EAC RID: 24236
		private GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State inoperational;

		// Token: 0x04005EAD RID: 24237
		private BeachChair.States.ReadyStates ready;

		// Token: 0x04005EAE RID: 24238
		private HashedString[] UNLIT_PST_ANIMS = new HashedString[]
		{
			"working_unlit_pst",
			"working_pst"
		};

		// Token: 0x04005EAF RID: 24239
		private HashedString[] LIT_PST_ANIMS = new HashedString[]
		{
			"working_lit_pst",
			"working_pst"
		};

		// Token: 0x04005EB0 RID: 24240
		private string[] SILLY_ANIMS = new string[]
		{
			"working_lit_loop1",
			"working_lit_loop2",
			"working_lit_loop3"
		};

		// Token: 0x020020AF RID: 8367
		public class LitWorkingStates : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State
		{
			// Token: 0x040091F8 RID: 37368
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State working;

			// Token: 0x040091F9 RID: 37369
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State silly;

			// Token: 0x040091FA RID: 37370
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State post;
		}

		// Token: 0x020020B0 RID: 8368
		public class WorkingStates : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State
		{
			// Token: 0x040091FB RID: 37371
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State working;

			// Token: 0x040091FC RID: 37372
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State post;
		}

		// Token: 0x020020B1 RID: 8369
		public class ReadyStates : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State
		{
			// Token: 0x040091FD RID: 37373
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State idle;

			// Token: 0x040091FE RID: 37374
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State working_pre;

			// Token: 0x040091FF RID: 37375
			public BeachChair.States.WorkingStates working_unlit;

			// Token: 0x04009200 RID: 37376
			public BeachChair.States.LitWorkingStates working_lit;

			// Token: 0x04009201 RID: 37377
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State post;
		}
	}

	// Token: 0x0200122C RID: 4652
	public class StatesInstance : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.GameInstance
	{
		// Token: 0x06007C15 RID: 31765 RVA: 0x002DFD06 File Offset: 0x002DDF06
		public StatesInstance(BeachChair smi) : base(smi)
		{
		}
	}
}
