using System;
using TUNING;
using UnityEngine;

// Token: 0x02000382 RID: 898
public class WireRefinedHighWattageConfig : BaseWireConfig
{
	// Token: 0x06001282 RID: 4738 RVA: 0x000636C0 File Offset: 0x000618C0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WireRefinedHighWattage";
		string anim = "utilities_electric_conduct_hiwatt_kanim";
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, BUILDINGS.DECOR.PENALTY.TIER3, none);
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		buildingDef.BuildLocationRule = BuildLocationRule.NotInTiles;
		return buildingDef;
	}

	// Token: 0x06001283 RID: 4739 RVA: 0x0006370A File Offset: 0x0006190A
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max50000, go);
	}

	// Token: 0x06001284 RID: 4740 RVA: 0x00063714 File Offset: 0x00061914
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
	}

	// Token: 0x04000A00 RID: 2560
	public const string ID = "WireRefinedHighWattage";
}
