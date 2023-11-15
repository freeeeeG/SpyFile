using System;

// Token: 0x0200088E RID: 2190
public class RedAlertMonitor : GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance>
{
	// Token: 0x06003FB8 RID: 16312 RVA: 0x0016454C File Offset: 0x0016274C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.off.EventTransition(GameHashes.EnteredRedAlert, (RedAlertMonitor.Instance smi) => Game.Instance, this.on, (RedAlertMonitor.Instance smi) => smi.master.gameObject.GetMyWorld().AlertManager.IsRedAlert());
		this.on.EventTransition(GameHashes.ExitedRedAlert, (RedAlertMonitor.Instance smi) => Game.Instance, this.off, (RedAlertMonitor.Instance smi) => !smi.master.gameObject.GetMyWorld().AlertManager.IsRedAlert()).Enter("EnableRedAlert", delegate(RedAlertMonitor.Instance smi)
		{
			smi.EnableRedAlert();
		}).ToggleEffect("RedAlert").ToggleExpression(Db.Get().Expressions.RedAlert, null);
	}

	// Token: 0x0400296A RID: 10602
	public GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x0400296B RID: 10603
	public GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x020016A6 RID: 5798
	public new class Instance : GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008BBC RID: 35772 RVA: 0x00316FB0 File Offset: 0x003151B0
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06008BBD RID: 35773 RVA: 0x00316FBC File Offset: 0x003151BC
		public void EnableRedAlert()
		{
			ChoreDriver component = base.GetComponent<ChoreDriver>();
			if (component != null)
			{
				Chore currentChore = component.GetCurrentChore();
				if (currentChore != null)
				{
					bool flag = false;
					for (int i = 0; i < currentChore.GetPreconditions().Count; i++)
					{
						if (currentChore.GetPreconditions()[i].id == ChorePreconditions.instance.IsNotRedAlert.id)
						{
							flag = true;
						}
					}
					if (flag)
					{
						component.StopChore();
					}
				}
			}
		}
	}
}
