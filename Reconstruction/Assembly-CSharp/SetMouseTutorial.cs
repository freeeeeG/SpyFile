using System;

// Token: 0x0200024D RID: 589
public class SetMouseTutorial : GuideEvent
{
	// Token: 0x06000F01 RID: 3841 RVA: 0x00027C4C File Offset: 0x00025E4C
	public override void Trigger()
	{
		Singleton<GameManager>.Instance.SetCamMovable(this.MoveAble);
		Singleton<GameManager>.Instance.SetMoveTutorial(this.MoveTurorial);
		Singleton<GameManager>.Instance.SetSizeTutorial(this.SizeTutorial);
	}

	// Token: 0x0400076A RID: 1898
	public bool MoveAble;

	// Token: 0x0400076B RID: 1899
	public bool MoveTurorial;

	// Token: 0x0400076C RID: 1900
	public bool SizeTutorial;
}
