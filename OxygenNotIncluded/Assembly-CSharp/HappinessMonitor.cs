using System;
using Klei.AI;
using STRINGS;

// Token: 0x02000725 RID: 1829
public class HappinessMonitor : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>
{
	// Token: 0x06003249 RID: 12873 RVA: 0x0010AE44 File Offset: 0x00109044
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.happy, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsHappy), UpdateRate.SIM_1000ms).Transition(this.unhappy, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsHappy)), UpdateRate.SIM_1000ms);
		this.happy.DefaultState(this.happy.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsHappy)), UpdateRate.SIM_1000ms);
		this.happy.wild.ToggleEffect((HappinessMonitor.Instance smi) => this.happyWildEffect).TagTransition(GameTags.Creatures.Wild, this.happy.tame, true);
		this.happy.tame.ToggleEffect((HappinessMonitor.Instance smi) => this.happyTameEffect).TagTransition(GameTags.Creatures.Wild, this.happy.wild, false);
		this.unhappy.DefaultState(this.unhappy.wild).Transition(this.satisfied, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsHappy), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Unhappy);
		this.unhappy.wild.ToggleEffect((HappinessMonitor.Instance smi) => this.unhappyWildEffect).TagTransition(GameTags.Creatures.Wild, this.unhappy.tame, true);
		this.unhappy.tame.ToggleEffect((HappinessMonitor.Instance smi) => this.unhappyTameEffect).TagTransition(GameTags.Creatures.Wild, this.unhappy.wild, false);
		this.happyWildEffect = new Effect("Happy", CREATURES.MODIFIERS.HAPPY.NAME, CREATURES.MODIFIERS.HAPPY.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.happyTameEffect = new Effect("Happy", CREATURES.MODIFIERS.HAPPY.NAME, CREATURES.MODIFIERS.HAPPY.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.unhappyWildEffect = new Effect("Unhappy", CREATURES.MODIFIERS.UNHAPPY.NAME, CREATURES.MODIFIERS.UNHAPPY.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.unhappyTameEffect = new Effect("Unhappy", CREATURES.MODIFIERS.UNHAPPY.NAME, CREATURES.MODIFIERS.UNHAPPY.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.happyTameEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, 9f, CREATURES.MODIFIERS.HAPPY.NAME, true, false, true));
		this.unhappyWildEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -15f, CREATURES.MODIFIERS.UNHAPPY.NAME, false, false, true));
		this.unhappyTameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -80f, CREATURES.MODIFIERS.UNHAPPY.NAME, false, false, true));
	}

	// Token: 0x0600324A RID: 12874 RVA: 0x0010B169 File Offset: 0x00109369
	private static bool IsHappy(HappinessMonitor.Instance smi)
	{
		return smi.happiness.GetTotalValue() >= smi.def.threshold;
	}

	// Token: 0x04001E2A RID: 7722
	private GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State satisfied;

	// Token: 0x04001E2B RID: 7723
	private HappinessMonitor.HappyState happy;

	// Token: 0x04001E2C RID: 7724
	private HappinessMonitor.UnhappyState unhappy;

	// Token: 0x04001E2D RID: 7725
	private Effect happyWildEffect;

	// Token: 0x04001E2E RID: 7726
	private Effect happyTameEffect;

	// Token: 0x04001E2F RID: 7727
	private Effect unhappyWildEffect;

	// Token: 0x04001E30 RID: 7728
	private Effect unhappyTameEffect;

	// Token: 0x020014A2 RID: 5282
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040065FC RID: 26108
		public float threshold;
	}

	// Token: 0x020014A3 RID: 5283
	public class UnhappyState : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
	{
		// Token: 0x040065FD RID: 26109
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;

		// Token: 0x040065FE RID: 26110
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
	}

	// Token: 0x020014A4 RID: 5284
	public class HappyState : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
	{
		// Token: 0x040065FF RID: 26111
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;

		// Token: 0x04006600 RID: 26112
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
	}

	// Token: 0x020014A5 RID: 5285
	public new class Instance : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.GameInstance
	{
		// Token: 0x06008567 RID: 34151 RVA: 0x00305A43 File Offset: 0x00303C43
		public Instance(IStateMachineTarget master, HappinessMonitor.Def def) : base(master, def)
		{
			this.happiness = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Happiness);
		}

		// Token: 0x04006601 RID: 26113
		public AttributeInstance happiness;
	}
}
