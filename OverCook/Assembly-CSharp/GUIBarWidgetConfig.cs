using System;
using UnityEngine;

// Token: 0x020009A1 RID: 2465
[Serializable]
public class GUIBarWidgetConfig
{
	// Token: 0x06003049 RID: 12361 RVA: 0x000E315A File Offset: 0x000E155A
	public GUIBarWidgetConfig DeepClone()
	{
		return base.MemberwiseClone() as GUIBarWidgetConfig;
	}

	// Token: 0x040026C5 RID: 9925
	public float m_width;

	// Token: 0x040026C6 RID: 9926
	public float m_height;

	// Token: 0x040026C7 RID: 9927
	public float m_border;

	// Token: 0x040026C8 RID: 9928
	public Transform m_transform;

	// Token: 0x040026C9 RID: 9929
	public Vector2 m_offset = new Vector2(0f, -64f);

	// Token: 0x040026CA RID: 9930
	public Color m_fillColor = Color.white;

	// Token: 0x040026CB RID: 9931
	public Color m_emptyColor = Color.black;

	// Token: 0x040026CC RID: 9932
	public Color m_borderColor = Color.black;
}
