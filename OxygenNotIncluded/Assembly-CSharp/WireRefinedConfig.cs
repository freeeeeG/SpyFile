using System;
using TUNING;
using UnityEngine;

// Token: 0x02000381 RID: 897
public class WireRefinedConfig : BaseWireConfig
{
	// Token: 0x0600127F RID: 4735 RVA: 0x00063668 File Offset: 0x00061868
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WireRefined";
		string anim = "utilities_electric_conduct_kanim";
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, BUILDINGS.DECOR.NONE, none);
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		return buildingDef;
	}

	// Token: 0x06001280 RID: 4736 RVA: 0x000636AB File Offset: 0x000618AB
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max2000, go);
	}

	// Token: 0x040009FF RID: 2559
	public const string ID = "WireRefined";
}
