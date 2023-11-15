using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000B7D RID: 2941
public class GenericMessage : Message
{
	// Token: 0x06005B57 RID: 23383 RVA: 0x00218CD2 File Offset: 0x00216ED2
	public GenericMessage(string _title, string _body, string _tooltip, KMonoBehaviour click_focus = null)
	{
		this.title = _title;
		this.body = _body;
		this.tooltip = _tooltip;
		this.clickFocus.Set(click_focus);
	}

	// Token: 0x06005B58 RID: 23384 RVA: 0x00218D07 File Offset: 0x00216F07
	public GenericMessage()
	{
	}

	// Token: 0x06005B59 RID: 23385 RVA: 0x00218D1A File Offset: 0x00216F1A
	public override string GetSound()
	{
		return null;
	}

	// Token: 0x06005B5A RID: 23386 RVA: 0x00218D1D File Offset: 0x00216F1D
	public override string GetMessageBody()
	{
		return this.body;
	}

	// Token: 0x06005B5B RID: 23387 RVA: 0x00218D25 File Offset: 0x00216F25
	public override string GetTooltip()
	{
		return this.tooltip;
	}

	// Token: 0x06005B5C RID: 23388 RVA: 0x00218D2D File Offset: 0x00216F2D
	public override string GetTitle()
	{
		return this.title;
	}

	// Token: 0x06005B5D RID: 23389 RVA: 0x00218D38 File Offset: 0x00216F38
	public override void OnClick()
	{
		KMonoBehaviour kmonoBehaviour = this.clickFocus.Get();
		if (kmonoBehaviour == null)
		{
			return;
		}
		Transform transform = kmonoBehaviour.transform;
		if (transform == null)
		{
			return;
		}
		Vector3 position = transform.GetPosition();
		position.z = -40f;
		CameraController.Instance.SetTargetPos(position, 8f, true);
		if (transform.GetComponent<KSelectable>() != null)
		{
			SelectTool.Instance.Select(transform.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x04003DA3 RID: 15779
	[Serialize]
	private string title;

	// Token: 0x04003DA4 RID: 15780
	[Serialize]
	private string tooltip;

	// Token: 0x04003DA5 RID: 15781
	[Serialize]
	private string body;

	// Token: 0x04003DA6 RID: 15782
	[Serialize]
	private Ref<KMonoBehaviour> clickFocus = new Ref<KMonoBehaviour>();
}
