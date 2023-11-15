using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000498 RID: 1176
public class CreatureCalorieMonitor : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>
{
	// Token: 0x06001A85 RID: 6789 RVA: 0x0008D770 File Offset: 0x0008B970
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.EventHandler(GameHashes.CaloriesConsumed, delegate(CreatureCalorieMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		}).ToggleBehaviour(GameTags.Creatures.Poop, new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback(CreatureCalorieMonitor.ReadyToPoop), delegate(CreatureCalorieMonitor.Instance smi)
		{
			smi.Poop();
		}).Update(new Action<CreatureCalorieMonitor.Instance, float>(CreatureCalorieMonitor.UpdateMetabolismCalorieModifier), UpdateRate.SIM_200ms, false);
		this.normal.TagTransition(GameTags.Creatures.PausedHunger, this.pause.commonPause, false).Transition(this.hungry, (CreatureCalorieMonitor.Instance smi) => smi.IsHungry(), UpdateRate.SIM_1000ms);
		this.hungry.DefaultState(this.hungry.hungry).ToggleTag(GameTags.Creatures.Hungry).EventTransition(GameHashes.CaloriesConsumed, this.normal, (CreatureCalorieMonitor.Instance smi) => !smi.IsHungry());
		this.hungry.hungry.TagTransition(GameTags.Creatures.PausedHunger, this.pause.commonPause, false).Transition(this.normal, (CreatureCalorieMonitor.Instance smi) => !smi.IsHungry(), UpdateRate.SIM_1000ms).Transition(this.hungry.outofcalories, (CreatureCalorieMonitor.Instance smi) => smi.IsOutOfCalories(), UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.Hungry, null);
		this.hungry.outofcalories.DefaultState(this.hungry.outofcalories.wild).Transition(this.hungry.hungry, (CreatureCalorieMonitor.Instance smi) => !smi.IsOutOfCalories(), UpdateRate.SIM_1000ms);
		this.hungry.outofcalories.wild.TagTransition(GameTags.Creatures.PausedHunger, this.pause.commonPause, false).TagTransition(GameTags.Creatures.Wild, this.hungry.outofcalories.tame, true).ToggleStatusItem(Db.Get().CreatureStatusItems.Hungry, null);
		this.hungry.outofcalories.tame.Enter("StarvationStartTime", new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State.Callback(CreatureCalorieMonitor.StarvationStartTime)).Exit("ClearStarvationTime", delegate(CreatureCalorieMonitor.Instance smi)
		{
			this.starvationStartTime.Set(Mathf.Min(-(GameClock.Instance.GetTime() - this.starvationStartTime.Get(smi)), 0f), smi, false);
		}).Transition(this.hungry.outofcalories.starvedtodeath, (CreatureCalorieMonitor.Instance smi) => smi.GetDeathTimeRemaining() <= 0f, UpdateRate.SIM_1000ms).TagTransition(GameTags.Creatures.PausedHunger, this.pause.starvingPause, false).TagTransition(GameTags.Creatures.Wild, this.hungry.outofcalories.wild, false).ToggleStatusItem(STRINGS.CREATURES.STATUSITEMS.STARVING.NAME, STRINGS.CREATURES.STATUSITEMS.STARVING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, default(HashedString), 129022, (string str, CreatureCalorieMonitor.Instance smi) => str.Replace("{TimeUntilDeath}", GameUtil.GetFormattedCycles(smi.GetDeathTimeRemaining(), "F1", false)), null, null).ToggleNotification((CreatureCalorieMonitor.Instance smi) => new Notification(STRINGS.CREATURES.STATUSITEMS.STARVING.NOTIFICATION_NAME, NotificationType.BadMinor, (List<Notification> notifications, object data) => STRINGS.CREATURES.STATUSITEMS.STARVING.NOTIFICATION_TOOLTIP + notifications.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false)).ToggleEffect((CreatureCalorieMonitor.Instance smi) => this.outOfCaloriesTame);
		this.hungry.outofcalories.starvedtodeath.Enter(delegate(CreatureCalorieMonitor.Instance smi)
		{
			smi.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Starvation);
		});
		this.pause.commonPause.TagTransition(GameTags.Creatures.PausedHunger, this.normal, true);
		this.pause.starvingPause.Exit("Recalculate StarvationStartTime", new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State.Callback(CreatureCalorieMonitor.RecalculateStartTimeOnUnpause)).TagTransition(GameTags.Creatures.PausedHunger, this.hungry.outofcalories.tame, true);
		this.outOfCaloriesTame = new Effect("OutOfCaloriesTame", STRINGS.CREATURES.MODIFIERS.OUT_OF_CALORIES.NAME, STRINGS.CREATURES.MODIFIERS.OUT_OF_CALORIES.TOOLTIP, 0f, false, false, false, null, -1f, 0f, null, "");
		this.outOfCaloriesTame.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -10f, STRINGS.CREATURES.MODIFIERS.OUT_OF_CALORIES.NAME, false, false, true));
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x0008DC00 File Offset: 0x0008BE00
	private static bool ReadyToPoop(CreatureCalorieMonitor.Instance smi)
	{
		return smi.stomach.IsReadyToPoop() && Time.time - smi.lastMealOrPoopTime >= smi.def.minimumTimeBeforePooping && !smi.IsInsideState(smi.sm.pause);
	}

	// Token: 0x06001A87 RID: 6791 RVA: 0x0008DC4D File Offset: 0x0008BE4D
	private static void UpdateMetabolismCalorieModifier(CreatureCalorieMonitor.Instance smi, float dt)
	{
		if (smi.IsInsideState(smi.sm.pause))
		{
			return;
		}
		smi.deltaCalorieMetabolismModifier.SetValue(1f - smi.metabolism.GetTotalValue() / 100f);
	}

	// Token: 0x06001A88 RID: 6792 RVA: 0x0008DC85 File Offset: 0x0008BE85
	private static void StarvationStartTime(CreatureCalorieMonitor.Instance smi)
	{
		if (smi.sm.starvationStartTime.Get(smi) <= 0f)
		{
			smi.sm.starvationStartTime.Set(GameClock.Instance.GetTime(), smi, false);
		}
	}

	// Token: 0x06001A89 RID: 6793 RVA: 0x0008DCBC File Offset: 0x0008BEBC
	private static void RecalculateStartTimeOnUnpause(CreatureCalorieMonitor.Instance smi)
	{
		float num = smi.sm.starvationStartTime.Get(smi);
		if (num < 0f)
		{
			float value = GameClock.Instance.GetTime() - Mathf.Abs(num);
			smi.sm.starvationStartTime.Set(value, smi, false);
		}
	}

	// Token: 0x04000EBF RID: 3775
	public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State normal;

	// Token: 0x04000EC0 RID: 3776
	public CreatureCalorieMonitor.PauseStates pause;

	// Token: 0x04000EC1 RID: 3777
	private CreatureCalorieMonitor.HungryStates hungry;

	// Token: 0x04000EC2 RID: 3778
	private Effect outOfCaloriesTame;

	// Token: 0x04000EC3 RID: 3779
	public StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.FloatParameter starvationStartTime;

	// Token: 0x02001127 RID: 4391
	public struct CaloriesConsumedEvent
	{
		// Token: 0x04005B73 RID: 23411
		public Tag tag;

		// Token: 0x04005B74 RID: 23412
		public float calories;
	}

	// Token: 0x02001128 RID: 4392
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06007889 RID: 30857 RVA: 0x002D689B File Offset: 0x002D4A9B
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Calories.Id);
		}

		// Token: 0x0600788A RID: 30858 RVA: 0x002D68C4 File Offset: 0x002D4AC4
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			List<Descriptor> list = new List<Descriptor>();
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_HEADER, UI.BUILDINGEFFECTS.TOOLTIPS.DIET_HEADER, Descriptor.DescriptorType.Effect, false));
			float dailyPlantGrowthConsumption = 1f;
			if (this.diet.consumedTags.Count > 0)
			{
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
				string newValue2;
				if (this.diet.eatsPlantsDirectly)
				{
					newValue2 = string.Join("\n", this.diet.consumedTags.Select(delegate(KeyValuePair<Tag, float> t)
					{
						dailyPlantGrowthConsumption = -calorie_loss_per_second / t.Value;
						GameObject prefab = Assets.GetPrefab(t.Key.ToString());
						Crop crop = prefab.GetComponent<Crop>();
						float num = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == crop.cropId).cropDuration / 600f;
						float num2 = 1f / num;
						return UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedPlantGrowth(-calorie_loss_per_second / t.Value * num2 * 100f, GameUtil.TimeSlice.PerCycle));
					}).ToArray<string>());
				}
				else
				{
					newValue2 = string.Join("\n", (from t in this.diet.consumedTags
					select UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(-calorie_loss_per_second / t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"))).ToArray<string>());
				}
				list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue2), Descriptor.DescriptorType.Effect, false));
			}
			if (this.diet.producedTags.Count > 0)
			{
				string newValue3 = string.Join(", ", (from t in this.diet.producedTags
				select t.Key.ProperName()).ToArray<string>());
				string newValue4;
				if (this.diet.eatsPlantsDirectly)
				{
					newValue4 = string.Join("\n", (from t in this.diet.producedTags
					select UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM_FROM_PLANT.text.Replace("{Item}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(t.Value * dailyPlantGrowthConsumption, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"))).ToArray<string>());
				}
				else
				{
					newValue4 = string.Join("\n", (from t in this.diet.producedTags
					select UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM.text.Replace("{Item}", t.Key.ProperName()).Replace("{Percent}", GameUtil.GetFormattedPercent(t.Value * 100f, GameUtil.TimeSlice.None))).ToArray<string>());
				}
				list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_PRODUCED.text.Replace("{Items}", newValue3), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_PRODUCED.text.Replace("{Items}", newValue4), Descriptor.DescriptorType.Effect, false));
			}
			return list;
		}

		// Token: 0x04005B75 RID: 23413
		public Diet diet;

		// Token: 0x04005B76 RID: 23414
		public float minPoopSizeInCalories = 100f;

		// Token: 0x04005B77 RID: 23415
		public float minimumTimeBeforePooping = 10f;

		// Token: 0x04005B78 RID: 23416
		public float deathTimer = 6000f;

		// Token: 0x04005B79 RID: 23417
		public bool storePoop;
	}

	// Token: 0x02001129 RID: 4393
	public class PauseStates : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
	{
		// Token: 0x04005B7A RID: 23418
		public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State commonPause;

		// Token: 0x04005B7B RID: 23419
		public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State starvingPause;
	}

	// Token: 0x0200112A RID: 4394
	public class HungryStates : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
	{
		// Token: 0x04005B7C RID: 23420
		public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State hungry;

		// Token: 0x04005B7D RID: 23421
		public CreatureCalorieMonitor.HungryStates.OutOfCaloriesState outofcalories;

		// Token: 0x02002092 RID: 8338
		public class OutOfCaloriesState : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
		{
			// Token: 0x0400918B RID: 37259
			public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State wild;

			// Token: 0x0400918C RID: 37260
			public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State tame;

			// Token: 0x0400918D RID: 37261
			public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State starvedtodeath;
		}
	}

	// Token: 0x0200112B RID: 4395
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Stomach
	{
		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x0600788E RID: 30862 RVA: 0x002D6C01 File Offset: 0x002D4E01
		// (set) Token: 0x0600788F RID: 30863 RVA: 0x002D6C09 File Offset: 0x002D4E09
		public Diet diet { get; private set; }

		// Token: 0x06007890 RID: 30864 RVA: 0x002D6C12 File Offset: 0x002D4E12
		public Stomach(Diet diet, GameObject owner, float min_poop_size_in_calories, bool storePoop)
		{
			this.diet = diet;
			this.owner = owner;
			this.minPoopSizeInCalories = min_poop_size_in_calories;
			this.storePoop = storePoop;
		}

		// Token: 0x06007891 RID: 30865 RVA: 0x002D6C44 File Offset: 0x002D4E44
		public void Poop()
		{
			float num = 0f;
			Tag tag = Tag.Invalid;
			byte disease_idx = byte.MaxValue;
			int num2 = 0;
			bool flag = false;
			for (int i = 0; i < this.caloriesConsumed.Count; i++)
			{
				CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[i];
				if (caloriesConsumedEntry.calories > 0f)
				{
					Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
					if (dietInfo != null && (!(tag != Tag.Invalid) || !(tag != dietInfo.producedElement)))
					{
						num += dietInfo.ConvertConsumptionMassToProducedMass(dietInfo.ConvertCaloriesToConsumptionMass(caloriesConsumedEntry.calories));
						tag = dietInfo.producedElement;
						disease_idx = dietInfo.diseaseIdx;
						num2 = (int)(dietInfo.diseasePerKgProduced * num);
						caloriesConsumedEntry.calories = 0f;
						this.caloriesConsumed[i] = caloriesConsumedEntry;
						flag = (flag || dietInfo.produceSolidTile);
					}
				}
			}
			if (num <= 0f || tag == Tag.Invalid)
			{
				return;
			}
			Element element = ElementLoader.GetElement(tag);
			global::Debug.Assert(element != null, "TODO: implement non-element tag spawning");
			int num3 = Grid.PosToCell(this.owner.transform.GetPosition());
			float temperature = this.owner.GetComponent<PrimaryElement>().Temperature;
			DebugUtil.DevAssert(!this.storePoop || !flag, "Stomach cannot both store poop & create a solid tile.", null);
			if (this.storePoop)
			{
				Storage component = this.owner.GetComponent<Storage>();
				if (element.IsLiquid)
				{
					component.AddLiquid(element.id, num, temperature, disease_idx, num2, false, true);
				}
				else if (element.IsGas)
				{
					component.AddGasChunk(element.id, num, temperature, disease_idx, num2, false, true);
				}
				else
				{
					component.AddOre(element.id, num, temperature, disease_idx, num2, false, true);
				}
			}
			else if (element.IsLiquid)
			{
				FallingWater.instance.AddParticle(num3, element.idx, num, temperature, disease_idx, num2, true, false, false, false);
			}
			else if (element.IsGas)
			{
				SimMessages.AddRemoveSubstance(num3, element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num, temperature, disease_idx, num2, true, -1);
			}
			else if (flag)
			{
				int num4 = this.owner.GetComponent<Facing>().GetFrontCell();
				if (!Grid.IsValidCell(num4))
				{
					global::Debug.LogWarningFormat("{0} attemping to Poop {1} on invalid cell {2} from cell {3}", new object[]
					{
						this.owner,
						element.name,
						num4,
						num3
					});
					num4 = num3;
				}
				SimMessages.AddRemoveSubstance(num4, element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num, temperature, disease_idx, num2, true, -1);
			}
			else
			{
				element.substance.SpawnResource(Grid.CellToPosCCC(num3, Grid.SceneLayer.Ore), num, temperature, disease_idx, num2, false, false, false);
			}
			KPrefabID component2 = this.owner.GetComponent<KPrefabID>();
			if (!Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(component2.PrefabTag))
			{
				Game.Instance.savedInfo.creaturePoopAmount.Add(component2.PrefabTag, 0f);
			}
			Dictionary<Tag, float> creaturePoopAmount = Game.Instance.savedInfo.creaturePoopAmount;
			Tag prefabTag = component2.PrefabTag;
			creaturePoopAmount[prefabTag] += num;
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, element.name, this.owner.transform, 1.5f, false);
		}

		// Token: 0x06007892 RID: 30866 RVA: 0x002D6FB4 File Offset: 0x002D51B4
		public List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry> GetCalorieEntries()
		{
			return this.caloriesConsumed;
		}

		// Token: 0x06007893 RID: 30867 RVA: 0x002D6FBC File Offset: 0x002D51BC
		public float GetTotalConsumedCalories()
		{
			float num = 0f;
			foreach (CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry in this.caloriesConsumed)
			{
				if (caloriesConsumedEntry.calories > 0f)
				{
					Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
					if (dietInfo != null && !(dietInfo.producedElement == Tag.Invalid))
					{
						num += caloriesConsumedEntry.calories;
					}
				}
			}
			return num;
		}

		// Token: 0x06007894 RID: 30868 RVA: 0x002D704C File Offset: 0x002D524C
		public float GetFullness()
		{
			return this.GetTotalConsumedCalories() / this.minPoopSizeInCalories;
		}

		// Token: 0x06007895 RID: 30869 RVA: 0x002D705C File Offset: 0x002D525C
		public bool IsReadyToPoop()
		{
			float totalConsumedCalories = this.GetTotalConsumedCalories();
			return totalConsumedCalories > 0f && totalConsumedCalories >= this.minPoopSizeInCalories;
		}

		// Token: 0x06007896 RID: 30870 RVA: 0x002D7088 File Offset: 0x002D5288
		public void Consume(Tag tag, float calories)
		{
			for (int i = 0; i < this.caloriesConsumed.Count; i++)
			{
				CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[i];
				if (caloriesConsumedEntry.tag == tag)
				{
					caloriesConsumedEntry.calories += calories;
					this.caloriesConsumed[i] = caloriesConsumedEntry;
					return;
				}
			}
			CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry item = default(CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry);
			item.tag = tag;
			item.calories = calories;
			this.caloriesConsumed.Add(item);
		}

		// Token: 0x06007897 RID: 30871 RVA: 0x002D7104 File Offset: 0x002D5304
		public Tag GetNextPoopEntry()
		{
			for (int i = 0; i < this.caloriesConsumed.Count; i++)
			{
				CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[i];
				if (caloriesConsumedEntry.calories > 0f)
				{
					Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
					if (dietInfo != null && !(dietInfo.producedElement == Tag.Invalid))
					{
						return dietInfo.producedElement;
					}
				}
			}
			return Tag.Invalid;
		}

		// Token: 0x04005B7E RID: 23422
		[Serialize]
		private List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry> caloriesConsumed = new List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry>();

		// Token: 0x04005B7F RID: 23423
		private float minPoopSizeInCalories;

		// Token: 0x04005B81 RID: 23425
		private GameObject owner;

		// Token: 0x04005B82 RID: 23426
		private bool storePoop;

		// Token: 0x02002093 RID: 8339
		[Serializable]
		public struct CaloriesConsumedEntry
		{
			// Token: 0x0400918E RID: 37262
			public Tag tag;

			// Token: 0x0400918F RID: 37263
			public float calories;
		}
	}

	// Token: 0x0200112C RID: 4396
	public new class Instance : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.GameInstance
	{
		// Token: 0x06007898 RID: 30872 RVA: 0x002D7174 File Offset: 0x002D5374
		public Instance(IStateMachineTarget master, CreatureCalorieMonitor.Def def) : base(master, def)
		{
			this.calories = Db.Get().Amounts.Calories.Lookup(base.gameObject);
			this.calories.value = this.calories.GetMax() * 0.9f;
			this.stomach = new CreatureCalorieMonitor.Stomach(def.diet, master.gameObject, def.minPoopSizeInCalories, def.storePoop);
			this.metabolism = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Metabolism);
			this.deltaCalorieMetabolismModifier = new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, 1f, DUPLICANTS.MODIFIERS.METABOLISM_CALORIE_MODIFIER.NAME, true, false, false);
			this.calories.deltaAttribute.Add(this.deltaCalorieMetabolismModifier);
		}

		// Token: 0x06007899 RID: 30873 RVA: 0x002D7259 File Offset: 0x002D5459
		public override void StartSM()
		{
			this.prefabID = base.gameObject.GetComponent<KPrefabID>();
			base.StartSM();
		}

		// Token: 0x0600789A RID: 30874 RVA: 0x002D7274 File Offset: 0x002D5474
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			this.calories.value += caloriesConsumedEvent.calories;
			this.stomach.Consume(caloriesConsumedEvent.tag, caloriesConsumedEvent.calories);
			this.lastMealOrPoopTime = Time.time;
		}

		// Token: 0x0600789B RID: 30875 RVA: 0x002D72C2 File Offset: 0x002D54C2
		public float GetDeathTimeRemaining()
		{
			return base.smi.def.deathTimer - (GameClock.Instance.GetTime() - base.sm.starvationStartTime.Get(base.smi));
		}

		// Token: 0x0600789C RID: 30876 RVA: 0x002D72F6 File Offset: 0x002D54F6
		public void Poop()
		{
			this.lastMealOrPoopTime = Time.time;
			this.stomach.Poop();
		}

		// Token: 0x0600789D RID: 30877 RVA: 0x002D730E File Offset: 0x002D550E
		public float GetCalories0to1()
		{
			return this.calories.value / this.calories.GetMax();
		}

		// Token: 0x0600789E RID: 30878 RVA: 0x002D7327 File Offset: 0x002D5527
		public bool IsHungry()
		{
			return this.GetCalories0to1() < 0.9f;
		}

		// Token: 0x0600789F RID: 30879 RVA: 0x002D7336 File Offset: 0x002D5536
		public bool IsOutOfCalories()
		{
			return this.GetCalories0to1() <= 0f;
		}

		// Token: 0x04005B83 RID: 23427
		public const float HUNGRY_RATIO = 0.9f;

		// Token: 0x04005B84 RID: 23428
		public AmountInstance calories;

		// Token: 0x04005B85 RID: 23429
		[Serialize]
		public CreatureCalorieMonitor.Stomach stomach;

		// Token: 0x04005B86 RID: 23430
		public float lastMealOrPoopTime;

		// Token: 0x04005B87 RID: 23431
		public AttributeInstance metabolism;

		// Token: 0x04005B88 RID: 23432
		public AttributeModifier deltaCalorieMetabolismModifier;

		// Token: 0x04005B89 RID: 23433
		public KPrefabID prefabID;
	}
}
