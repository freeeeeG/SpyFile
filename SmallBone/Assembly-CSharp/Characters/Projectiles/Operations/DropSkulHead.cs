using System;
using Data;
using GameResources;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000779 RID: 1913
	public class DropSkulHead : Operation
	{
		// Token: 0x0600276A RID: 10090 RVA: 0x000763C8 File Offset: 0x000745C8
		public override void Run(IProjectile projectile)
		{
			PoolObject poolObject = DropSkulHead.Assets.skulHead;
			if (GameData.HardmodeProgress.hardmode && GameData.Generic.skinIndex == 1)
			{
				poolObject = DropSkulHead.Assets.heroSkulHead;
			}
			poolObject.Spawn(base.transform.position, true);
		}

		// Token: 0x0200077A RID: 1914
		private class Assets
		{
			// Token: 0x04002182 RID: 8578
			internal static readonly PoolObject skulHead = CommonResource.instance.droppedSkulHead;

			// Token: 0x04002183 RID: 8579
			internal static readonly PoolObject heroSkulHead = CommonResource.instance.droppedHeroSkulHead;
		}
	}
}
