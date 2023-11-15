using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class HitCritical : ElementSkill
{
	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x06000214 RID: 532 RVA: 0x00007C38 File Offset: 0x00005E38
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				2,
				2,
				3
			};
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x06000215 RID: 533 RVA: 0x00007C54 File Offset: 0x00005E54
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized("1");
		}
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x06000216 RID: 534 RVA: 0x00007C6B File Offset: 0x00005E6B
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("FROZEN"));
		}
	}

	// Token: 0x06000217 RID: 535 RVA: 0x00007C87 File Offset: 0x00005E87
	public override void Build()
	{
		base.Build();
		this.buffInfo = new BuffInfo(EnemyBuffName.SlowIntensify, 1, false);
	}

	// Token: 0x06000218 RID: 536 RVA: 0x00007C9E File Offset: 0x00005E9E
	public override float Hit(float damage, IDamage target, Bullet bullet = null)
	{
		this.buffInfo.Stacks = Mathf.RoundToInt((float)this.strategy.FireCount * bullet.BulletEffectIntensify);
		target.DamageStrategy.ApplyBuff(this.buffInfo);
		return damage;
	}

	// Token: 0x0400012D RID: 301
	private BuffInfo buffInfo;
}
