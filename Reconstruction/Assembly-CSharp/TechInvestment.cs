using System;

// Token: 0x0200019A RID: 410
public class TechInvestment : Technology
{
	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x06000A78 RID: 2680 RVA: 0x0001C0D1 File Offset: 0x0001A2D1
	public override bool Add
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x06000A79 RID: 2681 RVA: 0x0001C0D4 File Offset: 0x0001A2D4
	public override TechnologyName TechnologyName
	{
		get
		{
			return TechnologyName.TECHINVESTMENT;
		}
	}

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x06000A7A RID: 2682 RVA: 0x0001C0D7 File Offset: 0x0001A2D7
	public override float KeyValue
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0001C0DE File Offset: 0x0001A2DE
	public override float KeyValue2
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x06000A7C RID: 2684 RVA: 0x0001C0E5 File Offset: 0x0001A2E5
	private float keyValue3
	{
		get
		{
			return 35f;
		}
	}

	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x06000A7D RID: 2685 RVA: 0x0001C0EC File Offset: 0x0001A2EC
	public override string DisplayValue1
	{
		get
		{
			return this.KeyValue.ToString();
		}
	}

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x06000A7E RID: 2686 RVA: 0x0001C108 File Offset: 0x0001A308
	public override string DisplayValue2
	{
		get
		{
			return this.KeyValue2.ToString();
		}
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x06000A7F RID: 2687 RVA: 0x0001C124 File Offset: 0x0001A324
	public override string DisplayValue3
	{
		get
		{
			return this.keyValue3.ToString();
		}
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x06000A80 RID: 2688 RVA: 0x0001C13F File Offset: 0x0001A33F
	// (set) Token: 0x06000A81 RID: 2689 RVA: 0x0001C148 File Offset: 0x0001A348
	public override float SaveValue
	{
		get
		{
			return (float)this.refactorThisTurn;
		}
		set
		{
			this.refactorThisTurn = (int)value;
		}
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x0001C152 File Offset: 0x0001A352
	public override void OnGet()
	{
		base.OnGet();
		if (this.IsAbnormal)
		{
			GameRes.LockCount -= (int)this.KeyValue2;
		}
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x0001C174 File Offset: 0x0001A374
	public override void OnRefactor(StrategyBase strategy)
	{
		base.OnRefactor(strategy);
		if (this.IsAbnormal)
		{
			Singleton<GameManager>.Instance.GainMoney((int)this.keyValue3);
		}
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x0001C196 File Offset: 0x0001A396
	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		if (!this.IsAbnormal && GameRes.Coin < 100)
		{
			Singleton<GameManager>.Instance.GainMoney((int)this.KeyValue);
			GameRes.GainGoldBattleTurn += (int)this.KeyValue;
		}
	}

	// Token: 0x040005AA RID: 1450
	private int refactorThisTurn;
}
