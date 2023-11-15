using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000493 RID: 1171
public class BeehiveCalorieMonitor : GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>
{
	// Token: 0x06001A75 RID: 6773 RVA: 0x0008D130 File Offset: 0x0008B330
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.EventHandler(GameHashes.CaloriesConsumed, delegate(BeehiveCalorieMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		}).ToggleBehaviour(GameTags.Creatures.Poop, new StateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.Transition.ConditionCallback(BeehiveCalorieMonitor.ReadyToPoop), delegate(BeehiveCalorieMonitor.Instance smi)
		{
			smi.Poop();
		}).Update(new Action<BeehiveCalorieMonitor.Instance, float>(BeehiveCalorieMonitor.UpdateMetabolismCalorieModifier), UpdateRate.SIM_200ms, false);
		this.normal.Transition(this.hungry, (BeehiveCalorieMonitor.Instance smi) => smi.IsHungry(), UpdateRate.SIM_1000ms);
		this.hungry.ToggleTag(GameTags.Creatures.Hungry).EventTransition(GameHashes.CaloriesConsumed, this.normal, (BeehiveCalorieMonitor.Instance smi) => !smi.IsHungry()).ToggleStatusItem(Db.Get().CreatureStatusItems.HiveHungry, null).Transition(this.normal, (BeehiveCalorieMonitor.Instance smi) => !smi.IsHungry(), UpdateRate.SIM_1000ms);
	}

	// Token: 0x06001A76 RID: 6774 RVA: 0x0008D276 File Offset: 0x0008B476
	private static bool ReadyToPoop(BeehiveCalorieMonitor.Instance smi)
	{
		return smi.stomach.IsReadyToPoop() && Time.time - smi.lastMealOrPoopTime >= smi.def.minimumTimeBeforePooping;
	}

	// Token: 0x06001A77 RID: 6775 RVA: 0x0008D2A3 File Offset: 0x0008B4A3
	private static void UpdateMetabolismCalorieModifier(BeehiveCalorieMonitor.Instance smi, float dt)
	{
		smi.deltaCalorieMetabolismModifier.SetValue(1f - smi.metabolism.GetTotalValue() / 100f);
	}

	// Token: 0x04000EB3 RID: 3763
	public GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.State normal;

	// Token: 0x04000EB4 RID: 3764
	public GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.State hungry;

	// Token: 0x02001118 RID: 4376
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x0600783B RID: 30779 RVA: 0x002D5B70 File Offset: 0x002D3D70
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Calories.Id);
		}

		// Token: 0x0600783C RID: 30780 RVA: 0x002D5B98 File Offset: 0x002D3D98
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			List<Descriptor> list = new List<Descriptor>();
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_HEADER, UI.BUILDINGEFFECTS.TOOLTIPS.DIET_HEADER, Descriptor.DescriptorType.Effect, false));
			float calorie_loss_per_second = 0f;
			foreach (AttributeModifier attributeModifier in Db.Get().traits.Get(obj.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
			{
				if (attributeModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
				{
					calorie_loss_per_second = attributeModifier.Value;
				}
			}
			string newValue = string.Join(", ", (from t in this.diet.consumedTags
			select t.Key.ProperName()).ToArray<string>());
			string newValue2 = string.Join("\n", (from t in this.diet.consumedTags
			select UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(-calorie_loss_per_second / t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"))).ToArray<string>());
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue2), Descriptor.DescriptorType.Effect, false));
			string newValue3 = string.Join(", ", (from t in this.diet.producedTags
			select t.Key.ProperName()).ToArray<string>());
			string newValue4 = string.Join("\n", (from t in this.diet.producedTags
			select UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM.text.Replace("{Item}", t.Key.ProperName()).Replace("{Percent}", GameUtil.GetFormattedPercent(t.Value * 100f, GameUtil.TimeSlice.None))).ToArray<string>());
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_PRODUCED.text.Replace("{Items}", newValue3), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_PRODUCED.text.Replace("{Items}", newValue4), Descriptor.DescriptorType.Effect, false));
			return list;
		}

		// Token: 0x04005B3C RID: 23356
		public Diet diet;

		// Token: 0x04005B3D RID: 23357
		public float minPoopSizeInCalories = 100f;

		// Token: 0x04005B3E RID: 23358
		public float minimumTimeBeforePooping = 10f;

		// Token: 0x04005B3F RID: 23359
		public bool storePoop = true;
	}

	// Token: 0x02001119 RID: 4377
	public new class Instance : GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.GameInstance
	{
		// Token: 0x0600783E RID: 30782 RVA: 0x002D5DF8 File Offset: 0x002D3FF8
		public Instance(IStateMachineTarget master, BeehiveCalorieMonitor.Def def) : base(master, def)
		{
			this.calories = Db.Get().Amounts.Calories.Lookup(base.gameObject);
			this.calories.value = this.calories.GetMax() * 0.9f;
			this.stomach = new CreatureCalorieMonitor.Stomach(def.diet, master.gameObject, def.minPoopSizeInCalories, def.storePoop);
			this.metabolism = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Metabolism);
			this.deltaCalorieMetabolismModifier = new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, 1f, DUPLICANTS.MODIFIERS.METABOLISM_CALORIE_MODIFIER.NAME, true, false, false);
			this.calories.deltaAttribute.Add(this.deltaCalorieMetabolismModifier);
		}

		// Token: 0x0600783F RID: 30783 RVA: 0x002D5EE0 File Offset: 0x002D40E0
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			this.calories.value += caloriesConsumedEvent.calories;
			this.stomach.Consume(caloriesConsumedEvent.tag, caloriesConsumedEvent.calories);
			this.lastMealOrPoopTime = Time.time;
		}

		// Token: 0x06007840 RID: 30784 RVA: 0x002D5F2E File Offset: 0x002D412E
		public void Poop()
		{
			this.lastMealOrPoopTime = Time.time;
			this.stomach.Poop();
		}

		// Token: 0x06007841 RID: 30785 RVA: 0x002D5F46 File Offset: 0x002D4146
		public float GetCalories0to1()
		{
			return this.calories.value / this.calories.GetMax();
		}

		// Token: 0x06007842 RID: 30786 RVA: 0x002D5F5F File Offset: 0x002D415F
		public bool IsHungry()
		{
			return this.GetCalories0to1() < 0.9f;
		}

		// Token: 0x04005B40 RID: 23360
		public const float HUNGRY_RATIO = 0.9f;

		// Token: 0x04005B41 RID: 23361
		public AmountInstance calories;

		// Token: 0x04005B42 RID: 23362
		[Serialize]
		public CreatureCalorieMonitor.Stomach stomach;

		// Token: 0x04005B43 RID: 23363
		public float lastMealOrPoopTime;

		// Token: 0x04005B44 RID: 23364
		public AttributeInstance metabolism;

		// Token: 0x04005B45 RID: 23365
		public AttributeModifier deltaCalorieMetabolismModifier;
	}
}
