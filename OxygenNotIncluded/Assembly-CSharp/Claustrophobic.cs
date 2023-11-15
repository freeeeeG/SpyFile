using System;

// Token: 0x020008AD RID: 2221
[SkipSaveFileSerialization]
public class Claustrophobic : StateMachineComponent<Claustrophobic.StatesInstance>
{
	// Token: 0x06004069 RID: 16489 RVA: 0x00168A92 File Offset: 0x00166C92
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0600406A RID: 16490 RVA: 0x00168AA0 File Offset: 0x00166CA0
	protected bool IsUncomfortable()
	{
		int num = 4;
		int cell = Grid.PosToCell(base.gameObject);
		for (int i = 0; i < num - 1; i++)
		{
			int num2 = Grid.OffsetCell(cell, 0, i);
			if (Grid.IsValidCell(num2) && Grid.Solid[num2])
			{
				return true;
			}
			if (Grid.IsValidCell(Grid.CellRight(cell)) && Grid.IsValidCell(Grid.CellLeft(cell)) && Grid.Solid[Grid.CellRight(cell)] && Grid.Solid[Grid.CellLeft(cell)])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x020016EF RID: 5871
	public class StatesInstance : GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic, object>.GameInstance
	{
		// Token: 0x06008D07 RID: 36103 RVA: 0x0031A72B File Offset: 0x0031892B
		public StatesInstance(Claustrophobic master) : base(master)
		{
		}
	}

	// Token: 0x020016F0 RID: 5872
	public class States : GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic>
	{
		// Token: 0x06008D08 RID: 36104 RVA: 0x0031A734 File Offset: 0x00318934
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.Update("ClaustrophobicCheck", delegate(Claustrophobic.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}, UpdateRate.SIM_1000ms, false);
			this.suffering.AddEffect("Claustrophobic").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04006D78 RID: 28024
		public GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic, object>.State satisfied;

		// Token: 0x04006D79 RID: 28025
		public GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic, object>.State suffering;
	}
}
