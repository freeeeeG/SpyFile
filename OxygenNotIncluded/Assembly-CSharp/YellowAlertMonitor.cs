using System;

// Token: 0x020008A5 RID: 2213
public class YellowAlertMonitor : GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance>
{
	// Token: 0x06004018 RID: 16408 RVA: 0x00166F38 File Offset: 0x00165138
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.off.EventTransition(GameHashes.EnteredYellowAlert, (YellowAlertMonitor.Instance smi) => Game.Instance, this.on, (YellowAlertMonitor.Instance smi) => YellowAlertManager.Instance.Get().IsOn());
		this.on.EventTransition(GameHashes.ExitedYellowAlert, (YellowAlertMonitor.Instance smi) => Game.Instance, this.off, (YellowAlertMonitor.Instance smi) => !YellowAlertManager.Instance.Get().IsOn()).Enter("EnableYellowAlert", delegate(YellowAlertMonitor.Instance smi)
		{
			smi.EnableYellowAlert();
		});
	}

	// Token: 0x040029C3 RID: 10691
	public GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040029C4 RID: 10692
	public GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x020016DD RID: 5853
	public new class Instance : GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008CD8 RID: 36056 RVA: 0x00319F80 File Offset: 0x00318180
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06008CD9 RID: 36057 RVA: 0x00319F89 File Offset: 0x00318189
		public void EnableYellowAlert()
		{
		}
	}
}
