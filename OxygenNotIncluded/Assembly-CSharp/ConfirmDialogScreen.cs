using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000ADE RID: 2782
public class ConfirmDialogScreen : KModalScreen
{
	// Token: 0x060055AF RID: 21935 RVA: 0x001F2D9D File Offset: 0x001F0F9D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
	}

	// Token: 0x060055B0 RID: 21936 RVA: 0x001F2DB1 File Offset: 0x001F0FB1
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x060055B1 RID: 21937 RVA: 0x001F2DB4 File Offset: 0x001F0FB4
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.OnSelect_CANCEL();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060055B2 RID: 21938 RVA: 0x001F2DD0 File Offset: 0x001F0FD0
	public void PopupConfirmDialog(string text, System.Action on_confirm, System.Action on_cancel, string configurable_text = null, System.Action on_configurable_clicked = null, string title_text = null, string confirm_text = null, string cancel_text = null, Sprite image_sprite = null)
	{
		while (base.transform.parent.GetComponent<Canvas>() == null && base.transform.parent.parent != null)
		{
			base.transform.SetParent(base.transform.parent.parent);
		}
		base.transform.SetAsLastSibling();
		this.confirmAction = on_confirm;
		this.cancelAction = on_cancel;
		this.configurableAction = on_configurable_clicked;
		int num = 0;
		if (this.confirmAction != null)
		{
			num++;
		}
		if (this.cancelAction != null)
		{
			num++;
		}
		if (this.configurableAction != null)
		{
			num++;
		}
		this.confirmButton.GetComponentInChildren<LocText>().text = ((confirm_text == null) ? UI.CONFIRMDIALOG.OK.text : confirm_text);
		this.cancelButton.GetComponentInChildren<LocText>().text = ((cancel_text == null) ? UI.CONFIRMDIALOG.CANCEL.text : cancel_text);
		this.confirmButton.GetComponent<KButton>().onClick += this.OnSelect_OK;
		this.cancelButton.GetComponent<KButton>().onClick += this.OnSelect_CANCEL;
		this.configurableButton.GetComponent<KButton>().onClick += this.OnSelect_third;
		this.cancelButton.SetActive(on_cancel != null);
		if (this.configurableButton != null)
		{
			this.configurableButton.SetActive(this.configurableAction != null);
			if (configurable_text != null)
			{
				this.configurableButton.GetComponentInChildren<LocText>().text = configurable_text;
			}
		}
		if (image_sprite != null)
		{
			this.image.sprite = image_sprite;
			this.image.gameObject.SetActive(true);
		}
		if (title_text != null)
		{
			this.titleText.key = "";
			this.titleText.text = title_text;
		}
		this.popupMessage.text = text;
	}

	// Token: 0x060055B3 RID: 21939 RVA: 0x001F2FA5 File Offset: 0x001F11A5
	public void OnSelect_OK()
	{
		if (this.deactivateOnConfirmAction)
		{
			this.Deactivate();
		}
		if (this.confirmAction != null)
		{
			this.confirmAction();
		}
	}

	// Token: 0x060055B4 RID: 21940 RVA: 0x001F2FC8 File Offset: 0x001F11C8
	public void OnSelect_CANCEL()
	{
		if (this.deactivateOnCancelAction)
		{
			this.Deactivate();
		}
		if (this.cancelAction != null)
		{
			this.cancelAction();
		}
	}

	// Token: 0x060055B5 RID: 21941 RVA: 0x001F2FEB File Offset: 0x001F11EB
	public void OnSelect_third()
	{
		if (this.deactivateOnConfigurableAction)
		{
			this.Deactivate();
		}
		if (this.configurableAction != null)
		{
			this.configurableAction();
		}
	}

	// Token: 0x060055B6 RID: 21942 RVA: 0x001F300E File Offset: 0x001F120E
	protected override void OnDeactivate()
	{
		if (this.onDeactivateCB != null)
		{
			this.onDeactivateCB();
		}
		base.OnDeactivate();
	}

	// Token: 0x04003972 RID: 14706
	private System.Action confirmAction;

	// Token: 0x04003973 RID: 14707
	private System.Action cancelAction;

	// Token: 0x04003974 RID: 14708
	private System.Action configurableAction;

	// Token: 0x04003975 RID: 14709
	public bool deactivateOnConfigurableAction = true;

	// Token: 0x04003976 RID: 14710
	public bool deactivateOnConfirmAction = true;

	// Token: 0x04003977 RID: 14711
	public bool deactivateOnCancelAction = true;

	// Token: 0x04003978 RID: 14712
	public System.Action onDeactivateCB;

	// Token: 0x04003979 RID: 14713
	[SerializeField]
	private GameObject confirmButton;

	// Token: 0x0400397A RID: 14714
	[SerializeField]
	private GameObject cancelButton;

	// Token: 0x0400397B RID: 14715
	[SerializeField]
	private GameObject configurableButton;

	// Token: 0x0400397C RID: 14716
	[SerializeField]
	private LocText titleText;

	// Token: 0x0400397D RID: 14717
	[SerializeField]
	private LocText popupMessage;

	// Token: 0x0400397E RID: 14718
	[SerializeField]
	private Image image;
}
