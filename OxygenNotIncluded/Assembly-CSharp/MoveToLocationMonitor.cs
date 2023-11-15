using System;
using STRINGS;

// Token: 0x02000888 RID: 2184
public class MoveToLocationMonitor : GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance>
{
	// Token: 0x06003F84 RID: 16260 RVA: 0x00162F10 File Offset: 0x00161110
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.DoNothing();
		this.moving.ToggleChore((MoveToLocationMonitor.Instance smi) => new MoveChore(smi.master, Db.Get().ChoreTypes.MoveTo, (MoveChore.StatesInstance smii) => smi.targetCell, false), this.satisfied);
	}

	// Token: 0x04002940 RID: 10560
	public GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002941 RID: 10561
	public GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, object>.State moving;

	// Token: 0x02001697 RID: 5783
	public new class Instance : GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008B7F RID: 35711 RVA: 0x00316970 File Offset: 0x00314B70
		public Instance(IStateMachineTarget master) : base(master)
		{
			master.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
		}

		// Token: 0x06008B80 RID: 35712 RVA: 0x00316994 File Offset: 0x00314B94
		private void OnRefreshUserMenu(object data)
		{
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.MOVETOLOCATION.NAME, new System.Action(this.OnClickMoveToLocation), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MOVETOLOCATION.TOOLTIP, true), 0.2f);
		}

		// Token: 0x06008B81 RID: 35713 RVA: 0x003169EE File Offset: 0x00314BEE
		private void OnClickMoveToLocation()
		{
			MoveToLocationTool.Instance.Activate(base.GetComponent<Navigator>());
		}

		// Token: 0x06008B82 RID: 35714 RVA: 0x00316A00 File Offset: 0x00314C00
		public void MoveToLocation(int cell)
		{
			this.targetCell = cell;
			base.smi.GoTo(base.smi.sm.satisfied);
			base.smi.GoTo(base.smi.sm.moving);
		}

		// Token: 0x06008B83 RID: 35715 RVA: 0x00316A3F File Offset: 0x00314C3F
		public override void StopSM(string reason)
		{
			base.master.Unsubscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
			base.StopSM(reason);
		}

		// Token: 0x04006C3C RID: 27708
		public int targetCell;
	}
}
