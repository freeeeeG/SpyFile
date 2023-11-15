using System;

// Token: 0x020008B2 RID: 2226
[SkipSaveFileSerialization]
public class SensitiveFeet : StateMachineComponent<SensitiveFeet.StatesInstance>
{
	// Token: 0x06004077 RID: 16503 RVA: 0x00168C82 File Offset: 0x00166E82
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06004078 RID: 16504 RVA: 0x00168C90 File Offset: 0x00166E90
	protected bool IsUncomfortable()
	{
		int num = Grid.CellBelow(Grid.PosToCell(base.gameObject));
		return Grid.IsValidCell(num) && Grid.Solid[num] && Grid.Objects[num, 9] == null;
	}

	// Token: 0x020016F9 RID: 5881
	public class StatesInstance : GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.GameInstance
	{
		// Token: 0x06008D1C RID: 36124 RVA: 0x0031AA16 File Offset: 0x00318C16
		public StatesInstance(SensitiveFeet master) : base(master)
		{
		}
	}

	// Token: 0x020016FA RID: 5882
	public class States : GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet>
	{
		// Token: 0x06008D1D RID: 36125 RVA: 0x0031AA20 File Offset: 0x00318C20
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.Update("SensitiveFeetCheck", delegate(SensitiveFeet.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}, UpdateRate.SIM_1000ms, false);
			this.suffering.AddEffect("UncomfortableFeet").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04006D80 RID: 28032
		public GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.State satisfied;

		// Token: 0x04006D81 RID: 28033
		public GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.State suffering;
	}
}
