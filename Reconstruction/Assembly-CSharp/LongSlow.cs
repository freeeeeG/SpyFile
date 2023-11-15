using System;
using System.Collections.Generic;

// Token: 0x0200004E RID: 78
public class LongSlow : ElementSkill
{
	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x060001F6 RID: 502 RVA: 0x000078D4 File Offset: 0x00005AD4
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				3,
				3,
				2
			};
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x060001F7 RID: 503 RVA: 0x000078F0 File Offset: 0x00005AF0
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x060001F8 RID: 504 RVA: 0x000078F7 File Offset: 0x00005AF7
	public override float KeyValue2
	{
		get
		{
			return 0.5f;
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x060001F9 RID: 505 RVA: 0x00007900 File Offset: 0x00005B00
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x060001FA RID: 506 RVA: 0x0000792C File Offset: 0x00005B2C
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x060001FB RID: 507 RVA: 0x00007967 File Offset: 0x00005B67
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.strategy.TurnFixCriticalPercentage += (float)this.strategy.WaterCount * this.KeyValue2;
	}
}
