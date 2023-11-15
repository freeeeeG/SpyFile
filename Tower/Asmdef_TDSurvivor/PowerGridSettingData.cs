using System;
using UnityEngine;

// Token: 0x0200002A RID: 42
[CreateAssetMenu(fileName = "PowerGridSettingData", menuName = "設定檔/PowerGridSettingData", order = 1)]
public class PowerGridSettingData : ScriptableObject
{
	// Token: 0x1700000E RID: 14
	// (get) Token: 0x060000D2 RID: 210 RVA: 0x00004891 File Offset: 0x00002A91
	public ePowerGridType PowerGridType
	{
		get
		{
			return this.type;
		}
	}

	// Token: 0x0400009A RID: 154
	[SerializeField]
	private ePowerGridType type;
}
