using System;
using GameResources;
using Singletons;

namespace FX
{
	// Token: 0x02000257 RID: 599
	public class ScreenFlashSpawner : Singleton<ScreenFlashSpawner>
	{
		// Token: 0x06000BC2 RID: 3010 RVA: 0x0002066A File Offset: 0x0001E86A
		public ScreenFlash Spawn(ScreenFlash.Info info)
		{
			PoolObject poolObject = ScreenFlashSpawner.Assets.effect.Spawn(true);
			poolObject.transform.parent = base.transform;
			ScreenFlash component = poolObject.GetComponent<ScreenFlash>();
			component.Play(info);
			return component;
		}

		// Token: 0x02000258 RID: 600
		private static class Assets
		{
			// Token: 0x040009CA RID: 2506
			internal static readonly PoolObject effect = CommonResource.instance.screenFlashEffect;
		}
	}
}
