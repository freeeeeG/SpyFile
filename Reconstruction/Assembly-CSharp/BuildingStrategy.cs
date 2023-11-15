using System;
using System.Collections.Generic;

// Token: 0x02000225 RID: 549
public class BuildingStrategy : RefactorStrategy
{
	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x06000D83 RID: 3459 RVA: 0x0002305B File Offset: 0x0002125B
	public override int FinalRange
	{
		get
		{
			return base.InitRange;
		}
	}

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x06000D84 RID: 3460 RVA: 0x00023063 File Offset: 0x00021263
	public override float FinalFireRate
	{
		get
		{
			return base.InitFireRate;
		}
	}

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x06000D85 RID: 3461 RVA: 0x0002306B File Offset: 0x0002126B
	public override float FinalSplashRange
	{
		get
		{
			return base.InitSplashRange;
		}
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x00023073 File Offset: 0x00021273
	public BuildingStrategy(TurretAttribute attribute, int quality, List<Composition> initCompositions = null) : base(attribute, quality, initCompositions)
	{
	}
}
