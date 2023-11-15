using System;

// Token: 0x0200061F RID: 1567
public class LandingBeacon : GameStateMachine<LandingBeacon, LandingBeacon.Instance>
{
	// Token: 0x0600278A RID: 10122 RVA: 0x000D6908 File Offset: 0x000D4B08
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.Update(new Action<LandingBeacon.Instance, float>(LandingBeacon.UpdateLineOfSight), UpdateRate.SIM_200ms, false);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.working, (LandingBeacon.Instance smi) => smi.operational.IsOperational);
		this.working.DefaultState(this.working.pre).EventTransition(GameHashes.OperationalChanged, this.off, (LandingBeacon.Instance smi) => !smi.operational.IsOperational);
		this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
		this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Enter("SetActive", delegate(LandingBeacon.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive", delegate(LandingBeacon.Instance smi)
		{
			smi.operational.SetActive(false, false);
		});
	}

	// Token: 0x0600278B RID: 10123 RVA: 0x000D6A4C File Offset: 0x000D4C4C
	public static void UpdateLineOfSight(LandingBeacon.Instance smi, float dt)
	{
		WorldContainer myWorld = smi.GetMyWorld();
		bool flag = true;
		int num = Grid.PosToCell(smi);
		int num2 = (int)myWorld.maximumBounds.y;
		while (Grid.CellRow(num) <= num2)
		{
			if (!Grid.IsValidCell(num) || Grid.Solid[num])
			{
				flag = false;
				break;
			}
			num = Grid.CellAbove(num);
		}
		if (smi.skyLastVisible != flag)
		{
			smi.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NoSurfaceSight, !flag, null);
			smi.operational.SetFlag(LandingBeacon.noSurfaceSight, flag);
			smi.skyLastVisible = flag;
		}
	}

	// Token: 0x040016DC RID: 5852
	public GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040016DD RID: 5853
	public LandingBeacon.WorkingStates working;

	// Token: 0x040016DE RID: 5854
	public static readonly Operational.Flag noSurfaceSight = new Operational.Flag("noSurfaceSight", Operational.Flag.Type.Requirement);

	// Token: 0x020012E3 RID: 4835
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020012E4 RID: 4836
	public class WorkingStates : GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040060FB RID: 24827
		public GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State pre;

		// Token: 0x040060FC RID: 24828
		public GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State loop;

		// Token: 0x040060FD RID: 24829
		public GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State pst;
	}

	// Token: 0x020012E5 RID: 4837
	public new class Instance : GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007EEA RID: 32490 RVA: 0x002EAC2C File Offset: 0x002E8E2C
		public Instance(IStateMachineTarget master, LandingBeacon.Def def) : base(master, def)
		{
			Components.LandingBeacons.Add(this);
			this.operational = base.GetComponent<Operational>();
			this.selectable = base.GetComponent<KSelectable>();
		}

		// Token: 0x06007EEB RID: 32491 RVA: 0x002EAC60 File Offset: 0x002E8E60
		public override void StartSM()
		{
			base.StartSM();
			LandingBeacon.UpdateLineOfSight(this, 0f);
		}

		// Token: 0x06007EEC RID: 32492 RVA: 0x002EAC73 File Offset: 0x002E8E73
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Components.LandingBeacons.Remove(this);
		}

		// Token: 0x06007EED RID: 32493 RVA: 0x002EAC86 File Offset: 0x002E8E86
		public bool CanBeTargeted()
		{
			return base.IsInsideState(base.sm.working);
		}

		// Token: 0x040060FE RID: 24830
		public Operational operational;

		// Token: 0x040060FF RID: 24831
		public KSelectable selectable;

		// Token: 0x04006100 RID: 24832
		public bool skyLastVisible = true;
	}
}
