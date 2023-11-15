using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000021 RID: 33
public static class DataSelector
{
	// Token: 0x0600019B RID: 411 RVA: 0x0000ABAC File Offset: 0x00008DAC
	public static VarColor RandomVarColor_WithRankAndRange(EnumRank rank, bool ifShooter)
	{
		VarColor[] data_VarColors = DataBase.Inst.Data_VarColors;
		List<VarColor> list = new List<VarColor>();
		foreach (VarColor varColor in data_VarColors)
		{
			if (varColor.rank <= rank && varColor.avaiEnemy)
			{
				if (ifShooter)
				{
					if (varColor.useRange == VarColor.EnumUseRange.COMMON || varColor.useRange == VarColor.EnumUseRange.ONLYSHOOT)
					{
						list.Add(varColor);
					}
				}
				else if (varColor.useRange == VarColor.EnumUseRange.COMMON)
				{
					list.Add(varColor);
				}
			}
		}
		int index = Random.Range(0, list.Count);
		return list[index];
	}

	// Token: 0x0600019C RID: 412 RVA: 0x0000AC38 File Offset: 0x00008E38
	public static int RandomEnemyModelNo_WithMapAndDay(EnumLevelType levelType, int wave, EnumRank rank)
	{
		if (TempData.inst.modeType != EnumModeType.NORMAL && rank == EnumRank.EPIC)
		{
			wave = 5;
		}
		List<int> list = new List<int>();
		EnemyModel[] data_EnemyModels = DataBase.Inst.Data_EnemyModels;
		if (wave <= 0)
		{
			Debug.LogError("wave <=0!");
			return 0;
		}
		for (int i = 0; i < data_EnemyModels.Length; i++)
		{
			EnemyModel enemyModel = data_EnemyModels[i];
			if (enemyModel.ifAvailable && enemyModel.rank == rank)
			{
				int weightTheWave = enemyModel.GetWeightTheWave(levelType, wave);
				if (weightTheWave > 0)
				{
					for (int j = 0; j < weightTheWave; j++)
					{
						list.Add(i);
					}
				}
			}
		}
		if (list.Count == 0)
		{
			Debug.LogError("intList.Count==0!");
			return 0;
		}
		int index = Random.Range(0, list.Count);
		return list[index];
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000ACF0 File Offset: 0x00008EF0
	public static Upgrade GetUpgrade_Random()
	{
		GameParameters.Upgrade upgrade = GameParameters.Inst.upgrade;
		float num = (float)upgrade.weight[0] * Battle.inst.factorBattleTotal.Shop_NormalRate;
		float num2 = (float)upgrade.weight[1] * Battle.inst.factorBattleTotal.Shop_RareRate;
		float num3 = (float)upgrade.weight[2] * Battle.inst.factorBattleTotal.Shop_EpicRate;
		float num4 = (float)upgrade.weight[3] * Battle.inst.factorBattleTotal.Shop_LegendaryRate;
		float num5 = Random.Range(0f, num + num2 + num3 + num4);
		if (num5 < num4)
		{
			return DataSelector.RandomUpgrade_WithRank(EnumRank.LEGENDARY);
		}
		if (num5 < num4 + num3)
		{
			return DataSelector.RandomUpgrade_WithRank(EnumRank.EPIC);
		}
		if (num5 < num4 + num3 + num2)
		{
			return DataSelector.RandomUpgrade_WithRank(EnumRank.RARE);
		}
		return DataSelector.RandomUpgrade_WithRank(EnumRank.NORMAL);
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000ADB4 File Offset: 0x00008FB4
	public static EnumRank GetEnumRank_Random_AsRune()
	{
		float num = 72f;
		float num2 = 18f;
		float num3 = 4.5f;
		float num4 = 0.9f;
		float num5 = Random.Range(0f, num + num2 + num3 + num4);
		if (num5 < num4)
		{
			return EnumRank.LEGENDARY;
		}
		if (num5 < num4 + num3)
		{
			return EnumRank.EPIC;
		}
		if (num5 < num4 + num3 + num2)
		{
			return EnumRank.RARE;
		}
		return EnumRank.NORMAL;
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0000AE08 File Offset: 0x00009008
	public static Upgrade RandomUpgrade_WithRank(EnumRank rank)
	{
		Upgrade[] data_Upgrades = DataBase.Inst.Data_Upgrades;
		List<Upgrade> list = new List<Upgrade>();
		foreach (Upgrade upgrade in data_Upgrades)
		{
			if (upgrade.ifAvailable && upgrade.rank == rank && !Battle.inst.upgradeShop.ifHasUpgradeID(upgrade.id))
			{
				if (BattleMapCanvas.inst.panel_UpgradeShop.mysteryBag.ifActive)
				{
					UI_Panel_Battle_UpgradeShop_MysteryBag mysteryBag = BattleMapCanvas.inst.panel_UpgradeShop.mysteryBag;
					if (upgrade.id == mysteryBag.slots[0].upgradeID || upgrade.id == mysteryBag.slots[1].upgradeID)
					{
						goto IL_290;
					}
				}
				if (TempData.inst.jobId == 9 || TempData.inst.jobId == 10)
				{
					bool flag = false;
					for (int j = 0; j < upgrade.upgradeIntTypes.Length; j++)
					{
						int num = upgrade.upgradeIntTypes[j];
						if (num == 1 || num == 2 || num == 4 || upgrade.id == 210)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						goto IL_290;
					}
				}
				if (Battle.inst.diffiLevel != 0 || upgrade.id != 208)
				{
					if (upgrade.numMax != -1 && Battle.inst.GetUpgradeNumWithID(upgrade.id) > upgrade.numMax)
					{
						Debug.LogError(upgrade.id + " Num>>??");
					}
					else if (upgrade.numMax == -1 || Battle.inst.GetUpgradeNumWithID(upgrade.id) != upgrade.numMax)
					{
						int num2 = 1;
						Rune currentRune = GameData.inst.CurrentRune;
						if (currentRune != null)
						{
							for (int k = 0; k < currentRune.props.Length; k++)
							{
								Rune_Property rune_Property = currentRune.props[k];
								if (rune_Property == null)
								{
									Debug.LogError("Error_PropNull!");
								}
								else if (rune_Property.bigType == EnumRunePropertyType.UPGRADETYPEBONUS)
								{
									for (int l = 0; l < upgrade.upgradeIntTypes.Length; l++)
									{
										if (upgrade.upgradeIntTypes[l] == rune_Property.smallType)
										{
											num2 *= MyTool.DecimalToInt(rune_Property.UpgradeType_GetNum(currentRune));
										}
									}
								}
								else if (rune_Property.bigType == EnumRunePropertyType.BLOCKUPGRADETYPE)
								{
									for (int m = 0; m < upgrade.upgradeIntTypes.Length; m++)
									{
										if (upgrade.upgradeIntTypes[m] == rune_Property.smallType)
										{
											num2 = 0;
											break;
										}
									}
								}
							}
						}
						for (int n = 0; n < num2; n++)
						{
							list.Add(upgrade);
						}
					}
				}
			}
			IL_290:;
		}
		if (list.Count == 0)
		{
			return DataBase.Inst.Data_Upgrades[70];
		}
		int index = Random.Range(0, list.Count);
		return list[index];
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000B0DC File Offset: 0x000092DC
	public static BattleBuff GetBuff_Random()
	{
		float num = 55f;
		if (Battle.inst.specialEffect[67] >= 1)
		{
			num = 0f;
		}
		float num2 = 30f;
		float num3 = 12f;
		float num4 = 3f;
		float num5 = Random.Range(0f, num + num2 + num3 + num4);
		if (num5 < num4)
		{
			return DataSelector.RandomBattleBuff_WithRank(EnumRank.LEGENDARY);
		}
		if (num5 < num4 + num3)
		{
			return DataSelector.RandomBattleBuff_WithRank(EnumRank.EPIC);
		}
		if (num5 < num4 + num3 + num2)
		{
			return DataSelector.RandomBattleBuff_WithRank(EnumRank.RARE);
		}
		return DataSelector.RandomBattleBuff_WithRank(EnumRank.NORMAL);
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x0000B15C File Offset: 0x0000935C
	private static BattleBuff RandomBattleBuff_WithRank(EnumRank rank)
	{
		BattleBuff[] data_BattleBuffs = DataBase.Inst.Data_BattleBuffs;
		List<BattleBuff> list = new List<BattleBuff>();
		foreach (BattleBuff battleBuff in data_BattleBuffs)
		{
			if (battleBuff.ifAvailable && !Battle.inst.upgradeShop.ifHasUpgradeID(battleBuff.typeID) && battleBuff.rank == rank)
			{
				list.Add(battleBuff);
			}
		}
		int index = Random.Range(0, list.Count);
		return list[index];
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x0000B1D4 File Offset: 0x000093D4
	public static Upgrade[] GetUpgradesInOrder()
	{
		Upgrade[] data_Upgrades = DataBase.Inst.Data_Upgrades;
		Upgrade[] array = new Upgrade[data_Upgrades.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = data_Upgrades[i];
		}
		for (int j = 0; j < array.Length - 1; j++)
		{
			for (int k = 0; k < array.Length - 1 - j; k++)
			{
				Upgrade upgrade = array[k];
				Upgrade upgrade2 = array[k + 1];
				if (upgrade.rank < upgrade2.rank || (upgrade.rank == upgrade2.rank && upgrade.orderID > upgrade2.orderID))
				{
					array[k] = upgrade2;
					array[k + 1] = upgrade;
				}
			}
		}
		return array;
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x0000B27C File Offset: 0x0000947C
	public static List<Upgrade> GetUpgradesWithType_InRankOrder(int targetTypeID)
	{
		List<Upgrade> list = new List<Upgrade>();
		foreach (Upgrade upgrade in DataBase.Inst.Data_Upgrades)
		{
			bool flag = false;
			for (int j = 0; j < upgrade.upgradeIntTypes.Length; j++)
			{
				if (upgrade.upgradeIntTypes[j] == targetTypeID)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				list.Add(upgrade);
			}
		}
		for (int k = 0; k < list.Count - 1; k++)
		{
			for (int l = 0; l < list.Count - 1 - k; l++)
			{
				Upgrade upgrade2 = list[l];
				Upgrade upgrade3 = list[l + 1];
				if (upgrade2.rank < upgrade3.rank || (upgrade2.rank == upgrade3.rank && upgrade2.orderID > upgrade3.orderID))
				{
					list[l] = upgrade3;
					list[l + 1] = upgrade2;
				}
			}
		}
		return list;
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000B374 File Offset: 0x00009574
	public static Guide[] GetGuidesInOrder()
	{
		Guide[] array = new Guide[DataBase.Inst.dataGuides.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = DataBase.Inst.dataGuides[i];
		}
		for (int j = 0; j < array.Length - 1; j++)
		{
			for (int k = 0; k < array.Length - 1 - j; k++)
			{
				if (array[k].orderNo > array[k + 1].orderNo)
				{
					Guide guide = array[k];
					array[k] = array[k + 1];
					array[k + 1] = guide;
				}
				else if (array[k].orderNo == array[k + 1].orderNo)
				{
					Debug.LogError("Error_Guide排序序号相同");
				}
			}
		}
		return array;
	}
}
