using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/TetrisSettingData", order = 1)]
public class TetrisSettingData : ScriptableObject
{
	// Token: 0x06000110 RID: 272 RVA: 0x000052CA File Offset: 0x000034CA
	public eTetrisType GetTetrisType()
	{
		return this.tetrisType;
	}

	// Token: 0x06000111 RID: 273 RVA: 0x000052D2 File Offset: 0x000034D2
	public GameObject GetPrefab()
	{
		return this.tetrisPrefab;
	}

	// Token: 0x040000BE RID: 190
	[SerializeField]
	private eTetrisType tetrisType;

	// Token: 0x040000BF RID: 191
	[SerializeField]
	private GameObject tetrisPrefab;
}
