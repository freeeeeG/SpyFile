using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000736 RID: 1846
public class ScaleGrowthMonitor : GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>
{
	// Token: 0x060032B2 RID: 12978 RVA: 0x0010D2F0 File Offset: 0x0010B4F0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		this.root.Enter(delegate(ScaleGrowthMonitor.Instance smi)
		{
			ScaleGrowthMonitor.UpdateScales(smi, 0f);
		}).Update(new Action<ScaleGrowthMonitor.Instance, float>(ScaleGrowthMonitor.UpdateScales), UpdateRate.SIM_1000ms, false);
		this.growing.DefaultState(this.growing.growing).Transition(this.fullyGrown, new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.AreScalesFullyGrown), UpdateRate.SIM_1000ms);
		this.growing.growing.Transition(this.growing.stunted, GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Not(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.IsInCorrectAtmosphere)), UpdateRate.SIM_1000ms).Enter(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback(ScaleGrowthMonitor.ApplyModifier)).Exit(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback(ScaleGrowthMonitor.RemoveModifier));
		this.growing.stunted.Transition(this.growing.growing, new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.IsInCorrectAtmosphere), UpdateRate.SIM_1000ms).ToggleStatusItem(CREATURES.STATUSITEMS.STUNTED_SCALE_GROWTH.NAME, CREATURES.STATUSITEMS.STUNTED_SCALE_GROWTH.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.fullyGrown.ToggleBehaviour(GameTags.Creatures.ScalesGrown, (ScaleGrowthMonitor.Instance smi) => smi.HasTag(GameTags.Creatures.CanMolt), null).Transition(this.growing, GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Not(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.AreScalesFullyGrown)), UpdateRate.SIM_1000ms);
	}

	// Token: 0x060032B3 RID: 12979 RVA: 0x0010D47C File Offset: 0x0010B67C
	private static bool IsInCorrectAtmosphere(ScaleGrowthMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi);
		return Grid.Element[num].id == smi.def.targetAtmosphere;
	}

	// Token: 0x060032B4 RID: 12980 RVA: 0x0010D4A9 File Offset: 0x0010B6A9
	private static bool AreScalesFullyGrown(ScaleGrowthMonitor.Instance smi)
	{
		return smi.scaleGrowth.value >= smi.scaleGrowth.GetMax();
	}

	// Token: 0x060032B5 RID: 12981 RVA: 0x0010D4C6 File Offset: 0x0010B6C6
	private static void ApplyModifier(ScaleGrowthMonitor.Instance smi)
	{
		smi.scaleGrowth.deltaAttribute.Add(smi.scaleGrowthModifier);
	}

	// Token: 0x060032B6 RID: 12982 RVA: 0x0010D4DE File Offset: 0x0010B6DE
	private static void RemoveModifier(ScaleGrowthMonitor.Instance smi)
	{
		smi.scaleGrowth.deltaAttribute.Remove(smi.scaleGrowthModifier);
	}

	// Token: 0x060032B7 RID: 12983 RVA: 0x0010D4F8 File Offset: 0x0010B6F8
	private static void UpdateScales(ScaleGrowthMonitor.Instance smi, float dt)
	{
		int num = (int)((float)smi.def.levelCount * smi.scaleGrowth.value / 100f);
		if (smi.currentScaleLevel != num)
		{
			KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < ScaleGrowthMonitor.SCALE_SYMBOL_NAMES.Length; i++)
			{
				bool is_visible = i <= num - 1;
				component.SetSymbolVisiblity(ScaleGrowthMonitor.SCALE_SYMBOL_NAMES[i], is_visible);
			}
			smi.currentScaleLevel = num;
		}
	}

	// Token: 0x04001E70 RID: 7792
	public ScaleGrowthMonitor.GrowingState growing;

	// Token: 0x04001E71 RID: 7793
	public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State fullyGrown;

	// Token: 0x04001E72 RID: 7794
	private AttributeModifier scaleGrowthModifier;

	// Token: 0x04001E73 RID: 7795
	private static HashedString[] SCALE_SYMBOL_NAMES = new HashedString[]
	{
		"scale_0",
		"scale_1",
		"scale_2",
		"scale_3",
		"scale_4"
	};

	// Token: 0x020014CA RID: 5322
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x060085DD RID: 34269 RVA: 0x003075DE File Offset: 0x003057DE
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ScaleGrowth.Id);
		}

		// Token: 0x060085DE RID: 34270 RVA: 0x00307604 File Offset: 0x00305804
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			List<Descriptor> list = new List<Descriptor>();
			if (this.targetAtmosphere == (SimHashes)0)
			{
				list.Add(new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)), Descriptor.DescriptorType.Effect, false));
			}
			else
			{
				list.Add(new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH_ATMO.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)).Replace("{Atmosphere}", this.targetAtmosphere.CreateTag().ProperName()), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_ATMO.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)).Replace("{Atmosphere}", this.targetAtmosphere.CreateTag().ProperName()), Descriptor.DescriptorType.Effect, false));
			}
			return list;
		}

		// Token: 0x04006666 RID: 26214
		public int levelCount;

		// Token: 0x04006667 RID: 26215
		public float defaultGrowthRate;

		// Token: 0x04006668 RID: 26216
		public SimHashes targetAtmosphere;

		// Token: 0x04006669 RID: 26217
		public Tag itemDroppedOnShear;

		// Token: 0x0400666A RID: 26218
		public float dropMass;
	}

	// Token: 0x020014CB RID: 5323
	public class GrowingState : GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State
	{
		// Token: 0x0400666B RID: 26219
		public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State growing;

		// Token: 0x0400666C RID: 26220
		public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State stunted;
	}

	// Token: 0x020014CC RID: 5324
	public new class Instance : GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.GameInstance, IShearable
	{
		// Token: 0x060085E1 RID: 34273 RVA: 0x003077E8 File Offset: 0x003059E8
		public Instance(IStateMachineTarget master, ScaleGrowthMonitor.Def def) : base(master, def)
		{
			this.scaleGrowth = Db.Get().Amounts.ScaleGrowth.Lookup(base.gameObject);
			this.scaleGrowth.value = this.scaleGrowth.GetMax();
			this.scaleGrowthModifier = new AttributeModifier(this.scaleGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 100f, CREATURES.MODIFIERS.SCALE_GROWTH_RATE.NAME, false, false, true);
		}

		// Token: 0x060085E2 RID: 34274 RVA: 0x00307873 File Offset: 0x00305A73
		public bool IsFullyGrown()
		{
			return this.currentScaleLevel == base.def.levelCount;
		}

		// Token: 0x060085E3 RID: 34275 RVA: 0x00307888 File Offset: 0x00305A88
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
			this.scaleGrowth.value = 0f;
			ScaleGrowthMonitor.UpdateScales(this, 0f);
		}

		// Token: 0x0400666D RID: 26221
		public AmountInstance scaleGrowth;

		// Token: 0x0400666E RID: 26222
		public AttributeModifier scaleGrowthModifier;

		// Token: 0x0400666F RID: 26223
		public int currentScaleLevel = -1;
	}
}
