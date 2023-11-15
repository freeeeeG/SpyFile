using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000363 RID: 867
public class TeleportalPadConfig : IBuildingConfig
{
	// Token: 0x060011B8 RID: 4536 RVA: 0x0005FFE4 File Offset: 0x0005E1E4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "TeleportalPad";
		int width = 4;
		int height = 4;
		string anim = "hqbase_kanim";
		int hitpoints = 250;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER5, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.BaseTimeUntilRepair = 400f;
		buildingDef.DefaultAnimState = "idle";
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(2, 0);
		buildingDef.ShowInBuildMenu = false;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort("TeleportalPad_ID_PORT_0", new CellOffset(-1, 0), STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT_INACTIVE, false, false),
			LogicPorts.Port.InputPort("TeleportalPad_ID_PORT_1", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT_INACTIVE, false, false),
			LogicPorts.Port.InputPort("TeleportalPad_ID_PORT_2", new CellOffset(1, 0), STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT_INACTIVE, false, false),
			LogicPorts.Port.InputPort("TeleportalPad_ID_PORT_3", new CellOffset(2, 0), STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT_INACTIVE, false, false),
			LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(-1, 1), STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.TELEPORTALPAD.LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.EnergyConsumptionWhenActive = 1600f;
		buildingDef.ExhaustKilowattsWhenActive = 16f;
		buildingDef.SelfHeatKilowattsWhenActive = 64f;
		SoundEventVolumeCache.instance.AddVolume("hqbase_kanim", "Portal_LP", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("hqbase_kanim", "Portal_open", NOISE_POLLUTION.NOISY.TIER4);
		SoundEventVolumeCache.instance.AddVolume("hqbase_kanim", "Portal_close", NOISE_POLLUTION.NOISY.TIER4);
		return buildingDef;
	}

	// Token: 0x060011B9 RID: 4537 RVA: 0x00060215 File Offset: 0x0005E415
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<TeleportalPad>();
		go.AddOrGet<Teleporter>();
		go.AddOrGet<PrimaryElement>().SetElement(SimHashes.Unobtanium, true);
	}

	// Token: 0x060011BA RID: 4538 RVA: 0x00060236 File Offset: 0x0005E436
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x040009A1 RID: 2465
	public const string ID = "TeleportalPad";

	// Token: 0x040009A2 RID: 2466
	public const string PORTAL_ID_PORT_0 = "TeleportalPad_ID_PORT_0";

	// Token: 0x040009A3 RID: 2467
	public const string PORTAL_ID_PORT_1 = "TeleportalPad_ID_PORT_1";

	// Token: 0x040009A4 RID: 2468
	public const string PORTAL_ID_PORT_2 = "TeleportalPad_ID_PORT_2";

	// Token: 0x040009A5 RID: 2469
	public const string PORTAL_ID_PORT_3 = "TeleportalPad_ID_PORT_3";
}
