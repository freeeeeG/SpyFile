using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B20 RID: 2848
public class InfoDialogScreen : KModalScreen
{
	// Token: 0x060057A0 RID: 22432 RVA: 0x00201062 File Offset: 0x001FF262
	public InfoScreenPlainText GetSubHeaderPrefab()
	{
		return this.subHeaderTemplate;
	}

	// Token: 0x060057A1 RID: 22433 RVA: 0x0020106A File Offset: 0x001FF26A
	public InfoScreenPlainText GetPlainTextPrefab()
	{
		return this.plainTextTemplate;
	}

	// Token: 0x060057A2 RID: 22434 RVA: 0x00201072 File Offset: 0x001FF272
	public InfoScreenLineItem GetLineItemPrefab()
	{
		return this.lineItemTemplate;
	}

	// Token: 0x060057A3 RID: 22435 RVA: 0x0020107A File Offset: 0x001FF27A
	public GameObject GetPrimaryButtonPrefab()
	{
		return this.leftButtonPrefab;
	}

	// Token: 0x060057A4 RID: 22436 RVA: 0x00201082 File Offset: 0x001FF282
	public GameObject GetSecondaryButtonPrefab()
	{
		return this.rightButtonPrefab;
	}

	// Token: 0x060057A5 RID: 22437 RVA: 0x0020108A File Offset: 0x001FF28A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
	}

	// Token: 0x060057A6 RID: 22438 RVA: 0x0020109E File Offset: 0x001FF29E
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x060057A7 RID: 22439 RVA: 0x002010A4 File Offset: 0x001FF2A4
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!this.escapeCloses)
		{
			e.TryConsume(global::Action.Escape);
			return;
		}
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
			return;
		}
		if (PlayerController.Instance != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060057A8 RID: 22440 RVA: 0x002010FB File Offset: 0x001FF2FB
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show && this.onDeactivateFn != null)
		{
			this.onDeactivateFn();
		}
	}

	// Token: 0x060057A9 RID: 22441 RVA: 0x0020111A File Offset: 0x001FF31A
	public InfoDialogScreen AddDefaultOK(bool escapeCloses = false)
	{
		this.AddOption(UI.CONFIRMDIALOG.OK, delegate(InfoDialogScreen d)
		{
			d.Deactivate();
		}, true);
		this.escapeCloses = escapeCloses;
		return this;
	}

	// Token: 0x060057AA RID: 22442 RVA: 0x00201155 File Offset: 0x001FF355
	public InfoDialogScreen AddDefaultCancel()
	{
		this.AddOption(UI.CONFIRMDIALOG.CANCEL, delegate(InfoDialogScreen d)
		{
			d.Deactivate();
		}, false);
		this.escapeCloses = true;
		return this;
	}

	// Token: 0x060057AB RID: 22443 RVA: 0x00201190 File Offset: 0x001FF390
	public InfoDialogScreen AddOption(string text, Action<InfoDialogScreen> action, bool rightSide = false)
	{
		GameObject gameObject = Util.KInstantiateUI(rightSide ? this.rightButtonPrefab : this.leftButtonPrefab, rightSide ? this.rightButtonPanel : this.leftButtonPanel, true);
		gameObject.gameObject.GetComponentInChildren<LocText>().text = text;
		gameObject.gameObject.GetComponent<KButton>().onClick += delegate()
		{
			action(this);
		};
		return this;
	}

	// Token: 0x060057AC RID: 22444 RVA: 0x00201208 File Offset: 0x001FF408
	public InfoDialogScreen AddOption(bool rightSide, out KButton button, out LocText buttonText)
	{
		GameObject gameObject = Util.KInstantiateUI(rightSide ? this.rightButtonPrefab : this.leftButtonPrefab, rightSide ? this.rightButtonPanel : this.leftButtonPanel, true);
		button = gameObject.GetComponent<KButton>();
		buttonText = gameObject.GetComponentInChildren<LocText>();
		return this;
	}

	// Token: 0x060057AD RID: 22445 RVA: 0x0020124F File Offset: 0x001FF44F
	public InfoDialogScreen SetHeader(string header)
	{
		this.header.text = header;
		return this;
	}

	// Token: 0x060057AE RID: 22446 RVA: 0x0020125E File Offset: 0x001FF45E
	public InfoDialogScreen AddSprite(Sprite sprite)
	{
		Util.KInstantiateUI<InfoScreenSpriteItem>(this.spriteItemTemplate.gameObject, this.contentContainer, false).SetSprite(sprite);
		return this;
	}

	// Token: 0x060057AF RID: 22447 RVA: 0x0020127E File Offset: 0x001FF47E
	public InfoDialogScreen AddPlainText(string text)
	{
		Util.KInstantiateUI<InfoScreenPlainText>(this.plainTextTemplate.gameObject, this.contentContainer, false).SetText(text);
		return this;
	}

	// Token: 0x060057B0 RID: 22448 RVA: 0x0020129E File Offset: 0x001FF49E
	public InfoDialogScreen AddLineItem(string text, string tooltip)
	{
		InfoScreenLineItem infoScreenLineItem = Util.KInstantiateUI<InfoScreenLineItem>(this.lineItemTemplate.gameObject, this.contentContainer, false);
		infoScreenLineItem.SetText(text);
		infoScreenLineItem.SetTooltip(tooltip);
		return this;
	}

	// Token: 0x060057B1 RID: 22449 RVA: 0x002012C5 File Offset: 0x001FF4C5
	public InfoDialogScreen AddSubHeader(string text)
	{
		Util.KInstantiateUI<InfoScreenPlainText>(this.subHeaderTemplate.gameObject, this.contentContainer, false).SetText(text);
		return this;
	}

	// Token: 0x060057B2 RID: 22450 RVA: 0x002012E8 File Offset: 0x001FF4E8
	public InfoDialogScreen AddSpacer(float height)
	{
		GameObject gameObject = new GameObject("spacer");
		gameObject.SetActive(false);
		gameObject.transform.SetParent(this.contentContainer.transform, false);
		LayoutElement layoutElement = gameObject.AddComponent<LayoutElement>();
		layoutElement.minHeight = height;
		layoutElement.preferredHeight = height;
		layoutElement.flexibleHeight = 0f;
		gameObject.SetActive(true);
		return this;
	}

	// Token: 0x060057B3 RID: 22451 RVA: 0x00201342 File Offset: 0x001FF542
	public InfoDialogScreen AddUI<T>(T prefab, out T spawn) where T : MonoBehaviour
	{
		spawn = Util.KInstantiateUI<T>(prefab.gameObject, this.contentContainer, true);
		return this;
	}

	// Token: 0x060057B4 RID: 22452 RVA: 0x00201364 File Offset: 0x001FF564
	public InfoDialogScreen AddDescriptors(List<Descriptor> descriptors)
	{
		for (int i = 0; i < descriptors.Count; i++)
		{
			this.AddLineItem(descriptors[i].IndentedText(), descriptors[i].tooltipText);
		}
		return this;
	}

	// Token: 0x04003B3E RID: 15166
	[SerializeField]
	private InfoScreenPlainText subHeaderTemplate;

	// Token: 0x04003B3F RID: 15167
	[SerializeField]
	private InfoScreenPlainText plainTextTemplate;

	// Token: 0x04003B40 RID: 15168
	[SerializeField]
	private InfoScreenLineItem lineItemTemplate;

	// Token: 0x04003B41 RID: 15169
	[SerializeField]
	private InfoScreenSpriteItem spriteItemTemplate;

	// Token: 0x04003B42 RID: 15170
	[Space(10f)]
	[SerializeField]
	private LocText header;

	// Token: 0x04003B43 RID: 15171
	[SerializeField]
	private GameObject contentContainer;

	// Token: 0x04003B44 RID: 15172
	[SerializeField]
	private GameObject leftButtonPrefab;

	// Token: 0x04003B45 RID: 15173
	[SerializeField]
	private GameObject rightButtonPrefab;

	// Token: 0x04003B46 RID: 15174
	[SerializeField]
	private GameObject leftButtonPanel;

	// Token: 0x04003B47 RID: 15175
	[SerializeField]
	private GameObject rightButtonPanel;

	// Token: 0x04003B48 RID: 15176
	private bool escapeCloses;

	// Token: 0x04003B49 RID: 15177
	public System.Action onDeactivateFn;
}
