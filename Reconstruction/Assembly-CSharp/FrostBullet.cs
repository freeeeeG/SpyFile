using System;
using System.Collections.Generic;

// Token: 0x02000094 RID: 148
public class FrostBullet : ElementSkill
{
	// Token: 0x17000196 RID: 406
	// (get) Token: 0x0600037B RID: 891 RVA: 0x0000A38C File Offset: 0x0000858C
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				1,
				2,
				3
			};
		}
	}

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x0600037C RID: 892 RVA: 0x0000A3A8 File Offset: 0x000085A8
	public override float KeyValue
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x0600037D RID: 893 RVA: 0x0000A3AF File Offset: 0x000085AF
	public override float KeyValue2
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x0600037E RID: 894 RVA: 0x0000A3B8 File Offset: 0x000085B8
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x0600037F RID: 895 RVA: 0x0000A3F4 File Offset: 0x000085F4
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x06000380 RID: 896 RVA: 0x0000A42F File Offset: 0x0000862F
	public override float Hit(float damage, IDamage target, Bullet bullet = null)
	{
		if (target.DamageStrategy.CurrentFrost / target.DamageStrategy.MaxFrost <= this.KeyValue)
		{
			target.DamageStrategy.ApplyFrost(bullet.SlowRate);
		}
		return base.Hit(damage, target, bullet);
	}
}
