using System;
using UnityEngine;

// Token: 0x02000A45 RID: 2629
public class FixGraphicsCorruption : MonoBehaviour
{
	// Token: 0x06004F22 RID: 20258 RVA: 0x001C002C File Offset: 0x001BE22C
	private void Start()
	{
		Camera component = base.GetComponent<Camera>();
		component.transparencySortMode = TransparencySortMode.Orthographic;
		component.tag = "Untagged";
	}

	// Token: 0x06004F23 RID: 20259 RVA: 0x001C0045 File Offset: 0x001BE245
	private void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, dest);
	}
}
