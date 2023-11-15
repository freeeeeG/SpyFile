using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000016 RID: 22
public class BattleManager : MonoBehaviour
{
	// Token: 0x1700006E RID: 110
	// (get) Token: 0x060000DF RID: 223 RVA: 0x0000641C File Offset: 0x0000461C
	private float WaveTimeHasGo
	{
		get
		{
			return this.waveTimeTotal - this.waveTimeLeft;
		}
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x060000E0 RID: 224 RVA: 0x0000642B File Offset: 0x0000462B
	private float WavePercent
	{
		get
		{
			return Mathf.Min(this.WaveTimeHasGo / this.waveTimeTotal, 1f);
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x060000E1 RID: 225 RVA: 0x00006444 File Offset: 0x00004644
	private int EnemyNumNormalShould
	{
		get
		{
			return Mathf.FloorToInt(this.WavePercent * (this.enemyNumNormalTotal - 1f) + 1f);
		}
	}

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x060000E2 RID: 226 RVA: 0x00006464 File Offset: 0x00004664
	private int EnemyNumEliteShould
	{
		get
		{
			return Mathf.FloorToInt(this.WavePercent * (this.enemyNumEliteTotal + 1f));
		}
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x060000E3 RID: 227 RVA: 0x0000647E File Offset: 0x0000467E
	private int EnemyNumBossShould
	{
		get
		{
			return Mathf.FloorToInt(this.WavePercent * (this.enemyNumBossTotal + 1f));
		}
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x060000E4 RID: 228 RVA: 0x00006498 File Offset: 0x00004698
	private int actualEnemyNum
	{
		get
		{
			return this.preEnemyNum + (int)math.round(this.enemySizeCount);
		}
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x000064B0 File Offset: 0x000046B0
	private void CS_InitBuffers()
	{
		this.bulletStats = new BattleManager.StructBulletStats[this.listMines.Count];
		for (int i = 0; i < this.bulletStats.Length; i++)
		{
			Bullet bullet = this.listMines[i];
			Transform transform = bullet.transform;
			this.bulletStats[i] = default(BattleManager.StructBulletStats);
			this.bulletStats[i].pos = transform.position;
			this.bulletStats[i].speed = bullet.TotalSpeed;
			this.bulletStats[i].radius = transform.localScale.x / 2f;
			this.bulletStats[i].direction = bullet.direction;
		}
		this.bulletStatsBuffer = new ComputeBuffer(this.bulletStats.Length, 24);
		this.bulletStatsBuffer.SetData(this.bulletStats);
		this.enemyStats = new BattleManager.StructEnemyStats[this.listEnemies.Count];
		for (int j = 0; j < this.enemyStats.Length; j++)
		{
			Transform transform2 = this.listEnemies[j].transform;
			this.enemyStats[j] = default(BattleManager.StructEnemyStats);
			this.enemyStats[j].pos = transform2.position;
			this.enemyStats[j].radius = this.listEnemies[j].unit.lastScale / 2f;
		}
		this.enemyStatsBuffer = new ComputeBuffer(this.listEnemies.Count, 12);
		this.enemyStatsBuffer.SetData(this.enemyStats);
		this.resultsBuffer = new ComputeBuffer(this.bulletStats.Length, 4);
		this.CS_results = new int[this.listMines.Count];
		for (int k = 0; k < this.CS_results.Length; k++)
		{
			this.CS_results[k] = -1;
		}
		this.resultsBuffer.SetData(this.CS_results);
		this.CS_BulletsAndEnemies.SetBuffer(0, "bulletStats", this.bulletStatsBuffer);
		this.CS_BulletsAndEnemies.SetBuffer(0, "enemyStats", this.enemyStatsBuffer);
		this.CS_BulletsAndEnemies.SetBuffer(0, "Result", this.resultsBuffer);
		this.CS_BulletsAndEnemies.SetInt("bulletsCount", this.listMines.Count);
		this.CS_BulletsAndEnemies.SetInt("enemiesCount", this.listEnemies.Count);
		this.CS_BulletsAndEnemies.SetFloat("frameTime", Time.fixedDeltaTime);
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00006764 File Offset: 0x00004964
	private void CS_Activate()
	{
		int threadGroupsX = Mathf.CeilToInt((float)this.bulletStats.Length / 32f);
		int threadGroupsY = Mathf.CeilToInt((float)this.enemyStats.Length / 32f);
		this.CS_BulletsAndEnemies.Dispatch(0, threadGroupsX, threadGroupsY, 1);
		this.CS_results = new int[this.listMines.Count];
		this.resultsBuffer.GetData(this.CS_results);
		List<BattleManager.ColliderSet> list = new List<BattleManager.ColliderSet>();
		int count = this.listMines.Count;
		List<int> list2 = new List<int>();
		for (int i = 0; i < count; i++)
		{
			int num = this.CS_results[i];
			if (num >= 0)
			{
				Bullet b = this.listMines[i];
				Enemy enemy = this.listEnemies[num];
				if (Battle.inst.specialEffect[74] < 1 || enemy.model.rank != EnumRank.NORMAL)
				{
					list2.Add(i);
					list.Add(new BattleManager.ColliderSet(b, enemy));
				}
			}
		}
		foreach (BattleManager.ColliderSet colliderSet in list)
		{
			if (!(colliderSet.bullet == null) && !(colliderSet.enemy == null))
			{
				colliderSet.bullet.MyColliderEnter2D(colliderSet.enemy.gameObject);
			}
		}
		for (int j = list2.Count - 1; j >= 0; j--)
		{
			this.listMines.RemoveAt(list2[j]);
		}
		this.bulletStatsBuffer.Dispose();
		this.enemyStatsBuffer.Dispose();
		this.resultsBuffer.Dispose();
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00006920 File Offset: 0x00004B20
	public void FactorMultiBuffs(ref Factor factor)
	{
		foreach (BattleBuff battleBuff in this.listBattleBuffs)
		{
			if (battleBuff.layerThis == 1)
			{
				factor *= battleBuff.factorMultis;
			}
			else
			{
				factor *= battleBuff.factorMultis.GetDoubles_Power(battleBuff.layerThis);
			}
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x060000E8 RID: 232 RVA: 0x000069A0 File Offset: 0x00004BA0
	private int EnemyAmountNeedToAcc
	{
		get
		{
			return Mathf.CeilToInt((float)(Battle.inst.SequalWave / 2 + 3));
		}
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x000069B6 File Offset: 0x00004BB6
	private void Awake()
	{
		BattleManager.inst = this;
	}

	// Token: 0x060000EA RID: 234 RVA: 0x000069C0 File Offset: 0x00004BC0
	private void Start()
	{
		Object.Instantiate<GameObject>(ResourceLibrary.Inst.Prefab_Players[TempData.inst.jobId], Vector2.zero, Quaternion.Euler(0f, 0f, MyTool.MouseToPoint0()));
		bool flag;
		if (!SaveFile.IfExistSaveByJson())
		{
			this.NewBattle();
			flag = true;
		}
		else if (!GameData.SaveFile.ifOnBattle)
		{
			this.NewBattle();
			flag = true;
		}
		else
		{
			this.LoadBattle();
			flag = false;
		}
		Player.inst.InitPlayerFactor();
		if (SaveFile.IfExistSaveByJson())
		{
			SaveFile saveFile = GameData.saveFile;
			if (saveFile.ifOnBattle)
			{
				if (TempData.inst.modeType == EnumModeType.WANDER && saveFile.modeType == EnumModeType.WANDER)
				{
					Player.inst.unit.life = (double)saveFile.battle_Wander_PlayerLife;
					if (Player_8_Machineguner.instJob8 != null)
					{
						Player_8_Machineguner.instJob8.UpdateFactorTotal_AfterGetHurt();
					}
					Player.inst.shield = saveFile.battle_Wander_PlayerShield;
					this.waveTimeLeft = (float)saveFile.battle_Wander_BattleTiemLeft;
					HealthPointControl.inst.UpdateHpUnits();
					BuffManager.RefreshAllStateBuff();
				}
				else if (TempData.inst.diffiOptFlag[28])
				{
					Player.inst.unit.life = (double)saveFile.battle_Wander_PlayerLife;
					if (Player_8_Machineguner.instJob8 != null)
					{
						Player_8_Machineguner.instJob8.UpdateFactorTotal_AfterGetHurt();
					}
					Player.inst.shield = saveFile.battle_Wander_PlayerShield;
					HealthPointControl.inst.UpdateHpUnits();
					BuffManager.RefreshAllStateBuff();
				}
			}
		}
		if (flag && GameData.inst.CurrentRune != null)
		{
			GameData.inst.CurrentRune.GetOriginUpgrades();
			Battle.inst.upgradeShop.TryUpdateShop();
			Battle.inst.upgradeShop.SpecialUpdateShop_AllLegend();
		}
		SqlManager.inst.NewOrLoad();
		TutorialMaster.inst.Activate();
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00006B76 File Offset: 0x00004D76
	private IEnumerator RandomLevelColorEvery(float seconds)
	{
		for (;;)
		{
			yield return new WaitForSeconds(seconds);
			Battle.inst.RandomLevelColor();
		}
		yield break;
	}

	// Token: 0x060000EC RID: 236 RVA: 0x00006B85 File Offset: 0x00004D85
	private void NewBattle()
	{
		BattleManager.lifeDrain_TimeLeft = 3f;
		this.GameOn = true;
		Debug.Log("BattleManager_New");
		Battle.inst.InitNewGame();
		Battle.inst.UpdateBattleFacs();
		this.TimeStage_Switch_Rest();
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00006BBC File Offset: 0x00004DBC
	private void LoadBattle()
	{
		this.GameOn = true;
		Debug.Log("BattleManager_Load");
		GameData.inst.LoadSaveSlot();
		TempData.inst.LoadSaveSlot();
		this.timeStage = EnumTimeStage.REST;
		BattleMapCanvas.inst.UpdateProcessPercent(this.timeStage, 1f);
		UI_Panel_Battle_BattleButtons.inst.Show();
		BattleMapCanvas.inst.UpdateTimeStageIndicator();
		UI_FloatTextControl.inst.Special_GoStage();
		Battle.inst.Score_Gain(0f);
		Battle.inst.Fragment_Gain(0);
		this.BattleItem_ReadFromSaveFile();
		this.Fragment_ReadFromSaveFile();
		this.LoadJusticeBlade();
	}

	// Token: 0x060000EE RID: 238 RVA: 0x00006C54 File Offset: 0x00004E54
	private void LoadJusticeBlade()
	{
		SaveFile saveFile = GameData.SaveFile;
		if (Battle.inst.GetBool_JusticeBlade())
		{
			int battle_JusticeBladeLayer = saveFile.battle_JusticeBladeLayer;
			for (int i = 0; i < battle_JusticeBladeLayer; i++)
			{
				BuffManager.UpgradeBuff_Justice();
			}
		}
	}

	// Token: 0x060000EF RID: 239 RVA: 0x000051D0 File Offset: 0x000033D0
	public void EnemyAmoutAddOne()
	{
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x000051D0 File Offset: 0x000033D0
	public void EnemyAmountReduceOne()
	{
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00006C8B File Offset: 0x00004E8B
	private void Update()
	{
		if (!this.GameOn)
		{
			return;
		}
		this.TimeStage_InUpdate();
		this.UpdateEnemySize();
		this.Update_LifeDrain();
		this.Update_ShieldCharger();
		this.Update_Fusion();
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00006CB4 File Offset: 0x00004EB4
	private void FixedUpdate()
	{
		if (this.listMines.Count == 0 || this.listEnemies.Count == 0)
		{
			return;
		}
		this.CS_InitBuffers();
		this.CS_Activate();
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00006CE0 File Offset: 0x00004EE0
	private void UpdateEnemySize()
	{
		this.enemySizeCount = 0.0;
		foreach (Enemy enemy in this.listEnemies)
		{
			double x = (double)enemy.unit.lastSize;
			this.enemySizeCount += math.sqrt(x);
		}
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00006D5C File Offset: 0x00004F5C
	private void Update_LifeDrain()
	{
		if (TempData.inst == null)
		{
			return;
		}
		if (TempData.inst.diffiOptFlag == null)
		{
			return;
		}
		if (!TempData.inst.diffiOptFlag[23])
		{
			return;
		}
		if (Player.inst == null)
		{
			return;
		}
		if (this.timeStage == EnumTimeStage.REST)
		{
			return;
		}
		BattleManager.lifeDrain_TimeLeft -= Time.deltaTime;
		if (BattleManager.lifeDrain_TimeLeft <= 0f)
		{
			BattleManager.lifeDrain_TimeLeft = 3f;
			BasicUnit unit = Player.inst.unit;
			double num = math.floor(unit.life * 0.20000000298023224);
			num = math.max(1.0, num);
			unit.GetHurt(num, Player.inst.unit, Vector2.zero, false, unit.transform.position, true);
		}
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00006E2C File Offset: 0x0000502C
	private void Update_ShieldCharger()
	{
		if (Battle.inst.specialEffect[59] <= 0)
		{
			return;
		}
		if (this.timeStage == EnumTimeStage.REST)
		{
			return;
		}
		this.shieldCharger_TimeLeft -= Time.deltaTime;
		if (this.shieldCharger_TimeLeft <= 0f)
		{
			this.shieldCharger_TimeLeft = 6f;
			Player.inst.RestoreShield(1);
		}
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00006E88 File Offset: 0x00005088
	private void TimeStage_Switch_Rest()
	{
		Debug.Log("进入准备阶段");
		Battle.inst.Score_Gain(0f);
		Battle.inst.Fragment_Gain(0);
		if (Battle.inst.SequalWave > 0)
		{
			SqlManager.inst.Thread_WaveEnd();
			new Thread(new ThreadStart(MySteamAchievement.Detect_LevelEnd)).Start();
		}
		if (TempData.inst.modeType == EnumModeType.NORMAL && Battle.inst.level == 4 && Battle.inst.wave == 5)
		{
			this.timeStage = EnumTimeStage.REST;
			BattleMapCanvas.inst.panel_BattleAward.InitAndSave(true);
			return;
		}
		Battle.inst.upgradeShop.TryUpdateShop();
		UI_Panel_Battle_BattleButtons.inst.Show();
		if (this.timeStage != EnumTimeStage.WAVEEND && this.timeStage != EnumTimeStage.UNINITED)
		{
			Debug.LogError("TimeStage!=WaveEnd!");
			return;
		}
		Battle.inst.NewWave();
		this.timeStage = EnumTimeStage.REST;
		BattleMapCanvas.inst.UpdateTimeStageIndicator();
		if (TempData.inst.modeType != EnumModeType.WANDER)
		{
			UI_FloatTextControl.inst.Special_GoStage();
		}
		if (!Battle.inst.IsFirstWave())
		{
			this.HpRecover();
			UI_FloatTextControl.inst.Special_ShopRefresh();
		}
		BattleMapCanvas.inst.UpdateProcessPercent(this.timeStage, 0f);
		this.DestroyAllBattleItems();
		SpecialEffects.AttackingStageEnd();
		Battle.inst.upgradeShop.SpecialUpdateShop_AllLegend();
		TutorialMaster.inst.Activate();
		base.StartCoroutine(this.SpaceWalk());
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00006FEE File Offset: 0x000051EE
	private IEnumerator SpaceWalk()
	{
		if (!TempData.inst.diffiOptFlag[22])
		{
			yield break;
		}
		if (TempData.inst.modeType == EnumModeType.WANDER)
		{
			yield break;
		}
		if (Player.inst == null || Player.inst.unit == null)
		{
			yield break;
		}
		Player.inst.unit.rb.velocity = Vector2.zero;
		yield return new WaitForSeconds(0.6f);
		if (Player.inst == null || Player.inst.unit == null)
		{
			yield break;
		}
		Player.inst.unit.rb.velocity = Vector2.zero;
		yield break;
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00006FF6 File Offset: 0x000051F6
	private void HpRecover()
	{
		if (TempData.inst.diffiOptFlag[28])
		{
			return;
		}
		Player.inst.unit.HealAll();
		Player.inst.RestoreShieldAll();
		UI_FloatTextControl.inst.Special_HpRecover();
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x0000702C File Offset: 0x0000522C
	public void TimeStage_Switch_Waving()
	{
		if (!BattleManager.inst.GameOn)
		{
			Debug.LogWarning("游戏已结束，无法开始新一局");
			return;
		}
		this.autoRun_CountDown = this.autoRun_IntervalMax;
		SaveFile.SaveByJson(true);
		if (TempData.inst.modeType == EnumModeType.WANDER)
		{
			this.wander_On = true;
		}
		UI_Panel_Battle_BattleButtons.inst.Hide();
		int num = FactorBattle.GetFloat_EnemyAmount_Total().RoundToInt();
		int num2 = 18;
		switch (TempData.inst.modeType)
		{
		case EnumModeType.NORMAL:
			if (Battle.inst.wave == 5)
			{
				num2 = 36;
			}
			else
			{
				num2 = 18;
			}
			break;
		case EnumModeType.ENDLESS:
			num2 = 36;
			break;
		case EnumModeType.WANDER:
			num2 = 30;
			break;
		default:
			Debug.LogError("模式错误");
			break;
		}
		num2 = ((float)num2 * Battle.inst.factorBattleTotal.EnemyGeneSpd).RoundToInt();
		if (this.timeStage != EnumTimeStage.REST)
		{
			Debug.LogError("TimeStage!=Rest!");
			return;
		}
		this.timeStage = EnumTimeStage.WAVING;
		if (TempData.inst.modeType != EnumModeType.WANDER)
		{
			UI_FloatTextControl.inst.Special_GoStage();
		}
		else
		{
			UI_FloatTextControl.inst.Special_Wander_LevelUp();
		}
		this.waveTimeTotal = (float)num2;
		if (TempData.inst.modeType != EnumModeType.WANDER || this.waveTimeLeft <= 0f)
		{
			this.waveTimeLeft = this.waveTimeTotal;
			SpecialEffects.AttackingStageStart();
		}
		BattleMapCanvas.inst.UpdateTimeStageIndicator();
		if (TempData.inst.modeType == EnumModeType.WANDER)
		{
			this.enemyNumEliteTotal = 0f;
			this.enemyNumNormalTotal = 0f;
		}
		else
		{
			this.enemyNumEliteTotal = (float)MyTool.DecimalToInt((float)num * FactorBattle.GetFloat_EliteProp_Total());
			this.enemyNumNormalTotal = (float)num - this.enemyNumEliteTotal;
		}
		switch (TempData.inst.modeType)
		{
		case EnumModeType.NORMAL:
			if (Battle.inst.wave % 5 == 0)
			{
				this.enemyNumBossTotal = 1f;
			}
			else
			{
				this.enemyNumBossTotal = 0f;
			}
			this.endless_BossLeft = this.enemyNumBossTotal.RoundToInt();
			break;
		case EnumModeType.ENDLESS:
			this.enemyNumBossTotal = (float)(1 + (Battle.inst.wave - 1) / 20);
			this.endless_BossLeft = this.enemyNumBossTotal.RoundToInt();
			break;
		case EnumModeType.WANDER:
			this.enemyNumBossTotal = 0f;
			break;
		default:
			Debug.LogError("模式错误");
			break;
		}
		this.enemyNumElitelNow = 0f;
		this.enemyNumNormalNow = 0f;
		this.enemyNumBossNow = 0f;
		SqlManager.inst.SqlDataInsert_OnWaveStart();
		Debug.Log(string.Concat(new object[]
		{
			"No.",
			Battle.inst.wave,
			"Waving amount=",
			num,
			" Time=",
			num2,
			" Elite Amount=",
			this.enemyNumEliteTotal,
			"\n当前关卡种类：",
			Battle.inst.CurrentLevelType,
			" 当前关卡层次",
			Battle.inst.ActualWave
		}));
		if (UI_Icon_BattleDPS.inst != null)
		{
			UI_Icon_BattleDPS.inst.ClearData();
		}
		if (this.autoRun_Text.gameObject.activeSelf)
		{
			this.autoRun_Text.gameObject.SetActive(false);
		}
	}

	// Token: 0x060000FA RID: 250 RVA: 0x0000734B File Offset: 0x0000554B
	private void TimeStage_Switch_Waveend()
	{
		if (this.timeStage != EnumTimeStage.WAVING)
		{
			Debug.LogError("TimeStage!=Waving!");
			return;
		}
		this.timeStage = EnumTimeStage.WAVEEND;
		BattleMapCanvas.inst.UpdateTimeStageIndicator();
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00007374 File Offset: 0x00005574
	private void TimeStage_InUpdate()
	{
		switch (this.timeStage)
		{
		case EnumTimeStage.REST:
		{
			this.Update_EndlessAutoRun();
			EnumModeType modeType = TempData.inst.modeType;
			if (modeType == EnumModeType.WANDER && this.wander_On)
			{
				this.TimeStage_Switch_Waving();
			}
			return;
		}
		case EnumTimeStage.WAVING:
			this.Update_BattleItem();
			if (TempData.inst.modeType != EnumModeType.WANDER)
			{
				this.waveTimeLeft -= Time.deltaTime * this.MyTimeScale();
			}
			else
			{
				this.waveTimeLeft -= Time.deltaTime;
				this.Wander_Waving_UpdateEnemyGeneTime();
				this.Wander_Waving_UpdateBossGene();
			}
			if (this.waveTimeLeft <= 0f)
			{
				this.waveTimeLeft = 0f;
			}
			if (this.waveTimeLeft <= 0f && this.enemyNumNormalTotal == this.enemyNumNormalNow && this.enemyNumEliteTotal == this.enemyNumElitelNow && this.enemyNumBossTotal == this.enemyNumBossNow)
			{
				this.TimeStage_Switch_Waveend();
			}
			else if (this.enemyNumNormalNow > this.enemyNumNormalTotal || this.enemyNumElitelNow > this.enemyNumEliteTotal || this.enemyNumBossNow > this.enemyNumBossTotal)
			{
				Debug.LogError("EnemyNumOut!!");
			}
			else
			{
				if (this.enemyNumNormalNow < this.enemyNumNormalTotal && this.enemyNumNormalNow < (float)this.EnemyNumNormalShould)
				{
					this.enemyNumNormalNow += 1f;
					base.StartCoroutine(this.GeneOneEnemy(EnumRank.NORMAL));
				}
				if (this.enemyNumElitelNow < this.enemyNumEliteTotal && this.enemyNumElitelNow < (float)this.EnemyNumEliteShould)
				{
					this.enemyNumElitelNow += 1f;
					base.StartCoroutine(this.GeneOneEnemy(EnumRank.RARE));
				}
				if (this.enemyNumBossNow < this.enemyNumBossTotal && this.enemyNumBossNow < (float)this.EnemyNumBossShould)
				{
					this.enemyNumBossNow += 1f;
					base.StartCoroutine(this.GeneOneEnemy(EnumRank.EPIC));
				}
			}
			BattleMapCanvas.inst.UpdateProcessPercent(this.timeStage, this.WaveTimeHasGo / this.waveTimeTotal);
			return;
		case EnumTimeStage.WAVEEND:
			if ((this.listEnemies.Count <= 0 && this.preEnemyNum <= 0) || TempData.inst.modeType == EnumModeType.WANDER)
			{
				this.TimeStage_Switch_Rest();
			}
			BattleMapCanvas.inst.UpdateProcessPercent(this.timeStage, 1f);
			return;
		default:
			return;
		}
	}

	// Token: 0x060000FC RID: 252 RVA: 0x000075B4 File Offset: 0x000057B4
	private void Update_EndlessAutoRun()
	{
		if (TempData.inst.modeType != EnumModeType.ENDLESS || this.timeStage != EnumTimeStage.REST || !this.autoRun_Flag)
		{
			this.autoRun_Flag = false;
			this.autoRun_CountDown = this.autoRun_IntervalMax;
			this.autoRun_Text.gameObject.SetActive(false);
			return;
		}
		if (BattleMapCanvas.inst.IfAnyWindowActive())
		{
			return;
		}
		this.autoRun_Text.gameObject.SetActive(true);
		this.autoRun_Text.text = LanguageText.Inst.diy.endlessAutoRunTip.Replace("cd", this.autoRun_CountDown.ToString("0.0")).ReplaceLineBreak();
		if (this.autoRun_CountDown > this.autoRun_IntervalMax)
		{
			this.autoRun_CountDown = this.autoRun_IntervalMax;
		}
		this.autoRun_CountDown -= Time.deltaTime;
		if (this.autoRun_CountDown <= 0f)
		{
			this.autoRun_CountDown = this.autoRun_IntervalMax;
			this.TimeStage_Switch_Waving();
			this.autoRun_Text.gameObject.SetActive(false);
		}
	}

	// Token: 0x060000FD RID: 253 RVA: 0x000076B6 File Offset: 0x000058B6
	public bool GetBool_IfOnBossFight()
	{
		return TempData.inst.modeType != EnumModeType.WANDER && this.enemyNumBossNow != 0f && this.endless_BossLeft != 0;
	}

	// Token: 0x060000FE RID: 254 RVA: 0x000076E1 File Offset: 0x000058E1
	private IEnumerator GeneOneEnemy(EnumRank rank)
	{
		this.preEnemyNum++;
		GameObject[] prefab_Enemys = ResourceLibrary.Inst.Prefab_Enemys;
		int num = DataSelector.RandomEnemyModelNo_WithMapAndDay(Battle.inst.CurrentLevelType, Battle.inst.ActualWave, rank);
		Vector2 position = BattleManager.ChooseGenePosInScene(DataBase.Inst.Data_EnemyModels[num].rank);
		GameObject gameObject = Object.Instantiate<GameObject>(prefab_Enemys[num], position, Quaternion.identity);
		Enemy enemyComp = gameObject.GetComponent<Enemy>();
		if (enemyComp.modelID != num)
		{
			Debug.LogWarning("IDError!");
		}
		GameObject gameObject2 = Object.Instantiate<GameObject>(ResourceLibrary.Inst.Prefab_Particle_EnemyPreGene, enemyComp.transform.position, Quaternion.identity);
		Particle partPreGene = gameObject2.GetComponent<Particle>();
		partPreGene.EnmeyPreGene_Init(enemyComp.unit.shapeType, enemyComp.unit.mainColor, Mathf.Sqrt(enemyComp.unit.EnemyFactorTotal.bodySize));
		gameObject.SetActive(false);
		float seconds = 1.2f;
		if (TempData.inst.diffiOptFlag[17] && (rank == EnumRank.EPIC || rank == EnumRank.RARE))
		{
			seconds = 0.3f;
		}
		yield return new WaitForSeconds(seconds);
		partPreGene.Close();
		enemyComp.gameObject.SetActive(true);
		if (!this.GameOn)
		{
			enemyComp.gameObject.GetComponent<BasicUnit>().Die(false);
		}
		else if (Player.inst != null)
		{
			enemyComp.Move_Direction = Player.inst.transform.position - position;
		}
		this.preEnemyNum--;
		yield break;
	}

	// Token: 0x060000FF RID: 255 RVA: 0x000076F8 File Offset: 0x000058F8
	public static Vector2 ChooseGenePosInScene(EnumRank rank)
	{
		if (TempData.inst.diffiOptFlag[17])
		{
			if (Player.inst == null)
			{
				return Vector2.zero;
			}
			return Player.inst.transform.position;
		}
		else
		{
			float num = BattleManager.inst.sceneRadiusSingleSize * GameParameters.Inst.WorldSize * SceneObj.inst.SceneSize * 0.95f;
			float num2;
			float num3;
			switch (UnityEngine.Random.Range(0, 4))
			{
			case 0:
				num2 = -num;
				num3 = UnityEngine.Random.Range(-num, num);
				break;
			case 1:
				num2 = num;
				num3 = UnityEngine.Random.Range(-num, num);
				break;
			case 2:
				num3 = -num;
				num2 = UnityEngine.Random.Range(-num, num);
				break;
			case 3:
				num3 = num;
				num2 = UnityEngine.Random.Range(-num, num);
				break;
			default:
				Debug.LogError("rd??");
				num2 = 0f;
				num3 = 0f;
				break;
			}
			if (rank == EnumRank.EPIC)
			{
				return new Vector2(num2 / 2f, num3 / 2f);
			}
			return new Vector2(num2, num3);
		}
	}

	// Token: 0x06000100 RID: 256 RVA: 0x000077F0 File Offset: 0x000059F0
	public static Vector2 ChooseBattleItemGenePosInScene()
	{
		float sceneSize = SceneObj.inst.SceneSize;
		float num = BattleManager.inst.sceneRadiusSingleSize * GameParameters.Inst.WorldSize * sceneSize;
		num -= 3f;
		float x = UnityEngine.Random.Range(-num, num);
		float y = UnityEngine.Random.Range(-num, num);
		return new Vector2(x, y);
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00007840 File Offset: 0x00005A40
	private void Update_BattleItem()
	{
		if (this.timeStage != EnumTimeStage.WAVING)
		{
			return;
		}
		if (this.timeToGeneBattleItem == -1f && Battle.inst.SequalWave <= 1)
		{
			this.timeToGeneBattleItem = this.BattleItem_RandomTime() / 3f;
			return;
		}
		if (TempData.inst.modeType == EnumModeType.WANDER)
		{
			this.timeToGeneBattleItem -= Time.deltaTime;
		}
		else
		{
			this.timeToGeneBattleItem -= Time.deltaTime * this.MyTimeScale();
		}
		if (this.timeToGeneBattleItem < 0f)
		{
			SpecialEffects.BattleItem_Random(0f, 0f);
			this.timeToGeneBattleItem = this.BattleItem_RandomTime();
		}
	}

	// Token: 0x06000102 RID: 258 RVA: 0x000078E8 File Offset: 0x00005AE8
	public float MyTimeScale()
	{
		float enemyAmount = Battle.inst.factorBattleTotal.EnemyAmount;
		float num = Mathf.Sqrt(enemyAmount);
		float num2 = 0f;
		EnumModeType modeType = TempData.inst.modeType;
		if (modeType > EnumModeType.ENDLESS)
		{
			if (modeType != EnumModeType.WANDER)
			{
				Debug.LogError("Error_模式错误！");
			}
			else
			{
				num2 = 9f * num;
			}
		}
		else
		{
			num2 = Mathf.Clamp((float)this.EnemyAmountNeedToAcc * enemyAmount, 1f * enemyAmount, 18f * num);
		}
		if ((float)this.actualEnemyNum <= num2)
		{
			return 6f;
		}
		if ((float)this.actualEnemyNum >= 60f * num)
		{
			return 0f;
		}
		if ((float)this.actualEnemyNum >= 50f * num)
		{
			return 0.09f;
		}
		if ((float)this.actualEnemyNum >= 40f * num)
		{
			return 0.27f;
		}
		if ((float)this.actualEnemyNum >= 30f * num)
		{
			return 0.54f;
		}
		return 1f;
	}

	// Token: 0x06000103 RID: 259 RVA: 0x000079C7 File Offset: 0x00005BC7
	private float BattleItem_RandomTime()
	{
		return (float)UnityEngine.Random.Range(9, 63);
	}

	// Token: 0x06000104 RID: 260 RVA: 0x000079D4 File Offset: 0x00005BD4
	public void BattleItem_ReadFromSaveFile()
	{
		foreach (SaveFile_BattleItem saveFile_BattleItem in GameData.SaveFile.battle_BattleItems)
		{
			int typeID = saveFile_BattleItem.typeID;
			Vector2 v = new Vector2((float)saveFile_BattleItem.x, (float)saveFile_BattleItem.y);
			Object.Instantiate<GameObject>(ResourceLibrary.Inst.Prefab_BattleItem, v, Quaternion.identity).GetComponent<BattleItem>().Init(typeID);
		}
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00007A40 File Offset: 0x00005C40
	public void Fragment_ReadFromSaveFile()
	{
		foreach (SaveFile_Fragment saveFile_Fragment in GameData.SaveFile.battle_Fragments)
		{
			Fragment.InstantiateFragments(new Vector2((float)saveFile_Fragment.x, (float)saveFile_Fragment.y), saveFile_Fragment.value, EnumShapeType.CIRCLE, Battle.inst.levelColor, (float)saveFile_Fragment.scale);
		}
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00007A9A File Offset: 0x00005C9A
	public void GameOver()
	{
		this.ifGameOver = true;
		base.StartCoroutine(this.StartGameOverAnimation());
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00007AB0 File Offset: 0x00005CB0
	public float GetFloat_TimePct()
	{
		return 1f - this.waveTimeLeft / this.waveTimeTotal;
	}

	// Token: 0x06000108 RID: 264 RVA: 0x00007AC5 File Offset: 0x00005CC5
	private IEnumerator StartGameOverAnimation()
	{
		float seconds = 0.5f;
		float durationTime = 2f;
		float endTime = 0.5f;
		yield return new WaitForSeconds(seconds);
		int cntTotal = this.listEnemies.Count;
		int cntNow = 0;
		float totalTime = durationTime;
		float timeSpend = 0f;
		int loop = 0;
		while (this.listEnemies.Count > 0)
		{
			try
			{
				int num = loop;
				loop = num + 1;
				if (loop > 10000)
				{
					Debug.LogError("Error_摧毁敌人的循环超过一万，跳出");
					break;
				}
				timeSpend += Time.deltaTime;
				int num2 = MyTool.DecimalToInt(timeSpend / totalTime * (float)cntTotal);
				int num3 = num2 - cntNow;
				if (this.listEnemies.Count == 0)
				{
					Debug.LogError("Error_敌人数量为0却仍准备遍历，跳出");
					break;
				}
				for (int i = 0; i < num3; i++)
				{
					if (this.listEnemies.Count == 0)
					{
						Debug.LogError("Error_敌人数量为0却仍在进行遍历，跳出");
						break;
					}
					Enemy enemy = this.listEnemies[0];
					if (enemy.unit.ifDie)
					{
						Object.Destroy(enemy.gameObject);
					}
					else
					{
						enemy.unit.Die(false);
					}
				}
				cntNow = num2;
			}
			catch (Exception ex)
			{
				Debug.LogError("Error_敌人死亡遍历出错:" + ex.ToString());
				break;
			}
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield return new WaitForSeconds(endTime);
		BattleMapCanvas.inst.panel_BattleAward.InitAndSave(false);
		yield break;
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00007AD4 File Offset: 0x00005CD4
	private void Update_Fusion()
	{
		bool flag = TempData.inst.diffiOptFlag[15];
		bool flag2 = TempData.inst.diffiOptFlag[31];
		if (!flag && !flag2)
		{
			return;
		}
		int num = 90;
		for (int i = 0; i < this.listEnemies.Count; i++)
		{
			Enemy enemy = this.listEnemies[i];
			if (enemy.fusionRank < num)
			{
				for (int j = i + 1; j < this.listEnemies.Count; j++)
				{
					Enemy enemy2 = this.listEnemies[j];
					float num2 = 1f;
					if (enemy.existTime >= num2 && enemy2.existTime >= num2 && (enemy.transform.position - enemy2.transform.position).magnitude < 0.5f * (enemy.unit.lastScale + enemy2.unit.lastScale))
					{
						if (flag && enemy2.fusionRank < num && enemy.modelID == enemy2.modelID)
						{
							float fragMulti = enemy.fragMulti + enemy2.fragMulti;
							float starMulti = enemy.starMulti + enemy2.starMulti;
							double life = enemy.unit.life + enemy2.unit.life;
							int num3 = enemy.fusionRank + enemy2.fusionRank;
							float shoot_TimeLeft = (enemy.shoot_TimeLeft * (float)enemy.fusionRank + enemy2.shoot_TimeLeft * (float)enemy2.fusionRank) / (float)num3;
							enemy.UpdateFusion(num3);
							enemy.unit.life = life;
							enemy.fragMulti = fragMulti;
							enemy.starMulti = starMulti;
							enemy.shoot_TimeLeft = shoot_TimeLeft;
							enemy2.unit.Die_Fusion();
						}
						else if (flag2)
						{
							enemy.BumperCar_HitEnemy(enemy2);
							enemy2.BumperCar_HitEnemy(enemy);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00007CCC File Offset: 0x00005ECC
	private void Wander_Waving_UpdateEnemyGeneTime()
	{
		if (this.wander_EnemyGeneTimeLeft <= 0f)
		{
			GameParameters.EnemySetting enemySet = GameParameters.Inst.enemySet;
			FactorBattle factorBattleTotal = Battle.inst.factorBattleTotal;
			float num = (float)enemySet.rankWeight_Normal * factorBattleTotal.Enemy_NormalRate;
			float num2 = (float)enemySet.rankWeight_Elite * factorBattleTotal.Enemy_EliteRate;
			if (MyTool.DecimalToBool(num2 / (num2 + num)))
			{
				base.StartCoroutine(this.GeneOneEnemy(EnumRank.RARE));
			}
			else
			{
				base.StartCoroutine(this.GeneOneEnemy(EnumRank.NORMAL));
			}
			int num3 = (15f * Battle.inst.factorBattleTotal.EnemyAmount).RoundToInt();
			this.wander_EnemyGeneTimeLeft = this.waveTimeTotal / (float)num3;
			return;
		}
		this.wander_EnemyGeneTimeLeft -= Time.deltaTime * this.MyTimeScale();
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00007D88 File Offset: 0x00005F88
	private void Wander_Waving_UpdateBossGene()
	{
		if (this.wander_BossFlag)
		{
			return;
		}
		if (this.wander_BossTimeLeft > 0f)
		{
			this.wander_BossTimeLeft -= Time.deltaTime;
			return;
		}
		this.wander_BossTimeLeft = this.wander_BossTimeDeltaMax;
		base.StartCoroutine(this.GeneOneEnemy(EnumRank.EPIC));
		this.wander_BossFlag = true;
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00007DDF File Offset: 0x00005FDF
	public void BossDie()
	{
		if (TempData.inst.modeType == EnumModeType.WANDER)
		{
			this.wander_BossFlag = false;
			return;
		}
		this.endless_BossLeft--;
		if (this.endless_BossLeft < 0)
		{
			Debug.LogError("Error_剩余boss数量有误！");
		}
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00007E18 File Offset: 0x00006018
	private void DestroyAllBattleItems()
	{
		if (!TempData.inst.diffiOptFlag[18])
		{
			return;
		}
		List<BattleItem> list = new List<BattleItem>();
		foreach (BattleItem item in this.listBattleItems)
		{
			list.Add(item);
		}
		for (int i = 0; i < list.Count; i++)
		{
			list[i].EndLife();
		}
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00007EA0 File Offset: 0x000060A0
	public int GetLayer_BuffID(int buffID)
	{
		foreach (BattleBuff battleBuff in this.listBattleBuffs)
		{
			if (battleBuff.typeID == buffID)
			{
				return battleBuff.layerThis;
			}
		}
		return 0;
	}

	// Token: 0x0400013B RID: 315
	public static BattleManager inst;

	// Token: 0x0400013C RID: 316
	[Range(0f, 36f)]
	public int Test_EnemyID;

	// Token: 0x0400013D RID: 317
	public List<Enemy> listEnemies = new List<Enemy>();

	// Token: 0x0400013E RID: 318
	public bool GameOn;

	// Token: 0x0400013F RID: 319
	public bool ifGameOver;

	// Token: 0x04000140 RID: 320
	public EnumTimeStage timeStage = EnumTimeStage.UNINITED;

	// Token: 0x04000141 RID: 321
	[SerializeField]
	public float sceneRadiusSingleSize = 14f;

	// Token: 0x04000142 RID: 322
	[Header("关卡时间")]
	[SerializeField]
	[CustomLabel("关卡总时间")]
	public float waveTimeTotal;

	// Token: 0x04000143 RID: 323
	[SerializeField]
	[CustomLabel("关卡剩余时间")]
	public float waveTimeLeft;

	// Token: 0x04000144 RID: 324
	[Header("普通敌人")]
	[SerializeField]
	[CustomLabel("关卡总生成普通敌人")]
	private float enemyNumNormalTotal;

	// Token: 0x04000145 RID: 325
	[SerializeField]
	[CustomLabel("关卡已生成普通敌人")]
	private float enemyNumNormalNow;

	// Token: 0x04000146 RID: 326
	[Header("精英敌人")]
	[SerializeField]
	[CustomLabel("关卡总生成精英敌人")]
	private float enemyNumEliteTotal;

	// Token: 0x04000147 RID: 327
	[SerializeField]
	[CustomLabel("关卡已生成精英敌人")]
	private float enemyNumElitelNow;

	// Token: 0x04000148 RID: 328
	[Header("Boss敌人")]
	[SerializeField]
	[CustomLabel("关卡总生成Boss敌人")]
	private float enemyNumBossTotal;

	// Token: 0x04000149 RID: 329
	[SerializeField]
	[CustomLabel("关卡已生成Boss敌人")]
	public float enemyNumBossNow;

	// Token: 0x0400014A RID: 330
	[SerializeField]
	[CustomLabel("关卡里剩余boss的数量（总数减去已被消灭的）")]
	public int endless_BossLeft;

	// Token: 0x0400014B RID: 331
	[SerializeField]
	private bool wander_BossFlag;

	// Token: 0x0400014C RID: 332
	[SerializeField]
	private float wander_BossTimeLeft = 1.8f;

	// Token: 0x0400014D RID: 333
	[SerializeField]
	private float wander_BossTimeDeltaMax = 1.8f;

	// Token: 0x0400014E RID: 334
	[Header("敌人总和")]
	[SerializeField]
	private double enemySizeCount;

	// Token: 0x0400014F RID: 335
	[Header("预生成敌人")]
	[SerializeField]
	private int preEnemyNum;

	// Token: 0x04000150 RID: 336
	[Header("战场道具")]
	[SerializeField]
	public List<BattleBuff> listBattleBuffs = new List<BattleBuff>();

	// Token: 0x04000151 RID: 337
	[SerializeField]
	private float timeToGeneBattleItem = -1f;

	// Token: 0x04000152 RID: 338
	[SerializeField]
	public List<BattleItem> listBattleItems = new List<BattleItem>();

	// Token: 0x04000153 RID: 339
	[SerializeField]
	public List<Fragment> listFragments = new List<Fragment>();

	// Token: 0x04000154 RID: 340
	[Header("漫游模式")]
	public bool wander_On;

	// Token: 0x04000155 RID: 341
	[SerializeField]
	private float wander_EnemyGeneTimeLeft;

	// Token: 0x04000156 RID: 342
	[Header("特殊机制")]
	public static float lifeDrain_TimeLeft = 3f;

	// Token: 0x04000157 RID: 343
	public float shieldCharger_TimeLeft = 6f;

	// Token: 0x04000158 RID: 344
	[Header("AutoRun")]
	public bool autoRun_Flag;

	// Token: 0x04000159 RID: 345
	public float autoRun_IntervalMax = 3f;

	// Token: 0x0400015A RID: 346
	public float autoRun_CountDown = 3f;

	// Token: 0x0400015B RID: 347
	public Text autoRun_Text;

	// Token: 0x0400015C RID: 348
	[Header("特殊子弹数组")]
	public List<SpecialBullet_Mine> listMines = new List<SpecialBullet_Mine>();

	// Token: 0x0400015D RID: 349
	public List<SpecialBullet_Grenade> listGrenades = new List<SpecialBullet_Grenade>();

	// Token: 0x0400015E RID: 350
	[Header("ComputeShader_Bullets")]
	public ComputeShader CS_BulletsAndEnemies;

	// Token: 0x0400015F RID: 351
	private ComputeBuffer bulletStatsBuffer;

	// Token: 0x04000160 RID: 352
	private ComputeBuffer enemyStatsBuffer;

	// Token: 0x04000161 RID: 353
	private ComputeBuffer resultsBuffer;

	// Token: 0x04000162 RID: 354
	private BattleManager.StructBulletStats[] bulletStats;

	// Token: 0x04000163 RID: 355
	private BattleManager.StructEnemyStats[] enemyStats;

	// Token: 0x04000164 RID: 356
	private int[] CS_results;

	// Token: 0x0200012E RID: 302
	private struct StructBulletStats
	{
		// Token: 0x04000939 RID: 2361
		public float2 pos;

		// Token: 0x0400093A RID: 2362
		public float radius;

		// Token: 0x0400093B RID: 2363
		public float speed;

		// Token: 0x0400093C RID: 2364
		public float2 direction;
	}

	// Token: 0x0200012F RID: 303
	private struct StructEnemyStats
	{
		// Token: 0x0400093D RID: 2365
		public float2 pos;

		// Token: 0x0400093E RID: 2366
		public float radius;
	}

	// Token: 0x02000130 RID: 304
	private class ColliderSet
	{
		// Token: 0x0600095A RID: 2394 RVA: 0x000357C7 File Offset: 0x000339C7
		public ColliderSet(Bullet b, Enemy e)
		{
			this.bullet = b;
			this.enemy = e;
		}

		// Token: 0x0400093F RID: 2367
		public Bullet bullet;

		// Token: 0x04000940 RID: 2368
		public Enemy enemy;
	}
}
