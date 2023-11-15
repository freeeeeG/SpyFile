using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000794 RID: 1940
public class DisableReflectionProbes : MonoBehaviour
{
	// Token: 0x06002586 RID: 9606 RVA: 0x000B19C0 File Offset: 0x000AFDC0
	private void Start()
	{
		bool supports3DRenderTextures = SystemInfo.supports3DRenderTextures;
		bool supportsCubemapArrayTextures = SystemInfo.supportsCubemapArrayTextures;
		bool supportsRenderToCubemap = SystemInfo.supportsRenderToCubemap;
		if (!supports3DRenderTextures || !supportsCubemapArrayTextures || !supportsRenderToCubemap)
		{
			MeshRenderer[] array = base.gameObject.RequestComponentsInImmediateChildren<MeshRenderer>();
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].reflectionProbeUsage = ReflectionProbeUsage.Off;
				}
			}
		}
	}
}
