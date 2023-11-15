using System;
using TUNING;
using UnityEngine;

// Token: 0x02000323 RID: 803
public class RoleStationConfig : IBuildingConfig
{
	// Token: 0x06001059 RID: 4185 RVA: 0x00058920 File Offset: 0x00056B20
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RoleStation";
		int width = 2;
		int height = 2;
		string anim = "job_station_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x0005898A File Offset: 0x00056B8A
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x0005899E File Offset: 0x00056B9E
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040008FE RID: 2302
	public const string ID = "RoleStation";
}
