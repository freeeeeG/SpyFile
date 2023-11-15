using System;
using UnityEngine;

// Token: 0x020001B3 RID: 435
public abstract class DebugDisplay
{
	// Token: 0x06000761 RID: 1889
	public abstract void OnSetUp();

	// Token: 0x06000762 RID: 1890 RVA: 0x0002E89F File Offset: 0x0002CC9F
	public virtual void OnDestroy()
	{
	}

	// Token: 0x06000763 RID: 1891
	public abstract void OnUpdate();

	// Token: 0x06000764 RID: 1892
	public abstract void OnDraw(ref Rect rect, GUIStyle style);

	// Token: 0x06000765 RID: 1893 RVA: 0x0002E8A1 File Offset: 0x0002CCA1
	public void DrawText(ref Rect rect, GUIStyle style, string text)
	{
		GUI.Label(rect, text, style);
		rect.y += rect.height;
	}
}
