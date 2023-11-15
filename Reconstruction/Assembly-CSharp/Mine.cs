using System;
using UnityEngine;

// Token: 0x02000211 RID: 529
public class Mine : Bullet
{
	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x06000D27 RID: 3367 RVA: 0x00021E3A File Offset: 0x0002003A
	public override BulletType BulletType
	{
		get
		{
			return BulletType.Self;
		}
	}

	// Token: 0x06000D28 RID: 3368 RVA: 0x00021E40 File Offset: 0x00020040
	public void SetAtt(MinerBullet uBullet)
	{
		this.turretParent = uBullet.turretParent;
		this.turretEffects = uBullet.turretParent.Strategy.TurretSkills;
		this.turretGlobalSkills = uBullet.turretParent.Strategy.GlobalSkills;
		base.BaseAttack = uBullet.BaseAttack;
		base.SplashRange = uBullet.SplashRange;
		base.CriticalRate = uBullet.CriticalRate;
		base.CriticalPercentage = uBullet.CriticalPercentage;
		base.SlowRate = uBullet.SlowRate;
		base.SplashPercentage = uBullet.SplashPercentage;
		base.SlowPercentage = uBullet.SlowPercentage;
		base.BulletDamageIntensify = uBullet.BulletDamageIntensify;
		base.BulletEffectIntensify = uBullet.BulletEffectIntensify;
		base.DamageAdjust = uBullet.DamageAdjust;
		this.mineTile = uBullet.TargetTile;
	}

	// Token: 0x06000D29 RID: 3369 RVA: 0x00021F0C File Offset: 0x0002010C
	public override void TriggerDamage()
	{
		base.TriggerPrehit();
		if (base.SplashRange > 0f)
		{
			StaticData.FillBuffer(base.transform.position, base.SplashRange, StaticData.EnemyLayerMask);
			this.HitSize = StaticData.BufferCount;
			for (int i = 0; i < this.HitSize; i++)
			{
				base.DamageProcess(StaticData.GetBuffer(i).GetComponent<TargetPoint>(), i < 9, true);
			}
		}
		this.effect = (Singleton<ObjectPool>.Instance.Spawn(base.SputteringEffect) as ParticalControl);
		this.effect.transform.position = base.transform.position;
		this.effect.transform.localScale = Mathf.Max(0.3f, base.SplashRange * 2f) * Vector3.one;
		this.effect.PlayEffect();
		Singleton<ObjectPool>.Instance.UnSpawn(this);
	}

	// Token: 0x06000D2A RID: 3370 RVA: 0x00022001 File Offset: 0x00020201
	public override bool GameUpdate()
	{
		this.DmgIncrease();
		return true;
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x0002200C File Offset: 0x0002020C
	private void DmgIncrease()
	{
		if (this.dmgIncreased > this.maxDmgIncreased)
		{
			return;
		}
		this.timeCounter += Time.deltaTime;
		if (this.timeCounter > this.dmgIncreaseInterval)
		{
			base.AttackIntensify += this.attackIncreasePerSecond;
			this.dmgIncreased += this.attackIncreasePerSecond;
			this.timeCounter = 0f;
		}
	}

	// Token: 0x06000D2C RID: 3372 RVA: 0x00022079 File Offset: 0x00020279
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<TargetPoint>())
		{
			this.TriggerDamage();
		}
	}

	// Token: 0x06000D2D RID: 3373 RVA: 0x0002208E File Offset: 0x0002028E
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		this.dmgIncreased = 0f;
		this.mineTile.hasMine = false;
	}

	// Token: 0x0400065F RID: 1631
	private BasicTile mineTile;

	// Token: 0x04000660 RID: 1632
	private ParticalControl effect;

	// Token: 0x04000661 RID: 1633
	private float timeCounter;

	// Token: 0x04000662 RID: 1634
	private float dmgIncreaseInterval = 1f;

	// Token: 0x04000663 RID: 1635
	private float dmgIncreased;

	// Token: 0x04000664 RID: 1636
	public float maxDmgIncreased;

	// Token: 0x04000665 RID: 1637
	public float attackIncreasePerSecond;
}
