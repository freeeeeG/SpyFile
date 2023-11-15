using System;
using UnityEngine;

// Token: 0x020000AF RID: 175
public class BeeSleepMonitor : GameStateMachine<BeeSleepMonitor, BeeSleepMonitor.Instance, IStateMachineTarget, BeeSleepMonitor.Def>
{
	// Token: 0x0600031B RID: 795 RVA: 0x00018CE2 File Offset: 0x00016EE2
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update(new Action<BeeSleepMonitor.Instance, float>(this.UpdateCO2Exposure), UpdateRate.SIM_1000ms, false).ToggleBehaviour(GameTags.Creatures.BeeWantsToSleep, new StateMachine<BeeSleepMonitor, BeeSleepMonitor.Instance, IStateMachineTarget, BeeSleepMonitor.Def>.Transition.ConditionCallback(this.ShouldSleep), null);
	}

	// Token: 0x0600031C RID: 796 RVA: 0x00018D1D File Offset: 0x00016F1D
	public bool ShouldSleep(BeeSleepMonitor.Instance smi)
	{
		return smi.CO2Exposure >= 5f;
	}

	// Token: 0x0600031D RID: 797 RVA: 0x00018D30 File Offset: 0x00016F30
	public void UpdateCO2Exposure(BeeSleepMonitor.Instance smi, float dt)
	{
		if (this.IsInCO2(smi))
		{
			smi.CO2Exposure += 1f;
		}
		else
		{
			smi.CO2Exposure -= 0.5f;
		}
		smi.CO2Exposure = Mathf.Clamp(smi.CO2Exposure, 0f, 10f);
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00018D88 File Offset: 0x00016F88
	public bool IsInCO2(BeeSleepMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi.gameObject);
		return Grid.IsValidCell(num) && Grid.Element[num].id == SimHashes.CarbonDioxide;
	}

	// Token: 0x02000E8C RID: 3724
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000E8D RID: 3725
	public new class Instance : GameStateMachine<BeeSleepMonitor, BeeSleepMonitor.Instance, IStateMachineTarget, BeeSleepMonitor.Def>.GameInstance
	{
		// Token: 0x06006FBC RID: 28604 RVA: 0x002B9C13 File Offset: 0x002B7E13
		public Instance(IStateMachineTarget master, BeeSleepMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x040053CC RID: 21452
		public float CO2Exposure;
	}
}
