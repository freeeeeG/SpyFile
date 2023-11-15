using System;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public static class StandardShaderHelper
{
	// Token: 0x06000803 RID: 2051 RVA: 0x0003140C File Offset: 0x0002F80C
	public static void SetupMaterialWithBlendMode(Material material, StandardShaderHelper.BlendMode blendMode)
	{
		switch (blendMode)
		{
		case StandardShaderHelper.BlendMode.Opaque:
			material.SetOverrideTag("RenderType", string.Empty);
			material.SetInt("_SrcBlend", 1);
			material.SetInt("_DstBlend", 0);
			material.SetInt("_ZWrite", 1);
			material.DisableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = -1;
			break;
		case StandardShaderHelper.BlendMode.Cutout:
			material.SetOverrideTag("RenderType", "TransparentCutout");
			material.SetInt("_SrcBlend", 1);
			material.SetInt("_DstBlend", 0);
			material.SetInt("_ZWrite", 1);
			material.EnableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 2450;
			break;
		case StandardShaderHelper.BlendMode.Fade:
			material.SetOverrideTag("RenderType", "Transparent");
			material.SetInt("_SrcBlend", 5);
			material.SetInt("_DstBlend", 10);
			material.SetInt("_ZWrite", 0);
			material.DisableKeyword("_ALPHATEST_ON");
			material.EnableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 3000;
			break;
		case StandardShaderHelper.BlendMode.Transparent:
			material.SetOverrideTag("RenderType", "Transparent");
			material.SetInt("_SrcBlend", 1);
			material.SetInt("_DstBlend", 10);
			material.SetInt("_ZWrite", 0);
			material.DisableKeyword("_ALPHATEST_ON");
			material.DisableKeyword("_ALPHABLEND_ON");
			material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = 3000;
			break;
		}
	}

	// Token: 0x020001D8 RID: 472
	public enum BlendMode
	{
		// Token: 0x04000660 RID: 1632
		Opaque,
		// Token: 0x04000661 RID: 1633
		Cutout,
		// Token: 0x04000662 RID: 1634
		Fade,
		// Token: 0x04000663 RID: 1635
		Transparent
	}
}
