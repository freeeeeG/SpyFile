﻿using System;
using System.Collections.Generic;
using System.IO;
using Klei.AI;
using ProcGen;
using UnityEngine;

// Token: 0x0200077A RID: 1914
public class EconomyDetails
{
	// Token: 0x060034F5 RID: 13557 RVA: 0x0011C6F4 File Offset: 0x0011A8F4
	public EconomyDetails()
	{
		this.massResourceType = new EconomyDetails.Resource.Type("Mass", "kg");
		this.heatResourceType = new EconomyDetails.Resource.Type("Heat Energy", "kdtu");
		this.energyResourceType = new EconomyDetails.Resource.Type("Energy", "joules");
		this.timeResourceType = new EconomyDetails.Resource.Type("Time", "seconds");
		this.attributeResourceType = new EconomyDetails.Resource.Type("Attribute", "units");
		this.caloriesResourceType = new EconomyDetails.Resource.Type("Calories", "kcal");
		this.amountResourceType = new EconomyDetails.Resource.Type("Amount", "units");
		this.buildingTransformationType = new EconomyDetails.Transformation.Type("Building");
		this.foodTransformationType = new EconomyDetails.Transformation.Type("Food");
		this.plantTransformationType = new EconomyDetails.Transformation.Type("Plant");
		this.creatureTransformationType = new EconomyDetails.Transformation.Type("Creature");
		this.dupeTransformationType = new EconomyDetails.Transformation.Type("Duplicant");
		this.referenceTransformationType = new EconomyDetails.Transformation.Type("Reference");
		this.effectTransformationType = new EconomyDetails.Transformation.Type("Effect");
		this.geyserActivePeriodTransformationType = new EconomyDetails.Transformation.Type("GeyserActivePeriod");
		this.geyserLifetimeTransformationType = new EconomyDetails.Transformation.Type("GeyserLifetime");
		this.energyResource = this.CreateResource(TagManager.Create("Energy"), this.energyResourceType);
		this.heatResource = this.CreateResource(TagManager.Create("Heat"), this.heatResourceType);
		this.duplicantTimeResource = this.CreateResource(TagManager.Create("DupeTime"), this.timeResourceType);
		this.caloriesResource = this.CreateResource(new Tag(Db.Get().Amounts.Calories.deltaAttribute.Id), this.caloriesResourceType);
		this.fixedCaloriesResource = this.CreateResource(new Tag(Db.Get().Amounts.Calories.Id), this.caloriesResourceType);
		foreach (Element element in ElementLoader.elements)
		{
			this.CreateResource(element);
		}
		foreach (Tag tag in new List<Tag>
		{
			GameTags.CombustibleLiquid,
			GameTags.CombustibleGas,
			GameTags.CombustibleSolid
		})
		{
			this.CreateResource(tag, this.massResourceType);
		}
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			this.CreateResource(foodInfo.Id.ToTag(), this.amountResourceType);
		}
		this.GatherStartingBiomeAmounts();
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			this.CreateTransformation(kprefabID, kprefabID.PrefabTag);
			if (kprefabID.GetComponent<GeyserConfigurator>() != null)
			{
				KPrefabID prefab_id = kprefabID;
				Tag prefabTag = kprefabID.PrefabTag;
				this.CreateTransformation(prefab_id, prefabTag.ToString() + "_ActiveOnly");
			}
		}
		foreach (Effect effect in Db.Get().effects.resources)
		{
			this.CreateTransformation(effect);
		}
		EconomyDetails.Transformation transformation = new EconomyDetails.Transformation(TagManager.Create("Duplicant"), this.dupeTransformationType, 1f, false);
		transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Oxygen), -0.1f));
		transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.CarbonDioxide), 0.1f * Assets.GetPrefab(MinionConfig.ID).GetComponent<OxygenBreather>().O2toCO2conversion));
		transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.duplicantTimeResource, 0.875f));
		transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.caloriesResource, -1.6666667f));
		transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(new Tag(Db.Get().Amounts.Bladder.deltaAttribute.Id), this.amountResourceType), 0.16666667f));
		this.transformations.Add(transformation);
		EconomyDetails.Transformation transformation2 = new EconomyDetails.Transformation(TagManager.Create("Electrolysis"), this.referenceTransformationType, 1f, false);
		transformation2.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Oxygen), 1.7777778f));
		transformation2.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Hydrogen), 0.22222222f));
		transformation2.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Water), -2f));
		this.transformations.Add(transformation2);
		EconomyDetails.Transformation transformation3 = new EconomyDetails.Transformation(TagManager.Create("MethaneCombustion"), this.referenceTransformationType, 1f, false);
		transformation3.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Methane), -1f));
		transformation3.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Oxygen), -4f));
		transformation3.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.CarbonDioxide), 2.75f));
		transformation3.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Water), 2.25f));
		this.transformations.Add(transformation3);
		EconomyDetails.Transformation transformation4 = new EconomyDetails.Transformation(TagManager.Create("CoalCombustion"), this.referenceTransformationType, 1f, false);
		transformation4.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Carbon), -1f));
		transformation4.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.Oxygen), -2.6666667f));
		transformation4.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(GameTags.CarbonDioxide), 3.6666667f));
		this.transformations.Add(transformation4);
	}

	// Token: 0x060034F6 RID: 13558 RVA: 0x0011CD48 File Offset: 0x0011AF48
	private static void WriteProduct(StreamWriter o, string a, string b)
	{
		o.Write(string.Concat(new string[]
		{
			"\"=PRODUCT(",
			a,
			", ",
			b,
			")\""
		}));
	}

	// Token: 0x060034F7 RID: 13559 RVA: 0x0011CD7B File Offset: 0x0011AF7B
	private static void WriteProduct(StreamWriter o, string a, string b, string c)
	{
		o.Write(string.Concat(new string[]
		{
			"\"=PRODUCT(",
			a,
			", ",
			b,
			", ",
			c,
			")\""
		}));
	}

	// Token: 0x060034F8 RID: 13560 RVA: 0x0011CDBC File Offset: 0x0011AFBC
	public void DumpTransformations(EconomyDetails.Scenario scenario, StreamWriter o)
	{
		List<EconomyDetails.Resource> used_resources = new List<EconomyDetails.Resource>();
		foreach (EconomyDetails.Transformation transformation in this.transformations)
		{
			if (scenario.IncludesTransformation(transformation))
			{
				foreach (EconomyDetails.Transformation.Delta delta in transformation.deltas)
				{
					if (!used_resources.Contains(delta.resource))
					{
						used_resources.Add(delta.resource);
					}
				}
			}
		}
		used_resources.Sort((EconomyDetails.Resource x, EconomyDetails.Resource y) => x.tag.Name.CompareTo(y.tag.Name));
		List<EconomyDetails.Ratio> list = new List<EconomyDetails.Ratio>();
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.Algae), this.GetResource(GameTags.Oxygen), false));
		list.Add(new EconomyDetails.Ratio(this.energyResource, this.GetResource(GameTags.Oxygen), false));
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.Oxygen), this.energyResource, false));
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.Water), this.GetResource(GameTags.Oxygen), false));
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.DirtyWater), this.caloriesResource, false));
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.Water), this.caloriesResource, false));
		list.Add(new EconomyDetails.Ratio(this.GetResource(GameTags.Fertilizer), this.caloriesResource, false));
		list.Add(new EconomyDetails.Ratio(this.energyResource, this.CreateResource(new Tag(Db.Get().Amounts.Stress.deltaAttribute.Id), this.amountResourceType), true));
		list.RemoveAll((EconomyDetails.Ratio x) => !used_resources.Contains(x.input) || !used_resources.Contains(x.output));
		o.Write("Id");
		o.Write(",Count");
		o.Write(",Type");
		o.Write(",Time(s)");
		int num = 4;
		foreach (EconomyDetails.Resource resource in used_resources)
		{
			o.Write(string.Concat(new string[]
			{
				", ",
				resource.tag.Name,
				"(",
				resource.type.unit,
				")"
			}));
			num++;
		}
		o.Write(",MassDelta");
		foreach (EconomyDetails.Ratio ratio in list)
		{
			o.Write(string.Concat(new string[]
			{
				", ",
				ratio.output.tag.Name,
				"(",
				ratio.output.type.unit,
				")/",
				ratio.input.tag.Name,
				"(",
				ratio.input.type.unit,
				")"
			}));
			num++;
		}
		string str = "B";
		o.Write("\n");
		int num2 = 1;
		this.transformations.Sort((EconomyDetails.Transformation x, EconomyDetails.Transformation y) => x.tag.Name.CompareTo(y.tag.Name));
		for (int i = 0; i < this.transformations.Count; i++)
		{
			EconomyDetails.Transformation transformation2 = this.transformations[i];
			if (scenario.IncludesTransformation(transformation2))
			{
				num2++;
			}
		}
		string text = "B" + (num2 + 4).ToString();
		int num3 = 1;
		for (int j = 0; j < this.transformations.Count; j++)
		{
			EconomyDetails.Transformation transformation3 = this.transformations[j];
			if (scenario.IncludesTransformation(transformation3))
			{
				if (transformation3.tag == new Tag(EconomyDetails.debugTag))
				{
					int num4 = 0 + 1;
				}
				num3++;
				o.Write("\"" + transformation3.tag.Name + "\"");
				o.Write("," + scenario.GetCount(transformation3.tag).ToString());
				o.Write(",\"" + transformation3.type.id + "\"");
				if (!transformation3.timeInvariant)
				{
					o.Write(",\"" + transformation3.timeInSeconds.ToString("0.00") + "\"");
				}
				else
				{
					o.Write(",\"invariant\"");
				}
				string a = str + num3.ToString();
				float num5 = 0f;
				bool flag = false;
				foreach (EconomyDetails.Resource resource2 in used_resources)
				{
					EconomyDetails.Transformation.Delta delta2 = null;
					foreach (EconomyDetails.Transformation.Delta delta3 in transformation3.deltas)
					{
						if (delta3.resource.tag == resource2.tag)
						{
							delta2 = delta3;
							break;
						}
					}
					o.Write(",");
					if (delta2 != null && delta2.amount != 0f)
					{
						if (delta2.resource.type == this.massResourceType)
						{
							flag = true;
							num5 += delta2.amount;
						}
						if (!transformation3.timeInvariant)
						{
							EconomyDetails.WriteProduct(o, a, (delta2.amount / transformation3.timeInSeconds).ToString("0.00000"), text);
						}
						else
						{
							EconomyDetails.WriteProduct(o, a, delta2.amount.ToString("0.00000"));
						}
					}
				}
				o.Write(",");
				if (flag)
				{
					num5 /= transformation3.timeInSeconds;
					EconomyDetails.WriteProduct(o, a, num5.ToString("0.00000"), text);
				}
				foreach (EconomyDetails.Ratio ratio2 in list)
				{
					o.Write(", ");
					EconomyDetails.Transformation.Delta delta4 = transformation3.GetDelta(ratio2.input);
					EconomyDetails.Transformation.Delta delta5 = transformation3.GetDelta(ratio2.output);
					if (delta5 != null && delta4 != null && delta4.amount < 0f && (delta5.amount > 0f || ratio2.allowNegativeOutput))
					{
						o.Write(delta5.amount / Mathf.Abs(delta4.amount));
					}
				}
				o.Write("\n");
			}
		}
		int num6 = 4;
		for (int k = 0; k < num; k++)
		{
			if (k >= num6 && k < num6 + used_resources.Count)
			{
				string text2 = ((char)(65 + k % 26)).ToString();
				int num7 = Mathf.FloorToInt((float)k / 26f);
				if (num7 > 0)
				{
					text2 = ((char)(65 + num7 - 1)).ToString() + text2;
				}
				o.Write(string.Concat(new string[]
				{
					"\"=SUM(",
					text2,
					"2: ",
					text2,
					num2.ToString(),
					")\""
				}));
			}
			o.Write(",");
		}
		string str2 = "B" + (num2 + 5).ToString();
		o.Write("\n");
		o.Write("\nTiming:");
		o.Write("\nTimeInSeconds:," + scenario.timeInSeconds.ToString());
		o.Write("\nSecondsPerCycle:," + 600f.ToString());
		o.Write("\nCycles:,=" + text + "/" + str2);
	}

	// Token: 0x060034F9 RID: 13561 RVA: 0x0011D6E8 File Offset: 0x0011B8E8
	public EconomyDetails.Resource CreateResource(Tag tag, EconomyDetails.Resource.Type resource_type)
	{
		foreach (EconomyDetails.Resource resource in this.resources)
		{
			if (resource.tag == tag)
			{
				return resource;
			}
		}
		EconomyDetails.Resource resource2 = new EconomyDetails.Resource(tag, resource_type);
		this.resources.Add(resource2);
		return resource2;
	}

	// Token: 0x060034FA RID: 13562 RVA: 0x0011D760 File Offset: 0x0011B960
	public EconomyDetails.Resource CreateResource(Element element)
	{
		return this.CreateResource(element.tag, this.massResourceType);
	}

	// Token: 0x060034FB RID: 13563 RVA: 0x0011D774 File Offset: 0x0011B974
	public EconomyDetails.Transformation CreateTransformation(Effect effect)
	{
		EconomyDetails.Transformation transformation = new EconomyDetails.Transformation(new Tag(effect.Id), this.effectTransformationType, 1f, false);
		foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
		{
			EconomyDetails.Resource resource = this.CreateResource(new Tag(attributeModifier.AttributeId), this.attributeResourceType);
			transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource, attributeModifier.Value));
		}
		this.transformations.Add(transformation);
		return transformation;
	}

	// Token: 0x060034FC RID: 13564 RVA: 0x0011D814 File Offset: 0x0011BA14
	public EconomyDetails.Transformation GetTransformation(Tag tag)
	{
		foreach (EconomyDetails.Transformation transformation in this.transformations)
		{
			if (transformation.tag == tag)
			{
				return transformation;
			}
		}
		return null;
	}

	// Token: 0x060034FD RID: 13565 RVA: 0x0011D878 File Offset: 0x0011BA78
	public EconomyDetails.Transformation CreateTransformation(KPrefabID prefab_id, Tag tag)
	{
		if (tag == new Tag(EconomyDetails.debugTag))
		{
			int num = 0 + 1;
		}
		Building component = prefab_id.GetComponent<Building>();
		ElementConverter component2 = prefab_id.GetComponent<ElementConverter>();
		EnergyConsumer component3 = prefab_id.GetComponent<EnergyConsumer>();
		ElementConsumer component4 = prefab_id.GetComponent<ElementConsumer>();
		BuildingElementEmitter component5 = prefab_id.GetComponent<BuildingElementEmitter>();
		Generator component6 = prefab_id.GetComponent<Generator>();
		EnergyGenerator component7 = prefab_id.GetComponent<EnergyGenerator>();
		ManualGenerator component8 = prefab_id.GetComponent<ManualGenerator>();
		ManualDeliveryKG[] components = prefab_id.GetComponents<ManualDeliveryKG>();
		StateMachineController component9 = prefab_id.GetComponent<StateMachineController>();
		Edible component10 = prefab_id.GetComponent<Edible>();
		Crop component11 = prefab_id.GetComponent<Crop>();
		Uprootable component12 = prefab_id.GetComponent<Uprootable>();
		ComplexRecipe complexRecipe = ComplexRecipeManager.Get().recipes.Find((ComplexRecipe r) => r.FirstResult == prefab_id.PrefabTag);
		List<FertilizationMonitor.Def> list = null;
		List<IrrigationMonitor.Def> list2 = null;
		GeyserConfigurator component13 = prefab_id.GetComponent<GeyserConfigurator>();
		Toilet component14 = prefab_id.GetComponent<Toilet>();
		FlushToilet component15 = prefab_id.GetComponent<FlushToilet>();
		RelaxationPoint component16 = prefab_id.GetComponent<RelaxationPoint>();
		CreatureCalorieMonitor.Def def = prefab_id.gameObject.GetDef<CreatureCalorieMonitor.Def>();
		if (component9 != null)
		{
			list = component9.GetDefs<FertilizationMonitor.Def>();
			list2 = component9.GetDefs<IrrigationMonitor.Def>();
		}
		EconomyDetails.Transformation transformation = null;
		float time_in_seconds = 1f;
		if (component10 != null)
		{
			transformation = new EconomyDetails.Transformation(tag, this.foodTransformationType, time_in_seconds, complexRecipe != null);
		}
		else if (component2 != null || component3 != null || component4 != null || component5 != null || component6 != null || component7 != null || component12 != null || component13 != null || component14 != null || component15 != null || component16 != null || def != null)
		{
			if (component12 != null || component11 != null)
			{
				if (component11 != null)
				{
					time_in_seconds = component11.cropVal.cropDuration;
				}
				transformation = new EconomyDetails.Transformation(tag, this.plantTransformationType, time_in_seconds, false);
			}
			else if (def != null)
			{
				transformation = new EconomyDetails.Transformation(tag, this.creatureTransformationType, time_in_seconds, false);
			}
			else if (component13 != null)
			{
				GeyserConfigurator.GeyserInstanceConfiguration geyserInstanceConfiguration = new GeyserConfigurator.GeyserInstanceConfiguration
				{
					typeId = component13.presetType,
					rateRoll = 0.5f,
					iterationLengthRoll = 0.5f,
					iterationPercentRoll = 0.5f,
					yearLengthRoll = 0.5f,
					yearPercentRoll = 0.5f
				};
				if (tag.Name.Contains("_ActiveOnly"))
				{
					float iterationLength = geyserInstanceConfiguration.GetIterationLength();
					transformation = new EconomyDetails.Transformation(tag, this.geyserActivePeriodTransformationType, iterationLength, false);
				}
				else
				{
					float yearLength = geyserInstanceConfiguration.GetYearLength();
					transformation = new EconomyDetails.Transformation(tag, this.geyserLifetimeTransformationType, yearLength, false);
				}
			}
			else
			{
				if (component14 != null || component15 != null)
				{
					time_in_seconds = 600f;
				}
				transformation = new EconomyDetails.Transformation(tag, this.buildingTransformationType, time_in_seconds, false);
			}
		}
		if (transformation != null)
		{
			if (component2 != null && component2.consumedElements != null)
			{
				foreach (ElementConverter.ConsumedElement consumedElement in component2.consumedElements)
				{
					EconomyDetails.Resource resource = this.CreateResource(consumedElement.Tag, this.massResourceType);
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource, -consumedElement.MassConsumptionRate));
				}
				if (component2.outputElements != null)
				{
					foreach (ElementConverter.OutputElement outputElement in component2.outputElements)
					{
						Element element = ElementLoader.FindElementByHash(outputElement.elementHash);
						EconomyDetails.Resource resource2 = this.CreateResource(element.tag, this.massResourceType);
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource2, outputElement.massGenerationRate));
					}
				}
			}
			if (component4 != null && component7 == null && (component2 == null || prefab_id.GetComponent<AlgaeHabitat>() != null))
			{
				EconomyDetails.Resource resource3 = this.GetResource(ElementLoader.FindElementByHash(component4.elementToConsume).tag);
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource3, -component4.consumptionRate));
			}
			if (component3 != null)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.energyResource, -component3.WattsNeededWhenActive));
			}
			if (component5 != null)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(component5.element), component5.emitRate));
			}
			if (component6 != null)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.energyResource, component6.GetComponent<Building>().Def.GeneratorWattageRating));
			}
			if (component7 != null)
			{
				if (component7.formula.inputs != null)
				{
					foreach (EnergyGenerator.InputItem inputItem in component7.formula.inputs)
					{
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(inputItem.tag), -inputItem.consumptionRate));
					}
				}
				if (component7.formula.outputs != null)
				{
					foreach (EnergyGenerator.OutputItem outputItem in component7.formula.outputs)
					{
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(outputItem.element), outputItem.creationRate));
					}
				}
			}
			if (component)
			{
				BuildingDef def2 = component.Def;
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.heatResource, def2.SelfHeatKilowattsWhenActive + def2.ExhaustKilowattsWhenActive));
			}
			if (component8)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.duplicantTimeResource, -1f));
			}
			if (component10)
			{
				EdiblesManager.FoodInfo foodInfo = component10.FoodInfo;
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.fixedCaloriesResource, foodInfo.CaloriesPerUnit * 0.001f));
				ComplexRecipeManager.Get().recipes.Find((ComplexRecipe a) => a.FirstResult == tag);
			}
			if (component11 != null)
			{
				EconomyDetails.Resource resource4 = this.CreateResource(TagManager.Create(component11.cropVal.cropId), this.amountResourceType);
				float num2 = (float)component11.cropVal.numProduced;
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource4, num2));
				GameObject prefab = Assets.GetPrefab(new Tag(component11.cropVal.cropId));
				if (prefab != null)
				{
					Edible component17 = prefab.GetComponent<Edible>();
					if (component17 != null)
					{
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.caloriesResource, component17.FoodInfo.CaloriesPerUnit * num2 * 0.001f));
					}
				}
			}
			if (complexRecipe != null)
			{
				foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
				{
					this.CreateResource(recipeElement.material, this.amountResourceType);
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(recipeElement.material), -recipeElement.amount));
				}
				foreach (ComplexRecipe.RecipeElement recipeElement2 in complexRecipe.results)
				{
					this.CreateResource(recipeElement2.material, this.amountResourceType);
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(recipeElement2.material), recipeElement2.amount));
				}
			}
			if (components != null)
			{
				for (int j = 0; j < components.Length; j++)
				{
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.duplicantTimeResource, -0.1f * transformation.timeInSeconds));
				}
			}
			if (list != null && list.Count > 0)
			{
				foreach (FertilizationMonitor.Def def3 in list)
				{
					foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in def3.consumedElements)
					{
						EconomyDetails.Resource resource5 = this.CreateResource(consumeInfo.tag, this.massResourceType);
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource5, -consumeInfo.massConsumptionRate * transformation.timeInSeconds));
					}
				}
			}
			if (list2 != null && list2.Count > 0)
			{
				foreach (IrrigationMonitor.Def def4 in list2)
				{
					foreach (PlantElementAbsorber.ConsumeInfo consumeInfo2 in def4.consumedElements)
					{
						EconomyDetails.Resource resource6 = this.CreateResource(consumeInfo2.tag, this.massResourceType);
						transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource6, -consumeInfo2.massConsumptionRate * transformation.timeInSeconds));
					}
				}
			}
			if (component13 != null)
			{
				GeyserConfigurator.GeyserInstanceConfiguration geyserInstanceConfiguration2 = new GeyserConfigurator.GeyserInstanceConfiguration
				{
					typeId = component13.presetType,
					rateRoll = 0.5f,
					iterationLengthRoll = 0.5f,
					iterationPercentRoll = 0.5f,
					yearLengthRoll = 0.5f,
					yearPercentRoll = 0.5f
				};
				if (tag.Name.Contains("_ActiveOnly"))
				{
					float amount = geyserInstanceConfiguration2.GetMassPerCycle() / 600f * geyserInstanceConfiguration2.GetIterationLength();
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(geyserInstanceConfiguration2.GetElement().CreateTag(), this.massResourceType), amount));
				}
				else
				{
					float amount2 = geyserInstanceConfiguration2.GetMassPerCycle() / 600f * geyserInstanceConfiguration2.GetYearLength() * geyserInstanceConfiguration2.GetYearPercent();
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(geyserInstanceConfiguration2.GetElement().CreateTag(), this.massResourceType), amount2));
				}
			}
			if (component14 != null)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(new Tag(Db.Get().Amounts.Bladder.deltaAttribute.Id), this.amountResourceType), -0.16666667f));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(SimHashes.Dirt), -component14.solidWastePerUse.mass));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(component14.solidWastePerUse.elementID), component14.solidWastePerUse.mass));
			}
			if (component15 != null)
			{
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(new Tag(Db.Get().Amounts.Bladder.deltaAttribute.Id), this.amountResourceType), -0.16666667f));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(SimHashes.Water), -component15.massConsumedPerUse));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.GetResource(SimHashes.DirtyWater), component15.massEmittedPerUse));
			}
			if (component16 != null)
			{
				foreach (AttributeModifier attributeModifier in component16.CreateEffect().SelfModifiers)
				{
					EconomyDetails.Resource resource7 = this.CreateResource(new Tag(attributeModifier.AttributeId), this.attributeResourceType);
					transformation.AddDelta(new EconomyDetails.Transformation.Delta(resource7, attributeModifier.Value));
				}
			}
			if (def != null)
			{
				this.CollectDietTransformations(prefab_id);
			}
			this.transformations.Add(transformation);
		}
		return transformation;
	}

	// Token: 0x060034FE RID: 13566 RVA: 0x0011E460 File Offset: 0x0011C660
	private void CollectDietTransformations(KPrefabID prefab_id)
	{
		Trait trait = Db.Get().traits.Get(prefab_id.GetComponent<Modifiers>().initialTraits[0]);
		CreatureCalorieMonitor.Def def = prefab_id.gameObject.GetDef<CreatureCalorieMonitor.Def>();
		WildnessMonitor.Def def2 = prefab_id.gameObject.GetDef<WildnessMonitor.Def>();
		List<AttributeModifier> list = new List<AttributeModifier>();
		list.AddRange(trait.SelfModifiers);
		list.AddRange(def2.tameEffect.SelfModifiers);
		float num = 0f;
		float num2 = 0f;
		foreach (AttributeModifier attributeModifier in list)
		{
			if (attributeModifier.AttributeId == Db.Get().Amounts.Calories.maxAttribute.Id)
			{
				num = attributeModifier.Value;
			}
			if (attributeModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
			{
				num2 = attributeModifier.Value;
			}
		}
		foreach (Diet.Info info in def.diet.infos)
		{
			foreach (Tag tag in info.consumedTags)
			{
				float time_in_seconds = Mathf.Abs(num / num2);
				float num3 = num / info.caloriesPerKg;
				float amount = num3 * info.producedConversionRate;
				EconomyDetails.Transformation transformation = new EconomyDetails.Transformation(new Tag(prefab_id.PrefabTag.Name + "Diet" + tag.Name), this.creatureTransformationType, time_in_seconds, false);
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(tag, this.massResourceType), -num3));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.CreateResource(new Tag(info.producedElement.ToString()), this.massResourceType), amount));
				transformation.AddDelta(new EconomyDetails.Transformation.Delta(this.caloriesResource, num));
				this.transformations.Add(transformation);
			}
		}
	}

	// Token: 0x060034FF RID: 13567 RVA: 0x0011E6A8 File Offset: 0x0011C8A8
	private static void CollectDietScenarios(List<EconomyDetails.Scenario> scenarios)
	{
		EconomyDetails.Scenario scenario = new EconomyDetails.Scenario("diets/all", 0f, null);
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			CreatureCalorieMonitor.Def def = kprefabID.gameObject.GetDef<CreatureCalorieMonitor.Def>();
			if (def != null)
			{
				EconomyDetails.Scenario scenario2 = new EconomyDetails.Scenario("diets/" + kprefabID.name, 0f, null);
				Diet.Info[] infos = def.diet.infos;
				for (int i = 0; i < infos.Length; i++)
				{
					foreach (Tag tag in infos[i].consumedTags)
					{
						Tag tag2 = kprefabID.PrefabTag.Name + "Diet" + tag.Name;
						scenario2.AddEntry(new EconomyDetails.Scenario.Entry(tag2, 1f));
						scenario.AddEntry(new EconomyDetails.Scenario.Entry(tag2, 1f));
					}
				}
				scenarios.Add(scenario2);
			}
		}
		scenarios.Add(scenario);
	}

	// Token: 0x06003500 RID: 13568 RVA: 0x0011E7F8 File Offset: 0x0011C9F8
	public void GatherStartingBiomeAmounts()
	{
		for (int i = 0; i < Grid.CellCount; i++)
		{
			if (global::World.Instance.zoneRenderData.worldZoneTypes[i] == SubWorld.ZoneType.Sandstone)
			{
				Element key = Grid.Element[i];
				float num = 0f;
				this.startingBiomeAmounts.TryGetValue(key, out num);
				this.startingBiomeAmounts[key] = num + Grid.Mass[i];
				this.startingBiomeCellCount++;
			}
		}
	}

	// Token: 0x06003501 RID: 13569 RVA: 0x0011E86D File Offset: 0x0011CA6D
	public EconomyDetails.Resource GetResource(SimHashes element)
	{
		return this.GetResource(ElementLoader.FindElementByHash(element).tag);
	}

	// Token: 0x06003502 RID: 13570 RVA: 0x0011E880 File Offset: 0x0011CA80
	public EconomyDetails.Resource GetResource(Tag tag)
	{
		foreach (EconomyDetails.Resource resource in this.resources)
		{
			if (resource.tag == tag)
			{
				return resource;
			}
		}
		DebugUtil.LogErrorArgs(new object[]
		{
			"Found a tag without a matching resource!",
			tag
		});
		return null;
	}

	// Token: 0x06003503 RID: 13571 RVA: 0x0011E900 File Offset: 0x0011CB00
	private float GetDupeBreathingPerSecond(EconomyDetails details)
	{
		return details.GetTransformation(TagManager.Create("Duplicant")).GetDelta(details.GetResource(GameTags.Oxygen)).amount;
	}

	// Token: 0x06003504 RID: 13572 RVA: 0x0011E928 File Offset: 0x0011CB28
	private EconomyDetails.BiomeTransformation CreateBiomeTransformationFromTransformation(EconomyDetails details, Tag transformation_tag, Tag input_resource_tag, Tag output_resource_tag)
	{
		EconomyDetails.Resource resource = details.GetResource(input_resource_tag);
		EconomyDetails.Resource resource2 = details.GetResource(output_resource_tag);
		EconomyDetails.Transformation transformation = details.GetTransformation(transformation_tag);
		float num = transformation.GetDelta(resource2).amount / -transformation.GetDelta(resource).amount;
		float num2 = this.GetDupeBreathingPerSecond(details) * 600f;
		return new EconomyDetails.BiomeTransformation((transformation_tag.Name + input_resource_tag.Name + "Cycles").ToTag(), resource, num / -num2);
	}

	// Token: 0x06003505 RID: 13573 RVA: 0x0011E9A0 File Offset: 0x0011CBA0
	private static void DumpEconomyDetails()
	{
		global::Debug.Log("Starting Economy Details Dump...");
		EconomyDetails details = new EconomyDetails();
		List<EconomyDetails.Scenario> list = new List<EconomyDetails.Scenario>();
		EconomyDetails.Scenario item = new EconomyDetails.Scenario("default", 1f, (EconomyDetails.Transformation t) => true);
		list.Add(item);
		EconomyDetails.Scenario item2 = new EconomyDetails.Scenario("all_buildings", 1f, (EconomyDetails.Transformation t) => t.type == details.buildingTransformationType);
		list.Add(item2);
		EconomyDetails.Scenario item3 = new EconomyDetails.Scenario("all_plants", 1f, (EconomyDetails.Transformation t) => t.type == details.plantTransformationType);
		list.Add(item3);
		EconomyDetails.Scenario item4 = new EconomyDetails.Scenario("all_creatures", 1f, (EconomyDetails.Transformation t) => t.type == details.creatureTransformationType);
		list.Add(item4);
		EconomyDetails.Scenario item5 = new EconomyDetails.Scenario("all_stress", 1f, (EconomyDetails.Transformation t) => t.GetDelta(details.GetResource(new Tag(Db.Get().Amounts.Stress.deltaAttribute.Id))) != null);
		list.Add(item5);
		EconomyDetails.Scenario item6 = new EconomyDetails.Scenario("all_foods", 1f, (EconomyDetails.Transformation t) => t.type == details.foodTransformationType);
		list.Add(item6);
		EconomyDetails.Scenario item7 = new EconomyDetails.Scenario("geysers/geysers_active_period_only", 1f, (EconomyDetails.Transformation t) => t.type == details.geyserActivePeriodTransformationType);
		list.Add(item7);
		EconomyDetails.Scenario item8 = new EconomyDetails.Scenario("geysers/geysers_whole_lifetime", 1f, (EconomyDetails.Transformation t) => t.type == details.geyserLifetimeTransformationType);
		list.Add(item8);
		EconomyDetails.Scenario scenario = new EconomyDetails.Scenario("oxygen/algae_distillery", 0f, null);
		scenario.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("AlgaeDistillery"), 3f));
		scenario.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("AlgaeHabitat"), 22f));
		scenario.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Duplicant"), 9f));
		scenario.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("WaterPurifier"), 1f));
		list.Add(scenario);
		EconomyDetails.Scenario scenario2 = new EconomyDetails.Scenario("oxygen/algae_habitat_electrolyzer", 0f, null);
		scenario2.AddEntry(new EconomyDetails.Scenario.Entry("AlgaeHabitat", 1f));
		scenario2.AddEntry(new EconomyDetails.Scenario.Entry("Duplicant", 1f));
		scenario2.AddEntry(new EconomyDetails.Scenario.Entry("Electrolyzer", 1f));
		list.Add(scenario2);
		EconomyDetails.Scenario scenario3 = new EconomyDetails.Scenario("oxygen/electrolyzer", 0f, null);
		scenario3.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Electrolyzer"), 1f));
		scenario3.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("LiquidPump"), 1f));
		scenario3.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Duplicant"), 9f));
		scenario3.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("HydrogenGenerator"), 1f));
		scenario3.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("GasPump"), 1f));
		list.Add(scenario3);
		EconomyDetails.Scenario scenario4 = new EconomyDetails.Scenario("purifiers/methane_generator", 0f, null);
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("MethaneGenerator"), 1f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("FertilizerMaker"), 3f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Electrolyzer"), 1f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("GasPump"), 1f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("LiquidPump"), 2f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("HydrogenGenerator"), 1f));
		scenario4.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("PrickleFlower"), 0f));
		list.Add(scenario4);
		EconomyDetails.Scenario scenario5 = new EconomyDetails.Scenario("purifiers/water_purifier", 0f, null);
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("WaterPurifier"), 1f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Compost"), 2f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Electrolyzer"), 1f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("LiquidPump"), 2f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("GasPump"), 1f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("HydrogenGenerator"), 1f));
		scenario5.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("PrickleFlower"), 29f));
		list.Add(scenario5);
		EconomyDetails.Scenario scenario6 = new EconomyDetails.Scenario("energy/petroleum_generator", 0f, null);
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("PetroleumGenerator"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("OilRefinery"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("WaterPurifier"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("LiquidPump"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("GasPump"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("CO2Scrubber"), 1f));
		scenario6.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("MethaneGenerator"), 1f));
		list.Add(scenario6);
		EconomyDetails.Scenario scenario7 = new EconomyDetails.Scenario("energy/coal_generator", 0f, (EconomyDetails.Transformation t) => t.tag.Name.Contains("Hatch"));
		scenario7.AddEntry(new EconomyDetails.Scenario.Entry("Generator", 1f));
		list.Add(scenario7);
		EconomyDetails.Scenario scenario8 = new EconomyDetails.Scenario("waste/outhouse", 0f, null);
		scenario8.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Outhouse"), 1f));
		scenario8.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Compost"), 1f));
		list.Add(scenario8);
		EconomyDetails.Scenario scenario9 = new EconomyDetails.Scenario("stress/massage_table", 0f, null);
		scenario9.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("MassageTable"), 1f));
		scenario9.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("ManualGenerator"), 1f));
		list.Add(scenario9);
		EconomyDetails.Scenario scenario10 = new EconomyDetails.Scenario("waste/flush_toilet", 0f, null);
		scenario10.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("FlushToilet"), 1f));
		scenario10.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("WaterPurifier"), 1f));
		scenario10.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("LiquidPump"), 1f));
		scenario10.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("FertilizerMaker"), 1f));
		list.Add(scenario10);
		EconomyDetails.CollectDietScenarios(list);
		foreach (EconomyDetails.Transformation transformation in details.transformations)
		{
			EconomyDetails.Transformation transformation_iter = transformation;
			EconomyDetails.Scenario item9 = new EconomyDetails.Scenario("transformations/" + transformation.tag.Name, 1f, (EconomyDetails.Transformation t) => transformation_iter == t);
			list.Add(item9);
		}
		foreach (EconomyDetails.Transformation transformation2 in details.transformations)
		{
			EconomyDetails.Scenario scenario11 = new EconomyDetails.Scenario("transformation_groups/" + transformation2.tag.Name, 0f, null);
			scenario11.AddEntry(new EconomyDetails.Scenario.Entry(transformation2.tag, 1f));
			foreach (EconomyDetails.Transformation transformation3 in details.transformations)
			{
				bool flag = false;
				foreach (EconomyDetails.Transformation.Delta delta in transformation2.deltas)
				{
					if (delta.resource.type != details.energyResourceType)
					{
						foreach (EconomyDetails.Transformation.Delta delta2 in transformation3.deltas)
						{
							if (delta.resource == delta2.resource)
							{
								scenario11.AddEntry(new EconomyDetails.Scenario.Entry(transformation3.tag, 0f));
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
			}
			list.Add(scenario11);
		}
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			EconomyDetails.Scenario scenario12 = new EconomyDetails.Scenario("food/" + foodInfo.Id, 0f, null);
			Tag tag2 = TagManager.Create(foodInfo.Id);
			scenario12.AddEntry(new EconomyDetails.Scenario.Entry(tag2, 1f));
			scenario12.AddEntry(new EconomyDetails.Scenario.Entry(TagManager.Create("Duplicant"), 1f));
			List<Tag> list2 = new List<Tag>();
			list2.Add(tag2);
			while (list2.Count > 0)
			{
				Tag tag = list2[0];
				list2.RemoveAt(0);
				ComplexRecipe complexRecipe = ComplexRecipeManager.Get().recipes.Find((ComplexRecipe a) => a.FirstResult == tag);
				if (complexRecipe != null)
				{
					foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
					{
						scenario12.AddEntry(new EconomyDetails.Scenario.Entry(recipeElement.material, 1f));
						list2.Add(recipeElement.material);
					}
				}
				foreach (KPrefabID kprefabID in Assets.Prefabs)
				{
					Crop component = kprefabID.GetComponent<Crop>();
					if (component != null && component.cropVal.cropId == tag.Name)
					{
						scenario12.AddEntry(new EconomyDetails.Scenario.Entry(kprefabID.PrefabTag, 1f));
						list2.Add(kprefabID.PrefabTag);
					}
				}
			}
			list.Add(scenario12);
		}
		if (!Directory.Exists("assets/Tuning/Economy"))
		{
			Directory.CreateDirectory("assets/Tuning/Economy");
		}
		foreach (EconomyDetails.Scenario scenario13 in list)
		{
			string path = "assets/Tuning/Economy/" + scenario13.name + ".csv";
			if (!Directory.Exists(System.IO.Path.GetDirectoryName(path)))
			{
				Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
			}
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				details.DumpTransformations(scenario13, streamWriter);
			}
		}
		float dupeBreathingPerSecond = details.GetDupeBreathingPerSecond(details);
		List<EconomyDetails.BiomeTransformation> list3 = new List<EconomyDetails.BiomeTransformation>();
		list3.Add(details.CreateBiomeTransformationFromTransformation(details, "MineralDeoxidizer".ToTag(), GameTags.Algae, GameTags.Oxygen));
		list3.Add(details.CreateBiomeTransformationFromTransformation(details, "AlgaeHabitat".ToTag(), GameTags.Algae, GameTags.Oxygen));
		list3.Add(details.CreateBiomeTransformationFromTransformation(details, "AlgaeHabitat".ToTag(), GameTags.Water, GameTags.Oxygen));
		list3.Add(details.CreateBiomeTransformationFromTransformation(details, "Electrolyzer".ToTag(), GameTags.Water, GameTags.Oxygen));
		list3.Add(new EconomyDetails.BiomeTransformation("StartingOxygenCycles".ToTag(), details.GetResource(GameTags.Oxygen), 1f / -(dupeBreathingPerSecond * 600f)));
		list3.Add(new EconomyDetails.BiomeTransformation("StartingOxyliteCycles".ToTag(), details.CreateResource(GameTags.OxyRock, details.massResourceType), 1f / -(dupeBreathingPerSecond * 600f)));
		string path2 = "assets/Tuning/Economy/biomes/starting_amounts.csv";
		if (!Directory.Exists(System.IO.Path.GetDirectoryName(path2)))
		{
			Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path2));
		}
		using (StreamWriter streamWriter2 = new StreamWriter(path2))
		{
			streamWriter2.Write("Resource,Amount");
			foreach (EconomyDetails.BiomeTransformation biomeTransformation in list3)
			{
				streamWriter2.Write("," + biomeTransformation.tag.ToString());
			}
			streamWriter2.Write("\n");
			streamWriter2.Write("Cells, " + details.startingBiomeCellCount.ToString() + "\n");
			foreach (KeyValuePair<Element, float> keyValuePair in details.startingBiomeAmounts)
			{
				streamWriter2.Write(keyValuePair.Key.id.ToString() + ", " + keyValuePair.Value.ToString());
				foreach (EconomyDetails.BiomeTransformation biomeTransformation2 in list3)
				{
					streamWriter2.Write(",");
					float num = biomeTransformation2.Transform(keyValuePair.Key, keyValuePair.Value);
					if (num > 0f)
					{
						streamWriter2.Write(num);
					}
				}
				streamWriter2.Write("\n");
			}
		}
		global::Debug.Log("Completed economy details dump!!");
	}

	// Token: 0x06003506 RID: 13574 RVA: 0x0011F928 File Offset: 0x0011DB28
	private static void DumpNameMapping()
	{
		string path = "assets/Tuning/Economy/name_mapping.csv";
		if (!Directory.Exists("assets/Tuning/Economy"))
		{
			Directory.CreateDirectory("assets/Tuning/Economy");
		}
		using (StreamWriter streamWriter = new StreamWriter(path))
		{
			streamWriter.Write("Game Name, Prefab Name, Anim Files\n");
			foreach (KPrefabID kprefabID in Assets.Prefabs)
			{
				string text = TagManager.StripLinkFormatting(kprefabID.GetProperName());
				Tag tag = kprefabID.PrefabID();
				if (!text.IsNullOrWhiteSpace() && !tag.Name.Contains("UnderConstruction") && !tag.Name.Contains("Preview"))
				{
					streamWriter.Write(text);
					TextWriter textWriter = streamWriter;
					string str = ",";
					Tag tag2 = tag;
					textWriter.Write(str + tag2.ToString());
					KAnimControllerBase component = kprefabID.GetComponent<KAnimControllerBase>();
					if (component != null)
					{
						foreach (KAnimFile kanimFile in component.AnimFiles)
						{
							streamWriter.Write("," + kanimFile.name);
						}
					}
					else
					{
						streamWriter.Write(",");
					}
					streamWriter.Write("\n");
				}
			}
		}
	}

	// Token: 0x04002000 RID: 8192
	private List<EconomyDetails.Transformation> transformations = new List<EconomyDetails.Transformation>();

	// Token: 0x04002001 RID: 8193
	private List<EconomyDetails.Resource> resources = new List<EconomyDetails.Resource>();

	// Token: 0x04002002 RID: 8194
	public Dictionary<Element, float> startingBiomeAmounts = new Dictionary<Element, float>();

	// Token: 0x04002003 RID: 8195
	public int startingBiomeCellCount;

	// Token: 0x04002004 RID: 8196
	public EconomyDetails.Resource energyResource;

	// Token: 0x04002005 RID: 8197
	public EconomyDetails.Resource heatResource;

	// Token: 0x04002006 RID: 8198
	public EconomyDetails.Resource duplicantTimeResource;

	// Token: 0x04002007 RID: 8199
	public EconomyDetails.Resource caloriesResource;

	// Token: 0x04002008 RID: 8200
	public EconomyDetails.Resource fixedCaloriesResource;

	// Token: 0x04002009 RID: 8201
	public EconomyDetails.Resource.Type massResourceType;

	// Token: 0x0400200A RID: 8202
	public EconomyDetails.Resource.Type heatResourceType;

	// Token: 0x0400200B RID: 8203
	public EconomyDetails.Resource.Type energyResourceType;

	// Token: 0x0400200C RID: 8204
	public EconomyDetails.Resource.Type timeResourceType;

	// Token: 0x0400200D RID: 8205
	public EconomyDetails.Resource.Type attributeResourceType;

	// Token: 0x0400200E RID: 8206
	public EconomyDetails.Resource.Type caloriesResourceType;

	// Token: 0x0400200F RID: 8207
	public EconomyDetails.Resource.Type amountResourceType;

	// Token: 0x04002010 RID: 8208
	public EconomyDetails.Transformation.Type buildingTransformationType;

	// Token: 0x04002011 RID: 8209
	public EconomyDetails.Transformation.Type foodTransformationType;

	// Token: 0x04002012 RID: 8210
	public EconomyDetails.Transformation.Type plantTransformationType;

	// Token: 0x04002013 RID: 8211
	public EconomyDetails.Transformation.Type creatureTransformationType;

	// Token: 0x04002014 RID: 8212
	public EconomyDetails.Transformation.Type dupeTransformationType;

	// Token: 0x04002015 RID: 8213
	public EconomyDetails.Transformation.Type referenceTransformationType;

	// Token: 0x04002016 RID: 8214
	public EconomyDetails.Transformation.Type effectTransformationType;

	// Token: 0x04002017 RID: 8215
	private const string GEYSER_ACTIVE_SUFFIX = "_ActiveOnly";

	// Token: 0x04002018 RID: 8216
	public EconomyDetails.Transformation.Type geyserActivePeriodTransformationType;

	// Token: 0x04002019 RID: 8217
	public EconomyDetails.Transformation.Type geyserLifetimeTransformationType;

	// Token: 0x0400201A RID: 8218
	private static string debugTag = "CO2Scrubber";

	// Token: 0x020014F9 RID: 5369
	public class Resource
	{
		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06008664 RID: 34404 RVA: 0x00308A6C File Offset: 0x00306C6C
		// (set) Token: 0x06008665 RID: 34405 RVA: 0x00308A74 File Offset: 0x00306C74
		public Tag tag { get; private set; }

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06008666 RID: 34406 RVA: 0x00308A7D File Offset: 0x00306C7D
		// (set) Token: 0x06008667 RID: 34407 RVA: 0x00308A85 File Offset: 0x00306C85
		public EconomyDetails.Resource.Type type { get; private set; }

		// Token: 0x06008668 RID: 34408 RVA: 0x00308A8E File Offset: 0x00306C8E
		public Resource(Tag tag, EconomyDetails.Resource.Type type)
		{
			this.tag = tag;
			this.type = type;
		}

		// Token: 0x02002174 RID: 8564
		public class Type
		{
			// Token: 0x17000A78 RID: 2680
			// (get) Token: 0x0600AA72 RID: 43634 RVA: 0x003745C7 File Offset: 0x003727C7
			// (set) Token: 0x0600AA73 RID: 43635 RVA: 0x003745CF File Offset: 0x003727CF
			public string id { get; private set; }

			// Token: 0x17000A79 RID: 2681
			// (get) Token: 0x0600AA74 RID: 43636 RVA: 0x003745D8 File Offset: 0x003727D8
			// (set) Token: 0x0600AA75 RID: 43637 RVA: 0x003745E0 File Offset: 0x003727E0
			public string unit { get; private set; }

			// Token: 0x0600AA76 RID: 43638 RVA: 0x003745E9 File Offset: 0x003727E9
			public Type(string id, string unit)
			{
				this.id = id;
				this.unit = unit;
			}
		}
	}

	// Token: 0x020014FA RID: 5370
	public class BiomeTransformation
	{
		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06008669 RID: 34409 RVA: 0x00308AA4 File Offset: 0x00306CA4
		// (set) Token: 0x0600866A RID: 34410 RVA: 0x00308AAC File Offset: 0x00306CAC
		public Tag tag { get; private set; }

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x0600866B RID: 34411 RVA: 0x00308AB5 File Offset: 0x00306CB5
		// (set) Token: 0x0600866C RID: 34412 RVA: 0x00308ABD File Offset: 0x00306CBD
		public EconomyDetails.Resource resource { get; private set; }

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x0600866D RID: 34413 RVA: 0x00308AC6 File Offset: 0x00306CC6
		// (set) Token: 0x0600866E RID: 34414 RVA: 0x00308ACE File Offset: 0x00306CCE
		public float ratio { get; private set; }

		// Token: 0x0600866F RID: 34415 RVA: 0x00308AD7 File Offset: 0x00306CD7
		public BiomeTransformation(Tag tag, EconomyDetails.Resource resource, float ratio)
		{
			this.tag = tag;
			this.resource = resource;
			this.ratio = ratio;
		}

		// Token: 0x06008670 RID: 34416 RVA: 0x00308AF4 File Offset: 0x00306CF4
		public float Transform(Element element, float amount)
		{
			if (this.resource.tag == element.tag)
			{
				return this.ratio * amount;
			}
			return 0f;
		}
	}

	// Token: 0x020014FB RID: 5371
	public class Ratio
	{
		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06008671 RID: 34417 RVA: 0x00308B1C File Offset: 0x00306D1C
		// (set) Token: 0x06008672 RID: 34418 RVA: 0x00308B24 File Offset: 0x00306D24
		public EconomyDetails.Resource input { get; private set; }

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06008673 RID: 34419 RVA: 0x00308B2D File Offset: 0x00306D2D
		// (set) Token: 0x06008674 RID: 34420 RVA: 0x00308B35 File Offset: 0x00306D35
		public EconomyDetails.Resource output { get; private set; }

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06008675 RID: 34421 RVA: 0x00308B3E File Offset: 0x00306D3E
		// (set) Token: 0x06008676 RID: 34422 RVA: 0x00308B46 File Offset: 0x00306D46
		public bool allowNegativeOutput { get; private set; }

		// Token: 0x06008677 RID: 34423 RVA: 0x00308B4F File Offset: 0x00306D4F
		public Ratio(EconomyDetails.Resource input, EconomyDetails.Resource output, bool allow_negative_output)
		{
			this.input = input;
			this.output = output;
			this.allowNegativeOutput = allow_negative_output;
		}
	}

	// Token: 0x020014FC RID: 5372
	public class Scenario
	{
		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06008678 RID: 34424 RVA: 0x00308B6C File Offset: 0x00306D6C
		// (set) Token: 0x06008679 RID: 34425 RVA: 0x00308B74 File Offset: 0x00306D74
		public string name { get; private set; }

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x0600867A RID: 34426 RVA: 0x00308B7D File Offset: 0x00306D7D
		// (set) Token: 0x0600867B RID: 34427 RVA: 0x00308B85 File Offset: 0x00306D85
		public float defaultCount { get; private set; }

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x0600867C RID: 34428 RVA: 0x00308B8E File Offset: 0x00306D8E
		// (set) Token: 0x0600867D RID: 34429 RVA: 0x00308B96 File Offset: 0x00306D96
		public float timeInSeconds { get; set; }

		// Token: 0x0600867E RID: 34430 RVA: 0x00308B9F File Offset: 0x00306D9F
		public Scenario(string name, float default_count, Func<EconomyDetails.Transformation, bool> filter)
		{
			this.name = name;
			this.defaultCount = default_count;
			this.filter = filter;
			this.timeInSeconds = 600f;
		}

		// Token: 0x0600867F RID: 34431 RVA: 0x00308BD2 File Offset: 0x00306DD2
		public void AddEntry(EconomyDetails.Scenario.Entry entry)
		{
			this.entries.Add(entry);
		}

		// Token: 0x06008680 RID: 34432 RVA: 0x00308BE0 File Offset: 0x00306DE0
		public float GetCount(Tag tag)
		{
			foreach (EconomyDetails.Scenario.Entry entry in this.entries)
			{
				if (entry.tag == tag)
				{
					return entry.count;
				}
			}
			return this.defaultCount;
		}

		// Token: 0x06008681 RID: 34433 RVA: 0x00308C4C File Offset: 0x00306E4C
		public bool IncludesTransformation(EconomyDetails.Transformation transformation)
		{
			if (this.filter != null && this.filter(transformation))
			{
				return true;
			}
			using (List<EconomyDetails.Scenario.Entry>.Enumerator enumerator = this.entries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.tag == transformation.tag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040066F3 RID: 26355
		private Func<EconomyDetails.Transformation, bool> filter;

		// Token: 0x040066F4 RID: 26356
		private List<EconomyDetails.Scenario.Entry> entries = new List<EconomyDetails.Scenario.Entry>();

		// Token: 0x02002175 RID: 8565
		public class Entry
		{
			// Token: 0x17000A7A RID: 2682
			// (get) Token: 0x0600AA77 RID: 43639 RVA: 0x003745FF File Offset: 0x003727FF
			// (set) Token: 0x0600AA78 RID: 43640 RVA: 0x00374607 File Offset: 0x00372807
			public Tag tag { get; private set; }

			// Token: 0x17000A7B RID: 2683
			// (get) Token: 0x0600AA79 RID: 43641 RVA: 0x00374610 File Offset: 0x00372810
			// (set) Token: 0x0600AA7A RID: 43642 RVA: 0x00374618 File Offset: 0x00372818
			public float count { get; private set; }

			// Token: 0x0600AA7B RID: 43643 RVA: 0x00374621 File Offset: 0x00372821
			public Entry(Tag tag, float count)
			{
				this.tag = tag;
				this.count = count;
			}
		}
	}

	// Token: 0x020014FD RID: 5373
	public class Transformation
	{
		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06008682 RID: 34434 RVA: 0x00308CC8 File Offset: 0x00306EC8
		// (set) Token: 0x06008683 RID: 34435 RVA: 0x00308CD0 File Offset: 0x00306ED0
		public Tag tag { get; private set; }

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06008684 RID: 34436 RVA: 0x00308CD9 File Offset: 0x00306ED9
		// (set) Token: 0x06008685 RID: 34437 RVA: 0x00308CE1 File Offset: 0x00306EE1
		public EconomyDetails.Transformation.Type type { get; private set; }

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06008686 RID: 34438 RVA: 0x00308CEA File Offset: 0x00306EEA
		// (set) Token: 0x06008687 RID: 34439 RVA: 0x00308CF2 File Offset: 0x00306EF2
		public float timeInSeconds { get; private set; }

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06008688 RID: 34440 RVA: 0x00308CFB File Offset: 0x00306EFB
		// (set) Token: 0x06008689 RID: 34441 RVA: 0x00308D03 File Offset: 0x00306F03
		public bool timeInvariant { get; private set; }

		// Token: 0x0600868A RID: 34442 RVA: 0x00308D0C File Offset: 0x00306F0C
		public Transformation(Tag tag, EconomyDetails.Transformation.Type type, float time_in_seconds, bool timeInvariant = false)
		{
			this.tag = tag;
			this.type = type;
			this.timeInSeconds = time_in_seconds;
			this.timeInvariant = timeInvariant;
		}

		// Token: 0x0600868B RID: 34443 RVA: 0x00308D3C File Offset: 0x00306F3C
		public void AddDelta(EconomyDetails.Transformation.Delta delta)
		{
			global::Debug.Assert(delta.resource != null);
			this.deltas.Add(delta);
		}

		// Token: 0x0600868C RID: 34444 RVA: 0x00308D58 File Offset: 0x00306F58
		public EconomyDetails.Transformation.Delta GetDelta(EconomyDetails.Resource resource)
		{
			foreach (EconomyDetails.Transformation.Delta delta in this.deltas)
			{
				if (delta.resource == resource)
				{
					return delta;
				}
			}
			return null;
		}

		// Token: 0x040066F6 RID: 26358
		public List<EconomyDetails.Transformation.Delta> deltas = new List<EconomyDetails.Transformation.Delta>();

		// Token: 0x02002176 RID: 8566
		public class Delta
		{
			// Token: 0x17000A7C RID: 2684
			// (get) Token: 0x0600AA7C RID: 43644 RVA: 0x00374637 File Offset: 0x00372837
			// (set) Token: 0x0600AA7D RID: 43645 RVA: 0x0037463F File Offset: 0x0037283F
			public EconomyDetails.Resource resource { get; private set; }

			// Token: 0x17000A7D RID: 2685
			// (get) Token: 0x0600AA7E RID: 43646 RVA: 0x00374648 File Offset: 0x00372848
			// (set) Token: 0x0600AA7F RID: 43647 RVA: 0x00374650 File Offset: 0x00372850
			public float amount { get; set; }

			// Token: 0x0600AA80 RID: 43648 RVA: 0x00374659 File Offset: 0x00372859
			public Delta(EconomyDetails.Resource resource, float amount)
			{
				this.resource = resource;
				this.amount = amount;
			}
		}

		// Token: 0x02002177 RID: 8567
		public class Type
		{
			// Token: 0x17000A7E RID: 2686
			// (get) Token: 0x0600AA81 RID: 43649 RVA: 0x0037466F File Offset: 0x0037286F
			// (set) Token: 0x0600AA82 RID: 43650 RVA: 0x00374677 File Offset: 0x00372877
			public string id { get; private set; }

			// Token: 0x0600AA83 RID: 43651 RVA: 0x00374680 File Offset: 0x00372880
			public Type(string id)
			{
				this.id = id;
			}
		}
	}
}
