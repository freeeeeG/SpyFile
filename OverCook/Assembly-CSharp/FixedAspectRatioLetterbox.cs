using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001C2 RID: 450
public class FixedAspectRatioLetterbox : MonoBehaviour
{
	// Token: 0x060007C5 RID: 1989 RVA: 0x0003084C File Offset: 0x0002EC4C
	private void Start()
	{
		this.m_manager = base.gameObject.RequestComponent<FixedAspectRatioManager>();
		if (this.m_manager == null)
		{
			this.m_manager = base.gameObject.transform.parent.gameObject.RequireComponent<FixedAspectRatioManager>();
		}
		this.m_manager.RegisterOnResolutionChanged(new GenericVoid<Rect, float, float>(this.ResizeLetterbox));
		this.m_manager.InitialiseComponent(new GenericVoid<Rect, float, float>(this.ResizeLetterbox));
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x000308C9 File Offset: 0x0002ECC9
	private void OnDestroy()
	{
		if (this.m_manager != null)
		{
			this.m_manager.UnregisterOnResolutionChanged(new GenericVoid<Rect, float, float>(this.ResizeLetterbox));
		}
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x000308F4 File Offset: 0x0002ECF4
	private void ResizeLetterbox(Rect _correctedRect, float _screenWidth, float _screenHeight)
	{
		if (this.m_top != null)
		{
			this.m_top.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, _screenHeight - _correctedRect.yMax * _screenHeight);
		}
		if (this.m_bottom != null)
		{
			this.m_bottom.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0f, _screenHeight - _correctedRect.yMax * _screenHeight);
		}
		if (this.m_left != null)
		{
			this.m_left.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, _screenWidth - _correctedRect.xMax * _screenWidth);
		}
		if (this.m_right != null)
		{
			this.m_right.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0f, _screenWidth - _correctedRect.xMax * _screenWidth);
		}
	}

	// Token: 0x04000629 RID: 1577
	[SerializeField]
	private Image m_top;

	// Token: 0x0400062A RID: 1578
	[SerializeField]
	private Image m_bottom;

	// Token: 0x0400062B RID: 1579
	[SerializeField]
	private Image m_left;

	// Token: 0x0400062C RID: 1580
	[SerializeField]
	private Image m_right;

	// Token: 0x0400062D RID: 1581
	private FixedAspectRatioManager m_manager;
}
