using System;
using System.Collections.Generic;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x0200002F RID: 47
[ExecuteInEditMode]
public class MB3_AtlasPackerRenderTexture : MonoBehaviour
{
	// Token: 0x060000C1 RID: 193 RVA: 0x00007F94 File Offset: 0x00006394
	public Texture2D OnRenderAtlas(MB3_TextureCombiner combiner)
	{
		this.fastRenderer = new MB_TextureCombinerRenderTexture();
		this._doRenderAtlas = true;
		Texture2D result = this.fastRenderer.DoRenderAtlas(base.gameObject, this.width, this.height, this.padding, this.rects, this.textureSets, this.indexOfTexSetToRender, this.texPropertyName, this.resultMaterialTextureBlender, this.isNormalMap, this.fixOutOfBoundsUVs, this.considerNonTextureProperties, combiner, this.LOG_LEVEL);
		this._doRenderAtlas = false;
		return result;
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00008016 File Offset: 0x00006416
	private void OnRenderObject()
	{
		if (this._doRenderAtlas)
		{
			this.fastRenderer.OnRenderObject();
			this._doRenderAtlas = false;
		}
	}

	// Token: 0x040000AA RID: 170
	private MB_TextureCombinerRenderTexture fastRenderer;

	// Token: 0x040000AB RID: 171
	private bool _doRenderAtlas;

	// Token: 0x040000AC RID: 172
	public int width;

	// Token: 0x040000AD RID: 173
	public int height;

	// Token: 0x040000AE RID: 174
	public int padding;

	// Token: 0x040000AF RID: 175
	public bool isNormalMap;

	// Token: 0x040000B0 RID: 176
	public bool fixOutOfBoundsUVs;

	// Token: 0x040000B1 RID: 177
	public bool considerNonTextureProperties;

	// Token: 0x040000B2 RID: 178
	public TextureBlender resultMaterialTextureBlender;

	// Token: 0x040000B3 RID: 179
	public Rect[] rects;

	// Token: 0x040000B4 RID: 180
	public Texture2D tex1;

	// Token: 0x040000B5 RID: 181
	public List<MB3_TextureCombiner.MB_TexSet> textureSets;

	// Token: 0x040000B6 RID: 182
	public int indexOfTexSetToRender;

	// Token: 0x040000B7 RID: 183
	public ShaderTextureProperty texPropertyName;

	// Token: 0x040000B8 RID: 184
	public MB2_LogLevel LOG_LEVEL = MB2_LogLevel.info;

	// Token: 0x040000B9 RID: 185
	public Texture2D testTex;

	// Token: 0x040000BA RID: 186
	public Material testMat;
}
