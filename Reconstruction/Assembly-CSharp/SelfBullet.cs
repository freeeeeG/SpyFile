using System;
using UnityEngine;

// Token: 0x02000223 RID: 547
public class SelfBullet : Bullet
{
	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x06000D78 RID: 3448 RVA: 0x00022F39 File Offset: 0x00021139
	public override BulletType BulletType
	{
		get
		{
			return BulletType.Self;
		}
	}

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x06000D79 RID: 3449 RVA: 0x00022F3C File Offset: 0x0002113C
	// (set) Token: 0x06000D7A RID: 3450 RVA: 0x00022F44 File Offset: 0x00021144
	public bool UnFrostEffect { get; set; }

	// Token: 0x06000D7B RID: 3451 RVA: 0x00022F50 File Offset: 0x00021150
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
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x00022FD2 File Offset: 0x000211D2
	public override bool GameUpdate()
	{
		return true;
	}

	// Token: 0x06000D7D RID: 3453 RVA: 0x00022FD5 File Offset: 0x000211D5
	public override void ReclaimBullet()
	{
	}

	// Token: 0x06000D7E RID: 3454 RVA: 0x00022FD8 File Offset: 0x000211D8
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<TargetPoint>())
		{
			base.DamageProcess(collision.GetComponent<TargetPoint>(), true, true);
			return;
		}
		if (this.UnFrostEffect)
		{
			ConcreteContent component = collision.GetComponent<ConcreteContent>();
			if (component == null)
			{
				return;
			}
			if (component == this.turretParent)
			{
				return;
			}
			if (!component.Activated)
			{
				component.UnFrost();
			}
		}
	}
}
