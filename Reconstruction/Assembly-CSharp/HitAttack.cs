using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class HitAttack : ElementSkill
{
	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000190 RID: 400 RVA: 0x00006DEE File Offset: 0x00004FEE
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				1,
				1,
				0
			};
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000191 RID: 401 RVA: 0x00006E0A File Offset: 0x0000500A
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x06000192 RID: 402 RVA: 0x00006E11 File Offset: 0x00005011
	public override float KeyValue2
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06000193 RID: 403 RVA: 0x00006E18 File Offset: 0x00005018
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000194 RID: 404 RVA: 0x00006E43 File Offset: 0x00005043
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("PENETRATION"));
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000195 RID: 405 RVA: 0x00006E60 File Offset: 0x00005060
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue2.ToString());
		}
	}

	// Token: 0x06000196 RID: 406 RVA: 0x00006E8B File Offset: 0x0000508B
	public override void Build()
	{
		base.Build();
		this.buffInfo = new BuffInfo(EnemyBuffName.DamageIntensify, 1, false);
	}

	// Token: 0x06000197 RID: 407 RVA: 0x00006EA1 File Offset: 0x000050A1
	public override float Hit(float damage, IDamage target, Bullet bullet = null)
	{
		this.buffInfo.Stacks = Mathf.RoundToInt((float)this.strategy.GoldCount * bullet.BulletEffectIntensify);
		target.DamageStrategy.ApplyBuff(this.buffInfo);
		return base.Hit(damage, target, bullet);
	}

	// Token: 0x0400011D RID: 285
	private BuffInfo buffInfo;
}
