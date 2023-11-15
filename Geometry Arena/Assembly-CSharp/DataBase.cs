using System;
using System.Data;
using System.IO;
using Excel;
using UnityEngine;

// Token: 0x02000003 RID: 3
[CreateAssetMenu(fileName = "DataBase", menuName = "CreateAsset/DataBase")]
public class DataBase : ScriptableObject
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000002 RID: 2 RVA: 0x0000206F File Offset: 0x0000026F
	public static DataBase Inst
	{
		get
		{
			if (AssetManager.inst != null)
			{
				return AssetManager.inst.dataBase;
			}
			return Resources.Load<DataBase>("Assets/DataBase");
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000003 RID: 3 RVA: 0x00002093 File Offset: 0x00000293
	public int NumBulletEffect
	{
		get
		{
			return this.numBulletEffect;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000004 RID: 4 RVA: 0x0000209B File Offset: 0x0000029B
	public int NumSpecialEffect
	{
		get
		{
			return this.numSpecialEffect;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000005 RID: 5 RVA: 0x000020A3 File Offset: 0x000002A3
	public VarColor[] Data_VarColors
	{
		get
		{
			return this.dataVarColors;
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000006 RID: 6 RVA: 0x000020AB File Offset: 0x000002AB
	public EnemyModel[] Data_EnemyModels
	{
		get
		{
			return this.dataEnemyModels;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000007 RID: 7 RVA: 0x000020B3 File Offset: 0x000002B3
	public PlayerModel[] DataPlayerModels
	{
		get
		{
			return this.dataPlayerModels;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000008 RID: 8 RVA: 0x000020BB File Offset: 0x000002BB
	public Upgrade[] Data_Upgrades
	{
		get
		{
			return this.dataUpgrades;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000009 RID: 9 RVA: 0x000020C3 File Offset: 0x000002C3
	public DifficultyOption[] Data_DifficultyOptions
	{
		get
		{
			return this.dataDifficultyOptions;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600000A RID: 10 RVA: 0x000020CB File Offset: 0x000002CB
	public Upgrade_BulletEffect[] Data_Upgrade_BulletEffects
	{
		get
		{
			return this.dataUpgradeBulletEffects;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600000B RID: 11 RVA: 0x000020D3 File Offset: 0x000002D3
	public BattleBuff[] Data_BattleBuffs
	{
		get
		{
			return this.dataBattleBuffs;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600000C RID: 12 RVA: 0x000020DB File Offset: 0x000002DB
	public RuneData[] Data_RuneDatas
	{
		get
		{
			return this.dataRuneDatas;
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x0600000D RID: 13 RVA: 0x000020E3 File Offset: 0x000002E3
	public UpgradeType[] Data_UpgradeTypes
	{
		get
		{
			return this.dataUpgradeTypes;
		}
	}

	// Token: 0x0600000E RID: 14 RVA: 0x000020EC File Offset: 0x000002EC
	public void ReadAllData()
	{
		this.ReadXlsx_VarColor();
		this.ReadXlsx_EnemyModel();
		this.ReadXlsx_PlayerModel();
		this.ReadXlsx_Talent();
		this.ReadXlsx_Upgrade();
		this.ReadXlsx_DifficultyOptions();
		this.ReadXlsx_BattleBuffs();
		this.ReadXlsx_RuneData();
		this.ReadXlsx_UpgradeTypes();
		this.ReadXlsx_SkilModules();
		this.ReadXlsx_Guides();
		this.ReadXlsx_TutorialData();
		Debug.Log("数据读取完毕");
	}

	// Token: 0x0600000F RID: 15 RVA: 0x0000214C File Offset: 0x0000034C
	public void Editor_TestData()
	{
		foreach (BattleBuff battleBuff in this.dataBattleBuffs)
		{
			if (battleBuff.ifAvailable)
			{
				Debug.Log(battleBuff.GetInfoWithFac() + "\n");
			}
		}
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002190 File Offset: 0x00000390
	private void ReadXlsx_VarColor()
	{
		DataSet dataSet = ExcelReaderFactory.CreateOpenXmlReader(new FileStream(Application.dataPath + "/Datas/VarColors.xlsx", FileMode.Open, FileAccess.Read)).AsDataSet();
		int count = dataSet.Tables[0].Rows.Count;
		int num = count - 1;
		this.dataVarColors = new VarColor[num];
		VarColor[] array = this.dataVarColors;
		for (int i = 1; i < count; i++)
		{
			array[i - 1] = new VarColor(null);
			VarColor varColor = array[i - 1];
			DataRow dataRow = dataSet.Tables[0].Rows[i];
			varColor.no = i - 1;
			if (varColor.no != int.Parse(dataRow[0].ToString()))
			{
				Debug.LogError("Error_No");
			}
			varColor.dataName = dataRow[1].ToString();
			varColor.names = this.Language_ArrayRead(dataRow, 28, 2);
			varColor.hue = (float)int.Parse(dataRow[2].ToString());
			varColor.saturation = 70f;
			varColor.value = 90f;
			varColor.rank = (EnumRank)int.Parse(dataRow[5].ToString());
			varColor.useRange = (VarColor.EnumUseRange)int.Parse(dataRow[6].ToString());
			varColor.avaiPlayer = (this.DataRow_GetInt(dataRow, 7) == 1);
			varColor.avaiEnemy = (this.DataRow_GetInt(dataRow, 8) == 1);
			this.ReadFactorMultisFromDataRow_Standard(dataRow, ref varColor.factorMultis, 11);
		}
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002324 File Offset: 0x00000524
	private void ReadXlsx_EnemyModel()
	{
		DataSet dataSet = ExcelReaderFactory.CreateOpenXmlReader(new FileStream(Application.dataPath + "/Datas/EnemyModels.xlsx", FileMode.Open, FileAccess.Read)).AsDataSet();
		int count = dataSet.Tables[0].Rows.Count;
		int num = count - 1;
		this.dataEnemyModels = new EnemyModel[num];
		EnemyModel[] array = this.dataEnemyModels;
		for (int i = 1; i < count; i++)
		{
			array[i - 1] = new EnemyModel(null);
			EnemyModel enemyModel = array[i - 1];
			DataRow dataRow = dataSet.Tables[0].Rows[i];
			enemyModel.no = i - 1;
			if (enemyModel.no != int.Parse(dataRow[0].ToString()))
			{
				Debug.LogError("Error_No");
			}
			enemyModel.name = dataRow[1].ToString();
			enemyModel.rank = (EnumRank)int.Parse(dataRow[2].ToString());
			enemyModel.type = (EnemyModel.EnumEnemyType)this.DataRow_GetInt(dataRow, 4);
			enemyModel.ifAvailable = (this.DataRow_GetInt(dataRow, 5) == 1);
			enemyModel.enemyGeneration = new EnemyGeneration[4];
			for (int j = 0; j < 4; j++)
			{
				enemyModel.enemyGeneration[j] = new EnemyGeneration();
				EnemyGeneration enemyGeneration = enemyModel.enemyGeneration[j];
				enemyGeneration.waveStart = this.DataRow_GetInt(dataRow, 31 + 3 * j);
				enemyGeneration.waveEnd = this.DataRow_GetInt(dataRow, 32 + 3 * j);
				enemyGeneration.weight = this.DataRow_GetInt(dataRow, 33 + 3 * j);
			}
			enemyModel.factorMultis.factorMultis[0] = this.DataRow_GetFloat(dataRow, 11);
			enemyModel.factorMultis.factorMultis[2] = this.DataRow_GetFloat(dataRow, 12);
			enemyModel.factorMultis.factorMultis[3] = this.DataRow_GetFloat(dataRow, 13);
			if (enemyModel.factorMultis.factorMultis[3] == 0f)
			{
				enemyModel.factorMultis.factorMultis[3] = 1f;
			}
			enemyModel.factorMultis.factorMultis[8] = this.DataRow_GetFloat(dataRow, 18);
			enemyModel.splitType = this.DataRow_GetInt(dataRow, 8);
			enemyModel.summonType = this.DataRow_GetInt(dataRow, 9);
			enemyModel.names[0] = this.DataRow_GetString(dataRow, 44);
			enemyModel.names[1] = this.DataRow_GetString(dataRow, 43);
		}
	}

	// Token: 0x06000012 RID: 18 RVA: 0x0000258C File Offset: 0x0000078C
	private void ReadXlsx_PlayerModel()
	{
		DataSet dataSet = ExcelReaderFactory.CreateOpenXmlReader(new FileStream(Application.dataPath + "/Datas/PlayerModels.xlsx", FileMode.Open, FileAccess.Read)).AsDataSet();
		int count = dataSet.Tables[0].Rows.Count;
		int num = count - 1;
		this.dataPlayerModels = new PlayerModel[num];
		PlayerModel[] array = this.dataPlayerModels;
		for (int i = 1; i < count; i++)
		{
			array[i - 1] = new PlayerModel();
			PlayerModel playerModel = array[i - 1];
			DataRow dataRow = dataSet.Tables[0].Rows[i];
			playerModel.no = i - 1;
			if (playerModel.no != int.Parse(dataRow[0].ToString()))
			{
				Debug.LogError("Error_No");
			}
			playerModel.dataName = dataRow[2].ToString();
			this.ReadFactorMultisFromDataRow(dataRow, ref playerModel.factorMultis, 4);
			playerModel.ifAvailable = (this.DataRow_GetInt(dataRow, 3) == 1);
			playerModel.unlockPreJobId = this.DataRow_GetInt(dataRow, 22);
			playerModel.unlockPreJobLevel = this.DataRow_GetInt(dataRow, 23);
			playerModel.jobName = this.Language_ArrayRead(dataRow, 1, 2);
			playerModel.jobInfo = this.Language_ArrayRead(dataRow, 24, 2);
			playerModel.skillLevels = new Skill[3];
		}
		for (int j = 0; j <= 11; j++)
		{
			PlayerModel playerModel2 = this.dataPlayerModels[j];
			for (int k = 0; k <= 2; k++)
			{
				playerModel2.skillLevels[k] = new Skill();
				Skill skill = playerModel2.skillLevels[k];
				skill.level = k + 1;
				int index = j * 3 + 1 + k;
				DataRow d = dataSet.Tables[1].Rows[index];
				this.DataRow_GetInt(d, 1);
				skill.skillType = (EnumSkillType)this.DataRow_GetInt(d, 2);
				skill.skillNameArray = this.Language_ArrayRead(d, 3, 2);
				skill.skillInfoArray = this.Language_ArrayRead(d, 6, 2);
				for (int l = 0; l < 11; l++)
				{
					skill.facs[l] = this.DataRow_GetFloat(d, 8 + l);
				}
			}
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000027D0 File Offset: 0x000009D0
	private void ReadXlsx_BattleBuffs()
	{
		DataSet dataSet = ExcelReaderFactory.CreateOpenXmlReader(new FileStream(Application.dataPath + "/Datas/BattleBuffs.xlsx", FileMode.Open, FileAccess.Read)).AsDataSet();
		int count = dataSet.Tables[0].Rows.Count;
		int num = count - 1;
		this.dataBattleBuffs = new BattleBuff[num];
		BattleBuff[] array = this.dataBattleBuffs;
		for (int i = 1; i < count; i++)
		{
			array[i - 1] = new BattleBuff();
			BattleBuff battleBuff = array[i - 1];
			DataRow d = dataSet.Tables[0].Rows[i];
			battleBuff.typeID = i - 1;
			battleBuff.ifAvailable = (d.GetInt(5) == 1);
			battleBuff.dataName = d.GetString(2);
			battleBuff.names = this.Language_ArrayRead(d, 1, 2);
			battleBuff.infos = this.Language_ArrayRead(d, 6, 2);
			battleBuff.rank = (EnumRank)d.GetInt(4);
			battleBuff.facs = new float[10];
			for (int j = 0; j < 10; j++)
			{
				battleBuff.facs[j] = this.DataRow_GetFloat(d, 10 + j);
			}
			battleBuff.lifeTimeMax = battleBuff.facs[0];
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002918 File Offset: 0x00000B18
	private void ReadXlsx_Talent()
	{
		DataSet dataSet = ExcelReaderFactory.CreateOpenXmlReader(new FileStream(Application.dataPath + "/Datas/Talents.xlsx", FileMode.Open, FileAccess.Read)).AsDataSet();
		int num = this.DataPlayerModels.Length;
		for (int i = 0; i < num; i++)
		{
			int count = dataSet.Tables[i].Rows.Count;
			int num2 = count - 1;
			this.dataPlayerModels[i].talents = new Talent[num2];
			Talent[] talents = this.dataPlayerModels[i].talents;
			for (int j = 1; j < count; j++)
			{
				talents[j - 1] = new Talent();
				Talent talent = talents[j - 1];
				DataRow d = dataSet.Tables[i].Rows[j];
				talent.id = this.DataRow_GetInt(d, 0);
				talent.job = this.DataRow_GetInt(d, 1);
				if (talent.job != i)
				{
					Debug.LogError("JobCntError!");
					return;
				}
				talent.Name = this.DataRow_GetString(d, 2);
				talent.originLevel = this.DataRow_GetInt(d, 3);
				talent.maxLevel = this.DataRow_GetInt(d, 4);
				talent.price = this.DataRow_GetInt(d, 5);
				for (int k = 0; k <= 3; k++)
				{
					talent.facs[k] = new Talent.Fac();
					int num3 = this.DataRow_GetInt(d, 6 + k * 2);
					talent.facs[k].type = num3;
					float num4 = this.DataRow_GetFloat(d, 7 + k * 2);
					if (num3 >= 0 && num3 <= 14)
					{
						talent.facs[k].num = GameParameters.Inst.FacStandards[num3] * num4;
					}
					else
					{
						talent.facs[k].num = num4;
					}
				}
			}
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002AF0 File Offset: 0x00000CF0
	private void ReadXlsx_DifficultyOptions()
	{
		DataSet dataSet = ExcelReaderFactory.CreateOpenXmlReader(new FileStream(Application.dataPath + "/Datas/DifficultyOptions.xlsx", FileMode.Open, FileAccess.Read)).AsDataSet();
		int count = dataSet.Tables[0].Rows.Count;
		int num = count - 1;
		this.dataDifficultyOptions = new DifficultyOption[num];
		DifficultyOption[] array = this.dataDifficultyOptions;
		for (int i = 1; i < count; i++)
		{
			array[i - 1] = new DifficultyOption();
			DifficultyOption difficultyOption = array[i - 1];
			DataRow d = dataSet.Tables[0].Rows[i];
			difficultyOption.id = this.DataRow_GetInt(d, 0);
			difficultyOption.dataName = this.DataRow_GetString(d, 2);
			difficultyOption.names = this.Language_ArrayRead(d, 1, 2);
			difficultyOption.infos = this.Language_ArrayRead(d, 4, 2);
			for (int j = 0; j <= 6; j++)
			{
				difficultyOption.facs[j] = new DifficultyOption.Fac();
				difficultyOption.facs[j].type = this.DataRow_GetInt(d, 11 + j * 2);
				difficultyOption.facs[j].num = this.DataRow_GetFloat(d, 12 + j * 2);
			}
			difficultyOption.dailyChances = new float[1];
			for (int k = 0; k < 1; k++)
			{
				difficultyOption.dailyChances[k] = this.DataRow_GetFloat(d, 30 + k);
			}
		}
		count = dataSet.Tables[1].Rows.Count;
		num = count - 1;
		GameParameters.Inst.difficultyLevel_FactorBattle = new FactorBattle[num];
		FactorBattle[] difficultyLevel_FactorBattle = GameParameters.Inst.difficultyLevel_FactorBattle;
		for (int l = 1; l < count; l++)
		{
			difficultyLevel_FactorBattle[l - 1] = new FactorBattle();
			FactorBattle factorBattle = difficultyLevel_FactorBattle[l - 1];
			DataRow d2 = dataSet.Tables[1].Rows[l];
			factorBattle.name = this.DataRow_GetString(d2, 13);
			for (int m = 0; m <= 12; m++)
			{
				factorBattle.factors[m] = (double)this.DataRow_GetFloat(d2, m);
			}
		}
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002D10 File Offset: 0x00000F10
	private void ReadXlsx_RuneData()
	{
		DataSet dataSet = ExcelReaderFactory.CreateOpenXmlReader(new FileStream(Application.dataPath + "/Datas/Rune.xlsx", FileMode.Open, FileAccess.Read)).AsDataSet();
		int count = dataSet.Tables[0].Rows.Count;
		int num = count - 1;
		this.dataRuneDatas = new RuneData[num];
		RuneData[] array = this.dataRuneDatas;
		for (int i = 1; i < count; i++)
		{
			array[i - 1] = new RuneData();
			RuneData runeData = array[i - 1];
			DataRow d = dataSet.Tables[0].Rows[i];
			runeData.id = this.DataRow_GetInt(d, 0);
			runeData.dataName = this.DataRow_GetString(d, 2);
			runeData.nameArray = this.Language_ArrayRead(d, 1, 2);
			runeData.bigType = (EnumRunePropertyType)this.DataRow_GetInt(d, 7);
			runeData.smallType = this.DataRow_GetInt(d, 8);
			runeData.factorPlayer_IfPositive = (this.DataRow_GetInt(d, 9) == 1);
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002E10 File Offset: 0x00001010
	private void ReadXlsx_UpgradeTypes()
	{
		DataSet dataSet = ExcelReaderFactory.CreateOpenXmlReader(new FileStream(Application.dataPath + "/Datas/UpgradeType.xlsx", FileMode.Open, FileAccess.Read)).AsDataSet();
		int count = dataSet.Tables[0].Rows.Count;
		int num = count - 1;
		this.dataUpgradeTypes = new UpgradeType[num];
		UpgradeType[] array = this.dataUpgradeTypes;
		for (int i = 1; i < count; i++)
		{
			array[i - 1] = new UpgradeType();
			UpgradeType upgradeType = array[i - 1];
			DataRow d = dataSet.Tables[0].Rows[i];
			upgradeType.ID = this.DataRow_GetInt(d, 0);
			upgradeType.dataName = this.DataRow_GetString(d, 2);
			upgradeType.typeNames = this.Language_ArrayRead(d, 1, 2);
			upgradeType.typeInfos = this.Language_ArrayRead(d, 4, 2);
			upgradeType.ifAvailableInRune = (this.DataRow_GetInt(d, 15) == 1);
			float h = (float)this.DataRow_GetInt(d, 10) / 360f;
			float s = (float)this.DataRow_GetInt(d, 11) / 100f;
			float v = (float)this.DataRow_GetInt(d, 12) / 100f;
			upgradeType.typeColor = Color.HSVToRGB(h, s, v);
			upgradeType.facs = new float[6];
			for (int j = 0; j < 6; j++)
			{
				upgradeType.facs[j] = this.DataRow_GetFloat(d, 20 + j);
			}
		}
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002F88 File Offset: 0x00001188
	private void ReadXlsx_Upgrade()
	{
		DataSet dataSet = ExcelReaderFactory.CreateOpenXmlReader(new FileStream(Application.dataPath + "/Datas/Upgrades.xlsx", FileMode.Open, FileAccess.Read)).AsDataSet();
		int count = dataSet.Tables[0].Rows.Count;
		int num = count - 1;
		this.dataUpgrades = new Upgrade[num];
		Upgrade[] array = this.dataUpgrades;
		this.numBulletEffect = 0;
		this.numSpecialEffect = 0;
		for (int i = 1; i < count; i++)
		{
			array[i - 1] = new Upgrade();
			Upgrade upgrade = array[i - 1];
			DataRow d = dataSet.Tables[0].Rows[i];
			upgrade.id = this.DataRow_GetInt(d, 0);
			upgrade.dataName = this.DataRow_GetString(d, 1);
			upgrade.names = this.Language_ArrayRead(d, 1, 2);
			upgrade.infos = this.Language_ArrayRead(d, 7, 2);
			upgrade.rank = (EnumRank)this.DataRow_GetInt(d, 3);
			upgrade.ifAvailable = (this.DataRow_GetInt(d, 4) == 1);
			upgrade.numMax = this.DataRow_GetInt(d, 5);
			upgrade.orderID = this.DataRow_GetFloat(d, 40);
			upgrade.buffFacs = new float[10];
			for (int j = 0; j < 10; j++)
			{
				upgrade.buffFacs[j] = this.DataRow_GetFloat(d, 26 + j);
			}
			upgrade.bulletEffectID = this.DataRow_GetInt(d, 9);
			upgrade.specialEffectID = this.DataRow_GetInt(d, 10);
			if (upgrade.bulletEffectID + 1 > this.numBulletEffect)
			{
				this.numBulletEffect = upgrade.bulletEffectID + 1;
			}
			if (upgrade.specialEffectID + 1 > this.numSpecialEffect)
			{
				this.numSpecialEffect = upgrade.specialEffectID + 1;
			}
			for (int k = 0; k <= 2; k++)
			{
				upgrade.facs[k] = new Upgrade.Fac();
				upgrade.facs[k].type = this.DataRow_GetInt(d, 11 + k * 2);
				upgrade.facs[k].numUnit = this.DataRow_GetFloat(d, 12 + k * 2);
			}
			for (int l = 0; l <= 2; l++)
			{
				upgrade.upgradeIntTypes[l] = this.DataRow_GetInt(d, 36 + l);
			}
		}
		this.dataUpgradeBulletEffects = new Upgrade_BulletEffect[this.numBulletEffect];
		for (int m = 1; m < count; m++)
		{
			Upgrade upgrade2 = array[m - 1];
			DataRow d2 = dataSet.Tables[0].Rows[m];
			if (upgrade2.bulletEffectID >= 0)
			{
				int bulletEffectID = upgrade2.bulletEffectID;
				this.dataUpgradeBulletEffects[bulletEffectID] = new Upgrade_BulletEffect();
				Upgrade_BulletEffect upgrade_BulletEffect = this.dataUpgradeBulletEffects[bulletEffectID];
				upgrade_BulletEffect.name = upgrade2.dataName;
				upgrade_BulletEffect.bulletEffectType = (Upgrade_BulletEffect.EnumBulletEffect)this.DataRow_GetInt(d2, 17);
				upgrade_BulletEffect.bulletEffectTrigRange = this.DataRow_GetFloat(d2, 18);
				for (int n = 0; n <= 2; n++)
				{
					upgrade_BulletEffect.bulletEffectFacs[n] = new Upgrade_BulletEffect.Fac();
					upgrade_BulletEffect.bulletEffectFacs[n].type = this.DataRow_GetInt(d2, 19 + n * 2);
					upgrade_BulletEffect.bulletEffectFacs[n].numMul = this.DataRow_GetFloat(d2, 20 + n * 2);
				}
				upgrade2.bulletEffect = upgrade_BulletEffect;
			}
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x000032E4 File Offset: 0x000014E4
	private void ReadXlsx_SkilModules()
	{
		DataSet dataSet = ExcelReaderFactory.CreateOpenXmlReader(new FileStream(Application.dataPath + "/Datas/SkillModules.xlsx", FileMode.Open, FileAccess.Read)).AsDataSet();
		for (int i = 0; i < this.dataPlayerModels.Length; i++)
		{
			DataTable dataTable = dataSet.Tables[i];
			PlayerModel playerModel = this.dataPlayerModels[i];
			int count = dataTable.Rows.Count;
			int num = count - 1;
			playerModel.skillModules = new SkillModule[num];
			SkillModule[] skillModules = playerModel.skillModules;
			for (int j = 1; j < count; j++)
			{
				skillModules[j - 1] = new SkillModule();
				SkillModule skillModule = skillModules[j - 1];
				DataRow dataRow = dataSet.Tables[i].Rows[j];
				skillModule.orderID = this.DataRow_GetInt(dataRow, 0);
				skillModule.effectID = this.DataRow_GetInt(dataRow, 1);
				skillModule.levelNeed = this.DataRow_GetInt(dataRow, 2);
				skillModule.dataName = this.DataRow_GetString(dataRow, 4);
				skillModule.skillModuleNames = this.Language_ArrayRead(dataRow, 3, 2);
				skillModule.skillModuleInfos = this.Language_ArrayRead(dataRow, 6, 2);
				this.ReadFactorMultisFromDataRow(dataRow, ref skillModule.factorMultis, 11);
				for (int k = 0; k < 10; k++)
				{
					skillModule.facs[k] = this.DataRow_GetFloat(dataRow, 27 + k);
				}
				if (skillModule.effectID > playerModel.skillModule_MaxEffectID)
				{
					playerModel.skillModule_MaxEffectID = skillModule.effectID;
				}
			}
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00003464 File Offset: 0x00001664
	private void ReadXlsx_Guides()
	{
		DataSet dataSet = ExcelReaderFactory.CreateOpenXmlReader(new FileStream(Application.dataPath + "/Datas/Guide.xlsx", FileMode.Open, FileAccess.Read)).AsDataSet();
		int count = dataSet.Tables[0].Rows.Count;
		int num = count - 1;
		this.dataGuides = new Guide[num];
		Guide[] array = this.dataGuides;
		for (int i = 1; i < count; i++)
		{
			array[i - 1] = new Guide();
			Guide guide = array[i - 1];
			DataRow d = dataSet.Tables[0].Rows[i];
			guide.dataID = this.DataRow_GetInt(d, 0);
			guide.ifAvailable = (this.DataRow_GetInt(d, 3) == 1);
			guide.dataName = this.DataRow_GetString(d, 2);
			guide.orderNo = this.DataRow_GetInt(d, 6);
			guide.names = this.Language_ArrayRead(d, 1, 2);
			guide.infos = this.Language_ArrayRead(d, 4, 2);
		}
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00003560 File Offset: 0x00001760
	private void ReadXlsx_TutorialData()
	{
		DataSet dataSet = ExcelReaderFactory.CreateOpenXmlReader(new FileStream(Application.dataPath + "/Datas/Tutorial.xlsx", FileMode.Open, FileAccess.Read)).AsDataSet();
		int count = dataSet.Tables[0].Rows.Count;
		int num = count - 1;
		this.dataTutorials = new TutorialData[num];
		TutorialData[] array = this.dataTutorials;
		for (int i = 1; i < count; i++)
		{
			array[i - 1] = new TutorialData();
			TutorialData tutorialData = array[i - 1];
			DataRow d = dataSet.Tables[0].Rows[i];
			tutorialData.ID = this.DataRow_GetInt(d, 0);
			tutorialData.preID = this.DataRow_GetInt(d, 1);
			tutorialData.posType = this.DataRow_GetInt(d, 6);
			tutorialData.name.SetStrings(this.DataRow_GetString(d, 2), this.DataRow_GetString(d, 3));
			tutorialData.info.SetStrings(this.DataRow_GetString(d, 4), this.DataRow_GetString(d, 5));
		}
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00003664 File Offset: 0x00001864
	private void ReadFactorMultisFromDataRow(DataRow dataRow, ref FactorMultis factorMultis, int startColumn)
	{
		factorMultis = new FactorMultis();
		for (int i = 1; i <= 15; i++)
		{
			factorMultis.factorMultis[i] = this.DataRow_GetFloat(dataRow, startColumn + i - 1);
		}
	}

	// Token: 0x0600001D RID: 29 RVA: 0x0000369C File Offset: 0x0000189C
	private void ReadFactorMultisFromDataRow_Standard(DataRow dataRow, ref FactorMultis factorMultis, int startColumn)
	{
		factorMultis = new FactorMultis();
		factorMultis.factorMultis = new float[16];
		float[] facStandards = this.gameParameters.FacStandards;
		for (int i = 1; i <= 15; i++)
		{
			if ((i >= 10 && i <= 13) || i == 1)
			{
				factorMultis.factorMultis[i] = 0f;
			}
			else
			{
				factorMultis.factorMultis[i] = 1f;
			}
			factorMultis.factorMultis[i] += this.DataRow_GetFloat(dataRow, startColumn + i - 1) * facStandards[i];
		}
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00003724 File Offset: 0x00001924
	private string DataRow_GetString(DataRow d, int num)
	{
		return d[num].ToString();
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00003732 File Offset: 0x00001932
	private int DataRow_GetInt(DataRow d, int num)
	{
		return int.Parse(d[num].ToString());
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00003745 File Offset: 0x00001945
	private float DataRow_GetFloat(DataRow d, int num)
	{
		return float.Parse(d[num].ToString());
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00003758 File Offset: 0x00001958
	public static Talent GetTalent(int jobID, int talentID)
	{
		return DataBase.Inst.DataPlayerModels[jobID].talents[talentID];
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00003770 File Offset: 0x00001970
	private string[] Language_ArrayRead(DataRow d, int startNum, int num)
	{
		string[] array = new string[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = d[startNum + i].ToString().Replace("\\n", "\n");
		}
		return array;
	}

	// Token: 0x04000003 RID: 3
	public int noUse = 1;

	// Token: 0x04000004 RID: 4
	[SerializeField]
	private GameParameters gameParameters;

	// Token: 0x04000005 RID: 5
	[SerializeField]
	private int numBulletEffect;

	// Token: 0x04000006 RID: 6
	[SerializeField]
	private int numSpecialEffect;

	// Token: 0x04000007 RID: 7
	[SerializeField]
	private VarColor[] dataVarColors;

	// Token: 0x04000008 RID: 8
	[SerializeField]
	private EnemyModel[] dataEnemyModels;

	// Token: 0x04000009 RID: 9
	[SerializeField]
	private PlayerModel[] dataPlayerModels;

	// Token: 0x0400000A RID: 10
	[SerializeField]
	private Upgrade[] dataUpgrades;

	// Token: 0x0400000B RID: 11
	[SerializeField]
	private DifficultyOption[] dataDifficultyOptions;

	// Token: 0x0400000C RID: 12
	[SerializeField]
	private Upgrade_BulletEffect[] dataUpgradeBulletEffects;

	// Token: 0x0400000D RID: 13
	[SerializeField]
	private BattleBuff[] dataBattleBuffs;

	// Token: 0x0400000E RID: 14
	[SerializeField]
	private RuneData[] dataRuneDatas;

	// Token: 0x0400000F RID: 15
	[SerializeField]
	private UpgradeType[] dataUpgradeTypes;

	// Token: 0x04000010 RID: 16
	[SerializeField]
	public Guide[] dataGuides;

	// Token: 0x04000011 RID: 17
	[SerializeField]
	public TutorialData[] dataTutorials;
}
