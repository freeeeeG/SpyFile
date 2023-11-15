using System;
using TUNING;
using UnityEngine;

// Token: 0x02000235 RID: 565
public class LogicWireConfig : BaseLogicWireConfig
{
	// Token: 0x06000B57 RID: 2903 RVA: 0x0003FD58 File Offset: 0x0003DF58
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LogicWire";
		string anim = "logic_wires_kanim";
		float construction_time = 3f;
		float[] tier_TINY = BUILDINGS.CONSTRUCTION_MASS_KG.TIER_TINY;
		EffectorValues none = NOISE_POLLUTION.NONE;
		return base.CreateBuildingDef(id, anim, construction_time, tier_TINY, BUILDINGS.DECOR.PENALTY.TIER0, none);
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x0003FD8B File Offset: 0x0003DF8B
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(LogicWire.BitDepth.OneBit, go);
	}

	// Token: 0x04000694 RID: 1684
	public const string ID = "LogicWire";
}
