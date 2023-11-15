using System;

// Token: 0x0200006E RID: 110
public static class eItemTypeExtension
{
	// Token: 0x06000283 RID: 643 RVA: 0x0000AAE8 File Offset: 0x00008CE8
	public static eCardType ToCardType(this eItemType itemType)
	{
		if (itemType == eItemType.NONE)
		{
			return eCardType.NONE;
		}
		if (itemType < (eItemType)2000)
		{
			return eCardType.TOWER_CARD;
		}
		if (itemType < (eItemType)3000)
		{
			return eCardType.PANEL_CARD;
		}
		if (itemType < (eItemType)4000)
		{
			return eCardType.BUFF_CARD;
		}
		return eCardType.NONE;
	}
}
