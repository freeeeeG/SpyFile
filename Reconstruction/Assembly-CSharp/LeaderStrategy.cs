using System;

// Token: 0x02000127 RID: 295
public class LeaderStrategy : BasicEnemyStrategy
{
	// Token: 0x170002FD RID: 765
	// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00014779 File Offset: 0x00012979
	public override bool IsEnemy
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x0001477C File Offset: 0x0001297C
	public LeaderStrategy(IDamage damageTarget) : base(damageTarget)
	{
	}

	// Token: 0x040003B9 RID: 953
	public float ReduceValue;
}
