using System;

// Token: 0x02000495 RID: 1173
public class CleaningMonitor : GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>
{
	// Token: 0x06001A7C RID: 6780 RVA: 0x0008D488 File Offset: 0x0008B688
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.clean;
		this.clean.ToggleBehaviour(GameTags.Creatures.Cleaning, (CleaningMonitor.Instance smi) => smi.CanCleanElementState(), delegate(CleaningMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		});
		this.cooldown.ScheduleGoTo((CleaningMonitor.Instance smi) => smi.def.coolDown, this.clean);
	}

	// Token: 0x04000EB7 RID: 3767
	public GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.State cooldown;

	// Token: 0x04000EB8 RID: 3768
	public GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.State clean;

	// Token: 0x0200111E RID: 4382
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005B5C RID: 23388
		public Element.State elementState = Element.State.Liquid;

		// Token: 0x04005B5D RID: 23389
		public CellOffset[] cellOffsets;

		// Token: 0x04005B5E RID: 23390
		public float coolDown = 30f;
	}

	// Token: 0x0200111F RID: 4383
	public new class Instance : GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.GameInstance
	{
		// Token: 0x0600785F RID: 30815 RVA: 0x002D635E File Offset: 0x002D455E
		public Instance(IStateMachineTarget master, CleaningMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06007860 RID: 30816 RVA: 0x002D6368 File Offset: 0x002D4568
		public bool CanCleanElementState()
		{
			int num = Grid.PosToCell(base.smi.transform.GetPosition());
			if (!Grid.IsValidCell(num))
			{
				return false;
			}
			if (!Grid.IsLiquid(num) && base.smi.def.elementState == Element.State.Liquid)
			{
				return false;
			}
			if (Grid.DiseaseCount[num] > 0)
			{
				return true;
			}
			if (base.smi.def.cellOffsets != null)
			{
				foreach (CellOffset offset in base.smi.def.cellOffsets)
				{
					int num2 = Grid.OffsetCell(num, offset);
					if (Grid.IsValidCell(num2) && Grid.DiseaseCount[num2] > 0)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
