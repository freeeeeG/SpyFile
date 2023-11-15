using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class CloseSlow : ElementSkill
{
	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x060001E5 RID: 485 RVA: 0x000076E6 File Offset: 0x000058E6
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				1,
				1,
				2
			};
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x060001E6 RID: 486 RVA: 0x00007702 File Offset: 0x00005902
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x060001E7 RID: 487 RVA: 0x00007709 File Offset: 0x00005909
	public override float KeyValue2
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x060001E8 RID: 488 RVA: 0x00007710 File Offset: 0x00005910
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000773B File Offset: 0x0000593B
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("PENETRATION"));
		}
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x060001EA RID: 490 RVA: 0x00007758 File Offset: 0x00005958
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue2.ToString());
		}
	}

	// Token: 0x060001EB RID: 491 RVA: 0x00007783 File Offset: 0x00005983
	public override void Build()
	{
		base.Build();
		this.buffInfo = new BuffInfo(EnemyBuffName.DamageIntensify, 1, false);
	}

	// Token: 0x060001EC RID: 492 RVA: 0x00007799 File Offset: 0x00005999
	public override float Hit(float damage, IDamage target, Bullet bullet = null)
	{
		this.buffInfo.Stacks = Mathf.RoundToInt((float)this.strategy.WaterCount * bullet.BulletEffectIntensify);
		target.DamageStrategy.ApplyBuff(this.buffInfo);
		return base.Hit(damage, target, bullet);
	}

	// Token: 0x0400012B RID: 299
	private BuffInfo buffInfo;
}
