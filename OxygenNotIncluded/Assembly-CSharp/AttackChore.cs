using System;
using UnityEngine;

// Token: 0x020003A4 RID: 932
public class AttackChore : Chore<AttackChore.StatesInstance>
{
	// Token: 0x06001381 RID: 4993 RVA: 0x00066C55 File Offset: 0x00064E55
	protected override void OnStateMachineStop(string reason, StateMachine.Status status)
	{
		this.CleanUpMultitool();
		base.OnStateMachineStop(reason, status);
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x00066C68 File Offset: 0x00064E68
	public AttackChore(IStateMachineTarget target, GameObject enemy) : base(Db.Get().ChoreTypes.Attack, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new AttackChore.StatesInstance(this);
		base.smi.sm.attackTarget.Set(enemy, base.smi, false);
		Game.Instance.Trigger(1980521255, enemy);
		base.SetPrioritizable(enemy.GetComponent<Prioritizable>());
	}

	// Token: 0x06001383 RID: 4995 RVA: 0x00066CE4 File Offset: 0x00064EE4
	public string GetHitAnim()
	{
		Workable component = base.smi.sm.attackTarget.Get(base.smi).gameObject.GetComponent<Workable>();
		if (component)
		{
			return MultitoolController.GetAnimationStrings(component, this.gameObject.GetComponent<Worker>(), "hit")[1].Replace("_loop", "");
		}
		return "hit";
	}

	// Token: 0x06001384 RID: 4996 RVA: 0x00066D4C File Offset: 0x00064F4C
	public void OnTargetMoved(object data)
	{
		int num = Grid.PosToCell(base.smi.master.gameObject);
		if (base.smi.sm.attackTarget.Get(base.smi) == null)
		{
			this.CleanUpMultitool();
			return;
		}
		if (base.smi.GetCurrentState() == base.smi.sm.attack)
		{
			int num2 = Grid.PosToCell(base.smi.sm.attackTarget.Get(base.smi).gameObject);
			IApproachable component = base.smi.sm.attackTarget.Get(base.smi).gameObject.GetComponent<IApproachable>();
			if (component != null)
			{
				CellOffset[] offsets = component.GetOffsets();
				if (num == num2 || !Grid.IsCellOffsetOf(num, num2, offsets))
				{
					if (this.multiTool != null)
					{
						this.CleanUpMultitool();
					}
					base.smi.GoTo(base.smi.sm.approachtarget);
				}
			}
			else
			{
				global::Debug.Log("has no approachable");
			}
		}
		if (this.multiTool != null)
		{
			this.multiTool.UpdateHitEffectTarget();
		}
	}

	// Token: 0x06001385 RID: 4997 RVA: 0x00066E67 File Offset: 0x00065067
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.attacker.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x06001386 RID: 4998 RVA: 0x00066E98 File Offset: 0x00065098
	protected override void End(string reason)
	{
		this.CleanUpMultitool();
		if (!base.smi.sm.attackTarget.IsNull(base.smi))
		{
			GameObject gameObject = base.smi.sm.attackTarget.Get(base.smi);
			Prioritizable component = gameObject.GetComponent<Prioritizable>();
			if (component != null && component.IsPrioritizable())
			{
				Prioritizable.RemoveRef(gameObject);
			}
		}
		base.End(reason);
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x00066F09 File Offset: 0x00065109
	public void OnTargetDestroyed(object data)
	{
		this.Fail("target destroyed");
	}

	// Token: 0x06001388 RID: 5000 RVA: 0x00066F16 File Offset: 0x00065116
	private void CleanUpMultitool()
	{
		if (base.smi.master.multiTool != null)
		{
			this.multiTool.DestroyHitEffect();
			this.multiTool.StopSM("attack complete");
			this.multiTool = null;
		}
	}

	// Token: 0x04000A70 RID: 2672
	private MultitoolController.Instance multiTool;

	// Token: 0x02000FDD RID: 4061
	public class StatesInstance : GameStateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.GameInstance
	{
		// Token: 0x060073B6 RID: 29622 RVA: 0x002C2F19 File Offset: 0x002C1119
		public StatesInstance(AttackChore master) : base(master)
		{
		}
	}

	// Token: 0x02000FDE RID: 4062
	public class States : GameStateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore>
	{
		// Token: 0x060073B7 RID: 29623 RVA: 0x002C2F24 File Offset: 0x002C1124
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approachtarget;
			this.root.ToggleStatusItem(Db.Get().DuplicantStatusItems.Fighting, (AttackChore.StatesInstance smi) => smi.master.gameObject).EventHandler(GameHashes.TargetLost, delegate(AttackChore.StatesInstance smi)
			{
				smi.master.Fail("target lost");
			}).Enter(delegate(AttackChore.StatesInstance smi)
			{
				smi.master.GetComponent<Weapon>().Configure(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0f);
			});
			this.approachtarget.InitializeStates(this.attacker, this.attackTarget, this.attack, null, null, NavigationTactics.Range_3_ProhibitOverlap).Enter(delegate(AttackChore.StatesInstance smi)
			{
				smi.master.CleanUpMultitool();
				smi.master.Trigger(1039067354, this.attackTarget.Get(smi));
				Health component = this.attackTarget.Get(smi).GetComponent<Health>();
				if (component == null || component.IsDefeated())
				{
					smi.StopSM("target defeated approachtarget");
				}
			});
			this.attack.Target(this.attacker).Enter(delegate(AttackChore.StatesInstance smi)
			{
				this.attackTarget.Get(smi).Subscribe(1088554450, new Action<object>(smi.master.OnTargetMoved));
				if (this.attackTarget != null && smi.master.multiTool == null)
				{
					smi.master.multiTool = new MultitoolController.Instance(this.attackTarget.Get(smi).GetComponent<Workable>(), smi.master.GetComponent<Worker>(), "attack", Assets.GetPrefab(EffectConfigs.AttackSplashId));
					smi.master.multiTool.StartSM();
				}
				this.attackTarget.Get(smi).Subscribe(1969584890, new Action<object>(smi.master.OnTargetDestroyed));
				smi.ScheduleGoTo(0.5f, this.success);
			}).Update(delegate(AttackChore.StatesInstance smi, float dt)
			{
				if (smi.master.multiTool != null)
				{
					smi.master.multiTool.UpdateHitEffectTarget();
				}
			}, UpdateRate.SIM_200ms, false).Exit(delegate(AttackChore.StatesInstance smi)
			{
				if (this.attackTarget.Get(smi) != null)
				{
					this.attackTarget.Get(smi).Unsubscribe(1088554450, new Action<object>(smi.master.OnTargetMoved));
				}
			});
			this.success.Enter("finishAttack", delegate(AttackChore.StatesInstance smi)
			{
				if (this.attackTarget.Get(smi) != null)
				{
					Transform transform = this.attackTarget.Get(smi).transform;
					Weapon component = this.attacker.Get(smi).gameObject.GetComponent<Weapon>();
					if (!(component != null))
					{
						smi.master.CleanUpMultitool();
						smi.StopSM("no weapon");
						return;
					}
					component.AttackTarget(transform.gameObject);
					Health component2 = this.attackTarget.Get(smi).GetComponent<Health>();
					if (component2 != null)
					{
						if (!component2.IsDefeated())
						{
							smi.GoTo(this.attack);
							return;
						}
						smi.master.CleanUpMultitool();
						smi.StopSM("target defeated success");
						return;
					}
				}
				else
				{
					smi.master.CleanUpMultitool();
					smi.StopSM("no target");
				}
			}).ReturnSuccess();
		}

		// Token: 0x04005720 RID: 22304
		public StateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.TargetParameter attackTarget;

		// Token: 0x04005721 RID: 22305
		public StateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.TargetParameter attacker;

		// Token: 0x04005722 RID: 22306
		public GameStateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.ApproachSubState<RangedAttackable> approachtarget;

		// Token: 0x04005723 RID: 22307
		public GameStateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.State attack;

		// Token: 0x04005724 RID: 22308
		public GameStateMachine<AttackChore.States, AttackChore.StatesInstance, AttackChore, object>.State success;
	}
}
