﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000371 RID: 881
public class VerticalWindTunnelConfig : IBuildingConfig
{
	// Token: 0x0600122A RID: 4650 RVA: 0x00061D8C File Offset: 0x0005FF8C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "VerticalWindTunnel";
		int width = 5;
		int height = 6;
		string anim = "wind_tunnel_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER6;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 1200f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		return buildingDef;
	}

	// Token: 0x0600122B RID: 4651 RVA: 0x00061E20 File Offset: 0x00060020
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		VerticalWindTunnel verticalWindTunnel = go.AddOrGet<VerticalWindTunnel>();
		verticalWindTunnel.specificEffect = "VerticalWindTunnel";
		verticalWindTunnel.trackingEffect = "RecentlyVerticalWindTunnel";
		verticalWindTunnel.basePriority = RELAXATION.PRIORITY.TIER4;
		verticalWindTunnel.displacementAmount_DescriptorOnly = 3f;
		ElementConsumer elementConsumer = go.AddComponent<ElementConsumer>();
		elementConsumer.configuration = ElementConsumer.Configuration.AllGas;
		elementConsumer.consumptionRate = 3f;
		elementConsumer.storeOnConsume = false;
		elementConsumer.showInStatusPanel = false;
		elementConsumer.consumptionRadius = 2;
		elementConsumer.sampleCellOffset = new Vector3(0f, -2f, 0f);
		elementConsumer.showDescriptor = false;
		ElementConsumer elementConsumer2 = go.AddComponent<ElementConsumer>();
		elementConsumer2.configuration = ElementConsumer.Configuration.AllGas;
		elementConsumer2.consumptionRate = 3f;
		elementConsumer2.storeOnConsume = false;
		elementConsumer2.showInStatusPanel = false;
		elementConsumer2.consumptionRadius = 2;
		elementConsumer2.sampleCellOffset = new Vector3(0f, 6f, 0f);
		elementConsumer2.showDescriptor = false;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x0600122C RID: 4652 RVA: 0x00061F36 File Offset: 0x00060136
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040009D7 RID: 2519
	public const string ID = "VerticalWindTunnel";

	// Token: 0x040009D8 RID: 2520
	private const float DISPLACEMENT_AMOUNT = 3f;
}
