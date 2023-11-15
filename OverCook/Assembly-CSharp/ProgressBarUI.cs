using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AF5 RID: 2805
[ExecuteInEditMode]
public class ProgressBarUI : BaseProgressBarUI
{
	// Token: 0x060038C8 RID: 14536 RVA: 0x0010BC20 File Offset: 0x0010A020
	protected override void UpdateFill()
	{
		if (this.m_filledImage == null)
		{
			this.m_filledImage = base.FillImage;
		}
		if (this.m_filledImage)
		{
			float num = (1f - this.m_scalingFactor) * 0.5f;
			float num2 = base.Value * this.m_scalingFactor + num;
			if (this.m_hasMask == null && this.m_filledImage.transform.parent != null)
			{
				this.m_hasMask = new bool?(this.m_filledImage.transform.parent.gameObject.RequestComponent<Mask>() != null);
			}
			bool flag = false;
			if (this.m_filledImage.transform.parent != null && this.m_hasMask != null && this.m_hasMask.Value)
			{
				RectTransform rectTransform = this.m_filledImage.transform.parent.transform as RectTransform;
				flag = (rectTransform != null);
				if (rectTransform != null)
				{
					Vector2 anchorMax = rectTransform.anchorMax;
					anchorMax.x = base.Value;
					rectTransform.anchorMax = anchorMax;
					flag = true;
				}
			}
			if (flag)
			{
				RectTransform rectTransform2 = this.m_filledImage.transform.parent.transform as RectTransform;
				rectTransform2.anchorMax = new Vector2(base.Value, rectTransform2.anchorMax.y);
			}
			else
			{
				RectTransform component = this.m_filledImage.gameObject.GetComponent<RectTransform>();
				component.localScale = new Vector2(base.Value, 1f);
			}
		}
		if (this.m_capImage == null)
		{
			this.m_capImage = base.CapImage;
		}
		if (this.m_capImage != null)
		{
			RectTransform rectTransform3;
			if (this.m_filledImage.transform.parent != null && this.m_hasMask != null && this.m_hasMask.Value)
			{
				rectTransform3 = (this.m_filledImage.transform.parent.transform as RectTransform);
			}
			else
			{
				rectTransform3 = this.m_filledImage.gameObject.GetComponent<RectTransform>();
			}
			RectTransform component2 = this.m_capImage.gameObject.GetComponent<RectTransform>();
			component2.anchorMin = new Vector2(rectTransform3.anchorMax.x, component2.anchorMin.y);
			component2.anchorMax = new Vector2(rectTransform3.anchorMax.x, component2.anchorMax.y);
		}
	}

	// Token: 0x060038C9 RID: 14537 RVA: 0x0010BEE0 File Offset: 0x0010A2E0
	protected override void OnCreateSubObjects(GameObject _container)
	{
		base.OnCreateSubObjects(_container);
		this.m_allChildRects.Clear();
		foreach (RectTransform rectTransform in _container.RequestComponentsRecursive<RectTransform>())
		{
			bool flag = false;
			for (int j = 0; j < this.m_notches.Length; j++)
			{
				if (this.m_notches[j].rectTransform == rectTransform)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.m_allChildRects.Add(rectTransform);
			}
		}
	}

	// Token: 0x060038CA RID: 14538 RVA: 0x0010BF70 File Offset: 0x0010A370
	protected override void OnRefreshSubObjectProperties(GameObject _container)
	{
		for (int i = 0; i < this.m_allChildRects.Count; i++)
		{
			UIUtils.SetupFillParentAreaRect(this.m_allChildRects[i]);
			this.m_allChildRects[i].pivot = new Vector2(0f, 1f);
		}
		base.OnRefreshSubObjectProperties(_container);
	}

	// Token: 0x060038CB RID: 14539 RVA: 0x0010BFD4 File Offset: 0x0010A3D4
	protected override Image CreateFillImage(GameObject _rect)
	{
		GameObject gameObject = GameObjectUtils.CreateOnParent<Image>(_rect.gameObject, "SubImage");
		gameObject.hideFlags = HideFlags.NotEditable;
		return gameObject.GetComponent<Image>();
	}

	// Token: 0x060038CC RID: 14540 RVA: 0x0010C000 File Offset: 0x0010A400
	protected override void PositionNotch(Image _notch, float _position)
	{
		RectTransform rectTransform = _notch.rectTransform;
		rectTransform.pivot = new Vector2(0.5f, 0f);
		rectTransform.sizeDelta = new Vector2(5f, 0f);
		rectTransform.anchorMin = new Vector2(_position, 0f);
		rectTransform.anchorMax = new Vector2(_position, 1f);
	}

	// Token: 0x04002D75 RID: 11637
	[SerializeField]
	[Range(0f, 1f)]
	private float m_scalingFactor = 1f;

	// Token: 0x04002D76 RID: 11638
	private Image m_filledImage;

	// Token: 0x04002D77 RID: 11639
	private Image m_capImage;

	// Token: 0x04002D78 RID: 11640
	private bool? m_hasMask;

	// Token: 0x04002D79 RID: 11641
	private List<RectTransform> m_allChildRects = new List<RectTransform>(0);
}
