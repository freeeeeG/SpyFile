using System;
using UnityEngine;

// Token: 0x02000B8A RID: 2954
public class TargetMessageDialog : MessageDialog
{
	// Token: 0x06005BB6 RID: 23478 RVA: 0x002199CB File Offset: 0x00217BCB
	public override bool CanDisplay(Message message)
	{
		return typeof(TargetMessage).IsAssignableFrom(message.GetType());
	}

	// Token: 0x06005BB7 RID: 23479 RVA: 0x002199E2 File Offset: 0x00217BE2
	public override void SetMessage(Message base_message)
	{
		this.message = (TargetMessage)base_message;
		this.description.text = this.message.GetMessageBody();
	}

	// Token: 0x06005BB8 RID: 23480 RVA: 0x00219A08 File Offset: 0x00217C08
	public override void OnClickAction()
	{
		MessageTarget target = this.message.GetTarget();
		SelectTool.Instance.SelectAndFocus(target.GetPosition(), target.GetSelectable());
	}

	// Token: 0x06005BB9 RID: 23481 RVA: 0x00219A37 File Offset: 0x00217C37
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.message.OnCleanUp();
	}

	// Token: 0x04003DC4 RID: 15812
	[SerializeField]
	private LocText description;

	// Token: 0x04003DC5 RID: 15813
	private TargetMessage message;
}
