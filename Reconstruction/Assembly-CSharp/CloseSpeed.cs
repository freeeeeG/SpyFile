using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class CloseSpeed : ElementSkill
{
	// Token: 0x17000097 RID: 151
	// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000720D File Offset: 0x0000540D
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				2,
				2,
				1
			};
		}
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x060001B6 RID: 438 RVA: 0x00007229 File Offset: 0x00005429
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized("1");
		}
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x060001B7 RID: 439 RVA: 0x00007240 File Offset: 0x00005440
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("FROZEN"));
		}
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x0000725C File Offset: 0x0000545C
	public override void Build()
	{
		base.Build();
		this.buffInfo = new BuffInfo(EnemyBuffName.SlowIntensify, 1, false);
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x00007273 File Offset: 0x00005473
	public override float Hit(float damage, IDamage target, Bullet bullet = null)
	{
		this.buffInfo.Stacks = Mathf.RoundToInt((float)this.strategy.WoodCount * bullet.BulletEffectIntensify);
		target.DamageStrategy.ApplyBuff(this.buffInfo);
		return damage;
	}

	// Token: 0x04000120 RID: 288
	private BuffInfo buffInfo;
}
