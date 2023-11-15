using System;
using UnityEngine;

// Token: 0x020004FA RID: 1274
public interface IRottable
{
	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06001DF6 RID: 7670
	GameObject gameObject { get; }

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x06001DF7 RID: 7671
	float RotTemperature { get; }

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06001DF8 RID: 7672
	float PreserveTemperature { get; }
}
