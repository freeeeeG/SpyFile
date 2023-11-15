using System;
using UnityEngine;

// Token: 0x020004FF RID: 1279
[AddComponentMenu("KMonoBehaviour/scripts/ScannerNetworkVisualizer")]
public class ScannerNetworkVisualizer : KMonoBehaviour
{
	// Token: 0x06001E23 RID: 7715 RVA: 0x000A1003 File Offset: 0x0009F203
	protected override void OnSpawn()
	{
		Components.ScannerVisualizers.Add(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x06001E24 RID: 7716 RVA: 0x000A101B File Offset: 0x0009F21B
	protected override void OnCleanUp()
	{
		Components.ScannerVisualizers.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x040010E1 RID: 4321
	public Vector2I OriginOffset = new Vector2I(0, 0);

	// Token: 0x040010E2 RID: 4322
	public int RangeMin;

	// Token: 0x040010E3 RID: 4323
	public int RangeMax;
}
