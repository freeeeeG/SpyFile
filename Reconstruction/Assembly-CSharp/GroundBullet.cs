using System;
using UnityEngine;

// Token: 0x02000200 RID: 512
public class GroundBullet : Bullet
{
	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00021172 File Offset: 0x0001F372
	public override BulletType BulletType
	{
		get
		{
			return BulletType.Ground;
		}
	}

	// Token: 0x06000CD9 RID: 3289 RVA: 0x00021175 File Offset: 0x0001F375
	public override bool GameUpdate()
	{
		base.RotateBullet(this.TargetPos);
		base.MoveTowards(this.TargetPos);
		return base.DistanceCheck(this.TargetPos);
	}

	// Token: 0x06000CDA RID: 3290 RVA: 0x0002119C File Offset: 0x0001F39C
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
				base.DamageProcess(StaticData.GetBuffer(i).GetComponent<TargetPoint>(), i < 16, true);
			}
		}
		this.effect = (Singleton<ObjectPool>.Instance.Spawn(base.SputteringEffect) as ParticalControl);
		this.effect.transform.position = base.transform.position;
		this.effect.transform.localScale = Mathf.Max(0.3f, base.SplashRange * 2f) * Vector3.one;
		this.effect.PlayEffect();
		base.TriggerDamage();
	}

	// Token: 0x04000642 RID: 1602
	private Collider2D[] hits = new Collider2D[20];

	// Token: 0x04000643 RID: 1603
	private ParticalControl effect;
}
