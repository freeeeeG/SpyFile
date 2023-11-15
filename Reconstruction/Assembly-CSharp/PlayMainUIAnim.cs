using System;

// Token: 0x0200024E RID: 590
public class PlayMainUIAnim : GuideEvent
{
	// Token: 0x06000F03 RID: 3843 RVA: 0x00027C86 File Offset: 0x00025E86
	public override void Trigger()
	{
		MainUI.PlayMainUIAnim(this.partID, this.key, this.value);
	}

	// Token: 0x0400076D RID: 1901
	public int partID;

	// Token: 0x0400076E RID: 1902
	public string key;

	// Token: 0x0400076F RID: 1903
	public bool value;
}
