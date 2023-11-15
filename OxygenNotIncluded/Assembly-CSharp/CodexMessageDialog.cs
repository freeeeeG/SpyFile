using System;
using UnityEngine;

// Token: 0x02000B77 RID: 2935
public class CodexMessageDialog : MessageDialog
{
	// Token: 0x06005B2C RID: 23340 RVA: 0x00218A82 File Offset: 0x00216C82
	public override bool CanDisplay(Message message)
	{
		return typeof(CodexUnlockedMessage).IsAssignableFrom(message.GetType());
	}

	// Token: 0x06005B2D RID: 23341 RVA: 0x00218A99 File Offset: 0x00216C99
	public override void SetMessage(Message base_message)
	{
		this.message = (CodexUnlockedMessage)base_message;
		this.description.text = this.message.GetMessageBody();
	}

	// Token: 0x06005B2E RID: 23342 RVA: 0x00218ABD File Offset: 0x00216CBD
	public override void OnClickAction()
	{
	}

	// Token: 0x06005B2F RID: 23343 RVA: 0x00218ABF File Offset: 0x00216CBF
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.message.OnCleanUp();
	}

	// Token: 0x04003D99 RID: 15769
	[SerializeField]
	private LocText description;

	// Token: 0x04003D9A RID: 15770
	private CodexUnlockedMessage message;
}
