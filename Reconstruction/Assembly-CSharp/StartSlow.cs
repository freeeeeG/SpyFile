using System;
using System.Collections.Generic;

// Token: 0x0200004F RID: 79
public class StartSlow : ElementSkill
{
	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x060001FD RID: 509 RVA: 0x0000799C File Offset: 0x00005B9C
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				4,
				4,
				2
			};
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x060001FE RID: 510 RVA: 0x000079B8 File Offset: 0x00005BB8
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x060001FF RID: 511 RVA: 0x000079BF File Offset: 0x00005BBF
	public override float KeyValue2
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x06000200 RID: 512 RVA: 0x000079C8 File Offset: 0x00005BC8
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x06000201 RID: 513 RVA: 0x000079F4 File Offset: 0x00005BF4
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x06000202 RID: 514 RVA: 0x00007A2F File Offset: 0x00005C2F
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.strategy.TurnFixSplashPercentage += (float)this.strategy.WaterCount * this.KeyValue2;
	}
}
