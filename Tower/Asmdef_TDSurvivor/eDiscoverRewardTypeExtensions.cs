using System;

// Token: 0x0200006A RID: 106
public static class eDiscoverRewardTypeExtensions
{
	// Token: 0x06000281 RID: 641 RVA: 0x0000AA9C File Offset: 0x00008C9C
	public static eCardType ToCardType(this eDiscoverRewardType damageType)
	{
		switch (damageType)
		{
		default:
			return eCardType.NONE;
		case eDiscoverRewardType.HP:
			return eCardType.HP_CARD;
		case eDiscoverRewardType.COIN:
			return eCardType.COIN_CARD;
		case eDiscoverRewardType.TOWER_CARD:
			return eCardType.TOWER_CARD;
		case eDiscoverRewardType.PANEL_CARD:
			return eCardType.PANEL_CARD;
		case eDiscoverRewardType.RANDOM_PANEL_CARD:
			return eCardType.PANEL_CARD;
		case eDiscoverRewardType.BUFF_CARD:
			return eCardType.BUFF_CARD;
		case eDiscoverRewardType.RANDOM_BUFF_CARD:
			return eCardType.BUFF_CARD;
		}
	}

	// Token: 0x06000282 RID: 642 RVA: 0x0000AAD3 File Offset: 0x00008CD3
	public static bool IsItemCard(this eDiscoverRewardType damageType)
	{
		return damageType == eDiscoverRewardType.PANEL_CARD || damageType == eDiscoverRewardType.BUFF_CARD || damageType == eDiscoverRewardType.RANDOM_PANEL_CARD || damageType == eDiscoverRewardType.RANDOM_BUFF_CARD;
	}
}
