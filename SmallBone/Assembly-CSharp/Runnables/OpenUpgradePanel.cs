using System;
using Scenes;

namespace Runnables
{
	// Token: 0x020002D7 RID: 727
	public sealed class OpenUpgradePanel : Runnable
	{
		// Token: 0x06000E91 RID: 3729 RVA: 0x0002D6CD File Offset: 0x0002B8CD
		public override void Run()
		{
			Scene<GameBase>.instance.uiManager.upgradeShop.Open();
		}
	}
}
