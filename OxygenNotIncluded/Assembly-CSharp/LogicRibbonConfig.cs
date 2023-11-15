using System;
using TUNING;
using UnityEngine;

// Token: 0x0200022C RID: 556
public class LogicRibbonConfig : BaseLogicWireConfig
{
	// Token: 0x06000B2F RID: 2863 RVA: 0x0003F1B4 File Offset: 0x0003D3B4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LogicRibbon";
		string anim = "logic_ribbon_kanim";
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		EffectorValues none = NOISE_POLLUTION.NONE;
		return base.CreateBuildingDef(id, anim, construction_time, tier, BUILDINGS.DECOR.PENALTY.TIER0, none);
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x0003F1E7 File Offset: 0x0003D3E7
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(LogicWire.BitDepth.FourBit, go);
	}

	// Token: 0x04000689 RID: 1673
	public const string ID = "LogicRibbon";
}
