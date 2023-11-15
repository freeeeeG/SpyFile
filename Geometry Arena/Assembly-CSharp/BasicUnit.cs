using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020000E7 RID: 231
public class BasicUnit : CommonObj
{
	// Token: 0x170000EE RID: 238
	// (get) Token: 0x0600080E RID: 2062 RVA: 0x0002DF65 File Offset: 0x0002C165
	// (set) Token: 0x0600080F RID: 2063 RVA: 0x0002DF72 File Offset: 0x0002C172
	[SerializeField]
	public double life
	{
		get
		{
			return this.lifeData;
		}
		set
		{
			this.lifeData = value;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000810 RID: 2064 RVA: 0x0002DF80 File Offset: 0x0002C180
	public double LifeMax
	{
		get
		{
			if (this.objType == EnumObjType.PLAYER)
			{
				return (double)this.playerFactorTotal.lifeMaxPlayer;
			}
			return this.EnemyFactorTotal.lifeMaxEnemy;
		}
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06000811 RID: 2065 RVA: 0x0002DFA3 File Offset: 0x0002C1A3
	// (set) Token: 0x06000812 RID: 2066 RVA: 0x0002DFAB File Offset: 0x0002C1AB
	[SerializeField]
	public Factor FactorBasic
	{
		get
		{
			return this.factor;
		}
		set
		{
			this.factor = value;
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000813 RID: 2067 RVA: 0x0002DFB4 File Offset: 0x0002C1B4
	[SerializeField]
	public Factor FactorTotalNew
	{
		get
		{
			EnumObjType objType = this.objType;
			if (objType == EnumObjType.PLAYER)
			{
				return this.playerFactorTotal;
			}
			if (objType == EnumObjType.ENEMY)
			{
				return this.factor;
			}
			Debug.LogError("WhatFactorTotal?");
			return this.factor;
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000814 RID: 2068 RVA: 0x0002DFA3 File Offset: 0x0002C1A3
	public Factor EnemyFactorTotal
	{
		get
		{
			return this.factor;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000815 RID: 2069 RVA: 0x0002DFEE File Offset: 0x0002C1EE
	private Color Color_Current
	{
		get
		{
			if (this.list_SprsToDye[0] == null)
			{
				Debug.LogWarning("Warning Spr==null : Return Red");
				return Color.red;
			}
			return this.list_SprsToDye[0].color;
		}
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000816 RID: 2070 RVA: 0x0002E025 File Offset: 0x0002C225
	public float Fac_MaxShootTime
	{
		get
		{
			if (this.objType == EnumObjType.PLAYER)
			{
				return 1f / BulletsOptimization.ActualFireSpeed();
			}
			return 1f / this.EnemyFactorTotal.fireSpd;
		}
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000817 RID: 2071 RVA: 0x0002E04D File Offset: 0x0002C24D
	public double Fac_ActualBulletDamage
	{
		get
		{
			if (this.objType == EnumObjType.PLAYER)
			{
				return BulletsOptimization.ActualBulletDamage();
			}
			return this.EnemyFactorTotal.bulletDmg;
		}
	}

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x06000818 RID: 2072 RVA: 0x0002E069 File Offset: 0x0002C269
	public float Fac_ActualRepulse
	{
		get
		{
			return this.FactorTotalNew.repulse;
		}
	}

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06000819 RID: 2073 RVA: 0x0002E076 File Offset: 0x0002C276
	public float Fac_ActualRecoil
	{
		get
		{
			return this.FactorTotalNew.recoil;
		}
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x0002E083 File Offset: 0x0002C283
	private void Update()
	{
		if (TempData.inst.currentSceneType != EnumSceneType.BATTLE)
		{
			return;
		}
		if (TimeManager.inst != null && TimeManager.inst.ifPause)
		{
			return;
		}
		bool flag = this.inited;
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x0002E0B4 File Offset: 0x0002C2B4
	private void FixedUpdate()
	{
		if (this.IfObj_Enemy() && !this.IfScene_Battle())
		{
			return;
		}
		if (TimeManager.inst != null && TimeManager.inst.ifPause)
		{
			return;
		}
		if (Time.timeScale == 0f)
		{
			return;
		}
		if (!this.inited)
		{
			return;
		}
		this.RecoverInFixedUpdate();
		this.FixedUpdate_ScaleSmooth();
		this.FixedUpdate_ColorSmooth();
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x0002E114 File Offset: 0x0002C314
	private bool IfObj_Enemy()
	{
		return this.objType == EnumObjType.ENEMY;
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x0002E11F File Offset: 0x0002C31F
	private bool IfObj_Player()
	{
		return this.objType == EnumObjType.PLAYER;
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x0001E1E3 File Offset: 0x0001C3E3
	private bool IfScene_Battle()
	{
		return TempData.inst.currentSceneType == EnumSceneType.BATTLE;
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x0002E12C File Offset: 0x0002C32C
	public double GetDouble_LifePct()
	{
		double result = this.life / this.LifeMax;
		if (double.IsInfinity(this.life) || double.IsInfinity(this.LifeMax) || double.IsNaN(this.life) || double.IsNaN(this.LifeMax))
		{
			result = 1.0;
		}
		return result;
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x0002E188 File Offset: 0x0002C388
	private void FixedUpdate_ScaleSmooth()
	{
		float num = this.FactorTotalNew.bodySize;
		if (TempData.inst.diffiOptFlag[20] && this.objType == EnumObjType.ENEMY)
		{
			float num2 = Mathf.Clamp((float)this.GetDouble_LifePct(), 0f, 1f);
			float num3 = (1f - num2) * 2f + 1f;
			if ((double)num3 > 2.5)
			{
				num3 = 2.5f;
			}
			this.puffer_SizeMulti = num3;
			num *= this.puffer_SizeMulti;
		}
		if (MyTool.ifSimiliar(num, this.lastSize))
		{
			return;
		}
		float target = Mathf.Sqrt(num);
		float current = this.lastScale;
		float smoothTime = 0.2f;
		if (this.objType == EnumObjType.PLAYER)
		{
			smoothTime = 0.05f;
		}
		float d = Mathf.SmoothDamp(current, target, ref this.scaleSmooth_Ref, smoothTime);
		this.rb.useAutoMass = false;
		this.rb.mass = num;
		base.transform.localScale = Vector2.one * d;
		this.lastScale = d;
		this.lastSize = this.lastScale * this.lastScale;
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x0002E29C File Offset: 0x0002C49C
	private void FixedUpdate_ColorSmooth()
	{
		if (this.lastColorSat == 0f)
		{
			this.lastColorSat = this.Color_Current.Saturation();
		}
		float num = this.lastColorSat;
		float num2 = this.mainColor.Saturation();
		if (!this.flagInvincible && this.Color_Current.a == 0f)
		{
			this.invincible_Alpha = 1f;
			this.DyeSprsWithColor(this.Color_Current.SetAlpha(1f));
		}
		if (MyTool.ifSimiliar(num, num2))
		{
			return;
		}
		float num3 = Mathf.SmoothDamp(num, num2, ref this.colorSmooth_Ref, 0.3f);
		num3 = math.clamp(num3, 0f, 1f);
		this.DyeSprsWithColor(this.mainColor.SetSaturation(num3));
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x0002E358 File Offset: 0x0002C558
	public void LifeAdd(double num, bool ifCanBlockByShield = true)
	{
		if (num == 0.0)
		{
			return;
		}
		if (num > 0.0)
		{
			if (!BattleManager.inst.GameOn)
			{
				return;
			}
			if (this.objType == EnumObjType.PLAYER && Battle.inst.specialEffect[45] >= 1 && this.life / this.LifeMax <= 0.20000000298023224)
			{
				num *= 2.0;
			}
			FloatDamageTextControl.inst.NewFloatBattleText(base.transform.position + this.lastScale * UnityEngine.Random.Range(0f, 1f) * UnityEngine.Random.insideUnitCircle, num, false, false, true);
		}
		if (this.objType == EnumObjType.PLAYER && num < 0.0)
		{
			Player inst = Player.inst;
			if (inst.shield > 0 && ifCanBlockByShield)
			{
				inst.RemoveShield(1);
				num = 0.0;
			}
		}
		if (this.objType == EnumObjType.PLAYER && num > 0.0 && Battle.inst.specialEffect[68] >= 1 && this.life == this.LifeMax && Player.inst.shield == 0)
		{
			Player.inst.RestoreShield(1);
		}
		this.life += num;
		if (this.objType == EnumObjType.PLAYER && num < 0.0 && this.life <= 0.0)
		{
			if (Battle.inst.specialEffect[51] > 0)
			{
				SpecialEffects.Amulet();
				Battle.inst.RemoveUpgrade(158);
				this.life = this.LifeMax;
			}
			else
			{
				this.life = 0.0;
			}
		}
		else if (this.life >= this.LifeMax)
		{
			this.life = this.LifeMax;
		}
		if (this.objType == EnumObjType.PLAYER)
		{
			HealthPointControl.inst.UpdateHpUnits();
			BuffManager.RefreshStateBuff_AboutLife();
		}
		if (num > 0.0)
		{
			SpecialEffects.PlayerGetHeal();
		}
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x0002E552 File Offset: 0x0002C752
	public void HealAll()
	{
		this.LifeAdd(this.LifeMax, true);
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x0002E561 File Offset: 0x0002C761
	public void HealPercent(float pct)
	{
		this.LifeAdd(this.LifeMax * (double)pct, true);
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x0002E573 File Offset: 0x0002C773
	public void HealHP(float hp)
	{
		this.LifeAdd((double)hp, true);
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x0002E580 File Offset: 0x0002C780
	public void GetHurt(double dmgNum, BasicUnit source, Vector2 dirRepulse, bool ifCrit, Vector2 hitPosition, bool ifTriggerDoubleEdge = true)
	{
		if (this.flagInvincible && source == this)
		{
			this.flagInvincible = false;
			if (this.coroutineInvincible != null)
			{
				base.StopCoroutine(this.coroutineInvincible);
			}
		}
		int num = Battle.inst.specialEffect[7];
		if (num >= 1 && ifTriggerDoubleEdge && !this.flagInvincible)
		{
			if (this.IfObj_Player())
			{
				base.StartCoroutine(this.DoubleEdge(num, (double)num));
			}
			else if (this.IfObj_Enemy())
			{
				dmgNum *= math.pow(2.0, (double)num);
			}
		}
		if (Battle.inst.specialEffect[77] >= 1)
		{
			double life = Player.inst.unit.life;
			double lifeMax = Player.inst.unit.LifeMax;
			if (this.IfObj_Enemy() && life / lifeMax >= 0.5)
			{
				dmgNum *= 2.0;
			}
			if (this.IfObj_Player() && life / lifeMax <= 0.5)
			{
				dmgNum *= 0.5;
			}
		}
		if (this.IfObj_Player())
		{
			int layer_BuffID = BattleManager.inst.GetLayer_BuffID(225);
			if (layer_BuffID > 0)
			{
				float num2 = DataBase.Inst.Data_Upgrades[225].buffFacs[3];
				dmgNum *= (double)(1f + num2 * (float)layer_BuffID);
			}
			if (Battle.inst.specialEffect[87] >= 1)
			{
				if (source == this)
				{
					dmgNum *= 0.5;
				}
				else
				{
					dmgNum *= 3.0;
				}
			}
			if (TempData.inst.jobId == 4)
			{
				if (TempData.inst.GetBool_SkillModuleOpenFlag(4))
				{
					dmgNum *= 2.0;
				}
				if (TempData.inst.GetBool_SkillModuleOpenFlag(5))
				{
					dmgNum *= 3.0;
				}
			}
		}
		dmgNum = math.ceil(dmgNum);
		if (dmgNum < 0.0)
		{
			Debug.LogError("dmgNum<0!  " + dmgNum);
			return;
		}
		if (double.IsNaN(dmgNum))
		{
			dmgNum = 0.0;
		}
		if (double.IsInfinity(dmgNum))
		{
			dmgNum = 1E+308;
		}
		if (!this.flagInvincible)
		{
			if (this.IfObj_Enemy() && UI_Icon_BattleDPS.inst != null)
			{
				UI_Icon_BattleDPS.inst.Damage_Add(dmgNum);
			}
			this.LifeAdd(-dmgNum, true);
			FloatDamageTextControl.inst.NewFloatBattleText(hitPosition, dmgNum, ifCrit, this.IfObj_Player(), false);
			if (this.life <= 0.0)
			{
				this.Die(true);
			}
			else
			{
				this.DyeSprsWithColor(this.mainColor.SetSaturation(0f));
				if (this.objType == EnumObjType.PLAYER)
				{
					base.StartCoroutine(this.Wheel());
				}
			}
		}
		if (this.objType == EnumObjType.PLAYER && !this.flagInvincible)
		{
			float player_InvincibleTime = Battle.inst.factorBattleTotal.Player_InvincibleTime;
			this.EnterInvincibleState(0.1f * player_InvincibleTime);
			SpecialEffects.PlayerGetHurt();
			SoundEffects.Inst.playerHurt.PlayRandom();
			Vector2 vector = dirRepulse * this.rb.mass * GameParameters.Inst.RepulseForceByEnemy;
			if (Battle.inst.specialEffect[72] >= 1)
			{
				vector *= 0.5f;
			}
			if (TempData.GetBool_Stridebreaker())
			{
				vector *= 3f;
			}
			this.rb.MyAddForce(this.playerFactorTotal.bodySize, vector);
		}
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x0002E8DB File Offset: 0x0002CADB
	private IEnumerator Wheel()
	{
		if (Battle.inst.specialEffect[47] <= 0)
		{
			yield break;
		}
		if (MyTool.DecimalToBool(0.55f))
		{
			yield return new WaitForSeconds(0.1f);
			this.LifeAdd(3.0, true);
			yield break;
		}
		yield return new WaitForSeconds(0.3f);
		this.HurtSelf(3, false);
		yield break;
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x0002E8EA File Offset: 0x0002CAEA
	private IEnumerator DoubleEdge(int num, double dmgNum)
	{
		double hurtTimes = (double)num;
		for (double i = 0.0; i < hurtTimes; i += 1.0)
		{
			yield return new WaitForSeconds(0.1f);
			this.HurtSelf((int)dmgNum, false);
		}
		yield break;
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x0002E907 File Offset: 0x0002CB07
	public void EnterInvincibleState(float pieceTime)
	{
		if (this.coroutineInvincible != null)
		{
			base.StopCoroutine(this.coroutineInvincible);
		}
		float player_InvincibleTime = Battle.inst.factorBattleTotal.Player_InvincibleTime;
		this.coroutineInvincible = base.StartCoroutine(this.InvincibleTime(4, pieceTime, pieceTime));
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x0002E942 File Offset: 0x0002CB42
	public void HurtSelf(int damage, bool ifTriggerDoubleEdge = true)
	{
		this.GetHurt((double)damage, this, Vector2.zero, false, base.transform.position, ifTriggerDoubleEdge);
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x0002E964 File Offset: 0x0002CB64
	public void SetCollis(bool flag)
	{
		if (this.objType != EnumObjType.ENEMY)
		{
			return;
		}
		if (this.collider2Ds.Length == 0)
		{
			Debug.LogError("Error:No Colli!");
		}
		Collider2D[] array = this.collider2Ds;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = flag;
		}
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x0002E9AC File Offset: 0x0002CBAC
	public void Die(bool ifNormal)
	{
		this.SetCollis(false);
		if (this.ifDie)
		{
			return;
		}
		this.SetCollis(false);
		this.ifDie = true;
		this.unitDieParticle();
		switch (this.objType)
		{
		case EnumObjType.UNINITED:
			Debug.LogError("ObjTypeError!");
			break;
		case EnumObjType.PLAYER:
			BattleManager.inst.GameOn = false;
			BattleManager.inst.GameOver();
			SoundEffects.Inst.enemyDie.PlayRandom();
			TimeManager.inst.PlayerDie();
			break;
		case EnumObjType.ENEMY:
		{
			Enemy component = base.gameObject.GetComponent<Enemy>();
			BattleManager.inst.listEnemies.Remove(component);
			if (ifNormal)
			{
				SpecialEffects.EnemyDie(component);
			}
			component.Trig_SplitAtOnce();
			int num = component.model.CoinNum(component.fragMulti);
			if (num > 0)
			{
				Fragment.InstantiateFragments(base.transform.position, num, this.shapeType, this.mainColor, this.lastScale / Mathf.Sqrt((float)num));
			}
			if (ifNormal)
			{
				TempData.inst.battle.Score_Gain(component.model.Score(component.starMulti));
			}
			if (component.summoned && !component.debug_HasAmountAdd)
			{
				Debug.LogError("数量还没加呢");
			}
			else
			{
				BattleManager.inst.EnemyAmountReduceOne();
			}
			SoundEffects.Inst.enemyDie.PlayRandom();
			if (ifNormal)
			{
				MySteamAchievement.AddStatInt("acc_Enemy_Normal", 1);
				EnumRank rank = component.model.rank;
				if (rank == EnumRank.RARE)
				{
					MySteamAchievement.AddStatInt("acc_Enemy_Elite", 1);
				}
				if (rank == EnumRank.EPIC)
				{
					MySteamAchievement.AddStatInt("acc_Enemy_Boss", 1);
				}
			}
			break;
		}
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x0002EB48 File Offset: 0x0002CD48
	public void Die_Fusion()
	{
		this.SetCollis(false);
		if (this.ifDie)
		{
			Debug.LogError("HaveDie!");
			return;
		}
		this.ifDie = true;
		this.unitDieParticle();
		EnumObjType objType = this.objType;
		if (objType == EnumObjType.ENEMY)
		{
			Enemy component = base.gameObject.GetComponent<Enemy>();
			BattleManager.inst.listEnemies.Remove(component);
			if (component.summoned && !component.debug_HasAmountAdd)
			{
				Debug.LogError("数量还没加呢");
			}
			else
			{
				BattleManager.inst.EnemyAmountReduceOne();
			}
			SoundEffects.Inst.enemyDie.PlayRandom();
		}
		else
		{
			Debug.LogError("ObjTypeError!");
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x0002EBF0 File Offset: 0x0002CDF0
	public void unitDieParticle()
	{
		Particle pool_Particle_GetOrNew = ObjectPool.inst.GetPool_Particle_GetOrNew(ResourceLibrary.Inst.Prefab_Particle_UnitBlastTop);
		pool_Particle_GetOrNew.transform.position = base.transform.position;
		pool_Particle_GetOrNew.Blast_Init(this.shapeType, this.mainColor, this.lastScale, false);
		if (!Setting.Inst.Option_EnemyBlastParticle)
		{
			return;
		}
		Particle pool_Particle_GetOrNew2 = ObjectPool.inst.GetPool_Particle_GetOrNew(ResourceLibrary.Inst.Prefab_Particle_UnitBlastBottom);
		pool_Particle_GetOrNew2.transform.position = base.transform.position;
		pool_Particle_GetOrNew2.Blast_Init(this.shapeType, this.mainColor, this.lastScale, false);
		Particle pool_Particle_GetOrNew3 = ObjectPool.inst.GetPool_Particle_GetOrNew(ResourceLibrary.Inst.Prefab_Particle_UnitBlastMiddle);
		pool_Particle_GetOrNew3.transform.position = base.transform.position;
		pool_Particle_GetOrNew3.Blast_Init(this.shapeType, this.mainColor, this.lastScale, false);
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x0002ECD0 File Offset: 0x0002CED0
	public GunObj Shoot_Sequential_ChooseNextEmitGun()
	{
		this.gun_CurrentEmit++;
		if (this.gun_CurrentEmit > this.gun_Objs.Length)
		{
			Debug.LogError("error:gun_CurrentEmit ");
		}
		else if (this.gun_CurrentEmit == this.gun_Objs.Length)
		{
			this.gun_CurrentEmit -= this.gun_Objs.Length;
		}
		return this.gun_Objs[this.gun_CurrentEmit];
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x0002ED3C File Offset: 0x0002CF3C
	public virtual void Awake()
	{
		if (TempData.inst.currentSceneType == EnumSceneType.MAINMENU && this.objType == EnumObjType.ENEMY)
		{
			SpriteRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material = ResourceLibrary.Inst.matNormalUnlit;
			}
		}
		else
		{
			if (this.bloom == null)
			{
				this.bloom = base.gameObject.AddComponent<BloomMaterialControl>();
			}
			this.bloom.InitMat(base.gameObject.GetComponentsInChildren<SpriteRenderer>(), ResourceLibrary.Inst.GlowIntensity_Unit, false);
		}
		this.rb = base.GetComponent<Rigidbody2D>();
		this.SetDrags();
		this.Start_GetSprs();
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x000051D0 File Offset: 0x000033D0
	public void Start()
	{
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x0002EDE4 File Offset: 0x0002CFE4
	private void RecoverInFixedUpdate()
	{
		if (float.IsInfinity(this.fac_CurShootTime) || float.IsNaN(this.fac_CurShootTime))
		{
			this.fac_CurShootTime = 0f;
		}
		if (this.fac_CurShootTime < this.Fac_MaxShootTime)
		{
			float num = Time.fixedUnscaledDeltaTime;
			num = Mathf.Min(num, FPSDetector.inst.curFixedTimeSet * 1.3f);
			if (this.objType == EnumObjType.PLAYER)
			{
				this.fac_CurShootTime += num;
				return;
			}
			if (this.IfObj_Enemy())
			{
				this.fac_CurShootTime += Time.fixedDeltaTime * TimeManager.inst.currentEnemySpeed;
				return;
			}
			this.fac_CurShootTime += Time.fixedDeltaTime;
		}
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x0002EE94 File Offset: 0x0002D094
	private void Start_GetSprs()
	{
		this.list_SprsToDye = new List<SpriteRenderer>();
		foreach (SpriteRenderer item in base.GetComponentsInChildren<SpriteRenderer>())
		{
			this.list_SprsToDye.Add(item);
		}
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x0002EED4 File Offset: 0x0002D0D4
	public void DyeSprsWithColor(Color cl)
	{
		this.lastColorSat = cl.Saturation();
		foreach (SpriteRenderer spriteRenderer in this.list_SprsToDye)
		{
			if (spriteRenderer == null)
			{
				Debug.LogWarning("Warning:Spr==Null");
			}
			else
			{
				spriteRenderer.color = cl.SetAlpha(this.invincible_Alpha);
			}
		}
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x0002EF54 File Offset: 0x0002D154
	public void PushByForce(Vector2 dir, float force)
	{
		Rigidbody2D[] componentsInChildren = base.GetComponentsInChildren<Rigidbody2D>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].simulated = false;
		}
		this.rb.simulated = true;
		Vector2 force2 = dir.normalized * force * this.rb.mass;
		this.rb.MyAddForce(this.FactorTotalNew.bodySize, force2);
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x0002EFC0 File Offset: 0x0002D1C0
	public bool Shoot_WantoShootOnce(Vector2 dir)
	{
		if (this.fac_CurShootTime < this.Fac_MaxShootTime)
		{
			return false;
		}
		this.fac_CurShootTime %= this.Fac_MaxShootTime;
		this.Shoot_ShootOnce(dir, this.prefab_Bullet);
		return true;
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x0002EFF4 File Offset: 0x0002D1F4
	public void Shoot_ShootOnce(Vector2 dir, GameObject prefab)
	{
		if (this.objType == EnumObjType.PLAYER)
		{
			SoundEffects.Inst.UpdatePitch();
		}
		SoundEffects.Inst.shoot.PlayRandom();
		this.Shoot_Recoil(-dir);
		if (this.gun_Parallel)
		{
			foreach (GunObj gun in this.gun_Objs)
			{
				this.Shoot_GeneBulletOnce(gun, prefab);
			}
			return;
		}
		this.Shoot_GeneBulletOnce(this.Shoot_Sequential_ChooseNextEmitGun(), prefab);
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x0002F068 File Offset: 0x0002D268
	public GameObject Shoot_GeneBulletOnce(GunObj gun, GameObject prefab)
	{
		gun.GunShiftOnceOnShoot();
		Vector2 vector = BasicUnit.Shoot_GetDirection(gun.GetDirection(), this.FactorTotalNew.accuracy);
		Vector2 v = gun.posEmit.position;
		if (prefab.GetComponent<Bullet>() != null)
		{
			Bullet bullet = ObjectPool.inst.GetPool_Bullet(prefab.name);
			if (bullet == null)
			{
				bullet = Object.Instantiate<GameObject>(prefab).GetComponent<Bullet>();
			}
			GameObject gameObject = bullet.gameObject;
			gameObject.name = prefab.name;
			gameObject.transform.position = v;
			bullet.BulletInit(this.shapeType, this.mainColor, vector, this);
			return gameObject;
		}
		if (prefab.GetComponent<Enemy>() != null)
		{
			Enemy component = base.gameObject.GetComponent<Enemy>();
			Enemy component2 = Object.Instantiate<GameObject>(prefab).GetComponent<Enemy>();
			component2.transform.position = v;
			component2.Move_Direction = vector;
			component2.summoned = true;
			component2.transform.Rotate(0f, 0f, gun.transform.rotation.eulerAngles.z);
			component2.unit.TransScale = 0.1f;
			component2.unit.PushByForce(vector, GameParameters.Inst.ForceEnemyShootOrigin);
			component2.UpdateFusion(component.fusionRank);
			return component2.gameObject;
		}
		return null;
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x0002F1C0 File Offset: 0x0002D3C0
	public static Vector2 Shoot_GetDirection(Vector2 originDirection, float accuracy)
	{
		float num = MyTool.Vec2toAngle180(originDirection);
		if (accuracy >= 1f)
		{
			return originDirection;
		}
		if (accuracy < -1f)
		{
			accuracy = -1f;
		}
		float num2 = (1f - accuracy) * 45f;
		return MyTool.AngleToVec2(UnityEngine.Random.Range(num - num2, num + num2));
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x0002F20C File Offset: 0x0002D40C
	public virtual void Move_WantoMoveOnce_InFixedUpdate(Vector2 dir, float speed)
	{
		if (Mathf.Abs(dir.x) > 0.5f && Mathf.Abs(dir.y) > 0.5f)
		{
			dir *= 0.707f;
		}
		this.physics_Speed = dir * speed;
		Vector2 vector;
		if (this.objType == EnumObjType.PLAYER)
		{
			float num = Time.fixedUnscaledDeltaTime;
			num = Mathf.Min(num, FPSDetector.inst.curFixedTimeSet * 1.3f);
			vector = this.physics_Speed * num;
		}
		else
		{
			vector = this.physics_Speed * Time.fixedDeltaTime;
		}
		if (!TempData.inst.diffiOptFlag[22] || this.rb == null)
		{
			base.gameObject.transform.Translate(vector, Space.World);
			return;
		}
		this.rb.AddForce(vector * 50f);
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x0002F2E8 File Offset: 0x0002D4E8
	public void Shoot_Recoil(Vector2 dir)
	{
		if (this.objType == EnumObjType.PLAYER && Battle.inst.specialEffect[8] >= 1)
		{
			dir = -dir;
		}
		float basicRecoilForce = GameParameters.Inst.BasicRecoilForce;
		float d = this.Fac_ActualRecoil * basicRecoilForce * this.FactorTotalNew.bulletSize * this.FactorTotalNew.bulletSpd;
		this.rb.MyAddForce(this.FactorTotalNew.bodySize, dir.normalized * d);
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x0002F364 File Offset: 0x0002D564
	private IEnumerator InvincibleTime(int twinkleTimes, float twinkleLastTime, float twinkleIntervalTime)
	{
		if (this.objType != EnumObjType.PLAYER)
		{
			Debug.LogError("这不是玩家！不能无敌！");
			yield return null;
		}
		else if (this.flagInvincible)
		{
			Debug.LogError("已处于无敌阶段");
			yield return null;
		}
		else
		{
			this.flagInvincible = true;
			int num;
			for (int i = 0; i < twinkleTimes; i = num + 1)
			{
				this.invincible_Alpha = 0f;
				this.DyeSprsWithColor(this.Color_Current.SetAlpha(this.invincible_Alpha));
				yield return new WaitForSeconds(twinkleLastTime);
				this.invincible_Alpha = 1f;
				this.DyeSprsWithColor(this.Color_Current.SetAlpha(this.invincible_Alpha));
				yield return new WaitForSeconds(twinkleIntervalTime);
				num = i;
			}
			this.flagInvincible = false;
		}
		yield break;
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x0002F388 File Offset: 0x0002D588
	public void Antibug_MoveRestriction()
	{
		float num = Mathf.Max(0f, 23.1f * SceneObj.inst.SceneSize - this.lastScale * 0.5f);
		Vector2 vector = base.transform.position;
		float num2 = vector.x;
		float num3 = vector.y;
		if (Mathf.Abs(num2) > num || Mathf.Abs(num3) > num)
		{
			num2 = Mathf.Clamp(num2, -num, num);
			num3 = Mathf.Clamp(num3, -num, num);
			base.transform.position = new Vector2(num2, num3);
		}
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x0002F418 File Offset: 0x0002D618
	public void Antibug_MoveRestriction_Strict()
	{
		float num = Mathf.Max(0f, 22f * SceneObj.inst.SceneSize - this.lastScale * 0.5f);
		Vector2 vector = base.transform.position;
		float num2 = vector.x;
		float num3 = vector.y;
		if (Mathf.Abs(num2) > num || Mathf.Abs(num3) > num)
		{
			num2 = Mathf.Clamp(num2, -num, num);
			num3 = Mathf.Clamp(num3, -num, num);
			base.transform.position = new Vector2(num2, num3);
		}
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x0002F4A8 File Offset: 0x0002D6A8
	public void SetDrags()
	{
		if (this.rb == null)
		{
			return;
		}
		this.rb.drag = (TempData.inst.diffiOptFlag[22] ? 0.3f : 5f);
		this.rb.angularDrag = 10f;
	}

	// Token: 0x040006A7 RID: 1703
	public bool inited;

	// Token: 0x040006A8 RID: 1704
	public int nouse = 123;

	// Token: 0x040006A9 RID: 1705
	public double noUseDouble = 123.0;

	// Token: 0x040006AA RID: 1706
	public float noUseFloat = 123f;

	// Token: 0x040006AB RID: 1707
	public ObscuredDouble noUseObDouble = 10.0;

	// Token: 0x040006AC RID: 1708
	[SerializeField]
	private ObscuredDouble lifeData = 10.0;

	// Token: 0x040006AD RID: 1709
	[SerializeField]
	private Collider2D[] collider2Ds = new Collider2D[0];

	// Token: 0x040006AE RID: 1710
	[SerializeField]
	private bool flagInvincible;

	// Token: 0x040006AF RID: 1711
	[SerializeField]
	private float invincible_Alpha = 1f;

	// Token: 0x040006B0 RID: 1712
	private Coroutine coroutineInvincible;

	// Token: 0x040006B1 RID: 1713
	public bool ifDie;

	// Token: 0x040006B2 RID: 1714
	[SerializeField]
	private Factor factor;

	// Token: 0x040006B3 RID: 1715
	public Factor playerFactorTotal = new Factor(null, false);

	// Token: 0x040006B4 RID: 1716
	[Header("ColorSmooth")]
	private float colorSmooth_Ref;

	// Token: 0x040006B5 RID: 1717
	[SerializeField]
	private float lastColorSat;

	// Token: 0x040006B6 RID: 1718
	[Header("ScaleSmooth")]
	private float scaleSmooth_Ref;

	// Token: 0x040006B7 RID: 1719
	[SerializeField]
	private float puffer_SizeMulti = 1f;

	// Token: 0x040006B8 RID: 1720
	[SerializeField]
	public float lastSize = 1f;

	// Token: 0x040006B9 RID: 1721
	[SerializeField]
	public float lastScale = 1f;

	// Token: 0x040006BA RID: 1722
	[Header("Guns")]
	[SerializeField]
	public GunObj[] gun_Objs;

	// Token: 0x040006BB RID: 1723
	[SerializeField]
	public bool gun_Parallel;

	// Token: 0x040006BC RID: 1724
	private int gun_CurrentEmit = -1;

	// Token: 0x040006BD RID: 1725
	[HideInInspector]
	public Vector2 physics_Speed = new Vector2(0f, 0f);

	// Token: 0x040006BE RID: 1726
	[SerializeField]
	private BloomMaterialControl bloom;

	// Token: 0x040006BF RID: 1727
	public float fac_CurShootTime;

	// Token: 0x040006C0 RID: 1728
	public Rigidbody2D rb;

	// Token: 0x040006C1 RID: 1729
	[Header("Sprites")]
	[SerializeField]
	public List<SpriteRenderer> list_SprsToDye = new List<SpriteRenderer>();

	// Token: 0x040006C2 RID: 1730
	[Header("Prefabs")]
	[SerializeField]
	public GameObject prefab_Bullet;
}
