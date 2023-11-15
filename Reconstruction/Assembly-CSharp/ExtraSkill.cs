using System;
using System.Collections.Generic;

// Token: 0x0200004B RID: 75
public class ExtraSkill : ElementSkill
{
	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000764E File Offset: 0x0000584E
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				2,
				2,
				2
			};
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000766A File Offset: 0x0000586A
	public override float KeyValue
	{
		get
		{
			return 0.03f;
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x060001E2 RID: 482 RVA: 0x00007674 File Offset: 0x00005874
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x000076AF File Offset: 0x000058AF
	public override float Hit(float damage, IDamage target, Bullet bullet = null)
	{
		target.DamageStrategy.ApplyFrost(target.DamageStrategy.MaxFrost * this.KeyValue * bullet.BulletEffectIntensify);
		return base.Hit(damage, target, bullet);
	}
}
