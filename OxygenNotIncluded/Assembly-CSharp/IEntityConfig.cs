using System;
using UnityEngine;

// Token: 0x02000792 RID: 1938
public interface IEntityConfig
{
	// Token: 0x060035F1 RID: 13809
	GameObject CreatePrefab();

	// Token: 0x060035F2 RID: 13810
	void OnPrefabInit(GameObject inst);

	// Token: 0x060035F3 RID: 13811
	void OnSpawn(GameObject inst);

	// Token: 0x060035F4 RID: 13812
	string[] GetDlcIds();
}
