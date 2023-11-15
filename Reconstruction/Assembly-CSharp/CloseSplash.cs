using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class CloseSplash : ElementSkill
{
	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000237 RID: 567 RVA: 0x00008068 File Offset: 0x00006268
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				1,
				1,
				4
			};
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x06000238 RID: 568 RVA: 0x00008084 File Offset: 0x00006284
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06000239 RID: 569 RVA: 0x0000808B File Offset: 0x0000628B
	public override float KeyValue2
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x0600023A RID: 570 RVA: 0x00008094 File Offset: 0x00006294
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x0600023B RID: 571 RVA: 0x000080BF File Offset: 0x000062BF
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("PENETRATION"));
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x0600023C RID: 572 RVA: 0x000080DC File Offset: 0x000062DC
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue2.ToString());
		}
	}

	// Token: 0x0600023D RID: 573 RVA: 0x00008107 File Offset: 0x00006307
	public override void Build()
	{
		base.Build();
		this.buffInfo = new BuffInfo(EnemyBuffName.DamageIntensify, 1, false);
	}

	// Token: 0x0600023E RID: 574 RVA: 0x0000811D File Offset: 0x0000631D
	public override float Hit(float damage, IDamage target, Bullet bullet = null)
	{
		this.buffInfo.Stacks = Mathf.RoundToInt((float)this.strategy.DustCount * bullet.BulletEffectIntensify);
		target.DamageStrategy.ApplyBuff(this.buffInfo);
		return base.Hit(damage, target, bullet);
	}

	// Token: 0x04000132 RID: 306
	private BuffInfo buffInfo;
}
