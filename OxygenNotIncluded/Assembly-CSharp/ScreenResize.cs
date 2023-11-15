using System;
using UnityEngine;

// Token: 0x0200091E RID: 2334
public class ScreenResize : MonoBehaviour
{
	// Token: 0x060043A0 RID: 17312 RVA: 0x0017ADD8 File Offset: 0x00178FD8
	private void Awake()
	{
		ScreenResize.Instance = this;
		this.isFullscreen = Screen.fullScreen;
		this.OnResize = (System.Action)Delegate.Combine(this.OnResize, new System.Action(this.SaveResolutionToPrefs));
	}

	// Token: 0x060043A1 RID: 17313 RVA: 0x0017AE10 File Offset: 0x00179010
	private void LateUpdate()
	{
		if (Screen.width != this.Width || Screen.height != this.Height || this.isFullscreen != Screen.fullScreen)
		{
			this.Width = Screen.width;
			this.Height = Screen.height;
			this.isFullscreen = Screen.fullScreen;
			this.TriggerResize();
		}
	}

	// Token: 0x060043A2 RID: 17314 RVA: 0x0017AE6B File Offset: 0x0017906B
	public void TriggerResize()
	{
		if (this.OnResize != null)
		{
			this.OnResize();
		}
	}

	// Token: 0x060043A3 RID: 17315 RVA: 0x0017AE80 File Offset: 0x00179080
	private void SaveResolutionToPrefs()
	{
		GraphicsOptionsScreen.OnResize();
	}

	// Token: 0x04002CCF RID: 11471
	public System.Action OnResize;

	// Token: 0x04002CD0 RID: 11472
	public static ScreenResize Instance;

	// Token: 0x04002CD1 RID: 11473
	private int Width;

	// Token: 0x04002CD2 RID: 11474
	private int Height;

	// Token: 0x04002CD3 RID: 11475
	private bool isFullscreen;
}
