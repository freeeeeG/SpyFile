using System;
using System.Collections.Generic;

// Token: 0x0200022B RID: 555
public class RotaryStrategy : RefactorStrategy
{
	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x06000D95 RID: 3477 RVA: 0x000235CE File Offset: 0x000217CE
	public override float FinalSplashRange
	{
		get
		{
			return (float)this.FinalRange;
		}
	}

	// Token: 0x06000D96 RID: 3478 RVA: 0x000235D7 File Offset: 0x000217D7
	public RotaryStrategy(TurretAttribute attribute, int quality, List<Composition> initCompositions = null) : base(attribute, quality, initCompositions)
	{
	}
}
