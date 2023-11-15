using System;

namespace UnityEngine.Rendering.Universal.PostProcessing
{
	// Token: 0x020000BC RID: 188
	[Flags]
	public enum InjectionPoint
	{
		// Token: 0x0400022E RID: 558
		AfterOpaqueAndSky = 1,
		// Token: 0x0400022F RID: 559
		BeforePostProcess = 2,
		// Token: 0x04000230 RID: 560
		AfterPostProcess = 4
	}
}
