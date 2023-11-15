using System;
using TUNING;
using UnityEngine;

// Token: 0x020001C0 RID: 448
public class GravitasContainerConfig : IBuildingConfig
{
	// Token: 0x060008DA RID: 2266 RVA: 0x00034700 File Offset: 0x00032900
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GravitasContainer";
		int width = 2;
		int height = 2;
		string anim = "gravitas_container_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.ShowInBuildMenu = false;
		buildingDef.Entombable = false;
		buildingDef.Floodable = false;
		buildingDef.Invincible = true;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x0003476D File Offset: 0x0003296D
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.Gravitas);
		go.AddOrGet<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.Building;
		Prioritizable.AddRef(go);
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x00034790 File Offset: 0x00032990
	public override void DoPostConfigureComplete(GameObject go)
	{
		PajamaDispenser pajamaDispenser = go.AddComponent<PajamaDispenser>();
		pajamaDispenser.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gravitas_container_kanim")
		};
		pajamaDispenser.SetWorkTime(30f);
	}

	// Token: 0x04000595 RID: 1429
	public const string ID = "GravitasContainer";

	// Token: 0x04000596 RID: 1430
	private const float WORK_TIME = 1.5f;
}
