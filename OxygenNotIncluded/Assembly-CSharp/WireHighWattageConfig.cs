using System;
using TUNING;
using UnityEngine;

// Token: 0x0200037E RID: 894
public class WireHighWattageConfig : BaseWireConfig
{
	// Token: 0x06001272 RID: 4722 RVA: 0x000634E4 File Offset: 0x000616E4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HighWattageWire";
		string anim = "utilities_electric_insulated_kanim";
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, BUILDINGS.DECOR.PENALTY.TIER5, none);
		buildingDef.BuildLocationRule = BuildLocationRule.NotInTiles;
		return buildingDef;
	}

	// Token: 0x06001273 RID: 4723 RVA: 0x00063523 File Offset: 0x00061723
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max20000, go);
	}

	// Token: 0x06001274 RID: 4724 RVA: 0x0006352D File Offset: 0x0006172D
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
	}

	// Token: 0x040009FC RID: 2556
	public const string ID = "HighWattageWire";
}
