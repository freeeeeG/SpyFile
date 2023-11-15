using System;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class Bullet_HomingMissile : ASingleTargetProjectile
{
	// Token: 0x0600019F RID: 415 RVA: 0x00007187 File Offset: 0x00005387
	private void LateUpdate()
	{
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000718C File Offset: 0x0000538C
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
			base.transform.position += this.startOffsetVector * Mathf.Sin(this.flyTimer / this.totalFlyTime * 3.1415927f);
			base.transform.forward = (base.transform.position - this.lastUpdatePosition).normalized;
		}
		else
		{
			if (this.targetMonster != null && this.targetMonster.IsAttackable())
			{
				int num = this.damage;
				this.targetMonster.Hit(num, this.damageType, default(Vector3));
				base.OnHit(this.targetMonster);
			}
			SoundManager.PlaySound("Cannon", "Cannon_Explode_1015", -1f, -1f, -1f);
			this.Despawn();
		}
		this.lastUpdatePosition = base.transform.position;
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x000072D2 File Offset: 0x000054D2
	public void Setup(int damage, eDamageType damageType = eDamageType.NONE)
	{
		this.damage = damage;
		this.damageType = damageType;
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x000072E4 File Offset: 0x000054E4
	protected override void SpawnProc()
	{
		this.startPosition = base.transform.position;
		this.rigidbody.isKinematic = false;
		this.rigidbody.velocity = base.transform.forward * this.speed;
		float num = Random.Range(0.8f, 1.2f) * this.speed;
		this.totalFlyTime = Vector3.Distance(this.targetMonster.HeadWorldPosition, base.transform.position) / num;
		this.flyTimer = 0f;
		this.startOffsetVector = new Vector3(Random.Range(-1f * this.randomOffsetMaxRange.x, this.randomOffsetMaxRange.x), Random.Range(0f, this.randomOffsetMaxRange.y), Random.Range(-1f * this.randomOffsetMaxRange.z, 0f));
		this.startOffsetVector = base.transform.rotation * this.startOffsetVector;
		this.lastUpdatePosition = base.transform.position - base.transform.forward;
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x0000740D File Offset: 0x0000560D
	protected override void DespawnProc()
	{
		this.rigidbody.velocity = Vector3.zero;
		this.rigidbody.isKinematic = true;
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000742B File Offset: 0x0000562B
	protected override void DestroyProc()
	{
	}

	// Token: 0x0400012E RID: 302
	[SerializeField]
	private float speed = 1f;

	// Token: 0x0400012F RID: 303
	[SerializeField]
	private Rigidbody rigidbody;

	// Token: 0x04000130 RID: 304
	[SerializeField]
	private Vector3 randomOffsetMaxRange;

	// Token: 0x04000131 RID: 305
	private float totalFlyTime;

	// Token: 0x04000132 RID: 306
	private float flyTimer;

	// Token: 0x04000133 RID: 307
	private Vector3 startPosition;

	// Token: 0x04000134 RID: 308
	private int damage;

	// Token: 0x04000135 RID: 309
	private Vector3 startOffsetVector;

	// Token: 0x04000136 RID: 310
	private Vector3 lastUpdatePosition;

	// Token: 0x04000137 RID: 311
	private Vector3 lastMonsterPosition;

	// Token: 0x04000138 RID: 312
	private eDamageType damageType;
}
