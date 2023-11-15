using System;
using UnityEngine;

// Token: 0x020009A3 RID: 2467
[Serializable]
public class GUIRect
{
	// Token: 0x06003050 RID: 12368 RVA: 0x000E3560 File Offset: 0x000E1960
	public GUIRect(Rect _rect, GUIAnchor _anchor, GUICoordSystem _coords)
	{
		this.m_rect = _rect;
		this.m_anchor = _anchor;
		this.m_coordSystem = _coords;
	}

	// Token: 0x06003051 RID: 12369 RVA: 0x000E357D File Offset: 0x000E197D
	public GUIRect DeepCopy()
	{
		return base.MemberwiseClone() as GUIRect;
	}

	// Token: 0x06003052 RID: 12370 RVA: 0x000E358C File Offset: 0x000E198C
	public Rect GetInPixels(Camera _camera)
	{
		Vector2 vector = GUIUtils.GUIToPixels(_camera, new Vector2(this.m_rect.x, this.m_rect.y), this.m_anchor, this.m_coordSystem);
		Vector2 vector2 = GUIUtils.GUIToPixels(_camera, new Vector2(this.m_rect.x + this.m_rect.width, this.m_rect.y + this.m_rect.height), this.m_anchor, this.m_coordSystem);
		float x = Mathf.Min(vector.x, vector2.x);
		float y = Mathf.Min(vector.y, vector2.y);
		float width = Mathf.Abs(vector.x - vector2.x);
		float height = Mathf.Abs(vector.y - vector2.y);
		return new Rect(x, y, width, height);
	}

	// Token: 0x040026D1 RID: 9937
	public Rect m_rect;

	// Token: 0x040026D2 RID: 9938
	public GUIAnchor m_anchor;

	// Token: 0x040026D3 RID: 9939
	public GUICoordSystem m_coordSystem;
}
