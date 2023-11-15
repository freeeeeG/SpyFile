using System;
using System.Collections.Generic;

// Token: 0x0200005E RID: 94
public class VulunScan : ElementSkill
{
	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06000262 RID: 610 RVA: 0x0000861C File Offset: 0x0000681C
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				13,
				13,
				13
			};
		}
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06000263 RID: 611 RVA: 0x0000863B File Offset: 0x0000683B
	public override float KeyValue
	{
		get
		{
			return 0.01f;
		}
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x06000264 RID: 612 RVA: 0x00008642 File Offset: 0x00006842
	public override float KeyValue2
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x06000265 RID: 613 RVA: 0x0000864C File Offset: 0x0000684C
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06000266 RID: 614 RVA: 0x00008688 File Offset: 0x00006888
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x06000267 RID: 615 RVA: 0x000086C3 File Offset: 0x000068C3
	public override void AfterShoot(Bullet bullet = null, IDamage target = null)
	{
		base.AfterShoot(bullet, target);
		if (target == null)
		{
			return;
		}
		bullet.BulletDamageIntensify += target.DamageStrategy.PathProgress * 100f * this.KeyValue2 * bullet.BulletEffectIntensify;
	}
}
