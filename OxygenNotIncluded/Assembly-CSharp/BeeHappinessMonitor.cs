using System;
using Klei.AI;
using STRINGS;

// Token: 0x0200070A RID: 1802
public class BeeHappinessMonitor : GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>
{
	// Token: 0x0600318A RID: 12682 RVA: 0x001077E8 File Offset: 0x001059E8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.happy, new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsHappy), UpdateRate.SIM_1000ms).Transition(this.unhappy, GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Not(new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsHappy)), UpdateRate.SIM_1000ms);
		this.happy.ToggleEffect((BeeHappinessMonitor.Instance smi) => this.happyEffect).TriggerOnEnter(GameHashes.Happy, null).Transition(this.satisfied, GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Not(new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsHappy)), UpdateRate.SIM_1000ms);
		this.unhappy.TriggerOnEnter(GameHashes.Unhappy, null).Transition(this.satisfied, new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsHappy), UpdateRate.SIM_1000ms).ToggleEffect((BeeHappinessMonitor.Instance smi) => this.unhappyEffect);
		this.happyEffect = new Effect("Happy", CREATURES.MODIFIERS.HAPPY.NAME, CREATURES.MODIFIERS.HAPPY.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.unhappyEffect = new Effect("Unhappy", CREATURES.MODIFIERS.UNHAPPY.NAME, CREATURES.MODIFIERS.UNHAPPY.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
	}

	// Token: 0x0600318B RID: 12683 RVA: 0x0010792E File Offset: 0x00105B2E
	private static bool IsHappy(BeeHappinessMonitor.Instance smi)
	{
		return smi.happiness.GetTotalValue() >= smi.def.threshold;
	}

	// Token: 0x04001DAF RID: 7599
	private GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.State satisfied;

	// Token: 0x04001DB0 RID: 7600
	private GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.State happy;

	// Token: 0x04001DB1 RID: 7601
	private GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.State unhappy;

	// Token: 0x04001DB2 RID: 7602
	private Effect happyEffect;

	// Token: 0x04001DB3 RID: 7603
	private Effect unhappyEffect;

	// Token: 0x02001463 RID: 5219
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006554 RID: 25940
		public float threshold;
	}

	// Token: 0x02001464 RID: 5220
	public new class Instance : GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.GameInstance
	{
		// Token: 0x06008480 RID: 33920 RVA: 0x00302B8B File Offset: 0x00300D8B
		public Instance(IStateMachineTarget master, BeeHappinessMonitor.Def def) : base(master, def)
		{
			this.happiness = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Happiness);
		}

		// Token: 0x04006555 RID: 25941
		public AttributeInstance happiness;
	}
}
