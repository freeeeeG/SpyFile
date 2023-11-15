using System;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

// Token: 0x02000015 RID: 21
[Serializable]
public class Battle
{
	// Token: 0x17000061 RID: 97
	// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000561F File Offset: 0x0000381F
	public static Battle inst
	{
		get
		{
			return TempData.inst.battle;
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x060000BA RID: 186 RVA: 0x00005639 File Offset: 0x00003839
	// (set) Token: 0x060000B9 RID: 185 RVA: 0x0000562B File Offset: 0x0000382B
	public long FragmentUsed
	{
		get
		{
			return this.fragmentUsedEnc;
		}
		set
		{
			this.fragmentUsedEnc = value;
		}
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x060000BC RID: 188 RVA: 0x00005654 File Offset: 0x00003854
	// (set) Token: 0x060000BB RID: 187 RVA: 0x00005646 File Offset: 0x00003846
	public long Fragment
	{
		get
		{
			return this.fragmentEnc;
		}
		set
		{
			this.fragmentEnc = value;
		}
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x060000BE RID: 190 RVA: 0x0000566F File Offset: 0x0000386F
	// (set) Token: 0x060000BD RID: 189 RVA: 0x00005661 File Offset: 0x00003861
	public long Score
	{
		get
		{
			return this.scoreEnc;
		}
		set
		{
			this.scoreEnc = value;
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x060000BF RID: 191 RVA: 0x0000567C File Offset: 0x0000387C
	// (set) Token: 0x060000C0 RID: 192 RVA: 0x00005689 File Offset: 0x00003889
	public int upgradeShop_RefreshPrice
	{
		get
		{
			return this.upgradeShop_RefreshPriceData;
		}
		set
		{
			this.upgradeShop_RefreshPriceData = value;
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005698 File Offset: 0x00003898
	public EnumLevelType CurrentLevelType
	{
		get
		{
			if (this.ListLevelTypes.Count == 0)
			{
				return EnumLevelType.CIRCLE;
			}
			if (TempData.inst.modeType == EnumModeType.NORMAL)
			{
				return this.ListLevelTypes[this.level - 1];
			}
			return this.ListLevelTypes[(this.wave - 1) % 4];
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x060000C2 RID: 194 RVA: 0x000056E9 File Offset: 0x000038E9
	public int ActualWave
	{
		get
		{
			if (TempData.inst.modeType == EnumModeType.NORMAL)
			{
				return this.wave;
			}
			return Mathf.Min((this.wave - 1) / 4 + 1, 5);
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x060000C3 RID: 195 RVA: 0x00005710 File Offset: 0x00003910
	public int SequalWave
	{
		get
		{
			return (this.level - 1) * 5 + this.wave;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x060000C5 RID: 197 RVA: 0x00005738 File Offset: 0x00003938
	// (set) Token: 0x060000C4 RID: 196 RVA: 0x00005723 File Offset: 0x00003923
	public int level
	{
		get
		{
			return (19960614 - this.levelEnc) / 140933;
		}
		set
		{
			this.levelEnc = 19960614 - value * 140933;
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x060000C7 RID: 199 RVA: 0x00005761 File Offset: 0x00003961
	// (set) Token: 0x060000C6 RID: 198 RVA: 0x0000574C File Offset: 0x0000394C
	public int wave
	{
		get
		{
			return (19960614 - this.waveEnc) / 140933;
		}
		set
		{
			this.waveEnc = 19960614 - value * 140933;
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x060000C8 RID: 200 RVA: 0x00005775 File Offset: 0x00003975
	[SerializeField]
	public FactorBattle FactorBattle_FromDifficultyLevel
	{
		get
		{
			return GameParameters.Inst.difficultyLevel_FactorBattle[this.diffiLevel];
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x060000C9 RID: 201 RVA: 0x00005788 File Offset: 0x00003988
	public FactorBattle factorBattleTotal
	{
		get
		{
			return this.factorBattle_FromDifficultyOption * this.FactorBattle_FromDifficultyLevel * FactorBattle.FactorBattleThisLevel();
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x060000CA RID: 202 RVA: 0x000057A5 File Offset: 0x000039A5
	public int DiffiLevelMax
	{
		get
		{
			return GameParameters.Inst.difficultyLevel_FactorBattle.Length - 1;
		}
	}

	// Token: 0x060000CB RID: 203 RVA: 0x000057B8 File Offset: 0x000039B8
	public void InitNewGame()
	{
		this.Fragment = 0L;
		this.Score = 0L;
		this.fragmentTotal = 0L;
		this.FragmentUsed = 0L;
		this.battle_EnterVersion = GameParameters.Inst.version;
		this.listUpgradeInt = new List<int>();
		this.bulletEffect = new bool[DataBase.Inst.NumBulletEffect];
		this.specialEffect = new int[DataBase.Inst.NumSpecialEffect];
		this.myRandom = new MyRandom();
		this.myRandom.InitRandomLists();
		this.upgradeShop = new UpgradeShop();
		this.level = 1;
		this.wave = 0;
		this.diffiLevel = 0;
		if (TempData.inst.diffiOptFlag[12])
		{
			this.diffiLevel = 8;
		}
		this.upgradeShop_IfLocked = false;
		this.upgradeShop_RefreshPrice = 0;
		this.InitListLevelTypes();
		this.RandomLevelColor();
	}

	// Token: 0x060000CC RID: 204 RVA: 0x0000589C File Offset: 0x00003A9C
	private void InitListLevelTypes()
	{
		this.ListLevelTypes = new List<EnumLevelType>();
		for (int i = 0; i < 3; i++)
		{
			this.ListLevelTypes.Add((EnumLevelType)i);
		}
		this.ListLevelTypes = this.ListLevelTypes.Shuffle<EnumLevelType>();
		this.ListLevelTypes.Add(EnumLevelType.FINAL);
		foreach (EnumLevelType enumLevelType in this.ListLevelTypes)
		{
		}
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00005928 File Offset: 0x00003B28
	public int GetUpgradeNumWithID(int id)
	{
		int num = 0;
		using (List<int>.Enumerator enumerator = this.listUpgradeInt.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == id)
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00005980 File Offset: 0x00003B80
	public void UpdateBattleFacs()
	{
		int num = DataBase.Inst.Data_DifficultyOptions.Length;
		for (int i = 0; i < num; i++)
		{
			bool flag = TempData.inst.diffiOptFlag[i];
			DifficultyOption difficultyOption = DataBase.Inst.Data_DifficultyOptions[i];
			if (flag)
			{
				foreach (DifficultyOption.Fac fac in difficultyOption.facs)
				{
					int type = fac.type;
					float num2 = fac.num;
					if (type != -1)
					{
						if (type >= 0 && type <= 20)
						{
							this.factorBattle_FromDifficultyOption.factors[type] *= (double)num2;
						}
						else if (type >= 100 && type <= 199)
						{
							if (MyTool.ifIsAddFactorID(type - 100))
							{
								this.FacMulPlayer.factorMultis[type - 100] += num2;
							}
							else
							{
								this.FacMulPlayer.factorMultis[type - 100] *= num2;
							}
						}
						else if (type >= 200 && type <= 299)
						{
							if (MyTool.ifIsAddFactorID(type - 200))
							{
								this.FacMulEnemy.factorMultis[type - 200] += num2;
							}
							else
							{
								this.FacMulEnemy.factorMultis[type - 200] *= num2;
							}
						}
						else
						{
							Debug.LogError("DifficultyOptionTypeError!:" + type);
						}
					}
				}
			}
		}
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00005AFC File Offset: 0x00003CFC
	public void Fragment_Gain(int num)
	{
		this.Fragment += (long)num;
		if (num > 0)
		{
			this.fragmentTotal += (long)num;
			MySteamAchievement.AddStatInt("acc_Fragments", num);
		}
		BattleMapCanvas.inst.UpdateFragmentNum();
		UI_Panel_Battle_UpgradeShop panel_UpgradeShop = BattleMapCanvas.inst.panel_UpgradeShop;
		if (panel_UpgradeShop.gameObject.activeSelf)
		{
			panel_UpgradeShop.UpdateLanguages();
		}
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00005B68 File Offset: 0x00003D68
	public void Fragment_Spend(int num)
	{
		this.Fragment -= (long)num;
		if (num > 0)
		{
			this.FragmentUsed += (long)num;
		}
		BattleMapCanvas.inst.UpdateFragmentNum();
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x00005B98 File Offset: 0x00003D98
	public void Score_Gain(float num)
	{
		if (Player.inst == null)
		{
			Debug.LogWarning("玩家已死亡 不加星星");
			return;
		}
		int num2 = MyTool.DecimalToInt(num * this.factorBattleTotal.StarGain);
		this.Score += (long)num2;
		GameData inst = GameData.inst;
		inst.starTotal += (long)num2;
		BattleMapCanvas.inst.UpdateScoreNum();
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00005C08 File Offset: 0x00003E08
	public void Upgrade_Gain(int id)
	{
		if (id == 166)
		{
			BattleMapCanvas.inst.panel_UpgradeShop.mysteryBag.Init();
			return;
		}
		if (id == 208)
		{
			if (this.diffiLevel == 0)
			{
				Debug.LogError("Error_F级吃不了后悔药");
				return;
			}
			this.diffiLevel--;
			UI_FloatTextControl.inst.Special_DiffiLevelDown();
			return;
		}
		else
		{
			if (id == 210)
			{
				this.Upgrade_Gain(7);
				this.Upgrade_Gain(8);
				return;
			}
			if (id == 209)
			{
				this.Upgrade_Gain(68);
				this.Upgrade_Gain(69);
				this.Upgrade_Gain(70);
				this.Upgrade_Gain(73);
				this.Upgrade_Gain(74);
				return;
			}
			if (id == 211)
			{
				this.upgradeShop.UpdateShop();
				UpgradeShop.Goods[] upgradeGoods = this.upgradeShop.upgradeGoods;
				for (int i = 0; i < upgradeGoods.Length; i++)
				{
					upgradeGoods[i].price = 0;
				}
				if (UI_Panel_Battle_UpgradeShop.inst && UI_Panel_Battle_UpgradeShop.inst.gameObject.activeSelf)
				{
					UI_Panel_Battle_UpgradeShop.inst.Init();
				}
				return;
			}
			if (id == 212)
			{
				for (int j = 0; j < 3; j++)
				{
					this.Upgrade_Gain(134);
				}
				return;
			}
			if (id == 213)
			{
				this.Upgrade_Gain(189);
				this.Upgrade_Gain(189);
				return;
			}
			if (id == 214)
			{
				this.Upgrade_Gain(100);
				this.Upgrade_Gain(92);
				this.Upgrade_Gain(97);
				this.Upgrade_Gain(71);
				return;
			}
			if (id == 215)
			{
				this.Upgrade_Gain(41);
				this.Upgrade_Gain(40);
				this.Upgrade_Gain(37);
				return;
			}
			Player.inst.UpdateFactorTotal(true);
			if (id < 0)
			{
				Debug.LogError("UpgradeID<0! error!");
				return;
			}
			BasicUnit unit = Player.inst.unit;
			int num = unit.playerFactorTotal.lifeMaxPlayer - (int)unit.life;
			Player inst = Player.inst;
			int num2 = inst.Get_MaxShields() - inst.shield;
			this.listUpgradeInt.Add(id);
			UI_FloatTextControl.inst.Special_GainUpgrade(id);
			Player.inst.UpdateFactorTotal(true);
			unit.life = (double)Mathf.Max(1, unit.playerFactorTotal.lifeMaxPlayer - num);
			inst.shield = inst.Get_MaxShields() - num2;
			HealthPointControl.inst.UpdateHpUnits();
			if (id == 59)
			{
				Battle.inst.upgradeShop_RefreshFreeChance = 1;
			}
			BuffManager.RefreshAllStateBuff();
			GameData.inst.record.Upgrade_GainOnce(id);
			return;
		}
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00005E68 File Offset: 0x00004068
	public void RemoveUpgrade(int id)
	{
		BasicUnit unit = Player.inst.unit;
		Player inst = Player.inst;
		int num = unit.playerFactorTotal.lifeMaxPlayer - (int)unit.life;
		int num2 = inst.Get_MaxShields() - inst.shield;
		if (!this.listUpgradeInt.Contains(id))
		{
			Debug.LogError("没有升级 " + id + " !");
			return;
		}
		this.listUpgradeInt.Remove(id);
		this.GetFactorMultis_Upgrates_CurBattle();
		UI_FloatTextControl.inst.Special_DeleteUpgrade(id);
		Player.inst.UpdateFactorTotal(true);
		unit.life = (double)Mathf.Max(1, unit.playerFactorTotal.lifeMaxPlayer - num);
		inst.shield = inst.Get_MaxShields() - num2;
		HealthPointControl.inst.UpdateHpUnits();
		BuffManager.RefreshAllStateBuff();
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x00005F34 File Offset: 0x00004134
	public FactorMultis GetFactorMultis_Upgrates_CurBattle()
	{
		DataBase inst = DataBase.Inst;
		this.ForShow_UpgradeNum = new int[inst.Data_Upgrades.Length];
		this.upgrade_NormalNum = 0;
		this.upgrade_RareNum = 0;
		this.upgrade_EpicNum = 0;
		this.upgrade_LegendNum = 0;
		this.bulletEffect = new bool[inst.NumBulletEffect];
		this.specialEffect = new int[DataBase.Inst.NumSpecialEffect];
		FactorMultis factorMultis = new FactorMultis();
		foreach (int num in Battle.inst.listUpgradeInt)
		{
			this.ForShow_UpgradeNum[num]++;
			Upgrade upgrade = inst.Data_Upgrades[num];
			if (upgrade.rank == EnumRank.NORMAL)
			{
				this.upgrade_NormalNum++;
			}
			if (upgrade.rank == EnumRank.RARE)
			{
				this.upgrade_RareNum++;
			}
			if (upgrade.rank == EnumRank.EPIC)
			{
				this.upgrade_EpicNum++;
			}
			if (upgrade.rank == EnumRank.LEGENDARY)
			{
				this.upgrade_LegendNum++;
			}
			if (upgrade.bulletEffectID != -1)
			{
				this.bulletEffect[upgrade.bulletEffectID] = true;
			}
			if (upgrade.specialEffectID != -1)
			{
				this.specialEffect[upgrade.specialEffectID]++;
			}
			for (int i = 0; i < upgrade.facs.Length; i++)
			{
				Upgrade.Fac fac = upgrade.facs[i];
				if (fac.type >= 0)
				{
					factorMultis.MultiOneFactor(fac.type, fac.NumPlus);
				}
			}
		}
		return factorMultis;
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x000060F0 File Offset: 0x000042F0
	public void RandomLevelColor()
	{
		if (this.DIY_EnemyColor)
		{
			this.levelColor = this.levelColor.SetSaturation(0.8f).SetValue(0.9f);
			return;
		}
		int varColorId = TempData.inst.varColorId;
		Color colorRGB = DataBase.Inst.Data_VarColors[varColorId].ColorRGB;
		float num = 0.16666667f;
		float num2 = colorRGB.Hue();
		float num3 = Random.Range(num2 + num, num2 + 1f - num);
		if (num3 > 1f)
		{
			num3 -= 1f;
		}
		this.levelColor = Color.red.SetHue(num3).SetSaturation(0.8f).SetValue(0.9f);
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00006198 File Offset: 0x00004398
	public void NewWave()
	{
		if (this.specialEffect[9] >= 1)
		{
			this.upgradeShop_RefreshFreeChance = 1;
		}
		else
		{
			this.upgradeShop_RefreshFreeChance = 0;
		}
		EnumModeType modeType = TempData.inst.modeType;
		if (modeType != EnumModeType.NORMAL)
		{
			if (modeType - EnumModeType.ENDLESS <= 1)
			{
				Debug.Log("无尽或漫游模式 NewWave");
				int num = this.wave;
				this.wave = num + 1;
			}
			else
			{
				Debug.LogError("模式错误");
			}
		}
		else
		{
			Debug.Log("普通模式 NewWave");
			int num = this.wave;
			this.wave = num + 1;
			if (this.wave >= 6)
			{
				this.wave -= 5;
				num = this.level;
				this.level = num + 1;
			}
		}
		if (this.wave >= 9999)
		{
			this.wave = 9999;
		}
		this.RandomLevelColor();
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x0000625C File Offset: 0x0000445C
	public bool IsFirstWave()
	{
		return this.level == 1 && this.wave == 1;
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00006273 File Offset: 0x00004473
	public int GetLevelWaveNum()
	{
		return this.SequalWave;
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x0000627B File Offset: 0x0000447B
	public static bool IfHasSpecialEffect(int id)
	{
		return Battle.inst.specialEffect[id] >= 1;
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00006290 File Offset: 0x00004490
	public int GetSplitUpgradeCount()
	{
		int num = 0;
		for (int i = 0; i < 5; i++)
		{
			if (this.ForShow_UpgradeNum[i] >= 1)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060000DB RID: 219 RVA: 0x000062BB File Offset: 0x000044BB
	public void Store_TryUnlock()
	{
		if (this.upgradeShop_IfLocked)
		{
			this.Store_SetLock(false);
		}
	}

	// Token: 0x060000DC RID: 220 RVA: 0x000062CC File Offset: 0x000044CC
	public void Store_SetLock(bool flag)
	{
		if (flag == this.upgradeShop_IfLocked)
		{
			return;
		}
		this.upgradeShop_IfLocked = flag;
		Debug.Log("待补充：商店解锁/上锁提示");
		LanguageText.FloatText floatText = LanguageText.Inst.floatText;
		if (this.upgradeShop_IfLocked)
		{
			UI_FloatTextControl.inst.Special_AnyString(floatText.upgradeStore_Locked);
			return;
		}
		UI_FloatTextControl.inst.Special_AnyString(floatText.upgradeStore_Unlocked);
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00006328 File Offset: 0x00004528
	public bool GetBool_JusticeBlade()
	{
		return this.specialEffect[83] >= 1;
	}

	// Token: 0x0400011D RID: 285
	public Color levelColor = Color.red;

	// Token: 0x0400011E RID: 286
	public int battle_EnterVersion;

	// Token: 0x0400011F RID: 287
	[CustomLabel("总碎片")]
	[SerializeField]
	public ObscuredLong fragmentTotal = 0L;

	// Token: 0x04000120 RID: 288
	[CustomLabel("使用碎片")]
	[SerializeField]
	private ObscuredLong fragmentUsedEnc = 0L;

	// Token: 0x04000121 RID: 289
	[CustomLabel("碎片")]
	[SerializeField]
	private ObscuredLong fragmentEnc = 0L;

	// Token: 0x04000122 RID: 290
	[Header("分数")]
	[SerializeField]
	public ObscuredLong scoreEnc = 0L;

	// Token: 0x04000123 RID: 291
	public bool upgradeShop_IfLocked;

	// Token: 0x04000124 RID: 292
	public int noUse;

	// Token: 0x04000125 RID: 293
	public int upgradeShop_RefreshFreeChance;

	// Token: 0x04000126 RID: 294
	public ObscuredInt upgradeShop_RefreshPriceData = 0;

	// Token: 0x04000127 RID: 295
	[SerializeField]
	public UpgradeShop upgradeShop;

	// Token: 0x04000128 RID: 296
	[Header("Level")]
	[SerializeField]
	public List<EnumLevelType> ListLevelTypes = new List<EnumLevelType>();

	// Token: 0x04000129 RID: 297
	public int levelEnc = -1;

	// Token: 0x0400012A RID: 298
	public int waveEnc = -1;

	// Token: 0x0400012B RID: 299
	[Header("Upgrades")]
	[SerializeField]
	public List<int> listUpgradeInt = new List<int>();

	// Token: 0x0400012C RID: 300
	[SerializeField]
	public bool[] bulletEffect = new bool[100];

	// Token: 0x0400012D RID: 301
	[SerializeField]
	public int[] specialEffect = new int[100];

	// Token: 0x0400012E RID: 302
	[SerializeField]
	public int[] ForShow_UpgradeNum = new int[100];

	// Token: 0x0400012F RID: 303
	public int upgrade_NormalNum;

	// Token: 0x04000130 RID: 304
	public int upgrade_RareNum;

	// Token: 0x04000131 RID: 305
	public int upgrade_EpicNum;

	// Token: 0x04000132 RID: 306
	public int upgrade_LegendNum;

	// Token: 0x04000133 RID: 307
	[Header("Difficulty")]
	public int diffiLevel;

	// Token: 0x04000134 RID: 308
	[SerializeField]
	public FactorBattle factorBattle_FromDifficultyOption = new FactorBattle();

	// Token: 0x04000135 RID: 309
	public FactorMultis FacMulEnemy = new FactorMultis();

	// Token: 0x04000136 RID: 310
	public FactorMultis FacMulPlayer = new FactorMultis();

	// Token: 0x04000137 RID: 311
	[Header("Random")]
	[SerializeField]
	public MyRandom myRandom = new MyRandom();

	// Token: 0x04000138 RID: 312
	[Header("DIY")]
	public bool DIY_EnemyColor;

	// Token: 0x04000139 RID: 313
	public bool DIY_BulletAlpha_BoolFlag;

	// Token: 0x0400013A RID: 314
	public float DIY_BulletAlpha_Float = 1f;
}
