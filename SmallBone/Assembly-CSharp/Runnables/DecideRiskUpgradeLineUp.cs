using System;
using Characters.Gear.Upgrades;
using Singletons;

namespace Runnables
{
	// Token: 0x020002D4 RID: 724
	public sealed class DecideRiskUpgradeLineUp : Runnable
	{
		// Token: 0x06000E8B RID: 3723 RVA: 0x0002D6A2 File Offset: 0x0002B8A2
		public override void Run()
		{
			Singleton<UpgradeShop>.Instance.LoadCusredLineUp();
		}
	}
}
