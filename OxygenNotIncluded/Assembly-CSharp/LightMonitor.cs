using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000884 RID: 2180
public class LightMonitor : GameStateMachine<LightMonitor, LightMonitor.Instance>
{
	// Token: 0x06003F74 RID: 16244 RVA: 0x001628E0 File Offset: 0x00160AE0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.unburnt;
		this.root.EventTransition(GameHashes.SicknessAdded, this.burnt, (LightMonitor.Instance smi) => smi.gameObject.GetSicknesses().Has(Db.Get().Sicknesses.Sunburn)).Update(new Action<LightMonitor.Instance, float>(LightMonitor.CheckLightLevel), UpdateRate.SIM_1000ms, false);
		this.unburnt.DefaultState(this.unburnt.safe).ParamTransition<float>(this.burnResistance, this.get_burnt, GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.IsLTEZero);
		this.unburnt.safe.DefaultState(this.unburnt.safe.unlit).Update(delegate(LightMonitor.Instance smi, float dt)
		{
			smi.sm.burnResistance.DeltaClamp(dt * 0.25f, 0f, 120f, smi);
		}, UpdateRate.SIM_200ms, false);
		this.unburnt.safe.unlit.ParamTransition<float>(this.lightLevel, this.unburnt.safe.normal_light, GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.IsGTZero);
		this.unburnt.safe.normal_light.ParamTransition<float>(this.lightLevel, this.unburnt.safe.unlit, GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.IsLTEZero).ParamTransition<float>(this.lightLevel, this.unburnt.safe.sunlight, (LightMonitor.Instance smi, float p) => p >= 40000f);
		this.unburnt.safe.sunlight.ParamTransition<float>(this.lightLevel, this.unburnt.safe.normal_light, (LightMonitor.Instance smi, float p) => p < 40000f).ParamTransition<float>(this.lightLevel, this.unburnt.burning, (LightMonitor.Instance smi, float p) => p >= 72000f).ToggleEffect("Sunlight_Pleasant");
		this.unburnt.burning.ParamTransition<float>(this.lightLevel, this.unburnt.safe.sunlight, (LightMonitor.Instance smi, float p) => p < 72000f).Update(delegate(LightMonitor.Instance smi, float dt)
		{
			smi.sm.burnResistance.DeltaClamp(-dt, 0f, 120f, smi);
		}, UpdateRate.SIM_200ms, false).ToggleEffect("Sunlight_Burning");
		this.get_burnt.Enter(delegate(LightMonitor.Instance smi)
		{
			smi.gameObject.GetSicknesses().Infect(new SicknessExposureInfo(Db.Get().Sicknesses.Sunburn.Id, DUPLICANTS.DISEASES.SUNBURNSICKNESS.SUNEXPOSURE));
		}).GoTo(this.burnt);
		this.burnt.EventTransition(GameHashes.SicknessCured, this.unburnt, (LightMonitor.Instance smi) => !smi.gameObject.GetSicknesses().Has(Db.Get().Sicknesses.Sunburn)).Exit(delegate(LightMonitor.Instance smi)
		{
			smi.sm.burnResistance.Set(120f, smi, false);
		});
	}

	// Token: 0x06003F75 RID: 16245 RVA: 0x00162BE0 File Offset: 0x00160DE0
	private static void CheckLightLevel(LightMonitor.Instance smi, float dt)
	{
		KPrefabID component = smi.GetComponent<KPrefabID>();
		if (component != null && component.HasTag(GameTags.Shaded))
		{
			smi.sm.lightLevel.Set(0f, smi, false);
			return;
		}
		int num = Grid.PosToCell(smi.gameObject);
		if (Grid.IsValidCell(num))
		{
			smi.sm.lightLevel.Set((float)Grid.LightIntensity[num], smi, false);
		}
	}

	// Token: 0x04002936 RID: 10550
	public const float BURN_RESIST_RECOVERY_FACTOR = 0.25f;

	// Token: 0x04002937 RID: 10551
	public StateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.FloatParameter lightLevel;

	// Token: 0x04002938 RID: 10552
	public StateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.FloatParameter burnResistance = new StateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);

	// Token: 0x04002939 RID: 10553
	public LightMonitor.UnburntStates unburnt;

	// Token: 0x0400293A RID: 10554
	public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State get_burnt;

	// Token: 0x0400293B RID: 10555
	public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State burnt;

	// Token: 0x0200168C RID: 5772
	public class UnburntStates : GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006C1A RID: 27674
		public LightMonitor.SafeStates safe;

		// Token: 0x04006C1B RID: 27675
		public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State burning;
	}

	// Token: 0x0200168D RID: 5773
	public class SafeStates : GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006C1C RID: 27676
		public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State unlit;

		// Token: 0x04006C1D RID: 27677
		public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State normal_light;

		// Token: 0x04006C1E RID: 27678
		public GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.State sunlight;
	}

	// Token: 0x0200168E RID: 5774
	public new class Instance : GameStateMachine<LightMonitor, LightMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008B5B RID: 35675 RVA: 0x003164EE File Offset: 0x003146EE
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.effects = base.GetComponent<Effects>();
		}

		// Token: 0x04006C1F RID: 27679
		public Effects effects;
	}
}
