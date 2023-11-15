using System;

// Token: 0x020008B3 RID: 2227
[SkipSaveFileSerialization]
public class SolitarySleeper : StateMachineComponent<SolitarySleeper.StatesInstance>
{
	// Token: 0x0600407A RID: 16506 RVA: 0x00168CE3 File Offset: 0x00166EE3
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0600407B RID: 16507 RVA: 0x00168CF0 File Offset: 0x00166EF0
	protected bool IsUncomfortable()
	{
		if (!base.gameObject.GetSMI<StaminaMonitor.Instance>().IsSleeping())
		{
			return false;
		}
		int num = 5;
		bool flag = true;
		bool flag2 = true;
		int cell = Grid.PosToCell(base.gameObject);
		for (int i = 1; i < num; i++)
		{
			int num2 = Grid.OffsetCell(cell, i, 0);
			int num3 = Grid.OffsetCell(cell, -i, 0);
			if (Grid.Solid[num3])
			{
				flag = false;
			}
			if (Grid.Solid[num2])
			{
				flag2 = false;
			}
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
			{
				if (flag && Grid.PosToCell(minionIdentity.gameObject) == num3)
				{
					return true;
				}
				if (flag2 && Grid.PosToCell(minionIdentity.gameObject) == num2)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x020016FB RID: 5883
	public class StatesInstance : GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.GameInstance
	{
		// Token: 0x06008D20 RID: 36128 RVA: 0x0031AAB6 File Offset: 0x00318CB6
		public StatesInstance(SolitarySleeper master) : base(master)
		{
		}
	}

	// Token: 0x020016FC RID: 5884
	public class States : GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper>
	{
		// Token: 0x06008D21 RID: 36129 RVA: 0x0031AAC0 File Offset: 0x00318CC0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.TagTransition(GameTags.Dead, null, false).EventTransition(GameHashes.NewDay, this.satisfied, null).Update("SolitarySleeperCheck", delegate(SolitarySleeper.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					if (smi.GetCurrentState() != this.suffering)
					{
						smi.GoTo(this.suffering);
						return;
					}
				}
				else if (smi.GetCurrentState() != this.satisfied)
				{
					smi.GoTo(this.satisfied);
				}
			}, UpdateRate.SIM_4000ms, false);
			this.suffering.AddEffect("PeopleTooCloseWhileSleeping").ToggleExpression(Db.Get().Expressions.Uncomfortable, null).Update("PeopleTooCloseSleepFail", delegate(SolitarySleeper.StatesInstance smi, float dt)
			{
				smi.master.gameObject.Trigger(1338475637, this);
			}, UpdateRate.SIM_1000ms, false);
			this.satisfied.DoNothing();
		}

		// Token: 0x04006D82 RID: 28034
		public GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.State satisfied;

		// Token: 0x04006D83 RID: 28035
		public GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.State suffering;
	}
}
