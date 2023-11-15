using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200021D RID: 541
public class SuperBullet : TargetBullet
{
	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x06000D59 RID: 3417 RVA: 0x00022709 File Offset: 0x00020909
	// (set) Token: 0x06000D5A RID: 3418 RVA: 0x00022711 File Offset: 0x00020911
	public float BounceSplashValue { get; set; }

	// Token: 0x06000D5B RID: 3419 RVA: 0x0002271C File Offset: 0x0002091C
	public void SetAtt(SuperBullet uBullet, TargetPoint target)
	{
		base.Target = target;
		this.TargetPos = target.Position;
		this.turretParent = uBullet.turretParent;
		this.turretEffects = uBullet.turretParent.Strategy.TurretSkills;
		this.turretGlobalSkills = uBullet.turretParent.Strategy.GlobalSkills;
		base.BaseAttack = uBullet.BaseAttack;
		base.AttackIntensify = uBullet.AttackIntensify;
		base.BulletSpeed = uBullet.BulletSpeed;
		base.SplashRange = uBullet.SplashRange;
		base.CriticalRate = uBullet.CriticalRate;
		base.CriticalPercentage = uBullet.CriticalPercentage;
		base.SlowRate = uBullet.SlowRate;
		base.SplashPercentage = uBullet.SplashPercentage;
		base.SlowPercentage = uBullet.SlowPercentage;
		base.BulletDamageIntensify = uBullet.BulletDamageIntensify;
		base.BulletEffectIntensify = uBullet.BulletEffectIntensify;
	}

	// Token: 0x06000D5C RID: 3420 RVA: 0x000227F8 File Offset: 0x000209F8
	public override void TriggerDamage()
	{
		if (this.BonuceTimes > 0)
		{
			this.detectTarget = Physics2D.OverlapCircleAll(base.transform.position, 2.5f, StaticData.EnemyLayerMask);
			this.listedTargets = this.detectTarget.ToList<Collider2D>();
			if (this.listedTargets.Count > 0)
			{
				if (base.Target != null)
				{
					this.listedTargets.Remove(base.Target.GetComponent<Collider2D>());
				}
				if (this.listedTargets.Count > 0)
				{
					TargetPoint component = this.listedTargets[0].GetComponent<TargetPoint>();
					SuperBullet superBullet = Singleton<ObjectPool>.Instance.Spawn(this) as SuperBullet;
					superBullet.transform.position = base.transform.position;
					superBullet.BonuceTimes = this.BonuceTimes - 1;
					superBullet.SetAtt(this, component);
					superBullet.BounceSplashValue = this.BounceSplashValue;
					superBullet.SplashRange += this.BounceSplashValue;
				}
			}
		}
		base.TriggerDamage();
	}

	// Token: 0x06000D5D RID: 3421 RVA: 0x00022902 File Offset: 0x00020B02
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		this.BonuceTimes = -1;
		this.BounceSplashValue = 0f;
	}

	// Token: 0x04000675 RID: 1653
	private Collider2D[] detectTarget;

	// Token: 0x04000676 RID: 1654
	private List<Collider2D> listedTargets;

	// Token: 0x04000677 RID: 1655
	public int BonuceTimes = -1;
}
