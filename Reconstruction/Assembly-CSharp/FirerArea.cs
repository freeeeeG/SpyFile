using System;
using UnityEngine;

// Token: 0x0200020E RID: 526
public class FirerArea : Bullet
{
	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x06000D1A RID: 3354 RVA: 0x00021B50 File Offset: 0x0001FD50
	public override BulletType BulletType
	{
		get
		{
			return BulletType.Self;
		}
	}

	// Token: 0x06000D1B RID: 3355 RVA: 0x00021B54 File Offset: 0x0001FD54
	public override void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
	{
		base.Target = target;
		this.TargetPos = (pos ?? target.Position);
		this.turretParent = turret;
		this.turretEffects = turret.Strategy.TurretSkills;
		this.turretGlobalSkills = turret.Strategy.GlobalSkills;
		base.TriggerShootEffect(target.Enemy);
		base.SetAttribute(turret);
		base.TriggerAfterShoot(target.Enemy);
		base.TriggerPrehit();
		this.TriggerHit();
	}

	// Token: 0x06000D1C RID: 3356 RVA: 0x00021BDC File Offset: 0x0001FDDC
	public override bool GameUpdate()
	{
		return true;
	}

	// Token: 0x06000D1D RID: 3357 RVA: 0x00021BDF File Offset: 0x0001FDDF
	public override void ReclaimBullet()
	{
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x00021BE4 File Offset: 0x0001FDE4
	private void TriggerHit()
	{
		StaticData.FillBuffer(base.transform.position, 0.75f, StaticData.EnemyLayerMask);
		this.HitSize = StaticData.BufferCount;
		for (int i = 0; i < this.HitSize; i++)
		{
			base.DamageProcess(StaticData.GetBuffer(i).GetComponent<TargetPoint>(), i < 16, true);
		}
	}

	// Token: 0x0400065B RID: 1627
	[SerializeField]
	private ParticleSystem fireEffect;
}
