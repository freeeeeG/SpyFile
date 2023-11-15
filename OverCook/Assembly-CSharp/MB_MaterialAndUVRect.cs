using System;
using UnityEngine;

// Token: 0x0200004E RID: 78
[Serializable]
public class MB_MaterialAndUVRect
{
	// Token: 0x06000228 RID: 552 RVA: 0x000192A0 File Offset: 0x000176A0
	public MB_MaterialAndUVRect(Material m, Rect destRect, Rect samplingRectMatAndUVTiling, Rect sourceMaterialTiling, Rect samplingEncapsulatinRect, string objName)
	{
		this.material = m;
		this.atlasRect = destRect;
		this.samplingRectMatAndUVTiling = samplingRectMatAndUVTiling;
		this.sourceMaterialTiling = sourceMaterialTiling;
		this.samplingEncapsulatinRect = samplingEncapsulatinRect;
		this.srcObjName = objName;
	}

	// Token: 0x06000229 RID: 553 RVA: 0x000192D5 File Offset: 0x000176D5
	public override int GetHashCode()
	{
		return this.material.GetInstanceID() ^ this.samplingEncapsulatinRect.GetHashCode();
	}

	// Token: 0x0600022A RID: 554 RVA: 0x000192F4 File Offset: 0x000176F4
	public override bool Equals(object obj)
	{
		return obj is MB_MaterialAndUVRect && this.material == ((MB_MaterialAndUVRect)obj).material && this.samplingEncapsulatinRect == ((MB_MaterialAndUVRect)obj).samplingEncapsulatinRect;
	}

	// Token: 0x04000167 RID: 359
	public Material material;

	// Token: 0x04000168 RID: 360
	public Rect atlasRect;

	// Token: 0x04000169 RID: 361
	public string srcObjName;

	// Token: 0x0400016A RID: 362
	public Rect samplingRectMatAndUVTiling;

	// Token: 0x0400016B RID: 363
	public Rect sourceMaterialTiling;

	// Token: 0x0400016C RID: 364
	public Rect samplingEncapsulatinRect;
}
