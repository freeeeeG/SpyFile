using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public class AttackStates : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>
{
	// Token: 0x060002E8 RID: 744 RVA: 0x000175C8 File Offset: 0x000157C8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.waitBeforeAttack;
		this.root.Enter("SetTarget", delegate(AttackStates.Instance smi)
		{
			this.target.Set(smi.GetSMI<ThreatMonitor.Instance>().MainThreat, smi, false);
			this.cellOffsets = smi.def.cellOffsets;
		});
		this.waitBeforeAttack.ScheduleGoTo((AttackStates.Instance smi) => UnityEngine.Random.Range(0f, 4f), this.approach);
		this.approach.InitializeStates(this.masterTarget, this.target, this.attack, null, this.cellOffsets, null).ToggleStatusItem(CREATURES.STATUSITEMS.ATTACK_APPROACH.NAME, CREATURES.STATUSITEMS.ATTACK_APPROACH.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.attack.DefaultState(this.attack.pre).ToggleStatusItem(CREATURES.STATUSITEMS.ATTACK.NAME, CREATURES.STATUSITEMS.ATTACK.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.attack.pre.PlayAnim((AttackStates.Instance smi) => smi.def.preAnim, KAnim.PlayMode.Once).Exit(delegate(AttackStates.Instance smi)
		{
			smi.GetComponent<Weapon>().AttackTarget(this.target.Get(smi));
		}).OnAnimQueueComplete(this.attack.pst);
		this.attack.pst.PlayAnim((AttackStates.Instance smi) => smi.def.pstAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Attack, false);
	}

	// Token: 0x04000201 RID: 513
	public StateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.TargetParameter target;

	// Token: 0x04000202 RID: 514
	public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.ApproachSubState<AttackableBase> approach;

	// Token: 0x04000203 RID: 515
	public CellOffset[] cellOffsets;

	// Token: 0x04000204 RID: 516
	public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State waitBeforeAttack;

	// Token: 0x04000205 RID: 517
	public AttackStates.AttackingStates attack;

	// Token: 0x04000206 RID: 518
	public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State behaviourcomplete;

	// Token: 0x02000E6C RID: 3692
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06006F7F RID: 28543 RVA: 0x002B95A8 File Offset: 0x002B77A8
		public Def(string pre_anim = "eat_pre", string pst_anim = "eat_pst", CellOffset[] cell_offsets = null)
		{
			this.preAnim = pre_anim;
			this.pstAnim = pst_anim;
			if (cell_offsets != null)
			{
				this.cellOffsets = cell_offsets;
			}
		}

		// Token: 0x04005385 RID: 21381
		public string preAnim;

		// Token: 0x04005386 RID: 21382
		public string pstAnim;

		// Token: 0x04005387 RID: 21383
		public CellOffset[] cellOffsets = new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(1, 0),
			new CellOffset(-1, 0),
			new CellOffset(1, 1),
			new CellOffset(-1, 1)
		};
	}

	// Token: 0x02000E6D RID: 3693
	public class AttackingStates : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State
	{
		// Token: 0x04005388 RID: 21384
		public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State pre;

		// Token: 0x04005389 RID: 21385
		public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State pst;
	}

	// Token: 0x02000E6E RID: 3694
	public new class Instance : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.GameInstance
	{
		// Token: 0x06006F81 RID: 28545 RVA: 0x002B962D File Offset: 0x002B782D
		public Instance(Chore<AttackStates.Instance> chore, AttackStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Attack);
		}
	}
}
