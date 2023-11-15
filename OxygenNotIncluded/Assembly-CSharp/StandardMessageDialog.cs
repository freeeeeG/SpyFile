using System;
using UnityEngine;

// Token: 0x02000B88 RID: 2952
public class StandardMessageDialog : MessageDialog
{
	// Token: 0x06005BAE RID: 23470 RVA: 0x0021995A File Offset: 0x00217B5A
	public override bool CanDisplay(Message message)
	{
		return typeof(Message).IsAssignableFrom(message.GetType());
	}

	// Token: 0x06005BAF RID: 23471 RVA: 0x00219971 File Offset: 0x00217B71
	public override void SetMessage(Message base_message)
	{
		this.message = base_message;
		this.description.text = this.message.GetMessageBody();
	}

	// Token: 0x06005BB0 RID: 23472 RVA: 0x00219990 File Offset: 0x00217B90
	public override void OnClickAction()
	{
	}

	// Token: 0x04003DC1 RID: 15809
	[SerializeField]
	private LocText description;

	// Token: 0x04003DC2 RID: 15810
	private Message message;
}
