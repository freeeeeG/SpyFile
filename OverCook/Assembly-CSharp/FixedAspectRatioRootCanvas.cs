using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001C5 RID: 453
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Canvas))]
public class FixedAspectRatioRootCanvas : MonoBehaviour
{
	// Token: 0x060007D1 RID: 2001 RVA: 0x00030BB3 File Offset: 0x0002EFB3
	private void Awake()
	{
		this.m_canvasScaler = base.gameObject.RequireComponent<CanvasScaler>();
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00030BC8 File Offset: 0x0002EFC8
	private void Start()
	{
		this.m_ratioManager = GameUtils.RequestManager<FixedAspectRatioManager>();
		if (this.m_ratioManager != null)
		{
			this.m_ratioManager.RegisterOnResolutionChanged(new GenericVoid<Rect, float, float>(this.OnResolutionChanged));
			this.m_ratioManager.InitialiseComponent(new GenericVoid<Rect, float, float>(this.OnResolutionChanged));
		}
		else
		{
			Vector2 defaultAspect = FixedAspectRatio.DefaultAspect;
			Rect rect = FixedAspectRatio.ComputeAspectRatioRect(defaultAspect.x, defaultAspect.y, (float)Screen.width, (float)Screen.height);
			this.m_canvasScaler.matchWidthOrHeight = ((!this.IsWiderThanDesiredAspect(rect)) ? 0f : 1f);
		}
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x00030C70 File Offset: 0x0002F070
	private void OnDestroy()
	{
		if (this.m_ratioManager != null)
		{
			this.m_ratioManager.UnregisterOnResolutionChanged(new GenericVoid<Rect, float, float>(this.OnResolutionChanged));
		}
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x00030C9A File Offset: 0x0002F09A
	private void OnResolutionChanged(Rect _rect, float _screenWidth, float _screenHeight)
	{
		this.m_canvasScaler.matchWidthOrHeight = ((!this.IsWiderThanDesiredAspect(_rect)) ? 0f : 1f);
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00030CC2 File Offset: 0x0002F0C2
	private bool IsWiderThanDesiredAspect(Rect _rect)
	{
		return _rect.width < 1f;
	}

	// Token: 0x04000636 RID: 1590
	private CanvasScaler m_canvasScaler;

	// Token: 0x04000637 RID: 1591
	private FixedAspectRatioManager m_ratioManager;
}
