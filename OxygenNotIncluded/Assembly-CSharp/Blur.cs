using System;
using UnityEngine;

// Token: 0x02000908 RID: 2312
public static class Blur
{
	// Token: 0x0600431A RID: 17178 RVA: 0x0017734D File Offset: 0x0017554D
	public static RenderTexture Run(Texture2D image)
	{
		if (Blur.blurMaterial == null)
		{
			Blur.blurMaterial = new Material(Shader.Find("Klei/PostFX/Blur"));
		}
		return null;
	}

	// Token: 0x04002BBF RID: 11199
	private static Material blurMaterial;
}
