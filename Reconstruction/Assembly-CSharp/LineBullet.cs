using System;
using UnityEngine;

// Token: 0x02000201 RID: 513
public class LineBullet : Bullet
{
	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x06000CDC RID: 3292 RVA: 0x000212AE File Offset: 0x0001F4AE
	public override BulletType BulletType
	{
		get
		{
			return BulletType.Penetrate;
		}
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x000212B1 File Offset: 0x0001F4B1
	public override void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
	{
		base.Initialize(turret, target, pos);
		this.hitInterval = 6f / base.BulletSpeed * 0.05f;
		base.TriggerPrehit();
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x000212DA File Offset: 0x0001F4DA
	public override bool GameUpdate()
	{
		this.DealDamageForward();
		return base.DistanceCheck(this.TargetPos);
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x000212EE File Offset: 0x0001F4EE
	public void FixedUpdate()
	{
		base.MoveTowardsRig(this.TargetPos);
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x000212FC File Offset: 0x0001F4FC
	private void DealDamageForward()
	{
		this.hitTimeCounter += Time.deltaTime;
		if (this.hitTimeCounter > this.hitInterval)
		{
			int num = Physics2D.RaycastNonAlloc(base.transform.position, base.transform.forward, this.hitTargets, 0.1f, StaticData.EnemyLayerMask);
			for (int i = 0; i < num; i++)
			{
				TargetPoint component = this.hitTargets[i].transform.GetComponent<TargetPoint>();
				base.DamageProcess(component, true, true);
			}
			this.hitTimeCounter = 0f;
		}
	}

	// Token: 0x04000644 RID: 1604
	private RaycastHit2D[] hitTargets = new RaycastHit2D[10];

	// Token: 0x04000645 RID: 1605
	protected float hitInterval = 0.05f;

	// Token: 0x04000646 RID: 1606
	private float hitTimeCounter;
}
