using System;

// Token: 0x020008B4 RID: 2228
[SkipSaveFileSerialization]
public class Workaholic : StateMachineComponent<Workaholic.StatesInstance>
{
	// Token: 0x0600407D RID: 16509 RVA: 0x00168DF4 File Offset: 0x00166FF4
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0600407E RID: 16510 RVA: 0x00168E01 File Offset: 0x00167001
	protected bool IsUncomfortable()
	{
		return base.smi.master.GetComponent<ChoreDriver>().GetCurrentChore() is IdleChore;
	}

	// Token: 0x020016FD RID: 5885
	public class StatesInstance : GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.GameInstance
	{
		// Token: 0x06008D25 RID: 36133 RVA: 0x0031ABCB File Offset: 0x00318DCB
		public StatesInstance(Workaholic master) : base(master)
		{
		}
	}

	// Token: 0x020016FE RID: 5886
	public class States : GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic>
	{
		// Token: 0x06008D26 RID: 36134 RVA: 0x0031ABD4 File Offset: 0x00318DD4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.Update("WorkaholicCheck", delegate(Workaholic.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}, UpdateRate.SIM_1000ms, false);
			this.suffering.AddEffect("Restless").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04006D84 RID: 28036
		public GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.State satisfied;

		// Token: 0x04006D85 RID: 28037
		public GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.State suffering;
	}
}
