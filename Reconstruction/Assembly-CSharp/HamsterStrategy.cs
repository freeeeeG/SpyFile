using System;

// Token: 0x02000125 RID: 293
public class HamsterStrategy : BasicEnemyStrategy
{
	// Token: 0x170002FB RID: 763
	// (get) Token: 0x0600079A RID: 1946 RVA: 0x000146C5 File Offset: 0x000128C5
	public override bool IsEnemy
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x000146C8 File Offset: 0x000128C8
	public HamsterStrategy(IDamage damageTarget) : base(damageTarget)
	{
	}
}
