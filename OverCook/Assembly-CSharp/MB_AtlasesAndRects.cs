using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004C RID: 76
[Serializable]
public class MB_AtlasesAndRects
{
	// Token: 0x04000161 RID: 353
	public Texture2D[] atlases;

	// Token: 0x04000162 RID: 354
	[NonSerialized]
	public List<MB_MaterialAndUVRect> mat2rect_map;

	// Token: 0x04000163 RID: 355
	public string[] texPropertyNames;
}
