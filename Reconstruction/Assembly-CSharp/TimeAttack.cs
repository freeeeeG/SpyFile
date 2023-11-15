using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000040 RID: 64
public class TimeAttack : ElementSkill
{
	// Token: 0x17000085 RID: 133
	// (get) Token: 0x06000199 RID: 409 RVA: 0x00006EE8 File Offset: 0x000050E8
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				2,
				2,
				0
			};
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x0600019A RID: 410 RVA: 0x00006F04 File Offset: 0x00005104
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized("1");
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x0600019B RID: 411 RVA: 0x00006F1B File Offset: 0x0000511B
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("FROZEN"));
		}
	}

	// Token: 0x0600019C RID: 412 RVA: 0x00006F37 File Offset: 0x00005137
	public override void Build()
	{
		base.Build();
		this.buffInfo = new BuffInfo(EnemyBuffName.SlowIntensify, 1, false);
	}

	// Token: 0x0600019D RID: 413 RVA: 0x00006F4E File Offset: 0x0000514E
	public override float Hit(float damage, IDamage target, Bullet bullet = null)
	{
		this.buffInfo.Stacks = Mathf.RoundToInt((float)this.strategy.GoldCount * bullet.BulletEffectIntensify);
		target.DamageStrategy.ApplyBuff(this.buffInfo);
		return damage;
	}

	// Token: 0x0400011E RID: 286
	private BuffInfo buffInfo;
}
