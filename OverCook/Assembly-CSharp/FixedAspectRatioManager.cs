using System;
using UnityEngine;

// Token: 0x020001C4 RID: 452
public class FixedAspectRatioManager : Manager
{
	// Token: 0x060007CB RID: 1995 RVA: 0x00030AAE File Offset: 0x0002EEAE
	public void InitialiseComponent(GenericVoid<Rect, float, float> callback)
	{
		if (this.m_bInitialised)
		{
			callback(this.m_correctedRect, this.m_screenWidth, this.m_screenHeight);
		}
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x00030AD4 File Offset: 0x0002EED4
	private void LateUpdate()
	{
		if ((float)Screen.width != this.m_screenWidth || (float)Screen.height != this.m_screenHeight)
		{
			this.m_screenWidth = (float)Screen.width;
			this.m_screenHeight = (float)Screen.height;
			this.m_correctedRect = FixedAspectRatio.ComputeAspectRatioRect(this.m_aspectRatio.x, this.m_aspectRatio.y, this.m_screenWidth, this.m_screenHeight);
			if (this.OnResolutionChangedCallback != null)
			{
				this.OnResolutionChangedCallback(this.m_correctedRect, this.m_screenWidth, this.m_screenHeight);
			}
			this.m_bInitialised = true;
		}
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x00030B77 File Offset: 0x0002EF77
	public void RegisterOnResolutionChanged(GenericVoid<Rect, float, float> callback)
	{
		this.OnResolutionChangedCallback = (GenericVoid<Rect, float, float>)Delegate.Combine(this.OnResolutionChangedCallback, callback);
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x00030B90 File Offset: 0x0002EF90
	public void UnregisterOnResolutionChanged(GenericVoid<Rect, float, float> callback)
	{
		this.OnResolutionChangedCallback = (GenericVoid<Rect, float, float>)Delegate.Remove(this.OnResolutionChangedCallback, callback);
	}

	// Token: 0x0400062F RID: 1583
	[SerializeField]
	private Vector2 m_aspectRatio = new Vector2(16f, 9f);

	// Token: 0x04000630 RID: 1584
	private Rect m_correctedRect = default(Rect);

	// Token: 0x04000631 RID: 1585
	private float m_screenWidth;

	// Token: 0x04000632 RID: 1586
	private float m_screenHeight;

	// Token: 0x04000633 RID: 1587
	private bool m_bInitialised;

	// Token: 0x04000634 RID: 1588
	private GenericVoid<Rect, float, float> OnResolutionChangedCallback = delegate(Rect _correctedRect, float _width, float _height)
	{
	};
}
