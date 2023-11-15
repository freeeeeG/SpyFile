using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200023E RID: 574
public class ManualHighEnergyParticleSpawnerConfig : IBuildingConfig
{
	// Token: 0x06000B8E RID: 2958 RVA: 0x00040C4B File Offset: 0x0003EE4B
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x00040C54 File Offset: 0x0003EE54
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ManualHighEnergyParticleSpawner";
		int width = 1;
		int height = 3;
		string anim = "manual_radbolt_generator_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.UseHighEnergyParticleOutputPort = true;
		buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 2);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "ManualHighEnergyParticleSpawner");
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		return buildingDef;
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x00040D04 File Offset: 0x0003EF04
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		go.AddOrGet<HighEnergyParticleStorage>();
		go.AddOrGet<LoopingSounds>();
		ComplexFabricator complexFabricator = go.AddOrGet<ComplexFabricator>();
		complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		complexFabricator.duplicantOperated = true;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		ComplexFabricatorWorkable complexFabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);
		complexFabricatorWorkable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_manual_radbolt_generator_kanim")
		};
		complexFabricatorWorkable.workLayer = Grid.SceneLayer.BuildingUse;
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.UraniumOre.CreateTag(), 1f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(ManualHighEnergyParticleSpawnerConfig.WASTE_MATERIAL, 0.5f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ManualHighEnergyParticleSpawner", array, array2), array, array2, 0, 5);
		complexRecipe.time = 40f;
		complexRecipe.description = string.Format(STRINGS.BUILDINGS.PREFABS.MANUALHIGHENERGYPARTICLESPAWNER.RECIPE_DESCRIPTION, SimHashes.UraniumOre.CreateTag().ProperName(), ManualHighEnergyParticleSpawnerConfig.WASTE_MATERIAL.ProperName());
		complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.HEP;
		complexRecipe.fabricators = new List<Tag>
		{
			TagManager.Create("ManualHighEnergyParticleSpawner")
		};
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(SimHashes.EnrichedUranium.CreateTag(), 1f)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(ManualHighEnergyParticleSpawnerConfig.WASTE_MATERIAL, 0.8f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		ComplexRecipe complexRecipe2 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("ManualHighEnergyParticleSpawner", array3, array4), array3, array4, 0, 25);
		complexRecipe2.time = 40f;
		complexRecipe2.description = string.Format(STRINGS.BUILDINGS.PREFABS.MANUALHIGHENERGYPARTICLESPAWNER.RECIPE_DESCRIPTION, SimHashes.EnrichedUranium.CreateTag().ProperName(), ManualHighEnergyParticleSpawnerConfig.WASTE_MATERIAL.ProperName());
		complexRecipe2.nameDisplay = ComplexRecipe.RecipeNameDisplay.HEP;
		complexRecipe2.fabricators = new List<Tag>
		{
			TagManager.Create("ManualHighEnergyParticleSpawner")
		};
		go.AddOrGet<ManualHighEnergyParticleSpawner>();
		RadiationEmitter radiationEmitter = go.AddComponent<RadiationEmitter>();
		radiationEmitter.emissionOffset = new Vector3(0f, 2f);
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.emitRadiusX = this.RAD_LIGHT_SIZE;
		radiationEmitter.emitRadiusY = this.RAD_LIGHT_SIZE;
		radiationEmitter.emitRads = 120f;
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x00040F38 File Offset: 0x0003F138
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040006BD RID: 1725
	public const string ID = "ManualHighEnergyParticleSpawner";

	// Token: 0x040006BE RID: 1726
	public const float MIN_LAUNCH_INTERVAL = 2f;

	// Token: 0x040006BF RID: 1727
	public const int MIN_SLIDER = 1;

	// Token: 0x040006C0 RID: 1728
	public const int MAX_SLIDER = 100;

	// Token: 0x040006C1 RID: 1729
	public const float RADBOLTS_PER_KG = 5f;

	// Token: 0x040006C2 RID: 1730
	public const float MASS_PER_CRAFT = 1f;

	// Token: 0x040006C3 RID: 1731
	public const float REFINED_BONUS = 5f;

	// Token: 0x040006C4 RID: 1732
	public const int RADBOLTS_PER_CRAFT = 5;

	// Token: 0x040006C5 RID: 1733
	public static readonly Tag WASTE_MATERIAL = SimHashes.DepletedUranium.CreateTag();

	// Token: 0x040006C6 RID: 1734
	private const float ORE_FUEL_TO_WASTE_RATIO = 0.5f;

	// Token: 0x040006C7 RID: 1735
	private const float REFINED_FUEL_TO_WASTE_RATIO = 0.8f;

	// Token: 0x040006C8 RID: 1736
	private short RAD_LIGHT_SIZE = 3;
}
