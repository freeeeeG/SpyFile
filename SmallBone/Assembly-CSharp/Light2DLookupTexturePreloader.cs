using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

// Token: 0x02000062 RID: 98
public class Light2DLookupTexturePreloader : MonoBehaviour
{
	// Token: 0x060001D0 RID: 464 RVA: 0x00008634 File Offset: 0x00006834
	private void Start()
	{
		Light2DLookupTexture.CreateLookupTextures();
		UnityEngine.Object.Destroy(this);
	}
}
