using System;
using Klei.AI;
using STRINGS;

// Token: 0x020008B1 RID: 2225
[SkipSaveFileSerialization]
public class PrefersWarmer : StateMachineComponent<PrefersWarmer.StatesInstance>
{
	// Token: 0x06004075 RID: 16501 RVA: 0x00168C6D File Offset: 0x00166E6D
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x020016F7 RID: 5879
	public class StatesInstance : GameStateMachine<PrefersWarmer.States, PrefersWarmer.StatesInstance, PrefersWarmer, object>.GameInstance
	{
		// Token: 0x06008D18 RID: 36120 RVA: 0x0031A9AE File Offset: 0x00318BAE
		public StatesInstance(PrefersWarmer master) : base(master)
		{
		}
	}

	// Token: 0x020016F8 RID: 5880
	public class States : GameStateMachine<PrefersWarmer.States, PrefersWarmer.StatesInstance, PrefersWarmer>
	{
		// Token: 0x06008D19 RID: 36121 RVA: 0x0031A9B7 File Offset: 0x00318BB7
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAttributeModifier(DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.NAME, (PrefersWarmer.StatesInstance smi) => this.modifier, null);
		}

		// Token: 0x04006D7F RID: 28031
		private AttributeModifier modifier = new AttributeModifier("ThermalConductivityBarrier", -0.005f, DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.NAME, false, false, true);
	}
}
