using System;
using Klei.AI;
using STRINGS;

// Token: 0x020008B0 RID: 2224
[SkipSaveFileSerialization]
public class PrefersColder : StateMachineComponent<PrefersColder.StatesInstance>
{
	// Token: 0x06004073 RID: 16499 RVA: 0x00168C58 File Offset: 0x00166E58
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x020016F5 RID: 5877
	public class StatesInstance : GameStateMachine<PrefersColder.States, PrefersColder.StatesInstance, PrefersColder, object>.GameInstance
	{
		// Token: 0x06008D14 RID: 36116 RVA: 0x0031A946 File Offset: 0x00318B46
		public StatesInstance(PrefersColder master) : base(master)
		{
		}
	}

	// Token: 0x020016F6 RID: 5878
	public class States : GameStateMachine<PrefersColder.States, PrefersColder.StatesInstance, PrefersColder>
	{
		// Token: 0x06008D15 RID: 36117 RVA: 0x0031A94F File Offset: 0x00318B4F
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAttributeModifier(DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME, (PrefersColder.StatesInstance smi) => this.modifier, null);
		}

		// Token: 0x04006D7E RID: 28030
		private AttributeModifier modifier = new AttributeModifier("ThermalConductivityBarrier", 0.005f, DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME, false, false, true);
	}
}
