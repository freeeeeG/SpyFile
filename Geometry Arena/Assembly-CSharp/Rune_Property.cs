using System;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000063 RID: 99
[Serializable]
public class Rune_Property
{
	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060003CB RID: 971 RVA: 0x0001829E File Offset: 0x0001649E
	public int TotalType
	{
		get
		{
			return (int)(this.bigType * (EnumRunePropertyType)1000 + this.smallType);
		}
	}

	// Token: 0x060003CC RID: 972 RVA: 0x000182B3 File Offset: 0x000164B3
	public void Clone(Rune_Property clone)
	{
		this.bigType = clone.bigType;
		this.smallType = clone.smallType;
		this.playerFactor_IfPositive = clone.playerFactor_IfPositive;
		this.rank = clone.rank;
		this.ratioFloat = clone.ratioFloat;
	}

	// Token: 0x060003CD RID: 973 RVA: 0x000182F4 File Offset: 0x000164F4
	public void InitFromRace(Rune rune, RuneData runeData, bool ifGold = false)
	{
		this.bigType = runeData.bigType;
		this.smallType = runeData.smallType;
		this.playerFactor_IfPositive = runeData.factorPlayer_IfPositive;
		if (ifGold)
		{
			this.rank = EnumRank.LEGENDARY;
		}
		else
		{
			this.rank = DataSelector.GetEnumRank_Random_AsRune();
		}
		this.RandomRatioFloat();
	}

	// Token: 0x060003CE RID: 974 RVA: 0x00018344 File Offset: 0x00016544
	public static EnumRunePropertyType GetRandomType()
	{
		EnumRunePropertyType result;
		if (MyTool.DecimalToBool(0.5f))
		{
			result = EnumRunePropertyType.FACTORPLAYER;
		}
		else if (MyTool.DecimalToBool(0.7f))
		{
			result = EnumRunePropertyType.ORIGINUPGRADE;
		}
		else if (MyTool.DecimalToBool(0.8f))
		{
			result = EnumRunePropertyType.UPGRADETYPEBONUS;
		}
		else
		{
			result = EnumRunePropertyType.BLOCKUPGRADETYPE;
		}
		return result;
	}

	// Token: 0x060003CF RID: 975 RVA: 0x00018384 File Offset: 0x00016584
	public void InitWithRune(Rune rune, Rune_Property[] propExclude, EnumRank setRank = EnumRank.UNINTED, int setType = -1, EnumRunePropertyType setBigType = EnumRunePropertyType.UNINITED)
	{
		if (setRank == EnumRank.UNINTED)
		{
			this.rank = DataSelector.GetEnumRank_Random_AsRune();
		}
		else
		{
			this.rank = setRank;
		}
		if (setType >= 0)
		{
			this.bigType = EnumRunePropertyType.ORIGINUPGRADE;
		}
		else if (setBigType != EnumRunePropertyType.UNINITED)
		{
			this.bigType = setBigType;
		}
		else
		{
			this.bigType = Rune_Property.GetRandomType();
		}
		switch (this.bigType)
		{
		case EnumRunePropertyType.FACTORPLAYER:
		{
			List<int> list = new List<int>
			{
				1,
				2,
				3,
				4,
				6,
				7,
				8,
				9,
				10,
				11,
				13,
				14,
				15
			};
			foreach (Rune_Property rune_Property in propExclude)
			{
				if (rune_Property != null && rune_Property != this && rune_Property.bigType == EnumRunePropertyType.FACTORPLAYER && list.Contains(rune_Property.smallType))
				{
					list.Remove(rune_Property.smallType);
				}
			}
			if (list.Count == 0)
			{
				this.bigType = EnumRunePropertyType.ORIGINUPGRADE;
			}
			else
			{
				this.smallType = list.GetRandom<int>();
				if (this.smallType == 8 || this.smallType == 9 || this.smallType == 14 || this.smallType == 15)
				{
					if (MyTool.DecimalToBool(0.5f))
					{
						this.playerFactor_IfPositive = false;
					}
					else
					{
						this.playerFactor_IfPositive = true;
					}
				}
				else
				{
					this.playerFactor_IfPositive = true;
				}
			}
			break;
		}
		case EnumRunePropertyType.ORIGINUPGRADE:
			this.OriginUpdate_Random(propExclude, setType);
			break;
		case EnumRunePropertyType.UPGRADETYPEBONUS:
			this.UpdateTypeMulti_Random(propExclude);
			if (MyTool.DecimalToBool(0.5f))
			{
				this.playerFactor_IfPositive = false;
			}
			else
			{
				this.playerFactor_IfPositive = true;
			}
			break;
		case EnumRunePropertyType.BLOCKUPGRADETYPE:
			this.BlockUpdateType_Random(propExclude);
			break;
		}
		this.RandomRatioFloat();
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x0001854C File Offset: 0x0001674C
	public string GetInfo(Rune rune)
	{
		LanguageText inst = LanguageText.Inst;
		LanguageText.RuneInfo runeInfo = inst.runeInfo;
		DataBase inst2 = DataBase.Inst;
		Color color = UI_Setting.Inst.rankColors[(int)this.rank];
		StringBuilder stringBuilder = new StringBuilder();
		switch (this.bigType)
		{
		case EnumRunePropertyType.FACTORPLAYER:
		{
			stringBuilder.Append(inst.factor[this.smallType] + " ");
			FactorMultis factorMultis = new FactorMultis();
			factorMultis.MultiOneFactor(this.smallType, this.PlayerFactor_GetNum(rune));
			float num = factorMultis.factorMultis[this.smallType];
			stringBuilder.Append(MyTool.GetColorfulStringWithTypeAndNum(1, this.smallType, (double)num, false, 0f, false));
			return stringBuilder.ToString().Colored(color);
		}
		case EnumRunePropertyType.ORIGINUPGRADE:
			stringBuilder.Append(runeInfo.prop_OriginUpgrade).Append(inst2.Data_Upgrades[this.smallType].Language_Name);
			return stringBuilder.ToString().Colored(color);
		case EnumRunePropertyType.UPGRADETYPEBONUS:
		{
			UpgradeType upgradeType = DataBase.Inst.Data_UpgradeTypes[this.smallType];
			string colorfulStringWithTypeAndNum = MyTool.GetColorfulStringWithTypeAndNum(1, 3, (double)this.UpgradeType_GetNum(rune), false, 0f, false);
			string colorfulStringWithTypeAndNum2 = MyTool.GetColorfulStringWithTypeAndNum(1, 3, (double)this.UpgradeType_GetPrice(rune), false, 0f, false);
			string value = runeInfo.prop_UpgradeType.Replace("sUpgradeType", upgradeType.Language_TypeName).Replace("sProp", colorfulStringWithTypeAndNum).Replace("sPrice", colorfulStringWithTypeAndNum2);
			stringBuilder.Append(value);
			return stringBuilder.ToString().Colored(color);
		}
		case EnumRunePropertyType.BLOCKUPGRADETYPE:
		{
			UpgradeType upgradeType2 = DataBase.Inst.Data_UpgradeTypes[this.smallType];
			stringBuilder.Append(runeInfo.prop_BlockUpgradeType.Replace("sUpgradeType", upgradeType2.Language_TypeName));
			return stringBuilder.ToString().Colored(color);
		}
		default:
			return "TypeError";
		}
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x00018724 File Offset: 0x00016924
	public float PlayerFactor_GetNum(Rune rune)
	{
		float num = (float)this.GetRatioTotal(rune) * 5f;
		float num2 = GameParameters.Inst.FacStandards[this.smallType] * num;
		if (!this.playerFactor_IfPositive)
		{
			num2 *= -1f;
		}
		if (this.smallType == 1 && this.playerFactor_IfPositive)
		{
			num2 = (float)Mathf.Max(1, num2.RoundToInt());
		}
		return num2;
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x00018784 File Offset: 0x00016984
	public float UpgradeType_GetNum(Rune rune)
	{
		float num = (float)this.GetRatioTotal(rune);
		return 1f + 2f * num;
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x000187A8 File Offset: 0x000169A8
	public float UpgradeType_GetPrice(Rune rune)
	{
		float num = (float)this.GetRatioTotal(rune);
		return 1f - num / 10f;
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x000187CC File Offset: 0x000169CC
	public void RandomRatioFloat()
	{
		float num = UnityEngine.Random.Range(0f, 0.125f);
		this.ratioFloat = math.max((double)num, this.ratioFloat);
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x000187FC File Offset: 0x000169FC
	public double GetRatioTotal(Rune rune)
	{
		double num = (double)((float)this.rank * 0.25f);
		double num2 = (double)((float)rune.rank * 0.075f / 3f);
		double num3 = 0.125 + num + num2 + 0.05;
		if (num3 < 0.0 || num3 > 1.01)
		{
			Debug.LogError("Error_RatioTotal=" + num3);
			num3 = math.clamp(num3, 0.0, 1.0);
		}
		return num3;
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x0001888C File Offset: 0x00016A8C
	public void RankLevelUp(Rune_Property[] propExclude)
	{
		if (this.rank == EnumRank.LEGENDARY)
		{
			return;
		}
		this.rank++;
		if (this.bigType == EnumRunePropertyType.ORIGINUPGRADE)
		{
			this.OriginUpdate_Random(propExclude, -1);
		}
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x000188B8 File Offset: 0x00016AB8
	private void OriginUpdate_Random(Rune_Property[] propExclude, int setType)
	{
		Upgrade[] data_Upgrades = DataBase.Inst.Data_Upgrades;
		List<Upgrade> list = new List<Upgrade>();
		foreach (Upgrade upgrade in data_Upgrades)
		{
			if (upgrade.ifAvailable)
			{
				if (setType == -1)
				{
					if (upgrade.rank != this.rank)
					{
						goto IL_E4;
					}
				}
				else
				{
					bool flag = false;
					int[] upgradeIntTypes = upgrade.upgradeIntTypes;
					for (int j = 0; j < upgradeIntTypes.Length; j++)
					{
						if (upgradeIntTypes[j] == setType)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						goto IL_E4;
					}
				}
				if (upgrade.id != 166 && upgrade.id != 208 && upgrade.id != 211)
				{
					bool flag2 = true;
					foreach (Rune_Property rune_Property in propExclude)
					{
						if (rune_Property != null && rune_Property != this && rune_Property.bigType == EnumRunePropertyType.ORIGINUPGRADE && upgrade.id == rune_Property.smallType)
						{
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						list.Add(upgrade);
					}
				}
			}
			IL_E4:;
		}
		this.smallType = list.GetRandom<Upgrade>().id;
		if (setType != -1)
		{
			this.rank = DataBase.Inst.Data_Upgrades[this.smallType].rank;
		}
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x000189E8 File Offset: 0x00016BE8
	private void UpdateTypeMulti_Random(Rune_Property[] propExclude)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < DataBase.Inst.Data_UpgradeTypes.Length; i++)
		{
			if (DataBase.Inst.Data_UpgradeTypes[i].ifAvailableInRune)
			{
				bool flag = true;
				foreach (Rune_Property rune_Property in propExclude)
				{
					if (rune_Property != null && rune_Property != this && rune_Property.bigType == EnumRunePropertyType.UPGRADETYPEBONUS && i == rune_Property.smallType)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(i);
				}
			}
		}
		this.smallType = list.GetRandom<int>();
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x00018A78 File Offset: 0x00016C78
	private void BlockUpdateType_Random(Rune_Property[] propExclude)
	{
		this.rank = EnumRank.LEGENDARY;
		List<int> list = new List<int>();
		for (int i = 0; i < DataBase.Inst.Data_UpgradeTypes.Length; i++)
		{
			if (DataBase.Inst.Data_UpgradeTypes[i].ifAvailableInRune)
			{
				bool flag = true;
				foreach (Rune_Property rune_Property in propExclude)
				{
					if (rune_Property != null && rune_Property != this && rune_Property.bigType == EnumRunePropertyType.BLOCKUPGRADETYPE && i == rune_Property.smallType)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(i);
				}
			}
		}
		this.smallType = list.GetRandom<int>();
	}

	// Token: 0x060003DA RID: 986 RVA: 0x00018B0F File Offset: 0x00016D0F
	public static bool AutoFuse_IfPropConflict(Rune_Property prop1, Rune_Property prop2)
	{
		return prop1.bigType == prop2.bigType && prop1.smallType == prop2.smallType;
	}

	// Token: 0x060003DB RID: 987 RVA: 0x00018B32 File Offset: 0x00016D32
	public static bool AutoFuse_IfPropEqual(Rune_Property prop1, Rune_Property prop2)
	{
		return prop1.bigType == prop2.bigType && prop1.smallType == prop2.smallType && (prop1.bigType != EnumRunePropertyType.FACTORPLAYER || prop1.playerFactor_IfPositive == prop2.playerFactor_IfPositive);
	}

	// Token: 0x04000335 RID: 821
	public EnumRunePropertyType bigType = EnumRunePropertyType.UNINITED;

	// Token: 0x04000336 RID: 822
	public int smallType = -1;

	// Token: 0x04000337 RID: 823
	public bool playerFactor_IfPositive = true;

	// Token: 0x04000338 RID: 824
	public EnumRank rank = EnumRank.UNINTED;

	// Token: 0x04000339 RID: 825
	public double ratioFloat;
}
