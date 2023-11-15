using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001D RID: 29
[Serializable]
public class UpgradeShop
{
	// Token: 0x06000172 RID: 370 RVA: 0x0000A0BC File Offset: 0x000082BC
	public bool ifHasUpgradeID(int id)
	{
		foreach (UpgradeShop.Goods goods in this.upgradeGoods)
		{
			if (goods != null && goods.upgradeID == id)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0000A0F1 File Offset: 0x000082F1
	public void TryUpdateShop()
	{
		if (Battle.inst.upgradeShop_IfLocked)
		{
			Debug.Log("商店已锁定，不刷新");
			return;
		}
		this.UpdateShop();
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0000A110 File Offset: 0x00008310
	public void UpdateShop()
	{
		Debug.Log("刷新商店");
		EnumRank rank = EnumRank.UNINTED;
		if (Battle.inst.specialEffect[69] >= 1 && MyRandom.GetFloat1_VipCard() <= 0.09f)
		{
			Debug.Log("刷新商店_VIP");
			rank = EnumRank.LEGENDARY;
		}
		this.upgradeGoods = new UpgradeShop.Goods[4];
		for (int i = 0; i < 3; i++)
		{
			this.upgradeGoods[i] = new UpgradeShop.Goods();
			if (i == 0 && Battle.inst.specialEffect[84] >= 1)
			{
				this.upgradeGoods[i].InitWithType(16);
			}
			else
			{
				this.upgradeGoods[i].InitWithRank(rank);
			}
		}
		this.upgradeGoods[3] = new UpgradeShop.Goods();
		this.upgradeGoods[3].InitWithRank(EnumRank.LEGENDARY);
	}

	// Token: 0x06000175 RID: 373 RVA: 0x0000A1C4 File Offset: 0x000083C4
	public void SpecialUpdateShop_AllLegend()
	{
		if (Battle.inst.SequalWave != 1)
		{
			return;
		}
		if (!TempData.inst.diffiOptFlag[19])
		{
			return;
		}
		Debug.Log("刷新商店 全传奇");
		this.upgradeGoods = new UpgradeShop.Goods[4];
		for (int i = 0; i < 3; i++)
		{
			this.upgradeGoods[i] = new UpgradeShop.Goods();
			this.upgradeGoods[i].InitWithRank(EnumRank.LEGENDARY);
		}
		this.upgradeGoods[3] = new UpgradeShop.Goods();
		this.upgradeGoods[3].InitWithRank(EnumRank.LEGENDARY);
	}

	// Token: 0x04000188 RID: 392
	[SerializeField]
	public UpgradeShop.Goods[] upgradeGoods = new UpgradeShop.Goods[4];

	// Token: 0x0200013A RID: 314
	[Serializable]
	public class Goods
	{
		// Token: 0x06000987 RID: 2439 RVA: 0x000361AC File Offset: 0x000343AC
		public void InitWithRank(EnumRank rank)
		{
			GameParameters.Upgrade upgrade = GameParameters.Inst.upgrade;
			Upgrade upgrade2;
			if (rank != EnumRank.UNINTED)
			{
				upgrade2 = DataSelector.RandomUpgrade_WithRank(rank);
			}
			else
			{
				upgrade2 = DataSelector.GetUpgrade_Random();
			}
			this.upgradeID = upgrade2.id;
			float num = Random.Range(upgrade.priceFloatRange.x, upgrade.priceFloatRange.y);
			float num2 = (float)Battle.inst.FragmentUsed * GameParameters.Inst.FactorUpgradeInflation + 1f;
			num2 = Mathf.Min(num2, 30f);
			this.price = Mathf.RoundToInt((float)upgrade.basicPrice * upgrade.pricePerRank[(int)upgrade2.rank] * num * num2);
			if (Battle.inst.specialEffect[50] >= 1 && MyRandom.GetFloat1_RoyaltyCard() <= 0.06f)
			{
				this.price = 0;
			}
			float num3 = 1f;
			Rune currentRune = GameData.inst.CurrentRune;
			if (currentRune != null)
			{
				for (int i = 0; i < currentRune.props.Length; i++)
				{
					Rune_Property rune_Property = currentRune.props[i];
					if (rune_Property == null)
					{
						Debug.LogError("Error_PropNull!");
					}
					else if (rune_Property.bigType == EnumRunePropertyType.UPGRADETYPEBONUS)
					{
						for (int j = 0; j < upgrade2.upgradeIntTypes.Length; j++)
						{
							if (upgrade2.upgradeIntTypes[j] == rune_Property.smallType)
							{
								num3 *= rune_Property.UpgradeType_GetPrice(currentRune);
								break;
							}
						}
					}
				}
			}
			this.price = MyTool.DecimalToInt(num3 * (float)this.price);
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0003631C File Offset: 0x0003451C
		public void InitWithType(int type)
		{
			GameParameters.Upgrade upgrade = GameParameters.Inst.upgrade;
			Upgrade[] data_Upgrades = DataBase.Inst.Data_Upgrades;
			List<Upgrade> list = new List<Upgrade>();
			foreach (Upgrade upgrade2 in data_Upgrades)
			{
				if (upgrade2.GetBool_IfHasType(type))
				{
					list.Add(upgrade2);
				}
			}
			if (type == 16)
			{
				if (Battle.inst.diffiLevel == 0)
				{
					list.Remove(data_Upgrades[208]);
				}
				if (TempData.inst.jobId == 9 || TempData.inst.jobId == 10)
				{
					list.Remove(data_Upgrades[210]);
				}
			}
			int index = MyRandom.GetInt_Other() % list.Count;
			Upgrade upgrade3 = list[index];
			this.upgradeID = upgrade3.id;
			float num = Random.Range(upgrade.priceFloatRange.x, upgrade.priceFloatRange.y);
			float num2 = (float)Battle.inst.FragmentUsed * GameParameters.Inst.FactorUpgradeInflation + 1f;
			num2 = Mathf.Min(num2, 30f);
			this.price = Mathf.RoundToInt((float)upgrade.basicPrice * upgrade.pricePerRank[(int)upgrade3.rank] * num * num2);
			if (Battle.inst.specialEffect[50] >= 1 && MyRandom.GetFloat1_RoyaltyCard() <= 0.06f)
			{
				this.price = 0;
			}
			float num3 = 1f;
			Rune currentRune = GameData.inst.CurrentRune;
			if (currentRune != null)
			{
				for (int j = 0; j < currentRune.props.Length; j++)
				{
					Rune_Property rune_Property = currentRune.props[j];
					if (rune_Property == null)
					{
						Debug.LogError("Error_PropNull!");
					}
					else if (rune_Property.bigType == EnumRunePropertyType.UPGRADETYPEBONUS)
					{
						for (int k = 0; k < upgrade3.upgradeIntTypes.Length; k++)
						{
							if (upgrade3.upgradeIntTypes[k] == rune_Property.smallType)
							{
								num3 *= rune_Property.UpgradeType_GetPrice(currentRune);
								break;
							}
						}
					}
				}
			}
			this.price = MyTool.DecimalToInt(num3 * (float)this.price);
		}

		// Token: 0x0400096E RID: 2414
		public int upgradeID = -1;

		// Token: 0x0400096F RID: 2415
		public int price = -1;
	}
}
