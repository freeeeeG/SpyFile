using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class Bullet_PoisionDart : ASingleTargetProjectile
{
	// Token: 0x060001AD RID: 429 RVA: 0x00007517 File Offset: 0x00005717
	private void LateUpdate()
	{
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0000751C File Offset: 0x0000571C
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
			float num = GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.POISON_FASTER) ? 0.25f : 0.2f;
			float duration = (float)this.damage / num;
			this.targetMonster.ApplyDamageDebuff(duration, num, 1, eDamageType.POISON, this.spawnSource.GetInstanceID());
			base.OnHit(this.targetMonster);
			this.Despawn();
		}
	}

	// Token: 0x060001AF RID: 431 RVA: 0x00007630 File Offset: 0x00005830
	public void Setup(int damage, eDamageType damageType = eDamageType.NONE)
	{
		this.damage = damage;
		this.damageType = damageType;
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00007640 File Offset: 0x00005840
	protected override void SpawnProc()
	{
		this.startPosition = base.transform.position;
		this.totalFlyTime = Vector3.Distance(this.targetMonster.HeadWorldPosition, base.transform.position) / this.speed;
		this.flyTimer = 0f;
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x00007691 File Offset: 0x00005891
	protected override void DespawnProc()
	{
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x00007693 File Offset: 0x00005893
	protected override void DestroyProc()
	{
	}

	// Token: 0x0400013C RID: 316
	[SerializeField]
	private float speed = 1f;

	// Token: 0x0400013D RID: 317
	[SerializeField]
	private Rigidbody rigidbody;

	// Token: 0x0400013E RID: 318
	private float totalFlyTime;

	// Token: 0x0400013F RID: 319
	private float flyTimer;

	// Token: 0x04000140 RID: 320
	private Vector3 startPosition;

	// Token: 0x04000141 RID: 321
	private int damage;

	// Token: 0x04000142 RID: 322
	private eDamageType damageType;
}
