using System;
using System.Collections.Generic;

// Token: 0x0200008E RID: 142
public class RapidBase : ElementSkill
{
	// Token: 0x17000182 RID: 386
	// (get) Token: 0x06000354 RID: 852 RVA: 0x00009DF0 File Offset: 0x00007FF0
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				0,
				1,
				2
			};
		}
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x06000355 RID: 853 RVA: 0x00009E0C File Offset: 0x0000800C
	public override float KeyValue
	{
		get
		{
			return 0.7f;
		}
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06000356 RID: 854 RVA: 0x00009E14 File Offset: 0x00008014
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x06000357 RID: 855 RVA: 0x00009E4F File Offset: 0x0000804F
	public override void Build()
	{
		base.Build();
		this.strategy.BaseFixFrostResist += this.KeyValue;
	}
}
