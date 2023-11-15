using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
public interface ICardDataSource
{
	// Token: 0x060000BE RID: 190
	eItemType GetItemType();

	// Token: 0x060000BF RID: 191
	Sprite GetCardIcon();

	// Token: 0x060000C0 RID: 192
	string GetLocNameString(bool isPrefix = true);

	// Token: 0x060000C1 RID: 193
	string GetLocFlavorTextString();

	// Token: 0x060000C2 RID: 194
	string GetLocStatsString();

	// Token: 0x060000C3 RID: 195
	AItemSettingData GetScriptableObjectData();
}
