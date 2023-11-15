using System;
using Klei.AI;

// Token: 0x0200093B RID: 2363
public class RobotBatteryMonitor : GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>
{
	// Token: 0x060044AC RID: 17580 RVA: 0x0018256C File Offset: 0x0018076C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.drainingStates;
		this.drainingStates.DefaultState(this.drainingStates.highBattery).Transition(this.deadBattery, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.BatteryDead), UpdateRate.SIM_200ms).Transition(this.needsRechargeStates, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.NeedsRecharge), UpdateRate.SIM_200ms);
		this.drainingStates.highBattery.Transition(this.drainingStates.lowBattery, GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Not(new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeDecent)), UpdateRate.SIM_200ms);
		this.drainingStates.lowBattery.Transition(this.drainingStates.highBattery, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeDecent), UpdateRate.SIM_200ms).ToggleStatusItem(delegate(RobotBatteryMonitor.Instance smi)
		{
			if (!smi.def.canCharge)
			{
				return Db.Get().RobotStatusItems.LowBatteryNoCharge;
			}
			return Db.Get().RobotStatusItems.LowBattery;
		}, (RobotBatteryMonitor.Instance smi) => smi.gameObject);
		this.needsRechargeStates.DefaultState(this.needsRechargeStates.lowBattery).Transition(this.deadBattery, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.BatteryDead), UpdateRate.SIM_200ms).Transition(this.drainingStates, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeComplete), UpdateRate.SIM_200ms).ToggleBehaviour(GameTags.Robots.Behaviours.RechargeBehaviour, (RobotBatteryMonitor.Instance smi) => smi.def.canCharge, null);
		this.needsRechargeStates.lowBattery.ToggleStatusItem(delegate(RobotBatteryMonitor.Instance smi)
		{
			if (!smi.def.canCharge)
			{
				return Db.Get().RobotStatusItems.LowBatteryNoCharge;
			}
			return Db.Get().RobotStatusItems.LowBattery;
		}, (RobotBatteryMonitor.Instance smi) => smi.gameObject).Transition(this.needsRechargeStates.mediumBattery, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeDecent), UpdateRate.SIM_200ms);
		this.needsRechargeStates.mediumBattery.Transition(this.needsRechargeStates.lowBattery, GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Not(new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeDecent)), UpdateRate.SIM_200ms).Transition(this.needsRechargeStates.trickleCharge, new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeFull), UpdateRate.SIM_200ms);
		this.needsRechargeStates.trickleCharge.Transition(this.needsRechargeStates.mediumBattery, GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Not(new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.Transition.ConditionCallback(RobotBatteryMonitor.ChargeFull)), UpdateRate.SIM_200ms);
		this.deadBattery.ToggleStatusItem(Db.Get().RobotStatusItems.DeadBattery, (RobotBatteryMonitor.Instance smi) => smi.gameObject).Enter(delegate(RobotBatteryMonitor.Instance smi)
		{
			if (smi.GetSMI<DeathMonitor.Instance>() != null)
			{
				smi.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.DeadBattery);
			}
		});
	}

	// Token: 0x060044AD RID: 17581 RVA: 0x00182816 File Offset: 0x00180A16
	public static bool NeedsRecharge(RobotBatteryMonitor.Instance smi)
	{
		return smi.amountInstance.value <= 0f || GameClock.Instance.IsNighttime();
	}

	// Token: 0x060044AE RID: 17582 RVA: 0x00182836 File Offset: 0x00180A36
	public static bool ChargeDecent(RobotBatteryMonitor.Instance smi)
	{
		return smi.amountInstance.value >= smi.amountInstance.GetMax() * smi.def.lowBatteryWarningPercent;
	}

	// Token: 0x060044AF RID: 17583 RVA: 0x0018285F File Offset: 0x00180A5F
	public static bool ChargeFull(RobotBatteryMonitor.Instance smi)
	{
		return smi.amountInstance.value >= smi.amountInstance.GetMax();
	}

	// Token: 0x060044B0 RID: 17584 RVA: 0x0018287C File Offset: 0x00180A7C
	public static bool ChargeComplete(RobotBatteryMonitor.Instance smi)
	{
		return smi.amountInstance.value >= smi.amountInstance.GetMax() && !GameClock.Instance.IsNighttime();
	}

	// Token: 0x060044B1 RID: 17585 RVA: 0x001828A5 File Offset: 0x00180AA5
	public static bool BatteryDead(RobotBatteryMonitor.Instance smi)
	{
		return !smi.def.canCharge && smi.amountInstance.value == 0f;
	}

	// Token: 0x04002D76 RID: 11638
	public StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.ObjectParameter<Storage> internalStorage = new StateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.ObjectParameter<Storage>();

	// Token: 0x04002D77 RID: 11639
	public RobotBatteryMonitor.NeedsRechargeStates needsRechargeStates;

	// Token: 0x04002D78 RID: 11640
	public RobotBatteryMonitor.DrainingStates drainingStates;

	// Token: 0x04002D79 RID: 11641
	public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State deadBattery;

	// Token: 0x0200177E RID: 6014
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006EFF RID: 28415
		public string batteryAmountId;

		// Token: 0x04006F00 RID: 28416
		public float lowBatteryWarningPercent;

		// Token: 0x04006F01 RID: 28417
		public bool canCharge;
	}

	// Token: 0x0200177F RID: 6015
	public class DrainingStates : GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State
	{
		// Token: 0x04006F02 RID: 28418
		public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State highBattery;

		// Token: 0x04006F03 RID: 28419
		public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State lowBattery;
	}

	// Token: 0x02001780 RID: 6016
	public class NeedsRechargeStates : GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State
	{
		// Token: 0x04006F04 RID: 28420
		public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State lowBattery;

		// Token: 0x04006F05 RID: 28421
		public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State mediumBattery;

		// Token: 0x04006F06 RID: 28422
		public GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.State trickleCharge;
	}

	// Token: 0x02001781 RID: 6017
	public new class Instance : GameStateMachine<RobotBatteryMonitor, RobotBatteryMonitor.Instance, IStateMachineTarget, RobotBatteryMonitor.Def>.GameInstance
	{
		// Token: 0x06008E7A RID: 36474 RVA: 0x0031F824 File Offset: 0x0031DA24
		public Instance(IStateMachineTarget master, RobotBatteryMonitor.Def def) : base(master, def)
		{
			this.amountInstance = Db.Get().Amounts.Get(def.batteryAmountId).Lookup(base.gameObject);
			this.amountInstance.SetValue(this.amountInstance.GetMax());
		}

		// Token: 0x04006F07 RID: 28423
		public AmountInstance amountInstance;
	}
}
