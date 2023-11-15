using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class Bullet_Fireball : ASingleTargetProjectile
{
	// Token: 0x0600018F RID: 399 RVA: 0x00006C5A File Offset: 0x00004E5A
	private void LateUpdate()
	{
	}

	// Token: 0x06000190 RID: 400 RVA: 0x00006C5C File Offset: 0x00004E5C
	private void Update()
	{
		if (this.state != AProjectile.eState.STARTED)
		{
			return;
		}
		this.flyTimer += Time.deltaTime;
		if (this.flyTimer < this.totalFlyTime)
		{
			base.transform.forward = base.GetFlyTargetPosition() - base.transform.position;
			base.transform.position = Vector3.Lerp(this.startPosition, base.GetFlyTargetPosition().WithY(0f), this.flyTimer / this.totalFlyTime);
			base.transform.position += this.maxFlightHeight * Vector3.up * Mathf.Sin(this.flyTimer / this.totalFlyTime * 3.1415927f);
			return;
		}
		Singleton<CameraManager>.Instance.ShakeCamera(0.066f, 0.004f, 0f);
		SoundManager.PlaySound("Cannon", "Cannon_Explode_1013", -1f, -1f, -1f);
		foreach (AMonsterBase amonsterBase in Singleton<MonsterManager>.Instance.GetMonstersInRange(base.transform.position.WithY(0f), this.explodeRange))
		{
			amonsterBase.Hit(this.damage, eDamageType.FIRE, default(Vector3));
			base.OnHit(amonsterBase);
		}
		if (this.particle_Explosion != null)
		{
			this.particle_Explosion.Play();
		}
		this.bounceCount++;
		if (this.bounceCount >= this.maxBounceCount)
		{
			this.Despawn();
			return;
		}
		AMonsterBase farthestMonsterInRange = Singleton<MonsterManager>.Instance.GetFarthestMonsterInRange(base.transform.position.WithY(0f), this.bounceMaxRange);
		if (farthestMonsterInRange == null || this.targetMonster == farthestMonsterInRange)
		{
			this.Despawn();
			return;
		}
		this.targetMonster = farthestMonsterInRange;
		this.flyTimer = 0f;
		this.startPosition = base.transform.position;
		this.totalFlyTime = Vector3.Distance(this.targetMonster.transform.position, base.transform.position) / this.speed;
		this.CalculateFlyHeight();
	}

	// Token: 0x06000191 RID: 401 RVA: 0x00006EB8 File Offset: 0x000050B8
	public void Setup(int damage)
	{
		this.damage = damage;
	}

	// Token: 0x06000192 RID: 402 RVA: 0x00006EC1 File Offset: 0x000050C1
	public void OverrideMaxBounceCount(int count)
	{
		this.maxBounceCount = count;
	}

	// Token: 0x06000193 RID: 403 RVA: 0x00006ECA File Offset: 0x000050CA
	public void OverrideMaxFlightHeight(float height)
	{
		this.maxFlightHeight = height;
	}

	// Token: 0x06000194 RID: 404 RVA: 0x00006ED4 File Offset: 0x000050D4
	protected override void SpawnProc()
	{
		this.startPosition = base.transform.position;
		this.CalculateFlyHeight();
		this.totalFlyTime = Vector3.Distance(this.targetMonster.transform.position, base.transform.position) / this.speed;
		this.flyTimer = 0f;
		this.bounceCount = 0;
	}

	// Token: 0x06000195 RID: 405 RVA: 0x00006F38 File Offset: 0x00005138
	private void CalculateFlyHeight()
	{
		if (Vector3.Distance(base.transform.position, this.targetMonster.transform.position) < this.decreaseFlightHeightRange)
		{
			this.flyHeight = this.flyHeight.Map(0f, this.decreaseFlightHeightRange, this.maxFlightHeight * 0.33f, this.maxFlightHeight);
			return;
		}
		this.flyHeight = this.maxFlightHeight;
	}

	// Token: 0x06000196 RID: 406 RVA: 0x00006FA8 File Offset: 0x000051A8
	protected override void DespawnProc()
	{
	}

	// Token: 0x06000197 RID: 407 RVA: 0x00006FAA File Offset: 0x000051AA
	protected override void DestroyProc()
	{
	}

	// Token: 0x0400011B RID: 283
	[SerializeField]
	private float speed = 1f;

	// Token: 0x0400011C RID: 284
	[SerializeField]
	private int maxBounceCount = 10;

	// Token: 0x0400011D RID: 285
	[SerializeField]
	private float bounceMaxRange = 10f;

	// Token: 0x0400011E RID: 286
	[SerializeField]
	private float maxFlightHeight = 10f;

	// Token: 0x0400011F RID: 287
	[SerializeField]
	private float decreaseFlightHeightRange = 5f;

	// Token: 0x04000120 RID: 288
	[SerializeField]
	private float explodeRange = 3f;

	// Token: 0x04000121 RID: 289
	private float totalFlyTime;

	// Token: 0x04000122 RID: 290
	private float flyTimer;

	// Token: 0x04000123 RID: 291
	private Vector3 startPosition;

	// Token: 0x04000124 RID: 292
	private int damage;

	// Token: 0x04000125 RID: 293
	private int bounceCount;

	// Token: 0x04000126 RID: 294
	private float flyHeight;
}
