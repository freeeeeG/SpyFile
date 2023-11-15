using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200073E RID: 1854
public class WildnessMonitor : GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>
{
	// Token: 0x060032F9 RID: 13049 RVA: 0x0010E600 File Offset: 0x0010C800
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.tame;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.wild.Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.RefreshAmounts)).Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.HideDomesticationSymbol)).Transition(this.tame, (WildnessMonitor.Instance smi) => !WildnessMonitor.IsWild(smi), UpdateRate.SIM_1000ms).ToggleEffect((WildnessMonitor.Instance smi) => smi.def.wildEffect).ToggleTag(GameTags.Creatures.Wild);
		this.tame.Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.RefreshAmounts)).Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.ShowDomesticationSymbol)).Transition(this.wild, new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.Transition.ConditionCallback(WildnessMonitor.IsWild), UpdateRate.SIM_1000ms).ToggleEffect((WildnessMonitor.Instance smi) => smi.def.tameEffect).Enter(delegate(WildnessMonitor.Instance smi)
		{
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().LogCritterTamed(smi.PrefabID());
		});
	}

	// Token: 0x060032FA RID: 13050 RVA: 0x0010E728 File Offset: 0x0010C928
	private static void HideDomesticationSymbol(WildnessMonitor.Instance smi)
	{
		foreach (KAnimHashedString symbol in WildnessMonitor.DOMESTICATION_SYMBOLS)
		{
			smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(symbol, false);
		}
	}

	// Token: 0x060032FB RID: 13051 RVA: 0x0010E760 File Offset: 0x0010C960
	private static void ShowDomesticationSymbol(WildnessMonitor.Instance smi)
	{
		foreach (KAnimHashedString symbol in WildnessMonitor.DOMESTICATION_SYMBOLS)
		{
			smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(symbol, true);
		}
	}

	// Token: 0x060032FC RID: 13052 RVA: 0x0010E796 File Offset: 0x0010C996
	private static bool IsWild(WildnessMonitor.Instance smi)
	{
		return smi.wildness.value > 0f;
	}

	// Token: 0x060032FD RID: 13053 RVA: 0x0010E7AC File Offset: 0x0010C9AC
	private static void RefreshAmounts(WildnessMonitor.Instance smi)
	{
		bool flag = WildnessMonitor.IsWild(smi);
		smi.wildness.hide = !flag;
		AttributeInstance attributeInstance = Db.Get().CritterAttributes.Happiness.Lookup(smi.gameObject);
		if (attributeInstance != null)
		{
			attributeInstance.hide = flag;
		}
		AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(smi.gameObject);
		if (amountInstance != null)
		{
			amountInstance.hide = flag;
		}
		AmountInstance amountInstance2 = Db.Get().Amounts.Temperature.Lookup(smi.gameObject);
		if (amountInstance2 != null)
		{
			amountInstance2.hide = flag;
		}
		AmountInstance amountInstance3 = Db.Get().Amounts.Fertility.Lookup(smi.gameObject);
		if (amountInstance3 != null)
		{
			amountInstance3.hide = flag;
		}
		AmountInstance amountInstance4 = Db.Get().Amounts.MilkProduction.Lookup(smi.gameObject);
		if (amountInstance4 != null)
		{
			amountInstance4.hide = flag;
		}
		AmountInstance amountInstance5 = Db.Get().Amounts.Beckoning.Lookup(smi.gameObject);
		if (amountInstance5 != null)
		{
			amountInstance5.hide = flag;
		}
	}

	// Token: 0x04001E98 RID: 7832
	public GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State wild;

	// Token: 0x04001E99 RID: 7833
	public GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State tame;

	// Token: 0x04001E9A RID: 7834
	private static readonly KAnimHashedString[] DOMESTICATION_SYMBOLS = new KAnimHashedString[]
	{
		"tag",
		"snapto_tag"
	};

	// Token: 0x020014D9 RID: 5337
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06008601 RID: 34305 RVA: 0x00307DEF File Offset: 0x00305FEF
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Wildness.Id);
		}

		// Token: 0x04006693 RID: 26259
		public Effect wildEffect;

		// Token: 0x04006694 RID: 26260
		public Effect tameEffect;
	}

	// Token: 0x020014DA RID: 5338
	public new class Instance : GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.GameInstance
	{
		// Token: 0x06008603 RID: 34307 RVA: 0x00307E1D File Offset: 0x0030601D
		public Instance(IStateMachineTarget master, WildnessMonitor.Def def) : base(master, def)
		{
			this.wildness = Db.Get().Amounts.Wildness.Lookup(base.gameObject);
			this.wildness.value = this.wildness.GetMax();
		}

		// Token: 0x04006695 RID: 26261
		public AmountInstance wildness;
	}
}
