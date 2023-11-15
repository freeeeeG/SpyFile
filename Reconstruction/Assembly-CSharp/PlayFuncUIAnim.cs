using System;

// Token: 0x02000252 RID: 594
public class PlayFuncUIAnim : GuideEvent
{
	// Token: 0x06000F0B RID: 3851 RVA: 0x00027D4F File Offset: 0x00025F4F
	public override void Trigger()
	{
		FuncUI.PlayFuncUIAnim(this.partID, this.key, this.value);
	}

	// Token: 0x04000777 RID: 1911
	public int partID;

	// Token: 0x04000778 RID: 1912
	public string key;

	// Token: 0x04000779 RID: 1913
	public bool value;
}
