using System;
using UnityEngine;

// Token: 0x020008AE RID: 2222
[SkipSaveFileSerialization]
public class Climacophobic : StateMachineComponent<Climacophobic.StatesInstance>
{
	// Token: 0x0600406C RID: 16492 RVA: 0x00168B32 File Offset: 0x00166D32
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0600406D RID: 16493 RVA: 0x00168B40 File Offset: 0x00166D40
	protected bool IsUncomfortable()
	{
		int num = 5;
		int cell = Grid.PosToCell(base.gameObject);
		if (this.isCellLadder(cell))
		{
			int num2 = 1;
			bool flag = true;
			bool flag2 = true;
			for (int i = 1; i < num; i++)
			{
				int cell2 = Grid.OffsetCell(cell, 0, i);
				int cell3 = Grid.OffsetCell(cell, 0, -i);
				if (flag && this.isCellLadder(cell2))
				{
					num2++;
				}
				else
				{
					flag = false;
				}
				if (flag2 && this.isCellLadder(cell3))
				{
					num2++;
				}
				else
				{
					flag2 = false;
				}
			}
			return num2 >= num;
		}
		return false;
	}

	// Token: 0x0600406E RID: 16494 RVA: 0x00168BC8 File Offset: 0x00166DC8
	private bool isCellLadder(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		return !(gameObject == null) && !(gameObject.GetComponent<Ladder>() == null);
	}

	// Token: 0x020016F1 RID: 5873
	public class StatesInstance : GameStateMachine<Climacophobic.States, Climacophobic.StatesInstance, Climacophobic, object>.GameInstance
	{
		// Token: 0x06008D0B RID: 36107 RVA: 0x0031A7CA File Offset: 0x003189CA
		public StatesInstance(Climacophobic master) : base(master)
		{
		}
	}

	// Token: 0x020016F2 RID: 5874
	public class States : GameStateMachine<Climacophobic.States, Climacophobic.StatesInstance, Climacophobic>
	{
		// Token: 0x06008D0C RID: 36108 RVA: 0x0031A7D4 File Offset: 0x003189D4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.Update("ClimacophobicCheck", delegate(Climacophobic.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}, UpdateRate.SIM_1000ms, false);
			this.suffering.AddEffect("Vertigo").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04006D7A RID: 28026
		public GameStateMachine<Climacophobic.States, Climacophobic.StatesInstance, Climacophobic, object>.State satisfied;

		// Token: 0x04006D7B RID: 28027
		public GameStateMachine<Climacophobic.States, Climacophobic.StatesInstance, Climacophobic, object>.State suffering;
	}
}
