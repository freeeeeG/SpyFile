using System;
using UnityEngine;

// Token: 0x02000909 RID: 2313
public class CameraReferenceTexture : MonoBehaviour
{
	// Token: 0x0600431B RID: 17179 RVA: 0x00177374 File Offset: 0x00175574
	private void OnPreCull()
	{
		if (this.quad == null)
		{
			this.quad = new FullScreenQuad("CameraReferenceTexture", base.GetComponent<Camera>(), this.referenceCamera.GetComponent<CameraRenderTexture>().ShouldFlip());
		}
		if (this.referenceCamera != null)
		{
			this.quad.Draw(this.referenceCamera.GetComponent<CameraRenderTexture>().GetTexture());
		}
	}

	// Token: 0x04002BC0 RID: 11200
	public Camera referenceCamera;

	// Token: 0x04002BC1 RID: 11201
	private FullScreenQuad quad;
}
