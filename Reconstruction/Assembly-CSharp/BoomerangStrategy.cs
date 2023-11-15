using System;
using System.Collections.Generic;

// Token: 0x02000224 RID: 548
public class BoomerangStrategy : RefactorStrategy
{
	// Token: 0x06000D80 RID: 3456 RVA: 0x0002303F File Offset: 0x0002123F
	public BoomerangStrategy(TurretAttribute attribute, int quality, List<Composition> initCompositions = null) : base(attribute, quality, initCompositions)
	{
	}

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x06000D81 RID: 3457 RVA: 0x0002304A File Offset: 0x0002124A
	// (set) Token: 0x06000D82 RID: 3458 RVA: 0x00023052 File Offset: 0x00021252
	public bool UnfrostEffect { get; set; }
}
