using System;
using UnityEngine;

// Token: 0x0200022E RID: 558
public class TargetBullet : Bullet
{
	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x06000E53 RID: 3667 RVA: 0x000248EF File Offset: 0x00022AEF
	public override BulletType BulletType
	{
		get
		{
			return BulletType.Target;
		}
	}

	// Token: 0x17000526 RID: 1318
	// (get) Token: 0x06000E54 RID: 3668 RVA: 0x000248F2 File Offset: 0x00022AF2
	// (set) Token: 0x06000E55 RID: 3669 RVA: 0x00024914 File Offset: 0x00022B14
	protected override Vector2 TargetPos
	{
		get
		{
			if (!(base.Target == null))
			{
				return base.Target.Position;
			}
			return base.TargetPos;
		}
		set
		{
			base.TargetPos = value;
		}
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x00024920 File Offset: 0x00022B20
	public override bool GameUpdate()
	{
		if (base.Target != null && !base.Target.gameObject.activeSelf)
		{
			this.TargetPos = base.Target.transform.position;
			base.Target = null;
		}
		base.RotateBullet(this.TargetPos);
		base.MoveTowards(this.TargetPos);
		return base.DistanceCheck(this.TargetPos);
	}

	// Token: 0x06000E57 RID: 3671 RVA: 0x00024994 File Offset: 0x00022B94
	public override void TriggerDamage()
	{
		if (base.SplashRange > 0f)
		{
			StaticData.FillBuffer(base.transform.position, base.SplashRange, StaticData.EnemyLayerMask);
			this.HitSize = StaticData.BufferCount;
		}
		base.TriggerPrehit();
		if (base.SplashRange > 0f)
		{
			for (int i = 0; i < this.HitSize; i++)
			{
				this.cTarget = StaticData.GetBuffer(i).GetComponent<TargetPoint>();
				if (this.cTarget == base.Target)
				{
					base.DamageProcess(this.cTarget, true, false);
				}
				else
				{
					base.DamageProcess(this.cTarget, i < 16, true);
				}
			}
		}
		else if (base.Target != null)
		{
			base.DamageProcess(base.Target, true, false);
		}
		this.effect = (Singleton<ObjectPool>.Instance.Spawn(base.SputteringEffect) as ParticalControl);
		this.effect.transform.position = base.transform.position;
		this.effect.transform.localScale = Mathf.Max(0.3f, base.SplashRange * 2f) * Vector3.one;
		this.effect.PlayEffect();
		base.TriggerDamage();
	}

	// Token: 0x040006D5 RID: 1749
	private TargetPoint cTarget;

	// Token: 0x040006D6 RID: 1750
	private ParticalControl effect;
}
