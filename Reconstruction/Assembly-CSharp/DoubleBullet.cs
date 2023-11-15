using System;
using System.Collections.Generic;

// Token: 0x02000095 RID: 149
public class DoubleBullet : ElementSkill
{
	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06000382 RID: 898 RVA: 0x0000A472 File Offset: 0x00008672
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				1,
				2,
				4
			};
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06000383 RID: 899 RVA: 0x0000A48E File Offset: 0x0000868E
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x06000384 RID: 900 RVA: 0x0000A498 File Offset: 0x00008698
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x06000385 RID: 901 RVA: 0x0000A4D3 File Offset: 0x000086D3
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(GameMultiLang.GetTraduction("WHENHIT"));
		}
	}

	// Token: 0x06000386 RID: 902 RVA: 0x0000A4EF File Offset: 0x000086EF
	public override void Build()
	{
		base.Build();
		this.strategy.BaseFixBulletEffectIntensify += this.KeyValue;
	}
}
