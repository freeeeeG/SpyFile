using System;

// Token: 0x02000251 RID: 593
public class SetGuideIndicatorPos : GuideEvent
{
	// Token: 0x06000F09 RID: 3849 RVA: 0x00027D0B File Offset: 0x00025F0B
	public override void Trigger()
	{
		Singleton<GuideGirlSystem>.Instance.GetGuideObj("GuideIndicator").GetComponent<GuideIndicator>().Show(this.Show, this.Show ? Singleton<GuideGirlSystem>.Instance.GetGuideObj(this.ObjName) : null);
	}

	// Token: 0x04000775 RID: 1909
	public bool Show;

	// Token: 0x04000776 RID: 1910
	public string ObjName;
}
