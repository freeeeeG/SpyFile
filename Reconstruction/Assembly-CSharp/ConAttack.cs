using System;
using System.Collections.Generic;

// Token: 0x02000042 RID: 66
public class ConAttack : ElementSkill
{
	// Token: 0x1700008D RID: 141
	// (get) Token: 0x060001A6 RID: 422 RVA: 0x00007054 File Offset: 0x00005254
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				4,
				4,
				0
			};
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x060001A7 RID: 423 RVA: 0x00007070 File Offset: 0x00005270
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x060001A8 RID: 424 RVA: 0x00007077 File Offset: 0x00005277
	public override float KeyValue2
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x060001A9 RID: 425 RVA: 0x00007080 File Offset: 0x00005280
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x060001AA RID: 426 RVA: 0x000070AC File Offset: 0x000052AC
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x060001AB RID: 427 RVA: 0x000070E7 File Offset: 0x000052E7
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.strategy.TurnFixSplashPercentage += (float)this.strategy.GoldCount * this.KeyValue2;
	}
}
