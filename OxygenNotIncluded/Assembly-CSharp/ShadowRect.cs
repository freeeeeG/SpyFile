using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BEE RID: 3054
public class ShadowRect : MonoBehaviour
{
	// Token: 0x0600608E RID: 24718 RVA: 0x0023AE30 File Offset: 0x00239030
	private void OnEnable()
	{
		if (this.RectShadow != null)
		{
			this.RectShadow.name = "Shadow_" + this.RectMain.name;
			this.MatchRect();
			return;
		}
		global::Debug.LogWarning("Shadowrect is missing rectshadow: " + base.gameObject.name);
	}

	// Token: 0x0600608F RID: 24719 RVA: 0x0023AE8C File Offset: 0x0023908C
	private void Update()
	{
		this.MatchRect();
	}

	// Token: 0x06006090 RID: 24720 RVA: 0x0023AE94 File Offset: 0x00239094
	protected virtual void MatchRect()
	{
		if (this.RectShadow == null || this.RectMain == null)
		{
			return;
		}
		if (this.shadowLayoutElement == null)
		{
			this.shadowLayoutElement = this.RectShadow.GetComponent<LayoutElement>();
		}
		if (this.shadowLayoutElement != null && !this.shadowLayoutElement.ignoreLayout)
		{
			this.shadowLayoutElement.ignoreLayout = true;
		}
		if (this.RectShadow.transform.parent != this.RectMain.transform.parent)
		{
			this.RectShadow.transform.SetParent(this.RectMain.transform.parent);
		}
		if (this.RectShadow.GetSiblingIndex() >= this.RectMain.GetSiblingIndex())
		{
			this.RectShadow.SetAsFirstSibling();
		}
		this.RectShadow.transform.localScale = Vector3.one;
		if (this.RectShadow.pivot != this.RectMain.pivot)
		{
			this.RectShadow.pivot = this.RectMain.pivot;
		}
		if (this.RectShadow.anchorMax != this.RectMain.anchorMax)
		{
			this.RectShadow.anchorMax = this.RectMain.anchorMax;
		}
		if (this.RectShadow.anchorMin != this.RectMain.anchorMin)
		{
			this.RectShadow.anchorMin = this.RectMain.anchorMin;
		}
		if (this.RectShadow.sizeDelta != this.RectMain.sizeDelta)
		{
			this.RectShadow.sizeDelta = this.RectMain.sizeDelta;
		}
		if (this.RectShadow.anchoredPosition != this.RectMain.anchoredPosition + this.ShadowOffset)
		{
			this.RectShadow.anchoredPosition = this.RectMain.anchoredPosition + this.ShadowOffset;
		}
		if (this.RectMain.gameObject.activeInHierarchy != this.RectShadow.gameObject.activeInHierarchy)
		{
			this.RectShadow.gameObject.SetActive(this.RectMain.gameObject.activeInHierarchy);
		}
	}

	// Token: 0x040041C5 RID: 16837
	public RectTransform RectMain;

	// Token: 0x040041C6 RID: 16838
	public RectTransform RectShadow;

	// Token: 0x040041C7 RID: 16839
	[SerializeField]
	protected Color shadowColor = new Color(0f, 0f, 0f, 0.6f);

	// Token: 0x040041C8 RID: 16840
	[SerializeField]
	protected Vector2 ShadowOffset = new Vector2(1.5f, -1.5f);

	// Token: 0x040041C9 RID: 16841
	private LayoutElement shadowLayoutElement;
}
