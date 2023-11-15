using System;

// Token: 0x0200025A RID: 602
public class RemoveBluePrint : GuideEvent
{
	// Token: 0x06000F1B RID: 3867 RVA: 0x00027E56 File Offset: 0x00026056
	public override void Trigger()
	{
		Singleton<GameManager>.Instance.RemoveBluePrint(this.ID);
	}

	// Token: 0x04000784 RID: 1924
	public int ID;
}
