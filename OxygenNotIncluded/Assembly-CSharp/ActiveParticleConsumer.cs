using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class ActiveParticleConsumer : GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>
{
	// Token: 0x06000164 RID: 356 RVA: 0x00009B50 File Offset: 0x00007D50
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.root.Enter(delegate(ActiveParticleConsumer.Instance smi)
		{
			smi.GetComponent<Operational>().SetFlag(ActiveParticleConsumer.canConsumeParticlesFlag, false);
		});
		this.inoperational.EventTransition(GameHashes.OnParticleStorageChanged, this.operational, new StateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.Transition.ConditionCallback(this.IsReady)).ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForHighEnergyParticles, null);
		this.operational.DefaultState(this.operational.waiting).EventTransition(GameHashes.OnParticleStorageChanged, this.inoperational, GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.Not(new StateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.Transition.ConditionCallback(this.IsReady))).ToggleOperationalFlag(ActiveParticleConsumer.canConsumeParticlesFlag);
		this.operational.waiting.EventTransition(GameHashes.ActiveChanged, this.operational.consuming, (ActiveParticleConsumer.Instance smi) => smi.GetComponent<Operational>().IsActive);
		this.operational.consuming.EventTransition(GameHashes.ActiveChanged, this.operational.waiting, (ActiveParticleConsumer.Instance smi) => !smi.GetComponent<Operational>().IsActive).Update(delegate(ActiveParticleConsumer.Instance smi, float dt)
		{
			smi.Update(dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x06000165 RID: 357 RVA: 0x00009CB0 File Offset: 0x00007EB0
	public bool IsReady(ActiveParticleConsumer.Instance smi)
	{
		return smi.storage.Particles >= smi.def.minParticlesForOperational;
	}

	// Token: 0x040000CD RID: 205
	public static readonly Operational.Flag canConsumeParticlesFlag = new Operational.Flag("canConsumeParticles", Operational.Flag.Type.Requirement);

	// Token: 0x040000CE RID: 206
	public GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State inoperational;

	// Token: 0x040000CF RID: 207
	public ActiveParticleConsumer.OperationalStates operational;

	// Token: 0x02000E31 RID: 3633
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06006EE8 RID: 28392 RVA: 0x002B8A44 File Offset: 0x002B6C44
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.BUILDINGEFFECTS.ACTIVE_PARTICLE_CONSUMPTION.Replace("{Rate}", GameUtil.GetFormattedHighEnergyParticles(this.activeConsumptionRate, GameUtil.TimeSlice.PerSecond, true)), UI.BUILDINGEFFECTS.TOOLTIPS.ACTIVE_PARTICLE_CONSUMPTION.Replace("{Rate}", GameUtil.GetFormattedHighEnergyParticles(this.activeConsumptionRate, GameUtil.TimeSlice.PerSecond, true)), Descriptor.DescriptorType.Requirement, false)
			};
		}

		// Token: 0x04005316 RID: 21270
		public float activeConsumptionRate = 1f;

		// Token: 0x04005317 RID: 21271
		public float minParticlesForOperational = 1f;

		// Token: 0x04005318 RID: 21272
		public string meterSymbolName;
	}

	// Token: 0x02000E32 RID: 3634
	public class OperationalStates : GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State
	{
		// Token: 0x04005319 RID: 21273
		public GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State waiting;

		// Token: 0x0400531A RID: 21274
		public GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.State consuming;
	}

	// Token: 0x02000E33 RID: 3635
	public new class Instance : GameStateMachine<ActiveParticleConsumer, ActiveParticleConsumer.Instance, IStateMachineTarget, ActiveParticleConsumer.Def>.GameInstance
	{
		// Token: 0x06006EEB RID: 28395 RVA: 0x002B8AC1 File Offset: 0x002B6CC1
		public Instance(IStateMachineTarget master, ActiveParticleConsumer.Def def) : base(master, def)
		{
			this.storage = master.GetComponent<HighEnergyParticleStorage>();
		}

		// Token: 0x06006EEC RID: 28396 RVA: 0x002B8AD7 File Offset: 0x002B6CD7
		public void Update(float dt)
		{
			this.storage.ConsumeAndGet(dt * base.def.activeConsumptionRate);
		}

		// Token: 0x0400531B RID: 21275
		public bool ShowWorkingStatus;

		// Token: 0x0400531C RID: 21276
		public HighEnergyParticleStorage storage;
	}
}
