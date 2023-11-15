using System;

// Token: 0x020000F5 RID: 245
public class Restorer : Enemy
{
	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06000623 RID: 1571 RVA: 0x000108EA File Offset: 0x0000EAEA
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.Restorer;
		}
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x000108ED File Offset: 0x0000EAED
	protected override void SetStrategy()
	{
		base.DamageStrategy = new RestorerStrategy(this);
	}
}
