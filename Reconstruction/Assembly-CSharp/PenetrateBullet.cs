using System;
using UnityEngine;

// Token: 0x02000203 RID: 515
public class PenetrateBullet : Bullet
{
	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00021468 File Offset: 0x0001F668
	public override BulletType BulletType
	{
		get
		{
			return BulletType.Penetrate;
		}
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x0002146C File Offset: 0x0001F66C
	public override void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
	{
		base.Initialize(turret, target, pos);
		this.initScale = base.transform.localScale;
		base.transform.localScale *= (1f + base.SplashRange / (base.SplashRange + 8f) * 5f) * (1f + this.turretParent.Strategy.FinalBulletSize);
		base.TriggerPrehit();
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x000214E5 File Offset: 0x0001F6E5
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		base.transform.localScale = this.initScale;
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x000214FE File Offset: 0x0001F6FE
	public override bool GameUpdate()
	{
		return base.DistanceCheck(this.TargetPos);
	}

	// Token: 0x06000CEB RID: 3307 RVA: 0x0002150C File Offset: 0x0001F70C
	public void FixedUpdate()
	{
		base.MoveTowardsRig(this.TargetPos);
	}

	// Token: 0x06000CEC RID: 3308 RVA: 0x0002151A File Offset: 0x0001F71A
	private void OnTriggerEnter2D(Collider2D collision)
	{
		this.tTarget = collision.GetComponent<TargetPoint>();
		if (this.tTarget)
		{
			base.DamageProcess(collision.GetComponent<TargetPoint>(), true, true);
		}
	}

	// Token: 0x04000647 RID: 1607
	private Vector3 initScale;

	// Token: 0x04000648 RID: 1608
	private TargetPoint tTarget;
}
