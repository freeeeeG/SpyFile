using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200033D RID: 829
public class SolidConduitInboxConfig : IBuildingConfig
{
	// Token: 0x060010D9 RID: 4313 RVA: 0x0005B128 File Offset: 0x00059328
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidConduitInbox";
		int width = 1;
		int height = 2;
		string anim = "conveyorin_kanim";
		int hitpoints = 100;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.OutputConduitType = ConduitType.Solid;
		buildingDef.PowerInputOffset = new CellOffset(0, 1);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidConduitInbox");
		return buildingDef;
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x0005B1FC File Offset: 0x000593FC
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x0005B220 File Offset: 0x00059420
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		Prioritizable.AddRef(go);
		go.AddOrGet<EnergyConsumer>();
		go.AddOrGet<Automatable>();
		List<Tag> list = new List<Tag>();
		list.AddRange(STORAGEFILTERS.NOT_EDIBLE_SOLIDS);
		list.AddRange(STORAGEFILTERS.FOOD);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.storageFilters = list;
		storage.allowItemRemoval = false;
		storage.onlyTransferFromLowerPriority = true;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		go.AddOrGet<TreeFilterable>();
		go.AddOrGet<SolidConduitInbox>();
		go.AddOrGet<SolidConduitDispenser>();
	}

	// Token: 0x04000932 RID: 2354
	public const string ID = "SolidConduitInbox";
}
