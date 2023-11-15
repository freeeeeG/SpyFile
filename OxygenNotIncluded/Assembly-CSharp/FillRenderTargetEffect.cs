using System;
using UnityEngine;

// Token: 0x02000A44 RID: 2628
public class FillRenderTargetEffect : MonoBehaviour
{
	// Token: 0x06004F1F RID: 20255 RVA: 0x001C000D File Offset: 0x001BE20D
	public void SetFillTexture(Texture tex)
	{
		this.fillTexture = tex;
	}

	// Token: 0x06004F20 RID: 20256 RVA: 0x001C0016 File Offset: 0x001BE216
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(this.fillTexture, null);
	}

	// Token: 0x04003382 RID: 13186
	private Texture fillTexture;
}
