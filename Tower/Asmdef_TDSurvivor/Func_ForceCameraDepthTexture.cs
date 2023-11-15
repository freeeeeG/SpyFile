using System;
using UnityEngine;

// Token: 0x0200019C RID: 412
[ExecuteInEditMode]
public class Func_ForceCameraDepthTexture : MonoBehaviour
{
	// Token: 0x06000AF4 RID: 2804 RVA: 0x0002A424 File Offset: 0x00028624
	private void Start()
	{
		Camera component = base.GetComponent<Camera>();
		if (component != null)
		{
			component.depthTextureMode = DepthTextureMode.Depth;
		}
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x0002A448 File Offset: 0x00028648
	private void Update()
	{
	}
}
