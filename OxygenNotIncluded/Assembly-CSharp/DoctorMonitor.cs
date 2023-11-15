using System;

// Token: 0x02000875 RID: 2165
public class DoctorMonitor : GameStateMachine<DoctorMonitor, DoctorMonitor.Instance>
{
	// Token: 0x06003F3B RID: 16187 RVA: 0x00160F7B File Offset: 0x0015F17B
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.ToggleUrge(Db.Get().Urges.Doctor);
	}

	// Token: 0x0200166A RID: 5738
	public new class Instance : GameStateMachine<DoctorMonitor, DoctorMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008AB3 RID: 35507 RVA: 0x0031475A File Offset: 0x0031295A
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
