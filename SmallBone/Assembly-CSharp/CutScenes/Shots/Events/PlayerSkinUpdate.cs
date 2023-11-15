using System;
using Services;
using Singletons;

namespace CutScenes.Shots.Events
{
	// Token: 0x02000207 RID: 519
	public class PlayerSkinUpdate : Event
	{
		// Token: 0x06000A7B RID: 2683 RVA: 0x0001CC1B File Offset: 0x0001AE1B
		public override void Run()
		{
			Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon.UpdateSkin();
		}
	}
}
