using System;
using System.Collections.Generic;

// Token: 0x02000229 RID: 553
public class FirerStrategy : RefactorStrategy
{
	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x06000D8A RID: 3466 RVA: 0x0002309E File Offset: 0x0002129E
	public override int FinalRange
	{
		get
		{
			return base.InitRange;
		}
	}

	// Token: 0x06000D8B RID: 3467 RVA: 0x000230A6 File Offset: 0x000212A6
	public FirerStrategy(TurretAttribute attribute, int quality, List<Composition> initCompositions = null) : base(attribute, quality, initCompositions)
	{
	}
}
