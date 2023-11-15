using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200071D RID: 1821
public class ElementGrowthMonitor : GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>
{
	// Token: 0x060031FF RID: 12799 RVA: 0x00109808 File Offset: 0x00107A08
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		this.root.Enter(delegate(ElementGrowthMonitor.Instance smi)
		{
			ElementGrowthMonitor.UpdateGrowth(smi, 0f);
		}).Update(new Action<ElementGrowthMonitor.Instance, float>(ElementGrowthMonitor.UpdateGrowth), UpdateRate.SIM_1000ms, false).EventHandler(GameHashes.EatSolidComplete, delegate(ElementGrowthMonitor.Instance smi, object data)
		{
			smi.OnEatSolidComplete(data);
		});
		this.growing.DefaultState(this.growing.growing).Transition(this.fullyGrown, new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Transition.ConditionCallback(ElementGrowthMonitor.IsFullyGrown), UpdateRate.SIM_1000ms).TagTransition(this.HungryTags, this.halted, false);
		this.growing.growing.Transition(this.growing.stunted, GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Not(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Transition.ConditionCallback(ElementGrowthMonitor.IsConsumedInTemperatureRange)), UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthGrowing, null).Enter(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State.Callback(ElementGrowthMonitor.ApplyModifier)).Exit(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State.Callback(ElementGrowthMonitor.RemoveModifier));
		this.growing.stunted.Transition(this.growing.growing, new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Transition.ConditionCallback(ElementGrowthMonitor.IsConsumedInTemperatureRange), UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthStunted, null).Enter(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State.Callback(ElementGrowthMonitor.ApplyModifier)).Exit(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State.Callback(ElementGrowthMonitor.RemoveModifier));
		this.halted.TagTransition(this.HungryTags, this.growing, true).ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthHalted, null);
		this.fullyGrown.ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthComplete, null).ToggleBehaviour(GameTags.Creatures.ScalesGrown, (ElementGrowthMonitor.Instance smi) => smi.HasTag(GameTags.Creatures.CanMolt), null).Transition(this.growing, GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Not(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Transition.ConditionCallback(ElementGrowthMonitor.IsFullyGrown)), UpdateRate.SIM_1000ms);
	}

	// Token: 0x06003200 RID: 12800 RVA: 0x00109A1B File Offset: 0x00107C1B
	private static bool IsConsumedInTemperatureRange(ElementGrowthMonitor.Instance smi)
	{
		return smi.lastConsumedTemperature == 0f || (smi.lastConsumedTemperature >= smi.def.minTemperature && smi.lastConsumedTemperature <= smi.def.maxTemperature);
	}

	// Token: 0x06003201 RID: 12801 RVA: 0x00109A57 File Offset: 0x00107C57
	private static bool IsFullyGrown(ElementGrowthMonitor.Instance smi)
	{
		return smi.elementGrowth.value >= smi.elementGrowth.GetMax();
	}

	// Token: 0x06003202 RID: 12802 RVA: 0x00109A74 File Offset: 0x00107C74
	private static void ApplyModifier(ElementGrowthMonitor.Instance smi)
	{
		if (smi.IsInsideState(smi.sm.growing.growing))
		{
			smi.elementGrowth.deltaAttribute.Add(smi.growingGrowthModifier);
			return;
		}
		if (smi.IsInsideState(smi.sm.growing.stunted))
		{
			smi.elementGrowth.deltaAttribute.Add(smi.stuntedGrowthModifier);
		}
	}

	// Token: 0x06003203 RID: 12803 RVA: 0x00109ADE File Offset: 0x00107CDE
	private static void RemoveModifier(ElementGrowthMonitor.Instance smi)
	{
		smi.elementGrowth.deltaAttribute.Remove(smi.growingGrowthModifier);
		smi.elementGrowth.deltaAttribute.Remove(smi.stuntedGrowthModifier);
	}

	// Token: 0x06003204 RID: 12804 RVA: 0x00109B0C File Offset: 0x00107D0C
	private static void UpdateGrowth(ElementGrowthMonitor.Instance smi, float dt)
	{
		int num = (int)((float)smi.def.levelCount * smi.elementGrowth.value / 100f);
		if (smi.currentGrowthLevel != num)
		{
			KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < ElementGrowthMonitor.GROWTH_SYMBOL_NAMES.Length; i++)
			{
				bool is_visible = i == num - 1;
				component.SetSymbolVisiblity(ElementGrowthMonitor.GROWTH_SYMBOL_NAMES[i], is_visible);
			}
			smi.currentGrowthLevel = num;
		}
	}

	// Token: 0x04001DF7 RID: 7671
	public Tag[] HungryTags = new Tag[]
	{
		GameTags.Creatures.Hungry
	};

	// Token: 0x04001DF8 RID: 7672
	public GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State halted;

	// Token: 0x04001DF9 RID: 7673
	public ElementGrowthMonitor.GrowingState growing;

	// Token: 0x04001DFA RID: 7674
	public GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State fullyGrown;

	// Token: 0x04001DFB RID: 7675
	private static HashedString[] GROWTH_SYMBOL_NAMES = new HashedString[]
	{
		"del_ginger1",
		"del_ginger2",
		"del_ginger3",
		"del_ginger4",
		"del_ginger5"
	};

	// Token: 0x0200148D RID: 5261
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x0600850C RID: 34060 RVA: 0x003042AA File Offset: 0x003024AA
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ElementGrowth.Id);
		}

		// Token: 0x0600850D RID: 34061 RVA: 0x003042D0 File Offset: 0x003024D0
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH_TEMP.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)).Replace("{TempMin}", GameUtil.GetFormattedTemperature(this.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)).Replace("{TempMax}", GameUtil.GetFormattedTemperature(this.maxTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_TEMP.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)).Replace("{TempMin}", GameUtil.GetFormattedTemperature(this.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)).Replace("{TempMax}", GameUtil.GetFormattedTemperature(this.maxTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false)
			};
		}

		// Token: 0x040065B9 RID: 26041
		public int levelCount;

		// Token: 0x040065BA RID: 26042
		public float defaultGrowthRate;

		// Token: 0x040065BB RID: 26043
		public Tag itemDroppedOnShear;

		// Token: 0x040065BC RID: 26044
		public float dropMass;

		// Token: 0x040065BD RID: 26045
		public float minTemperature;

		// Token: 0x040065BE RID: 26046
		public float maxTemperature;
	}

	// Token: 0x0200148E RID: 5262
	public class GrowingState : GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State
	{
		// Token: 0x040065BF RID: 26047
		public GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State growing;

		// Token: 0x040065C0 RID: 26048
		public GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State stunted;
	}

	// Token: 0x0200148F RID: 5263
	public new class Instance : GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.GameInstance, IShearable
	{
		// Token: 0x06008510 RID: 34064 RVA: 0x00304414 File Offset: 0x00302614
		public Instance(IStateMachineTarget master, ElementGrowthMonitor.Def def) : base(master, def)
		{
			this.elementGrowth = Db.Get().Amounts.ElementGrowth.Lookup(base.gameObject);
			this.elementGrowth.value = this.elementGrowth.GetMax();
			this.growingGrowthModifier = new AttributeModifier(this.elementGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 100f, CREATURES.MODIFIERS.ELEMENT_GROWTH_RATE.NAME, false, false, true);
			this.stuntedGrowthModifier = new AttributeModifier(this.elementGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 20f, CREATURES.MODIFIERS.ELEMENT_GROWTH_RATE.NAME, false, false, true);
		}

		// Token: 0x06008511 RID: 34065 RVA: 0x003044D8 File Offset: 0x003026D8
		public void OnEatSolidComplete(object data)
		{
			KPrefabID kprefabID = (KPrefabID)data;
			if (kprefabID == null)
			{
				return;
			}
			PrimaryElement component = kprefabID.GetComponent<PrimaryElement>();
			this.lastConsumedElement = component.ElementID;
			this.lastConsumedTemperature = component.Temperature;
		}

		// Token: 0x06008512 RID: 34066 RVA: 0x00304515 File Offset: 0x00302715
		public bool IsFullyGrown()
		{
			return this.currentGrowthLevel == base.def.levelCount;
		}

		// Token: 0x06008513 RID: 34067 RVA: 0x0030452C File Offset: 0x0030272C
		public void Shear()
		{
			PrimaryElement component = base.smi.GetComponent<PrimaryElement>();
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(base.def.itemDroppedOnShear), null, null);
			gameObject.transform.SetPosition(Grid.CellToPosCCC(Grid.CellLeft(Grid.PosToCell(this)), Grid.SceneLayer.Ore));
			PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
			component2.Temperature = component.Temperature;
			component2.Mass = base.def.dropMass;
			component2.AddDisease(component.DiseaseIdx, component.DiseaseCount, "Shearing");
			gameObject.SetActive(true);
			Vector2 initial_velocity = new Vector2(UnityEngine.Random.Range(-1f, 1f) * 1f, UnityEngine.Random.value * 2f + 2f);
			if (GameComps.Fallers.Has(gameObject))
			{
				GameComps.Fallers.Remove(gameObject);
			}
			GameComps.Fallers.Add(gameObject, initial_velocity);
			this.elementGrowth.value = 0f;
			ElementGrowthMonitor.UpdateGrowth(this, 0f);
		}

		// Token: 0x040065C1 RID: 26049
		public AmountInstance elementGrowth;

		// Token: 0x040065C2 RID: 26050
		public AttributeModifier growingGrowthModifier;

		// Token: 0x040065C3 RID: 26051
		public AttributeModifier stuntedGrowthModifier;

		// Token: 0x040065C4 RID: 26052
		public int currentGrowthLevel = -1;

		// Token: 0x040065C5 RID: 26053
		[Serialize]
		public SimHashes lastConsumedElement;

		// Token: 0x040065C6 RID: 26054
		[Serialize]
		public float lastConsumedTemperature;
	}
}
