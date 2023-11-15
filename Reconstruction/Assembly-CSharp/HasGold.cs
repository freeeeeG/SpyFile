using System;

// Token: 0x02000248 RID: 584
public class HasGold : GuideCondition
{
	// Token: 0x06000EF7 RID: 3831 RVA: 0x00027B16 File Offset: 0x00025D16
	public override bool Judge()
	{
		return GameRes.Coin >= this.Amount;
	}

	// Token: 0x04000762 RID: 1890
	public int Amount;
}
