using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

// Token: 0x0200001B RID: 27
public class TestTool : MonoBehaviour
{
	// Token: 0x0600014F RID: 335 RVA: 0x000097AC File Offset: 0x000079AC
	public void AddGeometryCoin()
	{
		GameData.inst.GeometryCoin_Get((long)this.addGCNum);
	}

	// Token: 0x06000150 RID: 336 RVA: 0x000097C0 File Offset: 0x000079C0
	public void GeneFrags()
	{
		for (int i = 0; i < 100; i++)
		{
			GameObject prefab_Fragment = ResourceLibrary.Inst.Prefab_Fragment;
			Fragment.InstantiateFragments(BattleManager.ChooseBattleItemGenePosInScene(), 5, EnumShapeType.CIRCLE, Color.yellow, 1f);
		}
	}

	// Token: 0x06000151 RID: 337 RVA: 0x000097FC File Offset: 0x000079FC
	public void QuickSaveData()
	{
		GameData.inst.GetStar(this.quickSavedata_Star);
		for (int i = 0; i < GameData.inst.jobs.Length; i++)
		{
			GameData.inst.jobs[i].mastery.exps = (long)this.quickSavedata_Master;
		}
	}

	// Token: 0x06000152 RID: 338 RVA: 0x0000984D File Offset: 0x00007A4D
	public void SetWave()
	{
		Battle.inst.wave = this.waveToSet;
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00009860 File Offset: 0x00007A60
	public void NewBattleBuffType()
	{
		Vector2 v = BattleManager.ChooseBattleItemGenePosInScene();
		Object.Instantiate<GameObject>(ResourceLibrary.Inst.Prefab_BattleItem, v, Quaternion.identity).GetComponent<BattleItem>().Init(this.battleBuffType);
	}

	// Token: 0x06000154 RID: 340 RVA: 0x000098A0 File Offset: 0x00007AA0
	public void NewBattleBuffTypeAll()
	{
		foreach (BattleBuff battleBuff in DataBase.Inst.Data_BattleBuffs)
		{
			if (battleBuff.ifAvailable)
			{
				Vector2 v = BattleManager.ChooseBattleItemGenePosInScene();
				Object.Instantiate<GameObject>(ResourceLibrary.Inst.Prefab_BattleItem, v, Quaternion.identity).GetComponent<BattleItem>().Init(battleBuff.typeID);
			}
		}
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00009904 File Offset: 0x00007B04
	public void TongjiUpgrade()
	{
		int[] array = new int[4];
		int[] array2 = new int[DataBase.Inst.Data_Upgrades.Length];
		for (int i = 0; i < 10000; i++)
		{
			Upgrade upgrade_Random = DataSelector.GetUpgrade_Random();
			int rank = (int)upgrade_Random.rank;
			int id = upgrade_Random.id;
			array[rank]++;
			array2[id]++;
		}
		Debug.Log(string.Concat(new object[]
		{
			array[0],
			" 稀有",
			array[1],
			" 史诗",
			array[2],
			" 传说",
			array[3]
		}));
		string text = "";
		for (int j = 0; j < array2.Length; j++)
		{
			text += string.Format("UpID {0} 的数量={1}\n", j, array2[j]);
		}
		Debug.Log("upID数量统计:\n" + text);
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00009A08 File Offset: 0x00007C08
	public void Player_Wudi()
	{
		BasicUnit unit = Player.inst.unit;
		unit.FactorBasic.factor[1] = 1000000.0;
		unit.life = unit.LifeMax;
		ObscuredDouble[] factor = unit.FactorBasic.factor;
		int num = 4;
		factor[num] *= 100.0;
		ObscuredDouble[] factor2 = unit.FactorBasic.factor;
		int num2 = 3;
		factor2[num2] *= 100.0;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00009AAC File Offset: 0x00007CAC
	public void Player_Fast()
	{
		BasicUnit unit = Player.inst.unit;
		unit.FactorBasic.factor[6] = 100.0;
		unit.FactorBasic.factor[7] = 2333.0;
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00009B00 File Offset: 0x00007D00
	public void GainAllSplit()
	{
		for (int i = 0; i <= 4; i++)
		{
			Battle.inst.Upgrade_Gain(i);
		}
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00009B24 File Offset: 0x00007D24
	public void ApplySetting()
	{
		Setting.Inst.ApplySettingSuddenly();
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00009B30 File Offset: 0x00007D30
	public void GoldenFinger()
	{
		if (TempData.inst.currentSceneType == EnumSceneType.BATTLE)
		{
			Battle.inst.Fragment_Gain(10000000);
		}
		GameData.inst.Star += 100000000000L;
	}

	// Token: 0x0600015B RID: 347 RVA: 0x00009B68 File Offset: 0x00007D68
	public void OutputUpdateVersion()
	{
		this.OutputTheVersion(this.index_OutputVersionLog);
	}

	// Token: 0x0600015C RID: 348 RVA: 0x00009B78 File Offset: 0x00007D78
	public void OutputNewestUpdateVersion()
	{
		UpdateLog asset_UpdateLog = LanguageText.Inst.asset_UpdateLog;
		this.OutputTheVersion(asset_UpdateLog.versions.Length - 1);
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00009BA0 File Offset: 0x00007DA0
	private void OutputTheVersion(int version)
	{
		UpdateLog asset_UpdateLog = AssetManager.inst.languageTexts[1].asset_UpdateLog;
		UpdateLog.Versions versions = asset_UpdateLog.versions[version];
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(versions.version).AppendLine();
		foreach (UpdateLog.SmallTitle smallTitle in versions.titles)
		{
			if (smallTitle.infos.Length != 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append(smallTitle.name).AppendLine();
				for (int j = 0; j < smallTitle.infos.Length; j++)
				{
					stringBuilder.Append(smallTitle.infos[j]).AppendLine();
				}
			}
		}
		asset_UpdateLog = AssetManager.inst.languageTexts[0].asset_UpdateLog;
		if (asset_UpdateLog.versions.Length == version + 1)
		{
			versions = asset_UpdateLog.versions[version];
			stringBuilder.Append(versions.version).AppendLine();
			foreach (UpdateLog.SmallTitle smallTitle2 in versions.titles)
			{
				if (smallTitle2.infos.Length != 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(smallTitle2.name).AppendLine();
					for (int k = 0; k < smallTitle2.infos.Length; k++)
					{
						stringBuilder.Append(smallTitle2.infos[k]).AppendLine();
					}
				}
			}
		}
		Debug.Log(stringBuilder.ToString());
		StreamWriter streamWriter = new StreamWriter("F:\\Users\\admin\\Desktop\\GA\\GA更新日志\\" + versions.version + ".txt");
		streamWriter.Write(stringBuilder.ToString());
		streamWriter.Close();
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00009D38 File Offset: 0x00007F38
	public void TestBool()
	{
		Battle inst = Battle.inst;
		GameData inst2 = GameData.inst;
		Debug.Log(inst.fragmentTotal);
		Debug.Log(inst.FragmentUsed);
		Debug.Log(inst.Fragment);
		Debug.Log(inst.Score);
		Debug.Log(11111111);
		Debug.Log(inst2.Star);
		Debug.Log(inst2.starUsed);
		Debug.Log(inst2.starTotal);
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00009DD0 File Offset: 0x00007FD0
	public void TestRuneAdd()
	{
		Rune rune = new Rune();
		rune.SetID_Random();
		rune.InitRandom(EnumRank.UNINTED, -1, false, -1, EnumRunePropertyType.UNINITED);
		GameData.inst.runes.Add(rune);
		Debug.Log(rune.GetInfo_RuneProps());
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00009E10 File Offset: 0x00008010
	public void TestRuneAll()
	{
		List<Rune> list = GameData.inst.runes;
		for (int i = 0; i < list.Count; i++)
		{
			Debug.Log(list[i].GetInfo_RuneProps());
		}
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00009E4C File Offset: 0x0000804C
	public void DebugJob0SkillModules()
	{
		PlayerModel playerModel = DataBase.Inst.DataPlayerModels[0];
		for (int i = 0; i < playerModel.skillModules.Length; i++)
		{
			Debug.Log(UI_ToolTipInfo.GetInfo_SkillModule(0, i, true, false));
		}
	}

	// Token: 0x06000162 RID: 354 RVA: 0x00009E87 File Offset: 0x00008087
	public IEnumerator FileTest()
	{
		for (;;)
		{
			new StreamWriter(Application.persistentDataPath + "//test.sav").Close();
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000170 RID: 368
	[SerializeField]
	private int index_OutputVersionLog;

	// Token: 0x04000171 RID: 369
	[SerializeField]
	private int waveToSet;

	// Token: 0x04000172 RID: 370
	[SerializeField]
	public SaveFile save;

	// Token: 0x04000173 RID: 371
	[SerializeField]
	[CustomLabel("成就ID")]
	public int achiID;

	// Token: 0x04000174 RID: 372
	[SerializeField]
	[CustomLabel("成就名字")]
	public string lockAchiName = "";

	// Token: 0x04000175 RID: 373
	[SerializeField]
	private int battleBuffType;

	// Token: 0x04000176 RID: 374
	[Header("存档修改")]
	[SerializeField]
	private long quickSavedata_Star;

	// Token: 0x04000177 RID: 375
	[SerializeField]
	private int quickSavedata_Master;

	// Token: 0x04000178 RID: 376
	[SerializeField]
	private int addGCNum;

	// Token: 0x04000179 RID: 377
	[Header("活动奖励")]
	[SerializeField]
	public int eventStarID;

	// Token: 0x0400017A RID: 378
	[SerializeField]
	public Rune[] runes = new Rune[0];
}
