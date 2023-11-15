using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000A61 RID: 2657
public class FileNameDialog : KModalScreen
{
	// Token: 0x06005057 RID: 20567 RVA: 0x001C7AD7 File Offset: 0x001C5CD7
	public override float GetSortKey()
	{
		return 150f;
	}

	// Token: 0x06005058 RID: 20568 RVA: 0x001C7ADE File Offset: 0x001C5CDE
	public void SetTextAndSelect(string text)
	{
		if (this.inputField == null)
		{
			return;
		}
		this.inputField.text = text;
		this.inputField.Select();
	}

	// Token: 0x06005059 RID: 20569 RVA: 0x001C7B08 File Offset: 0x001C5D08
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.confirmButton.onClick += this.OnConfirm;
		this.cancelButton.onClick += this.OnCancel;
		this.closeButton.onClick += this.OnCancel;
		this.inputField.onValueChanged.AddListener(delegate(string <p0>)
		{
			Util.ScrubInputField(this.inputField, false, false);
		});
		this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
	}

	// Token: 0x0600505A RID: 20570 RVA: 0x001C7B98 File Offset: 0x001C5D98
	protected override void OnActivate()
	{
		base.OnActivate();
		this.inputField.Select();
		this.inputField.ActivateInputField();
		CameraController.Instance.DisableUserCameraControl = true;
	}

	// Token: 0x0600505B RID: 20571 RVA: 0x001C7BC1 File Offset: 0x001C5DC1
	protected override void OnDeactivate()
	{
		CameraController.Instance.DisableUserCameraControl = false;
		base.OnDeactivate();
	}

	// Token: 0x0600505C RID: 20572 RVA: 0x001C7BD4 File Offset: 0x001C5DD4
	public void OnConfirm()
	{
		if (this.onConfirm != null && !string.IsNullOrEmpty(this.inputField.text))
		{
			string text = this.inputField.text;
			if (!text.EndsWith(".sav"))
			{
				text += ".sav";
			}
			this.onConfirm(text);
			this.Deactivate();
		}
	}

	// Token: 0x0600505D RID: 20573 RVA: 0x001C7C32 File Offset: 0x001C5E32
	private void OnEndEdit(string str)
	{
		if (Localization.HasDirtyWords(str))
		{
			this.inputField.text = "";
		}
	}

	// Token: 0x0600505E RID: 20574 RVA: 0x001C7C4C File Offset: 0x001C5E4C
	public void OnCancel()
	{
		if (this.onCancel != null)
		{
			this.onCancel();
		}
		this.Deactivate();
	}

	// Token: 0x0600505F RID: 20575 RVA: 0x001C7C67 File Offset: 0x001C5E67
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
		}
		else if (e.TryConsume(global::Action.DialogSubmit))
		{
			this.OnConfirm();
		}
		e.Consumed = true;
	}

	// Token: 0x06005060 RID: 20576 RVA: 0x001C7C94 File Offset: 0x001C5E94
	public override void OnKeyDown(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x0400349A RID: 13466
	public Action<string> onConfirm;

	// Token: 0x0400349B RID: 13467
	public System.Action onCancel;

	// Token: 0x0400349C RID: 13468
	[SerializeField]
	private KInputTextField inputField;

	// Token: 0x0400349D RID: 13469
	[SerializeField]
	private KButton confirmButton;

	// Token: 0x0400349E RID: 13470
	[SerializeField]
	private KButton cancelButton;

	// Token: 0x0400349F RID: 13471
	[SerializeField]
	private KButton closeButton;
}
