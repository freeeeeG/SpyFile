using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000B02 RID: 2818
public abstract class ScrollingListUIContainer : UISubElementContainer, IScrollingListUI
{
	// Token: 0x060038EF RID: 14575 RVA: 0x0010D419 File Offset: 0x0010B819
	protected ScrollingListEntry GetSelectedEntry()
	{
		return this.m_selectedText.RequireComponent<ScrollingListEntry>();
	}

	// Token: 0x060038F0 RID: 14576 RVA: 0x0010D426 File Offset: 0x0010B826
	protected ScrollingListEntry[] GetUnselectedEntries()
	{
		GameObject[] textElements = this.m_textElements;
		if (ScrollingListUIContainer.<>f__mg$cache0 == null)
		{
			ScrollingListUIContainer.<>f__mg$cache0 = new Converter<GameObject, ScrollingListEntry>(GameObjectUtils.RequireComponent<ScrollingListEntry>);
		}
		return textElements.ConvertAll(ScrollingListUIContainer.<>f__mg$cache0);
	}

	// Token: 0x060038F1 RID: 14577 RVA: 0x0010D450 File Offset: 0x0010B850
	public void SetNumberOfLines(int _numLines)
	{
		this.m_numLines = _numLines;
		while (this.m_selectionId >= this.m_anchorId + this.m_numLines)
		{
			this.SetAnchorElement(this.m_anchorId + 1);
		}
		base.RefreshSubElements();
	}

	// Token: 0x060038F2 RID: 14578 RVA: 0x0010D48A File Offset: 0x0010B88A
	protected void OnSetNames()
	{
		base.RefreshSubElements();
	}

	// Token: 0x060038F3 RID: 14579
	protected abstract ScrollingListUIContainer.NameData[] GetNameData();

	// Token: 0x060038F4 RID: 14580 RVA: 0x0010D492 File Offset: 0x0010B892
	public void MoveUp()
	{
		if (this.m_selectionId > 0)
		{
			this.SetSelection(this.m_selectionId - 1);
			if (this.m_selectionId < this.m_anchorId)
			{
				this.SetAnchorElement(this.m_anchorId - 1);
			}
		}
	}

	// Token: 0x060038F5 RID: 14581 RVA: 0x0010D4D0 File Offset: 0x0010B8D0
	public void MoveDown()
	{
		if (this.m_selectionId + 1 < this.GetNameData().Length)
		{
			this.SetSelection(this.m_selectionId + 1);
			if (this.m_selectionId >= this.m_anchorId + this.m_numLines)
			{
				this.SetAnchorElement(this.m_anchorId + 1);
			}
		}
	}

	// Token: 0x060038F6 RID: 14582 RVA: 0x0010D526 File Offset: 0x0010B926
	public int GetSelection()
	{
		return this.m_selectionId;
	}

	// Token: 0x060038F7 RID: 14583 RVA: 0x0010D530 File Offset: 0x0010B930
	protected override void OnCreateSubObjects(GameObject _container)
	{
		this.m_textPlane = base.CreateRect(_container, "TextPlane");
		float num = 1f / (float)this.m_numLines;
		ScrollingListUIContainer.NameData[] nameData = this.GetNameData();
		Array.Resize<GameObject>(ref this.m_textElements, nameData.Length);
		for (int i = 0; i < nameData.Length; i++)
		{
			GameObject gameObject = this.m_unselectedTextPrefab.InstantiateOnParent(this.m_textPlane, true);
			gameObject.hideFlags = HideFlags.NotEditable;
			ScrollingListEntry scrollingListEntry = gameObject.RequireComponent<ScrollingListEntry>();
			scrollingListEntry.SetNameData(nameData[i]);
			RectTransform rectTransform = gameObject.RequireComponent<RectTransform>();
			rectTransform.anchorMin = new Vector2(0f, 1f - (float)(i + 1) * num);
			rectTransform.anchorMax = new Vector2(1f, 1f - (float)i * num);
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.offsetMin = new Vector2(0f, 0f);
			rectTransform.offsetMax = new Vector2(0f, 0f);
			rectTransform.localScale = Vector2.one;
			this.m_textElements[i] = gameObject;
		}
		this.m_selectedText = this.m_selectedTextPrefab.InstantiateOnParent(this.m_textPlane, true);
		this.m_selectedText.hideFlags = HideFlags.NotEditable;
		this.SetSelection(this.m_selectionId);
		this.SetAnchorElement(0);
	}

	// Token: 0x060038F8 RID: 14584 RVA: 0x0010D688 File Offset: 0x0010BA88
	private void SetAnchorElement(int _anchor)
	{
		this.m_anchorId = _anchor;
		this.m_textPlane.anchorMin = new Vector2(0f, (float)this.m_anchorId / (float)this.m_numLines);
		this.m_textPlane.anchorMax = new Vector2(1f, 1f + (float)this.m_anchorId / (float)this.m_numLines);
		for (int i = 0; i < this.GetNameData().Length; i++)
		{
			this.m_textElements[i].SetActive(i >= this.m_anchorId && i < this.m_anchorId + this.m_numLines && i != this.m_selectionId);
		}
	}

	// Token: 0x060038F9 RID: 14585 RVA: 0x0010D740 File Offset: 0x0010BB40
	private void SetSelection(int _selectionId)
	{
		if (this.m_textElements.TryAtIndex(this.m_selectionId) != null)
		{
			this.m_textElements[this.m_selectionId].SetActive(true);
		}
		this.m_selectionId = _selectionId;
		if (this.m_textElements.TryAtIndex(this.m_selectionId) != null)
		{
			this.m_textElements[this.m_selectionId].SetActive(false);
			RectTransform rectTransform = this.m_textElements[this.m_selectionId].gameObject.RequireComponent<RectTransform>();
			RectTransform rectTransform2 = this.m_selectedText.gameObject.RequireComponent<RectTransform>();
			rectTransform2.anchorMin = rectTransform.anchorMin;
			rectTransform2.anchorMax = rectTransform.anchorMax;
			rectTransform2.pivot = rectTransform.pivot;
			rectTransform2.offsetMin = rectTransform.offsetMin;
			rectTransform2.offsetMax = rectTransform.offsetMax;
			rectTransform2.localScale = rectTransform.localScale;
			ScrollingListEntry scrollingListEntry = this.m_selectedText.RequireComponent<ScrollingListEntry>();
			scrollingListEntry.SetNameData(this.GetNameData()[this.m_selectionId]);
		}
	}

	// Token: 0x04002DB3 RID: 11699
	[SerializeField]
	private int m_numLines = 10;

	// Token: 0x04002DB4 RID: 11700
	[SerializeField]
	private GameObject m_unselectedTextPrefab;

	// Token: 0x04002DB5 RID: 11701
	[SerializeField]
	private GameObject m_selectedTextPrefab;

	// Token: 0x04002DB6 RID: 11702
	[SerializeField]
	[HideInInspector]
	private RectTransform m_textPlane;

	// Token: 0x04002DB7 RID: 11703
	[SerializeField]
	[HideInInspector]
	private GameObject[] m_textElements;

	// Token: 0x04002DB8 RID: 11704
	[SerializeField]
	[HideInInspector]
	private GameObject m_selectedText;

	// Token: 0x04002DB9 RID: 11705
	[SerializeField]
	private int m_selectionId;

	// Token: 0x04002DBA RID: 11706
	private int m_anchorId;

	// Token: 0x04002DBB RID: 11707
	[CompilerGenerated]
	private static Converter<GameObject, ScrollingListEntry> <>f__mg$cache0;

	// Token: 0x02000B03 RID: 2819
	[Serializable]
	public abstract class NameData
	{
	}
}
