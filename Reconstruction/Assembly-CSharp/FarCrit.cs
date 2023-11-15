using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class FarCrit : ElementSkill
{
	// Token: 0x170000CF RID: 207
	// (get) Token: 0x0600020B RID: 523 RVA: 0x00007B3E File Offset: 0x00005D3E
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				1,
				1,
				3
			};
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x0600020C RID: 524 RVA: 0x00007B5A File Offset: 0x00005D5A
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x0600020D RID: 525 RVA: 0x00007B61 File Offset: 0x00005D61
	public override float KeyValue2
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x0600020E RID: 526 RVA: 0x00007B68 File Offset: 0x00005D68
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x0600020F RID: 527 RVA: 0x00007B93 File Offset: 0x00005D93
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("PENETRATION"));
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x06000210 RID: 528 RVA: 0x00007BB0 File Offset: 0x00005DB0
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue2.ToString());
		}
	}

	// Token: 0x06000211 RID: 529 RVA: 0x00007BDB File Offset: 0x00005DDB
	public override void Build()
	{
		base.Build();
		this.buffInfo = new BuffInfo(EnemyBuffName.DamageIntensify, 1, false);
	}

	// Token: 0x06000212 RID: 530 RVA: 0x00007BF1 File Offset: 0x00005DF1
	public override float Hit(float damage, IDamage target, Bullet bullet = null)
	{
		this.buffInfo.Stacks = Mathf.RoundToInt((float)this.strategy.FireCount * bullet.BulletEffectIntensify);
		target.DamageStrategy.ApplyBuff(this.buffInfo);
		return base.Hit(damage, target, bullet);
	}

	// Token: 0x0400012C RID: 300
	private BuffInfo buffInfo;
}
