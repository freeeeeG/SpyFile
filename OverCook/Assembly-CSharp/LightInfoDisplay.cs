using System;
using UnityEngine;

// Token: 0x020001B2 RID: 434
public class LightInfoDisplay : DebugDisplay
{
	// Token: 0x0600075C RID: 1884 RVA: 0x0002F1B5 File Offset: 0x0002D5B5
	public override void OnSetUp()
	{
		this.m_bSceneHasLightmaps = (LightmapSettings.lightmaps != null && LightmapSettings.lightmaps.Length > 0);
		this.RefreshString();
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x0002F1DA File Offset: 0x0002D5DA
	public override void OnUpdate()
	{
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x0002F1DC File Offset: 0x0002D5DC
	public override void OnDraw(ref Rect rect, GUIStyle style)
	{
		base.DrawText(ref rect, style, this.m_displayString);
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x0002F1EC File Offset: 0x0002D5EC
	private void RefreshString()
	{
		this.m_displayString = "Lightmaps: " + ((!this.m_bSceneHasLightmaps) ? "No" : "Yes");
	}

	// Token: 0x04000606 RID: 1542
	public bool m_bSceneHasLightmaps;

	// Token: 0x04000607 RID: 1543
	public string m_displayString = string.Empty;
}
