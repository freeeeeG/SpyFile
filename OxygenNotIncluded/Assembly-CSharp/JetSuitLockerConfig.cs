using System;
using TUNING;
using UnityEngine;

// Token: 0x020001E8 RID: 488
public class JetSuitLockerConfig : IBuildingConfig
{
	// Token: 0x060009C1 RID: 2497 RVA: 0x00038748 File Offset: 0x00036948
	public override BuildingDef CreateBuildingDef()
	{
		string id = "JetSuitLocker";
		int width = 2;
		int height = 4;
		string anim = "changingarea_jetsuit_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float[] construction_mass = new float[]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
		};
		string[] construction_materials = refined_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "JetSuitLocker");
		return buildingDef;
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x000387D7 File Offset: 0x000369D7
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.secondaryInputPort;
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x000387EC File Offset: 0x000369EC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<SuitLocker>().OutfitTags = new Tag[]
		{
			GameTags.JetSuit
		};
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.capacityKG = 200f;
		go.AddComponent<JetSuitLocker>().portInfo = this.secondaryInputPort;
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("JetSuitLocker"),
			new Tag("JetSuitMarker")
		};
		go.AddOrGet<Storage>().capacityKg = 500f;
		Prioritizable.AddRef(go);
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x000388B5 File Offset: 0x00036AB5
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPort(go);
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x000388C6 File Offset: 0x00036AC6
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPort(go);
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x000388D6 File Offset: 0x00036AD6
	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
	}

	// Token: 0x04000616 RID: 1558
	public const string ID = "JetSuitLocker";

	// Token: 0x04000617 RID: 1559
	public const float O2_CAPACITY = 200f;

	// Token: 0x04000618 RID: 1560
	public const float SUIT_CAPACITY = 200f;

	// Token: 0x04000619 RID: 1561
	private ConduitPortInfo secondaryInputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 1));
}
