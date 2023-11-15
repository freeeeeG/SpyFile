using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001B6 RID: 438
	public class KnockbackEnemiesAction : Action
	{
		// Token: 0x06000A10 RID: 2576 RVA: 0x00027910 File Offset: 0x00025B10
		public override void Init()
		{
			this.player = PlayerController.Instance;
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0002791D File Offset: 0x00025B1D
		public override void Activate(GameObject target)
		{
			this.player.KnockbackNearby();
		}

		// Token: 0x04000719 RID: 1817
		private PlayerController player;
	}
}
