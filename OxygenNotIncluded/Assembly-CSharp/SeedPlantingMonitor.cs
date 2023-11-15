using System;
using UnityEngine;

// Token: 0x020000DD RID: 221
public class SeedPlantingMonitor : GameStateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>
{
	// Token: 0x060003FA RID: 1018 RVA: 0x0001EC0C File Offset: 0x0001CE0C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToPlantSeed, new StateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>.Transition.ConditionCallback(SeedPlantingMonitor.ShouldSearchForSeeds), delegate(SeedPlantingMonitor.Instance smi)
		{
			smi.RefreshSearchTime();
		});
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x0001EC5D File Offset: 0x0001CE5D
	public static bool ShouldSearchForSeeds(SeedPlantingMonitor.Instance smi)
	{
		return Time.time >= smi.nextSearchTime;
	}

	// Token: 0x02000F1D RID: 3869
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040054F8 RID: 21752
		public float searchMinInterval = 60f;

		// Token: 0x040054F9 RID: 21753
		public float searchMaxInterval = 300f;
	}

	// Token: 0x02000F1E RID: 3870
	public new class Instance : GameStateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>.GameInstance
	{
		// Token: 0x06007110 RID: 28944 RVA: 0x002BC197 File Offset: 0x002BA397
		public Instance(IStateMachineTarget master, SeedPlantingMonitor.Def def) : base(master, def)
		{
			this.RefreshSearchTime();
		}

		// Token: 0x06007111 RID: 28945 RVA: 0x002BC1A7 File Offset: 0x002BA3A7
		public void RefreshSearchTime()
		{
			this.nextSearchTime = Time.time + Mathf.Lerp(base.def.searchMinInterval, base.def.searchMaxInterval, UnityEngine.Random.value);
		}

		// Token: 0x040054FA RID: 21754
		public float nextSearchTime;
	}
}
