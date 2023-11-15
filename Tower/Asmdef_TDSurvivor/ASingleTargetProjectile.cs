using System;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class ASingleTargetProjectile : AProjectile
{
	// Token: 0x0600015A RID: 346 RVA: 0x0000612A File Offset: 0x0000432A
	protected Vector3 GetFlyTargetPosition()
	{
		if (this.targetMonster != null && this.targetMonster.IsAttackable())
		{
			this.lastMonsterHitPosition = this.targetMonster.HeadWorldPosition;
			return this.targetMonster.HeadWorldPosition;
		}
		return this.lastMonsterHitPosition;
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0000616A File Offset: 0x0000436A
	protected override void SpawnProc()
	{
	}

	// Token: 0x0600015C RID: 348 RVA: 0x0000616C File Offset: 0x0000436C
	protected override void DespawnProc()
	{
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0000616E File Offset: 0x0000436E
	protected override void DestroyProc()
	{
	}

	// Token: 0x040000EB RID: 235
	private Vector3 lastMonsterHitPosition;
}
