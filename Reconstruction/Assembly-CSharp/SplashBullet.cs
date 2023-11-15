using System;
using System.Collections.Generic;

// Token: 0x02000093 RID: 147
public class SplashBullet : ElementSkill
{
	// Token: 0x17000193 RID: 403
	// (get) Token: 0x06000376 RID: 886 RVA: 0x0000A2FB File Offset: 0x000084FB
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				0,
				3,
				4
			};
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06000377 RID: 887 RVA: 0x0000A317 File Offset: 0x00008517
	public override float KeyValue
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x06000378 RID: 888 RVA: 0x0000A320 File Offset: 0x00008520
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x06000379 RID: 889 RVA: 0x0000A34C File Offset: 0x0000854C
	public override void AfterShoot(Bullet bullet = null, IDamage target = null)
	{
		base.AfterShoot(bullet, target);
		float targetDistance = bullet.GetTargetDistance();
		bullet.SplashRange += this.KeyValue * targetDistance * bullet.BulletEffectIntensify;
	}
}
