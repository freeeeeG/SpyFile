using System;

// Token: 0x02000258 RID: 600
public class GainGold : GuideEvent
{
	// Token: 0x06000F17 RID: 3863 RVA: 0x00027DF0 File Offset: 0x00025FF0
	public override void Trigger()
	{
		GameRes.Coin += this.Amount;
	}

	// Token: 0x04000780 RID: 1920
	public int Amount;
}
