using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C6F RID: 3183
public class SpriteListDialogScreen : KModalScreen
{
	// Token: 0x06006559 RID: 25945 RVA: 0x0025A5BB File Offset: 0x002587BB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
		this.buttons = new List<SpriteListDialogScreen.Button>();
	}

	// Token: 0x0600655A RID: 25946 RVA: 0x0025A5DA File Offset: 0x002587DA
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x0600655B RID: 25947 RVA: 0x0025A5DD File Offset: 0x002587DD
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600655C RID: 25948 RVA: 0x0025A5F8 File Offset: 0x002587F8
	public void AddOption(string text, System.Action action)
	{
		GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, this.buttonPanel, true);
		this.buttons.Add(new SpriteListDialogScreen.Button
		{
			label = text,
			action = action,
			gameObject = gameObject
		});
	}

	// Token: 0x0600655D RID: 25949 RVA: 0x0025A644 File Offset: 0x00258844
	public void AddListRow(Sprite sprite, string text, float width = -1f, float height = -1f)
	{
		GameObject gameObject = Util.KInstantiateUI(this.listPrefab, this.listPanel, true);
		gameObject.GetComponentInChildren<LocText>().text = text;
		Image componentInChildren = gameObject.GetComponentInChildren<Image>();
		componentInChildren.sprite = sprite;
		if (sprite == null)
		{
			Color color = componentInChildren.color;
			color.a = 0f;
			componentInChildren.color = color;
		}
		if (width >= 0f || height >= 0f)
		{
			componentInChildren.GetComponent<AspectRatioFitter>().enabled = false;
			LayoutElement component = componentInChildren.GetComponent<LayoutElement>();
			component.minWidth = width;
			component.preferredWidth = width;
			component.minHeight = height;
			component.preferredHeight = height;
			return;
		}
		AspectRatioFitter component2 = componentInChildren.GetComponent<AspectRatioFitter>();
		float aspectRatio = (sprite == null) ? 1f : (sprite.rect.width / sprite.rect.height);
		component2.aspectRatio = aspectRatio;
	}

	// Token: 0x0600655E RID: 25950 RVA: 0x0025A71C File Offset: 0x0025891C
	public void PopupConfirmDialog(string text, string title_text = null)
	{
		foreach (SpriteListDialogScreen.Button button in this.buttons)
		{
			button.gameObject.GetComponentInChildren<LocText>().text = button.label;
			button.gameObject.GetComponent<KButton>().onClick += button.action;
		}
		if (title_text != null)
		{
			this.titleText.text = title_text;
		}
		this.popupMessage.text = text;
	}

	// Token: 0x0600655F RID: 25951 RVA: 0x0025A7B0 File Offset: 0x002589B0
	protected override void OnDeactivate()
	{
		if (this.onDeactivateCB != null)
		{
			this.onDeactivateCB();
		}
		base.OnDeactivate();
	}

	// Token: 0x0400458A RID: 17802
	public System.Action onDeactivateCB;

	// Token: 0x0400458B RID: 17803
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x0400458C RID: 17804
	[SerializeField]
	private GameObject buttonPanel;

	// Token: 0x0400458D RID: 17805
	[SerializeField]
	private LocText titleText;

	// Token: 0x0400458E RID: 17806
	[SerializeField]
	private LocText popupMessage;

	// Token: 0x0400458F RID: 17807
	[SerializeField]
	private GameObject listPanel;

	// Token: 0x04004590 RID: 17808
	[SerializeField]
	private GameObject listPrefab;

	// Token: 0x04004591 RID: 17809
	private List<SpriteListDialogScreen.Button> buttons;

	// Token: 0x02001BA3 RID: 7075
	private struct Button
	{
		// Token: 0x04007D56 RID: 32086
		public System.Action action;

		// Token: 0x04007D57 RID: 32087
		public GameObject gameObject;

		// Token: 0x04007D58 RID: 32088
		public string label;
	}
}
