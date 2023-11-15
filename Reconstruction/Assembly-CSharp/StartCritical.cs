using System;
using System.Collections.Generic;

// Token: 0x02000054 RID: 84
public class StartCritical : ElementSkill
{
	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000221 RID: 545 RVA: 0x00007DA4 File Offset: 0x00005FA4
	public override List<int> InitElements
	{
		get
		{
			return new List<int>
			{
				0,
				0,
				3
			};
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06000222 RID: 546 RVA: 0x00007DC0 File Offset: 0x00005FC0
	public override float KeyValue
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x06000223 RID: 547 RVA: 0x00007DC7 File Offset: 0x00005FC7
	public override float KeyValue2
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000224 RID: 548 RVA: 0x00007DD0 File Offset: 0x00005FD0
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue2.ToString());
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06000225 RID: 549 RVA: 0x00007DFC File Offset: 0x00005FFC
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x06000226 RID: 550 RVA: 0x00007E28 File Offset: 0x00006028
	public override void StartTurn2()
	{
		base.StartTurn2();
		this.strategy.TurnFixRange += (int)this.KeyValue * (this.strategy.FireCount / (int)this.KeyValue2);
		this.strategy.Concrete.GenerateRange();
	}

	// Token: 0x06000227 RID: 551 RVA: 0x00007E78 File Offset: 0x00006078
	public override void EndTurn()
	{
		base.EndTurn();
		this.strategy.Concrete.GenerateRange();
	}
}
