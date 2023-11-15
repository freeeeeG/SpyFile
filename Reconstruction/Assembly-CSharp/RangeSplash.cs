using System;
using System.Collections.Generic;

// Token: 0x02000057 RID: 87
public class RangeSplash : ElementSkill
{
	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000230 RID: 560 RVA: 0x00007FAC File Offset: 0x000061AC
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				4,
				4,
				4
			};
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000231 RID: 561 RVA: 0x00007FC8 File Offset: 0x000061C8
	public override float KeyValue
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000232 RID: 562 RVA: 0x00007FCF File Offset: 0x000061CF
	public override float KeyValue2
	{
		get
		{
			return 0.8f;
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06000233 RID: 563 RVA: 0x00007FD8 File Offset: 0x000061D8
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06000234 RID: 564 RVA: 0x00008004 File Offset: 0x00006204
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0000803F File Offset: 0x0000623F
	public override void Build()
	{
		base.Build();
		this.strategy.BaseFixTargetCount += (int)this.KeyValue;
	}
}
