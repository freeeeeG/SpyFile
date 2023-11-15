using System;
using System.Collections.Generic;

// Token: 0x0200005A RID: 90
public class TimeSplash : ElementSkill
{
	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000246 RID: 582 RVA: 0x00008209 File Offset: 0x00006409
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				0,
				0,
				4
			};
		}
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000247 RID: 583 RVA: 0x00008225 File Offset: 0x00006425
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000248 RID: 584 RVA: 0x0000822C File Offset: 0x0000642C
	public override float KeyValue2
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x06000249 RID: 585 RVA: 0x00008234 File Offset: 0x00006434
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue2.ToString());
		}
	}

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x0600024A RID: 586 RVA: 0x00008260 File Offset: 0x00006460
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0000828C File Offset: 0x0000648C
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.strategy.TurnFixRange += (int)this.KeyValue * (this.strategy.DustCount / (int)this.KeyValue2);
		this.strategy.Concrete.GenerateRange();
	}

	// Token: 0x0600024C RID: 588 RVA: 0x000082DC File Offset: 0x000064DC
	public override void EndTurn()
	{
		base.EndTurn();
		this.strategy.Concrete.GenerateRange();
	}
}
