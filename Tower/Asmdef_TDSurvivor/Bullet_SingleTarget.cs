using System;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class Bullet_SingleTarget : ASingleTargetProjectile
{
	// Token: 0x060001BA RID: 442 RVA: 0x0000773C File Offset: 0x0000593C
	private void LateUpdate()
	{
	}

	// Token: 0x060001BB RID: 443 RVA: 0x00007740 File Offset: 0x00005940
	private void Update()
	{
		if (this.state != AProjectile.eState.STARTED)
		{
			return;
		}
		if (this.targetMonster == null || !this.targetMonster.IsAttackable())
		{
			this.Despawn();
		}
		this.flyTimer += Time.deltaTime;
		if (this.flyTimer < this.totalFlyTime)
		{
			base.transform.forward = base.GetFlyTargetPosition() - base.transform.position;
			base.transform.position = Vector3.Lerp(this.startPosition, base.GetFlyTargetPosition(), this.flyTimer / this.totalFlyTime);
			return;
		}
		if (this.targetMonster != null && this.targetMonster.IsAttackable())
		{
			int num = this.damage;
			this.targetMonster.Hit(num, this.damageType, this.targetMonster.transform.position - this.spawnPosition);
			base.OnHit(this.targetMonster);
			this.Despawn();
		}
	}

	// Token: 0x060001BC RID: 444 RVA: 0x00007844 File Offset: 0x00005A44
	public void Setup(int damage, eDamageType damageType = eDamageType.NONE)
	{
		this.damage = damage;
		this.damageType = damageType;
	}

	// Token: 0x060001BD RID: 445 RVA: 0x00007854 File Offset: 0x00005A54
	protected override void SpawnProc()
	{
		this.startPosition = base.transform.position;
		this.totalFlyTime = Vector3.Distance(this.targetMonster.HeadWorldPosition, base.transform.position) / this.speed;
		this.flyTimer = 0f;
	}

	// Token: 0x060001BE RID: 446 RVA: 0x000078A5 File Offset: 0x00005AA5
	protected override void DespawnProc()
	{
	}

	// Token: 0x060001BF RID: 447 RVA: 0x000078A7 File Offset: 0x00005AA7
	protected override void DestroyProc()
	{
	}

	// Token: 0x04000145 RID: 325
	[SerializeField]
	private float speed = 1f;

	// Token: 0x04000146 RID: 326
	[SerializeField]
	private Rigidbody rigidbody;

	// Token: 0x04000147 RID: 327
	private float totalFlyTime;

	// Token: 0x04000148 RID: 328
	private float flyTimer;

	// Token: 0x04000149 RID: 329
	private Vector3 startPosition;

	// Token: 0x0400014A RID: 330
	private int damage;

	// Token: 0x0400014B RID: 331
	private eDamageType damageType;
}
