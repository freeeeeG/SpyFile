using System;
using Klei.AI;

// Token: 0x0200086A RID: 2154
public class BreathMonitor : GameStateMachine<BreathMonitor, BreathMonitor.Instance>
{
	// Token: 0x06003F0D RID: 16141 RVA: 0x0015FC00 File Offset: 0x0015DE00
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.DefaultState(this.satisfied.full).Transition(this.lowbreath, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsLowBreath), UpdateRate.SIM_200ms);
		this.satisfied.full.Transition(this.satisfied.notfull, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsNotFullBreath), UpdateRate.SIM_200ms).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.HideBreathBar));
		this.satisfied.notfull.Transition(this.satisfied.full, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsFullBreath), UpdateRate.SIM_200ms).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.ShowBreathBar));
		this.lowbreath.DefaultState(this.lowbreath.nowheretorecover).Transition(this.satisfied, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsFullBreath), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.RecoverBreath, new Func<BreathMonitor.Instance, bool>(BreathMonitor.IsNotInBreathableArea)).ToggleUrge(Db.Get().Urges.RecoverBreath).ToggleThought(Db.Get().Thoughts.Suffocating, null).ToggleTag(GameTags.HoldingBreath).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.ShowBreathBar)).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.UpdateRecoverBreathCell)).Update(new Action<BreathMonitor.Instance, float>(BreathMonitor.UpdateRecoverBreathCell), UpdateRate.RENDER_1000ms, true);
		this.lowbreath.nowheretorecover.ParamTransition<int>(this.recoverBreathCell, this.lowbreath.recoveryavailable, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>.Callback(BreathMonitor.IsValidRecoverCell));
		this.lowbreath.recoveryavailable.ParamTransition<int>(this.recoverBreathCell, this.lowbreath.nowheretorecover, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>.Callback(BreathMonitor.IsNotValidRecoverCell)).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.UpdateRecoverBreathCell)).ToggleChore(new Func<BreathMonitor.Instance, Chore>(BreathMonitor.CreateRecoverBreathChore), this.lowbreath.nowheretorecover);
	}

	// Token: 0x06003F0E RID: 16142 RVA: 0x0015FDF8 File Offset: 0x0015DFF8
	private static bool IsLowBreath(BreathMonitor.Instance smi)
	{
		WorldContainer myWorld = smi.master.gameObject.GetMyWorld();
		if (!(myWorld == null) && myWorld.AlertManager.IsRedAlert())
		{
			return smi.breath.value < 45.454548f;
		}
		return smi.breath.value < 72.72727f;
	}

	// Token: 0x06003F0F RID: 16143 RVA: 0x0015FE51 File Offset: 0x0015E051
	private static Chore CreateRecoverBreathChore(BreathMonitor.Instance smi)
	{
		return new RecoverBreathChore(smi.master);
	}

	// Token: 0x06003F10 RID: 16144 RVA: 0x0015FE5E File Offset: 0x0015E05E
	private static bool IsNotFullBreath(BreathMonitor.Instance smi)
	{
		return !BreathMonitor.IsFullBreath(smi);
	}

	// Token: 0x06003F11 RID: 16145 RVA: 0x0015FE69 File Offset: 0x0015E069
	private static bool IsFullBreath(BreathMonitor.Instance smi)
	{
		return smi.breath.value >= smi.breath.GetMax();
	}

	// Token: 0x06003F12 RID: 16146 RVA: 0x0015FE86 File Offset: 0x0015E086
	private static bool IsNotInBreathableArea(BreathMonitor.Instance smi)
	{
		return !smi.breather.IsBreathableElementAtCell(Grid.PosToCell(smi), null);
	}

	// Token: 0x06003F13 RID: 16147 RVA: 0x0015FE9D File Offset: 0x0015E09D
	private static void ShowBreathBar(BreathMonitor.Instance smi)
	{
		if (NameDisplayScreen.Instance != null)
		{
			NameDisplayScreen.Instance.SetBreathDisplay(smi.gameObject, new Func<float>(smi.GetBreath), true);
		}
	}

	// Token: 0x06003F14 RID: 16148 RVA: 0x0015FEC9 File Offset: 0x0015E0C9
	private static void HideBreathBar(BreathMonitor.Instance smi)
	{
		if (NameDisplayScreen.Instance != null)
		{
			NameDisplayScreen.Instance.SetBreathDisplay(smi.gameObject, null, false);
		}
	}

	// Token: 0x06003F15 RID: 16149 RVA: 0x0015FEEA File Offset: 0x0015E0EA
	private static bool IsValidRecoverCell(BreathMonitor.Instance smi, int cell)
	{
		return cell != Grid.InvalidCell;
	}

	// Token: 0x06003F16 RID: 16150 RVA: 0x0015FEF7 File Offset: 0x0015E0F7
	private static bool IsNotValidRecoverCell(BreathMonitor.Instance smi, int cell)
	{
		return !BreathMonitor.IsValidRecoverCell(smi, cell);
	}

	// Token: 0x06003F17 RID: 16151 RVA: 0x0015FF03 File Offset: 0x0015E103
	private static void UpdateRecoverBreathCell(BreathMonitor.Instance smi, float dt)
	{
		BreathMonitor.UpdateRecoverBreathCell(smi);
	}

	// Token: 0x06003F18 RID: 16152 RVA: 0x0015FF0C File Offset: 0x0015E10C
	private static void UpdateRecoverBreathCell(BreathMonitor.Instance smi)
	{
		smi.query.Reset();
		smi.navigator.RunQuery(smi.query);
		int num = smi.query.GetResultCell();
		if (!smi.breather.IsBreathableElementAtCell(num, null))
		{
			num = PathFinder.InvalidCell;
		}
		smi.sm.recoverBreathCell.Set(num, smi, false);
	}

	// Token: 0x040028D2 RID: 10450
	public BreathMonitor.SatisfiedState satisfied;

	// Token: 0x040028D3 RID: 10451
	public BreathMonitor.LowBreathState lowbreath;

	// Token: 0x040028D4 RID: 10452
	public StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.IntParameter recoverBreathCell;

	// Token: 0x02001649 RID: 5705
	public class LowBreathState : GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006B30 RID: 27440
		public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State nowheretorecover;

		// Token: 0x04006B31 RID: 27441
		public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State recoveryavailable;
	}

	// Token: 0x0200164A RID: 5706
	public class SatisfiedState : GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006B32 RID: 27442
		public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State full;

		// Token: 0x04006B33 RID: 27443
		public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State notfull;
	}

	// Token: 0x0200164B RID: 5707
	public new class Instance : GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008A36 RID: 35382 RVA: 0x003135E0 File Offset: 0x003117E0
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
			this.query = new SafetyQuery(Game.Instance.safetyConditions.RecoverBreathChecker, base.GetComponent<KMonoBehaviour>(), int.MaxValue);
			this.navigator = base.GetComponent<Navigator>();
			this.breather = base.GetComponent<OxygenBreather>();
		}

		// Token: 0x06008A37 RID: 35383 RVA: 0x00313651 File Offset: 0x00311851
		public int GetRecoverCell()
		{
			return base.sm.recoverBreathCell.Get(base.smi);
		}

		// Token: 0x06008A38 RID: 35384 RVA: 0x00313669 File Offset: 0x00311869
		public float GetBreath()
		{
			return this.breath.value / this.breath.GetMax();
		}

		// Token: 0x04006B34 RID: 27444
		public AmountInstance breath;

		// Token: 0x04006B35 RID: 27445
		public SafetyQuery query;

		// Token: 0x04006B36 RID: 27446
		public Navigator navigator;

		// Token: 0x04006B37 RID: 27447
		public OxygenBreather breather;
	}
}
