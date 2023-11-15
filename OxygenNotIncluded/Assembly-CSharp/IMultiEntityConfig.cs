using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000793 RID: 1939
public interface IMultiEntityConfig
{
	// Token: 0x060035F5 RID: 13813
	List<GameObject> CreatePrefabs();

	// Token: 0x060035F6 RID: 13814
	void OnPrefabInit(GameObject inst);

	// Token: 0x060035F7 RID: 13815
	void OnSpawn(GameObject inst);
}
