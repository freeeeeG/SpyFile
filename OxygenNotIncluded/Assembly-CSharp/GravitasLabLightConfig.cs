using System;
using TUNING;
using UnityEngine;

// Token: 0x020001C4 RID: 452
public class GravitasLabLightConfig : IBuildingConfig
{
	// Token: 0x060008FE RID: 2302 RVA: 0x000351F0 File Offset: 0x000333F0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GravitasLabLight";
		int width = 1;
		int height = 1;
		string anim = "gravitas_lab_light_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.ShowInBuildMenu = false;
		buildingDef.Entombable = false;
		buildingDef.Floodable = false;
		buildingDef.Invincible = true;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0003525D File Offset: 0x0003345D
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.Gravitas);
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0003526A File Offset: 0x0003346A
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040005A3 RID: 1443
	public const string ID = "GravitasLabLight";
}
