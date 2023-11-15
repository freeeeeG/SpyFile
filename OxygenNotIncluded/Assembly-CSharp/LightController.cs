using System;

// Token: 0x0200004B RID: 75
public class LightController : GameStateMachine<LightController, LightController.Instance>
{
	// Token: 0x06000168 RID: 360 RVA: 0x00009CE8 File Offset: 0x00007EE8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (LightController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (LightController.Instance smi) => !smi.GetComponent<Operational>().IsOperational).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingLight, null).Enter("SetActive", delegate(LightController.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(true, false);
		});
	}

	// Token: 0x040000D0 RID: 208
	public GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040000D1 RID: 209
	public GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x02000E35 RID: 3637
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000E36 RID: 3638
	public new class Instance : GameStateMachine<LightController, LightController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06006EF4 RID: 28404 RVA: 0x002B8B47 File Offset: 0x002B6D47
		public Instance(IStateMachineTarget master, LightController.Def def) : base(master, def)
		{
		}
	}
}
