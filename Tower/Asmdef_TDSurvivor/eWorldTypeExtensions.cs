using System;

// Token: 0x02000079 RID: 121
public static class eWorldTypeExtensions
{
	// Token: 0x06000287 RID: 647 RVA: 0x0000ABCC File Offset: 0x00008DCC
	public static int ToIndex(this eWorldType worldType)
	{
		switch (worldType)
		{
		case eWorldType.NONE:
		case eWorldType.WORLD_1_FOREST | eWorldType.WORLD_2_DESERT:
		case eWorldType.WORLD_1_FOREST | eWorldType.WORLD_3_GRAVEYARD:
		case eWorldType.WORLD_2_DESERT | eWorldType.WORLD_3_GRAVEYARD:
		case eWorldType.WORLD_1_FOREST | eWorldType.WORLD_2_DESERT | eWorldType.WORLD_3_GRAVEYARD:
			break;
		case eWorldType.WORLD_0_VILLAGE:
			return 0;
		case eWorldType.WORLD_1_FOREST:
			return 1;
		case eWorldType.WORLD_2_DESERT:
			return 2;
		case eWorldType.WORLD_3_GRAVEYARD:
			return 3;
		case eWorldType.WORLD_4_MOUNTAIN:
			return 4;
		default:
			if (worldType == eWorldType.WORLD_5_BOSS)
			{
				return 5;
			}
			break;
		}
		return -1;
	}
}
