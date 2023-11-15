using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x02000797 RID: 1943
[Serializable]
public class PerformanceSettings
{
	// Token: 0x0600258C RID: 9612 RVA: 0x000B1B14 File Offset: 0x000AFF14
	public void ApplyToScene()
	{
		Camera main = Camera.main;
		if (main.renderingPath == RenderingPath.UsePlayerSettings)
		{
			main.renderingPath = this.RenderPath;
		}
		this.SetComponentEnabled<ScreenSpaceAmbientOcclusion>(main.gameObject, this.UseSSAO);
		this.SetComponentEnabled<ColorCorrectionCurves>(main.gameObject, this.UseColorCorrection);
	}

	// Token: 0x0600258D RID: 9613 RVA: 0x000B1B64 File Offset: 0x000AFF64
	private void SetComponentEnabled<T>(GameObject _object, bool _enabled) where T : Behaviour
	{
		T t = _object.RequestComponent<T>();
		if (t != null)
		{
			t.enabled = _enabled;
		}
	}

	// Token: 0x04001D21 RID: 7457
	public bool UseSSAO;

	// Token: 0x04001D22 RID: 7458
	public bool UseColorCorrection;

	// Token: 0x04001D23 RID: 7459
	public RenderingPath RenderPath;
}
