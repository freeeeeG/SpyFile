using System;
using Characters;
using Level;
using Services;
using Singletons;

namespace CutScenes
{
	// Token: 0x020001AD RID: 429
	public class PlayerMovementBlock : State
	{
		// Token: 0x06000924 RID: 2340 RVA: 0x0001A397 File Offset: 0x00018597
		public override void Attach()
		{
			Singleton<Service>.Instance.levelManager.player.movement.blocked.Attach(State.key);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0001A3BC File Offset: 0x000185BC
		public override void Detach()
		{
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			if (levelManager == null)
			{
				return;
			}
			Character player = levelManager.player;
			if (player != null)
			{
				player.movement.blocked.Detach(State.key);
			}
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0001A387 File Offset: 0x00018587
		private void OnDisable()
		{
			this.Detach();
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0001A387 File Offset: 0x00018587
		private void OnDestroy()
		{
			this.Detach();
		}
	}
}
