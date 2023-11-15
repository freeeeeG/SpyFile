using System;
using System.Collections.Generic;

// Token: 0x0200005D RID: 93
public class BalanceGrid : ElementSkill
{
	// Token: 0x17000100 RID: 256
	// (get) Token: 0x0600025B RID: 603 RVA: 0x00008499 File Offset: 0x00006699
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				10,
				10,
				11
			};
		}
	}

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x0600025C RID: 604 RVA: 0x000084B8 File Offset: 0x000066B8
	public override float KeyValue
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x0600025D RID: 605 RVA: 0x000084BF File Offset: 0x000066BF
	public override float KeyValue2
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x0600025E RID: 606 RVA: 0x000084C8 File Offset: 0x000066C8
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x0600025F RID: 607 RVA: 0x000084F4 File Offset: 0x000066F4
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue2.ToString());
		}
	}

	// Token: 0x06000260 RID: 608 RVA: 0x00008520 File Offset: 0x00006720
	public override void StartTurn2()
	{
		base.StartTurn2();
		if ((float)this.strategy.GoldCount < this.KeyValue)
		{
			this.strategy.TempGoldCount += (int)this.KeyValue2;
		}
		if ((float)this.strategy.WoodCount < this.KeyValue)
		{
			this.strategy.TempWoodCount += (int)this.KeyValue2;
		}
		if ((float)this.strategy.WaterCount < this.KeyValue)
		{
			this.strategy.TempWaterCount += (int)this.KeyValue2;
		}
		if ((float)this.strategy.FireCount < this.KeyValue)
		{
			this.strategy.TempFireCount += (int)this.KeyValue2;
		}
		if ((float)this.strategy.DustCount < this.KeyValue)
		{
			this.strategy.TempDustCount += (int)this.KeyValue2;
		}
	}
}
