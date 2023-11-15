using System;
using System.Collections.Generic;

// Token: 0x02000226 RID: 550
public class ConstructorStrategy : RefactorStrategy
{
	// Token: 0x06000D87 RID: 3463 RVA: 0x0002307E File Offset: 0x0002127E
	public ConstructorStrategy(TurretAttribute attribute, int quality, List<Composition> initCompositions = null) : base(attribute, quality, initCompositions)
	{
	}
}
