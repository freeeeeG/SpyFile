using System;
using UnityEngine;

// Token: 0x020002B4 RID: 692
public interface IOreConfig
{
	// Token: 0x1700003B RID: 59
	// (get) Token: 0x06000E21 RID: 3617
	SimHashes ElementID { get; }

	// Token: 0x06000E22 RID: 3618
	GameObject CreatePrefab();
}
