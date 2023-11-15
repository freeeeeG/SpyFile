using System;

// Token: 0x020004C6 RID: 1222
public class KSelectableHealthBar : KSelectable
{
	// Token: 0x06001BE6 RID: 7142 RVA: 0x00094CA8 File Offset: 0x00092EA8
	public override string GetName()
	{
		int num = (int)(this.progressBar.PercentFull * (float)this.scaleAmount);
		return string.Format("{0} {1}/{2}", this.entityName, num, this.scaleAmount);
	}

	// Token: 0x04000F6E RID: 3950
	[MyCmpGet]
	private ProgressBar progressBar;

	// Token: 0x04000F6F RID: 3951
	private int scaleAmount = 100;
}
