using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class HitSplash : ElementSkill
{
	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06000240 RID: 576 RVA: 0x00008164 File Offset: 0x00006364
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				2,
				2,
				4
			};
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000241 RID: 577 RVA: 0x00008180 File Offset: 0x00006380
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized("1");
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000242 RID: 578 RVA: 0x00008197 File Offset: 0x00006397
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("FROZEN"));
		}
	}

	// Token: 0x06000243 RID: 579 RVA: 0x000081B3 File Offset: 0x000063B3
	public override void Build()
	{
		base.Build();
		this.buffInfo = new BuffInfo(EnemyBuffName.SlowIntensify, 1, false);
	}

	// Token: 0x06000244 RID: 580 RVA: 0x000081CA File Offset: 0x000063CA
	public override float Hit(float damage, IDamage target, Bullet bullet = null)
	{
		this.buffInfo.Stacks = Mathf.RoundToInt((float)this.strategy.DustCount * bullet.BulletEffectIntensify);
		target.DamageStrategy.ApplyBuff(this.buffInfo);
		return damage;
	}

	// Token: 0x04000133 RID: 307
	private BuffInfo buffInfo;
}
