using System;
using GameResources;
using Singletons;
using UnityEngine;

namespace FX
{
	// Token: 0x02000262 RID: 610
	public class VignetteSpawner : Singleton<VignetteSpawner>
	{
		// Token: 0x06000BF8 RID: 3064 RVA: 0x00020D6A File Offset: 0x0001EF6A
		public void Spawn(Color startColor, Color endColor, Curve curve)
		{
			PoolObject poolObject = VignetteSpawner.Assets.effect.Spawn(true);
			poolObject.transform.SetParent(base.transform, false);
			poolObject.GetComponent<Vignette>().Initialize(startColor, endColor, curve);
		}

		// Token: 0x02000263 RID: 611
		private static class Assets
		{
			// Token: 0x040009FF RID: 2559
			internal static readonly PoolObject effect = CommonResource.instance.vignetteEffect;
		}
	}
}
