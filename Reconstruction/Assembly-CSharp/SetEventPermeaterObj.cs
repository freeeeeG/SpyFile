using System;

// Token: 0x02000250 RID: 592
public class SetEventPermeaterObj : GuideEvent
{
	// Token: 0x06000F07 RID: 3847 RVA: 0x00027CDC File Offset: 0x00025EDC
	public override void Trigger()
	{
		Singleton<GuideGirlSystem>.Instance.SetEventPermeaterTarget(this.Show ? Singleton<GuideGirlSystem>.Instance.GetGuideObj(this.ObjName) : null);
	}

	// Token: 0x04000773 RID: 1907
	public bool Show;

	// Token: 0x04000774 RID: 1908
	public string ObjName;
}
