using System;

// Token: 0x02000255 RID: 597
public class ShowGuideBook : GuideEvent
{
	// Token: 0x06000F11 RID: 3857 RVA: 0x00027DA1 File Offset: 0x00025FA1
	public override void Trigger()
	{
		Singleton<GuideGirlSystem>.Instance.ShowGuideBook(this.PageID);
	}

	// Token: 0x0400077D RID: 1917
	public int PageID;
}
