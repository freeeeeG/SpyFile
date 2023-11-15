using System;

// Token: 0x0200024F RID: 591
public class RectMaskObj : GuideEvent
{
	// Token: 0x06000F05 RID: 3845 RVA: 0x00027CA7 File Offset: 0x00025EA7
	public override void Trigger()
	{
		Singleton<GuideGirlSystem>.Instance.SetRectMaskObj(this.Show ? Singleton<GuideGirlSystem>.Instance.GetGuideObj(this.RectName) : null, this.delayTime);
	}

	// Token: 0x04000770 RID: 1904
	public bool Show;

	// Token: 0x04000771 RID: 1905
	public string RectName;

	// Token: 0x04000772 RID: 1906
	public float delayTime;
}
