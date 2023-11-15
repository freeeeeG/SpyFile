using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000C78 RID: 3192
public class TextLinkHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x060065D3 RID: 26067 RVA: 0x0025FB68 File Offset: 0x0025DD68
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		if (!this.text.AllowLinks)
		{
			return;
		}
		int num = TMP_TextUtilities.FindIntersectingLink(this.text, KInputManager.GetMousePos(), null);
		if (num != -1)
		{
			string text = CodexCache.FormatLinkID(this.text.textInfo.linkInfo[num].GetLinkID());
			if (this.overrideLinkAction == null || this.overrideLinkAction(text))
			{
				if (!CodexCache.entries.ContainsKey(text))
				{
					SubEntry subEntry = CodexCache.FindSubEntry(text);
					if (subEntry == null || subEntry.disabled)
					{
						text = "PAGENOTFOUND";
					}
				}
				else if (CodexCache.entries[text].disabled)
				{
					text = "PAGENOTFOUND";
				}
				if (!ManagementMenu.Instance.codexScreen.gameObject.activeInHierarchy)
				{
					ManagementMenu.Instance.ToggleCodex();
				}
				ManagementMenu.Instance.codexScreen.ChangeArticle(text, true, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
			}
		}
	}

	// Token: 0x060065D4 RID: 26068 RVA: 0x0025FC56 File Offset: 0x0025DE56
	private void Update()
	{
		this.CheckMouseOver();
		if (TextLinkHandler.hoveredText == this && this.text.AllowLinks)
		{
			PlayerController.Instance.ActiveTool.SetLinkCursor(this.hoverLink);
		}
	}

	// Token: 0x060065D5 RID: 26069 RVA: 0x0025FC8D File Offset: 0x0025DE8D
	private void OnEnable()
	{
		this.CheckMouseOver();
	}

	// Token: 0x060065D6 RID: 26070 RVA: 0x0025FC95 File Offset: 0x0025DE95
	private void OnDisable()
	{
		this.ClearState();
	}

	// Token: 0x060065D7 RID: 26071 RVA: 0x0025FC9D File Offset: 0x0025DE9D
	private void Awake()
	{
		this.text = base.GetComponent<LocText>();
		if (this.text.AllowLinks && !this.text.raycastTarget)
		{
			this.text.raycastTarget = true;
		}
	}

	// Token: 0x060065D8 RID: 26072 RVA: 0x0025FCD1 File Offset: 0x0025DED1
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.SetMouseOver();
	}

	// Token: 0x060065D9 RID: 26073 RVA: 0x0025FCD9 File Offset: 0x0025DED9
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ClearState();
	}

	// Token: 0x060065DA RID: 26074 RVA: 0x0025FCE4 File Offset: 0x0025DEE4
	private void ClearState()
	{
		if (this == null || this.Equals(null))
		{
			return;
		}
		if (TextLinkHandler.hoveredText == this)
		{
			if (this.hoverLink && PlayerController.Instance != null && PlayerController.Instance.ActiveTool != null)
			{
				PlayerController.Instance.ActiveTool.SetLinkCursor(false);
			}
			TextLinkHandler.hoveredText = null;
			this.hoverLink = false;
		}
	}

	// Token: 0x060065DB RID: 26075 RVA: 0x0025FD58 File Offset: 0x0025DF58
	public void CheckMouseOver()
	{
		if (this.text == null)
		{
			return;
		}
		if (TMP_TextUtilities.FindIntersectingLink(this.text, KInputManager.GetMousePos(), null) != -1)
		{
			this.SetMouseOver();
			this.hoverLink = true;
			return;
		}
		if (TextLinkHandler.hoveredText == this)
		{
			this.hoverLink = false;
		}
	}

	// Token: 0x060065DC RID: 26076 RVA: 0x0025FDAA File Offset: 0x0025DFAA
	private void SetMouseOver()
	{
		if (TextLinkHandler.hoveredText != null && TextLinkHandler.hoveredText != this)
		{
			TextLinkHandler.hoveredText.hoverLink = false;
		}
		TextLinkHandler.hoveredText = this;
	}

	// Token: 0x04004613 RID: 17939
	private static TextLinkHandler hoveredText;

	// Token: 0x04004614 RID: 17940
	[MyCmpGet]
	private LocText text;

	// Token: 0x04004615 RID: 17941
	private bool hoverLink;

	// Token: 0x04004616 RID: 17942
	public Func<string, bool> overrideLinkAction;
}
