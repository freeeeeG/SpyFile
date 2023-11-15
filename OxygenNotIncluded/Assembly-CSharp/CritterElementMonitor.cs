using System;

// Token: 0x02000713 RID: 1811
public class CritterElementMonitor : GameStateMachine<CritterElementMonitor, CritterElementMonitor.Instance, IStateMachineTarget>
{
	// Token: 0x060031C6 RID: 12742 RVA: 0x0010873D File Offset: 0x0010693D
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update("UpdateInElement", delegate(CritterElementMonitor.Instance smi, float dt)
		{
			smi.UpdateCurrentElement(dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x02001475 RID: 5237
	public new class Instance : GameStateMachine<CritterElementMonitor, CritterElementMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x14000031 RID: 49
		// (add) Token: 0x060084AF RID: 33967 RVA: 0x00303050 File Offset: 0x00301250
		// (remove) Token: 0x060084B0 RID: 33968 RVA: 0x00303088 File Offset: 0x00301288
		public event Action<float> OnUpdateEggChances;

		// Token: 0x060084B1 RID: 33969 RVA: 0x003030BD File Offset: 0x003012BD
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060084B2 RID: 33970 RVA: 0x003030C6 File Offset: 0x003012C6
		public void UpdateCurrentElement(float dt)
		{
			this.OnUpdateEggChances(dt);
		}
	}
}
