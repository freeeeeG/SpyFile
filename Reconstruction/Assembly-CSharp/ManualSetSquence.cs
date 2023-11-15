using System;

// Token: 0x0200024C RID: 588
public class ManualSetSquence : GuideEvent
{
	// Token: 0x06000EFF RID: 3839 RVA: 0x00027C26 File Offset: 0x00025E26
	public override void Trigger()
	{
		Singleton<GameManager>.Instance.ManualSetSequence(this.EnemyType, this.Stage, this.Wave);
	}

	// Token: 0x04000767 RID: 1895
	public EnemyType EnemyType;

	// Token: 0x04000768 RID: 1896
	public float Stage;

	// Token: 0x04000769 RID: 1897
	public int Wave;
}
