using System;
using UnityEngine;

// Token: 0x0200004B RID: 75
public class Bullet_EmberFire : ASingleTargetProjectile
{
	// Token: 0x06000188 RID: 392 RVA: 0x00006A14 File Offset: 0x00004C14
	private void LateUpdate()
	{
	}

	// Token: 0x06000189 RID: 393 RVA: 0x00006A18 File Offset: 0x00004C18
	private void Update()
	{
		if (this.state != AProjectile.eState.STARTED)
		{
			return;
		}
		this.flyTimer += Time.deltaTime;
		if (this.flyTimer < this.totalFlyTime)
		{
			base.transform.position = Vector3.Lerp(this.startPosition, base.GetFlyTargetPosition(), Easing.GetEasingFunction(Easing.Type.EaseInCubic, this.flyTimer / this.totalFlyTime));
			base.transform.position += this.startOffsetVector * Mathf.Sin(this.flyTimer / this.totalFlyTime * 3.1415927f);
			return;
		}
		if (this.targetMonster != null && this.targetMonster.IsAttackable())
		{
			int num = this.damage;
			this.targetMonster.Hit(num, eDamageType.FIRE, default(Vector3));
			base.OnHit(this.targetMonster);
		}
		this.Despawn();
	}

	// Token: 0x0600018A RID: 394 RVA: 0x00006B02 File Offset: 0x00004D02
	public void Setup(int damage)
	{
		this.damage = damage;
	}

	// Token: 0x0600018B RID: 395 RVA: 0x00006B0C File Offset: 0x00004D0C
	protected override void SpawnProc()
	{
		this.startPosition = base.transform.position;
		this.rigidbody.isKinematic = false;
		this.rigidbody.velocity = base.transform.forward * this.speed;
		this.totalFlyTime = Vector3.Distance(this.targetMonster.HeadWorldPosition, base.transform.position) / this.speed;
		this.flyTimer = 0f;
		this.startOffsetVector = new Vector3(Random.Range(-1f * this.randomOffsetMaxRange.x, this.randomOffsetMaxRange.x), Random.Range(0f, this.randomOffsetMaxRange.y), Random.Range(-1f * this.randomOffsetMaxRange.z, this.randomOffsetMaxRange.z));
		Debug.DrawLine(base.transform.position, base.transform.position + this.startOffsetVector, Color.yellow, 2f);
	}

	// Token: 0x0600018C RID: 396 RVA: 0x00006C1C File Offset: 0x00004E1C
	protected override void DespawnProc()
	{
		this.rigidbody.velocity = Vector3.zero;
		this.rigidbody.isKinematic = true;
	}

	// Token: 0x0600018D RID: 397 RVA: 0x00006C3A File Offset: 0x00004E3A
	protected override void DestroyProc()
	{
	}

	// Token: 0x04000112 RID: 274
	[SerializeField]
	private float speed = 1f;

	// Token: 0x04000113 RID: 275
	[SerializeField]
	private Rigidbody rigidbody;

	// Token: 0x04000114 RID: 276
	[SerializeField]
	private Vector3 randomOffsetMaxRange;

	// Token: 0x04000115 RID: 277
	[SerializeField]
	private float maxFlightHeight = 10f;

	// Token: 0x04000116 RID: 278
	private float totalFlyTime;

	// Token: 0x04000117 RID: 279
	private float flyTimer;

	// Token: 0x04000118 RID: 280
	private Vector3 startPosition;

	// Token: 0x04000119 RID: 281
	private int damage;

	// Token: 0x0400011A RID: 282
	private Vector3 startOffsetVector;
}
