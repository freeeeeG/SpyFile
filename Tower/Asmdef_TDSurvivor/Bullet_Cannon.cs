using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000049 RID: 73
public class Bullet_Cannon : ASingleTargetProjectile
{
	// Token: 0x06000179 RID: 377 RVA: 0x000066CD File Offset: 0x000048CD
	private void LateUpdate()
	{
	}

	// Token: 0x0600017A RID: 378 RVA: 0x000066D0 File Offset: 0x000048D0
	private void Update()
	{
		if (this.state != AProjectile.eState.STARTED)
		{
			return;
		}
		this.flyTimer += Time.deltaTime;
		if (this.flyTimer < this.totalFlyTime)
		{
			base.transform.position = Vector3.Lerp(this.startPosition, base.GetFlyTargetPosition(), this.flyTimer / this.totalFlyTime);
			base.transform.position += this.maxFlightHeight * Vector3.up * Mathf.Sin(this.flyTimer / this.totalFlyTime * 3.1415927f);
			return;
		}
		foreach (AMonsterBase amonsterBase in Singleton<MonsterManager>.Instance.GetMonstersInRange(base.transform.position.WithY(0f), this.explodeRange))
		{
			amonsterBase.Hit(this.damage, eDamageType.FIRE, default(Vector3));
			base.OnHit(amonsterBase);
		}
		SoundManager.PlaySound("Cannon", "Cannon_Explode_1005", -1f, -1f, -1f);
		this.Despawn();
	}

	// Token: 0x0600017B RID: 379 RVA: 0x00006814 File Offset: 0x00004A14
	public void Setup(int damage)
	{
		this.damage = damage;
	}

	// Token: 0x0600017C RID: 380 RVA: 0x00006820 File Offset: 0x00004A20
	protected override void SpawnProc()
	{
		this.startPosition = base.transform.position;
		if (Vector3.Distance(base.transform.position, this.targetMonster.transform.position) < this.decreaseFlightHeightRange)
		{
			this.flyHeight = this.flyHeight.Map(0f, this.decreaseFlightHeightRange, this.maxFlightHeight * 0.33f, this.maxFlightHeight);
		}
		else
		{
			this.flyHeight = this.maxFlightHeight;
		}
		this.totalFlyTime = Vector3.Distance(this.targetMonster.HeadWorldPosition, base.transform.position) / this.speed;
		this.flyTimer = 0f;
	}

	// Token: 0x0600017D RID: 381 RVA: 0x000068D5 File Offset: 0x00004AD5
	protected override void DespawnProc()
	{
	}

	// Token: 0x0600017E RID: 382 RVA: 0x000068D7 File Offset: 0x00004AD7
	protected override void DestroyProc()
	{
	}

	// Token: 0x04000101 RID: 257
	[SerializeField]
	private float speed = 1f;

	// Token: 0x04000102 RID: 258
	[SerializeField]
	private Rigidbody rigidbody;

	// Token: 0x04000103 RID: 259
	[SerializeField]
	[FormerlySerializedAs("flightHeight")]
	private float maxFlightHeight = 10f;

	// Token: 0x04000104 RID: 260
	[SerializeField]
	private float decreaseFlightHeightRange = 5f;

	// Token: 0x04000105 RID: 261
	[SerializeField]
	private float explodeRange = 3f;

	// Token: 0x04000106 RID: 262
	private float totalFlyTime;

	// Token: 0x04000107 RID: 263
	private float flyTimer;

	// Token: 0x04000108 RID: 264
	private Vector3 startPosition;

	// Token: 0x04000109 RID: 265
	private int damage;

	// Token: 0x0400010A RID: 266
	private float flyHeight;
}
