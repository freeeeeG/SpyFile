using System;
using STRINGS;

// Token: 0x020000B9 RID: 185
public class DefendStates : GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>
{
	// Token: 0x06000355 RID: 853 RVA: 0x0001A744 File Offset: 0x00018944
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.protectEntity.moveToThreat;
		this.root.Enter("SetTarget", delegate(DefendStates.Instance smi)
		{
			this.target.Set(smi.GetSMI<EggProtectionMonitor.Instance>().MainThreat, smi, false);
		}).ToggleStatusItem(CREATURES.STATUSITEMS.ATTACKINGENTITY.NAME, CREATURES.STATUSITEMS.ATTACKINGENTITY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.protectEntity.moveToThreat.InitializeStates(this.masterTarget, this.target, this.protectEntity.attackThreat, null, new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(1, 0),
			new CellOffset(-1, 0),
			new CellOffset(1, 1),
			new CellOffset(-1, 1)
		}, null);
		this.protectEntity.attackThreat.Enter(delegate(DefendStates.Instance smi)
		{
			smi.Play("slap_pre", KAnim.PlayMode.Once);
			smi.Queue("slap", KAnim.PlayMode.Once);
			smi.Queue("slap_pst", KAnim.PlayMode.Once);
			smi.Schedule(0.5f, delegate
			{
				smi.GetComponent<Weapon>().AttackTarget(this.target.Get(smi));
			}, null);
		}).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Defend, false);
	}

	// Token: 0x0400023C RID: 572
	public StateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.TargetParameter target;

	// Token: 0x0400023D RID: 573
	public DefendStates.ProtectStates protectEntity;

	// Token: 0x0400023E RID: 574
	public GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State behaviourcomplete;

	// Token: 0x02000EAF RID: 3759
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000EB0 RID: 3760
	public new class Instance : GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.GameInstance
	{
		// Token: 0x06007013 RID: 28691 RVA: 0x002BA40B File Offset: 0x002B860B
		public Instance(Chore<DefendStates.Instance> chore, DefendStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Defend);
		}
	}

	// Token: 0x02000EB1 RID: 3761
	public class ProtectStates : GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State
	{
		// Token: 0x04005424 RID: 21540
		public GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.ApproachSubState<AttackableBase> moveToThreat;

		// Token: 0x04005425 RID: 21541
		public GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State attackThreat;
	}
}
