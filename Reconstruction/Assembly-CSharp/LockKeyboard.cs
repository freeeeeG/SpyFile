using System;

// Token: 0x0200025B RID: 603
public class LockKeyboard : GuideEvent
{
	// Token: 0x06000F1D RID: 3869 RVA: 0x00027E70 File Offset: 0x00026070
	public override void Trigger()
	{
		StaticData.LockKeyboard = this.isLock;
	}

	// Token: 0x04000785 RID: 1925
	public bool isLock;
}
