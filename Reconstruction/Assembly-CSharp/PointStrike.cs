using System;
using System.Collections.Generic;

// Token: 0x02000091 RID: 145
public class PointStrike : ElementSkill
{
	// Token: 0x1700018F RID: 399
	// (get) Token: 0x0600036D RID: 877 RVA: 0x0000A17C File Offset: 0x0000837C
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				0,
				2,
				3
			};
		}
	}

	// Token: 0x0600036E RID: 878 RVA: 0x0000A198 File Offset: 0x00008398
	public override void AfterShoot(Bullet bullet = null, IDamage target = null)
	{
		base.AfterShoot(bullet, target);
		if (target == null)
		{
			return;
		}
		if (target.DamageStrategy.IsFrost)
		{
			bullet.isCritical = true;
		}
	}
}
