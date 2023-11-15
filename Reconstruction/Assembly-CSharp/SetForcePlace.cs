using System;

// Token: 0x02000254 RID: 596
public class SetForcePlace : GuideEvent
{
	// Token: 0x06000F0F RID: 3855 RVA: 0x00027D8C File Offset: 0x00025F8C
	public override void Trigger()
	{
		GameRes.ForcePlace = this.ForcePlace;
	}

	// Token: 0x0400077C RID: 1916
	public ForcePlace ForcePlace;
}
