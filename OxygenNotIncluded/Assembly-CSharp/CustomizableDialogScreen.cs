using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AE3 RID: 2787
public class CustomizableDialogScreen : KModalScreen
{
	// Token: 0x060055CB RID: 21963 RVA: 0x001F34F7 File Offset: 0x001F16F7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
		this.buttons = new List<CustomizableDialogScreen.Button>();
	}

	// Token: 0x060055CC RID: 21964 RVA: 0x001F3516 File Offset: 0x001F1716
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x060055CD RID: 21965 RVA: 0x001F351C File Offset: 0x001F171C
	public void AddOption(string text, System.Action action)
	{
		GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, this.buttonPanel, true);
		this.buttons.Add(new CustomizableDialogScreen.Button
		{
			label = text,
			action = action,
			gameObject = gameObject
		});
	}

	// Token: 0x060055CE RID: 21966 RVA: 0x001F3568 File Offset: 0x001F1768
	public void PopupConfirmDialog(string text, string title_text = null, Sprite image_sprite = null)
	{
		foreach (CustomizableDialogScreen.Button button in this.buttons)
		{
			button.gameObject.GetComponentInChildren<LocText>().text = button.label;
			button.gameObject.GetComponent<KButton>().onClick += button.action;
		}
		if (image_sprite != null)
		{
			this.image.sprite = image_sprite;
			this.image.gameObject.SetActive(true);
		}
		if (title_text != null)
		{
			this.titleText.text = title_text;
		}
		this.popupMessage.text = text;
	}

	// Token: 0x060055CF RID: 21967 RVA: 0x001F3624 File Offset: 0x001F1824
	protected override void OnDeactivate()
	{
		if (this.onDeactivateCB != null)
		{
			this.onDeactivateCB();
		}
		base.OnDeactivate();
	}

	// Token: 0x0400398D RID: 14733
	public System.Action onDeactivateCB;

	// Token: 0x0400398E RID: 14734
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x0400398F RID: 14735
	[SerializeField]
	private GameObject buttonPanel;

	// Token: 0x04003990 RID: 14736
	[SerializeField]
	private LocText titleText;

	// Token: 0x04003991 RID: 14737
	[SerializeField]
	private LocText popupMessage;

	// Token: 0x04003992 RID: 14738
	[SerializeField]
	private Image image;

	// Token: 0x04003993 RID: 14739
	private List<CustomizableDialogScreen.Button> buttons;

	// Token: 0x020019FB RID: 6651
	private struct Button
	{
		// Token: 0x040077F1 RID: 30705
		public System.Action action;

		// Token: 0x040077F2 RID: 30706
		public GameObject gameObject;

		// Token: 0x040077F3 RID: 30707
		public string label;
	}
}
