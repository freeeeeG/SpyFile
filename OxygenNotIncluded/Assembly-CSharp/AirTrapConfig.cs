﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000585 RID: 1413
public class AirTrapConfig : IBuildingConfig
{
	// Token: 0x06002232 RID: 8754 RVA: 0x000BBDA4 File Offset: 0x000B9FA4
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CreatureAirTrap", 1, 2, "critter_trap_air_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.RAW_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("TRAP_HAS_PREY_STATUS_PORT", new CellOffset(0, 1), STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x06002233 RID: 8755 RVA: 0x000BBE7C File Offset: 0x000BA07C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(go);
		go.AddOrGet<ArmTrapWorkable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_critter_trap_air_kanim")
		};
		go.AddOrGet<Operational>();
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = true;
		storage.SetDefaultStoredItemModifiers(AirTrapConfig.StoredItemModifiers);
		storage.sendOnStoreOnSpawn = true;
		TrapTrigger trapTrigger = go.AddOrGet<TrapTrigger>();
		trapTrigger.trappableCreatures = new Tag[]
		{
			GameTags.Creatures.Flyer
		};
		trapTrigger.trappedOffset = new Vector2(0f, 1f);
		ReusableTrap.Def def = go.AddOrGetDef<ReusableTrap.Def>();
		def.releaseCellOffset = new CellOffset(0, 1);
		def.OUTPUT_LOGIC_PORT_ID = "TRAP_HAS_PREY_STATUS_PORT";
		def.lures = new Tag[]
		{
			GameTags.Creatures.FlyersLure
		};
		go.AddOrGet<LogicPorts>();
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x06002234 RID: 8756 RVA: 0x000BBF55 File Offset: 0x000BA155
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06002235 RID: 8757 RVA: 0x000BBF58 File Offset: 0x000BA158
	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.DoPostConfigure(go);
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddOrGet<SymbolOverrideController>().applySymbolOverridesEveryFrame = true;
		Lure.Def def = go.AddOrGetDef<Lure.Def>();
		def.defaultLurePoints = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		def.radius = 32;
		Prioritizable.AddRef(go);
	}

	// Token: 0x04001376 RID: 4982
	public const string ID = "CreatureAirTrap";

	// Token: 0x04001377 RID: 4983
	public const string OUTPUT_LOGIC_PORT_ID = "TRAP_HAS_PREY_STATUS_PORT";

	// Token: 0x04001378 RID: 4984
	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>();
}
