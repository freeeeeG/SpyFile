using System;

// Token: 0x02000256 RID: 598
public class SetTutorial : GuideEvent
{
	// Token: 0x06000F13 RID: 3859 RVA: 0x00027DBB File Offset: 0x00025FBB
	public override void Trigger()
	{
		Singleton<Game>.Instance.Tutorial = false;
	}
}
