using System;

// Token: 0x02000257 RID: 599
public class ShowGuideGirl : GuideEvent
{
	// Token: 0x06000F15 RID: 3861 RVA: 0x00027DD0 File Offset: 0x00025FD0
	public override void Trigger()
	{
		Singleton<GuideGirlSystem>.Instance.ShowGuideGirl(this.Show, this.PosID);
	}

	// Token: 0x0400077E RID: 1918
	public bool Show;

	// Token: 0x0400077F RID: 1919
	public int PosID;
}
