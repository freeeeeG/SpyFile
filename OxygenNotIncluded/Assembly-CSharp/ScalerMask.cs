using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000AF2 RID: 2802
[AddComponentMenu("KMonoBehaviour/scripts/ScalerMask")]
public class ScalerMask : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x1700065F RID: 1631
	// (get) Token: 0x0600564F RID: 22095 RVA: 0x001F6F66 File Offset: 0x001F5166
	private RectTransform ThisTransform
	{
		get
		{
			if (this._thisTransform == null)
			{
				this._thisTransform = base.GetComponent<RectTransform>();
			}
			return this._thisTransform;
		}
	}

	// Token: 0x17000660 RID: 1632
	// (get) Token: 0x06005650 RID: 22096 RVA: 0x001F6F88 File Offset: 0x001F5188
	private LayoutElement ThisLayoutElement
	{
		get
		{
			if (this._thisLayoutElement == null)
			{
				this._thisLayoutElement = base.GetComponent<LayoutElement>();
			}
			return this._thisLayoutElement;
		}
	}

	// Token: 0x06005651 RID: 22097 RVA: 0x001F6FAC File Offset: 0x001F51AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		DetailsScreen componentInParent = base.GetComponentInParent<DetailsScreen>();
		if (componentInParent)
		{
			DetailsScreen detailsScreen = componentInParent;
			detailsScreen.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(detailsScreen.pointerEnterActions, new KScreen.PointerEnterActions(this.OnPointerEnterGrandparent));
			DetailsScreen detailsScreen2 = componentInParent;
			detailsScreen2.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(detailsScreen2.pointerExitActions, new KScreen.PointerExitActions(this.OnPointerExitGrandparent));
		}
	}

	// Token: 0x06005652 RID: 22098 RVA: 0x001F7014 File Offset: 0x001F5214
	protected override void OnCleanUp()
	{
		DetailsScreen componentInParent = base.GetComponentInParent<DetailsScreen>();
		if (componentInParent)
		{
			DetailsScreen detailsScreen = componentInParent;
			detailsScreen.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Remove(detailsScreen.pointerEnterActions, new KScreen.PointerEnterActions(this.OnPointerEnterGrandparent));
			DetailsScreen detailsScreen2 = componentInParent;
			detailsScreen2.pointerExitActions = (KScreen.PointerExitActions)Delegate.Remove(detailsScreen2.pointerExitActions, new KScreen.PointerExitActions(this.OnPointerExitGrandparent));
		}
		base.OnCleanUp();
	}

	// Token: 0x06005653 RID: 22099 RVA: 0x001F707C File Offset: 0x001F527C
	private void Update()
	{
		if (this.SourceTransform != null)
		{
			this.SourceTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.ThisTransform.rect.width);
		}
		if (this.SourceTransform != null && (!this.hoverLock || !this.grandparentIsHovered || this.isHovered || this.queuedSizeUpdate))
		{
			this.ThisLayoutElement.minHeight = this.SourceTransform.rect.height + this.topPadding + this.bottomPadding;
			this.SourceTransform.anchoredPosition = new Vector2(0f, -this.topPadding);
			this.queuedSizeUpdate = false;
		}
		if (this.hoverIndicator != null)
		{
			if (this.SourceTransform != null && this.SourceTransform.rect.height > this.ThisTransform.rect.height)
			{
				this.hoverIndicator.SetActive(true);
				return;
			}
			this.hoverIndicator.SetActive(false);
		}
	}

	// Token: 0x06005654 RID: 22100 RVA: 0x001F7190 File Offset: 0x001F5390
	public void UpdateSize()
	{
		this.queuedSizeUpdate = true;
	}

	// Token: 0x06005655 RID: 22101 RVA: 0x001F7199 File Offset: 0x001F5399
	public void OnPointerEnterGrandparent(PointerEventData eventData)
	{
		this.grandparentIsHovered = true;
	}

	// Token: 0x06005656 RID: 22102 RVA: 0x001F71A2 File Offset: 0x001F53A2
	public void OnPointerExitGrandparent(PointerEventData eventData)
	{
		this.grandparentIsHovered = false;
	}

	// Token: 0x06005657 RID: 22103 RVA: 0x001F71AB File Offset: 0x001F53AB
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isHovered = true;
	}

	// Token: 0x06005658 RID: 22104 RVA: 0x001F71B4 File Offset: 0x001F53B4
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isHovered = false;
	}

	// Token: 0x04003A13 RID: 14867
	public RectTransform SourceTransform;

	// Token: 0x04003A14 RID: 14868
	private RectTransform _thisTransform;

	// Token: 0x04003A15 RID: 14869
	private LayoutElement _thisLayoutElement;

	// Token: 0x04003A16 RID: 14870
	public GameObject hoverIndicator;

	// Token: 0x04003A17 RID: 14871
	public bool hoverLock;

	// Token: 0x04003A18 RID: 14872
	private bool grandparentIsHovered;

	// Token: 0x04003A19 RID: 14873
	private bool isHovered;

	// Token: 0x04003A1A RID: 14874
	private bool queuedSizeUpdate = true;

	// Token: 0x04003A1B RID: 14875
	public float topPadding;

	// Token: 0x04003A1C RID: 14876
	public float bottomPadding;
}
