﻿using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class EggIncubatorConfig : IBuildingConfig
{
	// Token: 0x060001F1 RID: 497 RVA: 0x0000D778 File Offset: 0x0000B978
	public override BuildingDef CreateBuildingDef()
	{
		string id = "EggIncubator";
		int width = 2;
		int height = 3;
		string anim = "incubator_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.OverheatTemperature = 363.15f;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
		return buildingDef;
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x0000D80C File Offset: 0x0000BA0C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		BuildingTemplates.CreateDefaultStorage(go, false).SetDefaultStoredItemModifiers(EggIncubatorConfig.IncubatorStorage);
		EggIncubator eggIncubator = go.AddOrGet<EggIncubator>();
		eggIncubator.AddDepositTag(GameTags.Egg);
		eggIncubator.SetWorkTime(5f);
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x0000D840 File Offset: 0x0000BA40
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000131 RID: 305
	public const string ID = "EggIncubator";

	// Token: 0x04000132 RID: 306
	public static readonly List<Storage.StoredItemModifier> IncubatorStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Preserve
	};
}
