using System;
using Characters;
using Hardmode;
using Level.Npc;
using Runnables;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004ED RID: 1261
	public sealed class HardmodeMagicMirror : InteractiveObject
	{
		// Token: 0x060018C5 RID: 6341 RVA: 0x0004DA90 File Offset: 0x0004BC90
		private void Update()
		{
			if (this._dwarf.tryLevel > Singleton<HardmodeManager>.Instance.clearedLevel + 1 && base.activated)
			{
				base.Deactivate();
				return;
			}
			if (Singleton<HardmodeManager>.Instance.currentLevel <= Singleton<HardmodeManager>.Instance.clearedLevel + 1 && !base.activated)
			{
				base.Activate();
			}
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x0004DAEB File Offset: 0x0004BCEB
		public override void InteractWith(Character character)
		{
			base.InteractWith(character);
			this._execute.Run();
		}

		// Token: 0x0400159B RID: 5531
		[SerializeField]
		private Dwarf _dwarf;

		// Token: 0x0400159C RID: 5532
		[SerializeField]
		private Runnable _execute;
	}
}
