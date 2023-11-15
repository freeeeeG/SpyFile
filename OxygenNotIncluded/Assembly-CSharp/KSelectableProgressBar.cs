using System;

// Token: 0x020004C7 RID: 1223
public class KSelectableProgressBar : KSelectable
{
	// Token: 0x06001BE8 RID: 7144 RVA: 0x00094CFC File Offset: 0x00092EFC
	public override string GetName()
	{
		int num = (int)(this.progressBar.PercentFull * (float)this.scaleAmount);
		return string.Format("{0} {1}/{2}", this.entityName, num, this.scaleAmount);
	}

	// Token: 0x04000F70 RID: 3952
	[MyCmpGet]
	private ProgressBar progressBar;

	// Token: 0x04000F71 RID: 3953
	private int scaleAmount = 100;
}
