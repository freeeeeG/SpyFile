﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000346 RID: 838
public class SolidVentConfig : IBuildingConfig
{
	// Token: 0x06001108 RID: 4360 RVA: 0x0005BCD0 File Offset: 0x00059ED0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidVent";
		int width = 1;
		int height = 1;
		string anim = "conveyer_dropper_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.InputConduitType = ConduitType.Solid;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidVent");
		return buildingDef;
	}

	// Token: 0x06001109 RID: 4361 RVA: 0x0005BD7C File Offset: 0x00059F7C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x0600110A RID: 4362 RVA: 0x0005BD85 File Offset: 0x00059F85
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
	}

	// Token: 0x0600110B RID: 4363 RVA: 0x0005BDAD File Offset: 0x00059FAD
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<SimpleVent>();
		go.AddOrGet<SolidConduitConsumer>();
		go.AddOrGet<SolidConduitDropper>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.capacityKg = 100f;
		storage.showInUI = true;
	}

	// Token: 0x04000940 RID: 2368
	public const string ID = "SolidVent";

	// Token: 0x04000941 RID: 2369
	private const ConduitType CONDUIT_TYPE = ConduitType.Solid;
}
