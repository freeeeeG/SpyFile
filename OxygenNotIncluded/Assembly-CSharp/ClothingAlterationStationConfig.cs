﻿using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000039 RID: 57
public class ClothingAlterationStationConfig : IBuildingConfig
{
	// Token: 0x06000109 RID: 265 RVA: 0x00008150 File Offset: 0x00006350
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ClothingAlterationStation";
		int width = 4;
		int height = 3;
		string anim = "super_snazzy_suit_alteration_station_kanim";
		int hitpoints = 100;
		float construction_time = 240f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		return buildingDef;
	}

	// Token: 0x0600010A RID: 266 RVA: 0x000081CC File Offset: 0x000063CC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<DropAllWorkable>();
		Prioritizable.AddRef(go);
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.outputOffset = new Vector3(1f, 0f, 0f);
		ComplexFabricatorWorkable complexFabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
		complexFabricatorWorkable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_super_snazzy_suit_alteration_station_kanim")
		};
		complexFabricatorWorkable.workingPstComplete = new HashedString[]
		{
			"working_pst_complete"
		};
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		this.ConfigureRecipes();
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00008278 File Offset: 0x00006478
	private void ConfigureRecipes()
	{
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement("Funky_Vest".ToTag(), 1f, false),
			new ComplexRecipe.RecipeElement("BasicFabric".ToTag(), 3f)
		};
		foreach (EquippableFacadeResource equippableFacadeResource in Db.GetEquippableFacades().resources.FindAll((EquippableFacadeResource match) => match.DefID == "CustomClothing"))
		{
			ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("CustomClothing".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, equippableFacadeResource.Id, false)
			};
			ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ClothingAlterationStation", array, array2, equippableFacadeResource.Id), array, array2);
			complexRecipe.time = TUNING.EQUIPMENT.VESTS.CUSTOM_CLOTHING_FABTIME;
			complexRecipe.description = STRINGS.EQUIPMENT.PREFABS.CUSTOMCLOTHING.RECIPE_DESC;
			complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
			complexRecipe.fabricators = new List<Tag>
			{
				"ClothingAlterationStation"
			};
			complexRecipe.sortOrder = 1;
		}
	}

	// Token: 0x0600010C RID: 268 RVA: 0x000083AC File Offset: 0x000065AC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
			component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
			component.AttributeConverter = Db.Get().AttributeConverters.ArtSpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Art.Id;
			component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
			component.requiredSkillPerk = Db.Get().SkillPerks.CanClothingAlteration.Id;
			game_object.GetComponent<ComplexFabricator>().choreType = Db.Get().ChoreTypes.Art;
		};
	}

	// Token: 0x040000A0 RID: 160
	public const string ID = "ClothingAlterationStation";
}
