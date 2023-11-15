using System;
using UnityEngine;

// Token: 0x02000505 RID: 1285
[AddComponentMenu("KMonoBehaviour/scripts/SkyVisibilityVisualizer")]
public class SkyVisibilityVisualizer : KMonoBehaviour
{
	// Token: 0x06001E38 RID: 7736 RVA: 0x000A131E File Offset: 0x0009F51E
	private static bool HasSkyVisibility(int cell)
	{
		return Grid.ExposedToSunlight[cell] >= 1;
	}

	// Token: 0x040010EB RID: 4331
	public Vector2I OriginOffset = new Vector2I(0, 0);

	// Token: 0x040010EC RID: 4332
	public bool TwoWideOrgin;

	// Token: 0x040010ED RID: 4333
	public int RangeMin;

	// Token: 0x040010EE RID: 4334
	public int RangeMax;

	// Token: 0x040010EF RID: 4335
	public int ScanVerticalStep;

	// Token: 0x040010F0 RID: 4336
	public bool SkipOnModuleInteriors;

	// Token: 0x040010F1 RID: 4337
	public bool AllOrNothingVisibility;

	// Token: 0x040010F2 RID: 4338
	public Func<int, bool> SkyVisibilityCb = new Func<int, bool>(SkyVisibilityVisualizer.HasSkyVisibility);
}
