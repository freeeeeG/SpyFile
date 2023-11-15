using System;
using UnityEngine;

// Token: 0x0200091F RID: 2335
public class SimDebugViewCompositor : MonoBehaviour
{
	// Token: 0x060043A5 RID: 17317 RVA: 0x0017AE8F File Offset: 0x0017908F
	private void Awake()
	{
		SimDebugViewCompositor.Instance = this;
	}

	// Token: 0x060043A6 RID: 17318 RVA: 0x0017AE97 File Offset: 0x00179097
	private void OnDestroy()
	{
		SimDebugViewCompositor.Instance = null;
	}

	// Token: 0x060043A7 RID: 17319 RVA: 0x0017AE9F File Offset: 0x0017909F
	private void Start()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/SimDebugViewCompositor"));
		this.Toggle(false);
	}

	// Token: 0x060043A8 RID: 17320 RVA: 0x0017AEBD File Offset: 0x001790BD
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, this.material);
	}

	// Token: 0x060043A9 RID: 17321 RVA: 0x0017AECC File Offset: 0x001790CC
	public void Toggle(bool is_on)
	{
		base.enabled = is_on;
	}

	// Token: 0x04002CD4 RID: 11476
	public Material material;

	// Token: 0x04002CD5 RID: 11477
	public static SimDebugViewCompositor Instance;
}
