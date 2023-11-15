using System;
using UnityEngine;

// Token: 0x02000220 RID: 544
public class UltraLava : Bullet
{
	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x06000D64 RID: 3428 RVA: 0x000229A7 File Offset: 0x00020BA7
	// (set) Token: 0x06000D65 RID: 3429 RVA: 0x000229AF File Offset: 0x00020BAF
	public float SplashRangeIncreasePerSecond { get; set; }

	// Token: 0x170004AB RID: 1195
	// (get) Token: 0x06000D66 RID: 3430 RVA: 0x000229B8 File Offset: 0x00020BB8
	public override BulletType BulletType
	{
		get
		{
			return BulletType.Self;
		}
	}

	// Token: 0x170004AC RID: 1196
	// (get) Token: 0x06000D67 RID: 3431 RVA: 0x000229BB File Offset: 0x00020BBB
	// (set) Token: 0x06000D68 RID: 3432 RVA: 0x000229C3 File Offset: 0x00020BC3
	public float BulletLastTime
	{
		get
		{
			return this.bulletLastTime;
		}
		set
		{
			this.bulletLastTime = value;
		}
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x000229CC File Offset: 0x00020BCC
	public void SetAtt(UltraBullet uBullet)
	{
		base.Target = uBullet.Target;
		this.TargetPos = uBullet.Target.Position;
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
		base.transform.localScale = Vector3.one * base.SplashRange * 2f;
		this.timeCounter = 0f;
		this.triggerCounter = 0f;
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x00022AE4 File Offset: 0x00020CE4
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
	}

	// Token: 0x06000D6B RID: 3435 RVA: 0x00022B60 File Offset: 0x00020D60
	public override bool GameUpdate()
	{
		this.timeCounter += Time.deltaTime;
		this.triggerCounter += Time.deltaTime;
		base.SplashRange += Time.deltaTime * this.SplashRangeIncreasePerSecond;
		base.transform.localScale = Vector3.one * base.SplashRange * 2f;
		if (this.triggerCounter > this.triggerInterval)
		{
			this.TriggerDamage();
			this.triggerCounter = 0f;
		}
		if (this.timeCounter > this.BulletLastTime)
		{
			Singleton<ObjectPool>.Instance.UnSpawn(this);
			return false;
		}
		return true;
	}

	// Token: 0x0400067B RID: 1659
	private float bulletLastTime = 5f;

	// Token: 0x0400067C RID: 1660
	private float triggerInterval = 1f;

	// Token: 0x0400067E RID: 1662
	private float timeCounter;

	// Token: 0x0400067F RID: 1663
	private float triggerCounter;
}
