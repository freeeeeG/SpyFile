using System;
using TUNING;
using UnityEngine;

// Token: 0x0200037D RID: 893
public class WireConfig : BaseWireConfig
{
	// Token: 0x0600126F RID: 4719 RVA: 0x00063498 File Offset: 0x00061698
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Wire";
		string anim = "utilities_electric_kanim";
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		return base.CreateBuildingDef(id, anim, construction_time, tier, insulation, BUILDINGS.DECOR.PENALTY.TIER0, none);
	}

	// Token: 0x06001270 RID: 4720 RVA: 0x000634D0 File Offset: 0x000616D0
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max1000, go);
	}

	// Token: 0x040009FB RID: 2555
	public const string ID = "Wire";
}
