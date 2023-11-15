using System;
using UnityEngine;

// Token: 0x020003A2 RID: 930
public class RobotAi : GameStateMachine<RobotAi, RobotAi.Instance>
{
	// Token: 0x0600137B RID: 4987 RVA: 0x0006697C File Offset: 0x00064B7C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleStateMachine((RobotAi.Instance smi) => new DeathMonitor.Instance(smi.master, new DeathMonitor.Def())).Enter(delegate(RobotAi.Instance smi)
		{
			if (smi.HasTag(GameTags.Dead))
			{
				smi.GoTo(this.dead);
				return;
			}
			smi.GoTo(this.alive);
		});
		this.alive.DefaultState(this.alive.normal).TagTransition(GameTags.Dead, this.dead, false);
		this.alive.normal.TagTransition(GameTags.Stored, this.alive.stored, false).ToggleStateMachine((RobotAi.Instance smi) => new FallMonitor.Instance(smi.master, false, null));
		this.alive.stored.PlayAnim("in_storage").TagTransition(GameTags.Stored, this.alive.normal, true).ToggleBrain("stored").Enter(delegate(RobotAi.Instance smi)
		{
			smi.GetComponent<Navigator>().Pause("stored");
		}).Exit(delegate(RobotAi.Instance smi)
		{
			smi.GetComponent<Navigator>().Unpause("unstored");
		});
		this.dead.ToggleBrain("dead").ToggleComponent<Deconstructable>(false).ToggleStateMachine((RobotAi.Instance smi) => new FallWhenDeadMonitor.Instance(smi.master)).Enter("RefreshUserMenu", delegate(RobotAi.Instance smi)
		{
			smi.RefreshUserMenu();
		}).Enter("DropStorage", delegate(RobotAi.Instance smi)
		{
			smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
		});
	}

	// Token: 0x04000A6E RID: 2670
	public RobotAi.AliveStates alive;

	// Token: 0x04000A6F RID: 2671
	public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State dead;

	// Token: 0x02000FD7 RID: 4055
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000FD8 RID: 4056
	public class AliveStates : GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400570C RID: 22284
		public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State normal;

		// Token: 0x0400570D RID: 22285
		public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State stored;
	}

	// Token: 0x02000FD9 RID: 4057
	public new class Instance : GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060073A5 RID: 29605 RVA: 0x002C28F1 File Offset: 0x002C0AF1
		public Instance(IStateMachineTarget master, RobotAi.Def def) : base(master, def)
		{
			ChoreConsumer component = base.GetComponent<ChoreConsumer>();
			component.AddUrge(Db.Get().Urges.EmoteHighPriority);
			component.AddUrge(Db.Get().Urges.EmoteIdle);
		}

		// Token: 0x060073A6 RID: 29606 RVA: 0x002C292A File Offset: 0x002C0B2A
		public void RefreshUserMenu()
		{
			Game.Instance.userMenu.Refresh(base.master.gameObject);
		}
	}
}
