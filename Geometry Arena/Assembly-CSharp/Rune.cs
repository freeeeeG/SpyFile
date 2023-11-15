using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000062 RID: 98
[Serializable]
public class Rune
{
	// Token: 0x060003B6 RID: 950 RVA: 0x000173CF File Offset: 0x000155CF
	public string Get_Lang_RuneName()
	{
		return DataBase.Inst.Data_RuneDatas[this.typeID].Language_Name;
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x000173E7 File Offset: 0x000155E7
	public void SetID_Random()
	{
		this.typeID = Random.Range(0, 6);
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x000173F6 File Offset: 0x000155F6
	public void SetID(int ID)
	{
		this.typeID = ID;
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x00017400 File Offset: 0x00015600
	public void CloneFromRune(Rune clone)
	{
		this.name = clone.name;
		this.typeID = clone.typeID;
		this.rank = clone.rank;
		this.addiSlot = clone.addiSlot;
		this.props = new Rune_Property[clone.props.Length];
		for (int i = 0; i < this.props.Length; i++)
		{
			this.props[i] = new Rune_Property();
			this.props[i].Clone(clone.props[i]);
		}
		this.varColorID = clone.varColorID;
		this.ifFavorite = false;
		this.ifNew = false;
	}

	// Token: 0x060003BA RID: 954 RVA: 0x000174A0 File Offset: 0x000156A0
	public void InitRandom(EnumRank setRank = EnumRank.UNINTED, int setAddiSlot = -1, bool ifAllGold = false, int setUpType = -1, EnumRunePropertyType setBigType = EnumRunePropertyType.UNINITED)
	{
		this.varColorID = Random.Range(0, 12);
		if (this.typeID < 0)
		{
			this.SetID_Random();
		}
		if (setRank == EnumRank.UNINTED)
		{
			this.rank = DataSelector.GetEnumRank_Random_AsRune();
		}
		else
		{
			this.rank = setRank;
		}
		if (setAddiSlot == -1)
		{
			this.addiSlot = MyTool.DecimalToInt(0.5f);
		}
		else
		{
			this.addiSlot = setAddiSlot;
		}
		RuneData runeData = DataBase.Inst.Data_RuneDatas[this.typeID];
		this.name = runeData.Language_Name;
		int num = (int)(1 + this.rank + 1 + this.addiSlot);
		this.props = new Rune_Property[num];
		for (int i = 0; i < this.props.Length; i++)
		{
			this.props[i] = new Rune_Property();
			if (i == 0)
			{
				bool ifGold = false;
				if (ifAllGold && setAddiSlot == 4)
				{
					ifGold = true;
				}
				this.props[i].InitFromRace(this, runeData, ifGold);
			}
			else if (ifAllGold)
			{
				this.props[i].InitWithRune(this, this.props, EnumRank.LEGENDARY, setUpType, setBigType);
			}
			else
			{
				this.props[i].InitWithRune(this, this.props, EnumRank.UNINTED, setUpType, setBigType);
			}
		}
	}

	// Token: 0x060003BB RID: 955 RVA: 0x000175B4 File Offset: 0x000157B4
	public string GetInfo_RuneProps()
	{
		StringBuilder stringBuilder = new StringBuilder();
		Rune_Property[] props_Sort = this.GetProps_Sort();
		for (int i = 0; i < props_Sort.Length; i++)
		{
			if (i >= 1)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append(props_Sort[i].GetInfo(this));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060003BC RID: 956 RVA: 0x00017600 File Offset: 0x00015800
	public string GetInfo_Total()
	{
		string str = "";
		UI_Setting inst = UI_Setting.Inst;
		LanguageText inst2 = LanguageText.Inst;
		LanguageText.RuneInfo runeInfo = inst2.runeInfo;
		RuneData runeData = DataBase.Inst.Data_RuneDatas[this.typeID];
		string str2;
		if (this.addiSlot == 0)
		{
			str2 = "";
		}
		else
		{
			str2 = "+" + this.addiSlot.ToString();
		}
		return str + runeData.Language_Name.Sized(inst.ToolTip_HeaderSize).Colored(inst.rankColors[(int)this.rank]) + str2 + "\n" + (inst2.ranks[(int)this.rank] + runeInfo.rune).Sized(inst.ToolTip_NormalSize) + "\n" + this.GetInfo_RuneProps();
	}

	// Token: 0x060003BD RID: 957 RVA: 0x000176CC File Offset: 0x000158CC
	public string GetInfo_ExceptProp()
	{
		string str = "";
		UI_Setting inst = UI_Setting.Inst;
		LanguageText inst2 = LanguageText.Inst;
		LanguageText.RuneInfo runeInfo = inst2.runeInfo;
		RuneData runeData = DataBase.Inst.Data_RuneDatas[this.typeID];
		string str2;
		if (this.addiSlot == 0)
		{
			str2 = "";
		}
		else
		{
			str2 = "+" + this.addiSlot.ToString();
		}
		return str + runeData.Language_Name.Sized(inst.ToolTip_HeaderSize).Colored(inst.rankColors[(int)this.rank]) + str2 + "\n" + (inst2.ranks[(int)this.rank] + runeInfo.rune).Sized(inst.ToolTip_NormalSize);
	}

	// Token: 0x060003BE RID: 958 RVA: 0x00017788 File Offset: 0x00015988
	public FactorMultis GetFactorMultis_ThisRune()
	{
		FactorMultis factorMultis = new FactorMultis();
		for (int i = 0; i < this.props.Length; i++)
		{
			Rune_Property rune_Property = this.props[i];
			if (rune_Property.bigType == EnumRunePropertyType.FACTORPLAYER)
			{
				factorMultis.MultiOneFactor(rune_Property.smallType, rune_Property.PlayerFactor_GetNum(this));
			}
		}
		return factorMultis;
	}

	// Token: 0x060003BF RID: 959 RVA: 0x000177D4 File Offset: 0x000159D4
	public void GetOriginUpgrades()
	{
		for (int i = 0; i < this.props.Length; i++)
		{
			Rune_Property rune_Property = this.props[i];
			if (rune_Property.bigType == EnumRunePropertyType.ORIGINUPGRADE)
			{
				TempData.inst.battle.Upgrade_Gain(rune_Property.smallType);
			}
		}
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x0001781C File Offset: 0x00015A1C
	public static int Synthesize_SonRank(int fatherRankInt, int motherRankInt, float percent)
	{
		int num = Mathf.Min(fatherRankInt, motherRankInt);
		int num2 = Mathf.Max(fatherRankInt, motherRankInt);
		int num3 = num2 - num;
		float num4 = 1f / percent + 1f;
		float f = 1f / num4;
		int num5 = MyTool.DecimalToInt(percent * Mathf.Pow(f, (float)num3));
		return Mathf.Clamp(num2 + num5, 0, 4);
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x00017870 File Offset: 0x00015A70
	public static Rune Synthesize(Rune father, Rune mother)
	{
		Rune rune = new Rune();
		rune.rank = (EnumRank)Mathf.Min(3, Rune.Synthesize_SonRank((int)father.rank, (int)mother.rank, 0.36f));
		int num = Mathf.Min(father.addiSlot, mother.addiSlot);
		int num2 = Mathf.Max(father.addiSlot, mother.addiSlot);
		int num3;
		if (num2 - num >= 3)
		{
			num3 = num2 - 1;
		}
		else
		{
			num3 = Mathf.Min(4, Rune.Synthesize_SonRank(father.addiSlot, mother.addiSlot, 0.36f));
		}
		rune.addiSlot = num3;
		int num4 = (int)(1 + rune.rank + 1 + rune.addiSlot);
		rune.props = new Rune_Property[num4];
		Rune rune2;
		if (MyTool.DecimalToBool(0.5f))
		{
			rune2 = father;
		}
		else
		{
			rune2 = mother;
		}
		rune.name = rune2.name;
		rune.typeID = rune2.typeID;
		rune.varColorID = rune2.varColorID;
		rune.props[0] = new Rune_Property();
		rune.props[0].Clone(rune2.props[0]);
		rune.props[0].RandomRatioFloat();
		List<Rune_Property> list = new List<Rune_Property>();
		for (int i = 0; i < 2; i++)
		{
			Rune rune3 = null;
			if (i == 0)
			{
				rune3 = father;
			}
			else if (i == 1)
			{
				rune3 = mother;
			}
			else
			{
				Debug.LogError("Error_RuneParent??");
			}
			for (int j = 0; j < rune3.props.Length; j++)
			{
				bool flag = true;
				if (rune.props[0].TotalType == rune3.props[j].TotalType)
				{
					if (rune3.props[j].rank >= rune.props[0].rank)
					{
						rune.props[0].rank = rune3.props[j].rank;
						if (rune3.props[j].ratioFloat > rune.props[0].ratioFloat)
						{
							rune.props[0].ratioFloat = rune3.props[j].ratioFloat;
						}
					}
				}
				else
				{
					for (int k = 0; k < list.Count; k++)
					{
						if (list[k].TotalType == rune3.props[j].TotalType)
						{
							if (rune3.props[j].rank >= list[k].rank)
							{
								list[k].rank = rune3.props[j].rank;
								if (rune3.props[j].ratioFloat > list[k].ratioFloat)
								{
									list[k].ratioFloat = rune3.props[j].ratioFloat;
								}
							}
							flag = false;
							break;
						}
					}
					if (flag)
					{
						Rune_Property rune_Property = new Rune_Property();
						rune_Property.Clone(rune3.props[j]);
						list.Add(rune_Property);
					}
				}
			}
		}
		for (int l = 1; l < rune.props.Length; l++)
		{
			if (list.Count > 0)
			{
				Rune_Property random = list.GetRandom<Rune_Property>();
				list.Remove(random);
				rune.props[l] = new Rune_Property();
				rune.props[l].Clone(random);
				rune.props[l].RandomRatioFloat();
			}
			else
			{
				rune.props[l] = new Rune_Property();
				rune.props[l].InitWithRune(rune, rune.props, EnumRank.UNINTED, -1, EnumRunePropertyType.UNINITED);
			}
		}
		for (int m = 0; m < rune.props.Length; m++)
		{
			Rune_Property rune_Property2 = rune.props[m];
			if (rune_Property2.rank != EnumRank.LEGENDARY && MyTool.DecimalToBool(0.18f))
			{
				rune_Property2.RankLevelUp(rune.props);
			}
		}
		return rune;
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x00017C30 File Offset: 0x00015E30
	public static void AddRune(Rune rune)
	{
		GameData.inst.runes.Add(rune);
		MySteamAchievement.UnlockAchievement(39);
		if (rune.rank == EnumRank.EPIC)
		{
			MySteamAchievement.UnlockAchievement(40);
		}
		if (rune.rank == EnumRank.LEGENDARY)
		{
			MySteamAchievement.UnlockAchievement(41);
			if (rune.props.Length == 9)
			{
				MySteamAchievement.UnlockAchievement(42);
			}
		}
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x00017C87 File Offset: 0x00015E87
	public static void AddNewRune(Rune newRune)
	{
		newRune.ifNew = true;
		Rune.AddRune(newRune);
		TutorialMaster.inst.TrigID(16);
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x00017CA4 File Offset: 0x00015EA4
	public static bool RemoveRune(Rune rune)
	{
		List<Rune> runes = GameData.inst.runes;
		if (rune == null)
		{
			Debug.LogError("Error_移除空符文");
			return false;
		}
		if (!runes.Contains(rune))
		{
			Debug.LogError("Error_移除不在存档里的符文");
			return false;
		}
		Rune currentRune = GameData.inst.CurrentRune;
		Rune rune2 = null;
		Rune rune3 = null;
		if (GameData.inst.runeFusion_MaterialIndexs[0] >= 0)
		{
			rune2 = runes[GameData.inst.runeFusion_MaterialIndexs[0]];
		}
		if (GameData.inst.runeFusion_MaterialIndexs[1] >= 0)
		{
			rune3 = runes[GameData.inst.runeFusion_MaterialIndexs[1]];
		}
		GameData.inst.runes.Remove(rune);
		GameData.inst.currentRunesIndex[0] = -1;
		GameData.inst.runeFusion_MaterialIndexs[0] = -1;
		GameData.inst.runeFusion_MaterialIndexs[1] = -1;
		for (int i = 0; i < runes.Count; i++)
		{
			Rune rune4 = runes[i];
			if (currentRune == rune4)
			{
				GameData.inst.currentRunesIndex[0] = i;
			}
			if (rune2 == rune4)
			{
				GameData.inst.runeFusion_MaterialIndexs[0] = i;
			}
			if (rune3 == rune4)
			{
				GameData.inst.runeFusion_MaterialIndexs[1] = i;
			}
		}
		return true;
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x00017DC4 File Offset: 0x00015FC4
	private static Rune BigSynthesize(List<Rune> runesToSyn)
	{
		int count = runesToSyn.Count;
		if (count == 1)
		{
			return runesToSyn[0];
		}
		if (count == 2)
		{
			return Rune.Synthesize(runesToSyn[0], runesToSyn[1]);
		}
		int num = Mathf.Floor((float)(count - 1) / 2f).RoundToInt();
		int num2 = num + 1;
		List<Rune> list = new List<Rune>();
		List<Rune> list2 = new List<Rune>();
		for (int i = 0; i <= num; i++)
		{
			list.Add(runesToSyn[i]);
		}
		for (int j = num2; j <= count - 1; j++)
		{
			list2.Add(runesToSyn[j]);
		}
		return Rune.Synthesize(Rune.BigSynthesize(list), Rune.BigSynthesize(list2));
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x00017E74 File Offset: 0x00016074
	public static Rune NewBigRune(int num)
	{
		List<Rune> list = new List<Rune>();
		for (int i = 0; i < num; i++)
		{
			Rune rune = new Rune();
			rune.InitRandom(EnumRank.UNINTED, -1, false, -1, EnumRunePropertyType.UNINITED);
			list.Add(rune);
		}
		return Rune.BigSynthesize(list);
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x00017EB4 File Offset: 0x000160B4
	public static Rune InitSpecial_RuneStore(ref int price, int setSpecialType = -1)
	{
		int[] array = new int[]
		{
			54,
			24,
			6,
			15,
			1
		};
		int num = Random.Range(0, 100);
		int num2 = 0;
		int num3 = -1;
		if (setSpecialType == -1)
		{
			for (int i = 0; i < array.Length; i++)
			{
				num2 += array[i];
				if (num < num2)
				{
					num3 = i;
					break;
				}
			}
		}
		else
		{
			num3 = setSpecialType;
		}
		if (num3 < 0 || num3 >= array.Length)
		{
			Debug.LogError("TypeError!");
			num3 = 0;
		}
		Rune rune = new Rune();
		float value = 100f;
		switch (num3)
		{
		case 0:
			rune.InitRandom(EnumRank.UNINTED, -1, false, -1, EnumRunePropertyType.UNINITED);
			value = 20f * Mathf.Pow(2.5f, (float)rune.rank) * Random.Range(0.8f, 1.2f);
			break;
		case 1:
		{
			int num4 = Random.Range(15, 51);
			rune = Rune.NewBigRune(num4);
			value = (float)(num4 * 20) * Random.Range(0.39f, 0.6f);
			break;
		}
		case 2:
		{
			EnumRank enumRank = MyTool.DecimalToBool(0.75f) ? EnumRank.EPIC : EnumRank.LEGENDARY;
			int[] array2;
			if (enumRank == EnumRank.EPIC)
			{
				array2 = new int[]
				{
					0,
					3,
					4,
					5,
					11,
					12,
					7
				};
			}
			else
			{
				array2 = new int[]
				{
					3,
					4,
					5,
					11,
					12,
					7
				};
			}
			int num5 = array2[Random.Range(0, array2.Length)];
			Rune rune2 = rune;
			int setUpType = num5;
			rune2.InitRandom(enumRank, 0, false, setUpType, EnumRunePropertyType.UNINITED);
			value = 20f * Mathf.Pow(2.5f, (float)rune.rank) * Random.Range(0.8f, 1.2f) * 2f;
			break;
		}
		case 3:
			rune.InitRandom(EnumRank.NORMAL, 0, true, -1, EnumRunePropertyType.UNINITED);
			value = 90f * Random.Range(0.8f, 1.2f);
			break;
		case 4:
			rune.InitRandom(EnumRank.LEGENDARY, 4, true, -1, Rune_Property.GetRandomType());
			value = 900f * Random.Range(0.8f, 1.2f);
			break;
		}
		price = value.RoundToInt();
		return rune;
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x000180A0 File Offset: 0x000162A0
	public Rune_Property[] GetProps_Sort()
	{
		Rune_Property[] array = new Rune_Property[this.props.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.props[i];
		}
		for (int j = 0; j < array.Length - 1; j++)
		{
			for (int k = 1; k < array.Length - 1 - j; k++)
			{
				Rune_Property rune_Property = array[k];
				Rune_Property rune_Property2 = array[k + 1];
				if (rune_Property.bigType > rune_Property2.bigType)
				{
					array[k] = rune_Property2;
					array[k + 1] = rune_Property;
				}
				else if (rune_Property.bigType == rune_Property2.bigType)
				{
					if (rune_Property.rank > rune_Property2.rank)
					{
						array[k] = rune_Property2;
						array[k + 1] = rune_Property;
					}
					else if (rune_Property.rank == rune_Property2.rank && rune_Property.smallType > rune_Property2.smallType)
					{
						array[k] = rune_Property2;
						array[k + 1] = rune_Property;
					}
				}
			}
		}
		return array;
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x00018188 File Offset: 0x00016388
	public int GetInt_RecyclePrice()
	{
		if (this.typeID == 12 || this.typeID == 13)
		{
			return 1;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		switch (this.rank)
		{
		case EnumRank.NORMAL:
			num = 5;
			break;
		case EnumRank.RARE:
			num = 10;
			break;
		case EnumRank.EPIC:
			num = 20;
			break;
		case EnumRank.LEGENDARY:
			num = 40;
			break;
		}
		switch (this.addiSlot)
		{
		case 0:
			num2 = 0;
			break;
		case 1:
			num2 = 5;
			break;
		case 2:
			num2 = 10;
			break;
		case 3:
			num2 = 20;
			break;
		case 4:
			num2 = 40;
			break;
		}
		Rune_Property[] array = this.props;
		for (int i = 0; i < array.Length; i++)
		{
			switch (array[i].rank)
			{
			case EnumRank.NORMAL:
				num3++;
				break;
			case EnumRank.RARE:
				num3 += 4;
				break;
			case EnumRank.EPIC:
				num3 += 9;
				break;
			case EnumRank.LEGENDARY:
				num3 += 25;
				break;
			}
		}
		return num + num2 + num3;
	}

	// Token: 0x0400032D RID: 813
	public string name = "UNINITED";

	// Token: 0x0400032E RID: 814
	public int typeID = -1;

	// Token: 0x0400032F RID: 815
	public EnumRank rank = EnumRank.UNINTED;

	// Token: 0x04000330 RID: 816
	public int addiSlot;

	// Token: 0x04000331 RID: 817
	[SerializeField]
	public Rune_Property[] props;

	// Token: 0x04000332 RID: 818
	public int varColorID = -1;

	// Token: 0x04000333 RID: 819
	public bool ifFavorite;

	// Token: 0x04000334 RID: 820
	public bool ifNew;
}
