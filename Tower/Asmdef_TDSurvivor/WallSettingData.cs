using System;
using UnityEngine;

// Token: 0x02000040 RID: 64
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/WallSettingData", order = 1)]
public class WallSettingData : AItemSettingData
{
	// Token: 0x06000135 RID: 309 RVA: 0x000058A6 File Offset: 0x00003AA6
	public GameObject GetPrefab()
	{
		return this.wallPrefab;
	}

	// Token: 0x040000D6 RID: 214
	[SerializeField]
	[Header("牆壁的Prefab")]
	private GameObject wallPrefab;
}
