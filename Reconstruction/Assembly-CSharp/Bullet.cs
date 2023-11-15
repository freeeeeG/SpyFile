using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public abstract class Bullet : ReusableObject, IGameBehavior
{
	// Token: 0x17000228 RID: 552
	// (get) Token: 0x0600047E RID: 1150
	public abstract BulletType BulletType { get; }

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x0600047F RID: 1151 RVA: 0x0000C12D File Offset: 0x0000A32D
	// (set) Token: 0x06000480 RID: 1152 RVA: 0x0000C135 File Offset: 0x0000A335
	public TargetPoint Target
	{
		get
		{
			return this.target;
		}
		set
		{
			this.target = value;
		}
	}

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06000481 RID: 1153 RVA: 0x0000C13E File Offset: 0x0000A33E
	// (set) Token: 0x06000482 RID: 1154 RVA: 0x0000C146 File Offset: 0x0000A346
	protected virtual Vector2 TargetPos
	{
		get
		{
			return this.targetPos;
		}
		set
		{
			this.targetPos = value;
		}
	}

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06000483 RID: 1155 RVA: 0x0000C14F File Offset: 0x0000A34F
	// (set) Token: 0x06000484 RID: 1156 RVA: 0x0000C15D File Offset: 0x0000A35D
	public float BulletEffectIntensify
	{
		get
		{
			return 1f + this.bulletEffectIntensify;
		}
		set
		{
			this.bulletEffectIntensify = value;
		}
	}

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x06000485 RID: 1157 RVA: 0x0000C166 File Offset: 0x0000A366
	// (set) Token: 0x06000486 RID: 1158 RVA: 0x0000C16E File Offset: 0x0000A36E
	public float BulletSpeed
	{
		get
		{
			return this.bulletSpeed;
		}
		set
		{
			this.bulletSpeed = value;
		}
	}

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x06000487 RID: 1159 RVA: 0x0000C177 File Offset: 0x0000A377
	// (set) Token: 0x06000488 RID: 1160 RVA: 0x0000C17F File Offset: 0x0000A37F
	public float BaseAttack
	{
		get
		{
			return this.baseAttack;
		}
		set
		{
			this.baseAttack = value;
		}
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06000489 RID: 1161 RVA: 0x0000C188 File Offset: 0x0000A388
	// (set) Token: 0x0600048A RID: 1162 RVA: 0x0000C190 File Offset: 0x0000A390
	public float AttackIntensify
	{
		get
		{
			return this.attackIntensify;
		}
		set
		{
			this.attackIntensify = value;
		}
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x0600048B RID: 1163 RVA: 0x0000C199 File Offset: 0x0000A399
	// (set) Token: 0x0600048C RID: 1164 RVA: 0x0000C1A1 File Offset: 0x0000A3A1
	public float SplashRange
	{
		get
		{
			return this.sputteringRange;
		}
		set
		{
			this.sputteringRange = value;
		}
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x0600048D RID: 1165 RVA: 0x0000C1AA File Offset: 0x0000A3AA
	// (set) Token: 0x0600048E RID: 1166 RVA: 0x0000C1B2 File Offset: 0x0000A3B2
	public float SplashPercentage
	{
		get
		{
			return this.sputteringPercentage;
		}
		set
		{
			this.sputteringPercentage = value;
		}
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x0600048F RID: 1167 RVA: 0x0000C1BB File Offset: 0x0000A3BB
	public float FinalSplashPercentage
	{
		get
		{
			return this.SplashPercentage + this.SplashRange * 0.2f;
		}
	}

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x06000490 RID: 1168 RVA: 0x0000C1D0 File Offset: 0x0000A3D0
	// (set) Token: 0x06000491 RID: 1169 RVA: 0x0000C1D8 File Offset: 0x0000A3D8
	public float SlowPercentage
	{
		get
		{
			return this.slowPercentage;
		}
		set
		{
			this.slowPercentage = value;
		}
	}

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06000492 RID: 1170 RVA: 0x0000C1E1 File Offset: 0x0000A3E1
	public float FinalSlowPercentage
	{
		get
		{
			return this.SlowPercentage;
		}
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06000493 RID: 1171 RVA: 0x0000C1E9 File Offset: 0x0000A3E9
	// (set) Token: 0x06000494 RID: 1172 RVA: 0x0000C1F1 File Offset: 0x0000A3F1
	public float CriticalRate
	{
		get
		{
			return this.criticalRate;
		}
		set
		{
			this.criticalRate = value;
		}
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06000495 RID: 1173 RVA: 0x0000C1FA File Offset: 0x0000A3FA
	// (set) Token: 0x06000496 RID: 1174 RVA: 0x0000C202 File Offset: 0x0000A402
	public float CriticalPercentage
	{
		get
		{
			return this.criticalPercentage;
		}
		set
		{
			this.criticalPercentage = value;
		}
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06000497 RID: 1175 RVA: 0x0000C20B File Offset: 0x0000A40B
	// (set) Token: 0x06000498 RID: 1176 RVA: 0x0000C213 File Offset: 0x0000A413
	public float SlowRate
	{
		get
		{
			return this.slowRate;
		}
		set
		{
			this.slowRate = value;
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06000499 RID: 1177 RVA: 0x0000C21C File Offset: 0x0000A41C
	// (set) Token: 0x0600049A RID: 1178 RVA: 0x0000C224 File Offset: 0x0000A424
	public float BulletDamageIntensify
	{
		get
		{
			return this.bulletDamageIntensify;
		}
		set
		{
			this.bulletDamageIntensify = value;
		}
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x0600049B RID: 1179 RVA: 0x0000C22D File Offset: 0x0000A42D
	public float FinalDamage
	{
		get
		{
			return Mathf.Max(0f, this.BaseAttack * (1f + this.AttackIntensify) * (1f + Mathf.Max(-1f, this.BulletDamageIntensify))) * this.DamageAdjust;
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x0600049C RID: 1180 RVA: 0x0000C26A File Offset: 0x0000A46A
	// (set) Token: 0x0600049D RID: 1181 RVA: 0x0000C272 File Offset: 0x0000A472
	public float DamageAdjust
	{
		get
		{
			return this.damageAdjust;
		}
		set
		{
			this.damageAdjust = value;
		}
	}

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x0600049E RID: 1182 RVA: 0x0000C27B File Offset: 0x0000A47B
	// (set) Token: 0x0600049F RID: 1183 RVA: 0x0000C283 File Offset: 0x0000A483
	public ParticalControl SputteringEffect
	{
		get
		{
			return this.sputteringEffect;
		}
		set
		{
			this.sputteringEffect = value;
		}
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x0000C28C File Offset: 0x0000A48C
	public override void OnSpawn()
	{
		base.OnSpawn();
		Singleton<GameManager>.Instance.nonEnemies.Add(this);
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x0000C2A4 File Offset: 0x0000A4A4
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		this.isCritical = false;
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x0000C2B4 File Offset: 0x0000A4B4
	public virtual void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
	{
		this.Target = target;
		this.TargetPos = (pos ?? target.Position);
		this.turretParent = turret;
		this.turretEffects = turret.Strategy.TurretSkills;
		this.turretGlobalSkills = turret.Strategy.GlobalSkills;
		this.TriggerShootEffect(target ? target.Enemy : null);
		this.SetAttribute(turret);
		this.TriggerAfterShoot(target ? target.Enemy : null);
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x0000C348 File Offset: 0x0000A548
	protected void SetAttribute(TurretContent turret)
	{
		this.BaseAttack = turret.Strategy.BaseAttack;
		this.AttackIntensify = turret.Strategy.TotalAttackIntensify;
		this.BulletSpeed = turret.Strategy.Attribute.BulletSpeed;
		this.SplashRange = turret.Strategy.FinalSplashRange;
		this.CriticalRate = turret.Strategy.FinalCriticalRate;
		this.CriticalPercentage = turret.Strategy.FinalCriticalPercentage;
		this.SlowRate = turret.Strategy.FinalSlowRate;
		this.SplashPercentage = turret.Strategy.FinalSplashPercentage;
		this.SlowPercentage = turret.Strategy.FianlSlowPercentageOfSplash;
		this.BulletDamageIntensify = turret.Strategy.FinalBulletDamageIntensify;
		this.BulletEffectIntensify = turret.Strategy.FinalBulletEffectIntensify;
		this.DamageAdjust = 1f;
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x0000C420 File Offset: 0x0000A620
	protected void TriggerShootEffect(IDamage target)
	{
		foreach (TurretSkill turretSkill in this.turretEffects)
		{
			turretSkill.Shoot(target, this);
		}
		foreach (GlobalSkill globalSkill in this.turretGlobalSkills)
		{
			globalSkill.Shoot(target, this);
		}
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x0000C4B4 File Offset: 0x0000A6B4
	public void TriggerAfterShoot(IDamage target)
	{
		this.isCritical = (Random.value <= this.CriticalRate);
		foreach (TurretSkill turretSkill in this.turretEffects)
		{
			turretSkill.AfterShoot(this, target);
		}
		foreach (GlobalSkill globalSkill in this.turretGlobalSkills)
		{
			globalSkill.AfterShoot(this, target);
		}
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x0000C560 File Offset: 0x0000A760
	protected float TriggerHitEffect(IDamage target)
	{
		float num = this.FinalDamage;
		foreach (TurretSkill turretSkill in this.turretEffects)
		{
			num = turretSkill.Hit(num, target, this);
		}
		foreach (GlobalSkill globalSkill in this.turretGlobalSkills)
		{
			num = globalSkill.Hit(num, target, this);
		}
		return num;
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x0000C600 File Offset: 0x0000A800
	protected void TriggerSplashEffect(ConcreteContent content)
	{
		foreach (TurretSkill turretSkill in this.turretEffects)
		{
			turretSkill.Splash(content, this);
		}
		foreach (GlobalSkill globalSkill in this.turretGlobalSkills)
		{
			globalSkill.Splash(content, this);
		}
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x0000C694 File Offset: 0x0000A894
	protected void TriggerPrehit()
	{
		foreach (TurretSkill turretSkill in this.turretEffects)
		{
			turretSkill.PreHit(this);
		}
		foreach (GlobalSkill globalSkill in this.turretGlobalSkills)
		{
			globalSkill.PreHit(this);
		}
	}

	// Token: 0x060004A9 RID: 1193
	public abstract bool GameUpdate();

	// Token: 0x060004AA RID: 1194 RVA: 0x0000C728 File Offset: 0x0000A928
	protected bool DistanceCheck(Vector2 pos)
	{
		if ((base.transform.position - pos).magnitude < this.minDistanceToDealDamage)
		{
			this.TriggerDamage();
			return false;
		}
		return true;
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x0000C764 File Offset: 0x0000A964
	protected void RotateBullet(Vector2 pos)
	{
		Vector2 v = pos - base.transform.position;
		float zAngle = Vector3.SignedAngle(base.transform.up, v, base.transform.forward);
		base.transform.Rotate(0f, 0f, zAngle);
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x0000C7C0 File Offset: 0x0000A9C0
	protected void MoveTowards(Vector2 pos)
	{
		base.transform.position = Vector2.MoveTowards(base.transform.position, pos, this.BulletSpeed * Time.deltaTime);
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x0000C7F4 File Offset: 0x0000A9F4
	protected void MoveTowardsRig(Vector2 pos)
	{
		this.m_Rig.MovePosition(this.m_Rig.position + (pos - this.m_Rig.position).normalized * this.bulletSpeed * Time.fixedDeltaTime);
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x0000C84C File Offset: 0x0000AA4C
	public float GetTargetDistance()
	{
		return (this.turretParent.transform.position - this.TargetPos).magnitude;
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x0000C881 File Offset: 0x0000AA81
	public virtual void ReclaimBullet()
	{
		this.BulletEffectIntensify = 0f;
		this.BulletDamageIntensify = 0f;
		Singleton<ObjectPool>.Instance.UnSpawn(this);
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x0000C8A4 File Offset: 0x0000AAA4
	public virtual void TriggerDamage()
	{
		this.ReclaimBullet();
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x0000C8AC File Offset: 0x0000AAAC
	public virtual void DealRealDamage(float damage, IDamage target, Vector2 pos, bool showDamage = true, bool isSputtering = false)
	{
		this.slowTemp = (this.isCritical ? (this.SlowRate * this.CriticalPercentage) : this.SlowRate);
		target.DamageStrategy.ApplyFrost(isSputtering ? (this.FinalSlowPercentage * this.slowTemp) : this.slowTemp);
		this.finalDamage = (this.isCritical ? (damage * this.CriticalPercentage) : damage);
		if (isSputtering)
		{
			this.finalDamage *= this.FinalSplashPercentage;
		}
		target.DamageStrategy.ApplyDamage(this.finalDamage, out this.realDamage, this, true);
		this.addDamage = (int)this.realDamage;
		this.turretParent.Strategy.TotalDamage += (long)this.addDamage;
		this.turretParent.Strategy.TurnDamage += (long)this.addDamage;
		GameRes.TotalDamage += (long)this.addDamage;
		if (showDamage)
		{
			Singleton<StaticData>.Instance.ShowJumpDamage(pos, (long)this.addDamage, this.isCritical);
		}
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x0000C9C0 File Offset: 0x0000ABC0
	public void DamageProcess(TargetPoint target, bool showDamage = true, bool isSputtering = false)
	{
		float damage = this.TriggerHitEffect(target.Enemy);
		this.DealRealDamage(damage, target.Enemy, target.Position, showDamage, isSputtering);
	}

	// Token: 0x040001B2 RID: 434
	[SerializeField]
	private ParticalControl sputteringEffect;

	// Token: 0x040001B3 RID: 435
	[SerializeField]
	private Rigidbody2D m_Rig;

	// Token: 0x040001B4 RID: 436
	private TargetPoint target;

	// Token: 0x040001B5 RID: 437
	[HideInInspector]
	public TurretContent turretParent;

	// Token: 0x040001B6 RID: 438
	protected List<TurretSkill> turretEffects;

	// Token: 0x040001B7 RID: 439
	protected List<GlobalSkill> turretGlobalSkills;

	// Token: 0x040001B8 RID: 440
	private Vector2 targetPos;

	// Token: 0x040001B9 RID: 441
	protected readonly float minDistanceToDealDamage = 0.2f;

	// Token: 0x040001BA RID: 442
	private float bulletEffectIntensify;

	// Token: 0x040001BB RID: 443
	private float bulletSpeed;

	// Token: 0x040001BC RID: 444
	private float baseAttack;

	// Token: 0x040001BD RID: 445
	private float attackIntensify;

	// Token: 0x040001BE RID: 446
	private float sputteringRange;

	// Token: 0x040001BF RID: 447
	private float sputteringPercentage;

	// Token: 0x040001C0 RID: 448
	private float slowPercentage;

	// Token: 0x040001C1 RID: 449
	private float criticalRate;

	// Token: 0x040001C2 RID: 450
	private float criticalPercentage;

	// Token: 0x040001C3 RID: 451
	private float slowRate;

	// Token: 0x040001C4 RID: 452
	private float bulletDamageIntensify;

	// Token: 0x040001C5 RID: 453
	private float damageAdjust = 1f;

	// Token: 0x040001C6 RID: 454
	[HideInInspector]
	public bool isCritical;

	// Token: 0x040001C7 RID: 455
	[HideInInspector]
	public int HitSize;

	// Token: 0x040001C8 RID: 456
	private float slowTemp;

	// Token: 0x040001C9 RID: 457
	private float finalDamage;

	// Token: 0x040001CA RID: 458
	private float realDamage;

	// Token: 0x040001CB RID: 459
	protected int addDamage;
}
