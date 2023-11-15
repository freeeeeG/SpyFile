using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001A RID: 26
public static class SpecialEffects
{
	// Token: 0x0600012B RID: 299 RVA: 0x00008AC1 File Offset: 0x00006CC1
	public static void EnemyHitPlayer(Player player, Enemy enemy)
	{
		if (Player.inst != null)
		{
			SpecialEffects.Thorns(player, enemy);
		}
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00008AD8 File Offset: 0x00006CD8
	public static void BulletHitEnemy(Bullet bullet, Enemy enemy)
	{
		if ((bullet.bulletType == EnumBulletType.NORMAL || bullet.bulletType == EnumBulletType.MISSLE) && Player.inst != null)
		{
			if (Battle.inst.specialEffect[0] >= 1 && MyTool.DecimalToBool(DataBase.Inst.Data_Upgrades[50].buffFacs[0]))
			{
				SpecialEffects.ShootBulletTrackingOnce();
			}
			if (Battle.inst.specialEffect[2] >= 1 && bullet.ifCrit && MyTool.DecimalToBool(DataBase.Inst.Data_Upgrades[52].buffFacs[0]))
			{
				SpecialEffects.ShootBulletTrackingOnce();
			}
			if (Battle.inst.specialEffect[23] >= 1 && enemy.model.rank == EnumRank.EPIC && MyTool.DecimalToBool(DataBase.Inst.Data_Upgrades[49].buffFacs[0]))
			{
				SpecialEffects.ShootBulletTrackingOnce();
			}
		}
		if (bullet.ifCrit && bullet.bulletType != EnumBulletType.LASER && Battle.inst.specialEffect[19] >= 1)
		{
			SpecialEffects.NewExplosionWave(bullet.transform.position, 2f * Mathf.Sqrt(bullet.TotalSize), bullet.mainColor, bullet.TotalDamage, 0f, 0f, bullet.source ? bullet.source.shapeType : EnumShapeType.CIRCLE, false, true, false);
		}
	}

	// Token: 0x0600012D RID: 301 RVA: 0x000051D0 File Offset: 0x000033D0
	public static void ActiveSkill()
	{
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00008C28 File Offset: 0x00006E28
	public static void PlayerGetHurt()
	{
		if (Player.inst != null)
		{
			SpecialEffects.Explosive();
			SpecialEffects.DefenceGrenade();
			SpecialEffects.Delivery();
			SpecialEffects.LastHope();
		}
		if (Battle.inst.specialEffect[30] >= 1)
		{
			Player.inst.transform.position = BattleManager.ChooseBattleItemGenePosInScene();
		}
		BuffManager.UpgradeBuff_Revenge();
		if (Player_4_Tri.instTri != null)
		{
			Player_4_Tri.instTri.GetHurt();
		}
		if (Player_8_Machineguner.instJob8 != null)
		{
			Player_8_Machineguner.instJob8.UpdateFactorTotal_AfterGetHurt();
		}
		BuffManager.DeleteBuffFromUpgrade(228);
		SpecialEffects.Nurturer();
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00008CC1 File Offset: 0x00006EC1
	public static void PlayerGetHeal()
	{
		if (Player_8_Machineguner.instJob8 != null)
		{
			Player_8_Machineguner.instJob8.UpdateFactorTotal_AfterGetHurt();
		}
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00008CDA File Offset: 0x00006EDA
	public static void AttackingStageStart()
	{
		SpecialEffects.Syringe();
	}

	// Token: 0x06000131 RID: 305 RVA: 0x00008CE1 File Offset: 0x00006EE1
	public static void AttackingStageEnd()
	{
		SpecialEffects.Spoil();
		SpecialEffects.TimeBomb();
		SpecialEffects.Adrenaline();
		SpecialEffects.Ambush();
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00008CF7 File Offset: 0x00006EF7
	public static void BattleItemActivate()
	{
		SpecialEffects.Arsenal();
		SpecialEffects.Medkit();
		SpecialEffects.BattleShiled();
	}

	// Token: 0x06000133 RID: 307 RVA: 0x000051D0 File Offset: 0x000033D0
	public static void AfterPlayerGetHurt()
	{
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00008D08 File Offset: 0x00006F08
	public static void EnemyDie(Enemy enemy)
	{
		if (enemy.model.rank == EnumRank.RARE)
		{
			SpecialEffects.EliteHunter();
			if (Battle.inst.specialEffect[20] >= 1)
			{
				SpecialEffects.ShootBulletOnce_Grenade(true);
			}
		}
		else if (enemy.model.rank == EnumRank.EPIC)
		{
			SpecialEffects.AceShooter();
			SpecialEffects.Remains(enemy);
			BattleManager.inst.BossDie();
		}
		if (Battle.inst.specialEffect[18] >= 1)
		{
			SpecialEffects.NewExplosionWave(enemy.transform.position, Mathf.Sqrt(18f + 9f * enemy.unit.lastSize), Player.inst.unit.mainColor, enemy.unit.LifeMax * 0.30000001192092896, 0f, 0f, enemy.unit.shapeType, false, false, false);
		}
		if (TempData.inst.diffiOptFlag[24])
		{
			SpecialEffects.NewExplosionWave(enemy.transform.position, 3f + 3f * Mathf.Sqrt(enemy.unit.lastSize), enemy.unit.mainColor, 1.0, 0f, 0f, enemy.unit.shapeType, true, true, false);
		}
		if (enemy.model.rank == EnumRank.EPIC && Battle.inst.specialEffect[62] >= 1)
		{
			BattleManager.inst.StartCoroutine(SpecialEffects.DestroyEnemyWithRank(EnumRank.RARE, 0.3f, -1));
		}
		if (enemy.model.rank == EnumRank.RARE && Battle.inst.specialEffect[63] >= 1)
		{
			BattleManager.inst.StartCoroutine(SpecialEffects.DestroyEnemyWithRank(EnumRank.NORMAL, 0.3f, 3));
		}
		BuffManager.UpgradeBuff_Justice();
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00008EBC File Offset: 0x000070BC
	private static IEnumerator DestroyEnemyWithRank(EnumRank rank, float totalTime, int maxCount)
	{
		List<Enemy> listDestory = new List<Enemy>();
		foreach (Enemy enemy in BattleManager.inst.listEnemies)
		{
			if (maxCount > 0 && listDestory.Count >= maxCount)
			{
				break;
			}
			if (enemy.model.rank == rank)
			{
				listDestory.Add(enemy);
			}
		}
		foreach (Enemy item in listDestory)
		{
			BattleManager.inst.listEnemies.Remove(item);
		}
		int loop = 0;
		float timeSpend = 0f;
		int cntTotal = listDestory.Count;
		int cntNow = 0;
		while (listDestory.Count > 0)
		{
			int num = loop;
			loop = num + 1;
			if (loop > 1000)
			{
				Debug.LogError("Error_摧毁特定敌人_循环超过一千，跳出");
				break;
			}
			timeSpend += Time.deltaTime;
			int num2 = MyTool.DecimalToInt(timeSpend / totalTime * (float)cntTotal);
			int num3 = num2 - cntNow;
			try
			{
				if (listDestory.Count == 0)
				{
					Debug.LogError("Error_敌人数量为0却仍准备遍历，跳出");
					yield break;
				}
				for (int i = 0; i < num3; i++)
				{
					if (listDestory.Count == 0)
					{
						Debug.LogError("Error_敌人数量为0却仍在进行遍历，跳出");
						break;
					}
					Enemy enemy2 = listDestory[0];
					if (enemy2 != null)
					{
						if (enemy2.unit.ifDie)
						{
							Object.Destroy(enemy2.gameObject);
						}
						else
						{
							enemy2.unit.Die(true);
						}
					}
					listDestory.Remove(enemy2);
				}
				cntNow = num2;
			}
			catch (Exception ex)
			{
				Debug.LogError("Error_摧毁特定敌人_遍历出错:" + ex.ToString());
				yield break;
			}
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield break;
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00008EDC File Offset: 0x000070DC
	private static void Thorns(Player player, Enemy enemy)
	{
		bool flag = Battle.inst.specialEffect[4] != 0;
		Upgrade upgrade = DataBase.Inst.Data_Upgrades[54];
		if (!flag)
		{
			return;
		}
		double bulletDmg = player.unit.playerFactorTotal.bulletDmg;
		double num = bulletDmg * bulletDmg;
		int num2 = (int)(player.unit.LifeMax - player.unit.life);
		num *= (double)(1f + (float)num2 * upgrade.buffFacs[2]);
		enemy.unit.GetHurt(num, player.unit, Vector2.zero, false, enemy.transform.position, true);
	}

	// Token: 0x06000137 RID: 311 RVA: 0x00008F70 File Offset: 0x00007170
	private static void EliteHunter()
	{
		if (Battle.inst.specialEffect[5] == 0)
		{
			return;
		}
		Player.inst.unit.LifeAdd(1.0, true);
	}

	// Token: 0x06000138 RID: 312 RVA: 0x00008F9A File Offset: 0x0000719A
	private static void AceShooter()
	{
		if (Battle.inst.specialEffect[6] == 0)
		{
			return;
		}
		Player.inst.unit.LifeAdd(9.0, true);
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00008FC4 File Offset: 0x000071C4
	private static void Remains(Enemy enemy)
	{
		if (enemy.model.rank != EnumRank.EPIC)
		{
			return;
		}
		int num = Battle.inst.specialEffect[10];
		if (num <= 0)
		{
			return;
		}
		SpecialEffects.GeneNMines(enemy.transform.position, num);
	}

	// Token: 0x0600013A RID: 314 RVA: 0x00009009 File Offset: 0x00007209
	private static void DefenceGrenade()
	{
		if (Battle.inst.specialEffect[12] >= 1)
		{
			SpecialEffects.ShootBulletOnce_Grenade(true);
		}
	}

	// Token: 0x0600013B RID: 315 RVA: 0x00009024 File Offset: 0x00007224
	private static void Explosive()
	{
		if (Battle.inst.specialEffect[73] >= 1)
		{
			while (BattleManager.inst.listMines.Count > 0)
			{
				BattleManager.inst.listMines[0].ForceExplode(2f);
			}
			while (BattleManager.inst.listGrenades.Count > 0)
			{
				BattleManager.inst.listGrenades[0].ForceExplode(2f);
			}
		}
	}

	// Token: 0x0600013C RID: 316 RVA: 0x000090A0 File Offset: 0x000072A0
	private static void Delivery()
	{
		if (Battle.inst.specialEffect[85] <= 0)
		{
			return;
		}
		if (Player.inst == null || Player.inst.unit == null)
		{
			return;
		}
		BasicUnit unit = Player.inst.unit;
		if (unit.life / unit.LifeMax > 0.2)
		{
			return;
		}
		if (Player.inst.special_Delivery < 3f)
		{
			return;
		}
		Player.inst.special_Delivery = 0f;
		SpecialEffects.BattleItem_Random(0f, 0f);
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00009134 File Offset: 0x00007334
	private static void LastHope()
	{
		if (Battle.inst.specialEffect[60] <= 0)
		{
			return;
		}
		Player inst = Player.inst;
		if (inst.unit.life == 1.0)
		{
			if (!MyTool.DecimalToBool(0.25f))
			{
				return;
			}
			inst.RestoreShieldAll();
		}
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00009184 File Offset: 0x00007384
	private static void Nurturer()
	{
		if (!TempData.inst.diffiOptFlag[29])
		{
			return;
		}
		foreach (Enemy enemy in BattleManager.inst.listEnemies)
		{
			enemy.unit.HealPercent(0.1f);
		}
	}

	// Token: 0x0600013F RID: 319 RVA: 0x000091F4 File Offset: 0x000073F4
	private static void Arsenal()
	{
		int num = Battle.inst.specialEffect[13];
		if (num >= 1)
		{
			SpecialEffects.GeneNMines(Player.inst.transform.position, num);
		}
	}

	// Token: 0x06000140 RID: 320 RVA: 0x0000922D File Offset: 0x0000742D
	private static void Medkit()
	{
		if (Battle.inst.specialEffect[44] >= 1)
		{
			Player.inst.unit.LifeAdd(1.0, true);
		}
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00009258 File Offset: 0x00007458
	private static void BattleShiled()
	{
		if (Battle.inst.specialEffect[58] >= 1)
		{
			Player.inst.RestoreShield(1);
		}
	}

	// Token: 0x06000142 RID: 322 RVA: 0x00009275 File Offset: 0x00007475
	public static void Spoil()
	{
		if (Battle.inst.specialEffect[15] <= 0)
		{
			return;
		}
		SpecialEffects.BattleItem_Random(0f, 0f);
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00009298 File Offset: 0x00007498
	public static void BattleItem_Random(float x = 0f, float y = 0f)
	{
		int num = 1;
		if (Battle.inst.specialEffect[25] >= 1)
		{
			num = 2;
		}
		Vector2 vector;
		if (x == 0f && y == 0f)
		{
			vector = BattleManager.ChooseBattleItemGenePosInScene();
		}
		else
		{
			vector = new Vector2(x, y);
		}
		for (int i = 0; i < num; i++)
		{
			Vector2 v;
			if (num == 2)
			{
				v = vector + Random.insideUnitCircle * 3f;
			}
			else
			{
				v = vector;
			}
			Object.Instantiate<GameObject>(ResourceLibrary.Inst.Prefab_BattleItem, v, Quaternion.identity).GetComponent<BattleItem>().Init(DataSelector.GetBuff_Random().typeID);
		}
	}

	// Token: 0x06000144 RID: 324 RVA: 0x00009334 File Offset: 0x00007534
	public static void Adrenaline()
	{
		if (Battle.inst.ForShow_UpgradeNum.Length < 135)
		{
			Debug.LogWarning("Warning_未初始化数组");
			return;
		}
		if (Battle.inst.ForShow_UpgradeNum[134] <= 0)
		{
			return;
		}
		Battle.inst.RemoveUpgrade(134);
	}

	// Token: 0x06000145 RID: 325 RVA: 0x00009384 File Offset: 0x00007584
	public static void Ambush()
	{
		int num = Battle.inst.specialEffect[55];
		if (num >= 1)
		{
			SpecialEffects.GeneNMines(Player.inst.transform.position, num);
		}
	}

	// Token: 0x06000146 RID: 326 RVA: 0x000093C0 File Offset: 0x000075C0
	public static void TimeBomb()
	{
		if (Battle.inst.specialEffect[61] <= 0)
		{
			return;
		}
		List<SpecialBullet_Mine> list = new List<SpecialBullet_Mine>();
		foreach (SpecialBullet_Mine item in BattleManager.inst.listMines)
		{
			list.Add(item);
		}
		foreach (SpecialBullet_Mine specialBullet_Mine in list)
		{
			specialBullet_Mine.EndLife(true);
		}
	}

	// Token: 0x06000147 RID: 327 RVA: 0x0000946C File Offset: 0x0000766C
	public static void Syringe()
	{
		if (Battle.inst.specialEffect[54] <= 0)
		{
			return;
		}
		Battle.inst.Upgrade_Gain(134);
	}

	// Token: 0x06000148 RID: 328 RVA: 0x0000948E File Offset: 0x0000768E
	public static IEnumerator ShootBulletTrackingInTimeAndDelta(float totalTime, float deltaTime)
	{
		int cnt = MyTool.DecimalToInt(totalTime / deltaTime);
		int shootNum = 0;
		float timeSpend = 0f;
		while (timeSpend <= totalTime)
		{
			timeSpend += Time.deltaTime;
			int num = Mathf.CeilToInt(timeSpend / totalTime * (float)cnt);
			int num2 = num - shootNum;
			for (int i = 0; i < num2; i++)
			{
				SpecialEffects.ShootBulletTrackingOnce();
			}
			shootNum = num;
			if (num2 > 0)
			{
				SoundEffects.Inst.shoot.PlayRandom();
			}
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield break;
	}

	// Token: 0x06000149 RID: 329 RVA: 0x000094A4 File Offset: 0x000076A4
	public static void NewExplosionWave(Vector2 position, float scale, Color color, double damage, float crit, float critEffect, EnumShapeType shapeType, bool ifEnemy = false, bool ifTriggerDoubleSword = true, bool ifCrit = false)
	{
		SoundEffects.Inst.enemyDie.PlayRandom();
		GameObject pool_BlastWave = ObjectPool.inst.GetPool_BlastWave(ResourceLibrary.Inst.Prefab_BlastWave);
		pool_BlastWave.transform.position = position;
		Skill_Player8_Wave component = pool_BlastWave.GetComponent<Skill_Player8_Wave>();
		if (ifCrit && Player.inst != null)
		{
			damage *= (double)Player.inst.unit.playerFactorTotal.critDmg;
		}
		component.Init(scale, color, damage, crit, critEffect, shapeType, ifEnemy, ifTriggerDoubleSword);
	}

	// Token: 0x0600014A RID: 330 RVA: 0x00009526 File Offset: 0x00007726
	public static void ShootBulletTrackingOnce()
	{
		SoundEffects.Inst.bullet_Missle.PlayRandom();
		Player.inst.unit.Shoot_GeneBulletOnce(Player.inst.unit.Shoot_Sequential_ChooseNextEmitGun(), ResourceLibrary.Inst.Prefab_Bullet_Tracking);
	}

	// Token: 0x0600014B RID: 331 RVA: 0x00009560 File Offset: 0x00007760
	public static GameObject ShootBulletOnce_Mine()
	{
		if (Player.inst == null)
		{
			return null;
		}
		SoundEffects.Inst.bullet_Mine.PlayRandom();
		GameObject gameObject = Player.inst.unit.Shoot_GeneBulletOnce(Player.inst.unit.Shoot_Sequential_ChooseNextEmitGun(), ResourceLibrary.Inst.Prefab_Bullet_Mine);
		gameObject.transform.position = Player.inst.transform.position;
		return gameObject;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x000095D0 File Offset: 0x000077D0
	public static void GeneNMines(Vector2 pos, int n)
	{
		if (Player.inst == null)
		{
			return;
		}
		SoundEffects.Inst.bullet_Mine.PlayRandom();
		for (int i = 0; i < n; i++)
		{
			GameObject gameObject = Player.inst.unit.Shoot_GeneBulletOnce(Player.inst.unit.Shoot_Sequential_ChooseNextEmitGun(), ResourceLibrary.Inst.Prefab_Bullet_Mine);
			float d = (n == 1) ? 0f : (Player.inst.unit.lastScale / 2f + 0.5f);
			float f = (float)i * 360f / (float)n / 180f * 3.1415927f;
			Vector2 b = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * d;
			gameObject.transform.position = pos + b;
		}
	}

	// Token: 0x0600014D RID: 333 RVA: 0x000096A1 File Offset: 0x000078A1
	public static GameObject ShootBulletOnce_Grenade(bool ifSound = true)
	{
		if (ifSound)
		{
			SoundEffects.Inst.bullet_Grenade.PlayRandom();
		}
		return Player.inst.unit.Shoot_GeneBulletOnce(Player.inst.unit.Shoot_Sequential_ChooseNextEmitGun(), ResourceLibrary.Inst.Prefab_Bullet_Grenade);
	}

	// Token: 0x0600014E RID: 334 RVA: 0x000096E0 File Offset: 0x000078E0
	public static void Amulet()
	{
		if (Battle.inst.specialEffect[51] == 0)
		{
			Debug.LogError("Error_非法触发护身符");
			return;
		}
		float num = 0.6f;
		GameObject gameObject = Object.Instantiate<GameObject>(ResourceLibrary.Inst.Prefab_BlastWave);
		Skill_Player8_Wave component = gameObject.GetComponent<Skill_Player8_Wave>();
		BasicUnit unit = Player.inst.unit;
		Factor playerFactorTotal = unit.playerFactorTotal;
		gameObject.transform.position = Player.inst.transform.position;
		component.Init(component.maxScale * num, unit.mainColor, Battle.inst.factorBattleTotal.Enemy_ModLife * 10000.0, 0f, playerFactorTotal.critDmg, unit.shapeType, false, true);
		SoundEffects.Inst.skill_Blast.PlayRandom();
		SoundEffects.Inst.playerHurt.PlayRandom();
	}
}
