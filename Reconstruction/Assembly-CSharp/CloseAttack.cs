using System;
using System.Collections.Generic;

// Token: 0x02000041 RID: 65
public class CloseAttack : ElementSkill
{
	// Token: 0x17000088 RID: 136
	// (get) Token: 0x0600019F RID: 415 RVA: 0x00006F8D File Offset: 0x0000518D
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				3,
				3,
				0
			};
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x060001A0 RID: 416 RVA: 0x00006FA9 File Offset: 0x000051A9
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x060001A1 RID: 417 RVA: 0x00006FB0 File Offset: 0x000051B0
	public override float KeyValue2
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x060001A2 RID: 418 RVA: 0x00006FB8 File Offset: 0x000051B8
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x060001A3 RID: 419 RVA: 0x00006FE4 File Offset: 0x000051E4
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000701F File Offset: 0x0000521F
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.strategy.TurnFixCriticalPercentage += (float)this.strategy.GoldCount * this.KeyValue2;
	}
}
