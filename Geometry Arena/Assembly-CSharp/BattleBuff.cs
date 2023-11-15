using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x0200001F RID: 31
[Serializable]
public class BattleBuff
{
	// Token: 0x17000076 RID: 118
	// (get) Token: 0x0600017B RID: 379 RVA: 0x0000A286 File Offset: 0x00008486
	public string Lang_Name
	{
		get
		{
			return this.names[(int)Setting.Inst.language];
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x0600017C RID: 380 RVA: 0x0000A299 File Offset: 0x00008499
	public string Lang_Infos
	{
		get
		{
			return this.infos[(int)Setting.Inst.language];
		}
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0000A2AC File Offset: 0x000084AC
	public void Init(int typeID)
	{
		BattleBuff clone = DataBase.Inst.Data_BattleBuffs[typeID];
		this.Clone(clone);
		UI_FloatTextControl.inst.Special_BattleItemActive(this);
		if (this.lifeTimeMax != 0f)
		{
			UI_Panel_Battle_BattleBuffsShow.inst.NewBuffIcon(typeID, this);
		}
		switch (typeID)
		{
		case 0:
			this.InitFactorMultis_Type0_SuperFireSpd();
			break;
		case 1:
			this.InitFactorMultis_Type1_SuperCrit();
			break;
		case 2:
			this.InitFactorMultis_Type2_DoubleSpeed();
			break;
		case 3:
			this.InitEffect_Type3_TrackingBullets();
			break;
		case 4:
			this.InitEffect_Type4_HpSmallRecover();
			break;
		case 5:
			this.InitEffect_Type5_OneGrenade();
			break;
		case 6:
			this.InitEffect_Type6_OneMine();
			break;
		case 10:
			this.InitFactorMultis_Type10_TripeFireSpd();
			break;
		case 11:
			this.InitFactorMultis_Type11_RareCrit();
			break;
		case 12:
			this.InitEffect_Type12_BlastWave();
			break;
		case 13:
			this.InitEffect_Type13_HpRareRecover();
			break;
		case 14:
			Player.inst.StartCoroutine(this.InitEffect_Type14_TripleGrenade());
			break;
		case 20:
			this.InitFactorMultis_Type20_SlowMotion();
			break;
		case 21:
			this.InitFactorMultis_Type21_SuperLaser();
			break;
		case 22:
			this.InitEffect_Type22_HpAllRecover();
			break;
		case 23:
			Player.inst.StartCoroutine(this.InitEffect_Type23_MinesGroup());
			break;
		case 30:
			Player.inst.StartCoroutine(this.InitEffect_Type30_ChainBlastWave());
			break;
		case 31:
			Player.inst.StartCoroutine(this.InitEffect_Type31_GrenadeRain());
			break;
		case 32:
			Player.inst.StartCoroutine(this.InitEffect_Type32_UltraMinesGroup());
			break;
		case 33:
			this.InitFactorMultis_Type33_GodTime();
			break;
		}
		if (this.lifeTimeMax > 0f)
		{
			BattleManager.inst.StartCoroutine(this.BattleBuffAddAndRemove());
		}
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0000A495 File Offset: 0x00008695
	public IEnumerator BattleBuffAddAndRemove()
	{
		BattleManager.inst.listBattleBuffs.Add(this);
		yield return new WaitForSeconds(this.lifeTimeMax);
		BattleManager.inst.listBattleBuffs.Remove(this);
		yield break;
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000A4A4 File Offset: 0x000086A4
	public void Clone(BattleBuff clone)
	{
		this.dataName = clone.dataName;
		this.rank = clone.rank;
		this.typeID = clone.typeID;
		this.lifeTimeMax = clone.lifeTimeMax;
		this.facs = new float[clone.facs.Length];
		for (int i = 0; i < this.facs.Length; i++)
		{
			this.facs[i] = clone.facs[i];
		}
		this.factorMultis = new FactorMultis();
		this.names = new string[clone.names.Length];
		this.infos = new string[clone.infos.Length];
		for (int j = 0; j < this.names.Length; j++)
		{
			this.names[j] = clone.names[j];
		}
		for (int k = 0; k < this.infos.Length; k++)
		{
			this.infos[k] = clone.infos[k];
		}
		this.ifAvailable = clone.ifAvailable;
		this.source = clone.source;
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0000A5A8 File Offset: 0x000087A8
	public void SourceFromUpgrade(Upgrade up)
	{
		this.dataName = up.dataName;
		this.rank = up.rank;
		this.typeID = up.id;
		this.facs = up.buffFacs;
		this.names = new string[up.names.Length];
		this.infos = new string[up.infos.Length];
		for (int i = 0; i < this.names.Length; i++)
		{
			this.names[i] = up.names[i];
		}
		for (int j = 0; j < this.infos.Length; j++)
		{
			this.infos[j] = up.infos[j];
		}
		this.source = BattleBuff.EnumBuffSource.UPGRADE;
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000A658 File Offset: 0x00008858
	public bool GetBool_IfFromUpdate(int upID)
	{
		return this.source == BattleBuff.EnumBuffSource.UPGRADE && this.typeID == upID;
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000A66E File Offset: 0x0000886E
	private void InitFactorMultis_Type0_SuperFireSpd()
	{
		this.factorMultis.factorMultis[3] = this.facs[2];
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0000A685 File Offset: 0x00008885
	private void InitFactorMultis_Type1_SuperCrit()
	{
		this.factorMultis.factorMultis[10] = this.facs[2];
		this.factorMultis.factorMultis[11] = this.facs[4];
	}

	// Token: 0x06000184 RID: 388 RVA: 0x0000A6B3 File Offset: 0x000088B3
	private void InitFactorMultis_Type2_DoubleSpeed()
	{
		this.factorMultis.factorMultis[2] = this.facs[2];
	}

	// Token: 0x06000185 RID: 389 RVA: 0x0000A6CC File Offset: 0x000088CC
	private void InitEffect_Type3_TrackingBullets()
	{
		float totalTime = this.facs[0];
		float num = BulletsOptimization.ActualFireSpeed() * this.facs[2];
		num = Mathf.Clamp(num, 1f, 30f);
		float deltaTime = 1f / num;
		Player.inst.StartCoroutine(SpecialEffects.ShootBulletTrackingInTimeAndDelta(totalTime, deltaTime));
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0000A71C File Offset: 0x0000891C
	private void InitEffect_Type4_HpSmallRecover()
	{
		BasicUnit unit = Player.inst.unit;
		double lifeMax = unit.LifeMax;
		int num = (int)math.ceil((double)this.facs[1] * lifeMax);
		unit.LifeAdd((double)num, true);
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000A754 File Offset: 0x00008954
	private void InitEffect_Type5_OneGrenade()
	{
		SpecialEffects.ShootBulletOnce_Grenade(true);
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0000A75D File Offset: 0x0000895D
	private void InitEffect_Type6_OneMine()
	{
		SpecialEffects.ShootBulletOnce_Mine().transform.position = Player.inst.transform.position;
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0000A66E File Offset: 0x0000886E
	private void InitFactorMultis_Type10_TripeFireSpd()
	{
		this.factorMultis.factorMultis[3] = this.facs[2];
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0000A77D File Offset: 0x0000897D
	private void InitFactorMultis_Type11_RareCrit()
	{
		this.factorMultis.factorMultis[10] = this.facs[2];
		this.factorMultis.factorMultis[11] = Player.inst.unit.playerFactorTotal.critDmg;
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0000A7B8 File Offset: 0x000089B8
	private void InitEffect_Type12_BlastWave()
	{
		GameObject gameObject = Object.Instantiate<GameObject>(ResourceLibrary.Inst.Prefab_BlastWave);
		Skill_Player8_Wave component = gameObject.GetComponent<Skill_Player8_Wave>();
		BasicUnit unit = Player.inst.unit;
		Factor playerFactorTotal = unit.playerFactorTotal;
		gameObject.transform.position = Player.inst.transform.position;
		component.Init(component.maxScale * 0.36f, unit.mainColor, (double)this.facs[3] * playerFactorTotal.bulletDmg * playerFactorTotal.bulletDmg, 0f, playerFactorTotal.critDmg, unit.shapeType, false, true);
		SoundEffects.Inst.skill_Blast.PlayRandom();
		SoundEffects.Inst.playerHurt.PlayRandom();
	}

	// Token: 0x0600018C RID: 396 RVA: 0x0000A868 File Offset: 0x00008A68
	private void InitEffect_Type13_HpRareRecover()
	{
		BasicUnit unit = Player.inst.unit;
		double lifeMax = unit.LifeMax;
		int num = (int)math.ceil((double)this.facs[1] * lifeMax);
		unit.LifeAdd((double)num, true);
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000A8A0 File Offset: 0x00008AA0
	private IEnumerator InitEffect_Type14_TripleGrenade()
	{
		int num;
		for (int i = 0; i < 3; i = num + 1)
		{
			SpecialEffects.ShootBulletOnce_Grenade(true);
			yield return new WaitForSeconds(0.3f);
			num = i;
		}
		yield break;
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000A8A8 File Offset: 0x00008AA8
	private IEnumerator InitEffect_Type31_GrenadeRain()
	{
		int num = this.facs[1].Int();
		int num2;
		for (int i = 0; i < num; i = num2 + 1)
		{
			SpecialEffects.ShootBulletOnce_Grenade(true);
			yield return new WaitForSeconds(0.06f);
			num2 = i;
		}
		yield break;
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000A8B7 File Offset: 0x00008AB7
	private void InitFactorMultis_Type20_SlowMotion()
	{
		TimeManager.inst.SetBuffTimeScale(this.facs[1], this.facs[0]);
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0000A8D4 File Offset: 0x00008AD4
	private void InitFactorMultis_Type21_SuperLaser()
	{
		this.factorMultis.factorMultis[this.facs[1].Int()] = this.facs[2];
		this.factorMultis.factorMultis[this.facs[3].Int()] = this.facs[4];
		this.factorMultis.factorMultis[this.facs[5].Int()] = this.facs[6];
		this.factorMultis.factorMultis[this.facs[7].Int()] = this.facs[8];
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000A968 File Offset: 0x00008B68
	private void InitFactorMultis_Type33_GodTime()
	{
		this.factorMultis.factorMultis[this.facs[1].Int()] = this.facs[2];
		this.factorMultis.factorMultis[this.facs[3].Int()] = this.facs[4];
		this.factorMultis.factorMultis[this.facs[5].Int()] = this.facs[6];
		TimeManager.inst.SetBuffTimeScale(this.facs[7], this.facs[0]);
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0000A9F4 File Offset: 0x00008BF4
	private void InitEffect_Type22_HpAllRecover()
	{
		BasicUnit unit = Player.inst.unit;
		double lifeMax = unit.LifeMax;
		int num = (int)math.ceil((double)this.facs[1] * lifeMax);
		unit.LifeAdd((double)num, true);
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000AA2C File Offset: 0x00008C2C
	private IEnumerator InitEffect_Type23_MinesGroup()
	{
		int num = MyTool.DecimalToInt(this.facs[0] / this.facs[1]);
		int num2;
		for (int i = 0; i < num; i = num2 + 1)
		{
			SpecialEffects.ShootBulletOnce_Mine().transform.position = Player.inst.transform.position;
			yield return new WaitForSeconds(this.facs[1]);
			num2 = i;
		}
		yield break;
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000AA3B File Offset: 0x00008C3B
	private IEnumerator InitEffect_Type32_UltraMinesGroup()
	{
		int num = MyTool.DecimalToInt(this.facs[0] / this.facs[1]);
		int num2;
		for (int i = 0; i < num; i = num2 + 1)
		{
			SpecialEffects.ShootBulletOnce_Mine().transform.position = Player.inst.transform.position;
			yield return new WaitForSeconds(this.facs[1]);
			num2 = i;
		}
		yield break;
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0000AA4A File Offset: 0x00008C4A
	public string GetInfoWithFac()
	{
		return this.Lang_Infos.GetColorfulInfoWithFacs(this.facs, false);
	}

	// Token: 0x06000196 RID: 406 RVA: 0x0000AA5E File Offset: 0x00008C5E
	private IEnumerator InitEffect_Type30_ChainBlastWave()
	{
		int num2;
		for (int i = 0; i < 3; i = num2 + 1)
		{
			float num;
			if (i == 0)
			{
				num = 0.3f;
			}
			else if (i == 1)
			{
				num = 0.6f;
			}
			else
			{
				num = 1f;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(ResourceLibrary.Inst.Prefab_BlastWave);
			Skill_Player8_Wave component = gameObject.GetComponent<Skill_Player8_Wave>();
			BasicUnit unit = Player.inst.unit;
			Factor playerFactorTotal = unit.playerFactorTotal;
			gameObject.transform.position = Player.inst.transform.position;
			component.Init(component.maxScale * num, unit.mainColor, (double)this.facs[3] * playerFactorTotal.bulletDmg * playerFactorTotal.bulletDmg, 0f, playerFactorTotal.critDmg, unit.shapeType, false, true);
			SoundEffects.Inst.skill_Blast.PlayRandom();
			SoundEffects.Inst.playerHurt.PlayRandom();
			yield return new WaitForSeconds(this.facs[4]);
			num2 = i;
		}
		yield break;
	}

	// Token: 0x04000190 RID: 400
	public string dataName = "UNINITED";

	// Token: 0x04000191 RID: 401
	public string[] names = new string[3];

	// Token: 0x04000192 RID: 402
	public string[] infos = new string[3];

	// Token: 0x04000193 RID: 403
	public int typeID = -1;

	// Token: 0x04000194 RID: 404
	public int layerThis = 1;

	// Token: 0x04000195 RID: 405
	public EnumRank rank;

	// Token: 0x04000196 RID: 406
	public bool ifAvailable;

	// Token: 0x04000197 RID: 407
	public float lifeTimeMax = 3f;

	// Token: 0x04000198 RID: 408
	public float[] facs = new float[3];

	// Token: 0x04000199 RID: 409
	[HideInInspector]
	public FactorMultis factorMultis = new FactorMultis();

	// Token: 0x0400019A RID: 410
	public BattleBuff.EnumBuffSource source;

	// Token: 0x0200013B RID: 315
	public enum EnumBuffSource
	{
		// Token: 0x04000971 RID: 2417
		BATTLEITEM,
		// Token: 0x04000972 RID: 2418
		UPGRADE
	}
}
