using System;
using Scenes;

namespace Runnables
{
	// Token: 0x020002E1 RID: 737
	public sealed class CloseAllUIHealthBar : UICommands
	{
		// Token: 0x06000EAB RID: 3755 RVA: 0x0002D94D File Offset: 0x0002BB4D
		public override void Run()
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
		}
	}
}
