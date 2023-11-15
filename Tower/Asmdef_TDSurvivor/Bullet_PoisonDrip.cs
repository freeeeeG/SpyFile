using System;

// Token: 0x02000051 RID: 81
public class Bullet_PoisonDrip : AProjectile
{
	// Token: 0x060001B4 RID: 436 RVA: 0x000076A8 File Offset: 0x000058A8
	private void LateUpdate()
	{
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x000076AA File Offset: 0x000058AA
	public void Setup(float range, int damage)
	{
		this.range = range;
		this.damage = damage;
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x000076BC File Offset: 0x000058BC
	protected override void SpawnProc()
	{
		foreach (AMonsterBase amonsterBase in Singleton<MonsterManager>.Instance.GetMonstersInRange(base.transform.position, this.range))
		{
			base.OnHit(this.targetMonster);
		}
		this.Despawn();
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x00007730 File Offset: 0x00005930
	protected override void DespawnProc()
	{
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x00007732 File Offset: 0x00005932
	protected override void DestroyProc()
	{
	}

	// Token: 0x04000143 RID: 323
	private float range;

	// Token: 0x04000144 RID: 324
	private int damage;
}
