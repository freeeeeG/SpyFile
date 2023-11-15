using System;
using TUNING;
using UnityEngine;

// Token: 0x02000054 RID: 84
public class CornerMouldingConfig : IBuildingConfig
{
	// Token: 0x0600017D RID: 381 RVA: 0x0000B028 File Offset: 0x00009228
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CornerMoulding";
		int width = 1;
		int height = 1;
		string anim = "corner_tile_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.InCorner;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, new EffectorValues
		{
			amount = 5,
			radius = 3
		}, none, 0.2f);
		buildingDef.DefaultAnimState = "corner";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0000B0C3 File Offset: 0x000092C3
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000B0D6 File Offset: 0x000092D6
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040000EF RID: 239
	public const string ID = "CornerMoulding";
}
